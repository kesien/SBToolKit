using Newtonsoft.Json;
using SwitchServiceLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xceed.Document.NET;
using Xceed.Words.NET;
using SBToolKit.SwitchServiceLibrary.Properties;

namespace SwitchServiceLibrary
{
    public class FileService
    {

        public string WordSavePath { get; set; } = Resources.WordSavePath;
        public string ImportFileSavePath { get; set; } = Resources.ImportFileSavePath;
        private Stream _baseDocument;
        private List<LanguageModel> _supportedLanguages;


        public FileService()
        {
            _supportedLanguages = LoadLanguagesFromJSON();
            _baseDocument = LoadBaseDocument();
        }

        /// <summary>
        /// Generate the Word document which will be sent to the teacher.
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public string GenerateWordDoc(Course course)
        {
            string fullCourseNumber = $"{course.Coursenumber}/{DateTime.Now.ToString("yy")}";
            string fileName = $"{course.Coursenumber}_{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.docx";

            using DocX document = DocX.Load(_baseDocument);
            document.ReplaceText("{coursenumber}", fullCourseNumber);
            var table = document.Tables[0];
            table.Design = TableDesign.TableGrid;
            foreach (Participant participant in course.Participants)
            {
                var row = table.InsertRow();
                row.Cells[0].InsertParagraph(participant.Email);
                row.Cells[1].InsertParagraph(participant.Firstname);
                row.Cells[2].InsertParagraph(participant.Lastname);
                row.Cells[3].InsertParagraph("PW4you");
            }
            document.SaveAs(new FileStream(Path.Combine(WordSavePath, fileName), FileMode.Create, FileAccess.Write));
            return new FileInfo(fileName).FullName;
        }

        public string GenerateImportFile(Course course)
        {
            string fullCourseNumber = $"{course.Coursenumber}/{DateTime.Now.ToString("yy")}";
            string fileName = $"{course.Coursenumber}_{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.csv";
            LanguageModel language = _supportedLanguages.Where(language => language.Language == course.Language).FirstOrDefault();
            if (language is null)
            {
                return null;
            }

            return new FileInfo(fileName).FullName;
        }

        /// <summary>
        /// Loads the base Word template
        /// </summary>
        /// <returns></returns>
        private Stream LoadBaseDocument()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream baseDocument = assembly.GetManifestResourceStream("SwitchServiceLibrary.Resources.base_template.docx");
            return baseDocument;
        }
        private List<LanguageModel> LoadLanguagesFromJSON()
        {
            using MemoryStream memoryStream = new MemoryStream(Resources.languages);
            using StreamReader file = new StreamReader(memoryStream);
            string json = file.ReadToEnd();
            List<LanguageModel> languages = JsonConvert.DeserializeObject<List<LanguageModel>>(json);
            return languages;
        }
    }
}
