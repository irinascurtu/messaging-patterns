using Microsoft.AspNetCore.Mvc.Formatters;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductsApi.Models;
using System.Buffers;
using System.Net.Http.Headers;
using System.Text;

namespace ProductsApi.Infrastructure
{
    public class TrimmedOutputFormatter : OutputFormatter
    {

        public TrimmedOutputFormatter()
        {
            SupportedMediaTypes.Add("application/vnd.speaker.trimmed");
        }

        protected override bool CanWriteType(Type? type)
      => typeof(ProductTrimmedModel).IsAssignableFrom(type)
          || typeof(IEnumerable<ProductTrimmedModel>).IsAssignableFrom(type);



        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;
            var bufferWriter = context.WriterFactory(response.Body, Encoding.UTF8);

            var json = JsonConvert.SerializeObject(context.Object, Formatting.None);
            await bufferWriter.WriteAsync((json));
            await bufferWriter.FlushAsync();
        }
    }
}
