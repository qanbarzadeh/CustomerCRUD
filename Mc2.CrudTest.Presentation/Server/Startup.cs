using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using Mc2.CrudTest.Application.Mapping;
using Mc2.CrudTest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Mc2.CrudTest.Infrastructure.Repositories;
using Mc2.CrudTest.Application.Interfaces;
using Mc2.CrudTest.Application.Services;
using Mc2.CrudTest.Presentation.Server.Extensions;
using Mc2.CrudTest.Application.CommandHandlers;
using Mc2.CrudTest.Application.Commands;
using Mc2.CrudTest.Application.DTO;
using Mc2.CrudTest.Application.Queries;
using Mc2.CrudTest.Application.QueryHandlers;
using System.Collections.Generic;

namespace Mc2.CrudTest.Presentation.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("MyInMemoryDatabase"));

            services.AddScoped<ICustomerRepository, CustomerRepository>();

            services.AddScoped<ICommandHandler<CreateCustomerCommand>, CreateCustomerCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateCustomerCommand>, UpdateCustomerCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteCustomerCommand>, DeleteCustomerCommandHandler>();

            services.AddScoped<ICustomerService, CustomerService>();

            services.AddScoped<IQueryHandler<GetCustomerByIdQuery, CustomerDTO>, GetCustomerByIdQueryHandler>();
            services.AddScoped<IQueryHandler<GetAllCustomersQuery, IEnumerable<CustomerDTO>>, GetAllCustomersQueryHandler>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CustomerCRUD", Version = "v1" });
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = "swagger";
                });
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }




    }
}
