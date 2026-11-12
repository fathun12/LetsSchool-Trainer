using System.Globalization;

namespace LetsSchoolTrainer.Services;

public static class InputParser
{
    // Whole number from 0 to int.MaxValue (the plugin uses int.Parse for these).
    public static bool TryWhole(string? text, out string value)
    {
        value = "";
        if (!int.TryParse(text?.Trim(), NumberStyles.None, CultureInfo.InvariantCulture, out int n))
            return false;
        value = n.ToString(CultureInfo.InvariantCulture);
        return true;
    }

    // Non-negative number, comma or dot accepted.
    public static bool TryDecimal(string? text, out string value)
    {
        value = "";
        string t = (text ?? "").Trim().Replace(',', '.');
        if (!decimal.TryParse(t, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal d))
            return false;
        value = d.ToString(CultureInfo.InvariantCulture);
        return true;
    }

    public static bool TryParse(string? text, bool whole, out string value)
        => whole ? TryWhole(text, out value) : TryDecimal(text, out value);

    // Blank is allowed and means "leave unchanged" (value = "").
    public static bool TryOptional(string? text, bool whole, out string value)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            value = "";
            return true;
        }
        return TryParse(text, whole, out value);
    }
}