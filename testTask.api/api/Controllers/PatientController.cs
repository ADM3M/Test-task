using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Foundation.DataContracts;
using api.Foundation.Services.HL7DateComparison;
using api.Foundation.Services.Patients;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

public class PatientController : BaseController
{
    private readonly IPatientsService _patientsService;


    public PatientController(IPatientsService patientsService)
    {
        _patientsService = patientsService;
    }
    
    
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<PatientDataContract>>> GetAllPatients()
    {
        var patients = await _patientsService.GetAllPatients();

        return Ok(patients);
    }
    
    [HttpGet("{patientId}")]
    public async Task<ActionResult<PatientDataContract>> GetPatientById(string patientId)
    {
        var patient = await _patientsService.GetPatientById(patientId);

        if (patient is null)
        {
            return NotFound();
        }

        return patient;
    }

    [HttpGet("filter")]
    public async Task<ActionResult<IReadOnlyCollection<PatientDataContract>>> GetPatientsByBirthDate([FromQuery] string birthDate)
    {
        if (!ValidateHL7Date(birthDate))
        {
            return BadRequest("Invalid birthDate format");
        }

        var patients = await _patientsService.GetPatientByBirthDate(birthDate);

        return Ok(patients);
    }
    
    [HttpPost("create")]
    public async Task<ActionResult<IReadOnlyCollection<PatientDataContract>>> CreatePatients([FromBody] IReadOnlyCollection<PatientDataContract> patients)
    {
        var newPatients = await _patientsService.CreateRange(patients);

        return Ok(newPatients);
    }

    [HttpPut("update")]
    public async Task<ActionResult<PatientDataContract>> UpdatePatient([FromBody] PatientDataContract patient)
    {
        var updatedPatient = await _patientsService.UpdatePatient(patient);
        if (!updatedPatient.IsSuccessful)
        {
            var errorMessages = updatedPatient.Errors.Select(GetErrorMessage).ToList();
            
            return BadRequest(errorMessages);
        }

        return Ok(updatedPatient.Entity);
    }

    [HttpDelete("delete/{patientId}")]
    public async Task<ActionResult> RemovePatient(string patientId)
    {
        var deleteResult = await _patientsService.DeletePatient(patientId);
        if (deleteResult != null)
        {
            return BadRequest(GetErrorMessage(deleteResult.Value));
        }

        return Ok();
    }


    private bool ValidateHL7Date(string sourceDate)
    {
        if (String.IsNullOrEmpty(sourceDate))
        {
            return false;
        }

        if (sourceDate.Length < 3)
        {
            return false;
        }

        var mode = sourceDate[..2];
        if (!Hl7DateInfoParser.AvailableModes.Contains(mode))
        {
            return false;
        }

        var parseResult = DateTime.TryParse(sourceDate[2..], out var date);
        if (!parseResult)
        {
            return false;
        }

        return true;
    }
    
    private static string GetErrorMessage(PatientOperationError error) => error switch
    {
        PatientOperationError.PatientNotFound => PatientOperationErrorCode.PatientNotFound,
        _ => throw new ArgumentOutOfRangeException(nameof(error), "Unsupported enum value"),
    };
}