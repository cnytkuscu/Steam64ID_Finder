using Newtonsoft.Json;
using Steam64ID_Finder.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Steam64ID_Finder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Root myDeserializedClass;
        private void button1_Click(object sender, EventArgs e)
        {
            //textBox2.Text = GetSteamData();
            if (!textBox1.Text.Contains("https://steamcommunity.com/id/") || textBox1.Text == "https://steamcommunity.com/id/<userName>")
            {
                MessageBox.Show("Please Provide Valid Steam Profile Link !");
            }
            else
            {
                textBox2.Text = GetProfileHTML(textBox1.Text.Replace("https://steamcommunity.com/id/",""));
                Clipboard.SetText(textBox2.Text);
            }

        }


        private string GetSteamData(string steam64ID)
        {
            string profileData = string.Empty;
            // string url = @"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=5C86A414B7A83BD6B3B25B99F592405D&steamids=76561198137937316";
            string url = @"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=5C86A414B7A83BD6B3B25B99F592405D&steamids=" + steam64ID;


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            // request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                profileData = reader.ReadToEnd();
                myDeserializedClass = JsonConvert.DeserializeObject<Root>(profileData);
            }
            return myDeserializedClass.response.players[0].steamid;
        }



        private string GetProfileHTML(string profileName)
        {
            string profileHTML = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://steamcommunity.com/id/"+profileName); // Steam Sayfasına gitti.
            // request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                profileHTML = reader.ReadToEnd();
                string steam64Id = GetBetween(profileHTML, "commentthread_Profile_", "_area");
                return GetSteamData(steam64Id);


            }
            return "";
        }


        public static string GetBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }

        private void textBox2_DoubleClick(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox2.Text);
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox1.ForeColor = Color.Black;
        }
    }
}
