﻿namespace HospitalAPI.Models.DTO;

public class ReturnedPrescription
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public string PatientLastName { get; set; }
    public string DoctorLastName { get; set; }
}