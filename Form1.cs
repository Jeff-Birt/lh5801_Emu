using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lh5801_Emu
{

    public partial class Form1 : Form
    {
        lh5801 CPU = new lh5801();
        
        public Form1()
        {
            InitializeComponent();
            //CPU.LDX(0x0102);
            updateUI();
        }


        /// <summary>
        /// Update Text Boxes and Check Boxes
        /// </summary>
        private void updateUI()
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

            cbCarry.Checked = CPU.GetCarryFlag();
            cbInteruptEnable.Checked = CPU.GetInterruptEnableFlag();
            cbZero.Checked = CPU.GetZeroFlag();
            cbCarry.Checked = CPU.GetCarryFlag();
            cbOverflow.Checked = CPU.GetOverflowFlag();
            cbHalfCarry.Checked = CPU.GetHalfCarryFlag();
        }

        /// <summary>
        /// Poor excuse for hex editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHexDump_Click(object sender, EventArgs e)
        {
            tbHexDump.Text = CPU.HexDump();
        }

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
        private void btnRun_Click(object sender, EventArgs e)
        {
            CPU.SingleStep = false;
            CPU.Run();
            updateUI();
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

        /// <summary>
        /// Set address code in tBValue is poked into
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                tbValue.Text = CPU.ReadRAM((byte)Convert.ToUInt16(tbAddress.Text, 16)).ToString("X2");
                e.Handled = true;
            }
            updateUI();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                ushort address = (ushort)(Convert.ToInt16(tbAddress.Text, 16));
                //byte value = (Byte)(Convert.ToInt16(tbValue.Text, 16));
                string[] values = tbValue.Text.Split(' ');
                byte[] memVal = new byte[values.Length];

                for (int i=0; i < values.Length; i++)
                {
                    memVal[i] = (byte)(Convert.ToInt16(values[i], 16));
                }

                CPU.WriteRAM(address, memVal);

                e.Handled = true;

                tbHexDump.Text = CPU.HexDump();
                updateUI();
            }

        }


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


        #region Registers Text Boxes

        private void tbXH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                CPU.SetRegXH((byte)Convert.ToUInt16(tbXH.Text, 16));
                e.Handled = true;
                updateUI();
            }
        }

        private void tbXL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                CPU.SetRegXL((byte)Convert.ToUInt16(tbXL.Text, 16));
                e.Handled = true;
                updateUI();
            }
        }

        private void tbYH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                CPU.SetRegYH((byte)Convert.ToUInt16(tbYL.Text, 16));
                e.Handled = true;
                updateUI();
            }
        }

        private void tbYL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                CPU.SetRegYL((byte)Convert.ToUInt16(tbYL.Text, 16));
                e.Handled = true;
                updateUI();
            }
        }

        private void tbUH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                CPU.SetRegUH((byte)Convert.ToUInt16(tbUH.Text, 16));
                e.Handled = true;
                updateUI();
            }
        }

        private void tbUL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                CPU.SetRegUL((byte)Convert.ToUInt16(tbUL.Text, 16));
                e.Handled = true;
                updateUI();
            }
        }

        private void tbVH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                CPU.SetRegVL((byte)Convert.ToUInt16(tbVL.Text, 16));
                e.Handled = true;
                updateUI();
            }
        }

        private void tbVL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                CPU.SetRegVL((byte)Convert.ToUInt16(tbVL.Text, 16));
                e.Handled = true;
                updateUI();
            }
        }

        private void tbA_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                CPU.SetRegA((byte)Convert.ToUInt16(tbA.Text, 16));
                e.Handled = true;
                updateUI();
            }
        }

        private void tbP_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                CPU.SetRegP((ushort)Convert.ToUInt16(tbP.Text, 16));
                e.Handled = true;
                updateUI();
            }
        }

        private void tbS_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                CPU.SetRegS((ushort)Convert.ToUInt16(tbS.Text, 16));
                e.Handled = true;
                updateUI();
            }
        }




        #endregion Registers Text Boxes


    }
}
