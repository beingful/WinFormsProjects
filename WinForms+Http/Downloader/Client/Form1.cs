using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Net;

namespace Client
{
    public partial class Form1 : Form
    {
        private int _x = 270;
        private int _y = 30;
        private List<ProgressBar> _progressBars;
        private List<WebClient> _clients;
        private List<Label> _labels;

        public Form1()
        {
            _progressBars = new List<ProgressBar>();
            _clients = new List<WebClient>();
            _labels = new List<Label>();

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WebClient client = new WebClient();

            string url = urlTextBox.Text;
            string path = pathTextBox.Text;

            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            client.DownloadFileCompleted += Client_DownloadFileCompleted;

            _clients.Add(client);

            CreateTextBox(url);

            _y += 30;

            CreateProgressBar();

            CreateLabel();

            _y += 40;

            client.DownloadFileAsync(new Uri(url), path);
        }

        private void CreateTextBox(string url)
        {
            var tb = new TextBox
            {
                Location = new Point(_x, _y),
                Width = 240,
                Text = url,
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = true
            };

            this.Controls.Add(tb);
        }

        private void CreateProgressBar()
        {
            var progress = new ProgressBar
            {
                Location = new Point(_x, _y),
                Width = 240,
            };

            this.Controls.Add(progress);

            _progressBars.Add(progress);
        }

        private void CreateLabel()
        {
            var label = new Label
            {
                Location = new Point(_x + 250, _y),
                BorderStyle = BorderStyle.FixedSingle
            };

            this.Controls.Add(label);

            _labels.Add(label);
        }

        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            _labels
                    .ElementAt(_clients.IndexOf(sender as WebClient))
                                                                    .Text = "Finished";

            urlTextBox.Text = "";
            pathTextBox.Text = "";
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            _progressBars
                        .ElementAt(_clients.IndexOf(sender as WebClient))
                                                                        .Value = e.ProgressPercentage;

            _labels
                .ElementAt(_clients.IndexOf(sender as WebClient))
                                                               .Text =
                                    $"Received: {Math.Round((double)e.BytesReceived / e.TotalBytesToReceive, 2) * 100}%";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    pathTextBox.Text = sfd.FileName;
                }
            }
        }
    }
}
