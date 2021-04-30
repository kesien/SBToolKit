using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace SwitchService
{
    public static class XMLParser
    {
        #region Private methods
        /// <summary>
        /// Get the necessary info out of the raw data.
        /// </summary>
        /// <param name="rawData">A list of dictionaries containing all parsed data from the excel file.</param>
        /// <returns>A list of ParticipantModel.</returns>
        private static List<Participant> GetParticipants(List<Dictionary<string, string>> rawData)
        {
            List<Participant> participantList = new();
            foreach (var participantData in rawData)
            {
                participantList.Add(new Participant
                {
                    FirstName = participantData["Vorname"],
                    LastName = participantData["Nachname"],
                    Email = participantData["EMail"]
                });
            }
            return participantList;
        }

        /// <summary>
        /// Strip the sublevel.
        /// </summary>
        /// <param name="fullLevel">The full level of the course. E.g.: A1/2</param>
        /// <returns>The level without the sublevel.</returns>
        private static string GetLevel(string fullLevel)
        {
            if (fullLevel == string.Empty)
            {
                return "unknown";
            }
            return fullLevel.Split('/')[0];
        }
        

        
        /// <summary>
        /// Parse the downloaded excel (xml) file.
        /// </summary>
        /// <param name="filepath">Path to the Excel file.</param>
        /// <returns>A list of dictionaries with all participants data.</returns>
        private static List<Dictionary<string, string>> ParseXML(Stream xmlStream)
        {
            List<string> headerList = new();
            List<Dictionary<string, string>> courseParticipants = new();
            
            XmlDocument xml = new XmlDocument();
            xml.Load(xmlStream);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
            nsmgr.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");
            XmlElement root = xml.DocumentElement;

            // All rows
            XmlNodeList rows = root.SelectNodes("//ss:Row", nsmgr);

            // Header row
            XmlNodeList headers = root.SelectNodes("//ss:Row[@ss:StyleID='s27']/ss:Cell/ss:Data", nsmgr);
            foreach (XmlNode header in headers)
            {
                header.At
                headerList.Add(header.InnerText);
            }

            for (int i = 1; i < rows.Count; i++)
            {
                // All cells inside the row
                XmlNodeList cells = rows[i].SelectNodes("ss:Cell/ss:Data", nsmgr);
                List<string> participantData = new();

                foreach (XmlNode cell in cells)
                {
                    participantData.Add(cell.InnerText);
                }

                // Create a new dictionary with the header and the cell data
                Dictionary<string, string> pData = headerList.Zip(participantData, (k, v) => new { Key = k, Value = v })
                                     .ToDictionary(x => x.Key, x => x.Value);
                courseParticipants.Add(pData);
            }
            xmlStream.Dispose();
            return courseParticipants;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Parse the XML and get the necessary data.
        /// </summary>
        /// <param name="xmlFile">FileInfo object of the downloaded excel file.</param>
        /// <returns>A CourseModel object.</returns>
        public static Course GetCourseData(Stream xmlStream)
        {
            List<Dictionary<string, string>> rawData = ParseXML(xmlStream);
            if (rawData.Count == 0)
            {
                return null;
            }
            string courseNumber = rawData[0]["Kursnummer"];
            string language = rawData[0]["Sprache"];
            string level = GetLevel(rawData[0]["Niveau"]);
            List<Participant> participants = GetParticipants(rawData);
            return new Course { CourseNumber = courseNumber, Language = language, Level = level, Participants = participants };
        }
        #endregion
    }
}
