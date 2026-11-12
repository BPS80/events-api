using System.ComponentModel.DataAnnotations;

namespace EventsApi.DTOs;

public class EventRequest : IValidatableObject
{
    [Required(ErrorMessage = "Укажите название мероприятия.")]
    public string? Title { get; set; }

    public string? Description { get; set; }

    [Required(ErrorMessage = "Укажите дату начала.")]
    public DateTime? StartAt { get; set; }

    [Required(ErrorMessage = "Укажите дату окончания.")]
    public DateTime? EndAt { get; set; }

    public IEnumerable<ValidationResult> Validate(
        ValidationContext validationContext)
    {
        if (StartAt.HasValue &&
            EndAt.HasValue &&
            EndAt.Value <= StartAt.Value)
        {
            yield return new ValidationResult(
                "Дата окончания должна быть позже даты начала.",
                new[] { nameof(EndAt) });
        }
    }
}
