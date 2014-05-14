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
        public Boolean open = true;
        ArduinoControllerMain asd = new ArduinoControllerMain(true);
        int dirA, dirB, speedA, speedB;
       

        public Form1()
        {
            this.KeyPreview = true;
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            asd.SetComPort(1, 0, 1, 0);
            dirA = 1;
            dirB = 1;
            speedA = 100;
            speedB = 100;
        }

        private void button1_Click(object sender, EventArgs e)
        {
           //ArduinoControllerMain asd = new ArduinoControllerMain(true);
            int speedA_new = 0;
            int speedB_new = 0;
            speedA += 50;
            speedB += 50;
            if (speedA > 0)
            {
                dirA = 1;
                speedA_new = speedA;
            }
            else
            {
                dirA = 0;
                speedA_new = -speedA;
            }
            if (speedB > 0)
            {
                dirB = 1;
                speedB_new = speedB;
            }
            else
            {
                dirB = 0;
                speedB_new = -speedB;
            }

            if (speedA_new <= 100) speedA_new = 100;
            if (speedB_new <= 100) speedB_new = 100;
            if (speedA_new > 255) speedA_new = 240;
            if (speedB_new > 255) speedB_new = 240;
            asd.DetectArduino(dirA, speedA_new, dirB, speedB_new);
            Thread.Sleep(100);
        }

        private void button2_Click(object sender, EventArgs e)
        {
           // ArduinoControllerMain asd = new ArduinoControllerMain(true);
            //asd.SetComPort(1, 0, 1, 0);
            if(speedA<0)
            speedA = -100;
            else
                speedA = 100;
            if(speedB<0)
            speedB = -100;
            else
                speedA = 100;
            asd.DetectArduino(1, 000, 1, 0);
            Thread.Sleep(100);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //ArduinoControllerMain asd = new ArduinoControllerMain(true);
            int speedA_new = 0;
            int speedB_new = 0;
            speedA -= 50;
            speedB -= 50;
            if (speedA > 0)
            {
                dirA = 1;
                speedA_new = speedA;
            }
            else
            {
                dirA = 0;
                speedA_new = -speedA;
            }
            if (speedB > 0)
            {
                dirB = 1;
                speedB_new = speedB;
            }
            else
            {
                dirB = 0;
                speedB_new = -speedB;
            }
            if (speedA_new <= 100) speedA_new = 100;
            if (speedB_new <= 100) speedB_new = 100;
            if (speedA_new > 255) speedA_new = 240;
            if (speedB_new > 255) speedB_new = 240;
            asd.DetectArduino(dirA, speedA_new, dirB, speedB_new);
            Thread.Sleep(100);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            speedA = 100;
            speedB = 100;// ArduinoControllerMain asd = new ArduinoControllerMain(true);
            asd.DetectArduino(1, 107, 1, 207);
            Thread.Sleep(100);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            speedA = 100;
            speedB = 100;
            asd.DetectArduino(1, 207, 1, 107);
            Thread.Sleep(100);
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
        bool open;
        SerialPort currentPort;
        bool portFound;
        private bool p;

        public ArduinoControllerMain(bool p)
        {
            // TODO: Complete member initialization
            open = p;
        }
        
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
                    if (DetectArduino(dirA, s1,dirB, s2))
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
        public bool DetectArduino(int dirA, int s1a, int dirB, int s2a)
        {
            try
            {
                //if (currentPort.BytesToWrite > 0)
                //currentPort.DiscardOutBuffer();
                //The below setting are for the Hello handshake
                byte[] buffer = new byte[8];
                buffer[0] = Convert.ToByte(dirA);
                buffer[1] = Convert.ToByte(s1a/100);
                buffer[2] = Convert.ToByte(s1a/10%10);
                buffer[3] = Convert.ToByte(s1a%10);
                buffer[4] = Convert.ToByte(dirB);
                buffer[5] = Convert.ToByte(s2a/100);
                buffer[6] = Convert.ToByte(s2a/10%10);
                buffer[7] = Convert.ToByte(s2a%10);
                int intReturnASCII = 0;
                char charReturnValue = (Char)intReturnASCII;
                Debug.WriteLine("open");
                if (open)
                {
                    Debug.WriteLine(open);
                    open = false;
                    currentPort.Open();
                    Debug.WriteLine(open);
                }
                Debug.WriteLine("yes");
                currentPort.Write(buffer, 0, 8);
                while (currentPort.BytesToWrite > 0) ;
                Thread.Sleep(200);
                int count = currentPort.BytesToRead;
                int returnMessage = -1;
               /* while (count > 0)
                {
                    intReturnASCII = currentPort.ReadByte();
                    returnMessage = returnMessage + Convert.ToInt32(intReturnASCII);
                    count--;
                }*/
                //ComPort.name = returnMessage;
                //currentPort.Close();
                if (returnMessage==1)
                {
                    return true;
                }
                else
                {
                    currentPort.Write(buffer, 0, 8);
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
