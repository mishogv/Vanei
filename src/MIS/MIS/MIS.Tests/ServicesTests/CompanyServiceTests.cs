namespace MIS.Tests.ServicesTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using Models;

    using NUnit.Framework;

    using Services;

    public class CompanyServiceTests : BaseServiceTests
    {
        private MISDbContext dbContext;
        private ICompanyService companyService;

        private const string CompanyName = "Microsoft";
        private const string CompanyAddress = "MicrosoftStreet";

        [SetUp]
        public void Init()
        {
            var options = new DbContextOptionsBuilder<MISDbContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString())
                          .Options;

            this.dbContext = new MISDbContext(options);

            var userService = new UserService(this.dbContext);
            this.companyService = new CompanyService(this.dbContext, userService);
        }

        [Test]
        public async Task CreateCompany_WithValidData_ShouldReturnCorrectCompany()
        {
            var actual = await this.companyService.CreateAsync(CompanyName, CompanyAddress);
            var expected = await this.dbContext.Companies.FirstOrDefaultAsync();

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public async Task EditCompany_WithValidData_ShouldReturnCorrectCompany()
        {
            var company = await this.companyService.CreateAsync(CompanyName, CompanyAddress);
            var editedCompany = await this.companyService.EditAsync(company.Id, "Asd", "asd");

            Assert.AreEqual(company.Id, editedCompany.Id);
            Assert.AreEqual("Asd", editedCompany.Name);
            Assert.AreEqual("asd", editedCompany.Address);
        }

        [Test]
        public async Task EditCompany_WithInvalidId_ShouldReturnNull()
        {
            var editedCompany = await this.companyService.EditAsync("123", "Asd", "asd");

            Assert.IsNull(editedCompany);
        }

        [Test]
        public async Task DeleteCompany_WithValidData_ShouldReturnCorrectCompanyId()
        {
            var company = await this.companyService.CreateAsync(CompanyName, CompanyAddress);
            var deletedCompany = await this.companyService.DeleteAsync(company.Id);

            Assert.AreEqual(company.Id, deletedCompany.Id);
        }

        [Test]
        public async Task DeleteCompany_WithInvalidId_ShouldReturnNull()
        {
            var deletedCompany = await this.companyService.DeleteAsync("423");

            Assert.IsNull(deletedCompany);
        }

        [Test]
        public async Task GetCompany_WithValidData_ShouldReturnCorrectCompany()
        {
            var company = await this.companyService.CreateAsync(CompanyName, CompanyAddress);
            var actual = await this.companyService.GetCompanyAsync(company.Id);

            Assert.AreEqual(company.Id, actual.Id);
            Assert.AreEqual(company.Name, actual.Name);
            Assert.AreEqual(company.Address, actual.Address);
        }

        [Test]
        public async Task GetCompany_WithInvalidId_ShouldReturnNull()
        {
            var actual = await this.companyService.GetCompanyAsync("asd");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task RemoveEmployee_WithValidData_ShouldReturnCorrectEmployee()
        {
            var company = await this.companyService.CreateAsync(CompanyName, CompanyAddress);

            var employee = new MISUser()
            {
                CompanyId = company.Id,
                Email = "pesho",
                FirstName = "pesho",
                LastName = "pesho",
                UserName = "pesho",
            };

            await this.dbContext.AddAsync(employee);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.companyService.RemoveEmployeeAsync(employee.Id);

            Assert.AreEqual(0, actual.Employees.Count);
        }

        [Test]
        public async Task RemoveEmployee_WithInvalidData_ShouldReturnNull()
        {
            var company = await this.companyService.CreateAsync(CompanyName, CompanyAddress);

            var employee = new MISUser()
            {
                CompanyId = company.Id,
                Email = "pesho",
                FirstName = "pesho",
                LastName = "pesho",
                UserName = "pesho",
            };

            await this.dbContext.AddAsync(employee);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.companyService.RemoveEmployeeAsync("asd");

            Assert.IsNull(actual);
        }

        [Test]
        public async Task SetCompany_WithValidData_ShouldReturnCorrectCompanyId()
        {
            var company = await this.companyService.CreateAsync(CompanyName, CompanyAddress);

            var message = new Message() {AddedOn = DateTime.UtcNow, Username = "admin", Text = "asd"};

            await this.companyService.SetCompanyAsync(message, company.Id);

            Assert.AreEqual(company.Id, message.Company.Id);
        }

        [Test]
        public async Task SetCompany_WithInvalidId_ShouldReturnNull()
        {
            var company = await this.companyService.CreateAsync(CompanyName, CompanyAddress);

            var message = new Message() {AddedOn = DateTime.UtcNow, Username = "admin", Text = "asd"};

            await this.companyService.SetCompanyAsync(message, "asdasd");

            Assert.IsNull(message.Company);
        }

        [Test]
        public async Task SetWarehouseCompany_WithValidData_ShouldSetCorrectCompanyAndWarehouseIsFavoriteTrue()
        {
            var company = await this.companyService.CreateAsync(CompanyName, CompanyAddress);

            var wareHouse = new WareHouse() {Name = "Fruits"};

            await this.companyService.SetCompanyAsync(wareHouse, company.Id);

            Assert.AreEqual(company.Id, wareHouse.Company.Id);
            Assert.AreEqual(true, wareHouse.IsFavorite);
        }

        [Test]
        public async Task SetWarehouseCompany_WithValidData_ShouldSetCorrectCompanyAndWarehouseIsFavoriteFalse()
        {
            var company = await this.companyService.CreateAsync(CompanyName, CompanyAddress);

            var wareHouseForDb = new WareHouse()
            {
                Name = "Vegetables",
                IsFavorite = true,
                Company = await this.dbContext.Companies.FirstOrDefaultAsync(x => x.Id == company.Id)
            };

            await this.dbContext.AddAsync(wareHouseForDb);
            await this.dbContext.SaveChangesAsync();

            var wareHouse = new WareHouse() {Name = "Fruits"};

            await this.companyService.SetCompanyAsync(wareHouse, company.Id);

            Assert.AreEqual(company.Id, wareHouse.Company.Id);
            Assert.AreEqual(false, wareHouse.IsFavorite);
        }

        [Test]
        public async Task SetWarehouseCompany_WithInvalidData_ShouldReturnNull()
        {
            var company = await this.companyService.CreateAsync(CompanyName, CompanyAddress);

            var wareHouse = new WareHouse() {Name = "Fruits"};

            await this.companyService.SetCompanyAsync(wareHouse, "asd");

            Assert.IsNull(wareHouse.Company);
        }

        [Test]
        public async Task CreateCompany_WithValidData_ShouldWorkCorrectly()
        {
            var user = new MISUser()
            {
                Email = "pesho", FirstName = "pesho", LastName = "pesho", UserName = "pesho",
            };

            await this.dbContext.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            var company = await this.companyService.CreateAsync(CompanyName, CompanyAddress, user.Id);

            var employee = company.Employees.FirstOrDefault();

            Assert.AreEqual(user.Id, employee?.Id);
        }

        [Test]
        public async Task CreateCompany_WithInvalidData_ShouldReturnNull()
        {
            var user = new MISUser()
            {
                Email = "pesho", FirstName = "pesho", LastName = "pesho", UserName = "pesho",
            };

            await this.dbContext.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            var company = await this.companyService.CreateAsync(CompanyName, CompanyAddress, "asd");

            Assert.IsNull(company);
        }
    }
}