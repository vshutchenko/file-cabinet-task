using System;
using System.Collections.Generic;
using System.IO;

namespace FileCabinetGenerator
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            string outputFilePath;
            string format;
            int amountOfRecords;
            int startId;
            bool isParametersValid;
            Tuple<string, string, int, int> vallidParameters;

            Dictionary<string, string> parameters = ReadParameters(args);
            isParametersValid = IsParametersValid(parameters, out vallidParameters);

            if (isParametersValid)
            {
                format = vallidParameters.Item1;
                outputFilePath = vallidParameters.Item2;
                amountOfRecords = vallidParameters.Item3;
                startId = vallidParameters.Item4;

                Console.WriteLine($"{amountOfRecords} records were written to {outputFilePath}.");
            }
        }

        private static bool IsParametersValid(Dictionary<string, string> parameters, out Tuple<string, string, int, int> vallidParameters)
        {
            string outputFilePath;
            string amountString;
            string startIdString;
            string format;
            int amountOfRecords;
            int startId;

            vallidParameters = new Tuple<string, string, int, int>(string.Empty, string.Empty, 0, 0);

            if (parameters.TryGetValue("-T", out format) || parameters.TryGetValue("--OUTPUT-TYPE", out format))
            {
                if (!format.Equals("CSV", StringComparison.InvariantCultureIgnoreCase) && !format.Equals("XML", StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine("Invalid output format type (supported formats: csv, xml).");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Please, specify the output format.");
                return false;
            }

            if (parameters.TryGetValue("-O", out outputFilePath) || parameters.TryGetValue("--OUTPUT", out outputFilePath))
            {
                try
                {
                    FileStream fileStream = File.Create(outputFilePath);
                    fileStream.Close();
                    File.Delete(outputFilePath);
                }
                catch (Exception ex) when(
                ex is UnauthorizedAccessException
                || ex is PathTooLongException
                || ex is DirectoryNotFoundException)
                {
                    Console.WriteLine("Invalid file path.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Please, specify the output file path.");
                return false;
            }

            if (parameters.TryGetValue("-A", out amountString) || parameters.TryGetValue("--RECORDS-AMOUNT", out amountString))
            {
                if (!int.TryParse(amountString, out amountOfRecords) || amountOfRecords < 1)
                {
                    Console.WriteLine("Amount of records must be a positive integer.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Please, specify the amount of records.");
                return false;
            }

            if (parameters.TryGetValue("-I", out startIdString) || parameters.TryGetValue("--START-ID", out startIdString))
            {
                if (!int.TryParse(startIdString, out startId) || startId < 1)
                {
                    Console.WriteLine("Start id must be a positive integer.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Please, specify the start id.");
                return false;
            }

            vallidParameters = new Tuple<string, string, int, int>(format, outputFilePath, amountOfRecords, startId);
            return true;
        }

        private static Dictionary<string, string> ReadParameters(string[] args)
        {
            char dash = '-';
            string doubleDash = "--";
            string equals = "=";

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith(doubleDash))
                {
                    string[] currentParameter = args[i].Split(equals, 2);
                    parameters.Add(currentParameter[0].ToUpperInvariant(), currentParameter[^1]);
                }
                else if (args[i].StartsWith(dash) && (i + 1 < args.Length))
                {
                    parameters.Add(args[i].ToUpperInvariant(), args[i + 1]);
                    i++;
                }
            }

            return parameters;
        }
    }
}
