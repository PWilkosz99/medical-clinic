using MedicalClinic.Controllers;
using MedicalClinic.Interfaces;
using MedicalClinic.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MedicalClinic.Tests
{
    public class PatientControllerTests
    {
        [Fact]
        public async void Index_ReturnsViewResult_WithListOfPatients()
        {
            var mockRepository = new Mock<IPatientRepository>();
            var patients = new List<Patient>
            {
                new Patient { Id = 1, Firstname = "John", Lastname = "Doe", PersonalNumber = "12345" },
                new Patient { Id = 2, Firstname = "Jane", Lastname = "Smith", PersonalNumber = "67890" }
            };
            mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(patients);
            var controller = new PatientController(mockRepository.Object);

            var result = await controller.Index(null, null, null) as ViewResult;

            var model = Assert.IsAssignableFrom<IEnumerable<Patient>>(result.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async void Index_ReturnsViewResult_WithFilteredPatients()
        {
            var mockRepository = new Mock<IPatientRepository>();
            var patients = new List<Patient>
            {
                new Patient { Id = 1, Firstname = "John", Lastname = "Doe", PersonalNumber = "12345" },
                new Patient { Id = 2, Firstname = "Jane", Lastname = "Smith", PersonalNumber = "67890" },
            };
            mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(patients);
            var controller = new PatientController(mockRepository.Object);

            var result = await controller.Index(null, "John", null) as ViewResult;

            var model = Assert.IsAssignableFrom<IEnumerable<Patient>>(result.ViewData.Model);
            Assert.Single(model);
            Assert.Equal("John", model.First().Firstname);
        }

        [Fact]
        public async void Index_ReturnsViewResult_WithSortedPatients()
        {
            var mockRepository = new Mock<IPatientRepository>();
            var patients = new List<Patient>
            {
                new Patient { Id = 1, Firstname = "John", Lastname = "Doe", PersonalNumber = "12345" },
                new Patient { Id = 2, Firstname = "Jane", Lastname = "Smith", PersonalNumber = "67890" }
            };
            mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(patients);
            var controller = new PatientController(mockRepository.Object);

            var result = await controller.Index("lastname_asc", null, null) as ViewResult;

            var model = Assert.IsAssignableFrom<IEnumerable<Patient>>(result.ViewData.Model);
            Assert.Equal("Doe", model.First().Lastname);
        }

        [Fact]
        public async void Index_ReturnsViewResult_WithPaginatedPatients()
        {
            var mockRepository = new Mock<IPatientRepository>();
            var patients = Enumerable.Range(1, 10).Select(i => new Patient
            {
                Id = i,
                Firstname = $"Patient {i}",
                Lastname = $"Lastname {i}",
                PersonalNumber = $"1234{i}"
            }).ToList();
            mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(patients);
            var controller = new PatientController(mockRepository.Object);
            var result = await controller.Index(null, null, 2) as ViewResult;

            var model = Assert.IsAssignableFrom<IEnumerable<Patient>>(result.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async void Detail_ReturnsNotFound_WhenIdIsNull()
        {
            var mockRepository = new Mock<IPatientRepository>();
            var controller = new PatientController(mockRepository.Object);

            var result = await controller.Detail(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Detail_ReturnsNotFound_WhenPatientNotFound()
        {
            var mockRepository = new Mock<IPatientRepository>();
            mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Patient)null);
            var controller = new PatientController(mockRepository.Object);

            var result = await controller.Detail(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Detail_ReturnsViewResult_WithPatientDetails()
        {
            var patientId = 1;
            var patient = new Patient { Id = patientId, Firstname = "John", Lastname = "Doe", PersonalNumber = "12345" };
            var mockRepository = new Mock<IPatientRepository>();
            mockRepository.Setup(repo => repo.GetByIdAsync(patientId)).ReturnsAsync(patient);
            var controller = new PatientController(mockRepository.Object);

            var result = await controller.Detail(patientId) as ViewResult;

            Assert.NotNull(result);
            var model = Assert.IsType<Patient>(result.ViewData.Model);
            Assert.Equal(patientId, model.Id);
            Assert.Equal("John", model.Firstname);
            Assert.Equal("Doe", model.Lastname);
            Assert.Equal("12345", model.PersonalNumber);
        }

        [Fact]
        public void Create_ReturnsViewResult()
        {
            var mockRepository = new Mock<IPatientRepository>();
            var controller = new PatientController(mockRepository.Object);

            var result = controller.Create();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_RedirectsToAction_Index_WhenModelStateIsValid()
        {
            var patient = new Patient { Firstname = "John", Lastname = "Doe", PersonalNumber = "12345" };
            var mockRepository = new Mock<IPatientRepository>();
            mockRepository.Setup(repo => repo.Add(patient)).Returns(true);
            var controller = new PatientController(mockRepository.Object);
            controller.ModelState.Clear();

            var result = controller.Create(patient) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(nameof(PatientController.Index), result.ActionName);
        }

        [Fact]
        public void Update_RedirectsToAction_Index_WhenModelStateIsValid()
        {
            var patient = new Patient { Id = 1, Firstname = "John", Lastname = "Doe", PersonalNumber = "12345" };
            var mockRepository = new Mock<IPatientRepository>();
            mockRepository.Setup(repo => repo.Update(patient)).Returns(true);
            var controller = new PatientController(mockRepository.Object);
            controller.ModelState.Clear();

            var result = controller.Update(patient) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(nameof(PatientController.Index), result.ActionName);
        }

        [Fact]
        public void Update_ReturnsRedirectToAction_Detail_WhenModelStateIsInvalid()
        {
            var invalidPatient = new Patient { };
            var mockRepository = new Mock<IPatientRepository>();
            var controller = new PatientController(mockRepository.Object);
            controller.ModelState.AddModelError("Firstname", "Firstname is required");

            var result = controller.Update(invalidPatient) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(nameof(PatientController.Detail), result.ActionName);
            Assert.Equal(invalidPatient.Id, result.RouteValues["id"]);
        }

        [Fact]
        public async void Remove_ReturnsNotFound_WhenIdIsNull()
        {
            var mockRepository = new Mock<IPatientRepository>();
            var controller = new PatientController(mockRepository.Object);

            var result = await controller.Remove(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Remove_ReturnsNotFound_WhenPatientNotFound()
        {
            int patientId = 1;
            var mockRepository = new Mock<IPatientRepository>();
            mockRepository.Setup(repo => repo.GetByIdAsync(patientId)).ReturnsAsync((Patient)null);
            var controller = new PatientController(mockRepository.Object);

            var result = await controller.Remove(patientId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Remove_ReturnsRedirectToAction_Index_WhenPatientIsRemoved()
        {
            int patientId = 1;
            var patientToRemove = new Patient { Id = patientId };
            var mockRepository = new Mock<IPatientRepository>();
            mockRepository.Setup(repo => repo.GetByIdAsync(patientId)).ReturnsAsync(patientToRemove);
            var controller = new PatientController(mockRepository.Object);

            var result = await controller.Remove(patientId) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(nameof(PatientController.Index), result.ActionName);
        }
    }
}
