using HospitalAPI.Models;
using HospitalAPI.Models.DTO;

namespace HospitalAPI.Service;

public interface IDataBaseService
{
    IEnumerable<PrescriptionDTO>GetPrescriptionsData(string doctorLastName = "empty");
    Prescription AddPrescription(NewPrescriptionDTO newPrescription);
}