using Microsoft.OpenApi.Models;
using PostmanCollectionReader;
using System;

namespace PostmanToOpenApi
{
    public static class PostmanToOpenApiExtensions
    {
        public static OpenApiDocument ToOpenApiDocument(this string postmanCollectionJson, Setting setting = null)
        {
            if (string.IsNullOrWhiteSpace(postmanCollectionJson))
            {
                throw new ArgumentException($"'{nameof(postmanCollectionJson)}' cannot be null or whitespace.", nameof(postmanCollectionJson));
            }
            var postmanCollection = PostmanCollection.FromJson(postmanCollectionJson);
            return postmanCollection.ToOpenApiDocument(setting);
        }
        public static OpenApiDocument ToOpenApiDocument(this PostmanCollection postmanCollection, Setting setting = null)
        {
            if (postmanCollection is null)
            {
                throw new ArgumentNullException(nameof(postmanCollection));
            }
            setting = setting ?? new Setting();
            var openApiDocument = new OpenApiDocument();

            openApiDocument.Info = postmanCollection.Info.ToOpenApiInfo(setting);




            return openApiDocument;
        }
        public static OpenApiInfo ToOpenApiInfo(this Information information, Setting setting)
        {
            if (information is null)
            {
                throw new ArgumentNullException(nameof(information));
            }

            if (setting is null)
            {
                throw new ArgumentNullException(nameof(setting));
            }

            var info = new OpenApiInfo
            {
                Description = information.Description.HasValue
                    ? information.Description.Value.String
                    : null,
                Version = information.Version.HasValue
                    ? information.Version.Value.String
                    : setting.DefaultVersion
            };

            return info;
        }
    }
}
