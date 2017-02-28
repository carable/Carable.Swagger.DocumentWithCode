using System;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Carable.Swagger.DocumentWithCode
{
    public abstract class BaseDocumentOperation : Attribute
    {
        // ReSharper disable UnusedParameter.Local
        public abstract void Apply(Operation operation, ISchemaRegistry schemaRegistry);
    }
}