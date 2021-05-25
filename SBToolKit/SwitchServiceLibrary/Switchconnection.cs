using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Collections.Specialized;
using SwitchServiceLibrary.Exceptions;

namespace SwitchServiceLibrary
{
    public sealed class Switchconnection : ISwitchconnection
    {
        private HttpClientHandler _handler;
        private HttpClient _client;

        public Uri BaseAddress { get; init; } = new Uri("https://switch.flyingteachers.com");

        #region Default Constructor
        public Switchconnection()
        {
            _handler = new HttpClientHandler() { UseCookies = true, CookieContainer = new CookieContainer() };
            _client = new HttpClient(_handler) { BaseAddress = BaseAddress };
        }
        #endregion

        #region Login
        /// <summary>
        /// Login to the switchboard system.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        public async Task LoginAsync(string username, string password)
        {
            FormUrlEncodedContent content = new(new[]
            {
                new KeyValuePair<string, string>("usr", username),
                new KeyValuePair<string, string>("pwd", password),
            });

            HttpResponseMessage result = await _client.PostAsync("fl-teach/main.php", content);
            string response = await result.Content.ReadAsStringAsync();
            if (result.StatusCode != HttpStatusCode.OK || response.Contains("Invalid username or password"))
            {
                throw new InvalidUsernameOrPasswordException("Invalid username or password!");
            }
        }
        #endregion

        #region GenerateExcelList
        /// <summary>
        /// Send a GET request to generate the excel list.
        /// </summary>
        /// <param name="courseNumbers">List of coursenumbers</param>
        /// <returns>The response as string. E.g.: 'documents/kursteilnehmer_6208.xls'</returns>
        private async Task<string> GenerateExcelListAsync(List<string> courseNumbers)
        {
            UriBuilder builder = new(_client.BaseAddress + "fl-teach/mkReport.php");
            builder.Port = -1;
            NameValueCollection query = HttpUtility.ParseQueryString(builder.Query);
            query["rType"] = "5";
            query["lesson_id"] = string.Join(',', courseNumbers);
            query["output"] = "2";
            builder.Query = query.ToString();
            return await _client.GetStringAsync(builder.ToString());
        }
        #endregion

        #region DownloadExcelFile
        /// <summary>
        /// Download the generated Excel list.
        /// </summary>
        /// <param name="courseNumbers">List of coursenumbers</param>
        /// <returns></returns>
        public async Task<Stream> DownloadListAsync(params string[] courseNumbers)
        {
            List<string> validCourseNumbers = courseNumbers.Where(cn => cn.Length == 5 && int.TryParse(cn, out _)).ToList();
            string path = await GenerateExcelListAsync(validCourseNumbers);
            var response = await _client.GetAsync($"/cgi-bin/flshow.exe?doc={path}");
            string content = await response.Content.ReadAsStringAsync();
            if (content.Contains("invalid document id"))
            {
                throw new InvalidDocumentIdException("Invalid document id");
            }
            return await response.Content.ReadAsStreamAsync();
        }
        #endregion
    }
}