namespace P01_HospitalDatabase.Services.Contracts
{
    public interface IMedicamentService
    {
        void Add();

        void Update();

        void Delete();

        void List();

        void Prescribe();

        void RemovePrescription();
    }
}
