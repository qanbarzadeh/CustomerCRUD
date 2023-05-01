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
    public interface IGetAllCustomersQueryHandler : IQueryHandler<GetAllCustomersQuery, IEnumerable<CustomerDTO>>
    {
    }

    public class GetAllCustomersQueryHandler : IGetAllCustomersQueryHandler
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetAllCustomersQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerDTO>> Handle(GetAllCustomersQuery query)
        {
            var customers = await _customerRepository.GetAllAsync();

            if (customers == null)
            {
                throw new Exception("No customers found");
            }

            var customerDtos = _mapper.Map<IEnumerable<CustomerDTO>>(customers);

            return customerDtos;
        }
    }
}
