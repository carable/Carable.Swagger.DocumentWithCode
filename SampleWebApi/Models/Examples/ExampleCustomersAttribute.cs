using Carable.Swagger.DocumentWithCode;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SampleWebApi.Models.Examples
{
    public class ExampleCustomersAttribute:BaseDocumentOperation
    {
        public override void Apply(Operation operation, ISchemaRegistry schemaRegistry)
        {
            var exampleResponse = new []{new Customer("Eric Ericsson"),new Customer("Johan Johansson")};
            operation.Responses.AddOrUpdate(schemaRegistry, operation.OperationId, "200", new Response
            {
                Description = "Successfully getting a list of customers"
            }, example: exampleResponse);
        }
    }
}