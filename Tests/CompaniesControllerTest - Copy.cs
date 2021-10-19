/*using AutoMapper;
using GenFu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini_HR_app;
using Mini_HR_app.Controllers;
using Mini_HR_app.Data;
using Mini_HR_app.Models;
using Mini_HR_app.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class TestsFixture : IDisposable
    {
        private ApplicationDbContext _context;        

        public TestsFixture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            _context = new ApplicationDbContext(options);

            CreateTestData(_context);
        }

        public void Dispose()
        {
            foreach (var person in _context.People)
            {
                _context.Remove(person);
            }
            _context.SaveChanges();

            foreach (var employee in _context.Employees)
            {
                _context.Remove(employee);
            }
            _context.SaveChanges();

            foreach (var company in _context.Companies)
            {
                foreach (var companyEmployee in company.CompanyEmployees)
                {
                    _context.Remove(companyEmployee);
                }
            }
            _context.SaveChanges();

            foreach (var company in _context.Companies)
            {
                _context.Remove(company);
            }
            _context.SaveChanges();
        }

        private static void CreateTestData(ApplicationDbContext _context)
        {
            var id = 1;
            GenFu.GenFu.Configure<Company>()
                .Fill(p => p.Id, () => id++)
                .Fill(p => p.CompanyName).AsCity()
                .Fill(c => c.FiscalCode)
                .Fill(c => c.RegistryNo).AsLoremIpsumWords()
                .Fill(c => c.Euid).AsTwitterHandle()
                .Fill(c => c.DateOfEstablishment).AsPastDate()
                .Fill(c => c.NaceCode)
                .Fill(c => c.Activity).AsLoremIpsumWords()
                .Fill(c => c.ActivityDescription).AsLoremIpsumSentences()
                .Fill(c => c.Status == true);

            var companies = GenFu.GenFu.ListOf<Company>(10);

            _context.Companies.AddRange(companies);
            _context.SaveChanges();

            id = 1;
            GenFu.GenFu.Configure<Person>()
                .Fill(p => p.Id, () => id++)
                .Fill(p => p.Name).AsLastName()
                .Fill(p => p.Surname).AsFirstName()
                .Fill(p => p.Ssn)
                .Fill(p => p.BirthDate).AsPastDate()
                .Fill(p => p.BirthPlace).AsUsaState();

            var people = GenFu.GenFu.ListOf<Person>(150);

            _context.People.AddRange(people);
            _context.SaveChanges();

            id = 1;
            GenFu.GenFu.Configure<Employee>()
                .Fill(e => e.Id, () => id++)
                .Fill(e => e.Person).WithRandom(people);

            var employees = GenFu.GenFu.ListOf<Employee>(100);

            _context.Employees.AddRange(employees);
            _context.SaveChanges();

            GenFu.GenFu.Configure<CompanyEmployee>()
                .Fill(c => c.Company).WithRandom(companies)
                .Fill(c => c.Employee).WithRandom(employees)
                .Fill(companies => companies.Status == true);

            var companyEmployees = GenFu.GenFu.ListOf<CompanyEmployee>(100);

            int i = 0;

            foreach (var company in _context.Companies)
            {
                company.CompanyEmployees.Add(companyEmployees[i]);
            }

            _context.SaveChanges();
        }
    }

    public class CompaniesControllerTest : IClassFixture<TestsFixture>
    {
        private TestsFixture _data;

        public CompaniesControllerTest(TestsFixture data)
        {
            _data = data;
        }

        [Fact]
        public void AutoMapper_Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            config.AssertConfigurationIsValid();
        }       

        [Fact]
        public async Task GetCompanies_ReturnsAnActionResult_WithAListOfCompanies()
        {
            // Arrange
            var data = new CompaniesControllerTest(_data);

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = config.CreateMapper();

            var controller = new CompaniesController(_data, mapper);

            // Act
            var result = await controller.GetCompanies();

            // Assert
            var viewResult = Assert.IsType<ActionResult<IEnumerable<CompanyViewModel>>>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<CompanyViewModel>>(viewResult);
            Assert.Equal(2, model.Count());
        }

        [Theory]
        [InlineData(12)]
        [InlineData(120)]
        public async Task GetCompanyDetails_ReturnsAnActionResult_WithDetailsOfACompany(int id)
        {
            // Arrange
            var data = new CompaniesControllerTest(_data);

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = config.CreateMapper();

            var controller = new CompaniesController(_data, mapper);

            // Act
            var result = await controller.GetCompanyDetails(id);

            // Assert
            var viewResult = Assert.IsType<ActionResult<CompanyViewModel>>(result);
            var model = Assert.IsAssignableFrom<CompanyViewModel>(viewResult);
            Assert.Equal(12, model.Id);
        }

        [Theory]
        [InlineData(12)]
        [InlineData(120)]
        public async Task GetActiveEmployees_ReturnsAnActionResult_WithActiveEmployeesOfACompany(int id)
        {
            // Arrange
            var data = new CompaniesControllerTest(_data);

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = config.CreateMapper();

            var controller = new CompaniesController(_data, mapper);

            // Act
            var result = await controller.GetCompanyDetails(id);

            // Assert
            var viewResult = Assert.IsType<ActionResult<CompanyWithEmployeesViewModel>>(result);
            var model = Assert.IsAssignableFrom<CompanyWithEmployeesViewModel>(viewResult);
            Assert.Equal(12, model.Id);
            Assert.Equal(30, model.Employees.Count);
        }

        [Theory]
        [InlineData(12, 30)]
        [InlineData(12, 50)]
        [InlineData(120, 30)]
        [InlineData(120, 150)]
        public async Task GetEmployeeDetails_ReturnsAnActionResult_WithDetailsOfActiveEmployeeOfACompany(int idCompany, int idEmployee)
        {
            // Arrange
            var data = new CompaniesControllerTest(_data);

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = config.CreateMapper();

            var controller = new CompaniesController(_data, mapper);

            // Act
            var result = await controller.GetEmployeeDetails(idCompany, idEmployee);            

            // Assert
            var viewResult = Assert.IsType<ActionResult<CompanyWithEmployeesViewModel>>(result);
            var model = Assert.IsAssignableFrom<CompanyWithEmployeesViewModel>(viewResult);
            Assert.Equal(12, model.Id);
            Assert.Single(model.Employees);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.GetCompanyDetailsFromDataGenerator), MemberType = typeof(TestDataGenerator))]
        public async Task PutCompanyDetails_ReturnsAnActionResult_WithNoContent(int idCompany, CompanyViewModel companyViewModel)
        {
            // Arrange
            var data = new CompaniesControllerTest(_data);

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = config.CreateMapper();

            var controller = new CompaniesController(_data, mapper);

            // Act
            var result = await controller.PutCompanyDetails(idCompany, companyViewModel);

            // Assert
            //var actionResult = result as 
            var actionResult = Assert.IsAssignableFrom<ActionResult>(result);

        }
    }

    public class TestDataGenerator
    {
        public static IEnumerable<object[]> GetEmployeeDetailsFromDataGenerator()
        {
            yield return new object[] { 5, new Person { Name = "Tribbiani", Ssn = "2920908234534" } };
            yield return new object[] { 7, new Person { Name = "Mancini", Ssn = "2920908234434" } };
        }

        public static IEnumerable<object[]> GetCompanyDetailsFromDataGenerator()
        {
            yield return new object[] { 1, new Company { CompanyName = "Best SRL", FiscalCode = "RO 38617091" } };
            yield return new object[] { 2, new Company { CompanyName = "Mancini", FiscalCode = "RO 38514091" } };
        }
    }
}
*/