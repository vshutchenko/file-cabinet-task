using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace FileCabinetApp
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
        /// <param name="records">The array of instances of the <see cref="FileCabinetRecord"/> class.</param>
        public FileCabinetServiceSnapshot(FileCabinetRecord[] records)
        {
            this.records = records;
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
    }
}
