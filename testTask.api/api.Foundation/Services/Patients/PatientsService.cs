using System.Linq.Expressions;
using api.Common;
using api.Common.EntityLoadStrategy;
using api.DomainModels;
using api.Foundation.DataContracts;
using api.Foundation.Services.HL7DateComparison;
using api.Repositories.EntityOperationResult;
using api.Repositories.Repositories;

namespace api.Foundation.Services.Patients;

public class PatientsService : IPatientsService
{
    private readonly IPatientsRepository _patientsRepository;


    public PatientsService(IPatientsRepository patientsRepository)
    {
        _patientsRepository = patientsRepository;
    }
    
    
    public async Task<IReadOnlyCollection<PatientDataContract>> CreateRange(IReadOnlyCollection<PatientDataContract> patientDataContracts)
    {
        var patients = patientDataContracts.Select(CreateFrom).ToList();

        await _patientsRepository.AddRangeAsync(patients);
        
        await _patientsRepository.SaveChangesAsync();
        
        return patients.Select(CreateFrom).ToList();
    }

    public async Task<IReadOnlyCollection<PatientDataContract>> GetAllPatients()
    {
        var loadStrategy = GetLoadStrategy();
        var patients = await _patientsRepository.GetWhereAsync(loadStrategy: loadStrategy);
        var patientDataContracts = patients.Select(CreateFrom).ToList();

        return patientDataContracts;
    }

    public async Task<PatientDataContract?> GetPatientById(string patientId)
    {
        var loadStrategy = GetLoadStrategy();
        var patient = await _patientsRepository.FirstOrDefaultAsync(p => p.Id == patientId, loadStrategy);

        return patient != null ? CreateFrom(patient) : null;
    }

    public async Task<IReadOnlyCollection<PatientDataContract>> GetPatientByBirthDate(string sourceDateString)
    {
        if (String.IsNullOrEmpty(sourceDateString))
        {
            throw new ArgumentException("invalid date string format", nameof(sourceDateString));
        }

        var loadStrategy = GetLoadStrategy();
        var patients = await _patientsRepository.GetWhereAsync(loadStrategy: loadStrategy, asNoTracking: true);
        
        var hl7DateInfo = Hl7DateInfoParser.Parse(sourceDateString);
        var hl7Comparer = HL7DateComparisonProvider.GetComparer(hl7DateInfo.Mode);
        var filteredPatients = patients.Where(p => hl7Comparer(p.BirthDate, hl7DateInfo.Date)).ToList();

        var patientDataContracts = filteredPatients.Select(CreateFrom).ToList();

        return patientDataContracts;
    }

    public async Task<EntityOperationResult<PatientDataContract, PatientOperationError>> UpdatePatient(PatientDataContract patient)
    {
        var loadStrategy = GetLoadStrategy();
        var patientToUpdate = await _patientsRepository.FirstOrDefaultAsync(p => p.Id == patient.Id, loadStrategy);

        if (patientToUpdate is null)
        {
            Console.WriteLine("Update error! No entity with id {0} found.", patient.Id);

            return PatientOperationError.PatientNotFound;
        }

        UpdatePatientFrom(patientToUpdate, patient);

        await _patientsRepository.SaveChangesAsync();

        return CreateFrom(patientToUpdate);
    }

    public async Task<PatientOperationError?> DeletePatient(string patientId)
    {
        var loadStrategy = GetLoadStrategy();
        var patientToRemove = await _patientsRepository.FirstOrDefaultAsync(p => p.Id == patientId, loadStrategy);

        if (patientToRemove is null)
        {
            Console.WriteLine("Delete error! No entity with id {0} found.", patientId);

            return PatientOperationError.PatientNotFound;
        }

        _patientsRepository.Remove(patientToRemove);
        await _patientsRepository.SaveChangesAsync();

        return null;
    }


    private static IEntityLoadStrategy<Patient> GetLoadStrategy()
    {
        return new EntityLoadStrategy<Patient>(p => p.Name.Given);
    }
    
