using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.IO;
using P01_HospitalDatabase.Data;
using P01_HospitalDatabase.Data.Models;
using P01_HospitalDatabase.Helpers;
using P01_HospitalDatabase.Services.Contracts;

namespace P01_HospitalDatabase.Services
{
    public class PatientService : IPatientService
    {
        private readonly HospitalContext _context;
        private readonly IConsoleReader _reader;
        private readonly IEntityValidator _validator;

        public PatientService(
            HospitalContext context,
            IConsoleReader reader,
            IEntityValidator validator)
        {
            _context = context;
            _reader = reader;
            _validator = validator;
        }

        public void Add()
        {
            var patient = new Patient
            {
                FirstName = _reader.ReadRequiredText("First name: "),
                LastName = _reader.ReadRequiredText("Last name: "),
                Address = _reader.ReadRequiredText("Address: "),
                Email = _reader.ReadOptionalText("Email: "),
                HasInsurance = _reader.ReadBool("Has insurance (y/n): ")
            };

            SaveEntity(patient, "Patient added.");
        }

        public void Update()
        {
            if (!HasRecords())
            {
                return;
            }

            List();
            int patientId = _reader.ReadInt("Patient id: ");

            var patient = _context.Patients.Find(patientId);
            if (patient == null)
            {
                System.Console.WriteLine("Patient was not found.");
                return;
            }

            patient.FirstName = _reader.ReadRequiredText("First name: ");
            patient.LastName = _reader.ReadRequiredText("Last name: ");
            patient.Address = _reader.ReadRequiredText("Address: ");
            patient.Email = _reader.ReadOptionalText("Email: ");
            patient.HasInsurance = _reader.ReadBool("Has insurance (y/n): ");

            SaveEntity(patient, "Patient updated.");
        }

        public void Delete()
        {
            if (!HasRecords())
            {
                return;
            }

            List();
            int patientId = _reader.ReadInt("Patient id: ");

            var patient = _context.Patients.Find(patientId);
            if (patient == null)
            {
                System.Console.WriteLine("Patient was not found.");
                return;
            }

            _context.Patients.Remove(patient);
            _context.SaveChanges();

            System.Console.WriteLine("Patient deleted.");
        }

        public void List()
        {
            var patients = _context.Patients
                .Include(p => p.Diagnoses)
                .Include(p => p.Visitations)
                .Include(p => p.Prescriptions)
                .ThenInclude(pm => pm.Medicament)
                .OrderBy(p => p.PatientId)
                .ToList();

            if (patients.Count == 0)
            {
                System.Console.WriteLine("No patients found.");
                return;
            }

            foreach (var patient in patients)
            {
                string medicaments = patient.Prescriptions.Count == 0
                    ? "none"
                    : string.Join(", ", patient.Prescriptions.Select(pm => pm.Medicament.Name));

                System.Console.WriteLine(
                    $"{patient.PatientId}. {patient.FirstName} {patient.LastName} | " +
                    $"Diagnoses: {patient.Diagnoses.Count} | Visitations: {patient.Visitations.Count} | " +
                    $"Medicaments: {medicaments}");
            }
        }

        private bool HasRecords()
        {
            if (_context.Patients.Any())
            {
                return true;
            }

            System.Console.WriteLine("No patients found.");
            return false;
        }

        private void SaveEntity(Patient patient, string successMessage)
        {
            if (!_validator.TryValidate(patient, out var errors))
            {
                foreach (var error in errors)
                {
                    System.Console.WriteLine(error);
                }

                return;
            }

            if (_context.Entry(patient).State == EntityState.Detached)
            {
                _context.Patients.Add(patient);
            }

            _context.SaveChanges();
            System.Console.WriteLine(successMessage);
        }
    }
}
