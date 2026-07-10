using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Exceptions;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClinicProjectInfrastructure.Services
{
    public class BrevoEmailService : IEmailSender
    {
        private readonly HttpClient _httpClient;
        private readonly BrevoSettings _settings;
        private readonly ILogger<BrevoEmailService> _logger;

        public BrevoEmailService(
            HttpClient httpClient,
            IOptions<BrevoSettings> settings,
            ILogger<BrevoEmailService> logger)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("api-key", _settings.ApiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task SendEmailAsync(
            string to,
            string subject,
            string body,
            bool isHtml = true,
            CancellationToken cancellationToken = default)
        {
            var payload = new
            {
                sender = new { email = _settings.SenderEmail, name = _settings.SenderName },
                to = new[] { new { email = to } },
                subject,
                htmlContent = isHtml ? body : null,
                textContent = isHtml ? null : body
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("smtp/email", payload, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogError(
                        "Brevo email send failed. Status: {StatusCode}, Body: {ErrorBody}",
                        response.StatusCode, errorBody);

                    throw new EmailDeliveryException(
                        $"Failed to send email via Brevo. Status: {response.StatusCode}");
                }

                _logger.LogInformation("Email sent successfully to {Recipient} via Brevo", to);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error while sending email via Brevo to {Recipient}", to);
                throw new EmailDeliveryException("Email provider unreachable.", ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse Brevo response for {Recipient}", to);
                throw new EmailDeliveryException("Unexpected response from email provider.", ex);
            }
        }

        // Then in BrevoEmailService:
        //
        public async Task SendTemplateEmailAsync(BrevoTemplateEmailRequest request, CancellationToken cancellationToken = default)
        {
            var payload = new
            {
                to = new[] { new { email = request.To } },
                templateId = request.TemplateId,
                params_ = request.Params // NOTE: Brevo API field is actually "params", adjust JSON property name via [JsonPropertyName]
            };
            var response = await _httpClient.PostAsJsonAsync("smtp/email", payload, cancellationToken);
            // same error handling as above
        }

     
    }
}
