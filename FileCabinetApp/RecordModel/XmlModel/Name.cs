using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetApp.RecordModel.XmlModel
{
    /// <summary>
    /// This class represents xml model of name element.
    /// </summary>
    public class Name
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        [XmlAttribute("first")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        [XmlAttribute("last")]
        public string LastName { get; set; }
    }
}
