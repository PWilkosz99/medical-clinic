using MedicalClinic.Models;

namespace MedicalClinic.Interfaces
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAll();
        Task<Patient> GetByIdAsync(int id);
        bool Add(Patient patient);
        bool Update(Patient patient);
        bool Delete(Patient patient);
        bool Save();
    }
}
