using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    public class ExportCommandHandler : ServiceCommandHandlerBase
    {
        private const string Command = "EXPORT";

        public ExportCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.Equals(request.Command, Command, StringComparison.InvariantCultureIgnoreCase))
            {
                this.Export(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Export(string parameters)
        {
            if (string.IsNullOrEmpty(parameters) || string.IsNullOrWhiteSpace(parameters) || parameters.Split(' ', 2).Length < 2)
            {
                Console.WriteLine("Invalid parameters.");
                return;
            }

            string[] parametersArray = parameters.Split(' ', 2);
            string fileFormat = parametersArray[0];
            string filePath = parametersArray[1];

            if (File.Exists(filePath))
            {
                Console.Write($"File is exist - rewrite {filePath}? [Y/n] ");
                var key = Console.ReadKey();
                Console.WriteLine();
                if (key.Key != ConsoleKey.Y)
                {
                    return;
                }
            }
            else
            {
                try
                {
                    File.Create(filePath).Close();
                }
                catch (Exception ex) when (
                ex is DirectoryNotFoundException
                || ex is ArgumentException)
                {
                    Console.WriteLine($"Export failed: can't open file {filePath}.");
                    return;
                }
            }

            StreamWriter writer = new StreamWriter(filePath);
            FileCabinetServiceSnapshot serviceSnapshot = this.fileCabinetService.MakeSnapshot();

            if (fileFormat.Equals("CSV", StringComparison.InvariantCultureIgnoreCase))
            {
                serviceSnapshot.SaveToCsv(writer);
                Console.WriteLine($"All records are exported to file {filePath}.");
            }
            else if (fileFormat.Equals("XML", StringComparison.InvariantCultureIgnoreCase))
            {
                serviceSnapshot.SaveToXml(writer);
                Console.WriteLine($"All records are exported to file {filePath}.");
            }
            else
            {
                Console.WriteLine("Unknown file format.");
            }

            writer.Close();
        }
    }
}
