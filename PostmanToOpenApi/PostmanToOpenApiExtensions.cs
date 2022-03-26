using Microsoft.OpenApi.Models;
using PostmanCollectionReader;
using System;

namespace PostmanToOpenApi
{
    public static class PostmanToOpenApiExtensions
    {
        public static OpenApiDocument ConvertTo(this PostmanCollection postmanCollection)
        {
            if (postmanCollection is null)
            {
                throw new ArgumentNullException(nameof(postmanCollection));
            }

            var openApiDocument = new OpenApiDocument();


            return null;
        }
    }
}
