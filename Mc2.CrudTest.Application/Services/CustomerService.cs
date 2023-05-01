using AutoMapper;
using Mc2.CrudTest.Application.CommandHandlers;
using Mc2.CrudTest.Application.Commands;
using Mc2.CrudTest.Application.DTO;
using Mc2.CrudTest.Application.Interfaces;
using Mc2.CrudTest.Application.Queries;
using Mc2.CrudTest.Application.QueryHandlers;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Shared.Utilities;

namespace Mc2.CrudTest.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly ICommandHandler<CreateCustomerCommand> _createCustomerCommandHandler;
        private readonly ICommandHandler<UpdateCustomerCommand> _updateCustomerCommandHandler;
        private readonly ICommandHandler<DeleteCustomerCommand> _deleteCustomerCommandHandler;
        private readonly IQueryHandler<GetCustomerByIdQuery, CustomerDTO> _getCustomerByIdQueryHandler;
        private readonly IQueryHandler<GetAllCustomersQuery, IEnumerable<CustomerDTO>> _getAllCustomersQueryHandler;

        public CustomerService(
            IMapper mapper,
            ICustomerRepository customerRepository,
            ICommandHandler<CreateCustomerCommand> createCustomerCommandHandler,
            ICommandHandler<UpdateCustomerCommand> updateCustomerCommandHandler,
            ICommandHandler<DeleteCustomerCommand> deleteCustomerCommandHandler,
            IQueryHandler<GetCustomerByIdQuery, CustomerDTO> getCustomerByIdQueryHandler,
            IQueryHandler<GetAllCustomersQuery, IEnumerable<CustomerDTO>> getAllCustomersQueryHandler)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _createCustomerCommandHandler = createCustomerCommandHandler;
            _updateCustomerCommandHandler = updateCustomerCommandHandler;
            _deleteCustomerCommandHandler = deleteCustomerCommandHandler;
            _getCustomerByIdQueryHandler = getCustomerByIdQueryHandler;
            _getAllCustomersQueryHandler = getAllCustomersQueryHandler;
        }

        public async Task<CustomerDTO> Handle(GetCustomerByIdQuery query)
        {
            return await _getCustomerByIdQueryHandler.Handle(query);
        }

        public async Task<IEnumerable<CustomerDTO>> Handle(GetAllCustomersQuery query)
        {
            return await _getAllCustomersQueryHandler.Handle(query);
        }

        public async Task Handle(CreateCustomerCommand command)
        {
            await _createCustomerCommandHandler.Handle(command);
        }

        public async Task Handle(UpdateCustomerCommand command)
        {
            await _updateCustomerCommandHandler.Handle(command);
        }

        public async Task Handle(DeleteCustomerCommand command)
        {
            await _deleteCustomerCommandHandler.Handle(command);
        }
    }

}
