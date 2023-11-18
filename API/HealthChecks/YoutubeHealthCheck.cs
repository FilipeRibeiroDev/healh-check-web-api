using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace API.HealthChecks
{
    public class YoutubeHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
			try
			{
                var url = "https://www.youtube.com.io/@filipebritodev";

                using HttpClient client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = true});
                using var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                    return Task.FromResult(new HealthCheckResult(HealthStatus.Healthy, "Sistema Funcionando."));
                else
                    return Task.FromResult(new HealthCheckResult(HealthStatus.Degraded, "Sistema Não está Funcionando."));


            }
			catch (Exception)
			{
                return Task.FromResult(new HealthCheckResult(HealthStatus.Unhealthy, "Sistema Está fora do Ar."));
            }
        }
    }
}
