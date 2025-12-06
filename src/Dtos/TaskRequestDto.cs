namespace MinimalApi.Dtos;

public class TaskRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public DateTime DueDate { get; set; }
}
