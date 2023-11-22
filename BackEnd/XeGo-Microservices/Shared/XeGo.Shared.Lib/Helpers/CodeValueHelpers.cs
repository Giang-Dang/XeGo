namespace XeGo.Shared.Lib.Helpers
{
    public static class CodeValueHelpers
    {
        public static dynamic? GetOriginalValue(string input, string outputType)
        {
            switch (outputType.ToUpper())
            {
                case "INT":
                    return int.TryParse(input, out int intValue) ? intValue : null;
                case "DATETIME":
                    return DateTime.TryParse(input, out DateTime dateTimeValue) ? dateTimeValue : null;
                case "DOUBLE":
                    return double.TryParse(input, out double doubleValue) ? doubleValue : null;
                case "DECIMAL":
                    return decimal.TryParse(input, out decimal decimalValue) ? decimalValue : null;
                case "BOOL":
                    return bool.TryParse(input, out bool boolValue) ? boolValue : null;
                case "DATEONLY":
                    return DateOnly.TryParse(input, out DateOnly dateValue) ? dateValue : null;
                default:
                    return null;
            }
        }
    }
}
