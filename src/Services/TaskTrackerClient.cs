namespace MinimalApi.Services;

using MinimalApi.Dtos;
using System.Net.Http.Json;

public interface ITaskTrackerClient
{
    Task<List<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(Product product);
    Task<bool> DeleteProductAsync(int id);
}

public class TaskTrackerClient : ITaskTrackerClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _apiBaseUrl;
    private const string CorrelationIdHeader = "X-Correlation-ID";

    public TaskTrackerClient(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _apiBaseUrl = _configuration["ExternalApi:BaseUrl"] ?? "https://api.example.com";
    }

    private void AddCorrelationIdHeader(HttpRequestMessage request)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            // Try to get correlation ID from context items, or use trace ID
            var correlationId = httpContext.Items.TryGetValue("CorrelationId", out var id)
                ? id?.ToString()
                : httpContext.TraceIdentifier;

            if (!string.IsNullOrEmpty(correlationId))
            {
                request.Headers.Add(CorrelationIdHeader, correlationId);
            }
        }
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiBaseUrl}/products");
            AddCorrelationIdHeader(request);
            
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<Product>>() ?? new List<Product>();
        }
        catch (Exception ex)
        {
            // TODO: Add logging
            return new List<Product>();
        }
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiBaseUrl}/products/{id}");
            AddCorrelationIdHeader(request);
            
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<Product>();
        }
        catch (Exception ex)
        {
            // TODO: Add logging
            return null;
        }
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_apiBaseUrl}/products")
            {
                Content = JsonContent.Create(product)
            };
            AddCorrelationIdHeader(request);
            
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<Product>() ?? product;
        }
        catch (Exception ex)
        {
            // TODO: Add logging
            return product;
        }
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{_apiBaseUrl}/products/{id}");
            AddCorrelationIdHeader(request);
            
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            // TODO: Add logging
            return false;
        }
    }
}



