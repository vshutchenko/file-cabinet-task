using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Provides handler for export command.
    /// </summary>
    public class ExportCommandHandler : ServiceCommandHandlerBase
    {
        private const string Command = "EXPORT";

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">A reference to service class is needed because
        /// export command handler calls service methods.</param>
        public ExportCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        /// <summary>
        /// Handles the command or calls next command handler.
        /// </summary>
        /// <param name="request">A command with parameters.</param>
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
            FileCabinetServiceSnapshot serviceSnapshot = this.FileCabinetService.MakeSnapshot();

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
