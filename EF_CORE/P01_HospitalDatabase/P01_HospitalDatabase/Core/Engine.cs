using P01_HospitalDatabase.Data;
using P01_HospitalDatabase.Helpers;
using P01_HospitalDatabase.IO;
using P01_HospitalDatabase.Services;
using P01_HospitalDatabase.Services.Contracts;

namespace P01_HospitalDatabase.Core
{
    public class Engine
    {
        private readonly HospitalContext _context;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        private readonly IVisitationService _visitationService;
        private readonly IDiagnoseService _diagnoseService;
        private readonly IMedicamentService _medicamentService;

        public Engine(HospitalContext context)
        {
            _context = context;

            IConsoleReader reader = new ConsoleReader();
            IEntityValidator validator = new EntityValidator();

            _patientService = new PatientService(_context, reader, validator);
            _doctorService = new DoctorService(_context, reader, validator);
            _visitationService = new VisitationService(
                _context, reader, validator, _patientService, _doctorService);
            _diagnoseService = new DiagnoseService(
                _context, reader, validator, _patientService);
            _medicamentService = new MedicamentService(
                _context, reader, validator, _patientService);
        }

        public void Run()
        {
            while (true)
            {
                PrintMenu();

                string? command = System.Console.ReadLine();
                System.Console.WriteLine();

                if (!ProcessCommand(command))
                {
                    return;
                }

                WaitForContinue();
            }
        }

        private static void WaitForContinue()
        {
            System.Console.WriteLine();
            System.Console.Write("Press Enter to continue...");
            System.Console.ReadLine();
            System.Console.Clear();
        }

        private static void PrintMenu()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Hospital Database");
            System.Console.WriteLine("--- Patients ---");
            System.Console.WriteLine("1. Add patient");
            System.Console.WriteLine("2. Edit patient");
            System.Console.WriteLine("3. Delete patient");
            System.Console.WriteLine("4. List patients");
            System.Console.WriteLine("--- Doctors ---");
            System.Console.WriteLine("5. Add doctor");
            System.Console.WriteLine("6. Edit doctor");
            System.Console.WriteLine("7. Delete doctor");
            System.Console.WriteLine("8. List doctors");
            System.Console.WriteLine("--- Visitations ---");
            System.Console.WriteLine("9. Add visitation");
            System.Console.WriteLine("10. Edit visitation");
            System.Console.WriteLine("11. Delete visitation");
            System.Console.WriteLine("12. List visitations");
            System.Console.WriteLine("--- Diagnoses ---");
            System.Console.WriteLine("13. Add diagnose");
            System.Console.WriteLine("14. Edit diagnose");
            System.Console.WriteLine("15. Delete diagnose");
            System.Console.WriteLine("16. List diagnoses");
            System.Console.WriteLine("--- Medicaments ---");
            System.Console.WriteLine("17. Add medicament");
            System.Console.WriteLine("18. Edit medicament");
            System.Console.WriteLine("19. Delete medicament");
            System.Console.WriteLine("20. List medicaments");
            System.Console.WriteLine("21. Prescribe medicament");
            System.Console.WriteLine("22. Remove medicament prescription");
            System.Console.WriteLine("0. Exit");
            System.Console.Write("Choose: ");
        }

        private bool ProcessCommand(string? command)
        {
            switch (command)
            {
                case "1":
                    _patientService.Add();
                    break;
                case "2":
                    _patientService.Update();
                    break;
                case "3":
                    _patientService.Delete();
                    break;
                case "4":
                    _patientService.List();
                    break;
                case "5":
                    _doctorService.Add();
                    break;
                case "6":
                    _doctorService.Update();
                    break;
                case "7":
                    _doctorService.Delete();
                    break;
                case "8":
                    _doctorService.List();
                    break;
                case "9":
                    _visitationService.Add();
                    break;
                case "10":
                    _visitationService.Update();
                    break;
                case "11":
                    _visitationService.Delete();
                    break;
                case "12":
                    _visitationService.List();
                    break;
                case "13":
                    _diagnoseService.Add();
                    break;
                case "14":
                    _diagnoseService.Update();
                    break;
                case "15":
                    _diagnoseService.Delete();
                    break;
                case "16":
                    _diagnoseService.List();
                    break;
                case "17":
                    _medicamentService.Add();
                    break;
                case "18":
                    _medicamentService.Update();
                    break;
                case "19":
                    _medicamentService.Delete();
                    break;
                case "20":
                    _medicamentService.List();
                    break;
                case "21":
                    _medicamentService.Prescribe();
                    break;
                case "22":
                    _medicamentService.RemovePrescription();
                    break;
                case "0":
                    return false;
                default:
                    System.Console.WriteLine("Invalid option.");
                    break;
            }

            return true;
        }
    }
}
