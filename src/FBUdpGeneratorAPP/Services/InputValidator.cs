using System.Text.RegularExpressions;

namespace FBUdpGeneratorAPP.Services
{
    internal static class InputValidator
    {
        public static bool IsNumeric(string value)
        {
            Regex regex = new Regex("[^0-9]+");
            return regex.IsMatch(value);
        }
    }
}
