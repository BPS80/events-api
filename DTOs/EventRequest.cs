using System.ComponentModel.DataAnnotations;
using EventsApi.Utilities;

namespace EventsApi.DTOs;

public class EventRequest : IValidatableObject
{
    [Required(ErrorMessage = "Укажите название мероприятия.")]
    public string? Title { get; set; }

    public string? Description { get; set; }

    private DateTime? _startAt;
    private DateTime? _endAt;

    [Required(ErrorMessage = "Укажите дату начала.")]
    public DateTime? StartAt
    {
        get => _startAt;
        set => _startAt = value.HasValue
            ? DateTimeNormalizer.ToUtc(value.Value)
            : null;
    }

    [Required(ErrorMessage = "Укажите дату окончания.")]
    public DateTime? EndAt
    {
        get => _endAt;
        set => _endAt = value.HasValue
            ? DateTimeNormalizer.ToUtc(value.Value)
            : null;
    }

    public IEnumerable<ValidationResult> Validate(
       ValidationContext validationContext)
    {
        if (StartAt.HasValue && StartAt.Value == default(DateTime))
        {
            yield return new ValidationResult(
                "Дата начала не может быть значением по умолчанию.",
                new[] { nameof(StartAt) });
        }

        if (EndAt.HasValue && EndAt.Value == default(DateTime))
        {
            yield return new ValidationResult(
                "Дата окончания не может быть значением по умолчанию.",
                new[] { nameof(EndAt) });
        }

        if (StartAt.HasValue &&
            EndAt.HasValue &&
            StartAt.Value != default(DateTime) &&
            EndAt.Value != default(DateTime) &&
            EndAt.Value <= StartAt.Value)
        {
            yield return new ValidationResult(
                "Дата окончания должна быть позже даты начала.",
                new[] { nameof(EndAt) });
        }
    }
}
