using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minecraft_Username_Checker
{
    public partial class Form1 : Microsoft.WindowsAPICodePack.Shell.GlassForm
    {
        private DataTable table;
        public Form1()
        {
            InitializeComponent();
            table = new DataTable();
            table.Columns.Add("Username", Type.GetType("System.String"));
            table.Columns.Add("UUID", Type.GetType("System.String"));
            table.Columns.Add("Timestamp", Type.GetType("System.String"));
            table.Columns.Add("Error", Type.GetType("System.String"));
            dataGridView1.DataSource = table;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToOrderColumns = false;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetAeroGlassTransparency();
            ResetAeroGlass();
            ExcludeControlFromAeroGlass(dataGridView1);
            ExcludeControlFromAeroGlass(searchTextBox1);
            searchTextBox1.BackColor = Color.White;
            dataGridView1.BackgroundColor = Color.White;
        }

        private void searchTextBox1_SearchStarted(object sender, EventArgs e)
        {
            string url = string.Format("https://api.mojang.com/profiles/minecraft", searchTextBox1.Text);
            string json = string.Format("[\"{0}\"]", searchTextBox1.Text);
            string response = PostResponse(json, url);
            var jToken = JToken.Parse("{\"error\": \"ParseException\",\"errorMessage\": \"Empty String.\"}");
            DataRow newRow = table.NewRow();
            try
            {
                jToken = JToken.Parse(response);
            }
            catch (JsonReaderException)
            {
                newRow["error"] = jToken["error"] + " | " + jToken["errorMessage"];
                newRow["Username"] = searchTextBox1.Text + " (null)";
                newRow["UUID"] = "(null)";
                newRow["timestamp"] = "(null)";
                table.Rows.Add(newRow);
                return;
            }
            if(response.Equals("{\"error\": \"InternetException\",\"errorMessage\": \"No Internet.\"}"))
            {
                newRow["error"] = "No Internet Connection";
                newRow["Username"] = searchTextBox1.Text + " (null)";
                newRow["UUID"] = "(null)";
                newRow["timestamp"] = "(null)";
                table.Rows.Add(newRow);
                return;
            }
            if (!jToken.HasValues)
            {
                url = string.Format("https://api.mojang.com/users/profiles/minecraft/{0}?at=0", searchTextBox1.Text);
                response = ObtainResponse(url);
                try
                {
                    jToken = JToken.Parse(response);
                }
                catch (JsonReaderException)
                {
                    if (string.IsNullOrEmpty(response))
                    {
                        newRow["error"] = "Username not found.";
                    }
                    else
                    {
                        newRow["error"] = "Invalid server response.";
                    }
                    table.Rows.Add(newRow);
                    return;
                }
                if (jToken["error"] != null)
                {
                    newRow["error"] = jToken["error"] + " | " + jToken["errorMessage"];
                    newRow["Username"] = searchTextBox1.Text + " (null)";
                    newRow["UUID"] = "(null)";
                    newRow["timestamp"] = "(null)";
                    table.Rows.Add(newRow);
                    return;
                }
                newRow["Username"] = jToken["name"];
                newRow["UUID"] = jToken["id"];
                if (jToken["legacy"] != null)
                {
                    table.Columns.Add("Legacy", Type.GetType("System.Boolean"));
                    newRow["Legacy"] = true;
                }
                else if (table.Columns.Contains("Legacy"))
                {
                    newRow["Legacy"] = false;
                }

                if (jToken["legacy"] != null)
                {
                    table.Columns.Add("Demo", Type.GetType("System.Boolean"));
                    newRow["Demo"] = true;
                }
                else if (table.Columns.Contains("Demo"))
                {
                    newRow["Demo"] = false;
                }
                table.Rows.Add(newRow);
                url = string.Format("https://api.mojang.com/user/profiles/{0}/names", jToken["id"]);
            }
            else
            {
                if (jToken[0]["error"] != null)
                {
                    newRow["error"] = jToken["error"] + " | " + jToken["errorMessage"];
                    newRow["Username"] = searchTextBox1.Text + " (null)";
                    newRow["UUID"] = "(null)";
                    newRow["timestamp"] = "(null)";
                    table.Rows.Add(newRow);
                    return;
                }
                newRow["Username"] = jToken[0]["name"];
                newRow["UUID"] = jToken[0]["id"];
                if (jToken[0]["legacy"] != null)
                {
                    table.Columns.Add("Legacy", Type.GetType("System.Boolean"));
                    newRow["Legacy"] = true;
                }
                else if (table.Columns.Contains("Legacy"))
                {
                    newRow["Legacy"] = false;
                }

                if (jToken[0]["legacy"] != null)
                {
                    table.Columns.Add("Demo", Type.GetType("System.Boolean"));
                    newRow["Demo"] = true;
                }
                else if (table.Columns.Contains("Demo"))
                {
                    newRow["Demo"] = false;
                }
                table.Rows.Add(newRow);
                url = string.Format("https://api.mojang.com/user/profiles/{0}/names", jToken[0]["id"]);
            }
            response = ObtainResponse(url);
            jToken = JToken.Parse(response);
            newRow = table.NewRow();
            if (jToken.SelectToken("error")!=null)
            {
                newRow["error"] = jToken["error"] + " | " + jToken["errorMessage"];
                newRow["Username"] = searchTextBox1.Text + " (null)";
                newRow["UUID"] = "(null)";
                newRow["timestamp"] = "(null)";
                table.Rows.Add(newRow);
                return;
            }
            foreach (JToken item in jToken)
            {
                newRow = table.NewRow();
                newRow["username"] = item["name"] + " (history)";
                if (item["changedToAt"] != null)
                {
                    newRow["timestamp"] = UnixTimeStampToDateTime((double)item["changedToAt"]);
                }
                Console.WriteLine(item);
                Console.WriteLine(item["name"]);
                table.Rows.Add(newRow);
            }
        }

        private string ObtainResponse(string url)
        {
            string html = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }
                return html;
            }
            catch (Exception)
            {
                return "{\"error\": \"InternetException\",\"errorMessage\": \"No Internet.\"}";
            }
        }

        private string PostResponse(string json, string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return result;
                }
            }
            catch (Exception)
            {
                return "{\"error\": \"InternetException\",\"errorMessage\": \"No Internet.\"}";
            }
        }

        private void searchTextBox1_SearchCancelled(object sender, EventArgs e)
        {
            table.Rows.Clear();
            if (table.Columns.Contains("Legacy"))
            {
                table.Columns.Remove("Legacy");
            }
            if (table.Columns.Contains("Demo"))
            {
                table.Columns.Remove("Demo");
            }
        }

        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
