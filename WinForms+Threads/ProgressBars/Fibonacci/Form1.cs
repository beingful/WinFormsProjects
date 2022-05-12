using System;
using System.Windows.Forms;
using System.Threading;

namespace Fibonacci
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int to = Convert.ToInt32(textBox2.Text);

            ThreadPool.QueueUserWorkItem(
                number =>
                {
                    listBox2.Invoke(new Action(() => listBox2.Items.Add(0)));
                    for (int i = 1, j = 1, k = 1; i <= to; i++)
                    {
                        listBox1.Invoke(new Action(() => listBox1.Items.Add(i)));

                        if ((j += k) <= to)
                        {
                            ThreadPool.QueueUserWorkItem(
                            fibonacci =>
                            {
                                listBox2.Invoke(new Action(() => { listBox2.Items.Add(j); }));
                            });

                            k = j - k;
                        }

                        Thread.Sleep(100);
                    }
                }
            );
        }
    }
}
