namespace SoftProofing
{
    partial class SaveOptionsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveOptionsDialog));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.jpegQualityGroupBox = new System.Windows.Forms.GroupBox();
            this.jpegQualityUpDown = new System.Windows.Forms.NumericUpDown();
            this.jpegQualitySlider = new System.Windows.Forms.TrackBar();
            this.tiffOptionsPanel = new System.Windows.Forms.Panel();
            this.tiffLZWCheckBox = new System.Windows.Forms.CheckBox();
            this.horizontalResGroupBox = new System.Windows.Forms.GroupBox();
            this.horizontalResUpDown = new System.Windows.Forms.NumericUpDown();
            this.horizontalResSlider = new System.Windows.Forms.TrackBar();
            this.verticalResGroupBox = new System.Windows.Forms.GroupBox();
            this.verticalResUpDown = new System.Windows.Forms.NumericUpDown();
            this.verticalResSlider = new System.Windows.Forms.TrackBar();
            this.embeddedProfileLabel = new System.Windows.Forms.Label();
            this.embeddedProfleDescription = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.jpegQualityGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jpegQualityUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.jpegQualitySlider)).BeginInit();
            this.tiffOptionsPanel.SuspendLayout();
            this.horizontalResGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalResUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalResSlider)).BeginInit();
            this.verticalResGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.verticalResUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.verticalResSlider)).BeginInit();
            this.SuspendLayout();
            //
            // flowLayoutPanel1
            //
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.jpegQualityGroupBox);
            this.flowLayoutPanel1.Controls.Add(this.tiffOptionsPanel);
            this.flowLayoutPanel1.Controls.Add(this.horizontalResGroupBox);
            this.flowLayoutPanel1.Controls.Add(this.verticalResGroupBox);
            this.flowLayoutPanel1.Controls.Add(this.embeddedProfileLabel);
            this.flowLayoutPanel1.Controls.Add(this.embeddedProfleDescription);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(13, 26);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(322, 305);
            this.flowLayoutPanel1.TabIndex = 0;
            //
            // jpegQualityGroupBox
            //
            this.jpegQualityGroupBox.Controls.Add(this.jpegQualityUpDown);
            this.jpegQualityGroupBox.Controls.Add(this.jpegQualitySlider);
            this.jpegQualityGroupBox.Location = new System.Drawing.Point(3, 3);
            this.jpegQualityGroupBox.Name = "jpegQualityGroupBox";
            this.jpegQualityGroupBox.Size = new System.Drawing.Size(319, 69);
            this.jpegQualityGroupBox.TabIndex = 0;
            this.jpegQualityGroupBox.TabStop = false;
            this.jpegQualityGroupBox.Text = "Quality";
            //
            // jpegQualityUpDown
            //
            this.jpegQualityUpDown.Location = new System.Drawing.Point(263, 20);
            this.jpegQualityUpDown.Name = "jpegQualityUpDown";
            this.jpegQualityUpDown.Size = new System.Drawing.Size(53, 20);
            this.jpegQualityUpDown.TabIndex = 1;
            this.jpegQualityUpDown.Value = new decimal(new int[] {
            95,
            0,
            0,
            0});
            this.jpegQualityUpDown.ValueChanged += new System.EventHandler(this.jpegQualityUpDown_ValueChanged);
            //
            // jpegQualitySlider
            //
            this.jpegQualitySlider.Location = new System.Drawing.Point(7, 20);
            this.jpegQualitySlider.Maximum = 100;
            this.jpegQualitySlider.Name = "jpegQualitySlider";
            this.jpegQualitySlider.Size = new System.Drawing.Size(250, 45);
            this.jpegQualitySlider.TabIndex = 0;
            this.jpegQualitySlider.TickFrequency = 5;
            this.jpegQualitySlider.Value = 95;
            this.jpegQualitySlider.ValueChanged += new System.EventHandler(this.jpegQualitySlider_ValueChanged);
            //
            // tiffOptionsPanel
            //
            this.tiffOptionsPanel.Controls.Add(this.tiffLZWCheckBox);
            this.tiffOptionsPanel.Location = new System.Drawing.Point(3, 78);
            this.tiffOptionsPanel.Name = "tiffOptionsPanel";
            this.tiffOptionsPanel.Size = new System.Drawing.Size(319, 48);
            this.tiffOptionsPanel.TabIndex = 1;
            //
            // tiffLZWCheckBox
            //
            this.tiffLZWCheckBox.AutoSize = true;
            this.tiffLZWCheckBox.Checked = true;
            this.tiffLZWCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tiffLZWCheckBox.Location = new System.Drawing.Point(7, 18);
            this.tiffLZWCheckBox.Name = "tiffLZWCheckBox";
            this.tiffLZWCheckBox.Size = new System.Drawing.Size(113, 17);
            this.tiffLZWCheckBox.TabIndex = 0;
            this.tiffLZWCheckBox.Text = "LZW Compression";
            this.tiffLZWCheckBox.UseVisualStyleBackColor = true;
            //
            // horizontalResGroupBox
            //
            this.horizontalResGroupBox.Controls.Add(this.horizontalResUpDown);
            this.horizontalResGroupBox.Controls.Add(this.horizontalResSlider);
            this.horizontalResGroupBox.Location = new System.Drawing.Point(3, 132);
            this.horizontalResGroupBox.Name = "horizontalResGroupBox";
            this.horizontalResGroupBox.Size = new System.Drawing.Size(322, 73);
            this.horizontalResGroupBox.TabIndex = 4;
            this.horizontalResGroupBox.TabStop = false;
            this.horizontalResGroupBox.Text = "Horizontal Resolution";
            //
            // horizontalResUpDown
            //
            this.horizontalResUpDown.DecimalPlaces = 2;
            this.horizontalResUpDown.Location = new System.Drawing.Point(233, 20);
            this.horizontalResUpDown.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.horizontalResUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.horizontalResUpDown.Name = "horizontalResUpDown";
            this.horizontalResUpDown.Size = new System.Drawing.Size(83, 20);
            this.horizontalResUpDown.TabIndex = 1;
            this.horizontalResUpDown.Value = new decimal(new int[] {
            96,
            0,
            0,
            0});
            this.horizontalResUpDown.ValueChanged += new System.EventHandler(this.horizontalResUpDown_ValueChanged);
            //
            // horizontalResSlider
            //
            this.horizontalResSlider.LargeChange = 200;
            this.horizontalResSlider.Location = new System.Drawing.Point(6, 19);
            this.horizontalResSlider.Maximum = 6000;
            this.horizontalResSlider.Minimum = 1;
            this.horizontalResSlider.Name = "horizontalResSlider";
            this.horizontalResSlider.Size = new System.Drawing.Size(224, 45);
            this.horizontalResSlider.SmallChange = 10;
            this.horizontalResSlider.TabIndex = 0;
            this.horizontalResSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.horizontalResSlider.Value = 96;
            this.horizontalResSlider.ValueChanged += new System.EventHandler(this.horizontalResSlider_ValueChanged);
            //
            // verticalResGroupBox
            //
            this.verticalResGroupBox.Controls.Add(this.verticalResUpDown);
            this.verticalResGroupBox.Controls.Add(this.verticalResSlider);
            this.verticalResGroupBox.Location = new System.Drawing.Point(3, 211);
            this.verticalResGroupBox.Name = "verticalResGroupBox";
            this.verticalResGroupBox.Size = new System.Drawing.Size(322, 70);
            this.verticalResGroupBox.TabIndex = 5;
            this.verticalResGroupBox.TabStop = false;
            this.verticalResGroupBox.Text = "Vertical Resolution";
            //
            // verticalResUpDown
            //
            this.verticalResUpDown.DecimalPlaces = 2;
            this.verticalResUpDown.Location = new System.Drawing.Point(233, 20);
            this.verticalResUpDown.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.verticalResUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.verticalResUpDown.Name = "verticalResUpDown";
            this.verticalResUpDown.Size = new System.Drawing.Size(83, 20);
            this.verticalResUpDown.TabIndex = 1;
            this.verticalResUpDown.Value = new decimal(new int[] {
            96,
            0,
            0,
            0});
            this.verticalResUpDown.ValueChanged += new System.EventHandler(this.verticalResUpDown_ValueChanged);
            //
            // verticalResSlider
            //
            this.verticalResSlider.LargeChange = 200;
            this.verticalResSlider.Location = new System.Drawing.Point(3, 19);
            this.verticalResSlider.Maximum = 6000;
            this.verticalResSlider.Minimum = 1;
            this.verticalResSlider.Name = "verticalResSlider";
            this.verticalResSlider.Size = new System.Drawing.Size(224, 45);
            this.verticalResSlider.SmallChange = 10;
            this.verticalResSlider.TabIndex = 0;
            this.verticalResSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.verticalResSlider.Value = 96;
            this.verticalResSlider.ValueChanged += new System.EventHandler(this.verticalResSlider_ValueChanged);
            //
            // embeddedProfileLabel
            //
            this.embeddedProfileLabel.AutoSize = true;
            this.embeddedProfileLabel.Location = new System.Drawing.Point(3, 284);
            this.embeddedProfileLabel.Name = "embeddedProfileLabel";
            this.embeddedProfileLabel.Size = new System.Drawing.Size(93, 13);
            this.embeddedProfileLabel.TabIndex = 6;
            this.embeddedProfileLabel.Text = "Embedded Profile:";
            //
            // embeddedProfleDescription
            //
            this.embeddedProfleDescription.AutoSize = true;
            this.embeddedProfleDescription.Location = new System.Drawing.Point(102, 284);
            this.embeddedProfleDescription.Name = "embeddedProfleDescription";
            this.embeddedProfleDescription.Size = new System.Drawing.Size(142, 13);
            this.embeddedProfleDescription.TabIndex = 7;
            this.embeddedProfleDescription.Text = "embedded profile description";
            //
            // cancelButton
            //
            this.cancelButton.Location = new System.Drawing.Point(263, 346);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            //
            // okButton
            //
            this.okButton.Location = new System.Drawing.Point(182, 346);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            //
            // SaveOptionsDialog
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(347, 381);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SaveOptionsDialog";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SaveOptionsDialog";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.jpegQualityGroupBox.ResumeLayout(false);
            this.jpegQualityGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jpegQualityUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.jpegQualitySlider)).EndInit();
            this.tiffOptionsPanel.ResumeLayout(false);
            this.tiffOptionsPanel.PerformLayout();
            this.horizontalResGroupBox.ResumeLayout(false);
            this.horizontalResGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalResUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalResSlider)).EndInit();
            this.verticalResGroupBox.ResumeLayout(false);
            this.verticalResGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.verticalResUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.verticalResSlider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox jpegQualityGroupBox;
        private System.Windows.Forms.NumericUpDown jpegQualityUpDown;
        private System.Windows.Forms.TrackBar jpegQualitySlider;
        private System.Windows.Forms.Panel tiffOptionsPanel;
        private System.Windows.Forms.CheckBox tiffLZWCheckBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.GroupBox horizontalResGroupBox;
        private System.Windows.Forms.NumericUpDown horizontalResUpDown;
        private System.Windows.Forms.TrackBar horizontalResSlider;
        private System.Windows.Forms.GroupBox verticalResGroupBox;
        private System.Windows.Forms.NumericUpDown verticalResUpDown;
        private System.Windows.Forms.TrackBar verticalResSlider;
        private System.Windows.Forms.Label embeddedProfileLabel;
        private System.Windows.Forms.Label embeddedProfleDescription;
    }
}