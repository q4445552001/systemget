using System;
using System.Windows.Forms;
using System.Threading;

namespace systemget
{
    public partial class Form2 : Form
    {
        Thread loading,gif;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            loading.Abort();
            gif.Abort();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.pictureBox1.Image = Properties.Resources.cover_loading;
            
            loading = new Thread(() =>
            {
                while (true)
                {
                    label1.Text = "取得資源中，請稍後";
                    Thread.Sleep(1000);
                    for (int i = 0; i <= 2; i++)
                    {
                        label1.Text += ".";
                        Thread.Sleep(1000);
                    }
                }
            });

            gif = new Thread(() =>
            {
                while (true)
                {
                    pictureBox1.Refresh();
                    Thread.Sleep(100);
                }
            });

            loading.Start();
            gif.Start();
        }
        
    }
}
