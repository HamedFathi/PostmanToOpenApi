using Microsoft.OpenApi.Models;
using PostmanCollectionReader;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var openApiDocument = new OpenApiDocument
            {
                Info = postmanCollection.Info.ToOpenApiInfo(setting),
                Servers = postmanCollection.ToOpenApiServers(setting)
            };



            return openApiDocument;
        }
        private static OpenApiInfo ToOpenApiInfo(this Information information, Setting setting)
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
                    : setting.DefaultVersion,
                Title = information.Name
            };

            return info;
        }
        private static IList<OpenApiServer> ToOpenApiServers(this PostmanCollection postmanCollection, Setting setting)
        {
            if (postmanCollection is null)
            {
                throw new ArgumentNullException(nameof(postmanCollection));
            }

            if (setting is null)
            {
                throw new ArgumentNullException(nameof(setting));
            }

            var result = new List<OpenApiServer>();
            var uris = new HashSet<string>();

            var requests = postmanCollection.Item
                .Where(x => x.Request.HasValue)
                .Select(x => x.Request.Value)
                .Select(x => x.RequestClass.Url)
                .Where(x => x.HasValue)
                .Select(x => x.Value.UrlClass.Raw)
                .Distinct()
                .ToList()
                ;

            var responses = postmanCollection.Item
                .Where(x => x.Response.Any())
                .SelectMany(x => x.Response)
                .Where(x => x.ResponseClass.OriginalRequest.HasValue)
                .Where(x => x.ResponseClass.OriginalRequest.Value.RequestClass.Url.HasValue)
                .Select(x => x.ResponseClass.OriginalRequest.Value.RequestClass.Url.Value.UrlClass.Raw)
                ;

            var list = requests.Concat(responses);
            foreach (var item in list)
            {
                var uri = item.GetHostWithScheme();
                var status = uris.Add(uri);
                if (status)
                {
                    result.Add(new OpenApiServer
                    {
                        Url = uri
                    });
                }
            }

            return result;
        }
        private static OpenApiRequestBody ToOpenApiRequestBody(this RequestUnion requestUnion, Setting setting)
        {
            if (setting is null)
            {
                throw new ArgumentNullException(nameof(setting));
            }

            return null;
        }
        private static OpenApiResponses ToOpenApiResponses(this IList<Response> responses, Setting setting)
        {
            if (responses is null)
            {
                throw new ArgumentNullException(nameof(responses));
            }

            if (setting is null)
            {
                throw new ArgumentNullException(nameof(setting));
            }

            return null;
        }
        private static string GetHostWithScheme(this Uri uri)
        {
            if (uri is null)
            {
                throw new ArgumentNullException(nameof(uri));
            }
            var path = uri.Scheme + Uri.SchemeDelimiter + uri.Host;
            return path;
        }
        private static string GetHostWithScheme(this string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentException($"'{nameof(uri)}' cannot be null or whitespace.", nameof(uri));
            }
            var uriInfo = new Uri(uri);
            return uriInfo.GetHostWithScheme();
        }
    }
}
