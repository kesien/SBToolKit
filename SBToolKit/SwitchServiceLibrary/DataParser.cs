using SwitchServiceLibrary.Models;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;

namespace SwitchServiceLibrary
{
    public static class DataParser
    {
        private static DataSet XMLToDataSet(Stream dataStream)
        {
            DataSet ds = new DataSet();

            XmlDocument xml = new XmlDocument();
            xml.Load(dataStream);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
            nsmgr.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");
            XmlElement root = xml.DocumentElement;

            foreach (XmlNode node in root.SelectNodes("//ss:Worksheet", nsmgr))
            {
                DataTable dt = new DataTable(node.Attributes["ss:Name"].Value);
                ds.Tables.Add(dt);
                XmlNodeList rows = node.SelectNodes("ss:Table/ss:Row", nsmgr);
                if (rows.Count > 0)
                {
                    foreach (XmlNode data in rows[0].SelectNodes("ss:Cell/ss:Data", nsmgr))
                    {
                        dt.Columns.Add(data.InnerText, typeof(string));
                    }

                    for (int i = 1; i < rows.Count; i++)
                    {
                        var cells = rows[i].SelectNodes("ss:Cell/ss:Data", nsmgr);
                        DataRow row = dt.NewRow();
                        int columnIndex = 0;
                        foreach (XmlNode cell in cells)
                        {
                            row[columnIndex] = cell.InnerText;
                            columnIndex++;
                        }
                        dt.Rows.Add(row);
                    }
                }
            }

            return ds;
        }

        public static List<CourseModel> GetData(Stream dataStream)
        {
            var dataSet = XMLToDataSet(dataStream).Tables[0].AsEnumerable();

            List<ParticipantModel> participants = dataSet.Select(datarow => new ParticipantModel
            {
                ID = datarow.Field<string>("person_ID"),
                Firstname = datarow.Field<string>("Vorname"),
                Lastname = datarow.Field<string>("Nachname"),
                Email = datarow.Field<string>("EMail"),
                Coursenumber = datarow.Field<string>("Kursnummer")
            }).ToList();

            HashSet<string> courseNumbers = new();
            List<CourseModel> courses = new();

            foreach (var datarow in dataSet)
            {
                string cn = datarow.Field<string>("Kursnummer");
                if (!courseNumbers.Contains(cn))
                {
                    courses.Add(new CourseModel()
                    {
                        Coursenumber = cn,
                        Language = datarow.Field<string>("Sprache"),
                        Level = datarow.Field<string>("Niveau"),
                        Participants = participants.Where(participant => participant.Coursenumber == cn).ToList()
                    });
                    courseNumbers.Add(cn);

                }
            }

            return courses;
        }
    }
}
