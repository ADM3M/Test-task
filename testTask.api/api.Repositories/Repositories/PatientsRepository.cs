using api.DomainModels;

namespace api.Repositories.Repositories;

public class PatientsRepository : BaseRepository<Patient>, IPatientsRepository
{
    public PatientsRepository(TestTaskDbContext dbContext) : base(dbContext)
    {
        
    }

    public override void Remove(Patient patient)
    {
        DbSet.Attach(patient);
        DbSet.Remove(patient);
    
        var nameSet = DbContext.Set<PatientName>();
        nameSet.Remove(patient.Name);

        var givenSet = DbContext.Set<PatientGiven>();
        foreach (var patientGiven in patient.Name.Given)
        {
            givenSet.Remove(patientGiven);
        }
    }
}