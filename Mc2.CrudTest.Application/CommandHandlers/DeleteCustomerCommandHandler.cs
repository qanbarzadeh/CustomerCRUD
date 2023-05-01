using Mc2.CrudTest.Application.Commands;
using Mc2.CrudTest.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Application.CommandHandlers
{
    public class DeleteCustomerCommandHandler : ICommandHandler<DeleteCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;

        public DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task Handle(DeleteCustomerCommand request)
        {
            await _customerRepository.DeleteAsync(request.Id);
        }
    }

}
