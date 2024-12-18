using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.RegularExpressions;
using DmatAccountApi.Models;

namespace DmatAccountApi.Middleware
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/dmat/create") &&
                context.Request.Method == "POST")
            {
                var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var request = JsonSerializer.Deserialize<DmatAccountRequest>(body, options);

                var errors = ValidateRequest(request);

                if (errors.Count > 0)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json";

                    var errorResponse = new
                    {
                        RequestTime = DateTime.Now,
                        Status = "Failed",
                        Errors = errors
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                    
                    return;
                }

                // Reset the request body for the next middleware
                var requestBody = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(body));
                context.Request.Body = requestBody;
                context.Request.Body.Seek(0, SeekOrigin.Begin);
            }

            await _next(context);
        }

        private List<string?> ValidateRequest(DmatAccountRequest request)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(request);
            Validator.TryValidateObject(request, context, validationResults, true);

            var additionalErrors = validationResults.Select(vr => vr.ErrorMessage).ToList();

            // Custom validations
            if (request.SecCode.Length != 4)
            {
                additionalErrors.Add("SecCode must be exactly 4 alphanumeric characters.");
            }
            if (!Regex.IsMatch(request.SecCode, "^[a-zA-Z0-9]+$"))
            {
                additionalErrors.Add("SecCode must not contain special characters, spaces, or negative values.");
            }
            if (!request.SecCode.Any(char.IsLetter) || !request.SecCode.Any(char.IsDigit))
            {
                additionalErrors.Add("SecCode must contain both letters and numbers.");
            }

            if (request.RowCount <= 0)
            {
                additionalErrors.Add("RowCount must be greater than 0 and it must be a positive integer.");
            }
            if (!Regex.IsMatch(request.RowCount.ToString(), @"^\d+$"))
            {
                additionalErrors.Add("RowCount must contain only numeric digits (no special characters, letters, or spaces).");
            }
            if (request.RowCount >= int.MaxValue)
            {
                additionalErrors.Add("RowCount exceeds the maximum value for an int.");
            }

            if (request.PageIndex <= 0)
            {
                additionalErrors.Add("PageIndex must be greater than 0 and it must be a positive integer.");
            }
            if (!Regex.IsMatch(request.PageIndex.ToString(), @"^\d+$"))
            {
                additionalErrors.Add("PageIndex must contain only numeric digits (no special characters, letters, or spaces).");
            }
            if (request.PageIndex >= int.MaxValue)
            {
                additionalErrors.Add("PageIndex exceeds the maximum value for an int.");
            }

            if (request.DtDate.HasValue && request.DtDate.Value > DateTime.UtcNow)
            {
                additionalErrors.Add("DtDate cannot be a future date.");
            }

            return additionalErrors;
        }
    }
}



// using System.ComponentModel.DataAnnotations;
// using System.Text.Json;
// using System.Text.RegularExpressions;
// using DmatAccountApi.Models;

// namespace DmatAccountApi.Middleware
// {
//     public class ValidationMiddleware
//     {
//         private readonly RequestDelegate _next;

//         public ValidationMiddleware(RequestDelegate next)
//         {
//             _next = next;
//         }

//         public async Task InvokeAsync(HttpContext context)
//         {
//             if (context.Request.Path.StartsWithSegments("/api/dmat/create") &&
//                 context.Request.Method == "POST")
//             {
//                 var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
//                 var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
//                 var request = JsonSerializer.Deserialize<DmatAccountRequest>(body, options);

//                 var errors = ValidateRequest(request);

//                 if (errors.Count > 0)
//                 {
//                     context.Response.StatusCode = StatusCodes.Status400BadRequest;
//                     context.Response.ContentType = "application/json";

//                     await context.Response.WriteAsync(JsonSerializer.Serialize(new
//                     {
//                         RequestTime = DateTime.Now,
//                         Status = "Failed",
//                         Errors = errors
//                     }));

//                     return;
//                 }

//                 // Reset the request body for the next middleware
//                 var requestBody = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(body));
//                 context.Request.Body = requestBody;
//                 context.Request.Body.Seek(0, SeekOrigin.Begin);
//             }

//             await _next(context);
//         }

//         private List<string?> ValidateRequest(DmatAccountRequest request)
//         {
//             var validationResults = new List<ValidationResult>();
//             var context = new ValidationContext(request);
//             Validator.TryValidateObject(request, context, validationResults, true);

//             var additionalErrors = validationResults.Select(vr => vr.ErrorMessage).ToList();

//             // Custom validations
//             if (request.SecCode.Length != 4)
//             {
//                 additionalErrors.Add("SecCode must be exactly 4 alphanumeric characters.");
//             }
//             if (!Regex.IsMatch(request.SecCode, "^[a-zA-Z0-9]+$"))
//             {
//                 additionalErrors.Add("SecCode must not contain special characters, spaces, or negative values.");
//             }
//             if (!request.SecCode.Any(char.IsLetter) || !request.SecCode.Any(char.IsDigit))
//             {
//                 additionalErrors.Add("SecCode must contain both letters and numbers.");
//             }



//             if (request.RowCount <= 0)
//             {
//                 additionalErrors.Add("RowCount must be greater than 0 and it must be a positive integer.");
//             }
//             if (!Regex.IsMatch(request.RowCount.ToString(), @"^\d+$"))
//             {
//                 additionalErrors.Add("RowCount must contain only numeric digits (no special characters, letters, or spaces).");
//             }
//             if (request.RowCount >= int.MaxValue)
//             {
//                 additionalErrors.Add("RowCount exceeds the maximum value for an int.");
//             }


//             if (request.PageIndex <= 0)
//             {
//                 additionalErrors.Add("PageIndex must be greater than 0 and it must be a positive integer.");
//             }
//             if (!Regex.IsMatch(request.PageIndex.ToString(), @"^\d+$"))
//             {
//                 additionalErrors.Add("PageIndex must contain only numeric digits (no special characters, letters, or spaces).");
//             }
//             if (request.PageIndex >= int.MaxValue)
//             {
//                 additionalErrors.Add("PageIndex exceeds the maximum value for an int.");
//             }


//             if (request.DtDate.HasValue && request.DtDate.Value > DateTime.UtcNow)
//             {
//                 additionalErrors.Add("DtDate cannot be a future date.");
//             }

//             return additionalErrors;
//         }
//     }
// }

