
using AutoMapper;
using Mc2.CrudTest.Application.CommandHandlers;
using Mc2.CrudTest.Application.Commands;
using Mc2.CrudTest.Application.DTO;
using Mc2.CrudTest.Application.Interfaces;
using Mc2.CrudTest.Application.Mapping;
using Mc2.CrudTest.Application.Queries;
using Mc2.CrudTest.Application.Services;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Infrastructure.Data;
using Mc2.CrudTest.Presentation.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;



namespace Mc2.CrudTest.Application.Tests
{
    public class CustomerServiceTests
    {
        private readonly IMapper _mapper;
        private readonly DbContextOptions<Infrastructure.Data.ApplicationDbContext> _dbContextOptions;

        public CustomerServiceTests()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            var mapper = mapperConfiguration.CreateMapper();
            _mapper = mapper;


            _dbContextOptions = new DbContextOptionsBuilder<Infrastructure.Data.ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
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

            var mockQueryHandler = new Mock<IQueryHandler<GetCustomerByIdQuery, CustomerDTO>>();
            mockQueryHandler.Setup(handler => handler.Handle(It.IsAny<GetCustomerByIdQuery>())).ReturnsAsync(new CustomerDTO
            {
                Id = expectedCustomer.Id,
                FirstName = expectedCustomer.FirstName,
                LastName = expectedCustomer.LastName,
                DateOfBirth = expectedCustomer.DateOfBirth,
                PhoneNumber = expectedCustomer.PhoneNumber,
                Email = expectedCustomer.Email,
                BankAccountNumber = expectedCustomer.BankAccountNumber
            });

            var controller = new CustomersController(null, null, null, mockQueryHandler.Object, null);

            // Act
            var actionResult = await controller.Get(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var actualCustomer = Assert.IsType<CustomerDTO>(okResult.Value);
            Assert.Equal(expectedCustomer.Id, actualCustomer.Id);
            Assert.Equal(expectedCustomer.FirstName, actualCustomer.FirstName);
            Assert.Equal(expectedCustomer.LastName, actualCustomer.LastName);
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
                FirstName = c.FirstName,
                LastName = c.LastName,
                DateOfBirth = c.DateOfBirth,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                BankAccountNumber = c.BankAccountNumber
            }).ToList();

            var mockQueryHandler = new Mock<IQueryHandler<GetAllCustomersQuery, IEnumerable<CustomerDTO>>>();
            mockQueryHandler.Setup(handler => handler.Handle(It.IsAny<GetAllCustomersQuery>())).ReturnsAsync(expectedCustomerDTOs);

            var controller = new CustomersController(null, null, null, null, mockQueryHandler.Object);

            // Act
            var actionResult = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var actualCustomers = Assert.IsAssignableFrom<IEnumerable<CustomerDTO>>(okResult.Value);
            Assert.Equal(expectedCustomerDTOs.Count, actualCustomers.Count());
        }


        [Fact]
        public async Task AddCustomer_WithValidCustomer_ShouldAddCustomerToDatabase()
        {
            // Arrange
            var customerDto = CreateCustomerDto();
            var command = new CreateCustomerCommand(customerDto);
            var mockRepository = new Mock<ICustomerRepository>();
            mockRepository.Setup(repo => repo.IsEmailUniqueAsync(It.IsAny<string>())).ReturnsAsync(true);
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CustomerDTO, Customer>(customerDto)).Returns(new Customer());
            var handler = new CreateCustomerCommandHandler(mockMapper.Object, mockRepository.Object);

            // Act
            await handler.Handle(command);

            // Assert
            mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Customer>()), Times.Once);
        }

        public class CreateCustomerCommandHandlerTests
        {
            [Fact]
            public async Task Handle_WithInvalidPhoneNumber_ThrowsArgumentException()
            {
                // Arrange
                var customerDto = new CustomerDTO
                {
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1980, 1, 1),
                    PhoneNumber = "invalid",
                    Email = "johndoe@example.com",
                    BankAccountNumber = "123-456-789"
                };

                var mockMapper = new Mock<IMapper>();
                var mockCustomerRepository = new Mock<ICustomerRepository>();

                var handler = new CreateCustomerCommandHandler(mockMapper.Object, mockCustomerRepository.Object);

                // Act & Assert
                await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(new CreateCustomerCommand(customerDto)));
            }
        }

        //[Fact]
        //public async Task AddCustomer_WithInvalidEmail_ShouldThrowArgumentException()
        //{
        //    // Arrange
        //    var customerDto = new CustomerDTO
        //    {
        //        FirstName = "alireza",
        //        LastName = "Q",
        //        DateOfBirth = new DateTime(1984, 1, 1),
        //        PhoneNumber = "123-456-7890",
        //        Email = "invalid email address",
        //        BankAccountNumber = "123-456-789"
        //    };

        //    var mockCommandHandler = new Mock<ICommandHandler<CreateCustomerCommand>>();
        //    var mockQueryHandler = new Mock<IQueryHandler<IsEmailUniqueQuery, bool>>();
        //    mockQueryHandler.Setup(handler => handler.Handle(It.IsAny<IsEmailUniqueQuery>())).ReturnsAsync(false);

        //    var customerService = new CustomerService(_mapper, mockCommandHandler.Object, mockQueryHandler.Object);

        //    // Act & Assert
        //    var ex = await Assert.ThrowsAsync<ArgumentException>(() => customerService.AddCustomer(customerDto));
        //    Assert.Equal("Invalid email address", ex.Message);
        //}


        [Fact]
        public async Task AddCustomer_WithDuplicateEmail_ShouldThrowException()
        {
            // Arrange
            var customerDto = new CustomerDTO
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "+989391215575",
                Email = "johndoe@example.com",
                BankAccountNumber = "123-456-789"
            };

            // Mock ICustomerRepository
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            mockCustomerRepository.Setup(x => x.IsEmailUniqueAsync(customerDto.Email)).ReturnsAsync(false);

            // IMapper mock
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<Customer>(It.IsAny<CustomerDTO>())).Returns<CustomerDTO>(dto => new Customer(
                dto.FirstName,
                dto.LastName,
                dto.DateOfBirth,
                dto.PhoneNumber,
                dto.Email,
                dto.BankAccountNumber
            ));

            var createCustomerCommand = new CreateCustomerCommand(customerDto);
            var createCustomerCommandHandler = new CreateCustomerCommandHandler(mockMapper.Object, mockCustomerRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => createCustomerCommandHandler.Handle(createCustomerCommand));
        }



        [Fact]
        public void TestCustomerToCustomerDtoMapping()
        {
            // Arrange
            var customer = new Customer(
               firstname: "John",
               lastname: "Doe",
               dateOfBirth: new DateTime(1980, 1, 1),
               phoneNumber: "123-456-7890",
               email: "johndoe@example.com",
              bankAccountNumber: "123-456-789")
            {
                Id = 1
            }; 
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();

            // Act
            var customerDto = mapper.Map<CustomerDTO>(customer);

            // Assert
            Assert.Equal(customer.Id, customerDto.Id);
            Assert.Equal(customer.FirstName, customerDto.FirstName);
            Assert.Equal(customer.LastName, customerDto.LastName);
            Assert.Equal(customer.Email, customerDto.Email);
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
