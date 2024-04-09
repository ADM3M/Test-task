namespace api.Foundation.Services.HL7DateComparison;

public static class Hl7DateInfoParser
{
    public static readonly string[] AvailableModes = {
        Hl7ComparisonModeOptions.Eq,
        Hl7ComparisonModeOptions.Ne,
        Hl7ComparisonModeOptions.Gt,
        Hl7ComparisonModeOptions.Lt,
        Hl7ComparisonModeOptions.Ge,
        Hl7ComparisonModeOptions.Le,
    };
    
    public static HL7DateInfo Parse(string source)
    {
        var comparisonMode = GetMode(source[..2]);
        var date = DateTime.Parse(source[2..]);

        return new HL7DateInfo
        {
            Date = date,
            Mode = comparisonMode,
        };
    }


    private static Hl7ComparisonMode GetMode(string modeOption) => modeOption switch
    {
        Hl7ComparisonModeOptions.Eq => Hl7ComparisonMode.Eq,
        Hl7ComparisonModeOptions.Ne => Hl7ComparisonMode.Ne,
        Hl7ComparisonModeOptions.Gt => Hl7ComparisonMode.Gt,
        Hl7ComparisonModeOptions.Lt => Hl7ComparisonMode.Lt,
        Hl7ComparisonModeOptions.Ge => Hl7ComparisonMode.Ge,
        Hl7ComparisonModeOptions.Le => Hl7ComparisonMode.Le,
        Hl7ComparisonModeOptions.Sa => Hl7ComparisonMode.Sa,
        Hl7ComparisonModeOptions.Eb => Hl7ComparisonMode.Eb,
        Hl7ComparisonModeOptions.Ap => Hl7ComparisonMode.Ap,
        _ => throw new ArgumentOutOfRangeException(nameof(modeOption), "Unsupported enum value")
    };
}