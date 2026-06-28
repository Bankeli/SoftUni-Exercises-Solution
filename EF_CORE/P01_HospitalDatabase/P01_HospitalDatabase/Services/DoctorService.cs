using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.IO;
using P01_HospitalDatabase.Data;
using P01_HospitalDatabase.Data.Models;
using P01_HospitalDatabase.Helpers;
using P01_HospitalDatabase.Services.Contracts;

namespace P01_HospitalDatabase.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly HospitalContext _context;
        private readonly IConsoleReader _reader;
        private readonly IEntityValidator _validator;

        public DoctorService(
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
            var doctor = new Doctor
            {
                Name = _reader.ReadRequiredText("Doctor name: "),
                Specialty = _reader.ReadRequiredText("Specialty: ")
            };

            SaveEntity(doctor, "Doctor added.");
        }

        public void Update()
        {
            if (!HasRecords())
            {
                return;
            }

            List();
            int doctorId = _reader.ReadInt("Doctor id: ");

            var doctor = _context.Doctors.Find(doctorId);
            if (doctor == null)
            {
                System.Console.WriteLine("Doctor was not found.");
                return;
            }

            doctor.Name = _reader.ReadRequiredText("Doctor name: ");
            doctor.Specialty = _reader.ReadRequiredText("Specialty: ");

            SaveEntity(doctor, "Doctor updated.");
        }

        public void Delete()
        {
            if (!HasRecords())
            {
                return;
            }

            List();
            int doctorId = _reader.ReadInt("Doctor id: ");

            var doctor = _context.Doctors.Find(doctorId);
            if (doctor == null)
            {
                System.Console.WriteLine("Doctor was not found.");
                return;
            }

            _context.Doctors.Remove(doctor);
            _context.SaveChanges();

            System.Console.WriteLine("Doctor deleted.");
        }

        public void List()
        {
            var doctors = _context.Doctors
                .Include(d => d.Visitations)
                .OrderBy(d => d.DoctorId)
                .ToList();

            if (doctors.Count == 0)
            {
                System.Console.WriteLine("No doctors found.");
                return;
            }

            foreach (var doctor in doctors)
            {
                System.Console.WriteLine(
                    $"{doctor.DoctorId}. Dr. {doctor.Name} | {doctor.Specialty} | " +
                    $"Visitations: {doctor.Visitations.Count}");
            }
        }

        private bool HasRecords()
        {
            if (_context.Doctors.Any())
            {
                return true;
            }

            System.Console.WriteLine("No doctors found.");
            return false;
        }

        private void SaveEntity(Doctor doctor, string successMessage)
        {
            if (!_validator.TryValidate(doctor, out var errors))
            {
                foreach (var error in errors)
                {
                    System.Console.WriteLine(error);
                }

                return;
            }

            if (_context.Entry(doctor).State == EntityState.Detached)
            {
                _context.Doctors.Add(doctor);
            }

            _context.SaveChanges();
            System.Console.WriteLine(successMessage);
        }
    }
}
