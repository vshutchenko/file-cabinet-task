using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    public class FileCabinetRecordXmlReader
    {
        private StreamReader reader;

        public FileCabinetRecordXmlReader(StreamReader reader)
        {
            this.reader = reader;
        }

        public IList<FileCabinetRecord> ReadAll()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<FileCabinetRecordSerializable>), new XmlRootAttribute("records"));

            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            FileCabinetRecord record;

            List<FileCabinetRecordSerializable> deserializedRecords = (List<FileCabinetRecordSerializable>)xmlSerializer.Deserialize(reader);

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
