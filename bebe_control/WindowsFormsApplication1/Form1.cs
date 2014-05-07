using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
  
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.KeyPreview = true;
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
        }

        private void button1_Click(object sender, EventArgs e)
        {
           ArduinoControllerMain asd = new ArduinoControllerMain();

           asd.SetComPort(1,234,1,234);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ArduinoControllerMain asd = new ArduinoControllerMain();

            asd.SetComPort(1, 0, 1, 0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ArduinoControllerMain asd = new ArduinoControllerMain();
            asd.SetComPort(0, 234, 0, 234);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ArduinoControllerMain asd = new ArduinoControllerMain();
            asd.SetComPort(1, 107, 1, 207);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ArduinoControllerMain asd = new ArduinoControllerMain();
            asd.SetComPort(1, 207, 1, 107);
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                e.Handled = false;
                button1.PerformClick();
            }
            if (e.KeyCode == Keys.Space)
            {
                e.Handled = false;
                button2.PerformClick();
            }
            if (e.KeyCode == Keys.S)
            {
                e.Handled = false;
                button3.PerformClick();
            }
            if (e.KeyCode == Keys.A)
            {
                e.Handled = false;
                button4.PerformClick();
            }
            if (e.KeyCode == Keys.D)
            {
                e.Handled = false;
                button5.PerformClick();
            }
        }
    }
    public class ArduinoControllerMain
    {

        SerialPort currentPort;
        bool portFound;
        public void SetComPort(int dirA, int s1, int dirB, int s2)
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                double s2p = 0.87 * s2;
                s2 = (int)Math.Floor(s2p);
                Debug.Print(s2.ToString());
                foreach (string port in ports)
                {
                    currentPort = new SerialPort(port, 9600);
                    if (DetectArduino(dirA, s1/100, s1/10%10, s1%10, dirB, s2/100, s2/10%10, s2%10))
                    {
                        portFound = true;
                        break;
                    }
                    else
                    {
                        portFound = false;
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
        public bool DetectArduino(int dirA, int s11, int s12, int s13, int dirB, int s21, int s22, int s23)
        {
            try
            {
                //The below setting are for the Hello handshake
                byte[] buffer = new byte[8];
                buffer[0] = Convert.ToByte(dirA);
                buffer[1] = Convert.ToByte(s11);
                buffer[2] = Convert.ToByte(s12);
                buffer[3] = Convert.ToByte(s13);
                buffer[4] = Convert.ToByte(dirB);
                buffer[5] = Convert.ToByte(s21);
                buffer[6] = Convert.ToByte(s22);
                buffer[7] = Convert.ToByte(s23);
                int intReturnASCII = 0;
                char charReturnValue = (Char)intReturnASCII;
                currentPort.Open();
                currentPort.Write(buffer, 0, 8);
                Thread.Sleep(100);
                int count = currentPort.BytesToRead;
                string returnMessage = "";
                while (count > 0)
                {
                    intReturnASCII = currentPort.ReadByte();
                    returnMessage = returnMessage + Convert.ToChar(intReturnASCII);
                    count--;
                }
                //ComPort.name = returnMessage;
                currentPort.Close();
                if (returnMessage.Contains("HELLO FROM ARDUINO"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
