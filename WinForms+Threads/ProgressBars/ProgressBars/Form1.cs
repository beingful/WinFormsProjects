using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ProgressBars
{
    public partial class Form1 : Form
    {
        private Random _rand;
        private List<ProgressBar> _progressBars;
        private List<int> _filledBars;

        public Form1()
        {
            _rand = new Random();
            _filledBars = new List<int>();

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int barsCount = Convert.ToInt32(textBox1.Text);
            int yCoord = 150;
            int xCoord = 150;

            _progressBars = new List<ProgressBar>();

            for (int i = 0; i < barsCount; i++)
            {
                _progressBars.Add(new ProgressBar
                {
                    Location = new System.Drawing.Point(xCoord, yCoord),
                    Width = 150,
                    Maximum = 1000,
                    ForeColor = Color.FromArgb(_rand.Next(0, 150), _rand.Next(0, 150), _rand.Next(0, 150))
                });

                this.Controls.Add(_progressBars[i]);

                if (yCoord + _progressBars[i].Height > this.Size.Height)
                {
                    yCoord = 150;
                    xCoord += 170;
                }
                else
                {
                    yCoord += 50;
                }
            }

            Refresh();

            ThreadPool.QueueUserWorkItem(
                unused =>
                {
                    int filledCount = 0;
                    int curProcess;

                    while (filledCount < barsCount)
                    {
                        curProcess = _rand.Next(0, barsCount);

                        while (IsFilled(curProcess))
                        {
                            filledCount += FilledProgressBars(curProcess);
                            curProcess = _rand.Next(0, barsCount);
                        }

                        FillProgressBar(curProcess);
                    }
                }
            );
        }

        private bool IsFilled(int curProcess)
        {
            if (_progressBars[curProcess].Value < _progressBars[curProcess].Maximum)
            {
                return false;
            }

            return true;
        }

        private int FilledProgressBars(int curProcess)
        {
            if (_filledBars.FindIndex(elem => elem == curProcess) is -1)
            {
                ThreadPool.QueueUserWorkItem(
                    place =>
                    {
                        listBox1.Invoke(new Action(() => listBox1.Items.Add($"{curProcess} hourse")));
                    }
                    );

                _filledBars.Add(curProcess);

                return 1;
            }

            return 0;
        }

        private void FillProgressBar(int curProcess)
        {
            _progressBars[curProcess].Invoke(new Action(() =>
            {
                  Thread.Sleep(15);
                  _progressBars[curProcess].Increment(_rand.Next(0, 10));
            }));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text is "")
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }
    }
}
