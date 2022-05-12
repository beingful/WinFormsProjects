using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace FindWord
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var rf = new StreamReader(textBox2.Text);
            string findWord = textBox1.Text.ToLower();
            int repeat = 0;

            Action action = () =>
            {
                string[] buff;

                while (!rf.EndOfStream)
                {
                    buff = rf.ReadLine().ToLower().Split(',', '.', '!', ' ', ';', ':', '-').ToArray();

                    ThreadPool.QueueUserWorkItem(
                        find =>
                        {
                            buff = buff.Where(word => word == findWord).ToArray();
                            repeat += buff.Length;
                        });

                    Thread.Sleep(100);
                }
            };

            action.BeginInvoke(ar =>
            {
                Action act = (Action)ar.AsyncState;

                act.EndInvoke(ar);
                rf.Close();

                MessageBox.Show($"Repetitions number: {repeat}", findWord, MessageBoxButtons.OK, MessageBoxIcon.None);
            }, 
            action);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = dlg.FileName;
            }
        }
    }
}
