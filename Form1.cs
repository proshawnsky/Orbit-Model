using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ArduinoInterface
{
    public partial class Form1 : Form
    {
        private bool connected;
        private string message;
        public delegate void d1(string indata);
        private int enabled;
        public Form1()
        {
            InitializeComponent();
            getAvailablePorts();

        }
        private void getAvailablePorts()
        {
            String[] ports = SerialPort.GetPortNames();
            portList.Items.AddRange(ports);
        }

        private void closePort_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            status.Text = "Select Port";
            status.BackColor = Color.LightCoral;
            connected = false;
        }

        private void openPort_Click(object sender, EventArgs e)
        {
            try
            {
                if (portList.Text == "")
                {
                    status.Text = "No Port";

                }
                else
                {
                    serialPort1.PortName = portList.Text;
                    serialPort1.Open();
                    status.BackColor = Color.Lime;
                    status.Text = "Connected";
                    connected = true;
                }
            }
            catch (UnauthorizedAccessException)
            {
                status.Text = "Unauthorized";
                status.BackColor = Color.LightCoral;
            }
            catch (System.IO.IOException)
            {
                status.Text = "Unavailable";
                status.BackColor = Color.LightCoral;
            }

        }

        private void allButtons(object sender, EventArgs e)
        { 
            switch (((Button)sender).Name)
            {
                //RAAN, Inc setpoints, other Orbit Parameters
                // case "sendRAAN": message = 'r' + inputRAAN.Text; break;
                // case "sendInc": message = 'i' + inputInc.Text; break;
                // case "sendEcc": message = 'e' + inputEcc.Text; break;
                // case "sendSMA": message = 'a' + inputSMA.Text; break;
                // case "sendTA": message = 'f' + inputTA.Text; break;
                //  case "sendPeri": message = 'w' + inputPeri.Text; break;
                // case "play": message = "t" + Convert.ToString(timeRate.Value); break;
                default: message = Convert.ToString(((Button)sender).Tag); break; //all buttons not depending on an input field
            }
            sent.Text = message;
            if (connected) { serialPort1.Write(message); }
            else { //flashLabelRed(status);
                   //
                 }

        }
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            String indata = serialPort1.ReadLine();
            d1 writeit = new d1(Write2Form);
            Invoke(writeit, indata);
        }

        public void Write2Form(string indata)
        {
            //this function handles data sent from the Arduino
            char firstchar;
            //Double raan, inc;
            int mode;
            Color offColor = Color.LightCoral; Color onColor = Color.Lime; Color tempColor;

            firstchar = indata[0];
            received.Text = indata;
            switch (firstchar)
            {
                case 'D': //disable/enable bus
                    enabled = Convert.ToInt16(indata.Substring(1, 1));
                    if (enabled == 1) {
                        busEnableButton.BackColor = Color.Lime;
                        busDisableButton.BackColor = Color.Transparent;
                    }
                    else {
                        busEnableButton.BackColor = Color.Transparent;
                        busDisableButton.BackColor = Color.LightCoral;
                    };
                        break;
                case 'M': //Handle Mode Label Indicators
                    mode = Convert.ToInt16(indata.Substring(1, 1));
                    switch (mode)
                    {
                        case 0:
                            mode0Button.BackColor = Color.Lime;
                            mode1Button.BackColor = Color.Transparent;
                            mode2Button.BackColor = Color.Transparent;
                            mode3Button.BackColor = Color.Transparent;
                            mode4Button.BackColor = Color.Transparent;
                            mode5Button.BackColor = Color.Transparent;
                            break;
                        case 1:
                            mode0Button.BackColor = Color.Transparent;
                            mode1Button.BackColor = Color.Lime;
                            mode2Button.BackColor = Color.Transparent;
                            mode3Button.BackColor = Color.Transparent;
                            mode4Button.BackColor = Color.Transparent;
                            mode5Button.BackColor = Color.Transparent;
                            break;
                        case 2:
                            mode0Button.BackColor = Color.Transparent;
                            mode1Button.BackColor = Color.Transparent;
                            mode2Button.BackColor = Color.Lime;
                            mode3Button.BackColor = Color.Transparent;
                            mode4Button.BackColor = Color.Transparent;
                            mode5Button.BackColor = Color.Transparent;
                            break;
                        case 3:
                            mode0Button.BackColor = Color.Transparent;
                            mode1Button.BackColor = Color.Transparent;
                            mode2Button.BackColor = Color.Transparent;
                            mode3Button.BackColor = Color.Lime;
                            mode4Button.BackColor = Color.Transparent;
                            mode5Button.BackColor = Color.Transparent;
                            break;
                        case 4:
                            mode0Button.BackColor = Color.Transparent;
                            mode1Button.BackColor = Color.Transparent;
                            mode2Button.BackColor = Color.Transparent;
                            mode3Button.BackColor = Color.Transparent;
                            mode4Button.BackColor = Color.Lime;
                            mode5Button.BackColor = Color.Transparent;
                            break;
                        case 5:
                            mode0Button.BackColor = Color.Transparent;
                            mode1Button.BackColor = Color.Transparent;
                            mode2Button.BackColor = Color.Transparent;
                            mode3Button.BackColor = Color.Transparent;
                            mode4Button.BackColor = Color.Transparent;
                            mode5Button.BackColor = Color.Lime;
                            break;
                    }
                    break;
            }
        }
    }
}
