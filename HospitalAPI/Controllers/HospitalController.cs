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
    public IActionResult GetPrescriptionsData(string doctorLastName = "empty")
    {
        IEnumerable<ReturnedPrescription> result = _dataBaseService.GetPrescriptionsData(doctorLastName);

        return Ok(result);
    }

    [HttpPost]
    public IActionResult PostPrescription()
    {
        return Ok();
    }

}