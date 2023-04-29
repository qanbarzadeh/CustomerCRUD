
using AutoMapper;
using Mc2.CrudTest.Application.DTO;
using Mc2.CrudTest.Application.Repositories;
using Mc2.CrudTest.Application.Services;
using Mc2.CrudTest.Domain.Entities;
using Moq;



namespace Mc2.CrudTest.Application.Tests
{
    public class CustomerServiceTests
    {
        private readonly IMapper _mapper;
        public CustomerServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerDTO>();
            });

            _mapper = config.CreateMapper();
        }
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

            var mockMapper = new Mock<IMapper>();
            var mockRepository = new Mock<ICustomerRepository>();
            mockRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(expectedCustomer);
            var customerService = new CustomerService(mockMapper.Object, mockRepository.Object);
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

        [Fact]
        public async Task GetAllCustomers_ReturnsAllCustomers()
        {
            // Arrange
            var expectedCustomers = new List<Customer>()
            {
                 new Customer(
                 firstname: "John",
                 lastname: "Doe",
                 dateOfBirth: new DateTime(1980, 1, 1),
                 phoneNumber: "123-456-7890",
                 email: "johndoe@example.com",
                bankAccountNumber: "123-456-789")
                {
                Id = 1
                },
                 new Customer(
                firstname: "Jane",
                lastname: "Smith",
             dateOfBirth: new DateTime(1990, 5, 15),
            phoneNumber: "234-567-8901",
            email: "janesmith@example.com",
            bankAccountNumber: "234-567-890")
        {
            Id = 2
    },
            new Customer(
            firstname: "Bob",
            lastname: "Johnson",
            dateOfBirth: new DateTime(1985, 10, 20),
            phoneNumber: "345-678-9012",
            email: "bobjohnson@example.com",
            bankAccountNumber: "345-678-901")
          {
        Id = 3
         }
            };


            var expectedCustomerDTOs = expectedCustomers.Select(c => new CustomerDTO
            {
                Id = c.Id,
                Firstname = c.Firstname,
                Lastname = c.Lastname,
                DateOfBirth = c.DateOfBirth,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                BankAccountNumber = c.BankAccountNumber
            }).ToList();

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CustomerDTO>(It.IsAny<Customer>()))
                .Returns((Customer source) => expectedCustomerDTOs.Single(dto => dto.Id == source.Id));

            var mockRepository = new Mock<ICustomerRepository>();
            mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedCustomers);

            var customerService = new CustomerService(mockMapper.Object, mockRepository.Object);

            // Act
            var actualCustomers = await customerService.GetAllCustomers();

            // Assert
            Assert.NotNull(actualCustomers);
            Assert.Equal(expectedCustomers.Count, actualCustomers.Count());

        }
    }
}
