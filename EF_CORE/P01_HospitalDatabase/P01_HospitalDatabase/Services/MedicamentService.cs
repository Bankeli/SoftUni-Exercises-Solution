using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.IO;
using P01_HospitalDatabase.Data;
using P01_HospitalDatabase.Data.Models;
using P01_HospitalDatabase.Helpers;
using P01_HospitalDatabase.Services.Contracts;

namespace P01_HospitalDatabase.Services
{
    public class MedicamentService : IMedicamentService
    {
        private readonly HospitalContext _context;
        private readonly IConsoleReader _reader;
        private readonly IEntityValidator _validator;
        private readonly IPatientService _patientService;

        public MedicamentService(
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
            var medicament = new Medicament
            {
                Name = _reader.ReadRequiredText("Medicament name: ")
            };

            SaveEntity(medicament, "Medicament added.");
        }

        public void Update()
        {
            if (!HasRecords())
            {
                return;
            }

            List();
            int medicamentId = _reader.ReadInt("Medicament id: ");

            var medicament = _context.Medicaments.Find(medicamentId);
            if (medicament == null)
            {
                System.Console.WriteLine("Medicament was not found.");
                return;
            }

            medicament.Name = _reader.ReadRequiredText("Medicament name: ");

            SaveEntity(medicament, "Medicament updated.");
        }

        public void Delete()
        {
            if (!HasRecords())
            {
                return;
            }

            List();
            int medicamentId = _reader.ReadInt("Medicament id: ");

            var medicament = _context.Medicaments.Find(medicamentId);
            if (medicament == null)
            {
                System.Console.WriteLine("Medicament was not found.");
                return;
            }

            _context.Medicaments.Remove(medicament);
            _context.SaveChanges();

            System.Console.WriteLine("Medicament deleted.");
        }

        public void List()
        {
            var medicaments = _context.Medicaments
                .OrderBy(m => m.MedicamentId)
                .ToList();

            if (medicaments.Count == 0)
            {
                System.Console.WriteLine("No medicaments found.");
                return;
            }

            foreach (var medicament in medicaments)
            {
                System.Console.WriteLine($"{medicament.MedicamentId}. {medicament.Name}");
            }
        }

        public void Prescribe()
        {
            if (!_context.Patients.Any())
            {
                System.Console.WriteLine("No patients found.");
                return;
            }

            if (!HasRecords())
            {
                return;
            }

            _patientService.List();
            int patientId = _reader.ReadInt("Patient id: ");

            List();
            int medicamentId = _reader.ReadInt("Medicament id: ");

            if (!_context.Patients.Any(p => p.PatientId == patientId) ||
                !_context.Medicaments.Any(m => m.MedicamentId == medicamentId))
            {
                System.Console.WriteLine("Patient or medicament was not found.");
                return;
            }

            bool prescriptionExists = _context.PatientsMedicaments
                .Any(pm => pm.PatientId == patientId && pm.MedicamentId == medicamentId);

            if (prescriptionExists)
            {
                System.Console.WriteLine("This medicament is already prescribed to the patient.");
                return;
            }

            var prescription = new PatientMedicament
            {
                PatientId = patientId,
                MedicamentId = medicamentId
            };

            _context.PatientsMedicaments.Add(prescription);
            _context.SaveChanges();

            System.Console.WriteLine("Medicament prescribed.");
        }

        public void RemovePrescription()
        {
            if (!_context.Patients.Any())
            {
                System.Console.WriteLine("No patients found.");
                return;
            }

            if (!HasRecords())
            {
                return;
            }

            _patientService.List();
            int patientId = _reader.ReadInt("Patient id: ");

            List();
            int medicamentId = _reader.ReadInt("Medicament id: ");

            var prescription = _context.PatientsMedicaments
                .Find(patientId, medicamentId);

            if (prescription == null)
            {
                System.Console.WriteLine("Prescription was not found.");
                return;
            }

            _context.PatientsMedicaments.Remove(prescription);
            _context.SaveChanges();

            System.Console.WriteLine("Prescription removed.");
        }

        private bool HasRecords()
        {
            if (_context.Medicaments.Any())
            {
                return true;
            }

            System.Console.WriteLine("No medicaments found.");
            return false;
        }

        private void SaveEntity(Medicament medicament, string successMessage)
        {
            if (!_validator.TryValidate(medicament, out var errors))
            {
                foreach (var error in errors)
                {
                    System.Console.WriteLine(error);
                }

                return;
            }

            if (_context.Entry(medicament).State == EntityState.Detached)
            {
                _context.Medicaments.Add(medicament);
            }

            _context.SaveChanges();
            System.Console.WriteLine(successMessage);
        }
    }
}
