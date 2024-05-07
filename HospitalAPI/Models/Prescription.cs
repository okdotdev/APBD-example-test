namespace HospitalAPI.Models;

public class Prescription
{
    DateTime Date { get; set; }
    DateTime DueDate { get; set; }
    int IdPatient { get; set; }
    int IdDoctor { get; set; }
}