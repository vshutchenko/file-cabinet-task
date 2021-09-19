using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    public class ImportCommandHandler : ServiceCommandHandlerBase
    {
        private const string Command = "IMPORT";

        public ImportCommandHandler(IFileCabinetService fileCabinetService)
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
                this.fileCabinetService.Restore(serviceSnapshot);
            }
            else if (fileFormat.Equals("XML", StringComparison.InvariantCultureIgnoreCase) && fileInfo.Extension.ToUpperInvariant().Equals(".XML", StringComparison.InvariantCultureIgnoreCase))
            {
                serviceSnapshot.LoadFromXml(reader);
                this.fileCabinetService.Restore(serviceSnapshot);
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
