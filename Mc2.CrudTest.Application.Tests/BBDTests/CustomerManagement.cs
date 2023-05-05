using AutoMapper;
using Mc2.CrudTest.Application.CommandHandlers;
using Mc2.CrudTest.Application.Commands;
using Mc2.CrudTest.Application.DTO;
using Mc2.CrudTest.Application.Interfaces;
using Mc2.CrudTest.Application.Mapping;
using Mc2.CrudTest.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TechTalk.SpecFlow;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Mc2.CrudTest.Application.Tests.Tests
{
    [TestClass]
    [Binding]
    public class CustomerManagementSteps
    {
        private CustomerDTO _customerDto;
        private Customer _createdCustomer;
        private Mock<ICustomerRepository> _mockCustomerRepository;
        private IMapper _mapper;

        public CustomerManagementSteps()
        {
            // Initialize IMapper instance
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfiguration.CreateMapper();

            // Initialize Mock<ICustomerRepository> instance
            _mockCustomerRepository = new Mock<ICustomerRepository>();
        }
       
    
        [Given(@"I have entered valid customer information")]        
        public async Task GivenIHaveEnteredValidCustomerInformation()
        {
            _customerDto = CreateCustomerDto();
        }
        
        
        [When(@"I request to create a new customer")]
        public async Task WhenIRequestToCreateANewCustomer()
        {
            _mockCustomerRepository.Setup(x => x.IsEmailUniqueAsync(_customerDto.Email)).ReturnsAsync(true);
            _mockCustomerRepository.Setup(x => x.AddAsync(It.IsAny<Customer>()))
                .Callback<Customer>(c => _createdCustomer = c);

            var command = new CreateCustomerCommand(_customerDto);
            var commandHandler = new CreateCustomerCommandHandler(_mapper, _mockCustomerRepository.Object);

            await commandHandler.Handle(command);
        }
      
        
        [Then(@"the new customer should be created and returned")]
        public async Task ThenTheNewCustomerShouldBeCreatedAndReturned()
        {
            Assert.IsNotNull(_createdCustomer);
            Assert.AreEqual(_customerDto.FirstName, _createdCustomer.FirstName);
            Assert.AreEqual(_customerDto.LastName, _createdCustomer.LastName);
          
        }

        private static CustomerDTO CreateCustomerDto()
        {
            return new CustomerDTO
            {
                FirstName = "alireza",
                LastName = "Qanbarzadeh",
                DateOfBirth = new DateTime(1984, 1, 1),
                PhoneNumber = "+60173771596",
                Email = "areza@gmail.com",
                BankAccountNumber = "DE89370400440532013000"
            };
        }
    }
}