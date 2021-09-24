using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Service
{
    public class ServiceMeter : IFileCabinetService
    {
        private IFileCabinetService service;
        private Stopwatch stopwatch;

        public ServiceMeter(IFileCabinetService service)
        {
            this.service = service;
            this.stopwatch = new Stopwatch();
        }

        public int CreateRecord(RecordParameters recordParameters)
        {
            this.stopwatch.Restart();
            int id = this.service.CreateRecord(recordParameters);
            this.stopwatch.Stop();

            Print(nameof(this.CreateRecord), this.stopwatch.ElapsedTicks);

            return id;
        }

        public void EditRecord(int id, RecordParameters recordParameters)
        {
            this.stopwatch.Restart();
            this.service.EditRecord(id, recordParameters);
            this.stopwatch.Stop();

            Print(nameof(this.EditRecord), this.stopwatch.ElapsedTicks);
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            this.stopwatch.Restart();
            var records = this.service.FindByDateOfBirth(dateOfBirth);
            this.stopwatch.Stop();

            Print(nameof(this.FindByDateOfBirth), this.stopwatch.ElapsedTicks);

            return records;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.stopwatch.Restart();
            var records = this.service.FindByFirstName(firstName);
            this.stopwatch.Stop();

            Print(nameof(this.FindByFirstName), this.stopwatch.ElapsedTicks);

            return records;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.stopwatch.Restart();
            var records = this.service.FindByLastName(lastName);
            this.stopwatch.Stop();

            Print(nameof(this.FindByLastName), this.stopwatch.ElapsedTicks);

            return records;
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.stopwatch.Restart();
            var records = this.service.GetRecords();
            this.stopwatch.Stop();

            Print(nameof(this.GetRecords), this.stopwatch.ElapsedTicks);

            return records;
        }

        public Tuple<int, int> GetStat()
        {
            this.stopwatch.Restart();
            var recordsCount = this.service.GetStat();
            this.stopwatch.Stop();

            Print(nameof(this.GetStat), this.stopwatch.ElapsedTicks);

            return recordsCount;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            this.stopwatch.Restart();
            var snapshot = this.service.MakeSnapshot();
            this.stopwatch.Stop();

            Print(nameof(this.MakeSnapshot), this.stopwatch.ElapsedTicks);

            return snapshot;
        }

        public Tuple<int, int> Purge()
        {
            this.stopwatch.Restart();
            var deletedRecords = this.service.Purge();
            this.stopwatch.Stop();

            Print(nameof(this.Purge), this.stopwatch.ElapsedTicks);

            return deletedRecords;
        }

        public bool Remove(int id)
        {
            this.stopwatch.Restart();
            var isRecordExists = this.service.Remove(id);
            this.stopwatch.Stop();

            Print(nameof(this.Remove), this.stopwatch.ElapsedTicks);

            return isRecordExists;
        }

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
    }
}
