using System;
using System.Data;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Minecraft_Username_Checker
{
    public partial class MainForm : Form
    {
        private readonly DataTable _table;

        public MainForm()
        {
            InitializeComponent();
            _table = new DataTable();
            _table.Columns.Add("Username", typeof(string));
            _table.Columns.Add("UUID", typeof(string));
            _table.Columns.Add("Timestamp", typeof(string));
            _table.Columns.Add("Error", typeof(string));
            dataGridView.DataSource = _table;
            dataGridView.RowHeadersVisible = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.AllowUserToResizeColumns = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.AllowUserToOrderColumns = false;
        }

        private void CommitSearch(object sender, EventArgs e)
        {
            var url = "https://api.mojang.com/profiles/minecraft";
            var json = $"[\"{searchTextBox.Text}\"]";
            var response = PostResponse(json, url);
            var jToken = JToken.Parse("{\"error\": \"ParseException\",\"errorMessage\": \"Empty String.\"}");
            var newRow = _table.NewRow();
            try
            {
                jToken = JToken.Parse(response);
            }
            catch (JsonReaderException)
            {
                newRow["error"] = jToken["error"] + " | " + jToken["errorMessage"];
                newRow["Username"] = searchTextBox.Text + " (null)";
                newRow["UUID"] = "(null)";
                newRow["timestamp"] = "(null)";
                _table.Rows.Add(newRow);
                return;
            }

            if (response.Equals("{\"error\": \"InternetException\",\"errorMessage\": \"No Internet.\"}"))
            {
                newRow["error"] = "No Internet Connection";
                newRow["Username"] = searchTextBox.Text + " (null)";
                newRow["UUID"] = "(null)";
                newRow["timestamp"] = "(null)";
                _table.Rows.Add(newRow);
                return;
            }

            if (!jToken.HasValues)
            {
                try
                {
                    response = ObtainResponse($"https://api.mojang.com/users/profiles/minecraft/{searchTextBox.Text}?at=0");
                    jToken = JToken.Parse(response);
                }
                catch (Exception)
                {
                    if (string.IsNullOrEmpty(response))
                        newRow["error"] = "Username not found.";
                    else
                        newRow["error"] = "Invalid server response.";

                    _table.Rows.Add(newRow);
                    return;
                }

                if (jToken["error"] != null)
                {
                    newRow["error"] = jToken["error"] + " | " + jToken["errorMessage"];
                    newRow["Username"] = searchTextBox.Text + " (null)";
                    newRow["UUID"] = "(null)";
                    newRow["timestamp"] = "(null)";
                    _table.Rows.Add(newRow);
                    return;
                }

                newRow["Username"] = jToken["name"];
                newRow["UUID"] = jToken["id"];
                if (jToken["legacy"] != null)
                {
                    _table.Columns.Add("Legacy", typeof(bool));
                    newRow["Legacy"] = true;
                }
                else if (_table.Columns.Contains("Legacy"))
                {
                    newRow["Legacy"] = false;
                }

                if (jToken["legacy"] != null)
                {
                    _table.Columns.Add("Demo", typeof(bool));
                    newRow["Demo"] = true;
                }
                else if (_table.Columns.Contains("Demo"))
                {
                    newRow["Demo"] = false;
                }

                _table.Rows.Add(newRow);
                url = $"https://api.mojang.com/user/profiles/{jToken["id"]}/names";
            }
            else
            {
                if (jToken[0]["error"] != null)
                {
                    newRow["error"] = jToken["error"] + " | " + jToken["errorMessage"];
                    newRow["Username"] = searchTextBox.Text + " (null)";
                    newRow["UUID"] = "(null)";
                    newRow["timestamp"] = "(null)";
                    _table.Rows.Add(newRow);
                    return;
                }

                newRow["Username"] = jToken[0]["name"];
                newRow["UUID"] = jToken[0]["id"];
                if (jToken[0]["legacy"] != null)
                {
                    _table.Columns.Add("Legacy", typeof(bool));
                    newRow["Legacy"] = true;
                }
                else if (_table.Columns.Contains("Legacy"))
                {
                    newRow["Legacy"] = false;
                }

                if (jToken[0]["legacy"] != null)
                {
                    _table.Columns.Add("Demo", typeof(bool));
                    newRow["Demo"] = true;
                }
                else if (_table.Columns.Contains("Demo"))
                {
                    newRow["Demo"] = false;
                }

                _table.Rows.Add(newRow);
                url = $"https://api.mojang.com/user/profiles/{jToken[0]["id"]}/names";
            }

            response = ObtainResponse(url);
            jToken = JToken.Parse(response);
            newRow = _table.NewRow();
            if (jToken.SelectToken("error") != null)
            {
                newRow["error"] = jToken["error"] + " | " + jToken["errorMessage"];
                newRow["Username"] = searchTextBox.Text + " (null)";
                newRow["UUID"] = "(null)";
                newRow["timestamp"] = "(null)";
                _table.Rows.Add(newRow);
                return;
            }

            foreach (var item in jToken)
            {
                newRow = _table.NewRow();
                newRow["username"] = item["name"] + " (history)";
                if (item["changedToAt"] != null)
                    newRow["timestamp"] = UnixTimeStampToDateTime((double)item["changedToAt"]);

                Console.WriteLine(item);
                Console.WriteLine(item["name"]);
                _table.Rows.Add(newRow);
            }
        }

        /// <summary>
        /// Sends a request to the given URL.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="WebException">Thrown when the server returned an error.</exception>
        private static string ObtainResponse(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            string html;
            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream ?? throw new WebException()))
            {
                html = reader.ReadToEnd();
            }

            return html;
        }

        private static string PostResponse(string json, string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream() ?? throw new WebException()))
            {
                var result = streamReader.ReadToEnd();
                return result;
            }
        }

        private void SearchCancelled(object sender, EventArgs e)
        {
            _table.Rows.Clear();
            if (_table.Columns.Contains("Legacy")) _table.Columns.Remove("Legacy");

            if (_table.Columns.Contains("Demo")) _table.Columns.Remove("Demo");
        }

        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}