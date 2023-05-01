using AutoMapper;
using Mc2.CrudTest.Application.CommandHandlers;
using Mc2.CrudTest.Application.Commands;
using Mc2.CrudTest.Application.DTO;
using Mc2.CrudTest.Application.Interfaces;
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
            var createCustomerCommandHandler = new CreateCustomerCommandHandler(mockMapper.Object, mockRepository.Object);
            var createCustomerCommand = new CreateCustomerCommand(customerData);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => createCustomerCommandHandler.Handle(createCustomerCommand));
        }



        // Please create more tests based on project requirements as per in readme.md
    }
}
