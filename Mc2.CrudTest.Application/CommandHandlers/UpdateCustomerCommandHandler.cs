using AutoMapper;
using Mc2.CrudTest.Application.Commands;
using Mc2.CrudTest.Application.Interfaces;
using Mc2.CrudTest.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Application.CommandHandlers
{
    public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public UpdateCustomerCommandHandler(IMapper mapper, ICustomerRepository customerRepository)
        {
            _mapper = mapper;
            _customerRepository = customerRepository;
        }

        public async Task Handle(UpdateCustomerCommand request)
        {
            if (request.Customer == null)
            {
                throw new ArgumentNullException(nameof(request.Customer));
            }

            var customer = await _customerRepository.GetByIdAsync(request.Id);
            if (customer == null)
            {
                throw new ArgumentException($"Customer with ID {request.Id} not found");
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

            // Validate the bank account number
            if (!ValidationUtility.IsValidBankAccountNumber(request.Customer.BankAccountNumber))
            {
                throw new ArgumentException("Invalid bank account number");
            }

            // Update the customer properties
            customer.FirstName = request.Customer.FirstName;
            customer.LastName = request.Customer.LastName;
            customer.DateOfBirth = request.Customer.DateOfBirth;
            customer.PhoneNumber = request.Customer.PhoneNumber;
            customer.Email = request.Customer.Email;
            customer.BankAccountNumber = request.Customer.BankAccountNumber;

            // Save the changes
            await _customerRepository.UpdateAsync(customer);
        }
    }

}