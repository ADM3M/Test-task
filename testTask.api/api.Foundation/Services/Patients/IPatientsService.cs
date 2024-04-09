using api.Foundation.DataContracts;
using api.Repositories.EntityOperationResult;

namespace api.Foundation.Services.Patients;

public interface IPatientsService
{
    Task<IReadOnlyCollection<PatientDataContract>> CreateRange(IReadOnlyCollection<PatientDataContract> patientDataContracts);

    Task<IReadOnlyCollection<PatientDataContract>> GetAllPatients();
    
    Task<PatientDataContract?> GetPatientById(string patientId);

    Task<IReadOnlyCollection<PatientDataContract>> GetPatientByBirthDate(string sourceDateString);

    Task<EntityOperationResult<PatientDataContract, PatientOperationError>> UpdatePatient(PatientDataContract patient);

    Task<PatientOperationError?> DeletePatient(string patientId);
}