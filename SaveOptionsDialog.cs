/////////////////////////////////////////////////////////////////////////////////
//
// Soft Proofing Effect Plugin for Paint.NET
//
// This software is provided under the MIT License:
//   Copyright (C) 2016-2017 Nicholas Hayes
//
// See LICENSE.txt for complete licensing and attribution information.
//
/////////////////////////////////////////////////////////////////////////////////

using PaintDotNet;
using SoftProofing.Properties;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SoftProofing
{
    internal partial class SaveOptionsDialog : Form
    {
        private readonly bool tiff;

        public SaveOptionsDialog(bool tiff, ColorProfileInfo profileInfo)
        {
            if (profileInfo == null)
            {
                throw new ArgumentNullException("profileInfo");
            }

            UI.InitScaling(this);
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            InitializeComponent();

            this.tiff = tiff;
            if (tiff)
            {
                this.jpegQualityGroupBox.Visible = false;
                this.tiffOptionsPanel.Visible = true;
            }
            else
            {
                this.tiffOptionsPanel.Visible = false;
                this.jpegQualityGroupBox.Visible = true;
            }

            this.Text = string.Format(CultureInfo.CurrentCulture, Resources.SaveOptionsTitleFormat, tiff ? "TIFF" : "JPEG");
            this.embeddedProfleDescription.Text = profileInfo.Description;

            SetTabOrder();
            ResizeDialog();
        }

        public override Color BackColor
        {
            get
            {
                if (SystemInformation.HighContrast || !VisualStyleInformation.IsEnabledByUser)
                {
                    return DefaultBackColor;
                }

                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                PluginThemingUtil.UpdateControlBackColor(this);
            }
        }

        public override Color ForeColor
        {
            get
            {
                if (SystemInformation.HighContrast || !VisualStyleInformation.IsEnabledByUser)
                {
                    return DefaultForeColor;
                }

                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                PluginThemingUtil.UpdateControlForeColor(this);
            }
        }

        public SaveOptions Options
        {
            get
            {
                if (this.tiff)
                {
                    return new TIFFSaveOptions(tiffLZWCheckBox.Checked, (double)horizontalResUpDown.Value, (double)verticalResUpDown.Value);
                }
                else
                {
                    return new JPEGSaveOptions(jpegQualitySlider.Value, (double)horizontalResUpDown.Value, (double)verticalResUpDown.Value);
                }
            }
        }

        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);

            PluginThemingUtil.UpdateControlBackColor(this);
            PluginThemingUtil.UpdateControlForeColor(this);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void jpegQualitySlider_ValueChanged(object sender, EventArgs e)
        {
            if (jpegQualityUpDown.Value != jpegQualitySlider.Value)
            {
                jpegQualityUpDown.Value = jpegQualitySlider.Value;
            }
        }

        private void jpegQualityUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (jpegQualitySlider.Value != (int)jpegQualityUpDown.Value)
            {
                jpegQualitySlider.Value = (int)jpegQualityUpDown.Value;
            }
        }

        private void horizontalResSlider_ValueChanged(object sender, EventArgs e)
        {
            if (horizontalResUpDown.Value != horizontalResSlider.Value)
            {
                horizontalResUpDown.Value = horizontalResSlider.Value;
            }
        }

        private void horizontalResUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (horizontalResSlider.Value != (int)horizontalResUpDown.Value)
            {
                horizontalResSlider.Value = (int)horizontalResUpDown.Value;
            }
        }

        private void verticalResSlider_ValueChanged(object sender, EventArgs e)
        {
            if (verticalResUpDown.Value != verticalResSlider.Value)
            {
                verticalResUpDown.Value = verticalResSlider.Value;
            }
        }

        private void verticalResUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (verticalResSlider.Value != (int)verticalResUpDown.Value)
            {
                verticalResSlider.Value = (int)verticalResUpDown.Value;
            }
        }

        private void SetTabOrder()
        {
            int nextIndex = this.tiff ? 1 : 2;

            this.horizontalResSlider.TabIndex = nextIndex;
            this.horizontalResUpDown.TabIndex = nextIndex + 1;
            this.verticalResSlider.TabIndex = nextIndex + 2;
            this.verticalResUpDown.TabIndex = nextIndex + 3;
            this.okButton.TabIndex = nextIndex + 4;
            this.cancelButton.TabIndex = nextIndex + 5;
        }

        private void ResizeDialog()
        {
            SuspendLayout();
            int profileLabelBottom = this.embeddedProfileLabel.Bounds.Bottom;
            int scaleOffset = UI.ScaleHeight(50);

            int okButtonXLocation = this.okButton.Location.X;
            this.okButton.Location = new Point(okButtonXLocation, profileLabelBottom + this.okButton.Margin.Vertical + scaleOffset);
            int cancelButtonXLocation = cancelButton.Location.X;
            this.cancelButton.Location = new Point(cancelButtonXLocation, profileLabelBottom + this.cancelButton.Margin.Vertical + scaleOffset);
            this.okButton.BringToFront();
            this.cancelButton.BringToFront();

            int width = this.ClientSize.Width;
            this.ClientSize = new Size(width, this.ClientRectangle.Top + this.okButton.Bottom + this.okButton.Margin.Vertical);
            PerformLayout();
        }
    }
}
