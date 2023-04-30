using AutoMapper;
using Mc2.CrudTest.Application.DTO;
using Mc2.CrudTest.Application.Interfaces;
using Mc2.CrudTest.Application.Repositories;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Shared.Utilities;

namespace Mc2.CrudTest.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        

        public CustomerService(IMapper mapper , ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task AddCustomer(CustomerDTO customerDto)
        {
            if (customerDto == null)
            {
                throw new ArgumentNullException(nameof(customerDto));
            }

            // Validate the phone number
            if (!ValidationUtility.IsValidPhoneNumber(customerDto.PhoneNumber))
            {
                throw new ArgumentException("Invalid phone number");
            }

            // Validate the email
            if (!ValidationUtility.IsValidEmail(customerDto.Email))
            {
                throw new ArgumentException("Invalid email");
            }

            // Check if the email is unique
            if (!await _customerRepository.IsEmailUniqueAsync(customerDto.Email))
            {
                throw new ArgumentException("Email already exists");
            }

            // Validate the bank account number
            if (!ValidationUtility.IsValidBankAccountNumber(customerDto.BankAccountNumber))
            {
                throw new ArgumentException("Invalid bank account number");
            }


               var customer = _mapper.Map<Customer>(customerDto);
               await _customerRepository.AddAsync(customer);
            
        }

      

        public Task DeleteCustomer(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllCustomers()
        {
            try
            {
                var customers = await _customerRepository.GetAllAsync();

                if (customers == null)
                {
                    throw new Exception("No customers found");
                }

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Customer, CustomerDTO>();
                });

                IMapper mapper = config.CreateMapper();
                var customerDtos = mapper.Map<IEnumerable<CustomerDTO>>(customers);

                return customerDtos;
            }
            catch (Exception ex)
            {
                // Log the exception
                // Handle the exception gracefully, for example, by returning a default value or an error message
                throw ex;
            }
        }


        public async Task<CustomerDTO> GetCustomerById(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
            {
                return null;
            }

            var customerDto = new CustomerDTO
            {
                Id = customer.Id,
                Firstname = customer.Firstname,
                Lastname = customer.Lastname,
                DateOfBirth = customer.DateOfBirth,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                BankAccountNumber = customer.BankAccountNumber
            };

            return customerDto;
        }

        public async Task UpdateCustomer(int id, CustomerDTO customerDto)
        {
            if (customerDto == null)
            {
                throw new ArgumentNullException(nameof(customerDto));
            }

            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                throw new ArgumentException($"Customer with ID {id} not found");
            }

            // Validate the phone number
            if (!ValidationUtility.IsValidPhoneNumber(customerDto.PhoneNumber))
            {
                throw new ArgumentException("Invalid phone number");
            }

            // Validate the email
            if (!ValidationUtility.IsValidEmail(customerDto.Email))
            {
                throw new ArgumentException("Invalid email");
            }

          

            // Validate the bank account number
            if (!ValidationUtility.IsValidBankAccountNumber(customerDto.BankAccountNumber))
            {
                throw new ArgumentException("Invalid bank account number");
            }

            // Update the customer properties
            customer.Firstname = customerDto.Firstname;
            customer.Lastname = customerDto.Lastname;
            customer.DateOfBirth = customerDto.DateOfBirth;
            customer.PhoneNumber = customerDto.PhoneNumber;
            customer.Email = customerDto.Email;
            customer.BankAccountNumber = customerDto.BankAccountNumber;

            // Save the changes
            await _customerRepository.UpdateAsync(customer);
        }

    }
}
