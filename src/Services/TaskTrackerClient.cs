namespace MinimalApi.Services;

using MinimalApi.Dtos;
using System.Net.Http.Json;

public interface ITaskTrackerClient
{
    Task<List<TaskRequestDto>> GetAllTasksAsync();
    Task<TaskRequestDto?> GetTaskByIdAsync(int id);
    Task<TaskRequestDto> CreateTaskAsync(TaskRequestDto taskItem);
    Task<bool> DeleteTaskAsync(int id);
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

    public async Task<List<TaskRequestDto>> GetAllTasksAsync()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiBaseUrl}/tasks");
            AddCorrelationIdHeader(request);
            
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<TaskRequestDto>>() ?? new List<TaskRequestDto>();
        }
        catch (Exception ex)
        {
            // TODO: Add logging
            return new List<TaskRequestDto>();
        }
    }

    public async Task<TaskRequestDto?> GetTaskByIdAsync(int id)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiBaseUrl}/tasks/{id}");
            AddCorrelationIdHeader(request);
            
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TaskRequestDto>();
        }
        catch (Exception ex)
        {
            // TODO: Add logging
            return null;
        }
    }

    public async Task<TaskRequestDto> CreateTaskAsync(TaskRequestDto taskItem)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_apiBaseUrl}/tasks")
            {
                Content = JsonContent.Create(taskItem)
            };
            AddCorrelationIdHeader(request);
            
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TaskRequestDto>() ?? taskItem;
        }
        catch (Exception ex)
        {
            // TODO: Add logging
            return taskItem;
        }
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{_apiBaseUrl}/tasks/{id}");
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



