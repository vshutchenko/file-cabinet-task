using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using FileCabinetApp.RecordModel;
using FileCabinetApp.RecordModel.XmlModel;

namespace FileCabinetApp.Readers
{
    /// <summary>
    /// This class performs reading of xml files.
    /// </summary>
    public class FileCabinetRecordXmlReader
    {
        private StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="reader">An exemplar of <see cref="StreamReader"/> class opened to xml file.</param>
        public FileCabinetRecordXmlReader(StreamReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Reads all data from xml file.
        /// </summary>
        /// <returns>Collection of <see cref="FileCabinetRecord"/>.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<FileCabinetRecordSerializable>), new XmlRootAttribute("records"));

            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            FileCabinetRecord record;

            List<FileCabinetRecordSerializable> deserializedRecords = (List<FileCabinetRecordSerializable>)xmlSerializer.Deserialize(this.reader);

            for (int i = 0; i < deserializedRecords.Count; i++)
            {
                record = new FileCabinetRecord()
                {
                    Id = deserializedRecords[i].Id,
                    FirstName = deserializedRecords[i].Name.FirstName,
                    LastName = deserializedRecords[i].Name.LastName,
                    DateOfBirth = deserializedRecords[i].DateOfBirth,
                    Gender = deserializedRecords[i].Gender,
                    Experience = deserializedRecords[i].Experience,
                    Salary = deserializedRecords[i].Salary,
                };

                records.Add(record);
            }

            return records;
        }
    }
}
