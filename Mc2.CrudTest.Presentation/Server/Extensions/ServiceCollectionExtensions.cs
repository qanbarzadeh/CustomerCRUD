using Mc2.CrudTest.Application.CommandHandlers;
using Mc2.CrudTest.Application.Commands;
using Mc2.CrudTest.Application.DTO;
using Mc2.CrudTest.Application.Interfaces;
using Mc2.CrudTest.Application.Queries;
using Mc2.CrudTest.Application.QueryHandlers;
using Mc2.CrudTest.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;

namespace Mc2.CrudTest.Presentation.Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICommandHandler<CreateCustomerCommand>, CreateCustomerCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateCustomerCommand>, UpdateCustomerCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteCustomerCommand>, DeleteCustomerCommandHandler>();
            services.AddScoped<IQueryHandler<GetCustomerByIdQuery, CustomerDTO>, GetCustomerByIdQueryHandler>();
            services.AddScoped<IQueryHandler<GetAllCustomersQuery, IEnumerable<CustomerDTO>>, GetAllCustomersQueryHandler>();

            return services;
        }
    }
}