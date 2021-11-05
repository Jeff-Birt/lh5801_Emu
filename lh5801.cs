using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lh5801_Emu
{
    public class Reg16Bit
    {
        private byte _RH; // upper byte
        private byte _RL; // lower byte

        public Reg16Bit()
        {
            this._RH = (byte)0x00;
            this._RL = (byte)0x00;
            //this._RH = (byte)(value >> 8);
            //this._RL = (byte)(value & 0xFF);
        }

        public byte RH
        {
            get
            {
                return this._RH;
            }
            set
            {
                this._RH = value;
            }
        }

        public byte RL
        {
            get
            {
                return this._RL;
            }
            set
            {
                this._RL = value;
            }
        }

        public ushort R
        {
            get
            {
                return (ushort)((this._RH << 8) | (int)(this._RL));
            }
            set
            {
                this._RH = (byte)(value >> 8);
                this._RL = (byte)(value & 0xFF);
            }
        }
    }

    public class lh5801
    {
        /// <summary>
        /// These are individual bits on the real uC but we can cheat
        /// and use bytes.
        /// </summary>
        public struct StatusFlags
        {
            public bool C;  // Bit 0, Carry/Borrow
            public bool IE; // Bit 1, Interrupt Enable
            public bool Z;  // Bit 2, Zero
            public bool V;  // Bit 3, Overflow
            public bool H;  // Bit 4, Half Carry
            //              // Bit 5, not used
            //              // Bit 6, not used
            //              // Bit 7, not used
        }

        /// <summary>
        /// Represents lh5801 processor registers
        /// </summary>
        public struct Registers
        {
            public Reg16Bit P;  // Program counter
            public Reg16Bit S;  // Stack pointer
            public Reg16Bit X;  // X 
            public Reg16Bit Y;  // Y
            public Reg16Bit U;  // U
            public Reg16Bit V;  // V
            public Byte A;      // Accumulator
            public ushort TM;   // Timer
            public bool PU;     // PU bank Flip-flop
            public bool PV;     // PV bank flip-flop
            public bool DISP;   // LCD on/off control
        }

        public bool SingleStep = true;
        public bool StackWidth8 = false;
        public ushort tick = 0;   // number of processor clock cyclces
        public Registers REG;
        public StatusFlags FLAGS;
        public byte[] RAM_ME0 = new byte[0xFFFF + 0x01];
        public byte[] RAM_ME1 = new byte[0xFFFF + 0x01];
        List<Delegate> delegatesTbl1 = new List<Delegate>();
        List<Delegate> delegatesTbl2 = new List<Delegate>();

        /// <summary>
        /// The contructonator
        /// </summary>
        public lh5801()
        {
            REG = new Registers();
            ResetRegisters();
            Reset();
            ConfigDelegates();
        }

        /// <summary>
        /// Reset all Flags and Registers
        /// </summary>
        public void Reset (bool resetRAM = false)
        {
            ResetFlags();
            ResetRegisters();
            SingleStep = true;
            tick = 0;

            if (resetRAM) { ResetRAM(); }
        }

        /// <summary>
        /// Initialize ME0 and ME1 RAM to all zeros
        /// </summary>
        private void ResetRAM()
        {
            Array.Clear(RAM_ME0, 0, RAM_ME0.Length);
            Array.Clear(RAM_ME1, 0, RAM_ME0.Length);
        }

        /// <summary>
        /// Reset all Flags
        /// </summary>
        private void ResetFlags()
        {
            FLAGS.C  = false;
            FLAGS.H  = false;
            FLAGS.IE = false;
            FLAGS.V  = false;
            FLAGS.Z  = false;
        }

        /// <summary>
        /// Resest all Registers
        /// </summary>
        private void ResetRegisters()
        {
            REG.X    = new Reg16Bit();
            REG.Y    = new Reg16Bit();
            REG.U    = new Reg16Bit();
            REG.V    = new Reg16Bit();
            REG.P    = new Reg16Bit();
            REG.S    = new Reg16Bit();
            REG.S.R  = 0x1000;
            REG.A    = 0x00;
            REG.TM   = 0x0000;
            REG.PU   = false;
            REG.PV   = false;
            REG.DISP = false;
        }

        /// <summary>
        /// Configure Delegate List (function pointers)
        /// </summary>
        private void ConfigDelegates()
        {
            #region Opcode Table 1 0x00-0xFF

            #region Opcodes_0x00-0x0F
            delegatesTbl1.Add((Action)SBC_XL);      // Opcode 0x00, SBC XL
            delegatesTbl1.Add((Action)SBC_X_ME0);   // Opcode 0x01, SBC (X)
            delegatesTbl1.Add((Action)ADC_XL);      // Opcode 0x02, ADC XL
            delegatesTbl1.Add((Action)ADC_X_ME0);   // Opcode 0x03, ADC (X)
            delegatesTbl1.Add((Action)LDA_XL);      // Opcode 0x04, LDA XL
            delegatesTbl1.Add((Action)LDA_X_ME0);   // Opcode 0x05, LDA (X)
            delegatesTbl1.Add((Action)CPA_XL);      // Opcode 0x06, CPA XL
            delegatesTbl1.Add((Action)CPA_X_ME0);   // Opcode 0x07, CPA (X)
            delegatesTbl1.Add((Action)STA_XH);      // Opcode 0x08, STA XH
            delegatesTbl1.Add((Action)AND_X_ME0);   // Opcode 0x09
            delegatesTbl1.Add((Action)STA_XL);      // Opcode 0x0A, STA XL
            delegatesTbl1.Add((Action)ORA_X_ME0);   // Opcode 0x0B, ORA (X)
            delegatesTbl1.Add((Action)DCS_X_ME0);   // Opcode 0x0C, DCS (X)
            delegatesTbl1.Add((Action)EOR_X_ME0);   // Opcode 0x0D, EOR (X)
            delegatesTbl1.Add((Action)STA_X_ME0);   // Opcode 0x0E, STA (X)
            delegatesTbl1.Add((Action)BIT_X_ME0);   // Opcode 0x0F, BIT (X)
            #endregion Opcodes_0x00-0x0F

            #region Opcodes_0x10-0x1F
            delegatesTbl1.Add((Action)SBC_YL);      // Opcode 0x10, SBC YL
            delegatesTbl1.Add((Action)SBC_Y_ME0);   // Opcode 0x11, SBC (Y)
            delegatesTbl1.Add((Action)ADC_YL);      // Opcode 0x12, ADC YL
            delegatesTbl1.Add((Action)ADC_Y_ME0);   // Opcode 0x13, ADC (Y)
            delegatesTbl1.Add((Action)LDA_YL);      // Opcode 0x14, LDA YL
            delegatesTbl1.Add((Action)LDA_Y_ME0);   // Opcode 0x15, LDA (Y)
            delegatesTbl1.Add((Action)CPA_YL);      // Opcode 0x16, CPA YL
            delegatesTbl1.Add((Action)CPA_Y_ME0);   // Opcode 0x17, CPA (Y)
            delegatesTbl1.Add((Action)STA_YH);      // Opcode 0x18, STA YH
            delegatesTbl1.Add((Action)AND_Y_ME0);   // Opcode 0x19
            delegatesTbl1.Add((Action)STA_YL);      // Opcode 0x1A, STA YL
            delegatesTbl1.Add((Action)ORA_Y_ME0);   // Opcode 0x1B, ORA (Y)
            delegatesTbl1.Add((Action)DCS_Y_ME0);   // Opcode 0x1C, DCS (Y)
            delegatesTbl1.Add((Action)EOR_Y_ME0);   // Opcode 0x1D, EOR (Y)
            delegatesTbl1.Add((Action)STA_Y_ME0);   // Opcode 0x1E, STA (Y)
            delegatesTbl1.Add((Action)BIT_Y_ME0);   // Opcode 0x1F, BIT (Y)
            #endregion Opcodes_0x10-0x1F

            #region Opcodes_0x20-0x2F
            delegatesTbl1.Add((Action)SBC_UL);      // Opcode 0x20, SBC UL
            delegatesTbl1.Add((Action)SBC_U_ME0);   // Opcode 0x21, SBC (U)
            delegatesTbl1.Add((Action)ADC_UL);      // Opcode 0x22, ADC UL
            delegatesTbl1.Add((Action)ADC_U_ME0);   // Opcode 0x23, ADC (U)
            delegatesTbl1.Add((Action)LDA_UL);      // Opcode 0x24, LDA UL
            delegatesTbl1.Add((Action)LDA_U_ME0);   // Opcode 0x25, LDA (U)
            delegatesTbl1.Add((Action)CPA_UL);      // Opcode 0x26, CPA UL
            delegatesTbl1.Add((Action)CPA_U_ME0);   // Opcode 0x27, CPA (U)
            delegatesTbl1.Add((Action)STA_UH);      // Opcode 0x28, STA UH
            delegatesTbl1.Add((Action)AND_U_ME0);   // Opcode 0x29
            delegatesTbl1.Add((Action)STA_UL);      // Opcode 0x2A, STA UL
            delegatesTbl1.Add((Action)ORA_U_ME0);   // Opcode 0x2B, ORA (U)
            delegatesTbl1.Add((Action)DCS_U_ME0);   // Opcode 0x2C, DCS (U)
            delegatesTbl1.Add((Action)EOR_U_ME0);   // Opcode 0x2D, EOR (U)
            delegatesTbl1.Add((Action)STA_U_ME0);   // Opcode 0x2E, STA (U)
            delegatesTbl1.Add((Action)BIT_U_ME0);   // Opcode 0x2F, BIT (U)
            #endregion Opcodes_0x20-0x2F

            #region Opcodes_0x30-0x3F
            delegatesTbl1.Add((Action)SBC_VL);      // Opcode 0x30, SBC VL
            delegatesTbl1.Add((Action)SBC_V_ME0);   // Opcode 0x31, SBC (V)
            delegatesTbl1.Add((Action)ADC_VL);      // Opcode 0x32, ADC VL
            delegatesTbl1.Add((Action)ADC_V_ME0);   // Opcode 0x33, ADC (V)
            delegatesTbl1.Add((Action)LDA_VL);      // Opcode 0x34, LDA VL
            delegatesTbl1.Add((Action)LDA_V_ME0);   // Opcode 0x35, LDA (V)
            delegatesTbl1.Add((Action)CPA_VL);      // Opcode 0x36, CPA VL
            delegatesTbl1.Add((Action)CPA_V_ME0);   // Opcode 0x37, CPA (V)
            delegatesTbl1.Add((Action)NOP);         // Opcode 0x38, NOP and STA VH
            delegatesTbl1.Add((Action)AND_V_ME0);   // Opcode 0x39
            delegatesTbl1.Add((Action)STA_VL);      // Opcode 0x3A, STA VL
            delegatesTbl1.Add((Action)ORA_V_ME0);   // Opcode 0x3B, ORA (V)
            delegatesTbl1.Add((Action)DCS_V_ME0);   // Opcode 0x3C, DCS (V)
            delegatesTbl1.Add((Action)EOR_V_ME0);   // Opcode 0x3D, EOR (V)
            delegatesTbl1.Add((Action)STA_V_ME0);   // Opcode 0x3E, STA (V)
            delegatesTbl1.Add((Action)BIT_V_ME0);   // Opcode 0x3F, BIT (V)
            #endregion Opcodes_0x30-0x3F

            #region Opcodes_0x40-0x4F
            delegatesTbl1.Add((Action)INC_XL);      // Opcode 0x40, INC XL
            delegatesTbl1.Add((Action)SIN_X);       // Opcode 0x41, SIN X
            delegatesTbl1.Add((Action)DEC_XL);      // Opcode 0x42, DEC XL
            delegatesTbl1.Add((Action)SDE_X);       // Opcode 0x43, SDE X
            delegatesTbl1.Add((Action)INC_X);       // Opcode 0x44, INC X
            delegatesTbl1.Add((Action)LIN_X);       // Opcode 0x45, LIN X
            delegatesTbl1.Add((Action)DEC_X);       // Opcode 0x46, DEC X
            delegatesTbl1.Add((Action)LDE_X_ME0);   // Opcode 0x47, LDE X
            delegatesTbl1.Add((Action)LDI_XH_n);    // Opcode 0x48, LDI XH
            delegatesTbl1.Add((Action)ANI_X_n_ME0); // Opcode 0x49, ANI (X),n
            delegatesTbl1.Add((Action)LDI_XL_n);    // Opcode 0x4A, LDI XL
            delegatesTbl1.Add((Action)ORI_X_n_ME0); // Opcode 0x4B, ORI (X),n
            delegatesTbl1.Add((Action)CPI_XH_n);    // Opcode 0x4C, CPI XH,n
            delegatesTbl1.Add((Action)BII_X_n_ME0); // Opcode 0x4D, BII (X),n
            delegatesTbl1.Add((Action)CPI_XL_n);    // Opcode 0x4E, CPI XL,n
            delegatesTbl1.Add((Action)ADI_X_n);     // Opcode 0x4F, ADI (X),n
            #endregion Opcodes_0x40-0x4F

            #region Opcodes_0x50-0x5F
            delegatesTbl1.Add((Action)INC_YL);      // Opcode 0x50, INC YL
            delegatesTbl1.Add((Action)SIN_Y);       // Opcode 0x51, SIN Y
            delegatesTbl1.Add((Action)DEC_YL);      // Opcode 0x52, DEC YL
            delegatesTbl1.Add((Action)SDE_Y);       // Opcode 0x53, SDE Y
            delegatesTbl1.Add((Action)INC_Y);       // Opcode 0x54, INC Y
            delegatesTbl1.Add((Action)LIN_Y);       // Opcode 0x55, LIN Y
            delegatesTbl1.Add((Action)DEC_Y);       // Opcode 0x56, DEC Y
            delegatesTbl1.Add((Action)LDE_Y);       // Opcode 0x57, LDE Y
            delegatesTbl1.Add((Action)LDI_YH_n);    // Opcode 0x58, LDI YH
            delegatesTbl1.Add((Action)ANI_Y_n_ME0); // Opcode 0x59, ANI (Y),n
            delegatesTbl1.Add((Action)LDI_YL_n);    // Opcode 0x5A, LDI YL
            delegatesTbl1.Add((Action)ORI_Y_n_ME0); // Opcode 0x5B, ORI (Y),n
            delegatesTbl1.Add((Action)CPI_YH_n);    // Opcode 0x5C, CPI YH,n
            delegatesTbl1.Add((Action)BII_Y_n_ME0); // Opcode 0x5D, BII (Y),n
            delegatesTbl1.Add((Action)CPI_YL_n);    // Opcode 0x5E, CPI YL,n
            delegatesTbl1.Add((Action)ADI_Y_n);     // Opcode 0x5F, ADI (Y),n
            #endregion Opcodes_0x50-0x5F

            #region Opcodes_0x60-0x6F
            delegatesTbl1.Add((Action)INC_UL);      // Opcode 0x60, INC UL
            delegatesTbl1.Add((Action)SIN_U);       // Opcode 0x61, SIN U
            delegatesTbl1.Add((Action)DEC_UL);      // Opcode 0x62, DEC UL
            delegatesTbl1.Add((Action)SDE_U);       // Opcode 0x63, SDE U
            delegatesTbl1.Add((Action)INC_U);       // Opcode 0x64, INC U
            delegatesTbl1.Add((Action)LIN_U);       // Opcode 0x65, LIN U
            delegatesTbl1.Add((Action)DEC_U);       // Opcode 0x66, DEC U
            delegatesTbl1.Add((Action)LDE_U);       // Opcode 0x67, LDE U
            delegatesTbl1.Add((Action)LDI_UH_n);    // Opcode 0x68, LDI UH,n
            delegatesTbl1.Add((Action)ANI_U_n_ME0); // Opcode 0x69, ANI (U),n
            delegatesTbl1.Add((Action)LDI_UL_n);    // Opcode 0x6A, LDI UL,n
            delegatesTbl1.Add((Action)ORI_U_n_ME0); // Opcode 0x6B, ORI (U),n
            delegatesTbl1.Add((Action)CPI_UH_n);    // Opcode 0x6C, CPI UH,n
            delegatesTbl1.Add((Action)BII_U_n_ME0); // Opcode 0x6D, BII (U),n
            delegatesTbl1.Add((Action)CPI_UL_n);    // Opcode 0x6E, CPI UL,n
            delegatesTbl1.Add((Action)ADI_U_n);     // Opcode 0x6F, ADI (U),n
            #endregion Opcodes_0x60-0x6F

            #region Opcodes_0x70-0x7F
            delegatesTbl1.Add((Action)INC_VL);      // Opcode 0x70, INC VL
            delegatesTbl1.Add((Action)SIN_V);       // Opcode 0x71, SIN V
            delegatesTbl1.Add((Action)DEC_VL);      // Opcode 0x72, DEC VL
            delegatesTbl1.Add((Action)SDE_V);       // Opcode 0x73, SDE V
            delegatesTbl1.Add((Action)INC_V);       // Opcode 0x74, INC V 
            delegatesTbl1.Add((Action)LIN_V);       // Opcode 0x75, LIN V
            delegatesTbl1.Add((Action)DEC_V);       // Opcode 0x76, DEC V
            delegatesTbl1.Add((Action)LDE_V_ME0);   // Opcode 0x77, LDE V
            delegatesTbl1.Add((Action)LDI_VH_n);    // Opcode 0x78, LDI VH,n
            delegatesTbl1.Add((Action)ANI_V_n_ME0); // Opcode 0x79, ANI (V),n
            delegatesTbl1.Add((Action)LDI_VL_n);    // Opcode 0x7A, LDI VL,n
            delegatesTbl1.Add((Action)ORI_V_n_ME0); // Opcode 0x7B, ORI (V),n
            delegatesTbl1.Add((Action)CPI_VH_n);    // Opcode 0x7C, CPI VH,n
            delegatesTbl1.Add((Action)BII_V_n_ME0); // Opcode 0x7D, BII (V),n
            delegatesTbl1.Add((Action)CPI_VL_n);    // Opcode 0x7E, CPI VL,n
            delegatesTbl1.Add((Action)ADI_V_n);     // Opcode 0x7F, ADI (V),n
            #endregion Opcodes_0x70-0x7F

            #region Opcodes_0x80-0x8F
            delegatesTbl1.Add((Action)SBC_XH);      // Opcode 0x80, SBC XH
            delegatesTbl1.Add((Action)BCR_n_p);     // Opcode 0x81, BCR+e
            delegatesTbl1.Add((Action)ADC_XH);      // Opcode 0x82, ADC XH
            delegatesTbl1.Add((Action)BCS_n_p);     // Opcode 0x83, BCS+e
            delegatesTbl1.Add((Action)LDA_XH);      // Opcode 0x84, LDA XH
            delegatesTbl1.Add((Action)BHR_n_p);     // Opcode 0x85, BHR+e
            delegatesTbl1.Add((Action)CPA_XH);      // Opcode 0x86, CPA XH
            delegatesTbl1.Add((Action)BHS_n_p);     // Opcode 0x87, BHS+e
            delegatesTbl1.Add((Action)LOP_UL_e);    // Opcode 0x88. LOP UL,e
            delegatesTbl1.Add((Action)BZR_n_p);     // Opcode 0x89, BZR+e
            delegatesTbl1.Add((Action)RTI);         // Opcode 0x8A, RTI
            delegatesTbl1.Add((Action)BZS_n_p);     // Opcode 0x8B, BZS+e
            delegatesTbl1.Add((Action)DCA_X_ME0);   // Opcode 0x8C, DCA (X)
            delegatesTbl1.Add((Action)BVR_n_p);     // Opcode 0x8D, BVR+e
            delegatesTbl1.Add((Action)BCH_n_p);     // Opcode 0x8E, BCH+e
            delegatesTbl1.Add((Action)BVS_n_p);     // Opcode 0x8F, BVS+e
            #endregion Opcodes_0x80-0x8F

            #region Opcodes_0x90-0x9F
            delegatesTbl1.Add((Action)SBC_YH);      // Opcode 0x90, SBC YH
            delegatesTbl1.Add((Action)BCR_n_m);     // Opcode 0x91, BCR-e
            delegatesTbl1.Add((Action)ADC_YH);      // Opcode 0x92, ADC YH
            delegatesTbl1.Add((Action)BCS_n_m);     // Opcode 0x93, BCS-e
            delegatesTbl1.Add((Action)LDA_YH);      // Opcode 0x94, LDA YH
            delegatesTbl1.Add((Action)BHR_n_m);     // Opcode 0x95, BHE-e
            delegatesTbl1.Add((Action)CPA_YH);      // Opcode 0x96, CPA YH
            delegatesTbl1.Add((Action)BHS_n_m);     // Opcode 0x97, BHS-e
            delegatesTbl1.Add((Action)NOP);         // Opcode 0x98
            delegatesTbl1.Add((Action)BZR_n_m);     // Opcode 0x99, BZR-e
            delegatesTbl1.Add((Action)RTN);         // Opcode 0x9A, RTN
            delegatesTbl1.Add((Action)BZS_n_m);     // Opcode 0x9B, BZS-e
            delegatesTbl1.Add((Action)DCA_Y_ME0);   // Opcode 0x9C, DCA (Y)
            delegatesTbl1.Add((Action)BVR_n_m);     // Opcode 0x9D, BVR-e
            delegatesTbl1.Add((Action)BCH_n_m);     // Opcode 0x9E, BCH-e
            delegatesTbl1.Add((Action)BVS_n_m);     // Opcode 0x9F, BVS-e
            #endregion Opcodes_0x90-0x9F

            #region Opcodes_0xA0-0xAF
            delegatesTbl1.Add((Action)SBC_UH);      // Opcode 0xA0, SBC UH
            delegatesTbl1.Add((Action)SBC_pp_ME0);  // Opcode 0xA1, SBC (pp)
            delegatesTbl1.Add((Action)ADC_UH);      // Opcode 0xA2, ADC UH
            delegatesTbl1.Add((Action)ADC_pp_ME0);  // Opcode 0xA3, ADC (pp)
            delegatesTbl1.Add((Action)LDA_UH);      // Opcode 0xA4, LDA XH
            delegatesTbl1.Add((Action)LDA_pp_ME0);  // Opcode 0xA5, LDA (pp)
            delegatesTbl1.Add((Action)CPA_UH);      // Opcode 0xA6, CPA UH
            delegatesTbl1.Add((Action)CPA_pp_ME0);  // Opcode 0xA7, CPA (pp)
            delegatesTbl1.Add((Action)SPV);         // Opcode 0xA8, SPV
            delegatesTbl1.Add((Action)AND_pp_ME0);  // Opcode 0xA9
            delegatesTbl1.Add((Action)LDI_S_pp);    // Opcode 0xAA, LDI S,pp
            delegatesTbl1.Add((Action)ORA_pp_ME0);  // Opcode 0xAB, ORA (pp)
            delegatesTbl1.Add((Action)DCA_U_ME0);   // Opcode 0xAC, DCA (U)
            delegatesTbl1.Add((Action)EOR_pp_ME0);  // Opcode 0xAD, EOR (pp)
            delegatesTbl1.Add((Action)STA_pp_ME0);  // Opcode 0xAE, STA (pp)
            delegatesTbl1.Add((Action)BIT_pp_ME0);  // Opcode 0xAF, BIT (pp)
            #endregion Opcodes_0xA0-0xAF

            #region Opcodes_0xB0-0xBF
            delegatesTbl1.Add((Action)SBC_VH);      // Opcode 0xB0, SBC VH
            delegatesTbl1.Add((Action)SBI_A_n);     // Opcode 0xB1, SBI A,n
            delegatesTbl1.Add((Action)ADC_VH);      // Opcode 0xB2, ADC VH
            delegatesTbl1.Add((Action)ADI_A_n);     // Opcode 0xB3, ADI A,n
            delegatesTbl1.Add((Action)LDA_VH);      // Opcode 0xB4, LDA VH
            delegatesTbl1.Add((Action)LDI_A_n);     // Opcode 0xB5, LDI A,n
            delegatesTbl1.Add((Action)CPA_VH);      // Opcode 0xB6, CPA VH
            delegatesTbl1.Add((Action)CPI_A_n);     // Opcode 0xB7, CPI A,n
            delegatesTbl1.Add((Action)RPV);         // Opcode 0xB8, RPV
            delegatesTbl1.Add((Action)ANI_A_n);     // Opcode 0xB9, ANI A,n
            delegatesTbl1.Add((Action)JMP_pp);      // Opcode 0xBA, JMP pp
            delegatesTbl1.Add((Action)ORI_A_n);     // Opcode 0xBB, ORI A,n
            delegatesTbl1.Add((Action)DCA_V_ME0);   // Opcode 0xBC, DCA (V)
            delegatesTbl1.Add((Action)EAI_n);       // Opcode 0xBD, EAI n
            delegatesTbl1.Add((Action)SJP_pp);      // Opcode 0xBE, SJP pp
            delegatesTbl1.Add((Action)BII_A_n);     // Opcode 0xBF, BII A,n
            #endregion Opcodes_0xB0-0xBF
 
            #region Opcodes_0xC0-0xCF
            delegatesTbl1.Add((Action)VEJ_C0);      // Opcode 0xC0, VEJ (C0)
            delegatesTbl1.Add((Action)VCR_n);       // Opcode 0xC1, VCR n
            delegatesTbl1.Add((Action)VEJ_C2);      // Opcode 0xC2, VEJ (C2)
            delegatesTbl1.Add((Action)VCS_n);       // Opcode 0xC3, VCS n
            delegatesTbl1.Add((Action)VEJ_C4);      // Opcode 0xC4, VEJ (C4)
            delegatesTbl1.Add((Action)VHR_n);       // Opcode 0xC5, VHR n
            delegatesTbl1.Add((Action)VEJ_C6);      // Opcode 0xC6, VEJ (C6)
            delegatesTbl1.Add((Action)VHS_n);       // Opcode 0xC7, VHS n
            delegatesTbl1.Add((Action)VEJ_C8);      // Opcode 0xC8, VEJ (C8)
            delegatesTbl1.Add((Action)VZR_n);       // Opcode 0xC9, VZR n
            delegatesTbl1.Add((Action)VEJ_CA);      // Opcode 0xCA, VEJ (CA)
            delegatesTbl1.Add((Action)VZS_n);       // Opcode 0xCB, VZS n
            delegatesTbl1.Add((Action)VEJ_CC);      // Opcode 0xCC, VEJ (CC)
            delegatesTbl1.Add((Action)VMJ_n);       // Opcode 0xCD, VMJ n
            delegatesTbl1.Add((Action)VEJ_CE);      // Opcode 0xCE, VEJ (CE)
            delegatesTbl1.Add((Action)VVS_n);       // Opcode 0xCF, VVS n
            #endregion Opcodes_0xC0-0xCF

            #region Opcodes_0xD0-0xDF
            delegatesTbl1.Add((Action)VEJ_D0);      // Opcode 0xD0, VEJ (D0)
            delegatesTbl1.Add((Action)ROR);         // Opcode 0xD1, ROR
            delegatesTbl1.Add((Action)VEJ_D2);      // Opcode 0xD2, VEJ (D2)
            delegatesTbl1.Add((Action)DDR_X_ME0);   // Opcode 0xD3, DDR (X)
            delegatesTbl1.Add((Action)VEJ_D4);      // Opcode 0xD4, VEJ (D4)
            delegatesTbl1.Add((Action)SHR);         // Opcode 0xD5, SHR
            delegatesTbl1.Add((Action)VEJ_D6);      // Opcode 0xD6, VEJ (D6)
            delegatesTbl1.Add((Action)DRL_X_ME0);   // Opcode 0xD7, DRL (X)
            delegatesTbl1.Add((Action)VEJ_D8);      // Opcode 0xD8, VEJ (D8)
            delegatesTbl1.Add((Action)SHL);         // Opcode 0xD9, SHL
            delegatesTbl1.Add((Action)VEJ_DA);      // Opcode 0xDA, VEJ (DA)
            delegatesTbl1.Add((Action)ROL);         // Opcode 0xDB, ROL
            delegatesTbl1.Add((Action)VEJ_DC);      // Opcode 0xDC, VEJ (DC)
            delegatesTbl1.Add((Action)INC_A);       // Opcode 0xDD, INC A
            delegatesTbl1.Add((Action)VEJ_DE);      // Opcode 0xDE, VEJ (DE)
            delegatesTbl1.Add((Action)DEC_A);       // Opcode 0xDF,DEC A
            #endregion Opcodes_0xD0-0xDF

            #region Opcodes_0xE0-0xEF
            delegatesTbl1.Add((Action)VEJ_E0);      // Opcode 0xE0, VEJ (E0)
            delegatesTbl1.Add((Action)SPU);         // Opcode 0xE1, SPU
            delegatesTbl1.Add((Action)VEJ_E2);      // Opcode 0xE2, VEJ (E2)
            delegatesTbl1.Add((Action)RPU);         // Opcode 0xE3, RPU
            delegatesTbl1.Add((Action)VEJ_E4);      // Opcode 0xE4, VEJ (E4)
            delegatesTbl1.Add((Action)NOP);         // Opcode 0xE5
            delegatesTbl1.Add((Action)VEJ_E6);      // Opcode 0xE6, VEJ (E6)
            delegatesTbl1.Add((Action)NOP);         // Opcode 0xE7
            delegatesTbl1.Add((Action)VEJ_E8);      // Opcode 0xE8, VEJ (E8)
            delegatesTbl1.Add((Action)ANI_pp_n_ME0);// Opcode 0xE9, ANI (pp),n
            delegatesTbl1.Add((Action)VEJ_EA);      // Opcode 0xEA, VEJ (EA)
            delegatesTbl1.Add((Action)ORI_pp_n_ME0);// Opcode 0xEB, ORI (pp),n
            delegatesTbl1.Add((Action)VEJ_EC);      // Opcode 0xEC, VEJ (EC)
            delegatesTbl1.Add((Action)BII_pp_n_ME0);// Opcode 0xED, BII (pp),n
            delegatesTbl1.Add((Action)VEJ_EE);      // Opcode 0xEE, VEJ (EE)
            delegatesTbl1.Add((Action)ADI_pp_n_ME0);// Opcode 0xEF, ADI (pp),n
            #endregion Opcodes_0xE0-0xEF

            #region Opcodes_0xF0-0xFF
            delegatesTbl1.Add((Action)VEJ_F0);      // Opcode 0xF0, VEJ (F0)
            delegatesTbl1.Add((Action)AEX);         // Opcode 0xF1
            delegatesTbl1.Add((Action)VEJ_F2);      // Opcode 0xF2, VEJ (F2)
            delegatesTbl1.Add((Action)NOP);         // Opcode 0xF3
            delegatesTbl1.Add((Action)VEJ_F4);      // Opcode 0xF4, VEJ (F4)
            delegatesTbl1.Add((Action)TIN);         // Opcode 0xF5, TIN
            delegatesTbl1.Add((Action)VEJ_F6);      // Opcode 0xF6, VEJ (F6)
            delegatesTbl1.Add((Action)CIN);         // Opcode 0xF7, CIN
            delegatesTbl1.Add((Action)VEJ_F8);      // Opcode 0xF8, VEJ (F8)
            delegatesTbl1.Add((Action)REC);         // Opcode 0xF9, REC
            delegatesTbl1.Add((Action)VEJ_FA);      // Opcode 0xFA, VEJ (FA)
            delegatesTbl1.Add((Action)SEC);         // Opcode 0xFB, SEC
            delegatesTbl1.Add((Action)VEJ_FC);      // Opcode 0xFC, VEJ (FC)
            delegatesTbl1.Add((Action)FD_P2);       // Opcode 0xFD, FD xx
            delegatesTbl1.Add((Action)VEJ_FE);      // Opcode 0xFE, VEJ (FE)
            delegatesTbl1.Add((Action)NOP);         // Opcode 0xFF
            #endregion Opcodes_0xF0-0xFF

            #endregion Opcodes Table 1 0x00-0xFF

            #region Opcode Table 2 0xFD00-0xFDFF

            #region Opcodes_0xFD00-0xFD0F
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 00
            delegatesTbl2.Add((Action)SBC_X_ME1);   // Opcode FD 01, SBC #(X)
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 02
            delegatesTbl2.Add((Action)ADC_X_ME1);   // Opcode FD 03, ADC #(X)
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 04
            delegatesTbl2.Add((Action)LDA_X_ME1);   // Opcode FD 05, LDA #(X)
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 06
            delegatesTbl2.Add((Action)CPA_X_ME1);   // Opcode FD 07, CPA #(X)
            delegatesTbl2.Add((Action)LDX_X);       // Opcode FD 08, LDX X
            delegatesTbl2.Add((Action)AND_X_ME1);   // Opcode FD 09
            delegatesTbl2.Add((Action)POP_X);       // Opcode FD 0A, POP X
            delegatesTbl2.Add((Action)ORA_X_ME1);   // Opcode FD 0B, ORA #(X)
            delegatesTbl2.Add((Action)DCS_X_ME1);   // Opcode FD 0C, DCS #(X)
            delegatesTbl2.Add((Action)EOR_X_ME1);   // Opcode FD 0D, EOR #(X)
            delegatesTbl2.Add((Action)STA_X_ME1);   // Opcode FD 0E, STA #(X)
            delegatesTbl2.Add((Action)BIT_X_ME1);   // Opcode FD 0F, BIT #(X)
            #endregion Opcodes_0x00-0x0F

            #region Opcodes_0xFD10-0xFD1F
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 10
            delegatesTbl2.Add((Action)SBC_Y_ME1);   // Opcode FD 11, SBC #(Y)
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 12
            delegatesTbl2.Add((Action)ADC_Y_ME1);   // Opcode FD 13, ADC #(Y)
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 14
            delegatesTbl2.Add((Action)LDA_Y_ME1);   // Opcode FD 15, LDA #(Y)
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 16
            delegatesTbl2.Add((Action)CPA_Y_ME1);   // Opcode FD 17, CPA #(Y)
            delegatesTbl2.Add((Action)LDX_Y);       // Opcode FD 18, LDX Y
            delegatesTbl2.Add((Action)AND_Y_ME1);   // Opcode FD 19
            delegatesTbl2.Add((Action)POP_Y);       // Opcode FD 1A. POP Y
            delegatesTbl2.Add((Action)ORA_Y_ME1);   // Opcode FD 1B, ORA #(Y)
            delegatesTbl2.Add((Action)DCS_Y_ME1);   // Opcode FD 1C, DCS #(Y)
            delegatesTbl2.Add((Action)EOR_Y_ME1);   // Opcode FD 1D, EOR #(Y)
            delegatesTbl2.Add((Action)STA_Y_ME1);   // Opcode FD 1E, STA #(Y)
            delegatesTbl2.Add((Action)BIT_Y_ME1);   // Opcode FD 1F, BIT #(Y)
            #endregion Opcodes_0x10-0x1F

            #region Opcodes_0xFD20-0xFD2F
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 20
            delegatesTbl2.Add((Action)SBC_U_ME1);   // Opcode FD 21, SBC #(U)
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 22
            delegatesTbl2.Add((Action)ADC_U_ME1);   // Opcode FD 23, ADC #(U)
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 24
            delegatesTbl2.Add((Action)LDA_U_ME1);   // Opcode FD 25, LDA #(U)
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 26
            delegatesTbl2.Add((Action)CPA_U_ME1);   // Opcode FD 27, CPA #(U)
            delegatesTbl2.Add((Action)LDX_U);       // Opcode FD 28, LDX U
            delegatesTbl2.Add((Action)AND_U_ME1);   // Opcode FD 29
            delegatesTbl2.Add((Action)POP_U);       // Opcode FD 2A, POP U
            delegatesTbl2.Add((Action)ORA_U_ME1);   // Opcode FD 2B, ORA #(U)
            delegatesTbl2.Add((Action)DCS_U_ME1);   // Opcode FD 2C, DCS #(U)
            delegatesTbl2.Add((Action)EOR_U_ME1);   // Opcode FD 2D, EOR #(U)
            delegatesTbl2.Add((Action)STA_U_ME1);   // Opcode FD 2E, STA #(U)
            delegatesTbl2.Add((Action)BIT_U_ME1);   // Opcode FD 2F, BIT #(U)
            #endregion Opcodes_0x20-0x2F

            #region Opcodes_0xFD30-0xFD3F
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 30
            delegatesTbl2.Add((Action)SBC_V_ME1);   // Opcode FD 31, SBC #(V)
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 32
            delegatesTbl2.Add((Action)ADC_V_ME1);   // Opcode FD 33, ADC #(V)
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 34
            delegatesTbl2.Add((Action)LDA_V_ME1);   // Opcode FD 35, LDA #(V)
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 36
            delegatesTbl2.Add((Action)CPA_V_ME1);   // Opcode FD 37, CPA #(V)
            delegatesTbl2.Add((Action)LDX_V);       // Opcode FD 38, LDX V
            delegatesTbl2.Add((Action)AND_V_ME1);   // Opcode FD 39
            delegatesTbl2.Add((Action)POP_V);       // Opcode FD 3A, POP V
            delegatesTbl2.Add((Action)ORA_V_ME1);   // Opcode FD 3B, ORA #(V)
            delegatesTbl2.Add((Action)DCS_V_ME1);   // Opcode FD 3C, DCS #(V)
            delegatesTbl2.Add((Action)EOR_V_ME1);   // Opcode FD 3D, EOR #(V)
            delegatesTbl2.Add((Action)STA_V_ME1);   // Opcode FD 3E, STA #(X)
            delegatesTbl2.Add((Action)BIT_V_ME1);   // Opcode FD 3F, BIT #(V)
            #endregion Opcodes_0x30-0x3F

            #region Opcodes_0xFD40-0xFD4F
            delegatesTbl2.Add((Action)INC_XH);      // Opcode FD 40, INC XH
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 41
            delegatesTbl2.Add((Action)DEC_XH);      // Opcode FD 42, DEC XH
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 43
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 44
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 45
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 46
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 47
            delegatesTbl2.Add((Action)LDX_S);       // Opcode FD 48, LDX S
            delegatesTbl2.Add((Action)ANI_X_n_ME1); // Opcode FD 49, ANI #(X),n
            delegatesTbl2.Add((Action)STX_X);       // Opcode FD 4A, STX X
            delegatesTbl2.Add((Action)ORI_X_n_ME1); // Opcode FD 4B, ORI #(X),n
            delegatesTbl2.Add((Action)OFF);         // Opcode FD 4C, OFF
            delegatesTbl2.Add((Action)BII_X_n_ME1); // Opcode FD 4D, BII #(X),n
            delegatesTbl2.Add((Action)STX_S);       // Opcode FD 4E, STX S
            delegatesTbl2.Add((Action)ADI_X_n_ME1); // Opcode FD 4F, ADI #(X),n
            #endregion Opcodes_0x40-0x4F

            #region Opcodes_0xFD50-0xFD5F
            delegatesTbl2.Add((Action)INC_YH);      // Opcode FD 50, INC YH
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 51
            delegatesTbl2.Add((Action)DEC_YH);      // Opcode FD 52, DEC YH
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 53
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 54
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 55
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 56
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 57
            delegatesTbl2.Add((Action)LDX_P);       // Opcode FD 58, LDX P
            delegatesTbl2.Add((Action)ANI_Y_n_ME1); // Opcode FD 59, ANI #(Y),n
            delegatesTbl2.Add((Action)STX_Y);       // Opcode FD 5A, STX Y
            delegatesTbl2.Add((Action)ORI_Y_n_ME1); // Opcode FD 5B, ORI #(Y),n
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 5C
            delegatesTbl2.Add((Action)BII_Y_n_ME1); // Opcode FD 5D, BII #(Y),n
            delegatesTbl2.Add((Action)STX_P);       // Opcode FD 5E, STX P
            delegatesTbl2.Add((Action)ADI_Y_n_ME1); // Opcode FD 5F, ADI #(Y),n
            #endregion Opcodes_0x50-0x5F

            #region Opcodes_0xFD60-0xFD6F
            delegatesTbl2.Add((Action)INC_UH);      // Opcode FD 60, INC UH
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 61
            delegatesTbl2.Add((Action)DEC_UH);      // Opcode FD 62, DEC UH
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 63
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 64
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 65
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 66
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 67
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 68
            delegatesTbl2.Add((Action)ANI_U_n_ME1); // Opcode FD 69, ANI #(U),n
            delegatesTbl2.Add((Action)STX_U);       // Opcode FD 6A, STX U
            delegatesTbl2.Add((Action)ORI_U_n_ME1); // Opcode FD 6B, ORI #(U),n
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 6C
            delegatesTbl2.Add((Action)BII_U_n_ME1); // Opcode FD 6D, BII #(U),n
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 6E
            delegatesTbl2.Add((Action)ADI_U_n_ME1); // Opcode FD 6F, ADI #(U),n
            #endregion Opcodes_0x60-0x6F

            #region Opcodes_0xFD70-0xFD7F
            delegatesTbl2.Add((Action)INC_VH);      // Opcode FD 70, INC VH
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 71
            delegatesTbl2.Add((Action)DEC_VH);      // Opcode FD 72, DEC VH
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 73
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 74
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 75
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 76
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 77
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 78
            delegatesTbl2.Add((Action)ANI_V_n_ME1); // Opcode FD 79, ANI #(V),n
            delegatesTbl2.Add((Action)STX_V);       // Opcode FD 7A, STX V
            delegatesTbl2.Add((Action)ORI_V_n_ME1); // Opcode FD 7B, ORI #(V),n
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 7C
            delegatesTbl2.Add((Action)BII_V_n_ME1); // Opcode FD 7D, BII #(V),n
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 7E
            delegatesTbl2.Add((Action)ADI_V_n_ME1); // Opcode FD 7F, ADI #(V),n
            #endregion Opcodes_0x70-0x7F

            #region Opcodes_0xFD80-0xFD8F
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 80
            delegatesTbl2.Add((Action)SIE);         // Opcode FD 81. SIE
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 82
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 83
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 84
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 85
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 86
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 87
            delegatesTbl2.Add((Action)PSH_X);       // Opcode FD 88, PSH X
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 89
            delegatesTbl2.Add((Action)POP_A);       // Opcode FD 8A, POP A
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 8B
            delegatesTbl2.Add((Action)DCA_X_ME1);   // Opcode FD 8C, DCA #(X)
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 8D
            delegatesTbl2.Add((Action)CDV);         // Opcode FD 8E, CDV
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 8F
            #endregion Opcodes_0x80-0x8F

            #region Opcodes_0xFD90-0xFD9F
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 90
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 91
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 92
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 93
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 94
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 95
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 96
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 97
            delegatesTbl2.Add((Action)PSH_Y);       // Opcode FD 98, PSH Y
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 99
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 9A
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 9B
            delegatesTbl2.Add((Action)DCA_Y_ME1);   // Opcode FD 9C, DCA #(Y)
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 9D
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 9E
            delegatesTbl2.Add((Action)NOP);         // Opcode FD 9F
            #endregion Opcodes_0x90-0x9F

            #region Opcodes_0xFDA0-0xFDAF
            delegatesTbl2.Add((Action)NOP);         // Opcode FD A0
            delegatesTbl2.Add((Action)SBC_pp_ME1);  // Opcode FD A1, SBC #(pp)
            delegatesTbl2.Add((Action)NOP);         // Opcode FD A2
            delegatesTbl2.Add((Action)ADC_pp_ME1);  // Opcode FD A3, ADC #(pp)
            delegatesTbl2.Add((Action)NOP);         // Opcode FD A4
            delegatesTbl2.Add((Action)LDA_pp_ME1);  // Opcode FD A5, LDA #(pp)
            delegatesTbl2.Add((Action)NOP);         // Opcode FD A6
            delegatesTbl2.Add((Action)CPA_pp_ME1);  // Opcode FD A7, CPA #(pp)
            delegatesTbl2.Add((Action)PSH_U);       // Opcode FD A8, PSH U
            delegatesTbl2.Add((Action)AND_pp_ME1);  // Opcode FD A9
            delegatesTbl2.Add((Action)TTA);         // Opcode FD AA, TTA
            delegatesTbl2.Add((Action)ORA_pp_ME1);  // Opcode FD AB, ORA #(pp)
            delegatesTbl2.Add((Action)DCA_U_ME1);   // Opcode FD AC, DCA #(U)
            delegatesTbl2.Add((Action)EOR_pp_ME1);  // Opcode FD AD, EOR #(pp)
            delegatesTbl2.Add((Action)STA_pp_ME1);  // Opcode FD AE, STA #(pp)
            delegatesTbl2.Add((Action)BIT_pp_ME1);  // Opcode FD AF, BIT #(pp)
            #endregion Opcodes_0xA0-0xAF

            #region Opcodes_0xFDB0-0xFDBF
            delegatesTbl2.Add((Action)NOP);         // Opcode FD B0
            delegatesTbl2.Add((Action)HLT);         // Opcode FD B1, HLT
            delegatesTbl2.Add((Action)NOP);         // Opcode FD B2
            delegatesTbl2.Add((Action)NOP);         // Opcode FD B3
            delegatesTbl2.Add((Action)NOP);         // Opcode FD B4
            delegatesTbl2.Add((Action)NOP);         // Opcode FD B5
            delegatesTbl2.Add((Action)NOP);         // Opcode FD B6
            delegatesTbl2.Add((Action)NOP);         // Opcode FD B7
            delegatesTbl2.Add((Action)PSH_V);       // Opcode FD B8, PSH V
            delegatesTbl2.Add((Action)NOP);         // Opcode FD B9
            delegatesTbl2.Add((Action)ITA);         // Opcode FD BA, ITA
            delegatesTbl2.Add((Action)NOP);         // Opcode FD BB
            delegatesTbl2.Add((Action)DCA_V_ME1);   // Opcode FD BC, DCA #S(V)
            delegatesTbl2.Add((Action)NOP);         // Opcode FD BD
            delegatesTbl2.Add((Action)RIE);         // Opcode FD BE, RIE
            delegatesTbl2.Add((Action)NOP);         // Opcode FD BF
            #endregion Opcodes_0xB0-0xBF

            #region Opcodes_0xFDC0-0xFDCF
            delegatesTbl2.Add((Action)RDP);         // Opcode FD C0, RDP
            delegatesTbl2.Add((Action)SDP);         // Opcode FD C1, SDP
            delegatesTbl2.Add((Action)NOP);         // Opcode FD C2
            delegatesTbl2.Add((Action)NOP);         // Opcode FD C3
            delegatesTbl2.Add((Action)NOP);         // Opcode FD C4
            delegatesTbl2.Add((Action)NOP);         // Opcode FD C5
            delegatesTbl2.Add((Action)NOP);         // Opcode FD C6
            delegatesTbl2.Add((Action)NOP);         // Opcode FD C7
            delegatesTbl2.Add((Action)PSH_A);       // Opcode FD C8, PSH A
            delegatesTbl2.Add((Action)NOP);         // Opcode FD C9
            delegatesTbl2.Add((Action)ADR_X);       // Opcode FD CA, ADR X
            delegatesTbl2.Add((Action)NOP);         // Opcode FD CB
            delegatesTbl2.Add((Action)ATP);         // Opcode FD CC, ATP
            delegatesTbl2.Add((Action)NOP);         // Opcode FD CD
            delegatesTbl2.Add((Action)AM0);         // Opcode FD CE
            delegatesTbl2.Add((Action)NOP);         // Opcode FD CF
            #endregion Opcodes_0xC0-0xCF

            #region Opcodes_0xFDD0-0xFDDF
            delegatesTbl2.Add((Action)NOP);         // Opcode FD D0
            delegatesTbl2.Add((Action)NOP);         // Opcode FD D1
            delegatesTbl2.Add((Action)NOP);         // Opcode FD D2
            delegatesTbl2.Add((Action)DDR_X_ME1);   // Opcode FD D3, DDR #(X)
            delegatesTbl2.Add((Action)NOP);         // Opcode FD D4
            delegatesTbl2.Add((Action)NOP);         // Opcode FD D5
            delegatesTbl2.Add((Action)NOP);         // Opcode FD D6
            delegatesTbl2.Add((Action)DRL_X_ME1);   // Opcode FD D7, DRL #(X)
            delegatesTbl2.Add((Action)NOP);         // Opcode FD D8
            delegatesTbl2.Add((Action)NOP);         // Opcode FD D9
            delegatesTbl2.Add((Action)ADR_Y);       // Opcode FD DA, ADR Y
            delegatesTbl2.Add((Action)NOP);         // Opcode FD DB
            delegatesTbl2.Add((Action)NOP);         // Opcode FD DC
            delegatesTbl2.Add((Action)NOP);         // Opcode FD DD
            delegatesTbl2.Add((Action)AM1);         // Opcode FD DE
            delegatesTbl2.Add((Action)NOP);         // Opcode FD DF
            #endregion Opcodes_0xD0-0xDF

            #region Opcodes_0xFDE0-0xFDEF
            delegatesTbl2.Add((Action)NOP);         // Opcode FD E0
            delegatesTbl2.Add((Action)NOP);         // Opcode FD E1
            delegatesTbl2.Add((Action)NOP);         // Opcode FD E2
            delegatesTbl2.Add((Action)NOP);         // Opcode FD E3
            delegatesTbl2.Add((Action)NOP);         // Opcode FD E4
            delegatesTbl2.Add((Action)NOP);         // Opcode FD E5
            delegatesTbl2.Add((Action)NOP);         // Opcode FD E6
            delegatesTbl2.Add((Action)NOP);         // Opcode FD E7
            delegatesTbl2.Add((Action)NOP);         // Opcode FD E8
            delegatesTbl2.Add((Action)ANI_pp_n_ME1);// Opcode FD E9, ANI #(pp),n
            delegatesTbl2.Add((Action)ADR_U);       // Opcode FD EA, ADR U
            delegatesTbl2.Add((Action)ORI_pp_n_ME1);// Opcode FD EB, ORI #(pp),n
            delegatesTbl2.Add((Action)ATT);         // Opcode FD EC, ATT
            delegatesTbl2.Add((Action)BII_pp_n_ME1);// Opcode FD ED, BII #(pp),n
            delegatesTbl2.Add((Action)NOP);         // Opcode FD EE
            delegatesTbl2.Add((Action)ADI_pp_n_ME1);// Opcode FD EF, ADI (pp),n
            #endregion Opcodes_0xE0-0xEF

            #region Opcodes_0xFDF0-0xFDFF
            delegatesTbl2.Add((Action)NOP);         // Opcode FD F0
            delegatesTbl2.Add((Action)NOP);         // Opcode FD F1
            delegatesTbl2.Add((Action)NOP);         // Opcode FD F2
            delegatesTbl2.Add((Action)NOP);         // Opcode FD F3
            delegatesTbl2.Add((Action)NOP);         // Opcode FD F4
            delegatesTbl2.Add((Action)NOP);         // Opcode FD F5
            delegatesTbl2.Add((Action)NOP);         // Opcode FD F6
            delegatesTbl2.Add((Action)NOP);         // Opcode FD F7
            delegatesTbl2.Add((Action)NOP);         // Opcode FD F8
            delegatesTbl2.Add((Action)NOP);         // Opcode FD F9
            delegatesTbl2.Add((Action)ADR_V);       // Opcode FD FA, ADR V
            delegatesTbl2.Add((Action)NOP);         // Opcode FD FB
            delegatesTbl2.Add((Action)NOP);         // Opcode FD FC
            delegatesTbl2.Add((Action)NOP);         // Opcode FD FD
            delegatesTbl2.Add((Action)NOP);         // Opcode FD FE
            delegatesTbl2.Add((Action)NOP);         // Opcode FD FF
            #endregion Opcodes_0xF0-0xFF

            #endregion Opcodes Table 2 0xFD00-0xFDFF
        }


        #region Helpers

        /// <summary>
        /// Add two 8bit numbers
        /// Sets Carry, Half Carry, Overflow, Zero flags
        /// </summary>
        /// <param name="v1">Value 1</param>
        /// <param name="v2">Value 2</param>
        /// <returns>Sum of v1, v2</returns>
        private byte Add(byte v1, byte v2)
        {
            byte value = 0;

            int sum = v1 + v2 + (FLAGS.C ? 1 : 0);
            int halfCarry = (((v1 & 0x0F) + (v2 & 0x0F)) & 0x10);
            FLAGS.C = ((sum & 0xFF00) > 0) ? true : false;
            FLAGS.H = (halfCarry > 0);
            FLAGS.V = (((v1 ^ sum) & (v2 ^ sum) & 0x80) != 0) ? true : false;
            value = (byte)(sum & 0xFF);
            SetZFlag(value);

            return value;
        }

        /// <summary>
        /// BCD Addition of two 8bit numbers
        /// Sets Carry, Half Carry, Overflow, Zero flags
        /// </summary>
        /// <param name="v1">Value 1</param>
        /// <param name="v2">Value 2</param>
        /// <returns>Sum of v1, v2 in BCD format</returns>
        private byte BCDAdd(byte v1, byte v2)
        {
            byte value = 0;

            int sum = v1 + v2 + (FLAGS.C ? 1 : 0);
            int halfCarry = (((v1 & 0x0F) + (v2 & 0x0F)) & 0x10);

            if (((sum & 0x0F) > 0x09) | (halfCarry > 0))
            {
                sum += (byte)0x06; // decimal adjust low nibble
                FLAGS.H = true;
            }

            if ( (sum & 0xF0) > 0x90) { sum += (byte)0x60; } // decimal adjust high nibble

            FLAGS.C = ((sum & 0xFF00) > 0) ? true : false;
            FLAGS.V = (((v1 ^ sum) & (v2 ^ sum) & 0x80) != 0) ? true : false;
            value = (byte)(sum & 0xFF);

            SetZFlag(value);

            return value;
        }

        /// <summary>
        /// Subract, i.e. v1 - v2
        /// </summary>
        /// Calcualtes 2's compliment of V2 for the heck of it
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>v1-v2</returns>
        private byte Subtract(byte v1, byte v2)
        {
            byte value = 0;

            int v2c = (v2 ^ 0xFF) + 1;
            int sum = v1 + v2c + (FLAGS.C ? 1 : 0);
            int val2 = (((v1 & 0x0F) + (v2c & 0x0F)) & 0x10);
            FLAGS.C = ((sum & 0xFF00) > 0) ? false : true;
            FLAGS.H = (val2 > 0);
            FLAGS.V = (((v1 ^ sum) & (v2 ^ sum) & 0x80) != 0) ? true : false;
            value = (byte)(sum & 0xFF);

            SetZFlag(value);

            return value;
        }

        /// <summary>
        /// Subract, i.e. v1 - v2
        /// </summary>
        /// Calcualtes 2's compliment of V2 for the heck of it
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>v1-v2</returns>
        private byte BCDSubtract(byte v1, byte v2)
        {
            byte value = 0;

            int v2c = (v2 ^ 0xFF) + 1;
            int sum = v1 + v2c - (FLAGS.C ? 1 : 0);
            FLAGS.H = ((((v1 & 0x0F) + (v2c & 0x0F)) & 0x10) > 0);

            if (((sum & 0x0F) > 0x09) /*| (halfCarry > 0)*/)
            {
                sum -= (byte)0x06; // decimal adjust low nibble
                //FLAGS.H = true;
            } 

            if ((sum & 0xF0) > 0x90) { sum -= (byte)0x60; } // decimal adjust high nibble

            FLAGS.C = ((sum & 0xFF00) > 0) ? true : false;
            FLAGS.V = (((v1 ^ sum) & (v2 ^ sum) & 0x80) != 0) ? true : false;
            value = (byte)(sum & 0xFF);
            SetZFlag(value);

            return value;
        }

        /// <summary>
        /// Set Z Flag based on value of A register
        /// </summary>
        private void SetZFlag()
        {
            FLAGS.Z = (this.REG.A == 0);
        }

        /// <summary>
        /// Set Z Flag based on value passed
        /// </summary>
        private void SetZFlag(byte value)
        {
            FLAGS.Z = (value == 0);
        }

        /// <summary>
        /// Set Z Flag based on value passed
        /// </summary>
        private void SetZFlag(ushort value)
        {
            FLAGS.Z = (value == 0);
        }

        /// <summary>
        /// SetCompareFlags
        /// Compare value1 to value2, set C and Z flags
        /// </summary>
        private void SetCompareFlags(byte value1, byte value2)
        {

            if (value1 > value2)
            {
                FLAGS.C = true;
                FLAGS.Z = false;
            }
            else if (value1 == value2)
            {
                FLAGS.C = true;
                FLAGS.Z = true;
            }
            else
            {
                FLAGS.C = false;
                FLAGS.Z = false;
            }
        }

        /// <summary>
        /// Retrive byte from address P
        /// Advance Program Counter
        /// </summary>
        /// <returns></returns>
        private byte GetByte()
        {
            byte val = RAM_ME0[REG.P.R];
            REG.P.R += 1; // Advance Program Counter

            return val;
        }

        /// <summary>
        /// Retrive 16bit word (address, value) from address P, P+1
        /// Advance Program Counter
        /// </summary>
        /// <returns>Address</returns>
        private ushort GetWord()
        {
            ushort val = (ushort)((RAM_ME0[REG.P.R] << 8) | RAM_ME0[REG.P.R + 1]);
            REG.P.R += 2; // Advance Program Counter

            return val;
        }

        /// <summary>
        /// Retrive 16bit word from address 
        /// </summary>
        /// <returns>16bit value</returns>
        private ushort GetWord(ushort address)
        {
            return (ushort)((RAM_ME0[address] << 8) | RAM_ME0[address + 1]);
        }

        /// <summary>
        /// T Register, i.e. Flags
        /// </summary>
        /// <returns>Flags packed into byte</returns>
        public byte GetTREG()
        {
            byte value  = (byte)((FLAGS.H ? 1 : 0) << 4);
                 value |= (byte)((FLAGS.V ? 1 : 0) << 3);
                 value |= (byte)((FLAGS.Z ? 1 : 0) << 2);
                 value |= (byte)((FLAGS.IE ? 1 : 0) << 1);
                 value |= (byte)((FLAGS.C ? 1 : 0));

            return value;
        }

        /// <summary>
        /// Sets flags from saved values
        /// </summary>
        /// <param name="value">Flags packed into byte</param>
        private void SetTREG(byte value)
        {
            FLAGS.H  = (value & 0x10) > 0;
            FLAGS.V  = (value & 0x08) > 0;
            FLAGS.Z  = (value & 0x04) > 0;
            FLAGS.IE = (value & 0x02) > 0;
            FLAGS.C  = (value & 0x01) > 0;
        }

        #endregion Helpers

        #region UI Interface

        #region Flags

        /// <summary>
        /// Get state of Carry flag
        /// </summary>
        /// <returns>state of Carry Flag</returns>
        public bool GetCarryFlag()
        {
            return FLAGS.C;
        }

        /// <summary>
        /// Set state of Carry flag
        /// </summary>
        public void SetCarryFlag(bool state)
        {
            FLAGS.C = state;
        }

        /// <summary>
        /// Get state of InterruptEnable flag
        /// </summary>
        /// <returns>state of Carry Flag</returns>
        public bool GetInterruptEnableFlag()
        {
            return FLAGS.IE;
        }

        /// <summary>
        /// Set state of Interupt Enable flag
        /// </summary>
        public void SetInterruptEnableFlag(bool state)
        {
            FLAGS.IE = state;
        }

        /// <summary>
        /// Get state of Zero flag
        /// </summary>
        /// <returns>state of Carry Flag</returns>
        public bool GetZeroFlag()
        {
            return FLAGS.Z;
        }

        /// <summary>
        /// Set state of Zero flag
        /// </summary>
        public void SetZeroFlag(bool state)
        {
            FLAGS.Z = state;
        }

        /// <summary>
        /// Get state of Overflow flag
        /// </summary>
        /// <returns>state of Carry Flag</returns>
        public bool GetOverflowFlag()
        {
            return FLAGS.V;
        }

        /// <summary>
        /// Set state of Overflow flag
        /// </summary>
        public void SetOverflowFlag(bool state)
        {
            FLAGS.V = state;
        }

        /// <summary>
        /// Get state of HalfCarry flag
        /// </summary>
        /// <returns>state of Carry Flag</returns>
        public bool GetHalfCarryFlag()
        {
            return FLAGS.H;
        }

        /// <summary>
        /// Set state of Half Carry flag
        /// </summary>
        public void SetHalfCarryFlag(bool state)
        {
            FLAGS.H = state;
        }

        #endregion Flags

        #region Registers

        /// <summary>
        /// Returns a string representation of top 16 levels of the stack
        /// </summary>
        /// <returns></returns>
        public string GetStack()
        {
            StringBuilder stackString = new StringBuilder();

            for (ushort i = REG.S.R; i > (REG.S.R - 16); i--)
            {
                stackString.Append(RAM_ME0[i].ToString("X2"));
                if (StackWidth8) { stackString.Append(" "); }
            }

            return stackString.ToString();
        }

        #endregion Registers

        #region RAM Read/Write

        /// <summary>
        /// Read value at address from RAM bank ME1
        /// </summary>
        /// <param name="address"></param>
        /// <returns>Byte value at address</returns>
        public byte ReadRAM_ME0(ushort address)
        {
            return RAM_ME0[address];
        }

        /// <summary>
        /// Read value at address from RAM bank ME1
        /// </summary>
        /// <param name="address"></param>
        /// <returns>Byte value at address</returns>
        public byte ReadRAM_ME1(ushort address)
        {
            return RAM_ME1[address];
        }

        /// <summary>
        /// Write new byte value to address in RAM bank ME0
        /// </summary>
        /// <param name="address">Address to write to</param>
        /// <param name="value">Byte value</param>
        public void WriteRAM_ME0(ushort address, byte value)
        {
            RAM_ME0[address] = value;
        }

        /// <summary>
        /// Write new byte values starting at address in RAM bank ME0
        /// </summary>
        /// <param name="address">Starting address</param>
        /// <param name="values">Byte array to write</param>
        public void WriteRAM_ME0(ushort address, byte[] values)
        {
            for (int i=0; i < values.Length; i++)
            {
                RAM_ME0[address + i] = values[i];
            }
        }

        /// <summary>
        /// Write new byte value to address in RAM bank ME0
        /// </summary>
        /// <param name="address">Address to write to</param>
        /// <param name="value">Byte value</param>
        public void WriteRAM_ME1(ushort address, byte value)
        {
            RAM_ME1[address] = value;
        }

        /// <summary>
        /// Write new byte values starting at address in RAM bank ME0
        /// </summary>
        /// <param name="address">Starting address</param>
        /// <param name="values">Byte array to write</param>
        public void WriteRAM_ME1(ushort address, byte[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                RAM_ME1[address + i] = values[i];
            }
        }

        #endregion RAM Read/Write

        /// <summary>
        /// Run code at current progrma counter 
        /// Single step if SingleStep is set
        /// </summary>
        public void Run()
        {
            //do
            //{
                byte opcode = RAM_ME0[REG.P.R];
                delegatesTbl1[opcode].DynamicInvoke();
            //} while (!SingleStep);

        }

        #endregion UI Interface


        #region OPCODES 0x00-0xFF

        #region Opcodes_0x00-0x0F

        /// <summary>
        /// SBC XL
        /// A = A - XL
        /// Opcode 00, Bytes 1
        /// </summary>
        public void SBC_XL()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Subtract(this.REG.A, this.REG.X.RL);
            tick += 6;
            // flags set by subtraction function
        }

        /// <summary>
        /// SBC (X)
        /// A = A - (X)
        /// Opcode 01, Bytes 1
        /// </summary>
        public void SBC_X_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Subtract(this.REG.A, RAM_ME0[REG.X.R]);
            tick += 7;
            // flags set by subtraction function
        }

        /// <summary>
        /// ADC XL
        /// A = A + XL
        /// Opcode 0x02, Bytes 1
        /// </summary>
        private void ADC_XL()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Add(this.REG.A, this.REG.X.RL);
            tick += 6;
            // flags set by addition function
        }

        /// <summary>
        /// ADC (X)
        /// A = A + (X)
        /// Opcode 0x03, Bytes 1
        /// </summary>
        private void ADC_X_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Add(this.REG.A, this.RAM_ME0[REG.X.R]);
            
            // flags set by addition function
        }

        /// <summary>
        /// LDA XL
        /// A = XL
        /// Opcode 0x04, Bytes 1
        /// </summary>
        private void LDA_XL()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = this.REG.X.RL;
            SetZFlag();
            tick += 5;
        }

        /// <summary>
        /// LDA (X)
        /// A = (X)
        /// Opcode 0x05, Bytes 1
        /// </summary>
        private void LDA_X_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)this.REG.X.R;
            this.REG.A = RAM_ME0[address];
            SetZFlag();
            tick += 6;
        }

        /// <summary>
        /// CPA XL
        /// Opcode 06, Bytes 1
        /// Compare of A + XL
        /// </summary>
        private void CPA_XL()
        {
            REG.P.R += 1; // advance Program Counter
            SetCompareFlags(REG.A, REG.X.RL);
            tick += 6;
        }

        /// <summary>
        /// CPA (X)
        /// Opcode 07, Bytes 1
        /// Compare A + (X)
        /// </summary>
        private void CPA_X_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)REG.X.R;
            SetCompareFlags(REG.A, RAM_ME0[address]);
            tick += 7;
        }

        /// <summary>
        /// STA XH
        /// XH = A 
        /// Opcode 0x08, Bytes 1
        /// </summary>
        private void STA_XH()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.X.RH = this.REG.A;
            tick += 5;
            // no flag changes
        }

        /// <summary>
        /// AND (X)
        /// A = A & (X) 
        /// Opcode 0x09, Bytes 1
        /// </summary>
        private void AND_X_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = (byte)(this.REG.A & RAM_ME0[this.REG.X.R]);
            SetZFlag();
            tick += 7;
        }

        /// <summary>
        /// STA XL
        /// XL = A 
        /// Opcode 0x0A, Bytes 1
        /// </summary>
        private void STA_XL()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.X.RL = this.REG.A;
            tick += 5;
            // no flg changes
        }

        /// <summary>
        /// ORA (X)
        /// A = A | (X)
        /// Opcode 0B, Bytes 1
        /// </summary>
        private void ORA_X_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = (byte)(this.REG.A | RAM_ME0[REG.X.R]);
            SetZFlag();
            tick += 7;
        }

        /// <summary>
        /// DCS (X)
        /// A = A - (X) BCD Subtraction
        /// Opcode 0C, Bytes 1
        /// </summary>
        private void DCS_X_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME0[REG.X.R];
            this.REG.A = BCDSubtract(this.REG.A, value);
            tick += 13;
            // flags set by subtraction function
        }

        /// <summary>
        /// EOR (X)
        /// A = A ^ (X)
        /// Opcode 0D, Bytes 1
        /// </summary>
        private void EOR_X_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME0[REG.X.R];
            this.REG.A = (byte)(this.REG.A ^ value);
            SetZFlag();
            tick += 7;
        }

        /// <summary>
        /// STA (X)
        /// (X) = A 
        /// Opcode 0x0E, Bytes 1
        /// </summary>
        private void STA_X_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            RAM_ME0[REG.X.R] = this.REG.A;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// BIT (X)
        /// ZFLAG = A & (X)
        /// Opcode 0F, Bytes 1
        /// </summary>
        private void BIT_X_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)this.REG.X.R;
            byte result = (byte)(this.REG.A & RAM_ME0[address]);
            SetZFlag(result);
            tick += 7;
        }

        #endregion Opcodes_0x00-0x0F

        #region Opcodes_0x10-0x1F

        /// <summary>
        /// SBC YL
        /// A = A - YL
        /// Opcode 10, Bytes 1
        /// </summary>
        public void SBC_YL()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Subtract(this.REG.A, this.REG.Y.RL);
            tick += 6;
            // flags set by subtraction function
        }

        /// <summary>
        /// SBC (Y)
        /// A = A - (Y)
        /// Opcode 11, Bytes 1
        /// </summary>
        public void SBC_Y_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Subtract(this.REG.A, RAM_ME0[REG.Y.R]);
            tick += 7;
            // flags set by subtraction function
        }

        /// <summary>
        /// ADC YL
        /// A = A + YL
        /// Opcode 12, Bytes 1
        /// </summary>
        private void ADC_YL()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Add(this.REG.A, this.REG.Y.RL);
            tick += 6;
            // flags set by addition function
        }

        /// <summary>
        /// ADC (Y)
        /// A = A + (Y)
        /// Opcode 13, Bytes 1
        /// </summary>
        private void ADC_Y_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Add(this.REG.A, this.RAM_ME0[REG.Y.R]);
            // flags set by addition function
        }

        /// <summary>
        /// LDA YL
        /// A = YL
        /// Opcode 0x14, Bytes 1
        /// </summary>
        private void LDA_YL()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = this.REG.Y.RL;
            SetZFlag();
            tick += 5;
        }

        /// <summary>
        /// LDA (Y)
        /// A = (Y)
        /// Opcode 0x15, Bytes 1
        /// </summary>
        private void LDA_Y_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)this.REG.Y.R;
            this.REG.A = RAM_ME0[address];
            SetZFlag();
            tick += 6;
        }

        /// <summary>
        /// CPA YL
        /// Opcode 16, Bytes 1
        /// Compare of A + YL
        /// </summary>
        private void CPA_YL()
        {
            REG.P.R += 1; // advance Program Counter
            SetCompareFlags(REG.A, REG.Y.RL);
            tick += 6;
        }

        /// <summary>
        /// CPA (Y)
        /// Opcode 17, Bytes 1
        /// Compare A + (Y)
        /// </summary>
        private void CPA_Y_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)REG.Y.R;
            SetCompareFlags(REG.A, RAM_ME0[address]);
            tick += 7;
        }

        /// <summary>
        /// STA YH
        /// YH = A 
        /// Opcode 0x18, Bytes 1
        /// </summary>
        private void STA_YH()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.Y.RH = this.REG.A;
            tick += 5;
            // no flag changes
        }

        /// <summary>
        /// AND (Y)
        /// A = A & (Y) 
        /// Opcode 0x19, Bytes 1
        /// </summary>
        private void AND_Y_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = (byte)(this.REG.A & RAM_ME0[this.REG.Y.R]);
            SetZFlag();
            tick += 7;
        }

        /// <summary>
        /// STA YL
        /// YL = A 
        /// Opcode 0x1A, Bytes 1
        /// </summary>
        private void STA_YL()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.Y.RL = this.REG.A;
            tick += 5;
            // no flg changes
        }

        /// <summary>
        /// ORA (Y)
        /// A = A | (Y)
        /// Opcode 1B, Bytes 1
        /// </summary>
        private void ORA_Y_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = (byte)(this.REG.A | RAM_ME0[REG.Y.R]);
            SetZFlag();
            tick += 7;
        }

        /// <summary>
        /// DCS (Y)
        /// A = A - (Y) BCD Subtraction
        /// Opcode 1C, Bytes 1
        /// </summary>
        private void DCS_Y_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME0[REG.Y.R];
            this.REG.A = BCDSubtract(this.REG.A, value);
            tick += 13;
            // flags set by addition function
        }

        /// <summary>
        /// EOR (Y)
        /// A = A ^ (Y)
        /// Opcode 1D, Bytes 1
        /// </summary>
        private void EOR_Y_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME0[REG.Y.R];
            this.REG.A = (byte)(this.REG.A ^ value);
            SetZFlag();
            tick += 7;
        }

        /// <summary>
        /// STA (Y)
        /// (Y) = A 
        /// Opcode 0x1E, Bytes 1
        /// </summary>
        private void STA_Y_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            RAM_ME0[REG.Y.R] = this.REG.A;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// BIT (Y)
        /// ZFLAG = A & (Y)
        /// Opcode 1F, Bytes 1
        /// </summary>
        private void BIT_Y_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)this.REG.Y.R;
            byte result = (byte)(this.REG.A & RAM_ME0[address]);
            SetZFlag(result);
            tick += 7;
        }

        #endregion Opcodes_0x10-0x1F

        #region Opcodes_0x20-0x2F

        /// <summary>
        /// SBC UL
        /// A = A - UL
        /// Opcode 20, Bytes 1
        /// </summary>
        public void SBC_UL()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Subtract(this.REG.A, this.REG.U.RL);
            tick += 6;
            // flags set by subtraction function
        }

        /// <summary>
        /// SBC (U)
        /// A = A - (U)
        /// Opcode 21, Bytes 1
        /// </summary>
        public void SBC_U_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Subtract(this.REG.A, RAM_ME0[REG.U.R]);
            tick += 7;
            // flags set by subtraction function
        }

        /// <summary>
        /// ADC UL
        /// A = A + UL
        /// Opcode 22, Bytes 1
        /// </summary>
        private void ADC_UL()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Add(this.REG.A, this.REG.U.RL);
            tick += 6;
            // flags set by addition function
        }

        /// <summary>
        /// ADC (U)
        /// A = A + (U)
        /// Opcode 23, Bytes 1
        /// </summary>
        private void ADC_U_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Add(this.REG.A, this.RAM_ME0[REG.U.R]);
            // flags set by addition function
        }

        /// <summary>
        /// LDA UL
        /// A = UL
        /// Opcode 0x24, Bytes 1
        /// </summary>
        private void LDA_UL()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = this.REG.U.RL;
            SetZFlag();
            tick += 5;
        }

        /// <summary>
        /// LDA (U)
        /// A = (U)
        /// Opcode 0x25, Bytes 1
        /// </summary>
        private void LDA_U_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)this.REG.U.R;
            this.REG.A = RAM_ME0[address];
            SetZFlag();
            tick += 6;
        }

        /// <summary>
        /// CPA UL
        /// Opcode 26, Bytes 1
        /// Compare of A + UL
        /// </summary>
        private void CPA_UL()
        {
            REG.P.R += 1; // advance Program Counter
            SetCompareFlags(REG.A, REG.U.RL);
            tick += 6;
        }

        /// <summary>
        /// CPA (U)
        /// Opcode 27, Bytes 1
        /// Compare A + (U)
        /// </summary>
        private void CPA_U_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)REG.U.R;
            SetCompareFlags(REG.A, RAM_ME0[address]);
            tick += 7;
        }

        /// <summary>
        /// STA UH
        /// UH = A 
        /// Opcode 0x28, Bytes 1
        /// </summary>
        private void STA_UH()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.U.RH = this.REG.A;
            tick += 5;
            // no flag changes
        }

        /// <summary>
        /// AND (U)
        /// A = A & (U) 
        /// Opcode 0x29, Bytes 1
        /// </summary>
        private void AND_U_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = (byte)(this.REG.A & RAM_ME0[this.REG.U.R]);
            SetZFlag();
            tick += 7;
        }

        /// <summary>
        /// STA UL
        /// UL = A 
        /// Opcode 0x2A, Bytes 1
        /// </summary>
        private void STA_UL()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.U.RL = this.REG.A;
            tick += 5;
            // no flg changes
        }

        /// <summary>
        /// ORA (U)
        /// A = A | (U)
        /// Opcode 2B, Bytes 1
        /// </summary>
        private void ORA_U_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = (byte)(this.REG.A | RAM_ME0[REG.U.R]);
            SetZFlag();
            tick += 7;
        }

        /// <summary>
        /// DCS (U)
        /// A = A - (U) BCD Subtraction
        /// Opcode 2C, Bytes 1
        /// </summary>
        private void DCS_U_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME0[REG.U.R];
            this.REG.A = BCDSubtract(this.REG.A, value);
            tick += 13;
            // flags set by addition function
        }

        /// <summary>
        /// EOR (U)
        /// A = A ^ (U)
        /// Opcode 2D, Bytes 1
        /// </summary>
        private void EOR_U_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME0[REG.U.R];
            this.REG.A = (byte)(this.REG.A ^ value);
            SetZFlag();
            tick += 7;
        }

        /// <summary>
        /// STA (U)
        /// (U) = A 
        /// Opcode 0x2E, Bytes 1
        /// </summary>
        private void STA_U_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            RAM_ME0[REG.U.R] = this.REG.A;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// BIT (U)
        /// ZFLAG = A & (U)
        /// Opcode 2F, Bytes 1
        /// </summary>
        private void BIT_U_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)this.REG.U.R;
            byte result = (byte)(this.REG.A & RAM_ME0[address]);
            SetZFlag(result);
            tick += 7;
        }

        #endregion Opcodes_0x20-0x2F

        #region Opcodes_0x30-0x3F

        /// <summary>
        /// SBC VL
        /// A = A - VL
        /// Opcode 30, Bytes 1
        /// </summary>
        public void SBC_VL()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Subtract(this.REG.A, this.REG.V.RL);
            tick += 6;
            // flags set by subtraction function
        }

        /// <summary>
        /// SBC (V)
        /// A = A - (V)
        /// Opcode 31, Bytes 1
        /// </summary>
        public void SBC_V_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Subtract(this.REG.A, RAM_ME0[REG.V.R]);
            tick += 7;
            // flags set by subtraction function
        }

        /// <summary>
        /// ADC VL
        /// A = A + VL
        /// Opcode 32, Bytes 1
        /// </summary>
        private void ADC_VL()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Add(this.REG.A, this.REG.V.RL);
            tick += 6;
            // flags set by addition function
        }

        /// <summary>
        /// ADC (V)
        /// A = A + (U)
        /// Opcode 33, Bytes 1
        /// </summary>
        private void ADC_V_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Add(this.REG.A, this.RAM_ME0[REG.V.R]);
            // flags set by addition function
        }

        /// <summary>
        /// LDA VL
        /// A = VL
        /// Opcode 0x34, Bytes 1
        /// </summary>
        private void LDA_VL()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = this.REG.V.RL;
            SetZFlag();
            tick += 5;
        }

        /// <summary>
        /// LDA (V)
        /// A = (V)
        /// Opcode 0x35, Bytes 1
        /// </summary>
        private void LDA_V_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)this.REG.V.R;
            this.REG.A = RAM_ME0[address];
            SetZFlag();
            tick += 6;
        }

        /// <summary>
        /// CPA VL
        /// Opcode 36, Bytes 1
        /// Compare of A + VL
        /// </summary>
        private void CPA_VL()
        {
            REG.P.R += 1; // advance Program Counter
            SetCompareFlags(REG.A, REG.V.RL);
            tick += 6;
        }

        /// <summary>
        /// CPA (V)
        /// Opcode 37, Bytes 1
        /// Compare A + (V)
        /// </summary>
        private void CPA_V_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)REG.V.R;
            SetCompareFlags(REG.A, RAM_ME0[address]);
            tick += 7;
        }

        /// <summary>
        /// NOP
        /// Opcode 38, Bytes 1
        /// </summary>
        private void NOP()
        {
            REG.P.R += 1; // advance Program Counter
            tick += 5;
            // no effect on flags
        }

        /// <summary>
        /// AND (V)
        /// A = A & V 
        /// Opcode 0x39, Bytes 1
        /// </summary>
        private void AND_V_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = (byte)(this.REG.A & RAM_ME0[this.REG.V.R]);
            SetZFlag();
            tick += 7;
        }

        /// <summary>
        /// STA VL
        /// VL = A 
        /// Opcode 0x3A, Bytes 1
        /// </summary>
        private void STA_VL()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.V.RL = this.REG.A;
            tick += 5;
            // no flg changes
        }

        /// <summary>
        /// ORA (V)
        /// A = A | (V)
        /// Opcode 3B, Bytes 1
        /// </summary>
        private void ORA_V_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = (byte)(this.REG.A | RAM_ME0[REG.V.R]);
            SetZFlag();
            tick += 7;
        }

        /// <summary>
        /// DCS (V)
        /// A = A - (V) BCD Subtraction
        /// Opcode 3C, Bytes 1
        /// </summary>
        private void DCS_V_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME0[REG.V.R];
            this.REG.A = BCDSubtract(this.REG.A, value);
            tick += 13;
            // flags set by addition function
        }

        /// <summary>
        /// EOR (V)
        /// A = A ^ (V)
        /// Opcode 3D, Bytes 1
        /// </summary>
        private void EOR_V_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME0[REG.V.R];
            this.REG.A = (byte)(this.REG.A ^ value);
            SetZFlag();
            tick += 7;
        }

        /// <summary>
        /// STA (V)
        /// (V) = A 
        /// Opcode 0x3E, Bytes 1
        /// </summary>
        private void STA_V_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            RAM_ME0[REG.V.R] = this.REG.A;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// BIT (V)
        /// ZFLAG = A & (V)
        /// Opcode 3F, Bytes 1
        /// </summary>
        private void BIT_V_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)this.REG.V.R;
            byte result = (byte)(this.REG.A & RAM_ME0[address]);
            SetZFlag(result);
            tick += 7;
        }

        #endregion Opcodes_0x30-0x3F

        #region Opcodes_0x40-0x4F

        /// <summary>
        /// INC XL
        /// XL = XL + 1
        /// Opcode 40, Bytes 1
        /// </summary>
        private void INC_XL()
        {
            REG.P.R += 1; // advance Program Counter
            FLAGS.C = false;
            this.REG.X.RL = Add(this.REG.X.RL, (byte)0x01);
            tick += 5;
            // flags set by subtraction function
        }

        /// <summary>
        /// SIN X
        /// (X) = A then X = X + 1
        /// Opcode 41, Bytes 1
        /// </summary>
        private void SIN_X()
        {
            REG.P.R += 1; // advance Program Counter
            RAM_ME0[REG.X.R] = REG.A;
            REG.X.R += 1;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// DEC XL
        /// XL = XL - 1
        /// Opcode 42, Bytes 1
        /// </summary>
        private void DEC_XL()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.X.RL = Subtract(this.REG.X.RL, (byte)0x01);
            tick += 5;
            // flags set by subtraction function
        }

        /// <summary>
        /// SDE X
        /// (X) = A then X = X - 1
        /// Opcode 43, Bytes 1
        /// </summary>
        private void SDE_X()
        {
            REG.P.R += 1; // advance Program Counter
            RAM_ME0[REG.X.R] = REG.A;
            REG.X.R -= 1;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// INC X
        /// X = X + 1
        /// Opcode 44, Bytes 1
        /// </summary>
        private void INC_X()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.X.R += 1;
            tick += 5;
            // no flags changed
        }

        /// <summary>
        /// LDI X
        /// A = (X) then INC X
        /// Opcode 45, Bytes 1
        /// </summary>
        public void LIN_X()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = RAM_ME0[REG.X.R];
            REG.X.R += 1;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// DEC X
        /// X = X - 1
        /// Opcode 46, Bytes 1
        /// </summary>
        private void DEC_X()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.X.R -= 1;
            tick += 5;
            // no flags set
        }

        /// <summary>
        /// LDE X
        /// A = (X) then X = X - 1
        /// Opcode 0x47, Bytes 1
        /// </summary>
        private void LDE_X_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)this.REG.X.R;
            this.REG.A = RAM_ME0[address];
            REG.X.R -= 1;
            SetZFlag(REG.X.R);
            tick += 6;
        }

        /// <summary>
        /// LDI XH
        /// XH = n
        /// Opcode 48, Bytes 2
        /// </summary>
        public void LDI_XH_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            this.REG.X.RH = value;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// ANI (X),n
        /// (X) = (X) & n  
        /// Opcode 0x49, Bytes 2
        /// </summary>
        private void ANI_X_n_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            ushort address = (ushort)this.REG.X.R; // 
            RAM_ME0[address] = (byte)(RAM_ME0[address] & value);
            SetZFlag(RAM_ME0[address]);
            tick += 13;
        }

        /// <summary>
        /// LDI XL
        /// XL = n
        /// Opcode 4A, Bytes 2
        /// </summary>
        public void LDI_XL_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P +=1
            this.REG.X.RL = value;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// ORI (X),n
        /// (X) = (X) | n
        /// Opcode 4B, Bytes 2
        /// </summary>
        private void ORI_X_n_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte();
            ushort address = REG.X.R;
            RAM_ME0[address] = (byte)(RAM_ME0[address] | value);
            SetZFlag(RAM_ME0[address]);
            tick += 13;
        }

        /// <summary>
        /// CPI XH,n
        /// Opcode 4C, Bytes 2
        /// Compare of XH + n
        /// </summary>
        private void CPI_XH_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            SetCompareFlags(REG.X.RH, value);
            tick += 7;
        }

        /// <summary>
        /// BII (X),n
        /// FLAGS = (X) & n
        /// Opcode 4D, Bytes 2
        /// </summary>
        private void BII_X_n_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = this.GetByte(); // P += 1
            ushort address = (ushort)this.REG.X.R;
            byte result = (byte)(RAM_ME0[address] & value);
            SetZFlag(result);
            tick += 10;
        }

        /// <summary>
        /// CPI XL,n
        /// Opcode 4E, Bytes 2
        /// Compare of XL + n
        /// </summary>
        private void CPI_XL_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            SetCompareFlags(REG.X.RL, value);
            tick += 7;
        }

        /// <summary>
        /// ADI (X),n
        /// (X) = (X) + n
        /// Opcode 4F, Bytes 2
        /// </summary>
        private void ADI_X_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            this.RAM_ME0[REG.X.R] = Add(this.RAM_ME0[REG.X.R], value);
            tick += 13;
            // flags set by addition function
        }

        #endregion Opcodes_0x40-0x4F

        #region Opcodes_0x50-0x5F

        /// <summary>
        /// INC YL
        /// YL = YL + 1
        /// Opcode 50, Bytes 1
        /// </summary>
        private void INC_YL()
        {
            REG.P.R += 1; // advance Program Counter
            FLAGS.C = false;
            this.REG.Y.RL = Add(this.REG.Y.RL, (byte)0x01);
            tick += 5;
            // flags set by addition function
        }

        /// <summary>
        /// SIN Y
        /// (Y) = A then Y = Y + 1
        /// Opcode 51, Bytes 1
        /// </summary>
        private void SIN_Y()
        {
            REG.P.R += 1; // advance Program Counter
            RAM_ME0[REG.Y.R] = REG.A;
            REG.Y.R += 1;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// DEC YL
        /// YL = YL - 1
        /// Opcode 52, Bytes 1
        /// </summary>
        private void DEC_YL()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.Y.RL = Subtract(this.REG.Y.RL, (byte)0x01);
            tick += 5;
            // flags set by subtraction function
        }

        /// <summary>
        /// SDE Y
        /// (Y) = A then Y = Y - 1
        /// Opcode 53, Bytes 1
        /// </summary>
        private void SDE_Y()
        {
            REG.P.R += 1; // advance Program Counter
            RAM_ME0[REG.Y.R] = REG.A;
            REG.Y.R -= 1;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// INC Y
        /// Y = Y + 1
        /// Opcode 54, Bytes 1
        /// </summary>
        private void INC_Y()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.Y.R += 1;
            tick += 5;
            // no flags changed
        }

        /// <summary>
        /// LDI Y
        /// A = (Y) then INC Y
        /// Opcode 55, Bytes 1
        /// </summary>
        public void LIN_Y()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = RAM_ME0[REG.Y.R];
            REG.Y.R += 1;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// DEC Y
        /// Y = Y - 1
        /// Opcode 56, Bytes 1
        /// </summary>
        private void DEC_Y()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.Y.R -= 1;
            tick += 5;
            // no flags set
        }

        /// <summary>
        /// LDE Y
        /// A = (Y) then Y = Y - 1
        /// Opcode 0x57, Bytes 1
        /// </summary>
        private void LDE_Y()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)this.REG.Y.R;
            this.REG.A = RAM_ME0[address];
            REG.Y.R -= 1;
            SetZFlag(REG.Y.R);
            tick += 6;
        }

        /// <summary>
        /// LDI YH
        /// YH = n
        /// Opcode 58, Bytes 2
        /// </summary>
        public void LDI_YH_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            this.REG.Y.RH = value;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// ANI (Y),n
        /// (Y) = (Y) & n  
        /// Opcode 0x59, Bytes 2
        /// </summary>
        private void ANI_Y_n_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            ushort address = (ushort)this.REG.Y.R;
            RAM_ME0[address] = (byte)(RAM_ME0[address] & value);
            SetZFlag(RAM_ME0[address]);
            tick += 13;
        }

        /// <summary>
        /// LDI YL
        /// YL = n
        /// Opcode 5A, Bytes 2
        /// </summary>
        public void LDI_YL_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            this.REG.Y.RL = value;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// ORI (Y),n
        /// (Y) = (Y) | n
        /// Opcode 5B, Bytes 2
        /// </summary>
        private void ORI_Y_n_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte();
            ushort address = REG.Y.R;
            RAM_ME0[address] = (byte)(RAM_ME0[address] | value);
            SetZFlag(RAM_ME0[address]);
            tick += 13;
        }

        /// <summary>
        /// CPI YH,n
        /// Opcode 5C, Bytes 2
        /// Compare of YH + n
        /// </summary>
        private void CPI_YH_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            SetCompareFlags(REG.Y.RH, value);
            tick += 7;
        }

        /// <summary>
        /// BII (Y),n
        /// FLAGS = (Y) & n
        /// Opcode 5D, Bytes 2
        /// </summary>
        private void BII_Y_n_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = this.GetByte(); // P += 1
            ushort address = (ushort)this.REG.Y.R;
            byte result = (byte)(RAM_ME0[address] & value);
            SetZFlag(result);
            tick += 10;
        }

        /// <summary>
        /// CPI YL,n
        /// Opcode 5E, Bytes 2
        /// Compare of YL + n
        /// </summary>
        private void CPI_YL_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            SetCompareFlags(REG.Y.RL, value);
            tick += 7;
        }

        /// <summary>
        /// ADI (Y),n
        /// (Y) = (Y) + n
        /// Opcode 5F, Bytes 2
        /// </summary>
        private void ADI_Y_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            RAM_ME0[this.REG.Y.R] = Add(RAM_ME0[this.REG.Y.R], value);
            tick += 13;
            // no flag changes
        }

        #endregion Opcodes_0x50-0x5

        #region Opcodes_0x60-0x6F

        /// <summary>
        /// INC UL
        /// UL = UL + 1
        /// Opcode 60, Bytes 1
        /// </summary>
        private void INC_UL()
        {
            REG.P.R += 1; // advance Program Counter
            FLAGS.C = false;
            this.REG.U.RL = Add(this.REG.U.RL, (byte)0x01);
            tick += 5;
            // flags set by addition function
        }

        /// <summary>
        /// SIN U
        /// (U) = A then U = U + 1
        /// Opcode 61, Bytes 1
        /// </summary>
        private void SIN_U()
        {
            REG.P.R += 1; // advance Program Counter
            RAM_ME0[REG.U.R] = REG.A;
            REG.U.R += 1;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// DEC UL
        /// UL = UL - 1
        /// Opcode 62, Bytes 1
        /// </summary>
        private void DEC_UL()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.U.RL = Subtract(this.REG.U.RL, (byte)0x01);
            tick += 5;
            // flags set by subtraction function
        }

        /// <summary>
        /// SDE U
        /// (U) = A then U = U - 1
        /// Opcode 63, Bytes 1
        /// </summary>
        private void SDE_U()
        {
            REG.P.R += 1; // advance Program Counter
            RAM_ME0[REG.U.R] = REG.A;
            REG.U.R -= 1;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// INC U
        /// U = U + 1
        /// Opcode 64, Bytes 1
        /// </summary>
        private void INC_U()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.U.R += 1;
            tick += 5;
            // no flags changed
        }

        /// <summary>
        /// LDI U
        /// A = (U) then INC U
        /// Opcode 65, Bytes 1
        /// </summary>
        public void LIN_U()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = RAM_ME0[REG.U.R];
            REG.U.R += 1;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// DEC U
        /// U = U - 1
        /// Opcode 66, Bytes 1
        /// </summary>
        private void DEC_U()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.U.R -= 1;
            tick += 5;
            // no flags set
        }

        /// <summary>
        /// LDE U
        /// A = (U) then U = U - 1
        /// Opcode 0x67, Bytes 1
        /// </summary>
        private void LDE_U()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)this.REG.U.R;
            this.REG.A = RAM_ME0[address];
            REG.U.R -= 1;
            SetZFlag(REG.U.R);
            tick += 6;
        }

        /// <summary>
        /// LDI UH,n
        /// UH = n
        /// Opcode 68, Bytes 2
        /// </summary>
        public void LDI_UH_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            this.REG.U.RH = value;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// ANI (U),n
        /// (U) = (U) & n  
        /// Opcode 0x69, Bytes 2
        /// </summary>
        private void ANI_U_n_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            ushort address = (ushort)this.REG.U.R;
            RAM_ME0[address] = (byte)(RAM_ME0[address] & value);
            SetZFlag(RAM_ME0[address]);
            tick += 13;
        }

        /// <summary>
        /// LDI UL,n
        /// UL = n
        /// Opcode 6A, Bytes 2
        /// </summary>
        public void LDI_UL_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            this.REG.U.RL = value;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// ORI (U),n
        /// (U) = (U) | n
        /// Opcode 6B, Bytes 2
        /// </summary>
        private void ORI_U_n_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte();
            ushort address = REG.U.R;
            RAM_ME0[address] = (byte)(RAM_ME0[address] | value);
            SetZFlag(RAM_ME0[address]);
            tick += 13;
        }

        /// <summary>
        /// CPI UH,n
        /// Opcode 6C, Bytes 2
        /// Compare of UH + n
        /// </summary>
        private void CPI_UH_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            SetCompareFlags(REG.U.RH, value);
            tick += 7;
        }

        /// <summary>
        /// BII (U),n
        /// FLAGS = (U) & n
        /// Opcode 6D, Bytes 2
        /// </summary>
        private void BII_U_n_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = this.GetByte(); // P += 1
            ushort address = (ushort)this.REG.U.R;
            byte result = (byte)(RAM_ME0[address] & value);
            SetZFlag(result);
            tick += 10;
        }

        /// <summary>
        /// CPI UL,n
        /// Opcode 6E, Bytes 2
        /// Compare of UL + n
        /// </summary>
        private void CPI_UL_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            SetCompareFlags(REG.U.RL, value);
            tick += 7;
        }

        /// <summary>
        /// ADI (U),n
        /// (U) = (U) + n
        /// Opcode 6F, Bytes 2
        /// </summary>
        private void ADI_U_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            RAM_ME0[this.REG.U.R] = Add(RAM_ME0[this.REG.U.R], value);
            tick += 13;
            // no flag changes
        }

        #endregion Opcodes_0x60-0x6F

        #region Opcodes_0x70-0x7F

        /// <summary>
        /// INC VL
        /// VL = VL + 1
        /// Opcode 70, Bytes 1
        /// </summary>
        private void INC_VL()
        {
            REG.P.R += 1; // advance Program Counter
            FLAGS.C = false;
            this.REG.V.RL = Add(this.REG.V.RL, (byte)0x01);
            tick += 5;
            // flags set by addition function
        }

        /// <summary>
        /// SIN V
        /// (V) = A then V = V + 1
        /// Opcode 71, Bytes 1
        /// </summary>
        private void SIN_V()
        {
            REG.P.R += 1; // advance Program Counter
            RAM_ME0[REG.V.R] = REG.A;
            REG.V.R += 1;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// DEC VL
        /// VL = VL - 1
        /// Opcode 72, Bytes 1
        /// </summary>
        private void DEC_VL()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.V.RL = Subtract(this.REG.V.RL, (byte)0x01);
            tick += 5;
            // flags set by subtraction function
        }

        /// <summary>
        /// SDE V
        /// (V) = A then V = V - 1
        /// Opcode 73, Bytes 1
        /// </summary>
        private void SDE_V()
        {
            REG.P.R += 1; // advance Program Counter
            RAM_ME0[REG.V.R] = REG.A;
            REG.V.R -= 1;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// INC V
        /// V = V + 1
        /// Opcode 74, Bytes 1
        /// </summary>
        private void INC_V()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.V.R += 1;
            tick += 5;
            // no flags changed
        }

        /// <summary>
        /// LDI V
        /// A = (V) then INC V
        /// Opcode 75, Bytes 1
        /// </summary>
        public void LIN_V()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = RAM_ME0[REG.V.R];
            REG.V.R += 1;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// DEC V
        /// V = V - 1
        /// Opcode 76, Bytes 1
        /// </summary>
        private void DEC_V()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.V.R -= 1;
            tick += 5;
            // no flags set
        }

        /// <summary>
        /// LDE V
        /// A = (V) then V = V - 1
        /// Opcode 77, Bytes 1
        /// </summary>
        private void LDE_V_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)this.REG.V.R;
            this.REG.A = RAM_ME0[address];
            REG.V.R -= 1;
            SetZFlag(REG.V.R);
            tick += 6;
        }

        /// <summary>
        /// LDI VH,n
        /// VH = n
        /// Opcode 78, Bytes 2
        /// </summary>
        public void LDI_VH_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            this.REG.V.RH = value;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// ANI (V),n
        /// (V) = (V) & n  
        /// Opcode 0x79, Bytes 2
        /// </summary>
        private void ANI_V_n_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            ushort address = (ushort)this.REG.V.R;
            RAM_ME0[address] = (byte)(RAM_ME0[address] & value);
            SetZFlag(RAM_ME0[address]);
            tick += 13;
        }

        /// <summary>
        /// LDI VL,n
        /// VL = n
        /// Opcode 7A, Bytes 2
        /// </summary>
        public void LDI_VL_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            this.REG.V.RL = value;
            tick += 6;
            // no flag changes
        }

        /// <summary>
        /// ORI (V),n
        /// (V) = (V) | n
        /// Opcode 7B, Bytes 2
        /// </summary>
        private void ORI_V_n_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte();
            ushort address = REG.V.R;
            RAM_ME0[address] = (byte)(RAM_ME0[address] | value);
            SetZFlag(RAM_ME0[address]);
            tick += 13;
        }

        /// <summary>
        /// CPI VH,n
        /// Opcode 7C, Bytes 2
        /// Compare of VH + n
        /// </summary>
        private void CPI_VH_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            SetCompareFlags(REG.V.RH, value);
            tick += 7;
        }

        /// <summary>
        /// BII (V),n
        /// FLAGS = (V) & n
        /// Opcode 7D, Bytes 2
        /// </summary>
        private void BII_V_n_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = this.GetByte(); // P += 1
            ushort address = (ushort)this.REG.V.R;
            byte result = (byte)(RAM_ME0[address] & value);
            SetZFlag(result);
            tick += 10;
        }

        /// <summary>
        /// CPI VL,n
        /// Opcode 7E, Bytes 2
        /// Compare of VL + n
        /// </summary>
        private void CPI_VL_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            SetCompareFlags(REG.V.RL, value);
            tick += 7;
        }

        /// <summary>
        /// ADI (V),n
        /// (V) = (V) + n
        /// Opcode 7F, Bytes 2
        /// </summary>
        private void ADI_V_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            RAM_ME0[this.REG.V.R] = Add(RAM_ME0[this.REG.V.R], value);
            tick += 13;
            // no flag changes
        }

        #endregion Opcodes_0x70-0x7F

        #region Opcodes_0x80-0x8F

        /// <summary>
        /// SBC XH
        /// A = A - XH
        /// Opcode 80, Bytes 1
        /// </summary>
        public void SBC_XH()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Subtract(this.REG.A, this.REG.X.RH);
            tick += 6;
            // flags set by subtraction function
        }

        /// <summary>
        /// BCR+e
        /// Branch if Carry reset forward+
        /// Opcode 81, Bytes 2
        /// </summary>
        private void BCR_n_p()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1

            if (!FLAGS.C)
            {
                this.REG.P.R += value; // adjust program counter
                tick += 2; // two extra cycles for taking branch
            }
            tick += 8;
            // no flag changes
        }

        /// <summary>
        /// ADC XH
        /// A = A + XH
        /// Opcode 82, Bytes 1
        /// </summary>
        private void ADC_XH()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Add(this.REG.A, this.REG.X.RH);
            tick += 6;
            // flags set by addition function
        }

        /// <summary>
        /// BCS+e
        /// Branch if Carry set forward+
        /// Opcode 83, Bytes 2
        /// </summary>
        private void BCS_n_p()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1

            if (FLAGS.C)
            {
                this.REG.P.R += value; // adjust program counter
                tick += 2; // two extra cycles to take fwd branch
            }
            tick += 8; 
            // no flag changes
        }

        /// <summary>
        /// LDA XH
        /// A = XH
        /// Opcode 0x84, Bytes 1
        /// </summary>
        private void LDA_XH()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = this.REG.X.RH;
            SetZFlag();
            tick += 5;
        }

        /// <summary>
        /// BHR+e
        /// Branch if Half Carry reset forward+
        /// Opcode 85, Bytes 2
        /// </summary>
        private void BHR_n_p()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1

            if (!FLAGS.H)
            {
                this.REG.P.R += value; // adjust program counter
                tick += 2; // two extra cycles to take fwd branch
            }
            tick += 8;
            // no flag changes
        }

        /// <summary>
        /// CPA XH
        /// Opcode 86, Bytes 1
        /// Compare of A + XH
        /// </summary>
        private void CPA_XH()
        {
            REG.P.R += 1; // advance Program Counter
            SetCompareFlags(REG.A, REG.X.RH);
            tick += 6;
        }

        /// <summary>
        /// BHS+e
        /// Branch if Half Carry set forward+
        /// Opcode 87, Bytes 2
        /// </summary>
        private void BHS_n_p()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1

            if (FLAGS.H)
            {
                this.REG.P.R += value; // adjust program counter
                tick += 2; // two extra cycles to take fwd branch
            }
            tick += 8;
            // no flag changes
        }

        /// <summary>
        /// LOP UL,e
        /// UL = UL - 1, loop back 'e' if Borrow/Carry Flag not set
        /// Opcode 88, Bytes 2
        /// </summary>
        private void LOP_UL_e()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            REG.U.RL = Subtract(REG.U.RL, 0x01);

            if (!FLAGS.C)
            {
                this.REG.P.R -= value; // adjust program counter
                tick += 3; // three extra cycles for branch back
            }
            tick += 8;
            // no flag changes
        }

        /// <summary>
        /// BZR+e
        /// Branch if Zero reset forward+
        /// Opcode 89, Bytes 2
        /// </summary>
        private void BZR_n_p()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1

            if (!FLAGS.Z)
            {
                this.REG.P.R += value; // adjust program counter
                tick += 2; // two more clock cycles for fwd branch
            }
            tick += 8;
            // no flag changes
        }

        /// <summary>
        /// RTI
        /// Return from interrupt
        /// Opcode 8A, Bytes 1
        /// </summary>
        private void RTI()
        {
            REG.P.R += 1;               // advance Program Counter
            REG.S.R += 1;               // Adjust stack pointer to start of return address
            REG.P.R = GetWord(REG.S.R); // Retrive return address, i.e. new Program Counter
            REG.S.R += 2;               // Adjust stack pointer to start of T
            SetTREG(RAM_ME0[REG.S.R]);  // Retrieve Flags
            tick += 14;
        }

        /// <summary>
        /// BZS+e
        /// Branch if Zero set forward+
        /// Opcode 8B, Bytes 2
        /// </summary>
        private void BZS_n_p()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1

            if (FLAGS.Z)
            {
                this.REG.P.R += value; // adjust program counter
                tick += 2; // two more clock cycles for fwd branch
            }
            tick += 8;
            // no flag changes
        }

        /// <summary>
        /// DCA (X)
        /// A = A + (X) BCD Addition
        /// Opcode 8C, Bytes 1
        /// </summary>
        private void DCA_X_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME0[REG.X.R];
            this.REG.A = BCDAdd(this.REG.A, value);
            tick += 15;
            // flags set by addition function
        }

        /// <summary>
        /// BVR+e
        /// Branch if Overflow reset forward+
        /// Opcode 8D, Bytes 2
        /// </summary>
        private void BVR_n_p()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1

            if (!FLAGS.V)
            {
                this.REG.P.R += value; // adjust program counter
                tick += 2; // two more clock cycles for fwd branch
            }
            tick += 8;
            // no flag changes
        }

        /// <summary>
        /// BCH+e
        /// Branch unconditional forward+
        /// Opcode 8E, Bytes 2
        /// </summary>
        private void BCH_n_p()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            this.REG.P.R += value; // adjust program counter
            tick += 8;
            // no flag changes
        }

        /// <summary>
        /// BVS+e
        /// Branch if Overlow set forward+
        /// Opcode 8F, Bytes 2
        /// </summary>
        private void BVS_n_p()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1

            if (FLAGS.V)
            {
                this.REG.P.R += value; // adjust program counter
                tick += 2; // two more clock cycles for fwd branch
            }
            tick += 8;
            // no flag changes
        }

        #endregion Opcodes_0x80-0x8F

        #region Opcodes_0x90-0x9F

        /// <summary>
        /// SBC YH
        /// A = A - YH
        /// Opcode 90, Bytes 1
        /// </summary>
        public void SBC_YH()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Subtract(this.REG.A, this.REG.Y.RH);
            tick += 6;
            // flags set by subtraction function
        }

        /// <summary>
        /// BCR-e
        /// Branch if Carry reset back-
        /// Opcode 91, Bytes 2
        /// </summary>
        private void BCR_n_m()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1

            if (!FLAGS.C)
            {
                this.REG.P.R -= value; // adjust program counter
                tick += 3; // three extra cycles to take back branch
            }
            tick += 8; // three extra cycles to take back branch
            // no flag changes
        }

        /// <summary>
        /// ADC YH
        /// A = A + YH
        /// Opcode 92, Bytes 1
        /// </summary>
        private void ADC_YH()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Add(this.REG.A, this.REG.Y.RH);
            tick += 6;
            // flags set by addition function
        }

        /// <summary>
        /// BCS-e
        /// Branch if Carry set back-
        /// Opcode 93, Bytes 2
        /// </summary>
        private void BCS_n_m()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1

            if (FLAGS.C)
            {
                this.REG.P.R -= value; // adjust program counter
                tick += 2; // three extra cycles to take back branch
            }
            tick += 8;
            // no flag changes
        }

        /// <summary>
        /// LDA YH
        /// A = YH
        /// Opcode 0x94, Bytes 1
        /// </summary>
        private void LDA_YH()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = this.REG.Y.RH;
            SetZFlag();
            tick += 5;
        }

        /// <summary>
        /// BHR-e
        /// Branch if Half Carry reset back-
        /// Opcode 95, Bytes 2
        /// </summary>
        private void BHR_n_m()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1

            if (!FLAGS.H)
            {
                this.REG.P.R -= value; // adjust program counter
                tick += 3; // three extra cycles to take back branch
            }
            tick += 8;
            // no flag changes
        }

        /// <summary>
        /// CPA YH
        /// Opcode 96, Bytes 1
        /// Compare of A + YH
        /// </summary>
        private void CPA_YH()
        {
            REG.P.R += 1; // advance Program Counter
            SetCompareFlags(REG.A, REG.Y.RH);
            tick += 6;
        }

        /// <summary>
        /// BHS-e
        /// Branch if Half Carry set back-
        /// Opcode 97, Bytes 2
        /// </summary>
        private void BHS_n_m()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1

            if (FLAGS.H)
            {
                this.REG.P.R -= value; // adjust program counter
                tick += 3; // three extra cycles to take back branch
            }
            tick += 8;
            // no flag changes
        }

        /// <summary>
        /// BZR-e
        /// Branch if Zero reset back-
        /// Opcode 99, Bytes 2
        /// </summary>
        private void BZR_n_m()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1

            if (!FLAGS.Z)
            {
                this.REG.P.R -= value; // adjust program counter
                tick += 3; // three more clock cycles for back branch
            }
            tick += 8;
            // no flag changes
        }

        /// <summary>
        /// RTN
        /// Return from subroutine
        /// Opcode 9A, Bytes 1
        /// </summary>
        private void RTN()
        {
            REG.P.R += 1;               // advance Program Counter
            REG.S.R += 1;               // Adjust stack pointer to start of return address
            REG.P.R = GetWord(REG.S.R); // Retrive return address, i.e. new Program Counter
            REG.S.R += 1;               // Adjust stack pointer to end of return address
            tick += 11;
        }

        /// <summary>
        /// BZS-e
        /// Branch if Zero set back
        /// Opcode 9B, Bytes 2
        /// </summary>
        private void BZS_n_m()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1

            if (FLAGS.Z)
            {
                this.REG.P.R -= value; // adjust program counter
                tick += 3; // three more clock cycles for back branch
            }
            tick += 8;
            // no flag changes
        }

        /// <summary>
        /// DCA (Y)
        /// A = A + (Y) BCD Addition
        /// Opcode 9C, Bytes 1
        /// </summary>
        private void DCA_Y_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME0[REG.Y.R];
            this.REG.A = BCDAdd(this.REG.A, value);
            tick += 15;
            // flags set by addition function
        }

        /// <summary>
        /// BVR-e
        /// Branch if Overflow reset back-
        /// Opcode 9D, Bytes 2
        /// </summary>
        private void BVR_n_m()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1

            if (!FLAGS.V)
            {
                this.REG.P.R -= value; // adjust program counter
                tick += 3; // three more clock cycles for back branch
            }
            tick += 8;
            // no flag changes
        }

        /// <summary>
        /// BCH-e
        /// Branch unconditional back-
        /// Opcode 9E, Bytes 2
        /// </summary>
        private void BCH_n_m()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            this.REG.P.R -= value; // adjust program counter
            tick += 9;
            // no flag changes
        }

        /// <summary>
        /// BVS-e
        /// Branch if Overlow set back-
        /// Opcode 9F, Bytes 2
        /// </summary>
        private void BVS_n_m()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1

            if (FLAGS.V)
            {
                this.REG.P.R -= value; // adjust program counter
                tick += 3; // three more clock cycles for back branch
            }
            tick += 8;
            // no flag changes
        }

        #endregion Opcodes_0x90-0x9F

        #region Opcodes_0xA0-0xAF

        /// <summary>
        /// SBC UH
        /// A = A - UH
        /// Opcode A0, Bytes 1
        /// </summary>
        public void SBC_UH()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Subtract(this.REG.A, this.REG.U.RH);
            tick += 6;
            // flags set by subtraction function
        }

        /// <summary>
        /// SBC (pp)
        /// A = A - (pp)
        /// Opcode A1, Bytes 3
        /// </summary>
        public void SBC_pp_ME0()
        {
            REG.P.R += 1;                       // advance Program Counter
            byte value = RAM_ME0[GetWord()];    // P += 2
            this.REG.A = Subtract(this.REG.A, value);
            tick += 13;
            // flags set by subtraction function
        }

        /// <summary>
        /// ADC UH
        /// A = A + UH
        /// Opcode A2, Bytes 1
        /// </summary>
        private void ADC_UH()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Add(this.REG.A, this.REG.U.RH);
            tick += 6;
            // flags set by addition function
        }

        /// <summary>
        /// ADC (pp)
        /// Opcode A3, Bytes 3
        /// </summary>
        private void ADC_pp_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2
            this.REG.A = Add(this.REG.A, this.RAM_ME0[address]);
            tick += 13;
            // flags set by addition function
        }

        /// <summary>
        /// LDA UH
        /// A = UH
        /// Opcode 0xA4, Bytes 1
        /// </summary>
        private void LDA_UH()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = this.REG.U.RH;
            SetZFlag();
            tick += 5;
        }

        /// <summary>
        /// LDA (pp)
        /// A = (pp)
        /// Opcode 0xA5, Bytes 3
        /// </summary>
        private void LDA_pp_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2
            this.REG.A = RAM_ME0[address];
            SetZFlag();
            tick += 12;
        }

        /// <summary>
        /// CPA UH
        /// Opcode A6, Bytes 1
        /// Compare of A + UH
        /// </summary>
        private void CPA_UH()
        {
            REG.P.R += 1; // advance Program Counter
            SetCompareFlags(REG.A, REG.U.RH);
            tick += 6;
        }

        /// <summary>
        /// CPA (pp)
        /// Opcode A7, Bytes 3
        /// Compare A + (pp)
        /// </summary>
        private void CPA_pp_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2
            SetCompareFlags(REG.A, RAM_ME0[address]);
            tick += 13;
        }

        /// <summary>
        /// SPV
        /// Opcode A8, Bytes 1
        /// Set PV flip-flop. PV controls external device selection
        /// </summary>
        private void SPV()
        {
            REG.P.R += 1; // advance Program Counter
            REG.PV = true;
            tick += 4;
            // no flags changed
        }

        /// <summary>
        /// AND (pp)
        /// A = A & (pp) 
        /// Opcode 0xA9, Bytes 3
        /// </summary>
        private void AND_pp_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2
            this.REG.A = (byte)(this.REG.A & RAM_ME0[address]);
            SetZFlag();
            tick += 13;
        }

        /// <summary>
        /// LDI S,pp
        /// S = pp
        /// Opcode AA, Bytes 3
        /// </summary>
        private void LDI_S_pp()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2
            this.REG.S.R = address;
            tick += 12;
            // no flag changes
        }

        /// <summary>
        /// ORA (pp)
        /// A = A | (pp)
        /// Opcode AB, Bytes 3
        /// </summary>
        private void ORA_pp_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2
            this.REG.A = (byte)(this.REG.A | RAM_ME0[address]);
            SetZFlag();
            tick += 13;
        }

        /// <summary>
        /// DCA (U)
        /// A = A + (U) BCD Addition
        /// Opcode AC, Bytes 1
        /// </summary>
        private void DCA_U_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME0[REG.U.R];
            this.REG.A = BCDAdd(this.REG.A, value);
            tick += 15;
            // flags set by addition function
        }

        /// <summary>
        /// EOR (pp)
        /// A = A ^ (pp)
        /// Opcode AD, Bytes 3
        /// </summary>
        private void EOR_pp_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2;
            byte value = RAM_ME0[address];
            this.REG.A = (byte)(this.REG.A ^ value);
            SetZFlag();
            tick += 13;
        }

        /// <summary>
        /// STA pp
        /// (pp) = A 
        /// Opcode 0xAE, Bytes 3
        /// </summary>
        private void STA_pp_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2
            RAM_ME0[address] = this.REG.A;
            tick += 12;
            // no flag changes
        }

        /// <summary>
        /// BIT (pp)
        /// ZFLAG = A & (pp)
        /// Opcode AF, Bytes 1
        /// </summary>
        private void BIT_pp_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2
            byte result = (byte)(this.REG.A & RAM_ME0[address]);
            SetZFlag(result);
            tick += 13;
        }

        #endregion Opcodes_0xA0-0xAF

        #region Opcodes_0xB0-0xBF

        /// <summary>
        /// SBC VH
        /// A = A - VH
        /// Opcode B0, Bytes 1
        /// </summary>
        public void SBC_VH()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Subtract(this.REG.A, this.REG.V.RH);
            tick += 6;
            // flags set by subtraction function
        }

        /// <summary>
        /// SBI A,n
        /// A = A - n
        /// Opcode B1, Bytes 1
        /// </summary>
        public void SBI_A_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte();
            this.REG.A = Subtract(this.REG.A, value);
            tick += 7;
            // flags set by subtraction function
        }

        /// <summary>
        /// ADC VH
        /// A = A + VH
        /// Opcode B2, Bytes 1
        /// </summary>
        private void ADC_VH()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Add(this.REG.A, this.REG.V.RH);
            tick += 6;
            // flags set by addition function
        }

        /// <summary>
        /// ADI A,n
        /// A = A + n
        /// Opcode B3, Bytes 2
        /// </summary>
        private void ADI_A_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = this.GetByte(); // P += 1
            this.REG.A = Add(this.REG.A, value);
            tick += 7;
            // flags set by addition function
        }

        /// <summary>
        /// LDA VH
        /// A = VH
        /// Opcode 0xB4, Bytes 1
        /// </summary>
        private void LDA_VH()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = this.REG.V.RH;
            SetZFlag();
            tick += 5;
        }

        /// <summary>
        /// LDI A,n
        /// A = n
        /// Opcode B5, Bytes 2
        /// </summary>
        /// <param name="value"></param>
        public void LDI_A_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            this.REG.A = value;
            SetZFlag();
            tick += 6;
        }

        /// <summary>
        /// CPA VH
        /// Opcode B6, Bytes 1
        /// Compare of A + VH
        /// </summary>
        private void CPA_VH()
        {
            REG.P.R += 1; // advance Program Counter
            SetCompareFlags(REG.A, REG.V.RH);
            tick += 6;
        }

        /// <summary>
        /// CPI A,n
        /// Opcode B7, Bytes 2
        /// Compare of A + n
        /// </summary>
        private void CPI_A_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            SetCompareFlags(REG.A, value);
            tick += 7;
        }

        /// <summary>
        /// RPV
        /// Opcode B8, Bytes 1
        /// Reset PV Flip Flip, PV controls external device selection
        /// </summary>
        private void RPV()
        {
            REG.P.R += 1; // advance Program Counter
            REG.PV = false;
            tick += 4;
            // no flags changed
        }

        /// <summary>
        /// ANI A,n
        /// FLAGS = A & n
        /// Opcode B9, Bytes 2
        /// </summary>
        private void ANI_A_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = this.GetByte(); // P += 1
            this.REG.A = (byte)(this.REG.A & value);
            SetZFlag();
            tick += 7;
        }

        /// <summary>
        /// JMP pp
        /// Opcode BA, Bytes 2
        /// Jump to address (pp), unconditional.
        /// </summary>
        private void JMP_pp()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2
            REG.P.R = address;
            tick += 12;
        }

        /// <summary>
        /// ORI A,n
        /// A = A | n
        /// Opcode BB, Bytes 2
        /// </summary>
        private void ORI_A_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte();
            this.REG.A = (byte)(this.REG.A | value);
            SetZFlag();
            tick += 7;
        }

        /// <summary>
        /// DCA (V)
        /// A = A + (V) BCD Addition
        /// Opcode BC, Bytes 1
        /// </summary>
        private void DCA_V_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME0[REG.V.R];
            this.REG.A = BCDAdd(this.REG.A, value);
            tick += 15;
            // flags set by addition function
        }

        /// <summary>
        /// EAI n
        /// A = A ^ n
        /// Opcode BD, Bytes 2
        /// </summary>
        private void EAI_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            this.REG.A = (byte)(this.REG.A ^ value);
            SetZFlag();
            tick += 7;
        }

        /// <summary>
        /// SJP (pp)
        /// Subroutine Jump (CALL)
        /// Opcode BE, Bytes 2
        /// </summary>
        private void SJP_pp()
        {
            REG.P.R += 1;                       // advance Program Counter
            ushort address = GetWord();         // P + =2, now pointing to next instruction

            RAM_ME0[REG.S.R]     = REG.P.RL;    // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = address;                  // Set Program Counter to pp

            tick += 19;
        }

        /// <summary>
        /// BII A,n
        /// FLAGS = A & n
        /// Opcode BF, Bytes 2
        /// </summary>
        private void BII_A_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = this.GetByte(); // P += 1
            byte result = (byte)(this.REG.A & value);
            SetZFlag(result);
            tick += 7;
        }

        #endregion Opcodes_0xB0-0xBF

        #region Opcodes_0xC0-0xCF

        /// <summary>
        /// VEJ (C0)
        /// Vectored Call, $FF C0
        /// Opcode C0, Bytes 1
        /// </summary>
        public void VEJ_C0()
        {
            REG.P.R += 1; // advance Program Counter
            
            RAM_ME0[REG.S.R]     = REG.P.RL;    // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFC0;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// VCR n
        /// If Carry Reset do Indexed Vectored Call, $FFn, n = ($00-$F6)
        /// Opcode C1, Bytes 2
        /// </summary>
        public void VCR_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            if (!FLAGS.C)
            {
                ushort address = (ushort)(0xFF00 | value); 

                RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
                RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
                REG.S.R -= 2;                       // move stack pointer to next position

                REG.P.R = address;                  // Set Program Counter to pp
                tick += 13;                         // 13 more cycles if we take the sub call
            }
            tick += 8;
        }

        /// <summary>
        /// VEJ (C2)
        /// Vectored Call, $FF C2
        /// Opcode C2, Bytes 1
        /// </summary>
        public void VEJ_C2()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFC2;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// VCS n
        /// If Carry Set do Indexed Vectored Call, $FFn, n = ($00-$F6)
        /// Opcode C3, Bytes n
        /// </summary>
        public void VCS_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            if (FLAGS.C)
            {
                ushort address = (ushort)(0xFF00 | value);

                RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
                RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
                REG.S.R -= 2;                       // move stack pointer to next position

                REG.P.R = address;                  // Set Program Counter to pp
                tick += 13;                         // 13 more cycles if we take sub call
            }
            tick += 11;
        }

        /// <summary>
        /// VEJ (C4)
        /// Vectored Call, $FF C4
        /// Opcode C4, Bytes 1
        /// </summary>
        public void VEJ_C4()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFC4;                   // Set Program Counter to pp
            tick += 17;    
        }

        /// <summary>
        /// VHR n
        /// If Half-Carry Reset do Indexed Vectored Call. $FF n. n = ($00-$F6)
        /// Opcode C5, Bytes 2
        /// </summary>
        public void VHR_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            if (!FLAGS.H)
            {
                ushort address = (ushort)(0xFF00 | value);

                RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
                RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
                REG.S.R -= 2;                       // move stack pointer to next position

                REG.P.R = address;                  // Set Program Counter to pp
                tick += 13;
            }
            tick += 8;
        }

        /// <summary>
        /// VEJ (C6)
        /// Vectored Call, $FF C6
        /// Opcode C6, Bytes 1
        /// </summary>
        public void VEJ_C6()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFC6;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// VHS n
        /// If Half-Carry Set do Indexed Vectored Call. $FF n. n = ($00-$F6)
        /// Opcode C7, Bytes 2
        /// </summary>
        public void VHS_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            if (FLAGS.H)
            {
                ushort address = (ushort)(0xFF00 | value);

                RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
                RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
                REG.S.R -= 2;                       // move stack pointer to next position

                REG.P.R = address;                  // Set Program Counter to pp
                tick += 13;
            }
            tick += 8;
        }

        /// <summary>
        /// VEJ (C8)
        /// Vectored Call, $FF C8
        /// Opcode C8, Bytes 1
        /// </summary>
        public void VEJ_C8()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFC8;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// VZR n
        /// If Z Reset do Indexed Vectored Call. $FF n. n = ($00-$F6)
        /// Opcode C9, Bytes 2
        /// </summary>
        public void VZR_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            if (!FLAGS.Z)
            {
                ushort address = (ushort)(0xFF00 | value);

                RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
                RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
                REG.S.R -= 2;                       // move stack pointer to next position

                REG.P.R = address;                  // Set Program Counter to pp
                tick += 13;
            }
            tick += 8;
        }

        /// <summary>
        /// VEJ (CA)
        /// Vectored Call, $FF CA
        /// Opcode CA, Bytes 1
        /// </summary>
        public void VEJ_CA()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFCA;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// VZS n
        /// If Z Set do Indexed Vectored Call. $FF n. n = ($00-$F6)
        /// Opcode CB, Bytes 2
        /// </summary>
        public void VZS_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            if (FLAGS.Z)
            {
                ushort address = (ushort)(0xFF00 | value);

                RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
                RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
                REG.S.R -= 2;                       // move stack pointer to next position

                REG.P.R = address;                  // Set Program Counter to pp
                tick += 13;
            }
            tick += 8;
        }

        /// <summary>
        /// VEJ (CC)
        /// Vectored Call, $FF CC
        /// Opcode CC, Bytes 1
        /// </summary>
        public void VEJ_CC()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFCC;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// VMJ n
        /// Unconditional Indexed Vectored Call. $FF n. n = ($00-$F6)
        /// Opcode CD, Bytes 2
        /// </summary>
        public void VMJ_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            ushort address = (ushort)(0xFF00 | value);

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = address;                  // Set Program Counter to pp
            tick += 20;
        }

        /// <summary>
        /// VEJ (CE)
        /// Vectored Call, $FF CE
        /// Opcode CE, Bytes 1
        /// </summary>
        public void VEJ_CE()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFCE;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// VVS n
        /// If V Set do Indexed Vectored Call.. $FF n. n = ($00-$F6)
        /// Opcode CF, Bytes 2
        /// </summary>
        public void VVS_n()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            if (FLAGS.V)
            {
                ushort address = (ushort)(0xFF00 | value);

                RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
                RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
                REG.S.R -= 2;                   // move stack pointer to next position

                REG.P.R = address;              // Set Program Counter to pp
            }
        }

        #endregion Opcodes_0xC0-0xCF

        #region Opcodes_0xD0-0xDF

        /// <summary>
        /// VEJ (D0)
        /// Vectored Call, $FF D0
        /// Opcode D0, Bytes 1
        /// </summary>
        public void VEJ_D0()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFD0;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// ROR
        /// A = A >> 1. Rotated right through Carry
        /// Opcode D1, Bytes 1
        /// </summary>
        private void ROR()
        {
            REG.P.R += 1; // advance Program Counter
            int temp = REG.A | ((FLAGS.C ? 1 : 0) << 8); // Or Carry into Bit 8
            FLAGS.C = ((temp & 0x01) > 0); // Carry set from bit 0
            REG.A = (byte)((temp >> 1) & 0xFF); // shift right 1
            tick += 9;
            // no flag changes
        }

        /// <summary>
        /// VEJ (D2)
        /// Vectored Call, $FF D2
        /// Opcode D2, Bytes 1
        /// </summary>
        public void VEJ_D2()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFD2;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// DDR (X)
        /// Right rotation between Accumulator and (X)
        /// Opcode D3, Bytes 1
        /// </summary>
        private void DDR_X_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            //this.REG.A = Subtract(this.REG.A, (byte)0x01);
            tick += 12;
            // no flag changes
        }

        /// <summary>
        /// VEJ (D4)
        /// Vectored Call, $FF D4
        /// Opcode D4, Bytes 1
        /// </summary>
        public void VEJ_D4()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFD4;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// SHR
        /// A = A >> 1. Shifted through Carry, 0 into MSB
        /// Opcode D5, Bytes 1
        /// </summary>
        private void SHR()
        {
            REG.P.R += 1; // advance Program Counter
            FLAGS.C = ((REG.A & 0x01) > 0); // Carry set from bit 0
            REG.A = (byte)(REG.A >> 1);     // Shift right 1
            tick += 9;
            // only C flag changes
        }

        /// <summary>
        /// VEJ (D6)
        /// Vectored Call, $FF D6
        /// Opcode D6, Bytes 1
        /// </summary>
        public void VEJ_D6()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFD6;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// DRL (X)
        /// Left rotation between Accumulator and (X) (ME0)
        /// Opcode D7, Bytes 1
        /// </summary>
        private void DRL_X_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            //this.REG.A = Subtract(this.REG.A, (byte)0x01);
            tick += 12;
            // no flag changes
        }

        /// <summary>
        /// VEJ (D8)
        /// Vectored Call, $FF D8
        /// Opcode D8, Bytes 1
        /// </summary>
        public void VEJ_D8()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFD8;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// SHL
        /// AA = A << 1. Shifted through Carry, 0 into LSB
        /// Opcode D9, Bytes 1
        /// </summary>
        private void SHL()
        {
            REG.P.R += 1; // advance Program Counter
            int temp = (REG.A << 1); // Shift left
            FLAGS.C = ((temp & 0x100) > 0); // Carry set from bit 8
            REG.A = (byte)(temp & 0xFF);
            tick += 6;
            // only Carry flag changed
        }

        /// <summary>
        /// VEJ (DA)
        /// Vectored Call, $FF DA
        /// Opcode DA, Bytes 1
        /// </summary>
        public void VEJ_DA()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFDA;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// ROL
        /// A = A << 1. Rotated left through Carry
        /// Opcode DB, Bytes 1
        /// </summary>
        private void ROL()
        {
            REG.P.R += 1; // advance Program Counter
            int temp = (REG.A << 1) | (FLAGS.C ? 1 : 0); // Shift left then OR Carry into bit 0
            FLAGS.C = ((temp & 0x100) > 0); // Carry set from bit 8
            REG.A = (byte)(temp & 0xFF);
            tick += 8;
            // no flag changes
        }

        /// <summary>
        /// VEJ (DC)
        /// Vectored Call, $FF DC
        /// Opcode DC, Bytes 1
        /// </summary>
        public void VEJ_DC()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFDC;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// INC A
        /// A = A + 1
        /// Opcode DD, Bytes 1
        /// </summary>
        private void INC_A()
        {
            REG.P.R += 1; // advance Program Counter
            FLAGS.C = false;
            this.REG.A = Add(this.REG.A, (byte)0x01);
            tick += 5;
            // flags set by addition function
        }

        /// <summary>
        /// VEJ (DE)
        /// Vectored Call, $FF DE
        /// Opcode DE, Bytes 1
        /// </summary>
        public void VEJ_DE()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFDE;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// DEC A
        /// A = A - 1
        /// Opcode DF, Bytes 1
        /// </summary>
        private void DEC_A()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Subtract(this.REG.A, (byte)0x01);
            tick += 5;
            // flags set by subtraction function
        }

        #endregion Opcodes_0xD0-0xDF

        #region Opcodes_0xE0-0xEF

        /// <summary>
        /// VEJ (E0)
        /// Vectored Call, $FF E0
        /// Opcode E0, Bytes 1
        /// </summary>
        public void VEJ_E0()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFE0;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// SPU
        /// Opcode E1, Bytes 1
        /// Set PU Flip Flip, PU controls banking of external ROMs
        /// </summary>
        private void SPU()
        {
            REG.P.R += 1; // advance Program Counter
            REG.PU = true;
            tick += 4;
            // no flags changed
        }

        /// <summary>
        /// VEJ (E2)
        /// Vectored Call, $FF E2
        /// Opcode E2, Bytes 1
        /// </summary>
        public void VEJ_E2()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFE2;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// RPU
        /// Opcode E3, Bytes 1
        /// Reset PU Flip Flip, PU controls banking of external ROMs
        /// </summary>
        private void RPU()
        {
            REG.P.R += 1; // advance Program Counter
            REG.PU = false;
            tick += 4;
            // no flags changed
        }

        /// <summary>
        /// VEJ (E4)
        /// Vectored Call, $FF E4
        /// Opcode E4, Bytes 1
        /// </summary>
        public void VEJ_E4()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFE4;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// VEJ (E6)
        /// Vectored Call, $FF E6
        /// Opcode E6, Bytes 1
        /// </summary>
        public void VEJ_E6()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFE6;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// VEJ (E8)
        /// Vectored Call, $FF E8
        /// Opcode E8, Bytes 1
        /// </summary>
        public void VEJ_E8()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFE8;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// ANI (pp),n
        /// (pp) = (pp) & n (ME0) 
        /// Opcode 0xE9, Bytes 4
        /// </summary>
        private void ANI_pp_n_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2
            byte value = GetByte(); // P += 1
            RAM_ME0[address] = (byte)(RAM_ME0[address] & value);
            SetZFlag();
            tick += 19;
        }

        /// <summary>
        /// VEJ (EA)
        /// Vectored Call, $FF EA
        /// Opcode EA, Bytes 1
        /// </summary>
        public void VEJ_EA()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFEA;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// ORI (pp),n
        /// (pp) = (pp) | n
        /// Opcode EB, Bytes 4
        /// </summary>
        private void ORI_pp_n_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord();
            byte value = GetByte();
            RAM_ME0[address] = (byte)(RAM_ME0[address] | value);
            SetZFlag(RAM_ME0[address]);
            tick += 19;
        }

        /// <summary>
        /// VEJ (C0)
        /// Vectored Call, $FF EC
        /// Opcode EC, Bytes 1
        /// </summary>
        public void VEJ_EC()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFEC;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// BII (pp),n
        /// FLAGS = (pp) & n
        /// Opcode ED, Bytes 4
        /// </summary>
        private void BII_pp_n_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2
            byte value = this.GetByte(); // P += 1
            byte result = (byte)(RAM_ME0[address] & value);
            SetZFlag(result);
            tick += 16;
        }

        /// <summary>
        /// VEJ (EE)
        /// Vectored Call, $FF EE
        /// Opcode EE, Bytes 1
        /// </summary>
        public void VEJ_EE()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFEE;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// ADI (pp),n
        /// (pp) = (pp) + n
        /// Opcode EF, Bytes 4
        /// </summary>
        private void ADI_pp_n_ME0()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2
            byte value = GetByte(); // P += 1
            this.RAM_ME0[address] = Add(this.RAM_ME0[address], value);
            tick += 19;
            // flags set by addition function
        }

        #endregion Opcodes_0xF0-0xFF

        #region Opcodes_0xF0-0xFF

        /// <summary>
        /// VEJ (F0)
        /// Vectored Call, $FF F0
        /// Opcode F0, Bytes 1
        /// </summary>
        public void VEJ_F0()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFF0;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// AEX
        /// Opcode F1, Bytes 1
        /// Swap Accumulator High nibble & low nibble
        /// </summary>
        private void AEX()
        {
            REG.P.R += 1; // advance Program Counter
            byte low = (byte)((REG.A & 0x0F) << 4);
            byte high = (byte)((REG.A & 0xF0) >> 4);
            REG.A = (byte)(low | high);
            tick += 6;
        }

        /// <summary>
        /// VEJ (F2)
        /// Vectored Call, $FF F2
        /// Opcode F2, Bytes 1
        /// </summary>
        public void VEJ_F2()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFF2;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// VEJ (F4)
        /// Vectored Call, $FF F4
        /// Opcode F4, Bytes 1
        /// </summary>
        public void VEJ_F4()
        {
            REG.P.R += 1;                       // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFF4;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// TIN
        /// Opcode F5, Bytes 1
        /// (Y) = (X) then X = X + 1, Y = Y + 1
        /// </summary>
        private void TIN()
        {
            REG.P.R += 1; // advance Program Counter
            RAM_ME0[REG.Y.R] = RAM_ME0[REG.X.R];
            REG.X.R += 1;
            REG.Y.R += 1;
            tick += 7;
        }

        /// <summary>
        /// VEJ (F6)
        /// Vectored Call, $FF F6
        /// Opcode F6, Bytes 1
        /// </summary>
        public void VEJ_F6()
        {
            REG.P.R += 1; // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFF6;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// CIN
        /// Opcode F7, Bytes 1
        /// FLAGS = A compared to (X) register, then INC X
        /// </summary>
        private void CIN()
        {
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME0[REG.X.R];
            SetCompareFlags(REG.A, RAM_ME0[REG.X.R]);
            REG.X.R += 1; // inc X, no flags changed
            tick += 8;
        }

        /// <summary>
        /// VEJ (F8)
        /// Vectored Call, $FF F8
        /// Opcode F8, Bytes 1
        /// </summary>
        public void VEJ_F8()
        {
            REG.P.R += 1;                       // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFF8;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// REC
        /// Opcode F9, Bytes 1
        /// Reset Carry Flag
        /// </summary>
        private void REC()
        {
            REG.P.R += 1; // advance Program Counter
            FLAGS.C = false;
            tick += 4;
            // no flags changed
        }

        /// <summary>
        /// VEJ (FA)
        /// Vectored Call, $FF FA
        /// Opcode FA, Bytes 1
        /// </summary>
        public void VEJ_FA()
        {
            REG.P.R += 1;                       // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFFA;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// SEC
        /// Opcode FB, Bytes 1
        /// Set Carry Flag
        /// </summary>
        private void SEC()
        {
            REG.P.R += 1; // advance Program Counter
            FLAGS.C = true;
            tick += 4;
        }

        /// <summary>
        /// VEJ (FC)
        /// Vectored Call, $FF FC
        /// Opcode FC, Bytes 1
        /// </summary>
        public void VEJ_FC()
        {
            REG.P.R += 1;                       // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFFC;                   // Set Program Counter to pp
            tick += 17;
        }

        /// <summary>
        /// FD xx
        /// FD signals use of Opcode Table 2
        /// </summary>
        private void FD_P2()
        {
            REG.P.R += 1; // advance Program Counter
            byte opcode = RAM_ME0[REG.P.R];
            delegatesTbl2[opcode].DynamicInvoke();
            // read next opcode and index into table 2
        }

        /// <summary>
        /// VEJ (FE)
        /// Vectored Call, $FF FE
        /// Opcode FE, Bytes 1
        /// </summary>
        public void VEJ_FE()
        {
            REG.P.R += 1;                       // advance Program Counter

            RAM_ME0[REG.S.R] = REG.P.RL;        // (Stack Pointer)     = Program Counter Low Byte
            RAM_ME0[REG.S.R - 1] = REG.P.RH;    // (Stack Pointer - 1) = Program Counter Hi  Byte
            REG.S.R -= 2;                       // move stack pointer to next position

            REG.P.R = 0xFFFE;                   // Set Program Counter to pp
            tick += 17;
        }

        #endregion Opcodes_0xF0-0xFF

        #endregion OPCODES 0x00-0xFF

        #region OPCODES 0xFD00-0xFDFF

        #region Opcodes_0xFD00-0xFD0F

        /// <summary>
        /// SBC #(X)
        /// A = A - #(X)
        /// Opcode FD 01, Bytes 2
        /// </summary>
        public void SBC_X_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Subtract(this.REG.A, RAM_ME1[REG.X.R]);
            tick += 11;
            // flags set by subtraction function
        }
        
        /// <summary>
        /// ADC #(X)
        /// Opcode FD 03, Bytes 2
        /// </summary>
        private void ADC_X_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Add(this.REG.A, RAM_ME1[REG.X.R]);
            tick += 11;
            //flags set in addition function
        }

        /// <summary>
        /// LDA #(X)
        /// A = #(X)
        /// Opcode 0xFD 05, Bytes 2
        /// </summary>
        private void LDA_X_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)this.REG.X.R;
            this.REG.A = RAM_ME1[address];
            SetZFlag();
            tick += 10;
        }

        /// <summary>
        /// CPA #(X)
        /// Opcode FD 07, Bytes 2
        /// Compare A + #(X)
        /// </summary>
        private void CPA_X_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            SetCompareFlags(REG.A, RAM_ME1[REG.X.R]);
            tick += 11;
        }

        /// <summary>
        /// LDX X
        /// X = X
        /// Opcode FD 08, Bytes 2
        /// </summary>
        public void LDX_X()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.X.R = this.REG.X.R;
            tick += 11;
            // no flag changes
        }

        /// <summary>
        /// AND #(X)
        /// A = A & #(X) 
        /// Opcode 0xFD 09, Bytes 2
        /// </summary>
        private void AND_X_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            this.REG.A = (byte)(this.REG.A & RAM_ME1[this.REG.X.R]);
            SetZFlag();
            tick += 11;
        }

        /// <summary>
        /// POP X
        /// Stack -> X
        /// Opcode FD 0A, Bytes 2
        /// </summary>
        private void POP_X()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            REG.S.R += 1; // move stack pointer back one
            byte value = RAM_ME0[REG.S.R];
            REG.X.RH = value;
            REG.S.R += 1; // move stack pointer back one
            value = RAM_ME0[REG.S.R];
            REG.X.RL = value;
            tick += 15;
            // no flags changed
        }

        /// <summary>
        /// ORA #(X)
        /// A = A | #(X)
        /// Opcode FD 0B, Bytes 2
        /// </summary>
        private void ORA_X_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            this.REG.A = (byte)(this.REG.A | RAM_ME1[REG.X.R]);
            SetZFlag();
            tick += 11;
        }

        /// <summary>
        /// DCS #(X)
        /// A = A - #(X) BCD Subtraction
        /// Opcode FD 0C, Bytes 2
        /// </summary>
        private void DCS_X_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME1[REG.X.R];
            this.REG.A = BCDSubtract(this.REG.A, value);
            tick += 17;
            // flags set by addition function
        }

        /// <summary>
        /// EOR #(X)
        /// A = A ^ #(X)
        /// Opcode FD 0D, Bytes 2
        /// </summary>
        private void EOR_X_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME1[REG.X.R];
            this.REG.A = (byte)(this.REG.A ^ value);
            SetZFlag();
            tick += 11;
        }

        /// <summary>
        /// STA #(X)
        /// #(X) = A 
        /// Opcode FD 0E, Bytes 2
        /// </summary>
        private void STA_X_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            RAM_ME1[REG.X.R] = this.REG.A;
            tick += 10;
            // no flag changes
        }

        /// <summary>
        /// BIT #(X)
        /// ZFLAG = A & #(X)
        /// Opcode FD 0F, Bytes 2
        /// </summary>
        private void BIT_X_ME1()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)this.REG.X.R;
            byte result = (byte)(this.REG.A & RAM_ME1[address]);
            SetZFlag(result);
            tick += 11;
        }

        #endregion Opcodes_0xFD00-0xFD0F

        #region Opcodes_0xFD10-0xFD1F

        /// <summary>
        /// SBC #(Y)
        /// A = A - #(Y)
        /// Opcode FD 11, Bytes 2
        /// </summary>
        public void SBC_Y_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Subtract(this.REG.A, RAM_ME1[REG.Y.R]);
            tick += 11;
            // flags set by subtraction function
        }

        /// <summary>
        /// ADC #(Y)
        /// Opcode FD 13, Bytes 2
        /// </summary>
        private void ADC_Y_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Add(this.REG.A, RAM_ME1[REG.Y.R]);
            tick += 11;
            //flags set in addition function
        }

        /// <summary>
        /// LDA #(Y)
        /// A = #(Y)
        /// Opcode 0xFD 15, Bytes 2
        /// </summary>
        private void LDA_Y_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)this.REG.Y.R;
            this.REG.A = RAM_ME1[address];
            SetZFlag();
            tick += 10;
        }

        /// <summary>
        /// CPA #(Y)
        /// Opcode FD 17, Bytes 2
        /// Compare A + #(Y)
        /// </summary>
        private void CPA_Y_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            SetCompareFlags(REG.A, RAM_ME1[REG.Y.R]);
            tick += 11;
        }

        /// <summary>
        /// LDX Y
        /// X = Y
        /// Opcode FD 18, Bytes 2
        /// </summary>
        public void LDX_Y()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.X.R = this.REG.Y.R;
            tick += 11;
            // no flag changes
        }

        /// <summary>
        /// AND #(Y)
        /// A = A & #(Y) 
        /// Opcode FD 19, Bytes 2
        /// </summary>
        private void AND_Y_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            this.REG.A = (byte)(this.REG.A & RAM_ME1[this.REG.Y.R]);
            SetZFlag();
            tick += 11;
        }

        /// <summary>
        /// POP Y
        /// Stack -> Y
        /// Opcode FD 1A, Bytes 2
        /// </summary>
        private void POP_Y()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            REG.S.R += 1; // move stack pointer back one
            byte value = RAM_ME0[REG.S.R];
            REG.Y.RH = value;
            REG.S.R += 1; // move stack pointer back one
            value = RAM_ME0[REG.S.R];
            REG.Y.RL = value;
            tick += 15;
            // no flags changed
        }

        /// <summary>
        /// ORA #(Y)
        /// A = A | #(Y)
        /// Opcode FD 1B, Bytes 2
        /// </summary>
        private void ORA_Y_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            this.REG.A = (byte)(this.REG.A | RAM_ME1[REG.Y.R]);
            SetZFlag();
            tick += 11;
        }

        /// <summary>
        /// DCS #(Y)
        /// A = A - #(Y) BCD Subtraction
        /// Opcode FD 1C, Bytes 2
        /// </summary>
        private void DCS_Y_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME1[REG.Y.R];
            this.REG.A = BCDSubtract(this.REG.A, value);
            tick += 17;
            // flags set by addition function
        }

        /// <summary>
        /// EOR #(Y)
        /// A = A ^ #(Y)
        /// Opcode FD 1D, Bytes 2
        /// </summary>
        private void EOR_Y_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME1[REG.Y.R];
            this.REG.A = (byte)(this.REG.A ^ value);
            SetZFlag();
            tick += 11;
        }

        /// <summary>
        /// STA #(Y)
        /// #(Y) = A 
        /// Opcode FD 1E, Bytes 2
        /// </summary>
        private void STA_Y_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            RAM_ME1[REG.Y.R] = this.REG.A;
            tick += 10;
            // no flag changes
        }

        /// <summary>
        /// BIT #(Y)
        /// ZFLAG = A & #(Y)
        /// Opcode FD 1F, Bytes 2
        /// </summary>
        private void BIT_Y_ME1()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)this.REG.Y.R;
            byte result = (byte)(this.REG.A & RAM_ME1[address]);
            SetZFlag(result);
            tick += 11;
        }

        #endregion Opcodes_0xFD10-0xFD1F

        #region Opcodes_0xFD20-0xFD2F

        /// <summary>
        /// SBC #(U)
        /// A = A - #(U)
        /// Opcode FD 21, Bytes 2
        /// </summary>
        public void SBC_U_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Subtract(this.REG.A, RAM_ME1[REG.U.R]);
            tick += 11;
            // flags set by subtraction function
        }

        /// <summary>
        /// ADC #(U)
        /// Opcode FD 23, Bytes 2
        /// </summary>
        private void ADC_U_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Add(this.REG.A, RAM_ME1[REG.U.R]);
            tick += 11;
            //flags set in addition function
        }

        /// <summary>
        /// LDA #(U)
        /// A = #(U)
        /// Opcode 0xFD 25, Bytes 2
        /// </summary>
        private void LDA_U_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)this.REG.U.R;
            this.REG.A = RAM_ME1[address];
            SetZFlag();
            tick += 10;
        }

        /// <summary>
        /// CPA #(U)
        /// Opcode FD 27, Bytes 2
        /// Compare A + #(X)
        /// </summary>
        private void CPA_U_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            SetCompareFlags(REG.A, RAM_ME1[REG.U.R]);
            tick += 11;
        }

        /// <summary>
        /// LDX U
        /// X = U
        /// Opcode FD 28, Bytes 2
        /// </summary>
        public void LDX_U()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.X.R = this.REG.U.R;
            tick += 11;
            // no flag changes
        }

        /// <summary>
        /// AND #(U)
        /// A = A & #(U) 
        /// Opcode 0xFD 29, Bytes 2
        /// </summary>
        private void AND_U_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            this.REG.A = (byte)(this.REG.A & RAM_ME1[this.REG.U.R]);
            SetZFlag();
            tick += 11;
        }

        /// <summary>
        /// POP UY
        /// Stack -> U
        /// Opcode FD 2A, Bytes 2
        /// </summary>
        private void POP_U()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            REG.S.R += 1; // move stack pointer back one
            byte value = RAM_ME0[REG.S.R];
            REG.U.RH = value;
            REG.S.R += 1; // move stack pointer back one
            value = RAM_ME0[REG.S.R];
            REG.U.RL = value;
            tick += 15;
            // no flags changed
        }

        /// <summary>
        /// ORA #(U)
        /// A = A | #(U)
        /// Opcode FD 2B, Bytes 2
        /// </summary>
        private void ORA_U_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            this.REG.A = (byte)(this.REG.A | RAM_ME1[REG.U.R]);
            SetZFlag();
            tick += 11;
        }

        /// <summary>
        /// DCS #(U)
        /// A = A - #(U) BCD Subtraction
        /// Opcode FD 2C, Bytes 2
        /// </summary>
        private void DCS_U_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME1[REG.U.R];
            this.REG.A = BCDSubtract(this.REG.A, value);
            tick += 17;
            // flags set by addition function
        }

        /// <summary>
        /// EOR #(U)
        /// A = A ^ #(U)
        /// Opcode FD 2D, Bytes 2
        /// </summary>
        private void EOR_U_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME1[REG.U.R];
            this.REG.A = (byte)(this.REG.A ^ value);
            SetZFlag();
            tick += 11;
        }

        /// <summary>
        /// STA #(U)
        /// #(U) = A 
        /// Opcode FD 2E, Bytes 2
        /// </summary>
        private void STA_U_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            RAM_ME1[REG.U.R] = this.REG.A;
            tick += 10;
            // no flag changes
        }

        /// <summary>
        /// BIT #(U)
        /// ZFLAG = A & #(U)
        /// Opcode FD 2F, Bytes 1
        /// </summary>
        private void BIT_U_ME1()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)this.REG.U.R;
            byte result = (byte)(this.REG.A & RAM_ME1[address]);
            SetZFlag(result);
            tick += 11;
        }

        #endregion Opcodes_0xFD20-0xFD2F

        #region Opcodes_0xFD30-0xFD3F

        /// <summary>
        /// SBC #(V)
        /// A = A - #(V)
        /// Opcode FD 31, Bytes 2
        /// </summary>
        public void SBC_V_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            this.REG.A = Subtract(this.REG.A, RAM_ME1[REG.V.R]);
            tick += 11;
            // flags set by subtraction function
        }

        /// <summary>
        /// ADC #(V)
        /// Opcode FD 33, Bytes 2
        /// </summary>
        private void ADC_V_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 2; // advance Program Counter
            this.REG.A = Add(this.REG.A, RAM_ME1[REG.V.R]);
            tick += 11;
            //flags set in addition function
        }

        /// <summary>
        /// LDA #(V)
        /// A = #(V)
        /// Opcode 0xFD 35, Bytes 2
        /// </summary>
        private void LDA_V_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)this.REG.V.R;
            this.REG.A = RAM_ME1[address];
            SetZFlag();
            tick += 10;
        }

        /// <summary>
        /// CPA #(V)
        /// Opcode FD 37, Bytes 2
        /// Compare A + #(V)
        /// </summary>
        private void CPA_V_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            SetCompareFlags(REG.A, RAM_ME1[REG.V.R]);
            tick += 11;
        }

        /// <summary>
        /// LDX V
        /// X = V
        /// Opcode FD 38, Bytes 2
        /// </summary>
        public void LDX_V()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.X.R = this.REG.V.R;
            tick += 11;
            // no flag changes
        }

        /// <summary>
        /// AND #(V)
        /// A = A & #(V) 
        /// Opcode FD 39, Bytes 2
        /// </summary>
        private void AND_V_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            this.REG.A = (byte)(this.REG.A & RAM_ME1[this.REG.V.R]);
            SetZFlag();
            tick += 11;
        }

        /// <summary>
        /// POP V
        /// Stack -> V
        /// Opcode FD 3A, Bytes 2
        /// </summary>
        private void POP_V()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            REG.S.R += 1; // move stack pointer back one
            byte value = RAM_ME0[REG.S.R];
            REG.V.RH = value;
            REG.S.R += 1; // move stack pointer back one
            value = RAM_ME0[REG.S.R];
            REG.V.RL = value;
            tick += 15;
            // no flags changed
        }

        /// <summary>
        /// ORA #(V)
        /// A = A | #(V)
        /// Opcode FD 3B, Bytes 2
        /// </summary>
        private void ORA_V_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            this.REG.A = (byte)(this.REG.A | RAM_ME1[REG.V.R]);
            SetZFlag();
            tick += 11;
        }

        /// <summary>
        /// DCS #(V)
        /// A = A - #(V) BCD Subtraction
        /// Opcode FD 3C, Bytes 2
        /// </summary>
        private void DCS_V_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME1[REG.V.R];
            this.REG.A = BCDSubtract(this.REG.A, value);
            tick += 17;
            // flags set by addition function
        }

        /// <summary>
        /// EOR #(V)
        /// A = A ^ #(V)
        /// Opcode FD 3D, Bytes 2
        /// </summary>
        private void EOR_V_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME1[REG.V.R];
            this.REG.A = (byte)(this.REG.A ^ value);
            SetZFlag();
            tick += 11;
        }

        /// <summary>
        /// STA #(V)
        /// #(V) = A 
        /// Opcode FD 3E, Bytes 2
        /// </summary>
        private void STA_V_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            RAM_ME1[REG.V.R] = this.REG.A;
            tick += 10;
            // no flag changes
        }

        /// <summary>
        /// BIT #(V)
        /// ZFLAG = A & #(V)
        /// Opcode FD 3F, Bytes 1
        /// </summary>
        private void BIT_V_ME1()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = (ushort)this.REG.V.R;
            byte result = (byte)(this.REG.A & RAM_ME1[address]);
            SetZFlag(result);
            tick += 11;
        }

        #endregion Opcodes_0xFD30-0xFD3F

        #region Opcodes_0xFD40-0xFD4F

        /// <summary>
        /// INC XH
        /// XH = XH + 1
        /// Opcode FD 40, Bytes 1
        /// </summary>
        private void INC_XH()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            FLAGS.C = false;
            this.REG.X.RH = Add(this.REG.X.RH, (byte)0x01);
            tick += 9;
            // flags set by addittion function
        }

        /// <summary>
        /// DEC XH
        /// XH = XH - 1
        /// Opcode FD 42, Bytes 1
        /// </summary>
        private void DEC_XH()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            this.REG.X.RH = Subtract(this.REG.X.RH, (byte)0x01);
            tick += 9;
            // flags set by subtraction function
        }

        /// <summary>
        /// LDX S
        /// X = S
        /// Opcode FD 48, Bytes 2
        /// </summary>
        public void LDX_S()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.X.R = this.REG.S.R;
            tick += 11;
            // no flag changes
        }

        /// <summary>
        /// ANI #(X),n
        /// #(X) = #(X) & n  
        /// Opcode FD 49, Bytes 3
        /// </summary>
        private void ANI_X_n_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            ushort address = (ushort)this.REG.X.R;
            RAM_ME1[address] = (byte)(RAM_ME1[address] & value);
            SetZFlag(RAM_ME1[address]);
            tick += 17;
        }

        /// <summary>
        /// STX X
        /// X = X
        /// Opcode FD 4A, Bytes 2
        /// </summary>
        private void STX_X()
        {
            // FD handled before hand P += 1
            REG.X.R = REG.X.R;
            tick += 11;
            // no flags changed
        }

        /// <summary>
        /// ORI #(X),n
        /// #(X) = #(X) | n
        /// Opcode FD 4B, Bytes 3
        /// </summary>
        private void ORI_X_n_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            ushort address = REG.X.R;
            byte value = GetByte();
            RAM_ME1[address] = (byte)(RAM_ME1[address] | value);
            SetZFlag(RAM_ME1[address]);
            tick += 17;
        }

        /// <summary>
        /// OFF
        /// BF Flip-Flop reset, Not sure what BF flip-flop is
        /// Opcode FD 4C, Bytes 2
        /// </summary>
        private void OFF()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            tick += 8;
        }

        /// <summary>
        /// BII #(X),n
        /// FLAGS = #(X) & n
        /// Opcode FD 4D, Bytes 3
        /// </summary>
        private void BII_X_n_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte value = this.GetByte(); // P += 1
            ushort address = (ushort)this.REG.X.R;
            byte result = (byte)(RAM_ME1[address] & value);
            SetZFlag(result);
            tick += 14;
        }

        /// <summary>
        /// STX S
        /// S = X
        /// Opcode FD 4E, Bytes 2
        /// </summary>
        private void STX_S()
        {
            // FD handled before hand P += 1
            REG.S.R = REG.X.R;
            tick += 11;
            // no flags changed
        }

        /// <summary>
        /// ADI #(X),n
        /// Opcode FD 4F, Bytes 3
        /// </summary>
        private void ADI_X_n_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte val = GetByte(); // P += 1
            RAM_ME1[REG.X.R] = Add(RAM_ME1[REG.X.R], val);
            tick += 17;
            // flags set by addition function
        }

        #endregion Opcodes_0xFD40-0xFD4F

        #region Opcodes_0xFD50-0xFD5F

        /// <summary>
        /// INC YH
        /// YH = YH + 1
        /// Opcode FD 50, Bytes 1
        /// </summary>
        private void INC_YH()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            FLAGS.C = false;
            this.REG.Y.RH = Add(this.REG.Y.RH, (byte)0x01);
            tick += 9;
            // flags set by addittion function
        }

        /// <summary>
        /// DEC YH
        /// YH = YH - 1
        /// Opcode FD 52, Bytes 1
        /// </summary>
        private void DEC_YH()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.Y.RH = Subtract(this.REG.Y.RH, (byte)0x01);
            tick += 9;
            // flags set by subtraction function
        }

        /// <summary>
        /// LDX SP
        /// X = P
        /// Opcode FD 58, Bytes 2
        /// </summary>
        public void LDX_P()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.X.R = this.REG.P.R;
            tick += 11;
            // no flag changes
        }

        /// <summary>
        /// ANI #(Y),n
        /// #(Y) = #(Y) & n  
        /// Opcode FD 59, Bytes 3
        /// </summary>
        private void ANI_Y_n_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            ushort address = (ushort)this.REG.Y.R;
            RAM_ME1[address] = (byte)(RAM_ME1[address] & value);
            SetZFlag(RAM_ME1[address]);
            tick += 17;
        }

        /// <summary>
        /// STX Y
        /// Y = X
        /// Opcode FD 5A, Bytes 2
        /// </summary>
        private void STX_Y()
        {
            // FD handled before hand P += 1
            REG.Y.R = REG.X.R;
            tick += 11;
            // no flags changed
        }

        /// <summary>
        /// ORI #(Y),n
        /// #(Y) = #(Y) | n
        /// Opcode FD 5B, Bytes 3
        /// </summary>
        private void ORI_Y_n_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            ushort address = REG.Y.R;
            byte value = GetByte();
            RAM_ME1[address] = (byte)(RAM_ME1[address] | value);
            SetZFlag(RAM_ME1[address]);
            tick += 17;
        }

        /// <summary>
        /// BII #(Y),n
        /// FLAGS = #(Y) & n
        /// Opcode FD 5D, Bytes 3
        /// </summary>
        private void BII_Y_n_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte value = this.GetByte(); // P += 1
            ushort address = (ushort)this.REG.Y.R;
            byte result = (byte)(RAM_ME1[address] & value);
            SetZFlag(result);
            tick += 14;
        }

        /// <summary>
        /// STX S
        /// P = X.  Program_Counter = X
        /// Opcode FD 5E, Bytes 2
        /// </summary>
        private void STX_P()
        {
            // FD handled before hand P += 1
            REG.P.R = REG.X.R;
            tick += 11;
            // no flags changed
        }

        /// <summary>
        /// ADI #(Y),n
        /// Opcode FD 5F, Bytes 3
        /// </summary>
        private void ADI_Y_n_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte val = GetByte(); // P += 1
            RAM_ME1[REG.Y.R] = Add(RAM_ME1[REG.Y.R], val);
            tick += 17;
            // flags set by addition function
        }

        #endregion Opcodes_0xFD50-0xFD5F

        #region Opcodes_0xFD60-0xFD6F

        /// <summary>
        /// INC UH
        /// UH = UH + 1
        /// Opcode FD 60, Bytes 1
        /// </summary>
        private void INC_UH()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            FLAGS.C = false;
            this.REG.U.RH = Add(this.REG.U.RH, (byte)0x01);
            tick += 9;
            // flags set by addittion function
        }

        /// <summary>
        /// DEC UH
        /// UH = UH - 1
        /// Opcode FD 62, Bytes 1
        /// </summary>
        private void DEC_UH()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            this.REG.U.RH = Subtract(this.REG.U.RH, (byte)0x01);
            tick += 9;
            // flags set by subtraction function
        }

        /// <summary>
        /// ANI #(U),n
        /// #(U) = #(U) & n  
        /// Opcode FD 69, Bytes 3
        /// </summary>
        private void ANI_U_n_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            ushort address = (ushort)this.REG.U.R;
            RAM_ME1[address] = (byte)(RAM_ME1[address] & value);
            SetZFlag(RAM_ME1[address]);
            tick += 17;
        }

        /// <summary>
        /// STX U
        /// U = X
        /// Opcode FD 6A, Bytes 2
        /// </summary>
        private void STX_U()
        {
            // FD handled before hand P += 1
            REG.U.R = REG.X.R;
            tick += 11;
            // no flags changed
        }

        /// <summary>
        /// ORI #(U),n
        /// #(U) = #(U) | n
        /// Opcode FD 6B, Bytes 3
        /// </summary>
        private void ORI_U_n_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            ushort address = REG.U.R;
            byte value = GetByte();
            RAM_ME1[address] = (byte)(RAM_ME1[address] | value);
            SetZFlag(RAM_ME1[address]);
            tick += 17;
        }

        /// <summary>
        /// BII #(U),n
        /// FLAGS = #(U) & n
        /// Opcode FD 6D, Bytes 3
        /// </summary>
        private void BII_U_n_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte value = this.GetByte(); // P += 1
            ushort address = (ushort)this.REG.U.R;
            byte result = (byte)(RAM_ME1[address] & value);
            SetZFlag(result);
            tick += 14;
        }

        /// <summary>
        /// ADI #(U),n
        /// Opcode FD 6F, Bytes 3
        /// </summary>
        private void ADI_U_n_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte val = GetByte(); // P += 1
            RAM_ME1[REG.U.R] = Add(RAM_ME1[REG.U.R], val);
            tick += 17;
            // flags set by addition function
        }

        #endregion Opcodes_0xFD60-0xFD6F

        #region Opcodes_0xFD70-0xFD7F

        /// <summary>
        /// INC VH
        /// VH = VH + 1
        /// Opcode FD 70, Bytes 1
        /// </summary>
        private void INC_VH()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            FLAGS.C = false;
            this.REG.V.RH = Add(this.REG.V.RH, (byte)0x01);
            tick += 9;
            // flags set by addittion function
        }

        /// <summary>
        /// DEC VH
        /// VH = VH - 1
        /// Opcode FD 72, Bytes 1
        /// </summary>
        private void DEC_VH()
        {
            REG.P.R += 1; // advance Program Counter
            this.REG.V.RH = Subtract(this.REG.V.RH, (byte)0x01);
            tick += 9;
            // flags set by subtraction function
        }

        /// <summary>
        /// ANI #(V),n
        /// #(V) = #(V) & n  
        /// Opcode FD 79, Bytes 2
        /// </summary>
        private void ANI_V_n_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte value = GetByte(); // P += 1
            ushort address = (ushort)this.REG.V.R;
            RAM_ME1[address] = (byte)(RAM_ME1[address] & value);
            SetZFlag(RAM_ME1[address]);
            tick += 17;
        }

        /// <summary>
        /// STX V
        /// V = X
        /// Opcode FD 7A, Bytes 2
        /// </summary>
        private void STX_V()
        {
            // FD handled before hand P += 1
            REG.V.R = REG.X.R;
            tick += 11;
            // no flags changed
        }

        /// <summary>
        /// ORI #(V),n
        /// #(V) = #(V) | n
        /// Opcode FD 7B, Bytes 3
        /// </summary>
        private void ORI_V_n_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            ushort address = REG.V.R;
            byte value = GetByte();
            RAM_ME1[address] = (byte)(RAM_ME1[address] | value);
            SetZFlag(RAM_ME1[address]);
            tick += 17;
        }

        /// <summary>
        /// BII #(V),n
        /// FLAGS = #(V) & n
        /// Opcode FD 7D, Bytes 3
        /// </summary>
        private void BII_V_n_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte value = this.GetByte(); // P += 1
            ushort address = (ushort)this.REG.V.R;
            byte result = (byte)(RAM_ME1[address] & value);
            SetZFlag(result);
            tick += 14;
        }

        /// <summary>
        /// ADI #(V),n
        /// Opcode FD 7F, Bytes 3
        /// </summary>
        private void ADI_V_n_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte val = GetByte(); // P += 1
            RAM_ME1[REG.V.R] = Add(RAM_ME1[REG.V.R], val);
            tick += 17;
            // flags set by addition function
        }

        #endregion Opcodes_0xFD70-0xFD7F

        #region Opcodes_0xFD80-0xFD8F

        /// <summary>
        /// SIE
        /// Set Interrupt Enable Flag
        /// Opcode FD 81, Bytes 1
        /// </summary>
        private void SIE()
        {
            // FD handled before hand P += 1
            FLAGS.IE = true;
            tick += 8;
        }

        /// <summary>
        /// PSH X
        /// X -> Stack
        /// Opcode FD 88, Bytes 2
        /// </summary>
        private void PSH_X()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            RAM_ME0[REG.S.R]   = REG.X.RL;
            REG.S.R -= 1; // move stack pointer to next position
            RAM_ME0[REG.S.R] = REG.X.RH;
            REG.S.R -= 1; // move stack pointer to next position
            tick += 14;
        }

        /// <summary>
        /// POP A
        /// Stack -> A
        /// Opcode FD 8A, Bytes 2
        /// </summary>
        private void POP_A()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            REG.S.R += 1; // move stack pointer back one place to A
            byte value = RAM_ME0[REG.S.R];
            this.REG.A = value;
            SetZFlag();
            tick += 12;
        }

        /// <summary>
        /// DCA #(X)
        /// A = A + (X) BCD Addition
        /// Opcode FD 8C, Bytes 2
        /// </summary>
        private void DCA_X_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME1[REG.X.R];
            this.REG.A = BCDAdd(this.REG.A, value);
            tick += 19;
            // flags set by addition function
        }

        /// <summary>
        /// CDV
        /// Clear CPU clock divider, resets CPU 
        /// Opcode FD 8E, Bytes 1
        /// </summary>
        private void CDV()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            tick += 8; 
            // more to come
        }

        #endregion Opcodes_0xFD80-0xFD8F

        #region Opcodes_0xFD90-0xFD9F

        /// <summary>
        /// PSH Y
        /// Y -> Stack
        /// Opcode FD 98, Bytes 2
        /// </summary>
        private void PSH_Y()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            RAM_ME0[REG.S.R] = REG.Y.RL;
            REG.S.R -= 1; // move stack pointer to nextposition
            RAM_ME0[REG.S.R] = REG.Y.RH;
            REG.S.R -= 1; // move stack pointer to nextposition
            tick += 14;
            // no flags changed
        }

        /// <summary>
        /// DCA #(Y)
        /// A = A + (Y) BCD Addition
        /// Opcode FD 9C, Bytes 2
        /// </summary>
        private void DCA_Y_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME1[REG.Y.R];
            this.REG.A = BCDAdd(this.REG.A, value);
            tick += 19;
            // flags set by addition function
        }

        #endregion Opcodes_0xFD90-0xFD9F

        #region Opcodes_0xFDA0-0xFDAF

        /// <summary>
        /// SBC #(pp)
        /// A = A - #(pp)
        /// Opcode FD A1, Bytes 4
        /// </summary>
        public void SBC_pp_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME1[GetWord()]; // P += 2
            this.REG.A = Subtract(this.REG.A, value);
            tick += 17;
            // flags set by subtraction function
        }

        /// <summary>
        /// ADC #(pp)
        /// Opcode FD A3, Bytes 4
        /// </summary>
        private void ADC_pp_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2
            this.REG.A = Add(this.REG.A, this.RAM_ME1[address]);
            tick += 17;
            //flags set in addition function
        }

        /// <summary>
        /// LDA #(pp)
        /// A = #(pp)
        /// Opcode FD A5, Bytes 4
        /// </summary>
        private void LDA_pp_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2
            this.REG.A = RAM_ME1[address];
            SetZFlag();
            tick += 16;
        }

        /// <summary>
        /// CPA #(pp)
        /// Opcode FD A7, Bytes 3
        /// Compare A + #(pp)
        /// </summary>
        private void CPA_pp_ME1()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2
            SetCompareFlags(REG.A, RAM_ME1[address]);
            tick += 17;
        }

        /// <summary>
        /// PSH U
        /// U -> Stack
        /// Opcode FD A8, Bytes 2
        /// </summary>
        private void PSH_U()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            RAM_ME0[REG.S.R] = REG.U.RL;
            REG.S.R -= 1; // move stack pointer to nextposition
            RAM_ME0[REG.S.R] = REG.U.RH;
            REG.S.R -= 1; // move stack pointer to nextposition
            tick += 14;
            // no flags changed
        }

        /// <summary>
        /// AND #(pp)
        /// A = A & #(pp) 
        /// Opcode FD A9, Bytes 4
        /// </summary>
        private void AND_pp_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2
            this.REG.A = (byte)(this.REG.A & RAM_ME1[address]);
            SetZFlag(RAM_ME1[address]);
            tick += 17;
        }

        /// <summary>
        /// TTA
        /// A = T (flags) 
        /// Opcode FD AA, Bytes 2
        /// </summary>
        private void TTA()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            this.REG.A = GetTREG();
            SetZFlag();
            tick += 9;
        }

        /// <summary>
        /// ORA #(pp)
        /// A = A | #(pp)
        /// Opcode FD AB, Bytes 4
        /// </summary>
        private void ORA_pp_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2
            this.REG.A = (byte)(this.REG.A | RAM_ME1[address]);
            SetZFlag();
            tick += 17;
        }

        /// <summary>
        /// DCA #(U)
        /// A = A + (U) BCD Addition
        /// Opcode FD AC, Bytes 2
        /// </summary>
        private void DCA_U_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME1[REG.U.R];
            this.REG.A = BCDAdd(this.REG.A, value);
            tick += 19;
            // flags set by addition function
        }

        /// <summary>
        /// EOR #pp)
        /// A = A ^ #(pp)
        /// Opcode FD AD, Bytes 4
        /// </summary>
        private void EOR_pp_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2;
            byte value = RAM_ME1[address];
            this.REG.A = (byte)(this.REG.A ^ value);
            SetZFlag();
            tick += 11;
        }

        /// <summary>
        /// STA pp
        /// (pp) = A 
        /// Opcode FD AE, Bytes 4
        /// </summary>
        private void STA_pp_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2
            RAM_ME1[address] = this.REG.A;
            tick += 16;
            // no flag changes
        }

        /// <summary>
        /// BIT #(pp)
        /// ZFLAG = A & #(pp)
        /// Opcode FD AF, Bytes 1
        /// </summary>
        private void BIT_pp_ME1()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2
            byte result = (byte)(this.REG.A & RAM_ME1[address]);
            SetZFlag(result);
            tick += 17;
        }

        #endregion Opcodes_0xFDA0-0xFDAF

        #region Opcodes_0xFDB0-0xFDBF

        /// <summary>
        /// HLT
        /// Stop CPU
        /// Opcode FD B1, Bytes 1
        /// </summary>
        private void HLT()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            SingleStep = true;
            // Put into single step so we don't have to check
            // if halted every loop through RUN
            tick += 9;
        }

        /// <summary>
        /// PSH V
        /// V -> Stack
        /// Opcode FD B8, Bytes 2
        /// </summary>
        private void PSH_V()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            RAM_ME0[REG.S.R] = REG.V.RL;
            REG.S.R -= 1; // move stack pointer to nextposition
            RAM_ME0[REG.S.R] = REG.V.RH;
            REG.S.R -= 1; // move stack pointer to nextposition
            tick += 14;
            // no flags changed
        }

        /// <summary>
        /// ITA
        /// Input Port IN0-IN7 -> Accumulator
        /// Opcode FD BA, Bytes 2
        /// </summary>
        private void ITA()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            tick += 9;
            // no flags changed?
        }

        /// <summary>
        /// DCA #(V)
        /// A = A + (V) BCD Addition
        /// Opcode FD BC, Bytes 2
        /// </summary>
        private void DCA_V_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            byte value = RAM_ME1[REG.V.R];
            this.REG.A = BCDAdd(this.REG.A, value);
            tick += 19;
            // flags set by addition function
        }

        /// <summary>
        /// RIE
        /// Opcode FD BE, Bytes 2
        /// Reset CInterrupt Enable Flag
        /// </summary>
        private void RIE()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            FLAGS.IE = false;
            tick += 8;
            // no flags changed
        }

        #endregion Opcodes_0xFDB0-0xFDBF

        #region Opcodes_0xFDC0-0xFDCF

        /// <summary>
        /// RDP
        /// Resets LCD ON/OFF flip-flop
        /// Opcode FD C0, Bytes 2
        /// </summary>
        private void RDP()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            REG.DISP = false;
            tick += 8;
        }

        /// <summary>
        /// SDP
        /// Opcode FD C1, Bytes 2
        /// Sets LCD ON/OFF control flip-flop
        /// </summary>
        private void SDP()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            REG.DISP = true;
            tick += 9;
            // no flags changed
        }

        /// <summary>
        /// PSH A
        /// A -> Stack
        /// Opcode FD C8, Bytes 2
        /// </summary>
        private void PSH_A()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            RAM_ME0[REG.S.R] = REG.A;
            REG.S.R -= 1; // move stack pointer to next cell
            tick += 11;
            // no flags changed
        }

        /// <summary>
        /// ADR X
        /// X = X + A
        /// Opcode FD CA, Bytes 2
        /// </summary>
        private void ADR_X()
        {
            REG.P.R += 1; // advance Program Counter
            FLAGS.C = false; // not sure if this should be cleared
            REG.X.RL = Add(REG.X.RL, REG.A);
            REG.X.RH = Add(REG.X.RH, 0x00); // add in carry if any
            tick += 11;
            // flags set by addition function
        }

        /// <summary>
        /// ATP
        /// Opcode FD CC, Bytes 2
        /// A -> Data Bus
        /// </summary>
        private void ATP()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            //SetTREG(REG.A);
            tick += 9;
        }

        /// <summary>
        /// AM0
        /// Opcode FD CE, Bytes 2
        /// A -> Timer register, bit 9 padded with 0
        /// </summary>
        private void AM0()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            REG.TM = REG.A;
            tick += 9;
        }

        #endregion Opcodes_0xFDC0-0xFDCF

        #region Opcodes_0xFDD0-0xFDDF

        /// <summary>
        /// DDR #(X)
        /// Right rotation between Accumulator and #(X)
        /// Opcode FD D3, Bytes 2
        /// </summary>
        private void DDR_X_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            //this.REG.A = Subtract(this.REG.A, (byte)0x01);
            tick += 16;
            // no flag changes
        }

        /// <summary>
        /// DRL #(X)
        /// Left rotation between Accumulator and #(X)
        /// Opcode FD D7, Bytes 2
        /// </summary>
        private void DRL_X_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            tick += 16;
            // no flag changes
        }

        /// <summary>
        /// ADR Y
        /// Y = Y + A
        /// Opcode FD DA, Bytes 2
        /// </summary>
        private void ADR_Y()
        {
            REG.P.R += 1; // advance Program Counter
            FLAGS.C = false; // not sure if this should be cleared
            REG.Y.RL = Add(REG.Y.RL, REG.A);
            REG.Y.RH = Add(REG.Y.RH, 0x00); // add in carry if any
            tick += 11;
            // flags set by addition function
        }

        /// <summary>
        /// AM1
        /// Opcode FD DE, Bytes 2
        /// A -> Timer register, bit 9 padded with 1
        /// </summary>
        private void AM1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            REG.TM = (ushort)(REG.A | 0x100);
            tick += 9;
        }

        #endregion Opcodes_0xFDD0-0xFDDF

        #region Opcodes_0xFDE0-0xFDEF

        /// <summary>
        /// ANI #(pp),n
        /// #(pp) = #(pp) & n (ME1)
        /// Opcode FD E9, Bytes 5
        /// </summary>
        private void ANI_pp_n_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2
            byte value = GetByte(); // P += 1
            RAM_ME1[address] = (byte)(RAM_ME1[address] & value);
            SetZFlag();
            tick += 23;
        }

        /// <summary>
        /// ADR U
        /// U = U + A
        /// Opcode FD EA, Bytes 2
        /// </summary>
        private void ADR_U()
        {
            REG.P.R += 1; // advance Program Counter
            FLAGS.C = false; // not sure if this should be cleared
            REG.U.RL = Add(REG.U.RL, REG.A);
            REG.U.RH = Add(REG.U.RH, 0x00); // add in carry if any
            tick += 11;
            // flags set by addition function
        }

        /// <summary>
        /// ORI #(pp),n
        /// #(pp) = #(pp) | n
        /// Opcode FD EB, Bytes 5
        /// </summary>
        private void ORI_pp_n_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord();
            byte value = GetByte();
            RAM_ME1[address] = (byte)(RAM_ME1[address] | value);
            SetZFlag(RAM_ME1[address]);
            tick += 23;
        }

        /// <summary>
        /// ATT
        /// Opcode FD EC, Bytes 2
        /// A -> T Register
        /// </summary>
        private void ATT()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            SetTREG(REG.A);
            tick += 9;
        }

        /// <summary>
        /// BII #(pp),n
        /// FLAGS = #(pp) & n
        /// Opcode FD ED, Bytes 4
        /// </summary>
        private void BII_pp_n_ME1()
        {
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2
            byte value = this.GetByte(); // P += 1
            byte result = (byte)(RAM_ME1[address] & value);
            SetZFlag(result);
            tick += 20;
        }

        /// <summary>
        /// ADI #(pp),n
        /// Opcode FD EF, Bytes 5
        /// </summary>
        private void ADI_pp_n_ME1()
        {
            // FD handled before hand P += 1
            REG.P.R += 1; // advance Program Counter
            ushort address = GetWord(); // P += 2
            byte value = GetByte(); // P += 1
            this.RAM_ME1[address] = Add(this.RAM_ME1[address], value);
            tick += 23;
            // flags set by addition function
        }

        #endregion Opcodes_0xFDE0-0xFDEF

        #region Opcodes_0xFDF0-0xFDFF

        /// <summary>
        /// ADR V
        /// V = V + A
        /// Opcode FD FA, Bytes 2
        /// </summary>
        private void ADR_V()
        {
            REG.P.R += 1; // advance Program Counter
            FLAGS.C = false; // not sure if this should be cleared
            REG.V.RL = Add(REG.V.RL, REG.A);
            REG.V.RH = Add(REG.V.RH, 0x00); // add in carry if any
            tick += 11;
            // flags set by addition function
        }


        #endregion Opcodes_0xFDF0-0xFDFF

        #endregion OPCODES 0xFD00-0xFDFF

    }
}
