using SwitchService;
using SwitchServiceLibrary;
using SwitchServiceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            List<DataRequest> request = new List<DataRequest> 
            {
                new DataRequest { Coursenumber = "60055", Ebook = true, Homework = true, Cloudlearning = true },
                new DataRequest { Coursenumber = "60056", Ebook = false, Homework = false, Cloudlearning = true },
            };


            Switchconnection connection = new();
            await connection.Login("balazs", "iuZt5324");

            List<string> courses = request.Select(request => request.Coursenumber).ToList();
            var data = await connection.GetCourseData(courses);
            List<CourseModel> response = DataParser.GetData(data);
            foreach (var course in response)
            {
                Console.WriteLine(course);
            }
        }
    }
}