    private static Patient CreateFrom(PatientDataContract patientDataContract)
    {
        var patientNameId = ExternalId.Generate();
        var gender = patientDataContract.Gender != null ? CreateGenderFrom(patientDataContract.Gender) : null as Gender?;
        
        return new Patient
        {
            Id = ExternalId.Generate(),
            NameId = patientNameId,
            Name = CreateFrom(patientDataContract.Name, patientNameId),
            Gender = gender,
            BirthDate = patientDataContract.BirthDate,
            Active = patientDataContract.Active,
        };
    }

    private static PatientName CreateFrom(PatientNameDataContract patientNameDataContract, string? nameId)
    {
        var actualNameId = nameId ?? ExternalId.Generate();
        var patientGiven = patientNameDataContract.Given
            .Select(g => CreateFrom(actualNameId, g))
            .ToList();
        
        return new PatientName
        {
            Id = actualNameId,
            Use = patientNameDataContract.Use,
            Family = patientNameDataContract.Family,
            Given = patientGiven,
        };
    }

    private static PatientGiven CreateFrom(string patientNameId, string patientGiven)
    {
        return new PatientGiven
        {
            Id = ExternalId.Generate(),
            PatientNameId = patientNameId,
            Given = patientGiven,
        };
    }
    
    private static PatientDataContract CreateFrom(Patient patient)
    {
        return new PatientDataContract
        {
            Id = patient.Id,
            Name = CreateFrom(patient.Name),
            Gender = patient.Gender.HasValue ? CreateGenderFrom(patient.Gender.Value) : null,
            BirthDate = patient.BirthDate,
            Active = patient.Active,
        };
    }

    private static PatientNameDataContract CreateFrom(PatientName patientNameDataContract)
    {
        var patientNameGiven = patientNameDataContract.Given.Select(png => png.Given).ToList();

        return new PatientNameDataContract
        {
            Id = patientNameDataContract.Id,
            Use = patientNameDataContract.Use,
            Family = patientNameDataContract.Family,
            Given = patientNameGiven,
        };
    }

    private static Gender CreateGenderFrom(string gender) => gender switch
    {
        GenderOptions.Male => Gender.Male,
        GenderOptions.Female => Gender.Female,
        GenderOptions.Other => Gender.Other,
        GenderOptions.Unknown => Gender.Unknown,
        _ => throw new ArgumentOutOfRangeException(nameof(gender), "Unsupported enum value")
    };

    private static string CreateGenderFrom(Gender gender) => gender switch
    {
        Gender.Male => GenderOptions.Male,
        Gender.Female => GenderOptions.Female,
        Gender.Other => GenderOptions.Other,
        Gender.Unknown => GenderOptions.Unknown,
        _ => throw new ArgumentOutOfRangeException(nameof(gender), "Unsupported enum value")
    };

    private static void UpdatePatientFrom(Patient patient, PatientDataContract fromPatient)
    {
        var gender = fromPatient.Gender != null ? CreateGenderFrom(fromPatient.Gender) : null as Gender?;
        UpdatePatientNameFrom(patient.Name, fromPatient.Name);
        
        patient.Id = fromPatient.Id;
        patient.NameId = fromPatient.Name.Id;
        patient.Gender = gender;
        patient.BirthDate = fromPatient.BirthDate;
        patient.Active = fromPatient.Active;
    }

    private static void UpdatePatientNameFrom(PatientName patientName, PatientNameDataContract fromPatientName)
    {
        patientName.Id = fromPatientName.Id;
        patientName.Use = fromPatientName.Use;
        patientName.Family = fromPatientName.Family;

        UpdatePatientNameGivenFrom(patientName.Given, fromPatientName.Given);
    }

    private static void UpdatePatientNameGivenFrom(ICollection<PatientGiven> patientGiven, IReadOnlyCollection<string> fromGiven)
    {
        for (var i = 0; i < 2; i++)
        {
            patientGiven.ElementAt(i).Given = fromGiven.ElementAt(i);
        }
    }
}