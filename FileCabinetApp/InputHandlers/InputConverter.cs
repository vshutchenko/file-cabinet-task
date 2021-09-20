using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.InputHandlers
{
    /// <summary>
    /// Performs converting input data.
    /// </summary>
    public class InputConverter
    {
        /// <summary>
        /// Converts input string into <see cref="int"/> value.
        /// </summary>
        /// <param name="stringToConvert">String to convert.</param>
        /// <returns><see cref="Tuple{Boolean, String, Int32>"/>Where the first value indicates was conversion successful or not.
        /// The second value contains an error message if conversion failed.
        /// The third value contains result of conversion if it was successful.</returns>
        public Tuple<bool, string, int> IntConverter(string stringToConvert)
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

        /// <summary>
        /// Checks if input string is null or whitespace.
        /// </summary>
        /// <param name="stringToConvert">String to convert.</param>
        /// <returns><see cref="Tuple{Boolean, String, DateTime>"/>Where the first value indicates was conversion successful or not.
        /// The second value contains an error message if conversion failed.
        /// The third value contains result of conversion if it was successful.</returns>
        public Tuple<bool, string, string> StringConverter(string stringToConvert)
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

        /// <summary>
        /// Converts input string into <see cref="DateTime"/> value.
        /// </summary>
        /// <param name="stringToConvert">String to convert.</param>
        /// <returns><see cref="Tuple{Boolean, String, DateTime>"/>Where the first value indicates was conversion successful or not.
        /// The second value contains an error message if conversion failed.
        /// The third value contains result of conversion if it was successful.</returns>
        public Tuple<bool, string, DateTime> DateTimeConverter(string stringToConvert)
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

        /// <summary>
        /// Converts input string into <see cref="char"/> value.
        /// </summary>
        /// <param name="stringToConvert">String to convert.</param>
        /// <returns><see cref="Tuple{Boolean, String, Char>"/>Where the first value indicates was conversion successful or not.
        /// The second value contains an error message if conversion failed.
        /// The third value contains result of conversion if it was successful.</returns>
        public Tuple<bool, string, char> CharConverter(string stringToConvert)
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

        /// <summary>
        /// Converts input string into <see cref="short"/> value.
        /// </summary>
        /// <param name="stringToConvert">String to convert.</param>
        /// <returns><see cref="Tuple{Boolean, String, Int16>"/>Where the first value indicates was conversion successful or not.
        /// The second value contains an error message if conversion failed.
        /// The third value contains result of conversion if it was successful.</returns>
        public Tuple<bool, string, short> ShortConverter(string stringToConvert)
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

        /// <summary>
        /// Converts input string into <see cref="decimal"/> value.
        /// </summary>
        /// <param name="stringToConvert">String to convert.</param>
        /// <returns><see cref="Tuple{Boolean, String, Decimal>"/>Where the first value indicates was conversion successful or not.
        /// The second value contains an error message if conversion failed.
        /// The third value contains result of conversion if it was successful.</returns>
        public Tuple<bool, string, decimal> DecimalConverter(string stringToConvert)
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
