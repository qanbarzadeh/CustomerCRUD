using Mc2.CrudTest.Application.DTO;
using Mc2.CrudTest.Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Application.Interfaces
{
    public interface IQueryHandler<TQuery, TResult>
    {
        Task<TResult> Handle(TQuery query);

    }

    public interface IGetAllCustomersQueryHandler : IQueryHandler<GetAllCustomersQuery, IEnumerable<CustomerDTO>>
    {
    }
}
