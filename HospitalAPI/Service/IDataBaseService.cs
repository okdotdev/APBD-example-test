using HospitalAPI.Models.DTO;

namespace HospitalAPI.Service;

public interface IDataBaseService
{
    IEnumerable<ReturnedPrescription>GetPrescriptionsData(string doctorLastName = "empty");
}