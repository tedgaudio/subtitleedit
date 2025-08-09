namespace Nikse.SubtitleEdit.Forms.Assa
{
    sealed partial class SetLayer
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.numericUpDownLayer = new Nikse.SubtitleEdit.Controls.NikseUpDown();
            this.labelLayer = new System.Windows.Forms.Label();
            this.labelActor = new System.Windows.Forms.Label();
            this.comboBoxActor = new System.Windows.Forms.ComboBox();
            this.labelOnOffScreen = new System.Windows.Forms.Label();
            this.comboBoxOnOffScreen = new System.Windows.Forms.ComboBox();
            this.labelDiegetic = new System.Windows.Forms.Label();
            this.comboBoxDiegetic = new System.Windows.Forms.ComboBox();
            this.labelDFX = new System.Windows.Forms.Label();
            this.textBoxDFX = new System.Windows.Forms.TextBox();
            this.labelDialogueReverb = new System.Windows.Forms.Label();
            this.comboBoxDialogueReverb = new System.Windows.Forms.ComboBox();
            this.labelNotes = new System.Windows.Forms.Label();
            this.textBoxNotes = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonOK.Location = new System.Drawing.Point(336, 250);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonCancel.Location = new System.Drawing.Point(417, 250);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "C&ancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // numericUpDownLayer
            // 
            this.numericUpDownLayer.BackColor = System.Drawing.SystemColors.Window;
            this.numericUpDownLayer.BackColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.numericUpDownLayer.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
            this.numericUpDownLayer.BorderColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.numericUpDownLayer.ButtonForeColor = System.Drawing.SystemColors.ControlText;
            this.numericUpDownLayer.ButtonForeColorDown = System.Drawing.Color.Orange;
            this.numericUpDownLayer.ButtonForeColorOver = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.numericUpDownLayer.DecimalPlaces = 0;
            this.numericUpDownLayer.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownLayer.Location = new System.Drawing.Point(24, 33);
            this.numericUpDownLayer.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownLayer.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericUpDownLayer.Name = "numericUpDownLayer";
            this.numericUpDownLayer.Size = new System.Drawing.Size(76, 20);
            this.numericUpDownLayer.TabIndex = 8;
            this.numericUpDownLayer.TabStop = false;
            this.numericUpDownLayer.ThousandsSeparator = false;
            this.numericUpDownLayer.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericUpDownLayer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDownLayer_KeyDown);
            // 
            // labelLayer
            // 
            this.labelLayer.AutoSize = true;
            this.labelLayer.Location = new System.Drawing.Point(24, 14);
            this.labelLayer.Name = "labelLayer";
            this.labelLayer.Size = new System.Drawing.Size(33, 13);
            this.labelLayer.TabIndex = 9;
            this.labelLayer.Text = "Layer";
            // 
            // labelActor
            // 
            this.labelActor.AutoSize = true;
            this.labelActor.Location = new System.Drawing.Point(24, 54);
            this.labelActor.Name = "labelActor";
            this.labelActor.Size = new System.Drawing.Size(32, 13);
            this.labelActor.TabIndex = 10;
            this.labelActor.Text = "Actor";
            // 
            // comboBoxActor
            // 
            this.comboBoxActor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.comboBoxActor.FormattingEnabled = true;
            this.comboBoxActor.Location = new System.Drawing.Point(24, 70);
            this.comboBoxActor.Name = "comboBoxActor";
            this.comboBoxActor.Size = new System.Drawing.Size(120, 21);
            this.comboBoxActor.TabIndex = 11;
            // 
            // labelOnOffScreen
            // 
            this.labelOnOffScreen.AutoSize = true;
            this.labelOnOffScreen.Location = new System.Drawing.Point(180, 54);
            this.labelOnOffScreen.Name = "labelOnOffScreen";
            this.labelOnOffScreen.Size = new System.Drawing.Size(75, 13);
            this.labelOnOffScreen.TabIndex = 12;
            this.labelOnOffScreen.Text = "On/Off Screen";
            // 
            // comboBoxOnOffScreen
            // 
            this.comboBoxOnOffScreen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOnOffScreen.FormattingEnabled = true;
            this.comboBoxOnOffScreen.Items.AddRange(new object[] {
            "On Screen",
            "Off Screen"});
            this.comboBoxOnOffScreen.Location = new System.Drawing.Point(180, 70);
            this.comboBoxOnOffScreen.Name = "comboBoxOnOffScreen";
            this.comboBoxOnOffScreen.Size = new System.Drawing.Size(120, 21);
            this.comboBoxOnOffScreen.TabIndex = 13;
            // 
            // labelDiegetic
            // 
            this.labelDiegetic.AutoSize = true;
            this.labelDiegetic.Location = new System.Drawing.Point(24, 110);
            this.labelDiegetic.Name = "labelDiegetic";
            this.labelDiegetic.Size = new System.Drawing.Size(50, 13);
            this.labelDiegetic.TabIndex = 14;
            this.labelDiegetic.Text = "Diegetic";
            // 
            // comboBoxDiegetic
            // 
            this.comboBoxDiegetic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDiegetic.FormattingEnabled = true;
            this.comboBoxDiegetic.Items.AddRange(new object[] {
            "Diegetic",
            "Non-diegetic"});
            this.comboBoxDiegetic.Location = new System.Drawing.Point(24, 126);
            this.comboBoxDiegetic.Name = "comboBoxDiegetic";
            this.comboBoxDiegetic.Size = new System.Drawing.Size(120, 21);
            this.comboBoxDiegetic.TabIndex = 15;
            // 
            // labelDFX
            // 
            this.labelDFX.AutoSize = true;
            this.labelDFX.Location = new System.Drawing.Point(180, 110);
            this.labelDFX.Name = "labelDFX";
            this.labelDFX.Size = new System.Drawing.Size(26, 13);
            this.labelDFX.TabIndex = 16;
            this.labelDFX.Text = "DFX";
            // 
            // textBoxDFX
            // 
            this.textBoxDFX.Location = new System.Drawing.Point(180, 126);
            this.textBoxDFX.Name = "textBoxDFX";
            this.textBoxDFX.Size = new System.Drawing.Size(120, 20);
            this.textBoxDFX.TabIndex = 17;
            // 
            // labelDialogueReverb
            // 
            this.labelDialogueReverb.AutoSize = true;
            this.labelDialogueReverb.Location = new System.Drawing.Point(24, 166);
            this.labelDialogueReverb.Name = "labelDialogueReverb";
            this.labelDialogueReverb.Size = new System.Drawing.Size(84, 13);
            this.labelDialogueReverb.TabIndex = 18;
            this.labelDialogueReverb.Text = "Dialogue Reverb";
            // 
            // comboBoxDialogueReverb
            // 
            this.comboBoxDialogueReverb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDialogueReverb.FormattingEnabled = true;
            this.comboBoxDialogueReverb.Items.AddRange(new object[] {
            "None",
            "Low",
            "Mid",
            "High"});
            this.comboBoxDialogueReverb.Location = new System.Drawing.Point(24, 182);
            this.comboBoxDialogueReverb.Name = "comboBoxDialogueReverb";
            this.comboBoxDialogueReverb.Size = new System.Drawing.Size(120, 21);
            this.comboBoxDialogueReverb.TabIndex = 19;
            // 
            // labelNotes
            // 
            this.labelNotes.AutoSize = true;
            this.labelNotes.Location = new System.Drawing.Point(180, 166);
            this.labelNotes.Name = "labelNotes";
            this.labelNotes.Size = new System.Drawing.Size(35, 13);
            this.labelNotes.TabIndex = 20;
            this.labelNotes.Text = "Notes";
            // 
            // textBoxNotes
            // 
            this.textBoxNotes.Location = new System.Drawing.Point(180, 182);
            this.textBoxNotes.Multiline = true;
            this.textBoxNotes.Name = "textBoxNotes";
            this.textBoxNotes.Size = new System.Drawing.Size(280, 40);
            this.textBoxNotes.TabIndex = 21;
            // 
            // SetLayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 285);
            this.Controls.Add(this.labelLayer);
            this.Controls.Add(this.numericUpDownLayer);
            this.Controls.Add(this.labelActor);
            this.Controls.Add(this.comboBoxActor);
            this.Controls.Add(this.labelOnOffScreen);
            this.Controls.Add(this.comboBoxOnOffScreen);
            this.Controls.Add(this.labelDiegetic);
            this.Controls.Add(this.comboBoxDiegetic);
            this.Controls.Add(this.labelDFX);
            this.Controls.Add(this.textBoxDFX);
            this.Controls.Add(this.labelDialogueReverb);
            this.Controls.Add(this.comboBoxDialogueReverb);
            this.Controls.Add(this.labelNotes);
            this.Controls.Add(this.textBoxNotes);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetLayer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SetLayer";
            this.Shown += new System.EventHandler(this.SetLayer_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SetLayer_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private Nikse.SubtitleEdit.Controls.NikseUpDown numericUpDownLayer;
        private System.Windows.Forms.Label labelLayer;
        private System.Windows.Forms.Label labelActor;
        private System.Windows.Forms.ComboBox comboBoxActor;
        private System.Windows.Forms.Label labelOnOffScreen;
        private System.Windows.Forms.ComboBox comboBoxOnOffScreen;
        private System.Windows.Forms.Label labelDiegetic;
        private System.Windows.Forms.ComboBox comboBoxDiegetic;
        private System.Windows.Forms.Label labelDFX;
        private System.Windows.Forms.TextBox textBoxDFX;
        private System.Windows.Forms.Label labelDialogueReverb;
        private System.Windows.Forms.ComboBox comboBoxDialogueReverb;
        private System.Windows.Forms.Label labelNotes;
        private System.Windows.Forms.TextBox textBoxNotes;
    }
}