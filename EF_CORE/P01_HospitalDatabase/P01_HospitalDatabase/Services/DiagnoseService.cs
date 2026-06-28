using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.IO;
using P01_HospitalDatabase.Data;
using P01_HospitalDatabase.Data.Models;
using P01_HospitalDatabase.Helpers;
using P01_HospitalDatabase.Services.Contracts;

namespace P01_HospitalDatabase.Services
{
    public class DiagnoseService : IDiagnoseService
    {
        private readonly HospitalContext _context;
        private readonly IConsoleReader _reader;
        private readonly IEntityValidator _validator;
        private readonly IPatientService _patientService;

        public DiagnoseService(
            HospitalContext context,
            IConsoleReader reader,
            IEntityValidator validator,
            IPatientService patientService)
        {
            _context = context;
            _reader = reader;
            _validator = validator;
            _patientService = patientService;
        }

        public void Add()
        {
            if (!HasRequiredRecords())
            {
                return;
            }

            _patientService.List();
            int patientId = _reader.ReadInt("Patient id: ");

            if (!PatientExists(patientId))
            {
                return;
            }

            var diagnose = new Diagnose
            {
                Name = _reader.ReadRequiredText("Diagnose name: "),
                Comments = _reader.ReadRequiredText("Comments: "),
                PatientId = patientId
            };

            SaveEntity(diagnose, "Diagnose added.");
        }

        public void Update()
        {
            if (!HasRecords())
            {
                return;
            }

            List();
            int diagnoseId = _reader.ReadInt("Diagnose id: ");

            var diagnose = _context.Diagnoses.Find(diagnoseId);
            if (diagnose == null)
            {
                System.Console.WriteLine("Diagnose was not found.");
                return;
            }

            if (!HasRequiredRecords())
            {
                return;
            }

            _patientService.List();
            int patientId = _reader.ReadInt("Patient id: ");

            if (!PatientExists(patientId))
            {
                return;
            }

            diagnose.Name = _reader.ReadRequiredText("Diagnose name: ");
            diagnose.Comments = _reader.ReadRequiredText("Comments: ");
            diagnose.PatientId = patientId;

            SaveEntity(diagnose, "Diagnose updated.");
        }

        public void Delete()
        {
            if (!HasRecords())
            {
                return;
            }

            List();
            int diagnoseId = _reader.ReadInt("Diagnose id: ");

            var diagnose = _context.Diagnoses.Find(diagnoseId);
            if (diagnose == null)
            {
                System.Console.WriteLine("Diagnose was not found.");
                return;
            }

            _context.Diagnoses.Remove(diagnose);
            _context.SaveChanges();

            System.Console.WriteLine("Diagnose deleted.");
        }

        public void List()
        {
            var diagnoses = _context.Diagnoses
                .Include(d => d.Patient)
                .OrderBy(d => d.DiagnoseId)
                .ToList();

            if (diagnoses.Count == 0)
            {
                System.Console.WriteLine("No diagnoses found.");
                return;
            }

            foreach (var diagnose in diagnoses)
            {
                System.Console.WriteLine(
                    $"{diagnose.DiagnoseId}. {diagnose.Name} | " +
                    $"Patient: {diagnose.Patient.FirstName} {diagnose.Patient.LastName} | " +
                    $"{diagnose.Comments}");
            }
        }

        private bool HasRecords()
        {
            if (_context.Diagnoses.Any())
            {
                return true;
            }

            System.Console.WriteLine("No diagnoses found.");
            return false;
        }

        private bool HasRequiredRecords()
        {
            if (!_context.Patients.Any())
            {
                System.Console.WriteLine("No patients found.");
                return false;
            }

            return true;
        }

        private bool PatientExists(int patientId)
        {
            if (!_context.Patients.Any(p => p.PatientId == patientId))
            {
                System.Console.WriteLine("Patient was not found.");
                return false;
            }

            return true;
        }

        private void SaveEntity(Diagnose diagnose, string successMessage)
        {
            if (!_validator.TryValidate(diagnose, out var errors))
            {
                foreach (var error in errors)
                {
                    System.Console.WriteLine(error);
                }

                return;
            }

            if (_context.Entry(diagnose).State == EntityState.Detached)
            {
                _context.Diagnoses.Add(diagnose);
            }

            _context.SaveChanges();
            System.Console.WriteLine(successMessage);
        }
    }
}
