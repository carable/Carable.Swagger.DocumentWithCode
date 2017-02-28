using System.Linq;
using Carable.Swagger.DocumentWithCode;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SampleWebApi.Models.Examples
{
    public class ExampleInputCustomerAttribute:BaseDocumentOperationAttribute
    {
        public override void Apply(Operation operation, ISchemaRegistry schemaRegistry)
        {
            var exampleInput = new Customer("Eric Ericsson");
            operation.Parameters.Single(p=>p.Name=="customer").AddExampleToParameter(schemaRegistry,
                operation.OperationId,
                example: exampleInput
            );
        }
    }
}