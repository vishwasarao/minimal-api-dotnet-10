namespace MinimalApi.Validators;

using FluentValidation;
using MinimalApi.Dtos;

public class TaskRequestDtoValidator : AbstractValidator<TaskRequestDto>
{
    public TaskRequestDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.")
            .Must(BeValidStatus).WithMessage("Status must be one of: Pending, In Progress, Completed, Cancelled.");

        RuleFor(x => x.DueDate)
            .NotEmpty().WithMessage("Due date is required.")
            .GreaterThan(DateTime.UtcNow).WithMessage("Due date cannot be in the past.");
    }

    private bool BeValidStatus(string status)
    {
        var validStatuses = new[] { "Pending", "In Progress", "Completed", "Cancelled" };
        return validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase);
    }
}

