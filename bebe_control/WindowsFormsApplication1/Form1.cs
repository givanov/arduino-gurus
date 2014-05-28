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


/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
///////////////// TODO make acknoledgement for commands .!!!/////////////////
/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////

namespace WindowsFormsApplication1
{
  
    
    public partial class Form1 : Form
    {
        public Boolean port_open = false;
        ArduinoControllerMain bebe_comms = new ArduinoControllerMain();
        int dirA, dirB, speedA, speedB;
       

        public Form1()
        {
            this.KeyPreview = true;
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
           
            dirA = 1;
            dirB = 1;
            label2.Text = "Current speed:" + hScrollBar1.Value.ToString();
            speedA = hScrollBar1.Value;
            speedB = hScrollBar1.Value;
        }


        //////////////////////////////////////////////////////// 
        ///            BUTTONS CODE HERE                   ////
        ///////////////////////////////////////////////////////

        // Forward 
        private void button1_Click(object sender, EventArgs e)
        {

                label3.Text = "Left Wheel speed: " + speedA.ToString() + " Right Wheel speed: " + speedB.ToString();
                dirA = 1;
                dirB = 1;
                bebe_comms.SetSpeed(dirA, speedA, dirB, speedB);
            
            speedA = hScrollBar1.Value;
            speedB = hScrollBar1.Value;
            Thread.Sleep(100);
        }
  
        // Break
        private void button2_Click(object sender, EventArgs e)
        {

            bebe_comms.SetSpeed(1, 0, 1, 0);
            Thread.Sleep(100);
        }
        // Backward
        private void button3_Click(object sender, EventArgs e)
        {
            label3.Text = "Left Wheel speed: -" + speedA.ToString() + " Right Wheel speed:  -" + speedB.ToString();
            dirA = 0;
            dirB = 0;
            
            bebe_comms.SetSpeed(dirA, speedA, dirB, speedB);
            speedA = hScrollBar1.Value;
            speedB = hScrollBar1.Value;

            Thread.Sleep(100);
        }
        // Turn Left
        private void button4_Click(object sender, EventArgs e)
        {
            speedA = Convert.ToInt32(hScrollBar1.Value * 0.5);
            
        }
        // Turn Right
        private void button5_Click(object sender, EventArgs e)
        {
            speedB = Convert.ToInt32(hScrollBar1.Value * 0.5);
        }
        // Connect
        private void button6_Click(object sender, EventArgs e)
        {
           if (label1.Text.Equals("Connected!"))
           {
           } 
           else{
            if (bebe_comms.SetComPort())
            {
                label1.Text = "Connected!";
            } 
           }

        }
        // Disconnect
        private void button7_Click(object sender, EventArgs e)
        {
            if (bebe_comms.DisconnectArduino())
            {
                label1.Text = "Disconnected!";
            } 
        }

        // Scroll Bar value events
        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            speedA = hScrollBar1.Value;
            speedB = hScrollBar1.Value;
            label2.Text = "Current speed:" + hScrollBar1.Value.ToString();
        }

        // Key Press Handlers.
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
                
                if (e.KeyCode == Keys.Add)
                {
                    e.Handled = false;
                    if (hScrollBar1.Value < 250)
                    {
                        hScrollBar1.Value = hScrollBar1.Value + 1;
                    }
                }
                if (e.KeyCode == Keys.Subtract)
                {
                    e.Handled = false;
                    if (hScrollBar1.Value > 100)
                    {
                        hScrollBar1.Value = hScrollBar1.Value - 1;
                    }
                }
                if (e.KeyCode == Keys.W)
                {
                    e.Handled = false;
                    button1.PerformClick();
                    
                }
                if (e.KeyCode == Keys.B)
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
    //////////////////////////////////////////////////////// 
    ///            ARDUINO CODE HERE                   ////
    ///////////////////////////////////////////////////////

    public class ArduinoControllerMain
    {
        
        public SerialPort currentPort;
        
        public ArduinoControllerMain()
        {
            
        }
        
        public bool DisconnectArduino()
        {
            SetSpeed(0, 0, 0, 0);
            try
            {
                currentPort.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
           
        }
        public bool SetComPort()
        {
          
            try
            {
                string[] ports = SerialPort.GetPortNames();

                // Scan through the available ports.
                foreach (string port in ports)
                {
                   
                        Debug.Print(port.ToString());
                        currentPort = new SerialPort(port, 9600);
                        if (DetectArduino())
                        {
                            return true;
                        }
                        else
                        {
                            currentPort.Close();
                        }
                       
               }
               
            }
            catch (Exception e)
            {
                
            }
          
            return false;
        }

// Function checking for arduino port.
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
            {
            }

            if (response == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

            
        }
  
// Set Speeds Method.
        public void SetSpeed(int dirA, int speedA, int dirB, int speedB)
        {
            try
            {
                
                byte[] buffer = new byte[8];
                
                buffer[0] = Convert.ToByte(dirA);
                buffer[1] = Convert.ToByte(speedA);
                buffer[2] = Convert.ToByte(dirB);
                buffer[3] = Convert.ToByte(speedB);
               
                currentPort.Write(buffer, 0, 4);
              
                while (currentPort.BytesToWrite > 0) ;
                Thread.Sleep(200);
                
            }
            catch (Exception e)
            {
            }
        }
    }
}
