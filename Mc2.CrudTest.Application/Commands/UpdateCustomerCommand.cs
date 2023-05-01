using Mc2.CrudTest.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Application.Commands
{
    public class UpdateCustomerCommand
    {
        public int Id { get; set; }
        public CustomerDTO Customer { get; set; }

        public UpdateCustomerCommand(int id, CustomerDTO customer)
        {
            Id = id;
            Customer = customer;
        }
    }
}
