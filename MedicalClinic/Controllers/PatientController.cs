using MedicalClinic.Data;
using MedicalClinic.Interfaces;
using MedicalClinic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinic.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientRepository _patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<IActionResult> Index(string sortOrder, string search, int? page)
        {
            const int pageSize = 2;
            int pageIndex = page ?? 1;

            var patients = await _patientRepository.GetAll();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();

                patients = patients.Where(p =>
                    p.Firstname.ToLower().Contains(search) ||
                    p.Lastname.ToLower().Contains(search) ||
                    p.PersonalNumber.ToLower().Contains(search)
                );
            }

            patients = sortOrder switch
            {
                "firstname_asc" => patients.OrderBy(p => p.Firstname),
                "firstname_desc" => patients.OrderByDescending(p => p.Firstname),
                "lastname_asc" => patients.OrderBy(p => p.Lastname),
                "lastname_desc" => patients.OrderByDescending(p => p.Lastname),
                "personalnumber_asc" => patients.OrderBy(p => p.PersonalNumber),
                "personalnumber_desc" => patients.OrderByDescending(p => p.PersonalNumber),
                _ => patients.OrderBy(p => p.Firstname),
            };

            ViewBag.TotalPages = (int)Math.Ceiling(patients.Count() / (double)pageSize);
            ViewBag.PageIndex = pageIndex;
            patients = patients.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            ViewBag.HasPreviousPage = pageIndex > 1;
            ViewBag.HasNextPage = pageIndex < ViewBag.TotalPages;

            ViewBag.SearchQuery = search;
            ViewBag.SortOrder = sortOrder;

            return View(patients);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Patient patient = await _patientRepository.GetByIdAsync(id.Value);

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _patientRepository.Add(patient);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Create));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _patientRepository.Update(patient);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Detail), new { id = patient.Id });
        }

        public async Task<IActionResult> Remove(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Patient patient = await _patientRepository.GetByIdAsync(id.Value);

            if (patient == null)
            {
                return NotFound();
            }

            _patientRepository.Delete(patient);
            return RedirectToAction(nameof(Index));
        }
    }
}
