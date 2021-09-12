using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetGenerator
{
    public class Name
    {
        /// <summary>
        /// Gets or sets the first name of person.
        /// </summary>
        /// <value>The first name of person.</value>
        [XmlAttribute("first")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of person.
        /// </summary>
        /// <value>The last name of person.</value>
        [XmlAttribute("last")]
        public string LastName { get; set; }
    }
}
