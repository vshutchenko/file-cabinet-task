using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    /// <summary>
    /// Provides handler for import command.
    /// </summary>
    public class ImportCommandHandler : ServiceCommandHandlerBase
    {
        private const string Command = "IMPORT";

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">A reference to service class is needed because
        /// import command handler calls service methods.</param>
        public ImportCommandHandler(IFileCabinetService fileCabinetService)
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
                this.Import(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Import(string parameters)
        {
            if (string.IsNullOrEmpty(parameters) || string.IsNullOrWhiteSpace(parameters) || parameters.Split(' ', 2).Length < 2)
            {
                Console.WriteLine("Invalid parameters.");
                return;
            }

            string[] parametersArray = parameters.Split(' ', 2);
            string fileFormat = parametersArray[0];
            string filePath = parametersArray[1];
            FileStream fileStream;
            StreamReader reader;
            FileCabinetServiceSnapshot serviceSnapshot = new FileCabinetServiceSnapshot();

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Import error: file {filePath} is not exist.");
                return;
            }

            fileStream = new FileStream(filePath, FileMode.Open);
            reader = new StreamReader(fileStream);

            FileInfo fileInfo = new FileInfo(filePath);

            if (fileFormat.Equals("CSV", StringComparison.InvariantCultureIgnoreCase) && fileInfo.Extension.ToUpperInvariant().Equals(".CSV", StringComparison.InvariantCultureIgnoreCase))
            {
                serviceSnapshot.LoadFromCsv(reader);
                this.FileCabinetService.Restore(serviceSnapshot);
            }
            else if (fileFormat.Equals("XML", StringComparison.InvariantCultureIgnoreCase) && fileInfo.Extension.ToUpperInvariant().Equals(".XML", StringComparison.InvariantCultureIgnoreCase))
            {
                serviceSnapshot.LoadFromXml(reader);
                this.FileCabinetService.Restore(serviceSnapshot);
            }
            else
            {
                Console.WriteLine("Invalid file type.");
            }

            Console.WriteLine($"{serviceSnapshot.Records.Count} records were imported from {filePath}.");

            reader.Close();
            fileStream.Close();
        }
    }
}
