using Mc2.CrudTest.Application.DTO;
using Mc2.CrudTest.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Linq;
using Mc2.CrudTest.Application.Commands;
using Mc2.CrudTest.Application.Queries;
using System.Collections.Generic;

namespace Mc2.CrudTest.Presentation.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICommandHandler<CreateCustomerCommand> _createCustomerCommandHandler;
        private readonly ICommandHandler<UpdateCustomerCommand> _updateCustomerCommandHandler;
        private readonly ICommandHandler<DeleteCustomerCommand> _deleteCustomerCommandHandler;
        private readonly IQueryHandler<GetCustomerByIdQuery, CustomerDTO> _getCustomerByIdQueryHandler;
        private readonly IQueryHandler<GetAllCustomersQuery, IEnumerable<CustomerDTO>> _getAllCustomersQueryHandler;

        public CustomersController(
            ICommandHandler<CreateCustomerCommand> createCustomerCommandHandler,
            ICommandHandler<UpdateCustomerCommand> updateCustomerCommandHandler,
            ICommandHandler<DeleteCustomerCommand> deleteCustomerCommandHandler,
            IQueryHandler<GetCustomerByIdQuery, CustomerDTO> getCustomerByIdQueryHandler,
            IQueryHandler<GetAllCustomersQuery, IEnumerable<CustomerDTO>> getAllCustomersQueryHandler)
        {
            _createCustomerCommandHandler = createCustomerCommandHandler;
            _updateCustomerCommandHandler = updateCustomerCommandHandler;
            _deleteCustomerCommandHandler = deleteCustomerCommandHandler;
            _getCustomerByIdQueryHandler = getCustomerByIdQueryHandler;
            _getAllCustomersQueryHandler = getAllCustomersQueryHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllCustomersQuery();
            var customers = await _getAllCustomersQueryHandler.Handle(query);

            if (customers == null || !customers.Any())
            {
                return NoContent();
            }

            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetCustomerByIdQuery(id);
            var customer = await _getCustomerByIdQueryHandler.Handle(query);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerDTO customerDto)
        {
            var command = new CreateCustomerCommand(customerDto);
            await _createCustomerCommandHandler.Handle(command);

            return CreatedAtAction(nameof(Get), new { id = customerDto.Id }, customerDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerDTO customerDto)
        {
            var command = new UpdateCustomerCommand(id, customerDto);
            await _updateCustomerCommandHandler.Handle(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteCustomerCommand(id);
            await _deleteCustomerCommandHandler.Handle(command);

            return NoContent();
        }
    }

}
