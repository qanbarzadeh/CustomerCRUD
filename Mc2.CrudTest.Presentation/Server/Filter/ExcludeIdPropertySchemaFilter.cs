using Mc2.CrudTest.Domain.Entities;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Mc2.CrudTest.Presentation.Server.Filter
{

    public class ExcludeIdPropertySchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(Customer))
            {
                var propertyToRemove = schema.Properties.Keys.FirstOrDefault(x => x.ToLower() == "id");
                if (propertyToRemove != null)
                {
                    schema.Properties.Remove(propertyToRemove);
                }
            }
        }

    }
    }

