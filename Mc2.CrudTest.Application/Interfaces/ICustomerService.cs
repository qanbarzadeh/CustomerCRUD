using Mc2.CrudTest.Application.Commands;
using Mc2.CrudTest.Application.DTO;
using Mc2.CrudTest.Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerDTO> Handle(GetCustomerByIdQuery query);
        Task<IEnumerable<CustomerDTO>> Handle(GetAllCustomersQuery query);
        Task Handle(CreateCustomerCommand command);
        Task Handle(UpdateCustomerCommand command);
        Task Handle(DeleteCustomerCommand command);
    }
}
