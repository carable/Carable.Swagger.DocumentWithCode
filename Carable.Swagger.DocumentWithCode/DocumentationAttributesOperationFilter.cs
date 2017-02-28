using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Carable.Swagger.DocumentWithCode
{
    public class DocumentationAttributesOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var attribute = context.ApiDescription.ActionAttributes()
                .OfType<BaseDocumentOperation>()
                .FirstOrDefault();

            attribute?.Apply(operation, context.SchemaRegistry);
        }
    }
}