using System;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Carable.Swashbuckle.AspNetCore.DocumentWithCode
{
    public abstract class BaseDocumentOperationAttribute : Attribute
    {
        public abstract void Apply(Operation operation, ISchemaRegistry schemaRegistry);
    }
}