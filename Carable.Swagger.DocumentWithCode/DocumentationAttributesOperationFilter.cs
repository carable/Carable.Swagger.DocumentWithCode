using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Carable.Swagger.DocumentWithCode
{
    public class DocumentationAttributesOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var attributes = context.ApiDescription.ActionAttributes()
                .OfType<BaseDocumentOperation>()
                .ToList();
            foreach (var attribute in attributes)
            {
                attribute.Apply(operation, context.SchemaRegistry);
            }
        }
    }
}