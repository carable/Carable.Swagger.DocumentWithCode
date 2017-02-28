using Carable.Swagger.DocumentWithCode;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SampleWebApi.Models.Examples
{
    public class ExampleCustomerAttribute:BaseDocumentOperation
    {
        public override void Apply(Operation operation, ISchemaRegistry schemaRegistry)
        {
            var exampleResponse = new Customer("Eric Ericsson");
            operation.Responses.AddOrUpdate(schemaRegistry, operation.OperationId, "200", new Response
               {
                    Description = "Successfully finding a customer"
               }, example: exampleResponse);
        }
    }
}