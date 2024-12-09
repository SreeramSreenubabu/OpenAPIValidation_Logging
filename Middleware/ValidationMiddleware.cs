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

                    await context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        RequestTime = DateTime.Now,
                        Status = "Failed",
                        Errors = errors
                    }));

                    return;
                }

                // Reset the request body for the next middleware
                var requestBody = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(body));
                context.Request.Body = requestBody;
                context.Request.Body.Seek(0, SeekOrigin.Begin);
            }

            await _next(context);
        }

        private List<string> ValidateRequest(DmatAccountRequest request)
        {
            var errors = new List<string>();

            // Mandatory fields
            if (string.IsNullOrWhiteSpace(request.FullName) || request.FullName.Length > 100)
                errors.Add("FullName is required and must be less than 100 characters.");

            if (string.IsNullOrWhiteSpace(request.Email) || 
                !Regex.IsMatch(request.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                errors.Add("Email is required and must be a valid email address.");

            if (string.IsNullOrWhiteSpace(request.MobileNumber) || 
                !Regex.IsMatch(request.MobileNumber, @"^\d{10}$"))
                errors.Add("MobileNumber is required and must be a 10-digit number.");

            if (string.IsNullOrWhiteSpace(request.PanNumber) || 
                !Regex.IsMatch(request.PanNumber, @"^[A-Z]{5}\d{4}[A-Z]$"))
                errors.Add("PanNumber is required and must be a valid format.");

            if (string.IsNullOrWhiteSpace(request.AadharNumber) || 
                !Regex.IsMatch(request.AadharNumber, @"^\d{12}$"))
                errors.Add("AadharNumber is required and must be 12 digits.");

            if (string.IsNullOrWhiteSpace(request.DateOfBirth) || 
                !DateTime.TryParse(request.DateOfBirth, out var dob) || 
                dob.AddYears(18) > DateTime.Now)
                errors.Add("DateOfBirth is required and must be a valid date. Applicant must be at least 18 years old.");

            if (string.IsNullOrWhiteSpace(request.Address) || request.Address.Length > 250)
                errors.Add("Address is required and must be less than 250 characters.");

            // Optional fields
            if (!string.IsNullOrWhiteSpace(request.Occupation) && request.Occupation.Length > 50)
                errors.Add("Occupation must be less than 50 characters.");

            if (request.AnnualIncome.HasValue && request.AnnualIncome.Value < 0)
                errors.Add("AnnualIncome must be a positive value.");

            if (!string.IsNullOrWhiteSpace(request.NomineeName)&& request.NomineeName.Length > 100||request.NomineeName=="")
                errors.Add("NomineeName must be less than 100 characters.");

            return errors;
        }
    }
}
