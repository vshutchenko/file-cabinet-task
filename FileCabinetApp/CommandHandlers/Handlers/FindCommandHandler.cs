using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    public class FindCommandHandler : CommandHandlerBase
    {
        private const string Command = "FIND";
        private IFileCabinetService fileCabinetService;

        public FindCommandHandler(IFileCabinetService fileCabinetService)
        {
            this.fileCabinetService = fileCabinetService;
        }

        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.Equals(request.Command, Command, StringComparison.InvariantCultureIgnoreCase))
            {
                this.Find(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Find(string parameters)
        {
            if (string.IsNullOrEmpty(parameters) || string.IsNullOrWhiteSpace(parameters))
            {
                Console.WriteLine("Wrong parameters. Please specify name of property and text to search.");
                return;
            }

            string[] paramArray = parameters.Split(' ', 2);
            string propertyName = paramArray[0];
            string textToSearch = paramArray[1];

            if ((textToSearch.Length < 2) || (textToSearch[0] != '"') || (textToSearch[^1] != '"'))
            {
                Console.WriteLine("Wrong parameters.");
                return;
            }

            textToSearch = textToSearch[1..^1];

            ReadOnlyCollection<FileCabinetRecord> records = new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
            string record;

            if (string.Equals(propertyName, nameof(FileCabinetRecord.FirstName), StringComparison.InvariantCultureIgnoreCase))
            {
                records = this.fileCabinetService.FindByFirstName(textToSearch);
            }
            else if (string.Equals(propertyName, nameof(FileCabinetRecord.LastName), StringComparison.InvariantCultureIgnoreCase))
            {
                records = this.fileCabinetService.FindByLastName(textToSearch);
            }
            else if (string.Equals(propertyName, nameof(FileCabinetRecord.DateOfBirth), StringComparison.InvariantCultureIgnoreCase))
            {
                if (DateTime.TryParse(textToSearch, out DateTime dateOfBirth))
                {
                    records = this.fileCabinetService.FindByDateOfBirth(dateOfBirth);
                }
            }

            for (int i = 0; i < records.Count; i++)
            {
                record = $"#{records[i].Id}, " +
                $"{records[i].FirstName}, " +
                $"{records[i].LastName}, " +
                $"{records[i].DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}, " +
                $"{records[i].Gender}, " +
                $"{records[i].Experience}, " +
                $"{records[i].Salary}$";
                Console.WriteLine(record);
            }
        }
    }
}
