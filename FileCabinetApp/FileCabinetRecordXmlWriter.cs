using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;

namespace FileCabinetApp
{
    /// <summary>
    /// This class writes records into XML format.
    /// </summary>
    public class FileCabinetRecordXmlWriter
    {
        private XmlWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">The instance of the <see cref="XmlWriter"/> which will be used for writing records.</param>
        public FileCabinetRecordXmlWriter(XmlWriter writer)
        {
            this.writer = writer;
            this.writer.WriteStartDocument();
            this.writer.WriteStartElement("records");
        }

        /// <summary>
        /// Writes one record using an instance of <see cref="XmlWriter"/> class.
        /// </summary>
        /// <param name="record">The instance of the <see cref="FileCabinetRecord"/> class.</param>
        public void Write(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record), $"{nameof(record)} is null.");
            }

            this.writer.WriteStartElement("record");
            this.writer.WriteAttributeString("id", record.Id.ToString(CultureInfo.InvariantCulture));

            this.writer.WriteStartElement("name");
            this.writer.WriteAttributeString("first", record.FirstName);
            this.writer.WriteAttributeString("last", record.LastName);
            this.writer.WriteEndElement();

            this.writer.WriteStartElement("dateOfBirth");
            this.writer.WriteString(record.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            this.writer.WriteEndElement();

            this.writer.WriteStartElement("gender");
            this.writer.WriteString(record.Gender.ToString(CultureInfo.InvariantCulture));
            this.writer.WriteEndElement();

            this.writer.WriteStartElement("experience");
            this.writer.WriteString(record.Experience.ToString(CultureInfo.InvariantCulture));
            this.writer.WriteEndElement();

            this.writer.WriteStartElement("salary");
            this.writer.WriteString(record.Salary.ToString(CultureInfo.InvariantCulture));
            this.writer.WriteEndElement();

            this.writer.WriteEndElement();
        }
    }
}
