using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using FileCabinetApp.Iterators;
using FileCabinetApp.RecordModel;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// This class creates log file.
    /// This class implemented as a Decorator for <see cref="IFileCabinetService"/>.
    /// </summary>
    public class ServiceLogger : IFileCabinetService, IDisposable
    {
        private readonly string fileName = "log.txt";
        private IFileCabinetService service;
        private StreamWriter writer;
        private bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogger"/> class.
        /// </summary>
        /// <param name="service">An exemplar of service.</param>
        public ServiceLogger(IFileCabinetService service)
        {
            this.service = service;
            this.writer = new StreamWriter(this.fileName);
            this.writer.AutoFlush = true;
        }

        private enum LogMessageType
        {
            MethodWithoutParameters,
            MethodWithParameters,
            MethodReturnValue,
        }

        /// <inheritdoc/>
        public int CreateRecord(RecordParameters recordParameters)
        {
            string parameters = recordParameters.ToString();
            this.WriteMessage(LogMessageType.MethodWithParameters, nameof(this.CreateRecord), parameters);
            int id = this.service.CreateRecord(recordParameters);
            this.WriteMessage(LogMessageType.MethodReturnValue, nameof(this.CreateRecord), $"'{id}'");
            return id;
        }

        /// <inheritdoc/>
        public void EditRecord(int id, RecordParameters recordParameters)
        {
            string parameters = $"Id = '{id}', " + recordParameters.ToString();
            this.WriteMessage(LogMessageType.MethodWithParameters, nameof(this.EditRecord), parameters);
            this.service.EditRecord(id, recordParameters);
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            string parameters = $"DateOfBirth = '{dateOfBirth}'";
            this.WriteMessage(LogMessageType.MethodWithParameters, nameof(this.FindByDateOfBirth), parameters);
            var records = this.service.FindByDateOfBirth(dateOfBirth);
            //parameters = $"'{records.Count}' records";
            this.WriteMessage(LogMessageType.MethodReturnValue, nameof(this.FindByDateOfBirth), parameters);
            return records;
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            string parameters = $"FirstName = '{firstName}";
            this.WriteMessage(LogMessageType.MethodWithParameters, nameof(this.FindByFirstName), parameters);
            var records = this.service.FindByFirstName(firstName);
            //parameters = $"'{records.Count}' records";
            this.WriteMessage(LogMessageType.MethodReturnValue, nameof(this.FindByFirstName), parameters);
            return records;
        }

        /// <inheritdoc/>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            string parameters = $"LastName = '{lastName}'";
            this.WriteMessage(LogMessageType.MethodWithParameters, nameof(this.FindByLastName), parameters);
            var records = this.service.FindByLastName(lastName);
            //parameters = $"'{records.Count}' records";
            this.WriteMessage(LogMessageType.MethodReturnValue, nameof(this.FindByLastName), parameters);
            return records;
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.WriteMessage(LogMessageType.MethodWithoutParameters, nameof(this.GetRecords));
            var records = this.service.GetRecords();
            string parameters = $"'{records.Count}' records";
            this.WriteMessage(LogMessageType.MethodReturnValue, nameof(this.GetRecords), parameters);
            return records;
        }

        /// <inheritdoc/>
        public Tuple<int, int> GetStat()
        {
            this.WriteMessage(LogMessageType.MethodWithoutParameters, nameof(this.GetStat));
            var statTuple = this.service.GetStat();
            this.WriteMessage(LogMessageType.MethodReturnValue, nameof(this.GetStat), $"'{statTuple}'");
            return statTuple;
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            this.WriteMessage(LogMessageType.MethodWithoutParameters, nameof(this.MakeSnapshot));
            var snapshot = this.service.MakeSnapshot();
            string parameters = $"snapshot with '{snapshot.Records.Count}' records";
            this.WriteMessage(LogMessageType.MethodReturnValue, nameof(this.MakeSnapshot), parameters);
            return snapshot;
        }

        /// <inheritdoc/>
        public Tuple<int, int> Purge()
        {
            this.WriteMessage(LogMessageType.MethodWithoutParameters, nameof(this.Purge));
            var statTuple = this.service.Purge();
            this.WriteMessage(LogMessageType.MethodReturnValue, nameof(this.Purge), $"'{statTuple}'");
            return statTuple;
        }

        /// <inheritdoc/>
        public bool Remove(int id)
        {
            string parameters = $"Id = '{id}'";
            this.WriteMessage(LogMessageType.MethodWithParameters, nameof(this.Remove), parameters);
            bool isRecordExists = this.service.Remove(id);
            this.WriteMessage(LogMessageType.MethodReturnValue, nameof(this.Remove), $"'{isRecordExists}'");
            return isRecordExists;
        }

        /// <inheritdoc/>
        public void Restore(FileCabinetServiceSnapshot serviceSnapshot)
        {
            string parameters = $"{nameof(serviceSnapshot)} which contains '{serviceSnapshot.Records.Count}' records";
            this.WriteMessage(LogMessageType.MethodWithParameters, nameof(this.Restore), parameters);
            this.service.Restore(serviceSnapshot);
        }

        /// <summary>
        /// Free used resources. Calls Dispose(true) method.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Free used resources.
        /// </summary>
        /// <param name="disposing">Displays if resources disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                this.writer.Dispose();
            }

            this.isDisposed = true;
        }

        private void WriteMessage(LogMessageType type, string methodName, string parameters = "")
        {
            string message = DateTime.Now.ToString("MM/dd/yyyy HH:mm");

            switch (type)
            {
                case LogMessageType.MethodWithoutParameters:
                    message += $" - Calling {methodName}()";
                    break;
                case LogMessageType.MethodWithParameters:
                    message += $"- Calling {methodName}() with {parameters}";
                    break;
                case LogMessageType.MethodReturnValue:
                    message += $"- {methodName}() returned {parameters}";
                    break;
            }

            this.writer.WriteLine(message);
        }
    }
}
