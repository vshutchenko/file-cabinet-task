using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers.Handlers
{
    public class ListCommandHandler : CommandHandlerBase
    {
        private const string Command = "LIST";

        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.Equals(request.Command, Command, StringComparison.InvariantCultureIgnoreCase))
            {
                this.List(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void List(string parameters)
        {
            string record;
            ReadOnlyCollection<FileCabinetRecord> records = Program.fileCabinetService.GetRecords();

            for (int i = 0; i < Program.fileCabinetService.GetStat().Item2; i++)
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
