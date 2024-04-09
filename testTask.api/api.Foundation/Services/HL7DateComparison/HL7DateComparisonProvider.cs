using System.Linq.Expressions;

namespace api.Foundation.Services.HL7DateComparison;

public static class HL7DateComparisonProvider
{
    public static Func<DateTime, DateTime, bool> GetComparer(Hl7ComparisonMode mode)
    {
        return mode switch
        {
            Hl7ComparisonMode.Eq => (left, right) => left.Date == right.Date,
            Hl7ComparisonMode.Ne => (left, right) => left.Date != right.Date,
            Hl7ComparisonMode.Gt => (left, right) => left > right,
            Hl7ComparisonMode.Lt => (left, right) => left < right,
            Hl7ComparisonMode.Ge => (left, right) => left >= right,
            Hl7ComparisonMode.Le => (left, right) => left <= right,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), "Invalid comparison operation"),
        };
    }
}