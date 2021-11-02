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
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(40, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "XREG";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.label4.Location = new System.Drawing.Point(18, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(154, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Flags  H  V  Z  IE  C";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(40, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 15);
            this.label5.TabIndex = 12;
            this.label5.Text = "YREG";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(40, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 15);
            this.label6.TabIndex = 14;
            this.label6.Text = "UREG";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(40, 170);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 15);
            this.label7.TabIndex = 16;
            this.label7.Text = "VREG";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(40, 196);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 15);
            this.label8.TabIndex = 18;
            this.label8.Text = "AREG";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(347, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 13);
            this.label9.TabIndex = 22;
            this.label9.Text = "Memory";
            // 
            // tbAddress
            // 
            this.tbAddress.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbAddress.Location = new System.Drawing.Point(230, 384);
            this.tbAddress.Name = "tbAddress";
            this.tbAddress.Size = new System.Drawing.Size(41, 23);
            this.tbAddress.TabIndex = 24;
            this.tbAddress.Text = "0000";
            this.tbAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbAddress_KeyPress);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(227, 368);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(45, 13);
            this.label10.TabIndex = 25;
            this.label10.Text = "Address";
            // 
            // tbValue
            // 
            this.tbValue.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbValue.Location = new System.Drawing.Point(280, 384);
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
            this.label11.Location = new System.Drawing.Point(277, 368);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(34, 13);
            this.label11.TabIndex = 27;
            this.label11.Text = "Value";
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(56)))), ((int)(((byte)(26)))));
            this.btnReset.Location = new System.Drawing.Point(21, 406);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(68, 23);
            this.btnReset.TabIndex = 30;
            this.btnReset.Text = "RESET";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // cbHalfCarry
            // 
            this.cbHalfCarry.AutoSize = true;
            this.cbHalfCarry.Location = new System.Drawing.Point(67, 39);
            this.cbHalfCarry.Name = "cbHalfCarry";
            this.cbHalfCarry.Size = new System.Drawing.Size(15, 14);
            this.cbHalfCarry.TabIndex = 31;
            this.cbHalfCarry.UseVisualStyleBackColor = true;
            this.cbHalfCarry.Click += new System.EventHandler(this.cbHalfCarry_Click);
            // 
            // cbOverflow
            // 
            this.cbOverflow.AutoSize = true;
            this.cbOverflow.Location = new System.Drawing.Point(88, 39);
            this.cbOverflow.Name = "cbOverflow";
            this.cbOverflow.Size = new System.Drawing.Size(15, 14);
            this.cbOverflow.TabIndex = 32;
            this.cbOverflow.UseVisualStyleBackColor = true;
            this.cbOverflow.Click += new System.EventHandler(this.cbOverflow_Click);
            // 
            // cbZero
            // 
            this.cbZero.AutoSize = true;
            this.cbZero.Location = new System.Drawing.Point(110, 39);
            this.cbZero.Name = "cbZero";
            this.cbZero.Size = new System.Drawing.Size(15, 14);
            this.cbZero.TabIndex = 33;
            this.cbZero.UseVisualStyleBackColor = true;
            this.cbZero.Click += new System.EventHandler(this.cbZero_Click);
            // 
            // cbInteruptEnable
            // 
            this.cbInteruptEnable.AutoSize = true;
            this.cbInteruptEnable.Location = new System.Drawing.Point(134, 39);
            this.cbInteruptEnable.Name = "cbInteruptEnable";
            this.cbInteruptEnable.Size = new System.Drawing.Size(15, 14);
            this.cbInteruptEnable.TabIndex = 34;
            this.cbInteruptEnable.UseVisualStyleBackColor = true;
            this.cbInteruptEnable.Click += new System.EventHandler(this.cbInteruptEnable_Click);
            // 
            // cbCarry
            // 
            this.cbCarry.AutoSize = true;
            this.cbCarry.Location = new System.Drawing.Point(158, 39);
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
            this.label12.Location = new System.Drawing.Point(83, 71);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(21, 15);
            this.label12.TabIndex = 36;
            this.label12.Text = "RH";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(108, 71);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(21, 15);
            this.label13.TabIndex = 37;
            this.label13.Text = "RL";
            // 
            // tbYH
            // 
            this.tbYH.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbYH.Location = new System.Drawing.Point(81, 115);
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
            this.tbYL.Location = new System.Drawing.Point(101, 115);
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
            this.tbUH.Location = new System.Drawing.Point(81, 141);
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
            this.tbUL.Location = new System.Drawing.Point(101, 141);
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
            this.tbVH.Location = new System.Drawing.Point(81, 167);
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
            this.tbVL.Location = new System.Drawing.Point(101, 167);
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
            this.tbXL.Location = new System.Drawing.Point(101, 89);
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
            this.tbXH.Location = new System.Drawing.Point(81, 89);
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
            this.tbP.Location = new System.Drawing.Point(81, 232);
            this.tbP.Name = "tbP";
            this.tbP.Size = new System.Drawing.Size(42, 23);
            this.tbP.TabIndex = 53;
            this.tbP.Text = "0000";
            this.tbP.TextChanged += new System.EventHandler(this.tbRegHL_TextChanged);
            this.tbP.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbRegHL_KeyPress);
            // 
            // tbS
            // 
            this.tbS.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbS.Location = new System.Drawing.Point(81, 258);
            this.tbS.Name = "tbS";
            this.tbS.Size = new System.Drawing.Size(42, 23);
            this.tbS.TabIndex = 51;
            this.tbS.Text = "0000";
            this.tbS.TextChanged += new System.EventHandler(this.tbRegHL_TextChanged);
            this.tbS.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbRegHL_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(40, 261);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 15);
            this.label2.TabIndex = 49;
            this.label2.Text = "S";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(40, 235);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 15);
            this.label3.TabIndex = 48;
            this.label3.Text = "P";
            // 
            // btnRun
            // 
            this.btnRun.BackColor = System.Drawing.Color.Chartreuse;
            this.btnRun.Location = new System.Drawing.Point(21, 348);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(151, 23);
            this.btnRun.TabIndex = 54;
            this.btnRun.Text = "RUN";
            this.btnRun.UseVisualStyleBackColor = false;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // tbA
            // 
            this.tbA.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbA.Location = new System.Drawing.Point(101, 193);
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
            this.btnStep.Location = new System.Drawing.Point(21, 314);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(151, 23);
            this.btnStep.TabIndex = 56;
            this.btnStep.Text = "STEP";
            this.btnStep.UseVisualStyleBackColor = false;
            this.btnStep.Click += new System.EventHandler(this.btnStep_Click);
            // 
            // btnResetAll
            // 
            this.btnResetAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(56)))), ((int)(((byte)(26)))));
            this.btnResetAll.Location = new System.Drawing.Point(95, 406);
            this.btnResetAll.Name = "btnResetAll";
            this.btnResetAll.Size = new System.Drawing.Size(76, 23);
            this.btnResetAll.TabIndex = 57;
            this.btnResetAll.Text = "RESET ALL";
            this.btnResetAll.UseVisualStyleBackColor = false;
            this.btnResetAll.Click += new System.EventHandler(this.btnResetAll_Click);
            // 
            // btnPause
            // 
            this.btnPause.BackColor = System.Drawing.Color.Yellow;
            this.btnPause.Location = new System.Drawing.Point(21, 377);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(151, 23);
            this.btnPause.TabIndex = 58;
            this.btnPause.Text = "PAUSE";
            this.btnPause.UseVisualStyleBackColor = false;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // rbME0
            // 
            this.rbME0.AutoSize = true;
            this.rbME0.Checked = true;
            this.rbME0.Location = new System.Drawing.Point(733, 364);
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
            this.rbME1.Location = new System.Drawing.Point(733, 386);
            this.rbME1.Name = "rbME1";
            this.rbME1.Size = new System.Drawing.Size(74, 17);
            this.rbME1.TabIndex = 60;
            this.rbME1.Text = "RAM ME1";
            this.rbME1.UseVisualStyleBackColor = true;
            this.rbME1.CheckedChanged += new System.EventHandler(this.rbME1_CheckedChanged);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(230, 426);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 61;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(316, 426);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 62;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 461);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);
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
            this.Name = "Form1";
            this.Text = "Sharp lh5801 CPU Emulator";
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
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
    }
}

