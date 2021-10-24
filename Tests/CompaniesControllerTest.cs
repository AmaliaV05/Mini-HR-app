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
    public class Data
    {
        private ApplicationDbContext _context;
        public void DataTest(ApplicationDbContext context)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB-HR")
                .Options;

            _context = new ApplicationDbContext(options);

            _context.Companies.Add(new Company { CompanyName = "p1 test", DateOfEstablishment = new DateTime(2008, 3, 1, 0, 0, 0) });
            _context.Companies.Add(new Company { CompanyName = "p2 test", DateOfEstablishment = new DateTime(2020, 7, 1, 0, 0, 0) });
            _context.SaveChanges();
        }
    }
    public class CompaniesControllerTest : Data
    {
        private readonly ApplicationDbContext _context;

        *//*[Fact]
        public void AutoMapper_Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            config.AssertConfigurationIsValid();
        }*//*

        [Fact]
        public async Task GetCompanies_ReturnsAnActionResult_WithAListOfCompanies()
        {
            // Arrange
            DataTest(_context);
            var data = _context;

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = config.CreateMapper();

            var controller = new CompaniesController(data, mapper);

            // Act
            var result = await controller.GetCompanies();

            // Assert
            var viewResult = Assert.IsType<ActionResult<IEnumerable<CompanyViewModel>>>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<CompanyViewModel>>(viewResult);
            Assert.Equal(2, model.Count());
        }

        *//*[Theory]
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

    }*//*
    }
}
*/