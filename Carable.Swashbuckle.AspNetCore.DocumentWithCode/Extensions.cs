using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Carable.Swashbuckle.AspNetCore.DocumentWithCode
{
    public static class Extensions
    {
        public static void AddOrUpdate(this IDictionary<string, Response> responseDictionary, ISchemaRegistry schemaRegistry, string operationId, string statusCode, Response response, object example = null, bool allowMultipleExamples = false)
        {
            if (responseDictionary.ContainsKey(statusCode))
                responseDictionary[statusCode] = response;
            else
                responseDictionary.Add(statusCode, response);

            if (example!=null)
                responseDictionary[statusCode].Schema = schemaRegistry.AddExampleToSchemaDefinitions(operationId, example, statusCode, allowMultipleExamples);
        }

        public static void AddExampleToParameter(this IParameter parameter, ISchemaRegistry schemaRegistry, string operationId, object example, bool allowMultipleExamples = false)
        {
            if (example == null) return;

            var bodyParameter = parameter as BodyParameter;
            if (bodyParameter != null)
            {
                bodyParameter.Schema = schemaRegistry.AddExampleToSchemaDefinitions(operationId,
                    example, allowMultipleExamples: allowMultipleExamples);
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
        
        private static Schema AddExampleToSchemaDefinitions(this ISchemaRegistry schemaRegistry, string operationId, object example, string statusCode=null, bool allowMultipleExamples=false)
        {
            var type = example.GetType();
            schemaRegistry.GetOrRegister(type);

            var actualTypeName = type.Name.Replace("[]", string.Empty);
            var schema = schemaRegistry.Definitions[actualTypeName];
            if (!allowMultipleExamples)
            {
                schema.Example = example;
                return schema;
            }

            string exampleFakeTypeName;
                
            if (statusCode==null)
               exampleFakeTypeName = "examples<=" + actualTypeName + "<=" + operationId;
            else
               exampleFakeTypeName = "examples=>" + operationId + "=>" + statusCode + "=>" + actualTypeName;

            //Why? https://github.com/domaindrivendev/Ahoy/issues/228 and https://github.com/domaindrivendev/Swashbuckle/issues/397
            schemaRegistry.Definitions.Add(exampleFakeTypeName, new Schema
            {
                Ref = "#/definitions/" + exampleFakeTypeName,
                Example = example,
                Properties = schema?.Properties
            });

            return schemaRegistry.Definitions[exampleFakeTypeName];
        }


    }
}
