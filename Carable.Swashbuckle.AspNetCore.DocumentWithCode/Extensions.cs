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

            if (example!=null)
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

        //TODO: https://github.com/domaindrivendev/Ahoy/issues/234
        //public static void AddDefaultToParameter(this BodyParameter parameter, ISchemaRegistry schemaRegistry, string operationId, object @default)
        //{
        //    if (@default != null)
        //        parameter.Schema = schemaRegistry.AddDefaultToSchemaDefinitions(operationId, @default);
        //}
        
        private static Schema AddExampleToSchemaDefinitions(this ISchemaRegistry schemaRegistry, string operationId, object example, string statusCode=null)
        {
            var type = example.GetType();
            schemaRegistry.GetOrRegister(type);

            var actualTypeName = type.Name.Replace("[]", string.Empty);
            var schema = schemaRegistry.Definitions[actualTypeName];
            string exampleFakeTypeName=actualTypeName;
            while (schemaRegistry.Definitions.ContainsKey(exampleFakeTypeName))
            {
                exampleFakeTypeName += "'";// we do not want long type names, rather just append some ' to have short names
            }

            //Why? https://github.com/domaindrivendev/Ahoy/issues/228 and https://github.com/domaindrivendev/Swashbuckle/issues/397
            schemaRegistry.Definitions.Add(exampleFakeTypeName, new Schema
            {
                Ref = "#/definitions/" + exampleFakeTypeName,
                Example = example,
                Properties = schema?.Properties,
                Title = schema?.Title,
                Description = schema?.Description,
                Discriminator = schema?.Discriminator,
                Format = schema?.Format,
                Type = schema?.Type,
                AdditionalProperties = schema?.AdditionalProperties,
                ExternalDocs = schema?.ExternalDocs,
                Xml = schema?.Xml,
            });

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
