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
        public Boolean port_open = false;
        ArduinoControllerMain bebe_comms = new ArduinoControllerMain(false);
        int dirA, dirB, speedA, speedB;
       

        public Form1()
        {
            this.KeyPreview = true;
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            bebe_comms.SetComPort();
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
            if (speedA > 0 && speedA<100)
            {
                dirA = 1;
                speedA_new = 100;
            }
            else
            {
                dirA = 1;
                speedA_new = speedA;
            }
            if (speedB > 0 && speedB < 100)
            {
                dirB = 1;
                speedB_new = 100;
            }
            else
            {
                dirB = 1;
                speedB_new = speedB;
            }

            if (speedA_new <= 100) speedA_new = 100;
            if (speedB_new <= 100) speedB_new = 100;
            if (speedA_new > 255) speedA_new = 240;
            if (speedB_new > 255) speedB_new = 240;
            bebe_comms.SetSpeed(dirA, speedA_new, dirB, speedB_new);
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
            bebe_comms.SetSpeed(1, 000, 1, 0);
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
            bebe_comms.SetSpeed(dirA, speedA_new, dirB, speedB_new);
            Thread.Sleep(100);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            speedA = 100;
            speedB = 100;// ArduinoControllerMain asd = new ArduinoControllerMain(true);
            bebe_comms.SetSpeed(1, 107, 1, 207);
            Thread.Sleep(100);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            speedA = 100;
            speedB = 100;
            bebe_comms.SetSpeed(1, 207, 1, 107);
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
        bool port_open;
        SerialPort currentPort;
        bool portFound;
        private bool p;

        public ArduinoControllerMain(bool p)
        {
            // TODO: Complete member initialization
            port_open = p;
        }
        
        public void SetComPort()
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                //double s2p = 0.87 * s2;
                //s2 = (int)Math.Floor(s2p);
                
                foreach (string port in ports)
                {
                    Debug.Print(port.ToString());
                    currentPort = new SerialPort(port, 9600);
                    if (DetectArduino())
                    {
                        
                        portFound = true;
                        //currentPort.Open();
                        break;
                    }
                    else
                    {
                        portFound = false;
                        currentPort.Close();
                       // port_open = true;
                        //currentPort.Close();
                    }
                    Debug.WriteLine("port is found: " + portFound);
                }
            }
            catch (Exception e)
            {
            }
        }
        public bool DetectArduino()
        {
            
            byte[] buffer = new byte[1];
            buffer[0] = Convert.ToByte(1);
            try
            {
                currentPort.Open();
            }
            catch (Exception e)
            {
                return false;
            }
                int elapsed = 0;
            int timeout = 10;
            try
            {
                currentPort.Write(buffer, 0, 1);
                elapsed = 0;
                //retry for 10 seconds
                timeout = 10;
                Debug.WriteLine("sending bytes");
                while (currentPort.BytesToWrite > 0 && (elapsed < timeout))
                {
                    currentPort.Write(buffer, 0, 1);
                    Thread.Sleep(1000);
                    elapsed++;
                }
            }
            catch (Exception e)
            { }
            int count = currentPort.BytesToRead;
            elapsed = 0;
            Debug.WriteLine("receiving bytes");
            while (count == 0 && (elapsed < timeout) ) {
                count = currentPort.BytesToRead;
                Thread.Sleep(1000);
                elapsed++;
            }
            int response = 0;
            currentPort.ReadTimeout = 10000;
            try
            {
                 response = currentPort.ReadByte();
            }
            catch (Exception e)
            { }

            Debug.WriteLine("received: " + response);
            if (response == 1)
            {
                
                return true;
            }
            else
            {
                
                return false;
            }

            
        }
        public void SetSpeed(int dirA, int s1a, int dirB, int s2a)
        {
            try
            {
                
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
                /*if (!port_open)
                {
                    Debug.WriteLine(port_open);
                    port_open = false;
                    currentPort.Open();
                    Debug.WriteLine(port_open);
                }*/
                Debug.WriteLine("yes");
                currentPort.Write(buffer, 0, 8);
                while (currentPort.BytesToWrite > 0) ;
                Thread.Sleep(200);
                //int count = currentPort.BytesToRead;
                //int returnMessage = -1;
               /* while (count > 0)
                {
                    intReturnASCII = currentPort.ReadByte();
                    returnMessage = returnMessage + Convert.ToInt32(intReturnASCII);
                    count--;
                }
                if(returnMessage == -1)
                Debug.WriteLine("paketo e ok");
                else
                    Debug.WriteLine("paketo go nema");
                //ComPort.name = returnMessage;
                //currentPort.Close();
                if (returnMessage==1)
                {
                    return true;
                }
                else
                {
                    //currentPort.Write(buffer, 0, 8);
                    return false;
                }*/
            }
            catch (Exception e)
            {
                //return false;
            }
        }
    }
}
