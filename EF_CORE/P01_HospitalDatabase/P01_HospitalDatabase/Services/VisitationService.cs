using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.IO;
using P01_HospitalDatabase.Data;
using P01_HospitalDatabase.Data.Models;
using P01_HospitalDatabase.Helpers;
using P01_HospitalDatabase.Services.Contracts;

namespace P01_HospitalDatabase.Services
{
    public class VisitationService : IVisitationService
    {
        private readonly HospitalContext _context;
        private readonly IConsoleReader _reader;
        private readonly IEntityValidator _validator;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;

        public VisitationService(
            HospitalContext context,
            IConsoleReader reader,
            IEntityValidator validator,
            IPatientService patientService,
            IDoctorService doctorService)
        {
            _context = context;
            _reader = reader;
            _validator = validator;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        public void Add()
        {
            if (!HasRequiredRecords())
            {
                return;
            }

            _patientService.List();
            int patientId = _reader.ReadInt("Patient id: ");

            _doctorService.List();
            int doctorId = _reader.ReadInt("Doctor id: ");

            if (!EntityExists(patientId, doctorId))
            {
                return;
            }

            var visitation = new Visitation
            {
                Date = _reader.ReadDate("Date (yyyy-MM-dd): "),
                Comments = _reader.ReadRequiredText("Comments: "),
                PatientId = patientId,
                DoctorId = doctorId
            };

            SaveEntity(visitation, "Visitation added.");
        }

        public void Update()
        {
            if (!HasRecords())
            {
                return;
            }

            List();
            int visitationId = _reader.ReadInt("Visitation id: ");

            var visitation = _context.Visitations.Find(visitationId);
            if (visitation == null)
            {
                System.Console.WriteLine("Visitation was not found.");
                return;
            }

            if (!HasRequiredRecords())
            {
                return;
            }

            _patientService.List();
            int patientId = _reader.ReadInt("Patient id: ");

            _doctorService.List();
            int doctorId = _reader.ReadInt("Doctor id: ");

            if (!EntityExists(patientId, doctorId))
            {
                return;
            }

            visitation.Date = _reader.ReadDate("Date (yyyy-MM-dd): ");
            visitation.Comments = _reader.ReadRequiredText("Comments: ");
            visitation.PatientId = patientId;
            visitation.DoctorId = doctorId;

            SaveEntity(visitation, "Visitation updated.");
        }

        public void Delete()
        {
            if (!HasRecords())
            {
                return;
            }

            List();
            int visitationId = _reader.ReadInt("Visitation id: ");

            var visitation = _context.Visitations.Find(visitationId);
            if (visitation == null)
            {
                System.Console.WriteLine("Visitation was not found.");
                return;
            }

            _context.Visitations.Remove(visitation);
            _context.SaveChanges();

            System.Console.WriteLine("Visitation deleted.");
        }

        public void List()
        {
            var visitations = _context.Visitations
                .Include(v => v.Patient)
                .Include(v => v.Doctor)
                .OrderBy(v => v.VisitationId)
                .ToList();

            if (visitations.Count == 0)
            {
                System.Console.WriteLine("No visitations found.");
                return;
            }

            foreach (var visitation in visitations)
            {
                System.Console.WriteLine(
                    $"{visitation.VisitationId}. {visitation.Date:yyyy-MM-dd} | " +
                    $"Patient: {visitation.Patient.FirstName} {visitation.Patient.LastName} | " +
                    $"Doctor: Dr. {visitation.Doctor.Name} | {visitation.Comments}");
            }
        }

        private bool HasRecords()
        {
            if (_context.Visitations.Any())
            {
                return true;
            }

            System.Console.WriteLine("No visitations found.");
            return false;
        }

        private bool HasRequiredRecords()
        {
            if (!_context.Patients.Any())
            {
                System.Console.WriteLine("No patients found.");
                return false;
            }

            if (!_context.Doctors.Any())
            {
                System.Console.WriteLine("No doctors found.");
                return false;
            }

            return true;
        }

        private bool EntityExists(int patientId, int doctorId)
        {
            if (!_context.Patients.Any(p => p.PatientId == patientId) ||
                !_context.Doctors.Any(d => d.DoctorId == doctorId))
            {
                System.Console.WriteLine("Patient or doctor was not found.");
                return false;
            }

            return true;
        }

        private void SaveEntity(Visitation visitation, string successMessage)
        {
            if (!_validator.TryValidate(visitation, out var errors))
            {
                foreach (var error in errors)
                {
                    System.Console.WriteLine(error);
                }

                return;
            }

            if (_context.Entry(visitation).State == EntityState.Detached)
            {
                _context.Visitations.Add(visitation);
            }

            _context.SaveChanges();
            System.Console.WriteLine(successMessage);
        }
    }
}
