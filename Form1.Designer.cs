namespace lh5801_Emu
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tbAddress = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbValue = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.cbHalfCarry = new System.Windows.Forms.CheckBox();
            this.cbOverflow = new System.Windows.Forms.CheckBox();
            this.cbZero = new System.Windows.Forms.CheckBox();
            this.cbInteruptEnable = new System.Windows.Forms.CheckBox();
            this.cbCarry = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tbYH = new System.Windows.Forms.TextBox();
            this.tbYL = new System.Windows.Forms.TextBox();
            this.tbUH = new System.Windows.Forms.TextBox();
            this.tbUL = new System.Windows.Forms.TextBox();
            this.tbVH = new System.Windows.Forms.TextBox();
            this.tbVL = new System.Windows.Forms.TextBox();
            this.tbXL = new System.Windows.Forms.TextBox();
            this.tbXH = new System.Windows.Forms.TextBox();
            this.tbP = new System.Windows.Forms.TextBox();
            this.tbS = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRun = new System.Windows.Forms.Button();
            this.tbA = new System.Windows.Forms.TextBox();
            this.btnStep = new System.Windows.Forms.Button();
            this.btnResetAll = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.rbME0 = new System.Windows.Forms.RadioButton();
            this.rbME1 = new System.Windows.Forms.RadioButton();
            this.tbCycles = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tbStack = new System.Windows.Forms.TextBox();
            this.cbStackDispW = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.tbBP1 = new System.Windows.Forms.TextBox();
            this.tbBP2 = new System.Windows.Forms.TextBox();
            this.tbBP3 = new System.Windows.Forms.TextBox();
            this.tbBP4 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cbBP1 = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.cbBP2 = new System.Windows.Forms.CheckBox();
            this.cbBP3 = new System.Windows.Forms.CheckBox();
            this.cbBP4 = new System.Windows.Forms.CheckBox();
            this.label18 = new System.Windows.Forms.Label();
            this.lblCPU_Spd = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(28, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "XREG";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.label4.Location = new System.Drawing.Point(17, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(154, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Flags  H  V  Z  IE  C";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(28, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 15);
            this.label5.TabIndex = 12;
            this.label5.Text = "YREG";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(28, 146);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 15);
            this.label6.TabIndex = 14;
            this.label6.Text = "UREG";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(28, 172);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 15);
            this.label7.TabIndex = 16;
            this.label7.Text = "VREG";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(28, 198);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 15);
            this.label8.TabIndex = 18;
            this.label8.Text = "AREG";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(347, 32);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 13);
            this.label9.TabIndex = 22;
            this.label9.Text = "Memory";
            // 
            // tbAddress
            // 
            this.tbAddress.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbAddress.Location = new System.Drawing.Point(237, 488);
            this.tbAddress.Name = "tbAddress";
            this.tbAddress.Size = new System.Drawing.Size(41, 23);
            this.tbAddress.TabIndex = 24;
            this.tbAddress.Text = "0000";
            this.tbAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbAddress_KeyPress);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(234, 472);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(45, 13);
            this.label10.TabIndex = 25;
            this.label10.Text = "Address";
            // 
            // tbValue
            // 
            this.tbValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbValue.Location = new System.Drawing.Point(287, 488);
            this.tbValue.Name = "tbValue";
            this.tbValue.Size = new System.Drawing.Size(397, 23);
            this.tbValue.TabIndex = 26;
            this.tbValue.Text = "00";
            this.tbValue.TextChanged += new System.EventHandler(this.tbValue_TextChanged);
            this.tbValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbValue_KeyPress);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(284, 472);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(34, 13);
            this.label11.TabIndex = 27;
            this.label11.Text = "Value";
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(56)))), ((int)(((byte)(26)))));
            this.btnReset.Location = new System.Drawing.Point(31, 517);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(65, 25);
            this.btnReset.TabIndex = 30;
            this.btnReset.Text = "RESET";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // cbHalfCarry
            // 
            this.cbHalfCarry.AutoSize = true;
            this.cbHalfCarry.Location = new System.Drawing.Point(67, 52);
            this.cbHalfCarry.Name = "cbHalfCarry";
            this.cbHalfCarry.Size = new System.Drawing.Size(15, 14);
            this.cbHalfCarry.TabIndex = 31;
            this.cbHalfCarry.UseVisualStyleBackColor = true;
            this.cbHalfCarry.Click += new System.EventHandler(this.cbHalfCarry_Click);
            // 
            // cbOverflow
            // 
            this.cbOverflow.AutoSize = true;
            this.cbOverflow.Location = new System.Drawing.Point(88, 52);
            this.cbOverflow.Name = "cbOverflow";
            this.cbOverflow.Size = new System.Drawing.Size(15, 14);
            this.cbOverflow.TabIndex = 32;
            this.cbOverflow.UseVisualStyleBackColor = true;
            this.cbOverflow.Click += new System.EventHandler(this.cbOverflow_Click);
            // 
            // cbZero
            // 
            this.cbZero.AutoSize = true;
            this.cbZero.Location = new System.Drawing.Point(110, 52);
            this.cbZero.Name = "cbZero";
            this.cbZero.Size = new System.Drawing.Size(15, 14);
            this.cbZero.TabIndex = 33;
            this.cbZero.UseVisualStyleBackColor = true;
            this.cbZero.Click += new System.EventHandler(this.cbZero_Click);
            // 
            // cbInteruptEnable
            // 
            this.cbInteruptEnable.AutoSize = true;
            this.cbInteruptEnable.Location = new System.Drawing.Point(134, 52);
            this.cbInteruptEnable.Name = "cbInteruptEnable";
            this.cbInteruptEnable.Size = new System.Drawing.Size(15, 14);
            this.cbInteruptEnable.TabIndex = 34;
            this.cbInteruptEnable.UseVisualStyleBackColor = true;
            this.cbInteruptEnable.Click += new System.EventHandler(this.cbInteruptEnable_Click);
            // 
            // cbCarry
            // 
            this.cbCarry.AutoSize = true;
            this.cbCarry.Location = new System.Drawing.Point(158, 52);
            this.cbCarry.Name = "cbCarry";
            this.cbCarry.Size = new System.Drawing.Size(15, 14);
            this.cbCarry.TabIndex = 35;
            this.cbCarry.UseVisualStyleBackColor = true;
            this.cbCarry.Click += new System.EventHandler(this.cbCarry_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(68, 73);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(21, 15);
            this.label12.TabIndex = 36;
            this.label12.Text = "RH";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(90, 73);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(21, 15);
            this.label13.TabIndex = 37;
            this.label13.Text = "RL";
            // 
            // tbYH
            // 
            this.tbYH.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbYH.Location = new System.Drawing.Point(69, 117);
            this.tbYH.Name = "tbYH";
            this.tbYH.Size = new System.Drawing.Size(21, 23);
            this.tbYH.TabIndex = 40;
            this.tbYH.Text = "00";
            this.tbYH.TextChanged += new System.EventHandler(this.tbRegHL_TextChanged);
            this.tbYH.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbRegHL_KeyPress);
            // 
            // tbYL
            // 
            this.tbYL.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbYL.Location = new System.Drawing.Point(89, 117);
            this.tbYL.Name = "tbYL";
            this.tbYL.Size = new System.Drawing.Size(21, 23);
            this.tbYL.TabIndex = 39;
            this.tbYL.Text = "00";
            this.tbYL.TextChanged += new System.EventHandler(this.tbRegHL_TextChanged);
            this.tbYL.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbRegHL_KeyPress);
            // 
            // tbUH
            // 
            this.tbUH.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbUH.Location = new System.Drawing.Point(69, 143);
            this.tbUH.Name = "tbUH";
            this.tbUH.Size = new System.Drawing.Size(21, 23);
            this.tbUH.TabIndex = 42;
            this.tbUH.Text = "00";
            this.tbUH.TextChanged += new System.EventHandler(this.tbRegHL_TextChanged);
            this.tbUH.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbRegHL_KeyPress);
            // 
            // tbUL
            // 
            this.tbUL.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbUL.Location = new System.Drawing.Point(89, 143);
            this.tbUL.Name = "tbUL";
            this.tbUL.Size = new System.Drawing.Size(21, 23);
            this.tbUL.TabIndex = 41;
            this.tbUL.Text = "00";
            this.tbUL.TextChanged += new System.EventHandler(this.tbRegHL_TextChanged);
            this.tbUL.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbRegHL_KeyPress);
            // 
            // tbVH
            // 
            this.tbVH.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbVH.Location = new System.Drawing.Point(69, 169);
            this.tbVH.Name = "tbVH";
            this.tbVH.Size = new System.Drawing.Size(21, 23);
            this.tbVH.TabIndex = 44;
            this.tbVH.Text = "00";
            this.tbVH.TextChanged += new System.EventHandler(this.tbRegHL_TextChanged);
            this.tbVH.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbRegHL_KeyPress);
            // 
            // tbVL
            // 
            this.tbVL.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbVL.Location = new System.Drawing.Point(89, 169);
            this.tbVL.Name = "tbVL";
            this.tbVL.Size = new System.Drawing.Size(21, 23);
            this.tbVL.TabIndex = 43;
            this.tbVL.Text = "00";
            this.tbVL.TextChanged += new System.EventHandler(this.tbRegHL_TextChanged);
            this.tbVL.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbRegHL_KeyPress);
            // 
            // tbXL
            // 
            this.tbXL.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbXL.Location = new System.Drawing.Point(89, 91);
            this.tbXL.Name = "tbXL";
            this.tbXL.Size = new System.Drawing.Size(21, 23);
            this.tbXL.TabIndex = 46;
            this.tbXL.Text = "00";
            this.tbXL.WordWrap = false;
            this.tbXL.TextChanged += new System.EventHandler(this.tbRegHL_TextChanged);
            this.tbXL.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbRegHL_KeyPress);
            // 
            // tbXH
            // 
            this.tbXH.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbXH.Location = new System.Drawing.Point(69, 91);
            this.tbXH.Name = "tbXH";
            this.tbXH.Size = new System.Drawing.Size(21, 23);
            this.tbXH.TabIndex = 47;
            this.tbXH.Text = "00";
            this.tbXH.TextChanged += new System.EventHandler(this.tbRegHL_TextChanged);
            this.tbXH.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbRegHL_KeyPress);
            // 
            // tbP
            // 
            this.tbP.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbP.Location = new System.Drawing.Point(69, 221);
            this.tbP.Name = "tbP";
            this.tbP.Size = new System.Drawing.Size(41, 23);
            this.tbP.TabIndex = 53;
            this.tbP.Text = "0000";
            this.tbP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbP.TextChanged += new System.EventHandler(this.tbRegHL_TextChanged);
            this.tbP.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbRegHL_KeyPress);
            // 
            // tbS
            // 
            this.tbS.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbS.Location = new System.Drawing.Point(131, 91);
            this.tbS.Name = "tbS";
            this.tbS.Size = new System.Drawing.Size(40, 23);
            this.tbS.TabIndex = 51;
            this.tbS.Text = "0000";
            this.tbS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbS.TextChanged += new System.EventHandler(this.tbRegHL_TextChanged);
            this.tbS.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbRegHL_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(130, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 15);
            this.label2.TabIndex = 49;
            this.label2.Text = "Stack";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(48, 225);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 15);
            this.label3.TabIndex = 48;
            this.label3.Text = "P";
            // 
            // btnRun
            // 
            this.btnRun.BackColor = System.Drawing.Color.Chartreuse;
            this.btnRun.Location = new System.Drawing.Point(106, 454);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 25);
            this.btnRun.TabIndex = 54;
            this.btnRun.Text = "RUN";
            this.btnRun.UseVisualStyleBackColor = false;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // tbA
            // 
            this.tbA.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbA.Location = new System.Drawing.Point(89, 195);
            this.tbA.Name = "tbA";
            this.tbA.Size = new System.Drawing.Size(21, 23);
            this.tbA.TabIndex = 55;
            this.tbA.Text = "00";
            this.tbA.WordWrap = false;
            this.tbA.TextChanged += new System.EventHandler(this.tbRegHL_TextChanged);
            this.tbA.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbRegHL_KeyPress);
            // 
            // btnStep
            // 
            this.btnStep.BackColor = System.Drawing.Color.YellowGreen;
            this.btnStep.Location = new System.Drawing.Point(31, 454);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(65, 25);
            this.btnStep.TabIndex = 56;
            this.btnStep.Text = "STEP";
            this.btnStep.UseVisualStyleBackColor = false;
            this.btnStep.Click += new System.EventHandler(this.btnStep_Click);
            // 
            // btnResetAll
            // 
            this.btnResetAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(56)))), ((int)(((byte)(26)))));
            this.btnResetAll.Location = new System.Drawing.Point(102, 517);
            this.btnResetAll.Name = "btnResetAll";
            this.btnResetAll.Size = new System.Drawing.Size(80, 25);
            this.btnResetAll.TabIndex = 57;
            this.btnResetAll.Text = "RESET ALL";
            this.btnResetAll.UseVisualStyleBackColor = false;
            this.btnResetAll.Click += new System.EventHandler(this.btnResetAll_Click);
            // 
            // btnPause
            // 
            this.btnPause.BackColor = System.Drawing.Color.Yellow;
            this.btnPause.Location = new System.Drawing.Point(31, 486);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(150, 25);
            this.btnPause.TabIndex = 58;
            this.btnPause.Text = "PAUSE";
            this.btnPause.UseVisualStyleBackColor = false;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // rbME0
            // 
            this.rbME0.AutoSize = true;
            this.rbME0.Checked = true;
            this.rbME0.Location = new System.Drawing.Point(733, 478);
            this.rbME0.Name = "rbME0";
            this.rbME0.Size = new System.Drawing.Size(74, 17);
            this.rbME0.TabIndex = 59;
            this.rbME0.TabStop = true;
            this.rbME0.Text = "RAM ME0";
            this.rbME0.UseVisualStyleBackColor = true;
            this.rbME0.CheckedChanged += new System.EventHandler(this.rbME0_CheckedChanged);
            // 
            // rbME1
            // 
            this.rbME1.AutoSize = true;
            this.rbME1.Location = new System.Drawing.Point(733, 500);
            this.rbME1.Name = "rbME1";
            this.rbME1.Size = new System.Drawing.Size(74, 17);
            this.rbME1.TabIndex = 60;
            this.rbME1.Text = "RAM ME1";
            this.rbME1.UseVisualStyleBackColor = true;
            this.rbME1.CheckedChanged += new System.EventHandler(this.rbME1_CheckedChanged);
            // 
            // tbCycles
            // 
            this.tbCycles.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCycles.Location = new System.Drawing.Point(69, 247);
            this.tbCycles.Name = "tbCycles";
            this.tbCycles.Size = new System.Drawing.Size(42, 23);
            this.tbCycles.TabIndex = 63;
            this.tbCycles.Text = "0000";
            this.tbCycles.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbCycles.TextChanged += new System.EventHandler(this.tbRegHL_TextChanged);
            this.tbCycles.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbRegHL_KeyPress);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(14, 252);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(49, 15);
            this.label14.TabIndex = 64;
            this.label14.Text = "Cycles";
            // 
            // tbStack
            // 
            this.tbStack.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbStack.Location = new System.Drawing.Point(131, 120);
            this.tbStack.Multiline = true;
            this.tbStack.Name = "tbStack";
            this.tbStack.Size = new System.Drawing.Size(40, 127);
            this.tbStack.TabIndex = 65;
            this.tbStack.Text = "0000";
            this.tbStack.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cbStackDispW
            // 
            this.cbStackDispW.AutoSize = true;
            this.cbStackDispW.Location = new System.Drawing.Point(131, 253);
            this.cbStackDispW.Name = "cbStackDispW";
            this.cbStackDispW.Size = new System.Drawing.Size(15, 14);
            this.cbStackDispW.TabIndex = 66;
            this.cbStackDispW.UseVisualStyleBackColor = true;
            this.cbStackDispW.CheckedChanged += new System.EventHandler(this.cbStackDispW_CheckedChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(152, 252);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(35, 15);
            this.label15.TabIndex = 67;
            this.label15.Text = "8/16";
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Interval = 150;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // tbBP1
            // 
            this.tbBP1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBP1.Location = new System.Drawing.Point(69, 314);
            this.tbBP1.Name = "tbBP1";
            this.tbBP1.Size = new System.Drawing.Size(42, 23);
            this.tbBP1.TabIndex = 68;
            this.tbBP1.Text = "0000";
            this.tbBP1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbBP1.TextChanged += new System.EventHandler(this.tbRegHL_TextChanged);
            this.tbBP1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbRegHL_KeyPress);
            // 
            // tbBP2
            // 
            this.tbBP2.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBP2.Location = new System.Drawing.Point(69, 343);
            this.tbBP2.Name = "tbBP2";
            this.tbBP2.Size = new System.Drawing.Size(42, 23);
            this.tbBP2.TabIndex = 69;
            this.tbBP2.Text = "0000";
            this.tbBP2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbBP2.TextChanged += new System.EventHandler(this.tbRegHL_TextChanged);
            this.tbBP2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbRegHL_KeyPress);
            // 
            // tbBP3
            // 
            this.tbBP3.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBP3.Location = new System.Drawing.Point(69, 372);
            this.tbBP3.Name = "tbBP3";
            this.tbBP3.Size = new System.Drawing.Size(42, 23);
            this.tbBP3.TabIndex = 70;
            this.tbBP3.Text = "0000";
            this.tbBP3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbBP3.TextChanged += new System.EventHandler(this.tbRegHL_TextChanged);
            this.tbBP3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbRegHL_KeyPress);
            // 
            // tbBP4
            // 
            this.tbBP4.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBP4.Location = new System.Drawing.Point(69, 401);
            this.tbBP4.Name = "tbBP4";
            this.tbBP4.Size = new System.Drawing.Size(42, 23);
            this.tbBP4.TabIndex = 71;
            this.tbBP4.Text = "0000";
            this.tbBP4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbBP4.TextChanged += new System.EventHandler(this.tbRegHL_TextChanged);
            this.tbBP4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbRegHL_KeyPress);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(131, 291);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(56, 15);
            this.label16.TabIndex = 73;
            this.label16.Text = "Enabled";
            // 
            // cbBP1
            // 
            this.cbBP1.AutoSize = true;
            this.cbBP1.Location = new System.Drawing.Point(131, 318);
            this.cbBP1.Name = "cbBP1";
            this.cbBP1.Size = new System.Drawing.Size(15, 14);
            this.cbBP1.TabIndex = 72;
            this.cbBP1.UseVisualStyleBackColor = true;
            this.cbBP1.CheckedChanged += new System.EventHandler(this.cbBP_CheckedChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(34, 291);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(77, 15);
            this.label17.TabIndex = 74;
            this.label17.Text = "Breakpoint";
            // 
            // cbBP2
            // 
            this.cbBP2.AutoSize = true;
            this.cbBP2.Location = new System.Drawing.Point(131, 347);
            this.cbBP2.Name = "cbBP2";
            this.cbBP2.Size = new System.Drawing.Size(15, 14);
            this.cbBP2.TabIndex = 75;
            this.cbBP2.UseVisualStyleBackColor = true;
            this.cbBP2.CheckedChanged += new System.EventHandler(this.cbBP_CheckedChanged);
            // 
            // cbBP3
            // 
            this.cbBP3.AutoSize = true;
            this.cbBP3.Location = new System.Drawing.Point(131, 376);
            this.cbBP3.Name = "cbBP3";
            this.cbBP3.Size = new System.Drawing.Size(15, 14);
            this.cbBP3.TabIndex = 76;
            this.cbBP3.UseVisualStyleBackColor = true;
            this.cbBP3.CheckedChanged += new System.EventHandler(this.cbBP_CheckedChanged);
            // 
            // cbBP4
            // 
            this.cbBP4.AutoSize = true;
            this.cbBP4.Location = new System.Drawing.Point(131, 405);
            this.cbBP4.Name = "cbBP4";
            this.cbBP4.Size = new System.Drawing.Size(15, 14);
            this.cbBP4.TabIndex = 77;
            this.cbBP4.UseVisualStyleBackColor = true;
            this.cbBP4.CheckedChanged += new System.EventHandler(this.cbBP_CheckedChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(34, 431);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(63, 13);
            this.label18.TabIndex = 78;
            this.label18.Text = "CPU Speed";
            // 
            // lblCPU_Spd
            // 
            this.lblCPU_Spd.AutoSize = true;
            this.lblCPU_Spd.Location = new System.Drawing.Point(110, 431);
            this.lblCPU_Spd.Name = "lblCPU_Spd";
            this.lblCPU_Spd.Size = new System.Drawing.Size(40, 13);
            this.lblCPU_Spd.TabIndex = 79;
            this.lblCPU_Spd.Text = "000.00";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(147, 431);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(31, 13);
            this.label19.TabIndex = 80;
            this.label19.Text = "MHZ";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmLoad});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(884, 24);
            this.menuStrip1.TabIndex = 81;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmLoad
            // 
            this.tsmLoad.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.tsmLoad.Name = "tsmLoad";
            this.tsmLoad.Size = new System.Drawing.Size(37, 20);
            this.tsmLoad.Text = "File";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.lblCPU_Spd);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.cbBP4);
            this.Controls.Add(this.cbBP3);
            this.Controls.Add(this.cbBP2);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.cbBP1);
            this.Controls.Add(this.tbBP4);
            this.Controls.Add(this.tbBP3);
            this.Controls.Add(this.tbBP2);
            this.Controls.Add(this.tbBP1);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.cbStackDispW);
            this.Controls.Add(this.tbStack);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.tbCycles);
            this.Controls.Add(this.rbME1);
            this.Controls.Add(this.rbME0);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnResetAll);
            this.Controls.Add(this.btnStep);
            this.Controls.Add(this.tbA);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.tbP);
            this.Controls.Add(this.tbS);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbXH);
            this.Controls.Add(this.tbXL);
            this.Controls.Add(this.tbVH);
            this.Controls.Add(this.tbVL);
            this.Controls.Add(this.tbUH);
            this.Controls.Add(this.tbUL);
            this.Controls.Add(this.tbYH);
            this.Controls.Add(this.tbYL);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cbCarry);
            this.Controls.Add(this.cbInteruptEnable);
            this.Controls.Add(this.cbZero);
            this.Controls.Add(this.cbOverflow);
            this.Controls.Add(this.cbHalfCarry);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tbValue);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tbAddress);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "lh5801 Emu - Sharp lh5801 CPU Emulator";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbAddress;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbValue;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.CheckBox cbHalfCarry;
        private System.Windows.Forms.CheckBox cbOverflow;
        private System.Windows.Forms.CheckBox cbZero;
        private System.Windows.Forms.CheckBox cbInteruptEnable;
        private System.Windows.Forms.CheckBox cbCarry;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbYH;
        private System.Windows.Forms.TextBox tbYL;
        private System.Windows.Forms.TextBox tbUH;
        private System.Windows.Forms.TextBox tbUL;
        private System.Windows.Forms.TextBox tbVH;
        private System.Windows.Forms.TextBox tbVL;
        private System.Windows.Forms.TextBox tbXL;
        private System.Windows.Forms.TextBox tbXH;
        private System.Windows.Forms.TextBox tbP;
        private System.Windows.Forms.TextBox tbS;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox tbA;
        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.Button btnResetAll;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.RadioButton rbME0;
        private System.Windows.Forms.RadioButton rbME1;
        private System.Windows.Forms.TextBox tbCycles;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbStack;
        private System.Windows.Forms.CheckBox cbStackDispW;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Timer tmrUpdate;
        private System.Windows.Forms.TextBox tbBP1;
        private System.Windows.Forms.TextBox tbBP2;
        private System.Windows.Forms.TextBox tbBP3;
        private System.Windows.Forms.TextBox tbBP4;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.CheckBox cbBP1;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.CheckBox cbBP2;
        private System.Windows.Forms.CheckBox cbBP3;
        private System.Windows.Forms.CheckBox cbBP4;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label lblCPU_Spd;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmLoad;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    }
}

