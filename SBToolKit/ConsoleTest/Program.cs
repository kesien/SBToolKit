using SwitchService;
using SwitchServiceLibrary;
using SwitchServiceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Switchconnection connection = new();
            await connection.Login("balazs", "iuZt5324");
            var data = await connection.GetCourseData(new List<string> { "60055", "60056" });
            List<CourseModel> courses = DataParser.GetData(data);
            foreach (var course in courses)
            {
                Console.WriteLine(course);
            }
        }
    }
}
