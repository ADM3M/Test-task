using System;

namespace api.DomainModels;

public class Patient
{
    public string Id { get; set; }

    public string NameId { get; set; }
    
    public PatientName Name { get; set; }

    public Gender? Gender { get; set; }

    public DateTime BirthDate { get; set; }

    public bool? Active { get; set; }
}