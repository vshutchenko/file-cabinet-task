using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using FileCabinetApp.Iterators;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Measures execution time of service methods.
    /// This class implemented as a Decorator for <see cref="IFileCabinetService"/>.
    /// </summary>
    public class ServiceMeter : IFileCabinetService
    {
        private IFileCabinetService service;
        private Stopwatch stopwatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMeter"/> class.
        /// </summary>
        /// <param name="service">An exemplar of service.</param>
        public ServiceMeter(IFileCabinetService service)
        {
            this.service = service;
            this.stopwatch = new Stopwatch();
        }

        /// <inheritdoc />
        public int CreateRecord(RecordParameters recordParameters)
        {
            this.stopwatch.Restart();
            int id = this.service.CreateRecord(recordParameters);
            this.stopwatch.Stop();

            Print(nameof(this.CreateRecord), this.stopwatch.ElapsedTicks);

            return id;
        }

        /// <inheritdoc />
        public void EditRecord(int id, RecordParameters recordParameters)
        {
            this.stopwatch.Restart();
            this.service.EditRecord(id, recordParameters);
            this.stopwatch.Stop();

            Print(nameof(this.EditRecord), this.stopwatch.ElapsedTicks);
        }

        /// <inheritdoc />
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            this.stopwatch.Restart();
            var records = this.service.FindByDateOfBirth(dateOfBirth);
            this.stopwatch.Stop();

            Print(nameof(this.FindByDateOfBirth), this.stopwatch.ElapsedTicks);

            return records;
        }

        /// <inheritdoc />
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.stopwatch.Restart();
            var records = this.service.FindByFirstName(firstName);
            this.stopwatch.Stop();

            Print(nameof(this.FindByFirstName), this.stopwatch.ElapsedTicks);

            return records;
        }

        /// <inheritdoc />
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.stopwatch.Restart();
            var records = this.service.FindByLastName(lastName);
            this.stopwatch.Stop();

            Print(nameof(this.FindByLastName), this.stopwatch.ElapsedTicks);

            return records;
        }

        /// <inheritdoc />
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.stopwatch.Restart();
            var records = this.service.GetRecords();
            this.stopwatch.Stop();

            Print(nameof(this.GetRecords), this.stopwatch.ElapsedTicks);

            return records;
        }

        /// <inheritdoc />
        public Tuple<int, int> GetStat()
        {
            this.stopwatch.Restart();
            var recordsCount = this.service.GetStat();
            this.stopwatch.Stop();

            Print(nameof(this.GetStat), this.stopwatch.ElapsedTicks);

            return recordsCount;
        }

        /// <inheritdoc />
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            this.stopwatch.Restart();
            var snapshot = this.service.MakeSnapshot();
            this.stopwatch.Stop();

            Print(nameof(this.MakeSnapshot), this.stopwatch.ElapsedTicks);

            return snapshot;
        }

        /// <inheritdoc />
        public Tuple<int, int> Purge()
        {
            this.stopwatch.Restart();
            var deletedRecords = this.service.Purge();
            this.stopwatch.Stop();

            Print(nameof(this.Purge), this.stopwatch.ElapsedTicks);

            return deletedRecords;
        }

        /// <inheritdoc />
        public bool Remove(int id)
        {
            this.stopwatch.Restart();
            var isRecordExists = this.service.Remove(id);
            this.stopwatch.Stop();

            Print(nameof(this.Remove), this.stopwatch.ElapsedTicks);

            return isRecordExists;
        }

        /// <inheritdoc />
        public void Restore(FileCabinetServiceSnapshot serviceSnapshot)
        {
            this.stopwatch.Restart();
            this.service.Restore(serviceSnapshot);
            this.stopwatch.Stop();

            Print(nameof(this.Restore), this.stopwatch.ElapsedTicks);
        }

        private static void Print(string methodName, long elapsedTicks)
        {
            Console.WriteLine($"{methodName} method execution duration is {elapsedTicks} ticks.");
        }

        public int Insert(FileCabinetRecord record)
        {
            this.stopwatch.Restart();
            this.service.Insert(record);
            this.stopwatch.Stop();

            Print(nameof(this.Insert), this.stopwatch.ElapsedTicks);

            return record.Id;
        }
    }
}
