using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Carable.Swashbuckle.AspNetCore.DocumentWithCode
{
    public static class Extensions
    {
        public static void AddOrUpdate(this IDictionary<string, Response> responseDictionary, ISchemaRegistry schemaRegistry, string operationId, string statusCode, Response response, object example = null)
        {
            if (responseDictionary.ContainsKey(statusCode))
                responseDictionary[statusCode] = response;
            else
                responseDictionary.Add(statusCode, response);

            if (example != null)
                responseDictionary[statusCode].Schema = schemaRegistry.AddExampleToSchemaDefinitions(operationId, example, statusCode);
        }

        public static void AddExampleToParameter(this IParameter parameter, ISchemaRegistry schemaRegistry, string operationId, object example)
        {
            if (example == null) return;

            var bodyParameter = parameter as BodyParameter;
            if (bodyParameter != null)
            {
                bodyParameter.Schema = schemaRegistry.AddExampleToSchemaDefinitions(operationId,
                    example);
            }
            else
            {
                throw new NotImplementedException($"Unknown type of parameter! {parameter.GetType().Name}");
            }
        }

        private static Schema AddExampleToSchemaDefinitions(this ISchemaRegistry schemaRegistry, string operationId, object example, string statusCode = null)
        {
            var type = example.GetType();
            schemaRegistry.GetOrRegister(type);

            var actualTypeName = type.Name.Replace("[]", string.Empty);
            string exampleFakeTypeName = actualTypeName;

            if (!schemaRegistry.Definitions.ContainsKey(actualTypeName))
            {
                throw new Exception($"Could not find type name! {actualTypeName}");
            }
            else 
            {
                schemaRegistry.Definitions[actualTypeName].Example = example;
            }

            return schemaRegistry.Definitions[exampleFakeTypeName];
        }

        /// <summary>
        /// Try to include the assembly xml documentation.
        /// </summary>
        /// <param name="options">swagger gen options</param>
        /// <param name="assembly">The assembly with documenation</param>
        /// <returns>true if the documentation file was found</returns>
        public static bool TryIncludeXmlFromAssembly(this SwaggerGenOptions options, Assembly assembly)
        {
            var path = Path.GetDirectoryName(assembly.Location);
            var file = Path.Combine(path,
                Path.GetFileNameWithoutExtension(assembly.Location) + ".xml");
            if (File.Exists(file))
            {
                options.IncludeXmlComments(file);
                return true;
            }
            return false;
        }
    }
}
