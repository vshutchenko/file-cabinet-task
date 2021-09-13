using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    [XmlType("record")]
    public class FileCabinetRecordSerializable
    {
        /// <summary>
        /// Gets or sets the record id.
        /// </summary>
        /// <value>The record id.</value>
        [XmlAttribute("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the full name of person.
        /// </summary>
        /// <value>The full name of person.</value>
        [XmlElement("name")]
        public Name Name { get; set; }

        /// <summary>
        /// Gets or sets the person's date of birth.
        /// </summary>
        /// <value>The person's date of birth.</value>
        [XmlElement("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the gender of the person.
        /// </summary>
        /// <value>The gender of the person. 'M' - male, 'F' - female.</value>
        [XmlElement("gender")]
        public char Gender { get; set; }

        /// <summary>
        /// Gets or sets the experience of person.
        /// </summary>
        /// <value>The person's experience in work.</value>
        [XmlElement("experience")]
        public short Experience { get; set; }

        /// <summary>
        /// Gets or sets the salary of the person.
        /// </summary>
        /// <value>The salary of the person.</value>
        [XmlElement("salary")]
        public decimal Salary { get; set; }
    }
}
