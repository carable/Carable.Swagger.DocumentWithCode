# Swagger Document With Code

This works with @domaindrivendev [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)

## This is a fork of Document Code With Code

If your philosophy about documentation align with that view, then use [DalSoft.Swagger.DocumentCodeWithCode](https://github.com/DalSoft/DalSoft.Swagger.DocumentCodeWithCode) instead.

## Why?

The code here is to embellish your Web API Swagger documentation using code via attributes:
* You can provide detailed and contextual request or response examples
* You can workaround providing multiple request or response examples (https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/228
and https://github.com/domaindrivendev/Swashbuckle/issues/397)

## Getting Started 
Install the [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) package and configure as normal.

Install the Carable.Swagger.DocumentWithCode package using NuGet.
```dos
PM> Install-Package Carable.Swagger.DocumentWithCode
```
Modify startup.cs to include the OperationFilter.
```cs
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddSwaggerGen(options =>
    {
        ...
        options.OperationFilter<DocumentationAttributesOperationFilter>();
    });
}
```

## Providing Documenation using Code

Inherit the BaseDocumentOperation. Now you can you document by setting the properties.
```cs
public class AddPetAttribute : BaseDocumentOperationAttribute
{
    public override void Apply(Operation operation, ISchemaRegistry schemaRegistry) 
    {
        operation.Summary = "Add a new pet to the store";
        operation.Description = "Pet object that needs to be added to the store";
    }
}
```

## Providing Examples using Code
Use the the extension AddExampleToParameter and AddOrUpdate to provide request and response examples.

```cs
public class AddPetAttribute : BaseDocumentOperationAttribute
{
    public override void Apply(Operation operation, ISchemaRegistry schemaRegistry)
    {
    	operation.Parameters.Single(p=>p.Name=="pet").AddExampleToParameter(schemaRegistry, operation.OperationId, 
        	new Pet { Id = 1, Name = "doggie", Status = "available" });
        
        var exampleResponse = new Pet[] { new Pet { Id = 1, Name = "doggie", Status = "available" } };        
        operation.Responses.AddOrUpdate(schemaRegistry, operation.OperationId, "200", new Response
        {
            Description = "success"
        }, example:exampleResponse);
    }
}
```

## Providing Multiple Example using Code
Use the allowMultipleExamples parameter for AddExampleToParameter and AddOrUpdate to provide multiple examples. Multiple examples are useful for shared resources such as errors.
```cs
public class AddPetAttribute : BaseDocumentOperationAttribute
{
    public override void Apply(Operation operation, ISchemaRegistry schemaRegistry)
    {
        var loginFailed = new Error { Id="loginFailed"  Description = "Login has Failed" };
            
        operation.Responses.AddOrUpdate(schemaRegistry, operation.OperationId, "401", 
        new Response { Description = "Login Failed" }, example:loginFailed, allowMultipleExamples:true);
        
        var validationFailed = new Error { Id="validationFailed"  Description = "Validation Failed" };
        
        operation.Responses.AddOrUpdate(schemaRegistry, operation.OperationId, "400",
        new Response { Description = "Validation Failed"}, example:validationFailed, allowMultipleExamples:true);
    }
}
```
