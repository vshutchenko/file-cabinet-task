using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            bool isParametersValid;
            Tuple<string, string, int, int> vallidParameters;

            Dictionary<string, string> parameters = ReadParameters(args);
            isParametersValid = IsParametersValid(parameters, out vallidParameters);

            if (isParametersValid)
            {
                string format = vallidParameters.Item1;
                string outputFilePath = vallidParameters.Item2;
                int amountOfRecords = vallidParameters.Item3;
                int startId = vallidParameters.Item4;
                ReadOnlyCollection<FileCabinetRecord> records = GenerateRecords(startId, amountOfRecords);
                ExportToCsv(outputFilePath, records);
            }
        }

        private static void ExportToCsv(string filePath, ReadOnlyCollection<FileCabinetRecord> records)
        {
            StreamWriter writer = new StreamWriter(filePath);
            for (int i = 0; i < records.Count; i++)
            {
                string record = $"#{records[i].Id}, " +
                    $"{records[i].FirstName}, " +
                    $"{records[i].LastName}, " +
                    $"{records[i].DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}, " +
                    $"{records[i].Gender}, " +
                    $"{records[i].Experience}, " +
                    $"{records[i].Salary}$";
                writer.WriteLine(record);
            }

            writer.Close();
            Console.WriteLine($"{records.Count} records were written to {filePath}.");
        }

        private static ReadOnlyCollection<FileCabinetRecord> GenerateRecords(int startId, int amountOfRecords)
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();

            string[] firstNames = new[] { "Justin", "Brian", "Gilbert", "William", "Camron", "John", "Matthew", "Paul", "Stephie", "Ann", "Ariana", "Latoya", "Julio" };
            string[] lastNames = new[] { "Thompson", "Simpson", "Olson", "Richardson", "Johnston", "John", "Taylor", "Monroe", "Ellis", "Walker", "Wilson", "Davis", "Martin" };
            char[] genders = new[] { 'F', 'M' };
            Random random = new Random();

            for (int i = startId; i < startId + amountOfRecords; i++)
            {
                int firstNameIndex = random.Next(0, firstNames.Length);
                int lastNameIndex = random.Next(0, lastNames.Length);
                int genderIndex = random.Next(0, genders.Length);

                DateTime dateOfBirth = new DateTime(1950, 1, 1);
                int range = (DateTime.Today - dateOfBirth).Days;
                dateOfBirth.AddDays(random.Next(range));

                short experence = (short)random.Next(0, short.MaxValue);
                decimal salary = random.Next(0, int.MaxValue);

                FileCabinetRecord record = new FileCabinetRecord() { Id = i, FirstName = firstNames[firstNameIndex], LastName = lastNames[lastNameIndex], DateOfBirth = dateOfBirth, Gender = genders[genderIndex], Experience = experence, Salary = salary };
                records.Add(record);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
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
