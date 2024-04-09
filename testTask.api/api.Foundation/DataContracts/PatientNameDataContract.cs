namespace api.Foundation.DataContracts;

public class PatientNameDataContract
{
    public string Id { get; set; }

    public string Use { get; set; }

    public string Family { get; set; }

    public IReadOnlyCollection<string> Given { get; set; }
}