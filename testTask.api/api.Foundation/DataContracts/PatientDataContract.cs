namespace api.Foundation.DataContracts;

public class PatientDataContract
{
    public string Id { get; set; }
    
    public PatientNameDataContract Name { get; set; }

    public string? Gender { get; set; }

    public DateTime BirthDate { get; set; }

    public bool? Active { get; set; }
}