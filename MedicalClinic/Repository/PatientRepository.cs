using MedicalClinic.Data;
using MedicalClinic.Interfaces;
using MedicalClinic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MedicalClinic.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Patient patient)
        {
            _context.Patients.Add(patient);
            return Save();
        }

        public bool Delete(Patient patient)
        {
            _context.Patients.Remove(patient);
            return Save();
        }

        public async Task<IEnumerable<Patient>> GetAll()
        {
            return await _context.Patients.ToListAsync();
        }

        public Task<Patient> GetByIdAsync(int id)
        {
            return _context.Patients.Include(p => p.Address).FirstOrDefaultAsync(p => p.Id == id);
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public bool Update(Patient patient)
        {
            _context.Patients.Update(patient);
            return Save();
        }
    }
}
