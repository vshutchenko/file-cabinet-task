using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FileCabinetApp.Readers;
using FileCabinetApp.RecordModel;
using FileCabinetApp.Writers;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// This class represent a snapshot of the <see cref="IFileCabinetService"/>.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecord[] records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        public FileCabinetServiceSnapshot()
        {
            this.records = Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="records">The array of instances of the <see cref="FileCabinetRecord"/> class.</param>
        public FileCabinetServiceSnapshot(FileCabinetRecord[] records)
        {
            this.records = records;
        }

        /// <summary>
        /// Gets collection of records store in snapshot.
        /// </summary>
        /// <value>
        /// Collection of records.
        /// </value>
        public ReadOnlyCollection<FileCabinetRecord> Records
        {
            get
            {
                return new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>(this.records));
            }
        }

        /// <summary>
        /// Writes array of records in a CSV format using a <see cref="StreamWriter"/> class.
        /// </summary>
        /// <param name="writer">The instance of the <see cref="StreamWriter"/> class which will be used for writing data.</param>
        public void SaveToCsv(StreamWriter writer)
        {
            FileCabinetRecordCsvWriter csvWriter = new FileCabinetRecordCsvWriter(writer);
            for (int i = 0; i < this.records.Length; i++)
            {
                csvWriter.Write(this.records[i]);
            }
        }

        /// <summary>
        /// Writes array of records in a XML format using a <see cref="StreamWriter"/> class.
        /// </summary>
        /// <param name="writer">The instance of the <see cref="StreamWriter"/> class which will be used for writing data.</param>
        public void SaveToXml(StreamWriter writer)
        {
            var xmlWriter = XmlWriter.Create(writer);
            FileCabinetRecordXmlWriter fileCabinetRecordXmlWriter = new FileCabinetRecordXmlWriter(xmlWriter);
            for (int i = 0; i < this.records.Length; i++)
            {
                fileCabinetRecordXmlWriter.Write(this.records[i]);
            }

            xmlWriter.Close();
        }

        /// <summary>
        /// Loads records from csv file.
        /// </summary>
        /// <param name="reader">A stream to csv file.</param>
        public void LoadFromCsv(StreamReader reader)
        {
            FileCabinetRecordCsvReader csvReader = new FileCabinetRecordCsvReader(reader);
            var records = csvReader.ReadAll();
            this.records = new FileCabinetRecord[records.Count];
            records.CopyTo(this.records, 0);
        }

        /// <summary>
        /// Loads records from xml file.
        /// </summary>
        /// <param name="reader">A stream to xml file.</param>
        public void LoadFromXml(StreamReader reader)
        {
            FileCabinetRecordXmlReader xmlReader = new FileCabinetRecordXmlReader(reader);
            var records = xmlReader.ReadAll();
            this.records = new FileCabinetRecord[records.Count];
            records.CopyTo(this.records, 0);
        }
    }
}
