using HospitalAPI.Models;
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

    public  IEnumerable<PrescriptionDTO> GetPrescriptionsData(string doctorLastName = "empty")
    {
        using SqlConnection connection = GetConnection();
        int doctorId = doctorLastName != "empty" ? FindDoctorId(doctorLastName) : 0;

        string query = doctorLastName != "empty"
            ? "SELECT * FROM Prescription WHERE IdDoctor = @doctorId"
            : "SELECT * FROM Prescription";

        using SqlCommand command = new(query, connection);

        if (doctorId != 0)
        {
            command.Parameters.AddWithValue("@doctorId", doctorId);
        }

        using SqlDataReader reader = command.ExecuteReader();
        List<PrescriptionDTO> prescriptions = new();
        while (reader.Read())
        {
            prescriptions.Add(new PrescriptionDTO
            {
                IdPrescription = reader.GetInt32(0),
                Date = reader.GetDateTime(1),
                DueDate = reader.GetDateTime(2),
                PatientLastName = FindPatientLastName(reader.GetInt32(3)),
                DoctorLastName = FindDoctorLastName(reader.GetInt32(4))
            });
        }


        prescriptions = prescriptions.OrderByDescending(p => p.Date).ToList();

        return prescriptions;
    }



    private int FindDoctorId(string doctorLastName)
    {
        using SqlConnection connection = GetConnection();
        string query = "SELECT IdDoctor FROM Doctor WHERE LastName = @doctorLastName";
        using SqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@doctorLastName", doctorLastName);
        return (int)command.ExecuteScalar();
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

    public Prescription AddPrescription(NewPrescriptionDTO newPrescription)
    {
        if (newPrescription.DueDate <= newPrescription.Date)
        {
            throw new ArgumentException("DueDate must be later than Date.");
        }

        Prescription prescription = new()
        {
            Date = newPrescription.Date,
            DueDate = newPrescription.DueDate,
            IdPatient = newPrescription.IdPatient,
            IdDoctor = newPrescription.IdDoctor
        };

        using SqlConnection connection = GetConnection();
        string query = "INSERT INTO Prescription (Date, DueDate, IdPatient, IdDoctor) VALUES (@date, @dueDate, @idPatient, @idDoctor); SELECT SCOPE_IDENTITY()";
        using SqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@date", prescription.Date);
        command.Parameters.AddWithValue("@dueDate", prescription.DueDate);
        command.Parameters.AddWithValue("@idPatient", prescription.IdPatient);
        command.Parameters.AddWithValue("@idDoctor", prescription.IdDoctor);

        prescription.IdPrescription = Convert.ToInt32(command.ExecuteScalar());

        return prescription;
    }
}