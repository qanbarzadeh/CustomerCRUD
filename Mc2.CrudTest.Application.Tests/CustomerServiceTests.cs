
using Mc2.CrudTest.Application.Repositories;
using Mc2.CrudTest.Application.Services;
using Mc2.CrudTest.Domain.Entities;
using Moq;



namespace Mc2.CrudTest.Application.Tests
{
    public class CustomerServiceTests
    {
        [Fact]
        public async Task GetCustomerById_WithValidId_ReturnsCustomerDto()
        {
            // Arrange
            int id = 1;
            var expectedCustomer = new Customer(
                firstname: "Sara",
                lastname: "White",
                dateOfBirth: new DateTime(1990, 1, 1),
                phoneNumber: "123asdfas",
                email: "saraw@gmail.com",
                bankAccountNumber: "123-456-789")
            {
                Id = id
            };


            var mockRepository = new Mock<ICustomerRepository>();
            mockRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(expectedCustomer);
            var customerService = new CustomerService(mockRepository.Object);

            // Act
            var actualCustomer = await customerService.GetCustomerById(id);

            // Assert
            Assert.NotNull(actualCustomer);
            Assert.Equal(expectedCustomer.Id, actualCustomer.Id);
            Assert.Equal(expectedCustomer.Firstname, actualCustomer.Firstname);
            Assert.Equal(expectedCustomer.Lastname, actualCustomer.Lastname);
            Assert.Equal(expectedCustomer.DateOfBirth, actualCustomer.DateOfBirth);
            Assert.Equal(expectedCustomer.PhoneNumber, actualCustomer.PhoneNumber);
            Assert.Equal(expectedCustomer.Email, actualCustomer.Email);
            Assert.Equal(expectedCustomer.BankAccountNumber, actualCustomer.BankAccountNumber);
        }
    }
}
