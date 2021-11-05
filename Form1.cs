using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.IO;

namespace lh5801_Emu
{

    public partial class Form1 : Form
    {
        lh5801 CPU = new lh5801();
        private System.ComponentModel.Design.ByteViewer byteviewer;
        System.Text.RegularExpressions.Regex isHexSpc = new System.Text.RegularExpressions.Regex("^[a-fA-F0-9\\s]+$");
        System.Text.RegularExpressions.Regex isHexSpcKey = new System.Text.RegularExpressions.Regex("^[a-fA-F0-9\\s\\cC\\cV\\cX\\b]+$");
        System.Text.RegularExpressions.Regex isHex = new System.Text.RegularExpressions.Regex("^[a-fA-F0-9]+$");
        System.Text.RegularExpressions.Regex isHexKey = new System.Text.RegularExpressions.Regex("^[a-fA-F0-9\\cC\\cV\\cX\\b]+$");

        private readonly SynchronizationContext synchronizationContext;
        private DateTime previousTime = DateTime.Now;

        /// <summary>
        /// You guessed it, the constructor!
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            synchronizationContext = SynchronizationContext.Current;

            byteviewer = new ByteViewer();
            byteviewer.Location = new Point(210, 39);
            byteviewer.Size = new Size(410, 310);
            byteviewer.SetBytes(CPU.RAM_ME0);
            byteviewer.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.Controls.Add(byteviewer);

            updateUI();
        }


