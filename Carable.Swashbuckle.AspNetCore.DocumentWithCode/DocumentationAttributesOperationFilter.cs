using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Carable.Swashbuckle.AspNetCore.DocumentWithCode
{
    public class DocumentationAttributesOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var attributes = context.ApiDescription.ActionAttributes()
                .OfType<BaseDocumentOperationAttribute>()
                .ToList();
            foreach (var attribute in attributes)
            {
                attribute.Apply(operation, context.SchemaRegistry);
            }
        }
    }
}