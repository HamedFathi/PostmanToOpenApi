using Microsoft.OpenApi.Models;
using PostmanCollectionReader;
using System;

namespace PostmanToOpenApi
{
    public static class PostmanToOpenApiExtensions
    {
        public static OpenApiDocument ToOpenApiDocument(this PostmanCollection postmanCollection, Setting setting = null)
        {
            if (postmanCollection is null)
            {
                throw new ArgumentNullException(nameof(postmanCollection));
            }
            setting = setting ?? new Setting();
            var openApiDocument = new OpenApiDocument();

            openApiDocument.Info = postmanCollection.Info.ToOpenApiInfo(setting);




            return null;
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