        /// <summary>
        /// Update Text Boxes and Check Boxes
        /// </summary>
        private void updateUI(int value = 0)
        {
            DateTime timeNow = DateTime.Now;

            if ((DateTime.Now - previousTime).Milliseconds <= 50) return;

            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                tbXH.Text = CPU.REG.X.RH.ToString("X2");
                tbXL.Text = CPU.REG.X.RL.ToString("X2");
                tbYH.Text = CPU.REG.Y.RH.ToString("X2");
                tbYL.Text = CPU.REG.Y.RL.ToString("X2");
                tbUH.Text = CPU.REG.U.RH.ToString("X2");
                tbUL.Text = CPU.REG.U.RL.ToString("X2");
                tbVH.Text = CPU.REG.V.RH.ToString("X2");
                tbVL.Text = CPU.REG.V.RL.ToString("X2");
                tbA.Text = CPU.REG.A.ToString("X2");
                tbS.Text = CPU.REG.S.R.ToString("X4");
                tbP.Text = CPU.REG.P.R.ToString("X4");
                tbAddress.Text = tbAddress.Text.ToUpper();

                cbCarry.Checked = CPU.GetCarryFlag();
                cbInteruptEnable.Checked = CPU.GetInterruptEnableFlag();
                cbZero.Checked = CPU.GetZeroFlag();
                cbCarry.Checked = CPU.GetCarryFlag();
                cbOverflow.Checked = CPU.GetOverflowFlag();
                cbHalfCarry.Checked = CPU.GetHalfCarryFlag();

                tbCycles.Text = CPU.tick.ToString("X4");
                tbStack.Text = CPU.GetStack();

                byteviewer.Refresh();
            }), value);
            
            previousTime = timeNow;
        }

        #region RUN Controls

        /// <summary>
        /// Single step next opcode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStep_Click(object sender, EventArgs e)
        {
            CPU.SingleStep = true;
            CPU.Run();
            updateUI();
        }

        /// <summary>
        /// Run code until paused
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnRun_Click(object sender, EventArgs e)
        {
            CPU.SingleStep = false;
            btnRun.Enabled = false;
            CPU.tick = 0;

            await Task.Run(() =>
            {
                do
                {
                    CPU.Run();
                    updateUI(); // eventually send frequency ?
                } while (!CPU.SingleStep);
            });

            btnRun.Enabled = true;
        }

        /// <summary>
        /// Set SingleStep to pause after current opcode is done
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPause_Click(object sender, EventArgs e)
        {
            CPU.SingleStep = true;
            updateUI();
        }

        /// <summary>
        /// Resets CPU but does not clear ME0 or ME1 RAM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            CPU.Reset();
            updateUI();
        }

        /// <summary>
        /// Resets CPU and clears ME0 and ME1 RAM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetAll_Click(object sender, EventArgs e)
        {
            CPU.Reset(true);
            updateUI();
        }

        #endregion RUN Controls

        #region Registers and HEX Dump

        /// <summary>
        /// Handles all 8 and 16 bit register inputs and validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbRegHL_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;

            if (c == (char)Keys.Enter)
            {
                byte value = (byte)Convert.ToUInt16(((TextBox)sender).Text, 16);
                ushort valWord = (ushort)Convert.ToUInt16(((TextBox)sender).Text, 16);

                switch (((TextBox)sender).Name)
                {
                    case ("tbXH"):
                        CPU.REG.X.RH = (value);
                        break;

                    case ("tbXL"):
                        CPU.REG.X.RL = (value);
                        break;

                    case ("tbYH"):
                        CPU.REG.Y.RH = (value);
                        break;

                    case ("tbYL"):
                        CPU.REG.Y.RL = (value);
                        break;

                    case ("tbUH"):
                        CPU.REG.U.RH = (value);
                        break;

                    case ("tbUL"):
                        CPU.REG.U.RL = (value);
                        break;

                    case ("tbVH"):
                        CPU.REG.V.RH = (value);
                        break;

                    case ("tbVL"):
                        CPU.REG.V.RL = (value);
                        break;

                    case ("tbA"):
                        CPU.REG.A = (value);
                        break;

                    case ("tbP"):
                        CPU.REG.P.R = (valWord);
                        break;

                    case ("tbS"):
                        CPU.REG.S.R = (valWord);
                        break;
                }

                e.Handled = true;
                updateUI();
            }
            else if (!isHexKey.IsMatch(c.ToString()))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Set address code that tBValue is poked into
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;

            if (c == (char)Keys.Enter)
            {
                e.Handled = true;
                byteviewer.SetStartLine(Convert.ToUInt16(tbAddress.Text, 16) / 0x10);
                updateUI();
            }
            else if (!isHexKey.IsMatch(c.ToString()))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Validates any texted pasted into text box
        /// Only hex charecters allowed 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbRegHL_TextChanged(object sender, EventArgs e)
        {
            string txt = ((TextBox)sender).Text;
            bool isValid = false;

            if (string.IsNullOrEmpty(txt))
            {
                isValid = false;
            }
            else
            {
                isValid = isHex.IsMatch(txt);
            }

            if (!isValid) { ((TextBox)sender).Text = ""; }
        }

        /// <summary>
        /// Parse data entered into tbValue and poke into RAM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool isValid = true;
            char c = e.KeyChar;

            if (c == (char)Keys.Enter)
            {
                ushort  address = (ushort)(Convert.ToInt16(tbAddress.Text, 16));
                string[] values = tbValue.Text.Split(' ');
                byte[]   memVal = new byte[values.Length];

                for (int i=0; i < values.Length; i++)
                {
                    if (values[i].Length < 3)
                    {
                        memVal[i] = (byte)(Convert.ToInt16(values[i], 16));
                    }
                    else
                    {
                        string message = values[i] + " is > $FF";
                        MessageBox.Show(message, "Oops!");
                        i = values.Length;
                        isValid = false;
                    } 
                }

                // write into selected RAM area if input valid
                if (isValid)
                {
                    if (rbME0.Checked)
                    {
                        CPU.WriteRAM_ME0(address, memVal);
                    }
                    else
                    {
                        CPU.WriteRAM_ME1(address, memVal);
                    }

                    byteviewer.SetStartLine(address / 0x10);
                }

                e.Handled = true;

                updateUI();
            }
            else if (!isHexSpcKey.IsMatch(c.ToString()))
            {
                e.Handled = true;
            }

        }

        /// <summary>
        /// Validates any texted pasted into text box
        /// Only hex charecters and space allowed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbValue_TextChanged(object sender, EventArgs e)
        {
            string txt = tbValue.Text;
            bool isValid = false;

            if (string.IsNullOrEmpty(txt))
            {
                isValid = false;
            }
            else
            {
                isValid = isHexSpc.IsMatch(txt);
            }

            if (!isValid) { tbValue.Text = ""; }
        }

        /// <summary>
        /// Toggle between ME0 and ME1 hex dump
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbME0_CheckedChanged(object sender, EventArgs e)
        {
            byteviewer.SetBytes(CPU.RAM_ME0);
            updateUI();
        }

        /// <summary>
        /// Toggle between ME0 and ME1 hex dump
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbME1_CheckedChanged(object sender, EventArgs e)
        {
            byteviewer.SetBytes(CPU.RAM_ME1);
            updateUI();
        }

        /// <summary>
        /// Set stack display width
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbStackDispW_CheckedChanged(object sender, EventArgs e)
        {
            CPU.StackWidth8 = cbStackDispW.Checked;
            updateUI();
        }

        #endregion Registers and HEX Dump

        #region Flags Check Boxes

        /// <summary>
        /// Forward user change Carry flag check box to CPU
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbCarry_Click(object sender, EventArgs e)
        {
            CPU.SetCarryFlag(cbCarry.Checked);
        }

        /// <summary>
        /// Forward user change Interupt Enable flag check box to CPU
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbInteruptEnable_Click(object sender, EventArgs e)
        {
            CPU.SetInterruptEnableFlag(cbCarry.Checked);
        }

        /// <summary>
        /// Forward user change Zero flag check box to CPU
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbZero_Click(object sender, EventArgs e)
        {
            CPU.SetZeroFlag(cbCarry.Checked);
        }

        /// <summary>
        /// Forward user change Overflowt Enable flag check box to CPU
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbOverflow_Click(object sender, EventArgs e)
        {
            CPU.SetOverflowFlag(cbCarry.Checked);
        }

        /// <summary>
        /// Forward user change Half Carry Enable flag check box to CPU
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbHalfCarry_Click(object sender, EventArgs e)
        {
            CPU.SetHalfCarryFlag(cbCarry.Checked);
        }




        #endregion Flags Check Boxes

        /// <summary>
        /// Load a binary file into selected RAM bank
        /// starting at 'Address'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1;
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.ShowDialog();

            string inputFile = openFileDialog1.FileName;

            FileStream inputfs = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            BinaryReader fileReader = new BinaryReader(inputfs);
            long fileSize = inputfs.Length;
            ushort targetAddress = (ushort)Convert.ToInt16(tbAddress.Text, 16);
            ushort spaceLeft = (ushort)(0xFFFF - targetAddress);

            if (fileSize > spaceLeft )
            {
                string message = "File too large";
                string title = "Oops!";
                MessageBox.Show(message, title);
            }
            else
            {
                for (long i = 0; i < fileSize; i++)
                {
                    if (rbME0.Checked)
                    {
                        CPU.RAM_ME0[targetAddress + i] = fileReader.ReadByte();
                    }
                    else
                    {
                        CPU.RAM_ME1[targetAddress + i] = fileReader.ReadByte();
                    }
                    
                }
                updateUI();
            }        
        }

        /// <summary>
        /// Save entire ME0 or ME1 to binary file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog1;
            saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog1.ShowDialog();
            string outputFile = saveFileDialog1.FileName;

            long fileSize = 0xFFFF;

            FileStream outputfs = new FileStream(outputFile, FileMode.Create);
            BinaryWriter fileWriter = new BinaryWriter(outputfs);

            for (long i = 0; i < fileSize; i++)
            {
                if (rbME0.Checked)
                {
                    fileWriter.Write((byte)CPU.RAM_ME0[i]);
                }
                else
                {
                    fileWriter.Write((byte)CPU.RAM_ME1[i]);
                }
            }
        }

    }
}
