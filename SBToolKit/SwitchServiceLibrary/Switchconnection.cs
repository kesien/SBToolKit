using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SwitchService
{
    public class Switchconnection
    {
        private HttpClientHandler _handler;
        private HttpClient _client;

        #region Default Constructor
        public Switchconnection()
        {
            _handler = new HttpClientHandler() { UseCookies = true, CookieContainer = new CookieContainer() };
            _client = new HttpClient(_handler) { BaseAddress = new Uri("https://switch.flyingteachers.com") };
        }
        #endregion

        #region Login
        /// <summary>
        /// Try to login to the switchboard system.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public async Task Login(string username, string password)
        {
            FormUrlEncodedContent content = new (new[]
            {
                new KeyValuePair<string, string>("usr", username),
                new KeyValuePair<string, string>("pwd", password),
            });

            HttpResponseMessage result = await _client.PostAsync("fl-teach/main.php", content);
            string response = await result.Content.ReadAsStringAsync();
            if (result.StatusCode != HttpStatusCode.OK || response.Contains("Invalid username or password")) 
            { 
                throw new HttpRequestException("Invalid username or password!");
            }
        }
        #endregion

        #region GenerateExcelList
        /// <summary>
        /// Send a GET request to generate the excel list.
        /// </summary>
        /// <param name="courseNumbers">List of coursenumbers</param>
        /// <returns>The response as string. E.g.: 'documents/kursteilnehmer_6208.xls'</returns>
        private async Task<string> GenerateExcelList(List<string> courseNumbers)
        {
            UriBuilder builder = new(_client.BaseAddress + "fl-teach/mkReport.php");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["rType"] = "5";
            query["lesson_id"] = string.Join(',', courseNumbers);
            query["output"] = "2";
            builder.Query = query.ToString();
            string requestUri = builder.ToString();
            return await _client.GetStringAsync(requestUri);
        }
        #endregion

        #region DownloadExcelFile
        /// <summary>
        /// Download the Excel List.
        /// </summary>
        /// <param name="courseNumbers">List of coursenumbers</param>
        /// <returns></returns>
        public async Task<Stream> GetCourseData(List<string> courseNumbers)
        {
            string path = await GenerateExcelList(courseNumbers);
            var response = await _client.GetAsync($"/cgi-bin/flshow.exe?doc={path}");
            string content = await response.Content.ReadAsStringAsync();
            if (content.Contains("invalid document id"))
            {
                throw new HttpRequestException("Invalid document ID");
            }
            return await response.Content.ReadAsStreamAsync();
        }
        #endregion
    }
}