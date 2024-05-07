using HospitalAPI.Models.DTO;

namespace HospitalAPI.Service;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;


public class DataBaseService : IDataBaseService
{

    private readonly IConfiguration _configuration;

    public DataBaseService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private SqlConnection GetConnection()
    {
        SqlConnection connection = new(_configuration.GetConnectionString("Default"));
        connection.Open();
        return connection;
    }

    public  IEnumerable<ReturnedPrescription> GetPrescriptionsData(string doctorLastName = "empty")
    {
        using (SqlConnection connection = GetConnection())
        {
            string query = "SELECT * FROM Prescription";
            using (SqlCommand command = new(query, connection))
            {

                using (SqlDataReader reader = command.ExecuteReader())
                {

                   // return list of prescriptions
                    List<ReturnedPrescription> prescriptions = new();
                    while (reader.Read())
                    {
                        prescriptions.Add(new ReturnedPrescription
                        {
                            IdPrescription = reader.GetInt32(0),
                            Date = reader.GetDateTime(1),
                            DueDate = reader.GetDateTime(2),
                            PatientLastName = FindPatientLastName(reader.GetInt32(3)),
                            DoctorLastName = FindDoctorLastName(reader.GetInt32(4))
                        });
                    }
                    return prescriptions;
                }
            }

        }

    }


    private string FindDoctorLastName(int idDoctor)
    {
        using (SqlConnection connection = GetConnection())
        {
            string query = "SELECT LastName FROM Doctor WHERE IdDoctor = @idDoctor";
            using (SqlCommand command = new(query, connection))
            {
                command.Parameters.AddWithValue("@idDoctor", idDoctor);
                return (string)command.ExecuteScalar();
            }
        }
    }

    private string FindPatientLastName(int idPatient)
    {
        using (SqlConnection connection = GetConnection())
        {
            string query = "SELECT LastName FROM Patient WHERE IdPatient = @idPatient";
            using (SqlCommand command = new(query, connection))
            {
                command.Parameters.AddWithValue("@idPatient", idPatient);
                return (string)command.ExecuteScalar();
            }
        }
    }
}