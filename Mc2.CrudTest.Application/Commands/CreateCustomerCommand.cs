using Mc2.CrudTest.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Application.Commands
{
    public class CreateCustomerCommand 
    {
        public CustomerDTO Customer { get; set; }

        public CreateCustomerCommand(CustomerDTO customer)
        {
            Customer = customer;
        }
    }
}
