using HospitalAPI.Models;
using HospitalAPI.Models.DTO;
using HospitalAPI.Service;

namespace HospitalAPI.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/prescriptions")]
public class HospitalController : Controller
{
    private readonly IDataBaseService _dataBaseService;

    public HospitalController(IDataBaseService dataBaseService)
    {
        _dataBaseService = dataBaseService;
    }


    [HttpGet]
    public IActionResult GetPrescriptionsData(string? doctorLastName)
    {
        try
        {
            IEnumerable<PrescriptionDTO> result = string.IsNullOrEmpty(doctorLastName)
                ? _dataBaseService.GetPrescriptionsData()
                : _dataBaseService.GetPrescriptionsData(doctorLastName);

            if (!result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public IActionResult PostPrescription([FromBody] NewPrescriptionDTO newPrescription)
    {
        try
        {
            Prescription prescription = _dataBaseService.AddPrescription(newPrescription);

            return Ok(prescription);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}