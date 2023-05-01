using AutoMapper;
using Mc2.CrudTest.Application.DTO;
using Mc2.CrudTest.Application.Interfaces;
using Mc2.CrudTest.Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Application.QueryHandlers
{
    public class GetCustomerByIdQueryHandler : IQueryHandler<GetCustomerByIdQuery, CustomerDTO>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetCustomerByIdQueryHandler(IMapper mapper, ICustomerRepository customerRepository)
        {
            _mapper = mapper;
            _customerRepository = customerRepository;
        }

        public async Task<CustomerDTO> Handle(GetCustomerByIdQuery query)
        {
            var customer = await _customerRepository.GetByIdAsync(query.Id);

            if (customer == null)
            {
                return null;
            }

            var customerDto = _mapper.Map<CustomerDTO>(customer);

            return customerDto;
        }
    }

}
