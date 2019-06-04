using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Media;
using System.Threading;


namespace systemget
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form.CheckForIllegalCrossThreadCalls = false;
            new Thread(() =>
            {
                SoundPlayer nico = new SoundPlayer(Properties.Resources.NicoNicoNi);
                nico.Play();
            }).Start();
            
            Form2 f = new Form2();
            f.StartPosition = FormStartPosition.CenterScreen;
            f.Show();

            try
            {
                //作業系統
                ManagementObjectSearcher sys = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
                foreach (ManagementObject mObject in sys.Get())
                    richTextBox1.AppendText("作業系統 : " + mObject["Name"].ToString().Split('|')[0] + "\n");

                //主機板
                ManagementObjectSearcher board = new ManagementObjectSearcher("select * from Win32_baseboard");
                foreach (ManagementObject mObject in board.Get())
                    richTextBox1.AppendText("主機板 : " + mObject["Manufacturer"].ToString() + " " + mObject["Product"].ToString() + "\n");

                //CPU
                ManagementObjectSearcher CPUID = new ManagementObjectSearcher("select * from Win32_Processor");
                foreach (ManagementObject mObject in CPUID.Get())
                    richTextBox1.AppendText("CPU : " + mObject["Name"].ToString() + "\n");


                /*ManagementObjectSearcher dd = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
                ManagementObjectCollection osDetailsCollection = dd.Get();
                foreach (ManagementObject mObject in osDetailsCollection)
                    foreach (PropertyData prop in mObject.Properties)
                        Console.WriteLine("{0}: {1}", prop.Name, prop.Value);*/


                //GPU
                ManagementObjectSearcher GPUID = new ManagementObjectSearcher("select * from Win32_VideoController");
                foreach (ManagementObject mObject in GPUID.Get())
                    richTextBox1.AppendText("顯示卡 : " + mObject["Name"].ToString() + "\n");

                //硬碟
                ManagementObjectSearcher DISK = new ManagementObjectSearcher("select * from Win32_DiskDrive");
                foreach (ManagementObject mObject in DISK.Get())
                    richTextBox1.AppendText("硬碟 : " + mObject["Model"].ToString() + "\n");

                //記憶體
                string outValue = string.Empty;
                ManagementObjectSearcher RAM = new ManagementObjectSearcher("select * from Win32_PhysicalMemory");
                foreach (ManagementObject mObject in RAM.Get())
                {
                    int type = Int32.Parse(mObject["MemoryType"].ToString());
                    switch (type)
                    {
                        case 0: outValue = "Unknown"; break;
                        case 1: outValue = "Other"; break;
                        case 2: outValue = "DRAM"; break;
                        case 3: outValue = "Synchronous DRAM"; break;
                        case 4: outValue = "Cache DRAM"; break;
                        case 5: outValue = "EDO"; break;
                        case 6: outValue = "EDRAM"; break;
                        case 7: outValue = "VRAM"; break;
                        case 8: outValue = "SRAM"; break;
                        case 9: outValue = "RAM"; break;
                        case 10: outValue = "ROM"; break;
                        case 11: outValue = "Flash"; break;
                        case 12: outValue = "EEPROM"; break;
                        case 13: outValue = "FEPROM"; break;
                        case 14: outValue = "EPROM"; break;
                        case 15: outValue = "CDRAM"; break;
                        case 16: outValue = "3DRAM"; break;
                        case 17: outValue = "SDRAM"; break;
                        case 18: outValue = "SGRAM"; break;
                        case 19: outValue = "RDRAM"; break;
                        case 20: outValue = "DDR"; break;
                        case 21: outValue = "DDR-2"; break;
                        case 22: outValue = "DDR2 FB-DIMM"; break;
                        case 24: outValue = "DDR3"; break;
                        case 25: outValue = "FBD2"; break;
                        default: outValue = "Non"; break;
                    }
                    richTextBox1.AppendText("記憶體 : " + outValue + " " + mObject["Speed"] + " " + ((UInt64)mObject["Capacity"] / 1024 / 1024) + " MB\n".ToString());
                }

                //晶片組
                richTextBox1.AppendText("\n晶片組 : \n");
                List<string> termsList = new List<string>();
                ManagementObjectSearcher chip = new ManagementObjectSearcher("select * from Win32_PnPEntity");
                foreach (ManagementObject mObject in chip.Get())
                    if (mObject["Caption"] != null)
                        termsList.Add(mObject["Caption"].ToString());

                foreach (var srstring in termsList)
                {
                    string[] split = srstring.Split(' ');
                    foreach (var row in split)
                        if (row.StartsWith("Chipset"))
                            richTextBox1.AppendText("" + srstring + "\n");
                }
            }
            catch(Exception echo)
            {
                MessageBox.Show(echo.ToString());
            }

            f.Close();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string line in richTextBox1.Lines)
                sb.AppendLine(line);

            Clipboard.SetText(sb.ToString());

            //Clipboard.SetText(string.Join(Environment.NewLine, listBox1.Items.OfType<string>().ToArray()));
            Show("內容複製完成");
        }

        public static void Show(string MessageText)
        {
            MessageBox.Show(MessageText, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void splitContainer1_Paint(object sender, PaintEventArgs e)
        {
            splitContainer1.SplitterDistance = splitContainer1.Width - splitContainer1.SplitterWidth;
        }
    }
}
