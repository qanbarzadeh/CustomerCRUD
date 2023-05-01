using AutoMapper;
using Mc2.CrudTest.Application.Commands;
using Mc2.CrudTest.Application.Interfaces;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Shared.Utilities;

namespace Mc2.CrudTest.Application.CommandHandlers
{
    public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(IMapper mapper, ICustomerRepository customerRepository)
        {
            _mapper = mapper;
            _customerRepository = customerRepository;
        }

        public async Task Handle(CreateCustomerCommand request)
        {
            if (request.Customer == null)
            {
                throw new ArgumentNullException(nameof(request.Customer));
            }

            // Validate the phone number
            if (!ValidationUtility.IsValidPhoneNumber(request.Customer.PhoneNumber))
            {
                throw new ArgumentException("Invalid phone number");
            }

            // Validate the email
            if (!ValidationUtility.IsValidEmail(request.Customer.Email))
            {
                throw new ArgumentException("Invalid email");
            }

            // Check if the email is unique
            if (!await _customerRepository.IsEmailUniqueAsync(request.Customer.Email))
            {
                throw new ArgumentException("Email already exists");
            }

            // Validate the bank account number
            if (!ValidationUtility.IsValidBankAccountNumber(request.Customer.BankAccountNumber))
            {
                throw new ArgumentException("Invalid bank account number");
            }

            var customer = _mapper.Map<Customer>(request.Customer);
            await _customerRepository.AddAsync(customer);
        }
    }
}
