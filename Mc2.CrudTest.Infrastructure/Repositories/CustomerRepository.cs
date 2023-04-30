using Mc2.CrudTest.Application.Repositories;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public async  Task AddAsync(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            try
            {
                await _context.Customers.AddAsync(customer);
            }
            catch (Exception ex)
            {
               
                throw new InvalidOperationException("An error occurred while adding the customer.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var customer = new Customer { Id = id };

            // Attach the customer to the context with the EntityState as 'Deleted'
            _context.Entry(customer).State = EntityState.Deleted;

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Check if the customer exists
                var existingCustomer = await _context.Customers.FindAsync(id);

                if (existingCustomer == null)
                {
                    throw new ArgumentException($"Customer with id {id} not found");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            try
            {
                var customers = await _context.Customers.ToListAsync();
                return customers;
            }
            catch (Exception ex)
            {
                //log exception here 
                return Enumerable.Empty<Customer>();
            }
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null)
                {
                    throw new InvalidOperationException($"Customer with ID {id} not found");
                }
                return customer;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, $"Error in {nameof(GetByIdAsync)} method of {nameof(CustomerRepository)}");
                throw;
            }
        }

        public async Task UpdateAsync(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            try
            {
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
             
                throw new InvalidOperationException($"Could not update customer with ID {customer.Id}", ex);
            }
            catch (Exception ex)
            {
             
                throw ex;
            }
        }
    }
}
