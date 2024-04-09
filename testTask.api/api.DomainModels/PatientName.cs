using System.Collections.Generic;

namespace api.DomainModels;

public class PatientName
{
    public string Id { get; set; }

    public string Use { get; set; }

    public string Family { get; set; }

    public ICollection<PatientGiven> Given { get; set; }
}