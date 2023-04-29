using Mc2.CrudTest.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerDTO> GetCustomerById(int id);
        Task<IEnumerable<CustomerDTO>> GetAllCustomers();
        Task AddCustomer(CustomerDTO customerDto);
        Task UpdateCustomer(int id, CustomerDTO customerDto);
        Task DeleteCustomer(int id);

    }
}
