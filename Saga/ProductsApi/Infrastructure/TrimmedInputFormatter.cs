namespace ProductsApi.Infrastructure
{
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Extensions.Primitives;
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public class TrimmedInputFormatter : InputFormatter
    {
        public TrimmedInputFormatter()
        {
            // Add the supported media type
            SupportedMediaTypes.Add("application/vnd.speaker.trimmed");

        }

        protected override bool CanReadType(Type type)
        {
            // Specify which types the formatter can read
            return type == typeof(string);
            return true;
        }


        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;
            using (var reader = new StreamReader(request.Body, Encoding.UTF8))
            {
                var content = await reader.ReadToEndAsync();

                // Process the content as needed (e.g., trim whitespace)
                var trimmedContent = content.Trim();

                return await InputFormatterResult.SuccessAsync(trimmedContent);
            }
        }
    }
}

