
using AutoMapper;
using Mc2.CrudTest.Application.DTO;
using Mc2.CrudTest.Application.Mapping;
using Mc2.CrudTest.Application.Repositories;
using Mc2.CrudTest.Application.Services;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Infrastructure.Data;
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

            var mockMapper = new Mock<IMapper>();
            var mockRepository = new Mock<ICustomerRepository>();
            mockRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(expectedCustomer);
            var customerService = new CustomerService(mockMapper.Object, mockRepository.Object);
            // Act
            var actualCustomer = await customerService.GetCustomerById(id);

            // Assert
            Assert.NotNull(actualCustomer);
            Assert.Equal(expectedCustomer.Id, actualCustomer.Id);
            Assert.Equal(expectedCustomer.FirstName, actualCustomer.FirstName);
            Assert.Equal(expectedCustomer.LastName, actualCustomer.LastName);
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
                FirstName = c.FirstName,
                LastName = c.LastName,
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

        [Fact]
        public async Task AddCustomer_WithValidCustomer_ShouldAddCustomerToDatabase()
        {
            // Arrange
            var customerDto = CreateCustomerDto();

            using (var context = new ApplicationDbContext(_dbContextOptions))
            {

                // Add a customer to the database so that there is at least one customer in the database
                var customer = new Customer("alireza", "Qanbarzadeh", new DateTime(1984, 1, 1), "+60173771596", "areza@gmail.com", "DE89370400440532013000"); 
                await context.Customers.AddAsync(customer);
                await context.SaveChangesAsync();

                var mockCustomerRepository = new Mock<ICustomerRepository>();
                mockCustomerRepository.Setup(repo => repo.IsEmailUniqueAsync(It.IsAny<string>())).ReturnsAsync(true);

                var customerService = new CustomerService(_mapper, mockCustomerRepository.Object);

                // Act
                await customerService.AddCustomer(customerDto);

                // Assert
                var savedCustomer = await context.Customers.SingleAsync();
                Assert.Equal(customerDto.FirstName, savedCustomer.FirstName);
                Assert.Equal(customerDto.LastName, savedCustomer.LastName);
                Assert.Equal(customerDto.DateOfBirth, savedCustomer.DateOfBirth);
                Assert.Equal(customerDto.PhoneNumber, savedCustomer.PhoneNumber);
                Assert.Equal(customerDto.Email, savedCustomer.Email);
                Assert.Equal(customerDto.BankAccountNumber, savedCustomer.BankAccountNumber);
            }
        }




        [Fact]
        public async Task AddCustomer_WithInvalidPhoneNumber_ThrowsArgumentException()
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

            var customerService = new CustomerService(mockMapper.Object, mockCustomerRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => customerService.AddCustomer(customerDto));
        }

        [Fact]
        public async Task AddCustomer_WithInvalidEmail_ShouldThrowArgumentException()
        {
            // Arrange
            var customerDto = new CustomerDTO
            {
                FirstName = "alireza",
                LastName = "Q",
                DateOfBirth = new DateTime(1984, 1, 1),
                PhoneNumber = "123-456-7890",
                Email = "invalid email address",
                BankAccountNumber = "123-456-789"
            };

            var mockMapper = new Mock<IMapper>();
            var mockRepository = new Mock<ICustomerRepository>();
            var customerService = new CustomerService(mockMapper.Object, mockRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => customerService.AddCustomer(customerDto));
        }

        [Fact]
        public async Task AddCustomer_WithInvalidBankAccountNumber_ShouldThrowArgumentException()
        {
            // Arrange
            var customerDto = CreateCustomerDto();
            customerDto.BankAccountNumber = "invalid23452345";

            var mockCustomerRepository = new Mock<ICustomerRepository>();
            mockCustomerRepository.Setup(repo => repo.IsEmailUniqueAsync(It.IsAny<string>())).ReturnsAsync(true);

            var customerService = new CustomerService(_mapper, mockCustomerRepository.Object);

            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => customerService.AddCustomer(customerDto));

            // Assert
            Assert.Equal("Invalid bank account number", ex.Message);
        }


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
            mockCustomerRepository.Setup(x => x.IsEmailUniqueAsync(It.IsAny<string>())).ReturnsAsync(false);

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

            // Create CustomerService instance
            var customerService = new CustomerService(mockMapper.Object, mockCustomerRepository.Object);

            // Act
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => customerService.AddCustomer(customerDto));
            // Assert
            Assert.Equal("Email already exists", ex.Message);

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
