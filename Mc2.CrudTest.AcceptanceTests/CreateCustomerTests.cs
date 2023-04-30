using AutoMapper;
using Mc2.CrudTest.Application.DTO;
using Mc2.CrudTest.Application.Repositories;
using Mc2.CrudTest.Application.Services;
using Moq;
using System;
using Xunit;

namespace Mc2.CrudTest.AcceptanceTests
{
    public class BddTddTests
    {
        //[Fact]
        //public void CreateCustomerValid_ReturnsSuccess()
        //{
        //    // Todo: Refer to readme.md 
        //}

        [Fact]
        public async void CreateCustomerWithInvalidPhoneNumber_ReturnsError()
        {
            // Arrange
            var customerData = new CustomerDTO
            {
                FirstName = "Sara",
                LastName = "White",
                DateOfBirth = new DateTime(1980, 1, 1),
                PhoneNumber = "invalid",
                Email = "ghxalireza@gmail.com",
                BankAccountNumber = "122345wdfg789"
            };

            var mockMapper = new Mock<IMapper>();
            var mockRepository = new Mock<ICustomerRepository>();
            var customerService = new CustomerService(mockMapper.Object, mockRepository.Object);

            
            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => customerService.AddCustomer(customerData));

            // Assert
            Assert.Equal("Invalid phone number", ex.Message);
        }


        // Please create more tests based on project requirements as per in readme.md
    }
}
