using System;
using System.IO.Ports;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading;

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

        private Thread serialReadThread;
        private volatile bool isSerialReaderActive = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            label4.Visible = false;
            // ��վɵĴ����б�
            comboBox1.Items.Clear();

            trackBar1.Value = 2;

            // ��ȡ���п��ô�������
            string[] ports = SerialPort.GetPortNames();

            if (ports.Length > 0)
            {

                // ��ӵ���������
                foreach (string port in ports)
                {
                    comboBox1.Items.Add(port);
                }

                // �Զ�ѡ���һ������ѡ��
                comboBox1.SelectedIndex = 0;
            }

            APP_TIMER.Start();
        }

        int ITV = 2;
        private void button1_Click(object sender, EventArgs e)
        {

            if (serialPort != null && serialPort.IsOpen)
            {
                ;
            }
            else
            {
                MessageBox.Show("����δ�򿪣�");
                return;
            }

            serialPort.DiscardInBuffer();  // ������ջ��棨���뻺������
            serialPort.DiscardOutBuffer(); // ����ѡ��������ͻ���

            //pictureBox7.Visible = false;
            //pictureBox1.Visible = true;
            ITV = trackBar1.Value;
            SEG_TIMER.Interval = ITV * 1000; // ���ö�ʱ�����Ϊ1��
            button1.BackColor = Color.Green;
            SEG_TIMER.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // ��վɵĴ����б�
                comboBox1.Items.Clear();

                // ��ȡ���п��ô�������
                string[] ports = SerialPort.GetPortNames();

                if (ports.Length == 0)
                {
                    MessageBox.Show("δ��⵽�����豸��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // ��ӵ���������
                foreach (string port in ports)
                {
                    comboBox1.Items.Add(port);
                }

                // �Զ�ѡ���һ������ѡ��
                comboBox1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("��������ʧ��: " + ex.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("����δ�򿪣�");
                return;
            }

            serialPort.DiscardInBuffer();  // ������ջ��棨���뻺������
            serialPort.DiscardOutBuffer(); // ����ѡ��������ͻ���

            SEG_COUNT = 0;
            serialPort.Write("SHOW_000\r\n");       // �����ַ���
            System.Threading.Thread.Sleep(200);     // ��ʱ200����
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
            button1.Text = "��ʼ���������";
            label1.Text = "���Խ���";
            button1.BackColor = Color.FromArgb(0, 174, 239);
        }

        string cmd = string.Empty;

        private void SEG_TIMER_Tick(object sender, EventArgs e)
        {
            cmd = "SHOW_" + SEG_COUNT.ToString("X3") + "\r\n";

            button1.Text = "���Խ�����";

            try
            {
                if (serialPort != null && serialPort.IsOpen)
                {
                    //string data = textBox1.Text;
                    serialPort.Write(cmd);  // �����ַ���
                    //MessageBox.Show("���ͳɹ���");
                }
                else
                {
                    MessageBox.Show("����δ�򿪣�");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ��: " + ex.Message);
            }


            label1.Text = "��ǰ��ʾ�����: " + SEG_COUNT.ToString("");

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

            if (button3.Text == "���ӳɹ�")
            {
                serialPort.Close();
                button3.Text = "���Ӳ��԰�";
                button3.BackColor = Color.FromArgb(0, 174, 239);
                return;
            }
            else
            {

                try
                {
                    if (serialPort.IsOpen)
                    {
                        button3.BackColor = System.Drawing.Color.Green;
                        MessageBox.Show("���������ӡ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // �� comboBox1 �л�ȡ������
                    string? selectedPort = comboBox1.SelectedItem?.ToString();
                    if (string.IsNullOrEmpty(selectedPort))
                    {
                        MessageBox.Show("����ѡ��һ�����ڣ�", "����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // ��ʼ�����ڲ����������ʵȿɰ������ã�
                    serialPort.BaudRate = 9600;
                    serialPort.DataBits = 8;
                    serialPort.Parity = Parity.None;
                    serialPort.StopBits = StopBits.One;
                    serialPort.PortName = selectedPort;

                    serialPort.Open();

                    System.Threading.Thread.Sleep(100); // ��ʱ100����

                    serialPort.Write("CONN_REQ\r\n");       // �����ַ���

                    System.Threading.Thread.Sleep(500); // ��ʱ100����

                    try
                    {
                        if (serialPort.BytesToRead >= 10)
                        {
                            byte[] buffer = new byte[10];
                            serialPort.Read(buffer, 0, 10);  // ��ȡ10���ֽ�

                            string hexString = Encoding.ASCII.GetString(buffer);

                            if (hexString == "CONN_ACK\r\n")
                            {

                                button3.Text = "���ӳɹ�";
                                button3.BackColor = System.Drawing.Color.Green;
                            }
                            else
                            {
                                this.Invoke(new Action(() =>
                                {
                                    MessageBox.Show("���Ӳ��԰�ʧ��");
                                }));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show("�������ݳ���" + ex.Message);
                        }));
                    }
                    //MessageBox.Show("�������ӳɹ���", "�ɹ�", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("��������ʧ�ܣ�" + ex.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            ITV = trackBar1.Value;
            SEG_TIMER.Interval = ITV * 1000; // ���ö�ʱ�����Ϊ1��
        }

        UInt32 APP_USING_COUNT = 0;
        private void APP_TIMER_Tick(object sender, EventArgs e)
        {
            APP_USING_COUNT++;

            //if (APP_USING_COUNT == 120)
            //{
            //    button1.Visible = false;
            //    button2.Visible = false;
            //    button3.Visible = false;
            //    button4.Visible = false;
            //    comboBox1.Visible = false;
            //    label1.Visible = false;
            //    label3.Visible = false;
            //    trackBar1.Visible = false;

            //    label4.Visible = true;
            //    label4.Text = "������ʹ��ʱ�䵽����������������ƣ�";
            //    label4.ForeColor = Color.Red;
            //    //MessageBox.Show();
            //}


            //if (APP_USING_COUNT > 135)
            //{
            //    APP_TIMER.Stop();
            //    Application.Exit();
            //}
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void UpdatePanels(int green_key, int red_key)
        {
            panel8.BackColor = (green_key & (1 << 0)) != 0 ? Color.Green : SystemColors.Control;
            panel9.BackColor = (green_key & (1 << 1)) != 0 ? Color.Green : SystemColors.Control;
            panel4.BackColor = (green_key & (1 << 2)) != 0 ? Color.Green : SystemColors.Control;
            panel5.BackColor = (green_key & (1 << 3)) != 0 ? Color.Green : SystemColors.Control;
            panel3.BackColor = (green_key & (1 << 4)) != 0 ? Color.Green : SystemColors.Control;
            panel6.BackColor = (green_key & (1 << 5)) != 0 ? Color.Green : SystemColors.Control;

            panel14.BackColor = (red_key & (1 << 0)) != 0 ? Color.Tomato : SystemColors.Control;
            panel13.BackColor = (red_key & (1 << 1)) != 0 ? Color.Tomato : SystemColors.Control;
            panel12.BackColor = (red_key & (1 << 2)) != 0 ? Color.Tomato : SystemColors.Control;
            panel11.BackColor = (red_key & (1 << 3)) != 0 ? Color.Tomato : SystemColors.Control;
            panel10.BackColor = (red_key & (1 << 4)) != 0 ? Color.Tomato : SystemColors.Control;
            panel7.BackColor = (red_key & (1 << 5)) != 0 ? Color.Tomato : SystemColors.Control;
        }

        private void ReadSerialDataLoop()
        {
            while (isSerialReaderActive)
            {
                try
                {
                    if (serialPort.BytesToRead >= 9)
                    {
                        byte[] buffer = new byte[9];
                        serialPort.Read(buffer, 0, 9);

                        string response = Encoding.ASCII.GetString(buffer);

                        if (response.Length >= 9)
                        {
                            string hexPart = response.Substring(3, 4); // ����3��ʼȡ4λ

                            int value = Convert.ToInt32(hexPart, 16);
                            int green_key = value & 0x00FF;
                            int red_key = (value >> 8) & 0x00FF;

                            Console.WriteLine("value     : 0x{0:X4} ({1})", value, Convert.ToString(value, 2).PadLeft(16, '0'));
                            Console.WriteLine("green_key : 0x{0:X2} ({1})", green_key, Convert.ToString(green_key, 2).PadLeft(8, '0'));
                            Console.WriteLine("red_key   : 0x{0:X2} ({1})", red_key, Convert.ToString(red_key, 2).PadLeft(8, '0'));

                            this.Invoke(new Action(() =>
                            {
                                UpdatePanels(green_key, red_key);
                            }));
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show("�����߳��쳣��" + ex.Message);
                    }));
                    isSerialReaderActive = false;
                }

                Thread.Sleep(100); // ���������ѯ
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen == false)
            {
                MessageBox.Show("����δ���ӣ��������Ӵ����豸");
                return;
            }

            UpdatePanels(0, 0);
            
            serialPort.DiscardInBuffer();  // ������ջ��棨���뻺������
            serialPort.DiscardOutBuffer(); // ����ѡ��������ͻ���

            button5.BackColor = Color.Green;

            serialPort.Write("READ_PIN\r\n");       // �����ַ���

            System.Threading.Thread.Sleep(300); // ��ʱ100����


            if (!isSerialReaderActive)
            {
                isSerialReaderActive = true;
                serialReadThread = new Thread(ReadSerialDataLoop);
                serialReadThread.IsBackground = true;
                serialReadThread.Start();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen == false)
            {
                MessageBox.Show("����δ���ӣ��������Ӵ����豸");
                return;
            }

            serialPort.DiscardInBuffer();  // ������ջ��棨���뻺������
            serialPort.DiscardOutBuffer(); // ����ѡ��������ͻ���

            serialPort.Write("SET1_RC5\r\n");       // �����ַ���
            System.Threading.Thread.Sleep(100); // ��ʱ100����

            try
            {
                if (serialPort.BytesToRead >= 7)
                {
                    byte[] buffer = new byte[7];
                    serialPort.Read(buffer, 0, 7);

                    string response = Encoding.ASCII.GetString(buffer);

                    if (response.Length >= 7)
                    {
                        string hexPart = response.Substring(3, 2); // ����3��ʼȡ4λ

                        int value = Convert.ToInt32(hexPart, 16);

                        if (value < 3)
                        {
                            panel2.BackColor = Color.Green;

                            if (value == 0)
                            {
                                label5.Text = "SPI���Գɹ�";
                                label5.ForeColor = Color.Green;
                                panel15.BackColor = Color.Green;
                                panel16.BackColor = Color.Green;
                            }
                            else if(value == 2)
                            {
                                panel15.BackColor = SystemColors.Control;
                                panel16.BackColor = Color.Green;
                                label5.Text = "SPI����ʧ��";
                                label5.ForeColor = Color.Red;
                            }
                            else if (value == 1)
                            {
                                panel15.BackColor = Color.Green;
                                panel16.BackColor = SystemColors.Control;
                                label5.Text = "SPI����ʧ��";
                                label5.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            label5.Text = "SPI����ʧ��";
                            label5.ForeColor = Color.Red;
                            panel15.BackColor = SystemColors.Control;
                            panel16.BackColor = SystemColors.Control;
                            panel2.BackColor = SystemColors.Control;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show("�����߳��쳣��" + ex.Message);
                }));
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen == false)
            {
                MessageBox.Show("����δ���ӣ��������Ӵ����豸");
                return;
            }

            serialPort.DiscardInBuffer();  // ������ջ��棨���뻺������
            serialPort.DiscardOutBuffer(); // ����ѡ��������ͻ���

            serialPort.Write("SET0_RC5\r\n");       // �����ַ���
            System.Threading.Thread.Sleep(100); // ��ʱ100����
            //serialPort.Write("SET0_RC5\r\n");       // �����ַ���



            try
            {
                if (serialPort.BytesToRead >= 7)
                {
                    byte[] buffer = new byte[7];
                    serialPort.Read(buffer, 0, 7);

                    string response = Encoding.ASCII.GetString(buffer);

                    if (response.Length >= 7)
                    {
                        string hexPart = response.Substring(3, 2); // ����3��ʼȡ4λ

                        int value = Convert.ToInt32(hexPart, 16);

                        //if (value == 3)
                        {                                                                    
                            label5.Text = "SPIδ����";                                          
                            label5.ForeColor = Color.Black;
                            panel15.BackColor = SystemColors.Control;
                            panel16.BackColor = SystemColors.Control;
                            panel2.BackColor = SystemColors.Control;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show("�����߳��쳣��" + ex.Message);
                }));
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen == false)
            {
                MessageBox.Show("����δ���ӣ��������Ӵ����豸");
                return;
            }

            serialPort.DiscardInBuffer();  // ������ջ��棨���뻺������
            serialPort.DiscardOutBuffer(); // ����ѡ��������ͻ���

            serialPort.Write("STOP_PIN\r\n");       // �����ַ���

            isSerialReaderActive = false;

            UpdatePanels(0, 0);

            button5.BackColor = Color.FromArgb(0, 174, 239);

            if (serialReadThread != null && serialReadThread.IsAlive)
            {
                serialReadThread.Join();
            }

            System.Threading.Thread.Sleep(200); // ��ʱ100����
        }
    }
}
