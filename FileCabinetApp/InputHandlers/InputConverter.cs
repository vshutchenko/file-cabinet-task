using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.InputHandlers
{
    public static class InputConverter
    {
        public static Tuple<bool, string, int> IntConverter(string stringToConvert)
        {
            Tuple<bool, string, int> conversionResult;
            string conversionErrorMessage = string.Empty;

            bool isConverted = int.TryParse(stringToConvert, out int convertedValue);

            if (!isConverted)
            {
                conversionErrorMessage = "Input value is not a number.";
            }

            conversionResult = new Tuple<bool, string, int>(isConverted, conversionErrorMessage, convertedValue);

            return conversionResult;
        }

        public static Tuple<bool, string, string> StringConverter(string stringToConvert)
        {
            bool isConverted = true;
            string conversionErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(stringToConvert) || string.IsNullOrEmpty(stringToConvert))
            {
                isConverted = false;
                conversionErrorMessage = "Input string is null, empty or whitespace";
            }

            Tuple<bool, string, string> conversionResult = new Tuple<bool, string, string>(isConverted, conversionErrorMessage, stringToConvert);
            return conversionResult;
        }

        public static Tuple<bool, string, DateTime> DateTimeConverter(string stringToConvert)
        {
            Tuple<bool, string, DateTime> conversionResult;
            string conversionErrorMessage = string.Empty;

            bool isConverted = DateTime.TryParse(stringToConvert, out DateTime date);

            if (!isConverted)
            {
                conversionErrorMessage = "Incorrect date format";
            }

            conversionResult = new Tuple<bool, string, DateTime>(isConverted, conversionErrorMessage, date);

            return conversionResult;
        }

        public static Tuple<bool, string, char> CharConverter(string stringToConvert)
        {
            Tuple<bool, string, char> conversionResult;
            string conversionErrorMessage = string.Empty;

            bool isConverted = char.TryParse(stringToConvert, out char convertedValue);

            if (!isConverted)
            {
                conversionErrorMessage = "Input value is not a single character";
            }

            conversionResult = new Tuple<bool, string, char>(isConverted, conversionErrorMessage, convertedValue);

            return conversionResult;
        }

        public static Tuple<bool, string, short> ShortConverter(string stringToConvert)
        {
            Tuple<bool, string, short> conversionResult;
            string conversionErrorMessage = string.Empty;

            bool isConverted = short.TryParse(stringToConvert, out short convertedValue);

            if (!isConverted)
            {
                conversionErrorMessage = "Input value is not a number or a too big value";
            }

            conversionResult = new Tuple<bool, string, short>(isConverted, conversionErrorMessage, convertedValue);

            return conversionResult;
        }

        public static Tuple<bool, string, decimal> DecimalConverter(string stringToConvert)
        {
            Tuple<bool, string, decimal> conversionResult;
            string conversionErrorMessage = string.Empty;

            bool isConverted = decimal.TryParse(stringToConvert, out decimal convertedValue);

            if (!isConverted)
            {
                conversionErrorMessage = "Input value is not a number";
            }

            conversionResult = new Tuple<bool, string, decimal>(isConverted, conversionErrorMessage, convertedValue);

            return conversionResult;
        }
    }
}
