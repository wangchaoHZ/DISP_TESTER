using System;
using System.IO.Ports;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DISP_TESTER
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        UInt16 SEG_COUNT = 1;

        SerialPort serialPort = new SerialPort();

        private void Form1_Load(object sender, EventArgs e)
        {
            label4.Visible = false;
            // 清空旧的串口列表
            comboBox1.Items.Clear();

            trackBar1.Value = 3;

            // 获取所有可用串口名称
            string[] ports = SerialPort.GetPortNames();

            if (ports.Length > 0)
            {

                // 添加到下拉框中
                foreach (string port in ports)
                {
                    comboBox1.Items.Add(port);
                }

                // 自动选择第一个（可选）
                comboBox1.SelectedIndex = 0;
            }

            APP_TIMER.Start();
        }

        int ITV = 3;
        private void button1_Click(object sender, EventArgs e)
        {

            if (serialPort != null && serialPort.IsOpen)
            {
                ;
            }
            else
            {
                MessageBox.Show("串口未打开！");
                return;
            }

            //pictureBox7.Visible = false;
            //pictureBox1.Visible = true;
            ITV = trackBar1.Value;
            SEG_TIMER.Interval = ITV * 1000; // 设置定时器间隔为1秒

            SEG_TIMER.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // 清空旧的串口列表
                comboBox1.Items.Clear();

                // 获取所有可用串口名称
                string[] ports = SerialPort.GetPortNames();

                if (ports.Length == 0)
                {
                    MessageBox.Show("未检测到串口设备。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 添加到下拉框中
                foreach (string port in ports)
                {
                    comboBox1.Items.Add(port);
                }

                // 自动选择第一个（可选）
                comboBox1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("串口搜索失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                ;
            }
            else
            {
                MessageBox.Show("串口未打开！");
                return;
            }

            SEG_COUNT = 0;
            serialPort.Write("SHOW_000\r\n");       // 发送字符串
            System.Threading.Thread.Sleep(200);     // 延时200毫秒
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            pictureBox5.Visible = false;
            pictureBox6.Visible = false;
            pictureBox7.Visible = true;
            pictureBox8.Visible = true;
            pictureBox9.Visible = true;
            pictureBox10.Visible = true;
            pictureBox11.Visible = true;
            pictureBox12.Visible = true;
            SEG_TIMER.Stop();
            button1.Text = "开始测试数码管";
            label1.Text = "测试进度";
        }

        string cmd = string.Empty;

        private void SEG_TIMER_Tick(object sender, EventArgs e)
        {
            cmd = "SHOW_" + SEG_COUNT.ToString("X3") + "\r\n";

            button1.Text = "测试进行中";

            try
            {
                if (serialPort != null && serialPort.IsOpen)
                {
                    //string data = textBox1.Text;
                    serialPort.Write(cmd);  // 发送字符串
                    //MessageBox.Show("发送成功！");
                }
                else
                {
                    MessageBox.Show("串口未打开！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发送失败: " + ex.Message);
            }


            label1.Text = "当前显示数码管: " + SEG_COUNT.ToString("");

            if (SEG_COUNT == 1)
            {
                pictureBox6.Visible = false;
                pictureBox12.Visible = true;
                //SEG_COUNT = 1;
                pictureBox1.Visible = true;
                pictureBox7.Visible = false;
            }

            if (SEG_COUNT == 2)
            {
                pictureBox1.Visible = false;
                pictureBox7.Visible = true;
                //SEG_COUNT = 2;
                pictureBox2.Visible = true;
                pictureBox8.Visible = false;
            }

            if (SEG_COUNT == 3)
            {
                //SEG_COUNT = 3;
                pictureBox2.Visible = false;
                pictureBox8.Visible = true;

                pictureBox3.Visible = true;
                pictureBox9.Visible = false;
            }

            if (SEG_COUNT == 4)
            {
                //SEG_COUNT = 3;
                pictureBox3.Visible = false;
                pictureBox9.Visible = true;

                pictureBox4.Visible = true;
                pictureBox10.Visible = false;
            }

            if (SEG_COUNT == 5)
            {
                //SEG_COUNT = 3;
                pictureBox4.Visible = false;
                pictureBox10.Visible = true;

                pictureBox5.Visible = true;
                pictureBox11.Visible = false;
            }

            if (SEG_COUNT == 6)
            {
                //SEG_COUNT = 3;
                pictureBox5.Visible = false;
                pictureBox11.Visible = true;

                pictureBox6.Visible = true;
                pictureBox12.Visible = false;
            }

            if (SEG_COUNT == 6)
            {
                SEG_COUNT = 0;
            }

            SEG_COUNT++;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (button3.Text == "连接成功")
            {
                serialPort.Close();
                button3.Text = "连接测试板";
                button3.BackColor = Color.FromArgb(0, 174, 239);
                return;
            }
            else
            {

                try
                {
                    if (serialPort.IsOpen)
                    {
                        MessageBox.Show("串口已连接。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // 从 comboBox1 中获取串口名
                    string? selectedPort = comboBox1.SelectedItem?.ToString();
                    if (string.IsNullOrEmpty(selectedPort))
                    {
                        MessageBox.Show("请先选择一个串口！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 初始化串口参数（波特率等可按需设置）
                    serialPort.BaudRate = 9600;
                    serialPort.DataBits = 8;
                    serialPort.Parity = Parity.None;
                    serialPort.StopBits = StopBits.One;
                    serialPort.PortName = selectedPort;

                    serialPort.Open();

                    System.Threading.Thread.Sleep(100); // 延时100毫秒

                    serialPort.Write("CONN_REQ\r\n");       // 发送字符串

                    System.Threading.Thread.Sleep(500); // 延时100毫秒

                    try
                    {
                        if (serialPort.BytesToRead >= 10)
                        {
                            byte[] buffer = new byte[10];
                            serialPort.Read(buffer, 0, 10);  // 读取10个字节

                            string hexString = Encoding.ASCII.GetString(buffer);

                            if (hexString == "CONN_ACK\r\n")
                            {

                                button3.Text = "连接成功";
                                button3.BackColor = System.Drawing.Color.Green;
                            }
                            else
                            {
                                this.Invoke(new Action(() =>
                                {
                                    MessageBox.Show("连接测试板失败");
                                }));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show("接收数据出错：" + ex.Message);
                        }));
                    }
                    //MessageBox.Show("串口连接成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("串口连接失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            ITV = trackBar1.Value;
            SEG_TIMER.Interval = ITV * 1000; // 设置定时器间隔为1秒
        }

        UInt32 APP_USING_COUNT = 0;
        private void APP_TIMER_Tick(object sender, EventArgs e)
        {
            APP_USING_COUNT++;

            if (APP_USING_COUNT == 90)
            {
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
                button4.Visible = false;
                comboBox1.Visible = false;
                label1.Visible = false;
                label3.Visible = false;
                trackBar1.Visible = false;

                label4.Visible = true;
                label4.Text = "软件免费使用时间到，请申请解除软件限制！";
                label4.ForeColor = Color.Red;
                //MessageBox.Show();
            }


            if (APP_USING_COUNT > 100)
            {
                APP_TIMER.Stop();
                Application.Exit();
            }
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
