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
            // ��վɵĴ����б�
            comboBox1.Items.Clear();

            trackBar1.Value = 3;

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

        int ITV = 3;
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

            //pictureBox7.Visible = false;
            //pictureBox1.Visible = true;
            ITV = trackBar1.Value;
            SEG_TIMER.Interval = ITV * 1000; // ���ö�ʱ�����Ϊ1��

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
                label4.Text = "������ʹ��ʱ�䵽����������������ƣ�";
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
