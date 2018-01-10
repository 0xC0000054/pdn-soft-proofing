/////////////////////////////////////////////////////////////////////////////////
//
// Soft Proofing Effect Plugin for Paint.NET
//
// This software is provided under the MIT License:
//   Copyright (C) 2016-2018 Nicholas Hayes
//
// See LICENSE.txt for complete licensing and attribution information.
//
/////////////////////////////////////////////////////////////////////////////////

using PaintDotNet;
using PaintDotNet.Effects;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Xml.Serialization;
using SoftProofing.Properties;
using System.Globalization;

namespace SoftProofing
{
    internal sealed class SoftProofingConfigDialog : EffectConfigDialog
    {
        private Button saveButton;
        private Button cancelButton;
        private HeaderLabel inputProfileHeader;
        private Button chooseInputProfileButton;
        private HeaderLabel displayProfileHeader;
        private HeaderLabel proofingProfileHeader;
        private Button chooseMonitorProfileButton;
        private HeaderLabel monitorIntentHeader;
        private ComboBox displayIntentCombo;
        private HeaderLabel proofingIntentHeader;
        private ComboBox proofingIntentCombo;
        private CheckBox gamutWarningCheckBox;
        private CheckBox blackPointCheckBox;
        private Label inputProfileDescription;
        private Label displayProfileDescription;
        private ComboBox proofingProfilesCombo;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private Button addProofingProfileButton;
        private Button removeProofingProfileButton;
        private ToolTip toolTip1;
        private IContainer components;

        private ColorProfileInfo inputProfile;
        private ColorProfileInfo displayProfile;
        private ColorProfileInfoCollection proofingColorProfiles;
        private int proofingProfileIndex;
        private int suppressTokenUpdateCounter;
        private XMLSettingsContainer xmlSettings;
        private ColorButton gamutWarningColorButton;
        private string destinationProfileTempPath;

        public SoftProofingConfigDialog()
        {
            UI.InitScaling(this);
            InitializeComponent();
            this.inputProfile = null;
            this.displayProfile = null;
            this.proofingColorProfiles = new ColorProfileInfoCollection();
            this.xmlSettings = new XMLSettingsContainer();
            this.destinationProfileTempPath = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                {
                    this.components.Dispose();
                    this.components = null;
                }
            }

            // Try to delete the temporary color profile here as the GC should
            // have already swept away the ColorContext in the SaveImage method that locks the file.
            if (!string.IsNullOrEmpty(this.destinationProfileTempPath))
            {
                try
                {
                    File.Delete(this.destinationProfileTempPath);
                }
                catch (IOException)
                {
                    // Ignore the exception thrown if the file is still locked.
                }
            }

            base.Dispose(disposing);
        }

        private void PushSuppressTokenUpdate()
        {
            this.suppressTokenUpdateCounter++;
        }

        private void PopSuppressTokenUpdate()
        {
            this.suppressTokenUpdateCounter--;
        }

        private void UpdateConfigToken()
        {
            if (this.suppressTokenUpdateCounter == 0)
            {
                FinishTokenUpdate();
            }
        }

        private DialogResult ShowErrorMessage(string message)
        {
            return MessageBox.Show(this, message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
        }

        private void InitializeProfileListFromToken(ColorProfileInfoCollection collection)
        {
            this.proofingColorProfiles.AddRange(collection);

            for (int i = 0; i < collection.Count; i++)
            {
                this.proofingProfilesCombo.Items.Add(collection[i].Description);
            }
        }

        protected override void InitialInitToken()
        {
            this.theEffectToken = new SoftProofingConfigToken(
                null,
                null,
                RenderingIntent.Perceptual,
                null,
                -1,
                RenderingIntent.RelativeColorimetric,
                false,
                System.Drawing.Color.Gray,
                true
                );
        }

        protected override void InitDialogFromToken(EffectConfigToken effectTokenCopy)
        {
            SoftProofingConfigToken token = (SoftProofingConfigToken)effectTokenCopy;

            PushSuppressTokenUpdate();

            if (token.InputColorProfile != null)
            {
                this.inputProfile = token.InputColorProfile;
                this.inputProfileDescription.Text = this.inputProfile.Description;
            }

            if (token.DisplayColorProfile != null)
            {
                this.displayProfile = token.DisplayColorProfile;
                this.displayProfileDescription.Text = this.displayProfile.Description;
            }

            if (token.ProofingColorProfiles != null)
            {
                if (this.proofingColorProfiles.Count == 0)
                {
                    InitializeProfileListFromToken(token.ProofingColorProfiles);
                }

                if (token.ProofingProfileIndex >= 0)
                {
                    this.proofingProfilesCombo.SelectedIndex = token.ProofingProfileIndex + 1;
                }
            }

            this.displayIntentCombo.SelectedIndex = (int)token.DisplayIntent;
            this.proofingIntentCombo.SelectedIndex = (int)token.ProofingIntent;
            this.gamutWarningCheckBox.Checked = token.ShowGamutWarning;
            this.gamutWarningColorButton.Value = token.GamutWarningColor;
            this.blackPointCheckBox.Checked = token.BlackPointCompensation;

            PopSuppressTokenUpdate();
        }

        protected override void InitTokenFromDialog()
        {
            SoftProofingConfigToken token = (SoftProofingConfigToken)this.theEffectToken;

            token.InputColorProfile = this.inputProfile;
            token.DisplayColorProfile = this.displayProfile;
            token.DisplayIntent = (RenderingIntent)this.displayIntentCombo.SelectedIndex;
            token.ProofingColorProfiles = this.proofingColorProfiles;
            token.ProofingProfileIndex = this.proofingProfileIndex;
            token.ProofingIntent = (RenderingIntent)this.proofingIntentCombo.SelectedIndex;
            token.ShowGamutWarning = this.gamutWarningCheckBox.Checked;
            token.GamutWarningColor = this.gamutWarningColorButton.Value;
            token.BlackPointCompensation = this.blackPointCheckBox.Checked;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.chooseInputProfileButton = new System.Windows.Forms.Button();
            this.chooseMonitorProfileButton = new System.Windows.Forms.Button();
            this.displayIntentCombo = new System.Windows.Forms.ComboBox();
            this.proofingIntentCombo = new System.Windows.Forms.ComboBox();
            this.blackPointCheckBox = new System.Windows.Forms.CheckBox();
            this.inputProfileDescription = new System.Windows.Forms.Label();
            this.displayProfileDescription = new System.Windows.Forms.Label();
            this.proofingProfilesCombo = new System.Windows.Forms.ComboBox();
            this.gamutWarningCheckBox = new System.Windows.Forms.CheckBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.addProofingProfileButton = new System.Windows.Forms.Button();
            this.removeProofingProfileButton = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.proofingIntentHeader = new PaintDotNet.HeaderLabel();
            this.monitorIntentHeader = new PaintDotNet.HeaderLabel();
            this.proofingProfileHeader = new PaintDotNet.HeaderLabel();
            this.displayProfileHeader = new PaintDotNet.HeaderLabel();
            this.inputProfileHeader = new PaintDotNet.HeaderLabel();
            this.gamutWarningColorButton = new SoftProofing.ColorButton();
            this.SuspendLayout();
            //
            // saveButton
            //
            this.saveButton.Location = new System.Drawing.Point(194, 313);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 11;
            this.saveButton.Text = "Save...";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            //
            // cancelButton
            //
            this.cancelButton.Location = new System.Drawing.Point(275, 313);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            //
            // chooseInputProfileButton
            //
            this.chooseInputProfileButton.Location = new System.Drawing.Point(275, 31);
            this.chooseInputProfileButton.Name = "chooseInputProfileButton";
            this.chooseInputProfileButton.Size = new System.Drawing.Size(75, 23);
            this.chooseInputProfileButton.TabIndex = 1;
            this.chooseInputProfileButton.Text = "Browse...";
            this.toolTip1.SetToolTip(this.chooseInputProfileButton, "Choose a color profile for the image");
            this.chooseInputProfileButton.UseVisualStyleBackColor = true;
            this.chooseInputProfileButton.Click += new System.EventHandler(this.chooseInputProfileButton_Click);
            //
            // chooseMonitorProfileButton
            //
            this.chooseMonitorProfileButton.Location = new System.Drawing.Point(275, 78);
            this.chooseMonitorProfileButton.Name = "chooseMonitorProfileButton";
            this.chooseMonitorProfileButton.Size = new System.Drawing.Size(75, 23);
            this.chooseMonitorProfileButton.TabIndex = 2;
            this.chooseMonitorProfileButton.Text = "Browse...";
            this.toolTip1.SetToolTip(this.chooseMonitorProfileButton, "Choose a color profile for the current monitor");
            this.chooseMonitorProfileButton.UseVisualStyleBackColor = true;
            this.chooseMonitorProfileButton.Click += new System.EventHandler(this.chooseMonitorProfileButton_Click);
            //
            // displayIntentCombo
            //
            this.displayIntentCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.displayIntentCombo.FormattingEnabled = true;
            this.displayIntentCombo.Items.AddRange(new object[] {
            "Perceptual",
            "Relative Colormetric",
            "Saturation",
            "Absolute Colormetric"});
            this.displayIntentCombo.Location = new System.Drawing.Point(13, 128);
            this.displayIntentCombo.MaxDropDownItems = 4;
            this.displayIntentCombo.Name = "displayIntentCombo";
            this.displayIntentCombo.Size = new System.Drawing.Size(225, 21);
            this.displayIntentCombo.TabIndex = 3;
            this.toolTip1.SetToolTip(this.displayIntentCombo, "Set the display rendering intent");
            this.displayIntentCombo.SelectedIndexChanged += new System.EventHandler(this.displayIntentCombo_SelectedIndexChanged);
            //
            // proofingIntentCombo
            //
            this.proofingIntentCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.proofingIntentCombo.FormattingEnabled = true;
            this.proofingIntentCombo.Items.AddRange(new object[] {
            "Perceptual",
            "Relative Colormetric",
            "Saturation",
            "Absolute Colormetric"});
            this.proofingIntentCombo.Location = new System.Drawing.Point(15, 223);
            this.proofingIntentCombo.MaxDropDownItems = 4;
            this.proofingIntentCombo.Name = "proofingIntentCombo";
            this.proofingIntentCombo.Size = new System.Drawing.Size(223, 21);
            this.proofingIntentCombo.TabIndex = 7;
            this.toolTip1.SetToolTip(this.proofingIntentCombo, "Set the soft proofing rendering intent");
            this.proofingIntentCombo.SelectedIndexChanged += new System.EventHandler(this.proofingIntentCombo_SelectedIndexChanged);
            //
            // blackPointCheckBox
            //
            this.blackPointCheckBox.AutoSize = true;
            this.blackPointCheckBox.Checked = true;
            this.blackPointCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.blackPointCheckBox.Location = new System.Drawing.Point(12, 291);
            this.blackPointCheckBox.Name = "blackPointCheckBox";
            this.blackPointCheckBox.Size = new System.Drawing.Size(148, 17);
            this.blackPointCheckBox.TabIndex = 10;
            this.blackPointCheckBox.Text = "Black point compensation";
            this.toolTip1.SetToolTip(this.blackPointCheckBox, "Adjust for differences in the darkest colors of the input and target profiles");
            this.blackPointCheckBox.UseVisualStyleBackColor = true;
            this.blackPointCheckBox.CheckedChanged += new System.EventHandler(this.blackPointCheckBox_CheckedChanged);
            //
            // inputProfileDescription
            //
            this.inputProfileDescription.AutoSize = true;
            this.inputProfileDescription.Location = new System.Drawing.Point(12, 36);
            this.inputProfileDescription.Name = "inputProfileDescription";
            this.inputProfileDescription.Size = new System.Drawing.Size(116, 13);
            this.inputProfileDescription.TabIndex = 18;
            this.inputProfileDescription.Text = "Input profile description";
            //
            // displayProfileDescription
            //
            this.displayProfileDescription.AutoSize = true;
            this.displayProfileDescription.Location = new System.Drawing.Point(12, 83);
            this.displayProfileDescription.Name = "displayProfileDescription";
            this.displayProfileDescription.Size = new System.Drawing.Size(126, 13);
            this.displayProfileDescription.TabIndex = 19;
            this.displayProfileDescription.Text = "Display profile description";
            //
            // proofingProfilesCombo
            //
            this.proofingProfilesCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.proofingProfilesCombo.FormattingEnabled = true;
            this.proofingProfilesCombo.Items.AddRange(new object[] {
            "None"});
            this.proofingProfilesCombo.Location = new System.Drawing.Point(15, 180);
            this.proofingProfilesCombo.Name = "proofingProfilesCombo";
            this.proofingProfilesCombo.Size = new System.Drawing.Size(254, 21);
            this.proofingProfilesCombo.TabIndex = 4;
            this.toolTip1.SetToolTip(this.proofingProfilesCombo, "Select the device profile to simulate");
            this.proofingProfilesCombo.SelectedIndexChanged += new System.EventHandler(this.proofingProfilesCombo_SelectedIndexChanged);
            //
            // gamutWarningCheckBox
            //
            this.gamutWarningCheckBox.AutoSize = true;
            this.gamutWarningCheckBox.Location = new System.Drawing.Point(12, 268);
            this.gamutWarningCheckBox.Name = "gamutWarningCheckBox";
            this.gamutWarningCheckBox.Size = new System.Drawing.Size(159, 17);
            this.gamutWarningCheckBox.TabIndex = 8;
            this.gamutWarningCheckBox.Text = "Mark colors ouside of gamut";
            this.toolTip1.SetToolTip(this.gamutWarningCheckBox, "Mark the colors that are outside of the gamut for the target device");
            this.gamutWarningCheckBox.UseVisualStyleBackColor = true;
            this.gamutWarningCheckBox.CheckedChanged += new System.EventHandler(this.gamutWarningCheckBox_CheckedChanged);
            //
            // openFileDialog1
            //
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Color Profiles (*.icm;*.icc)|*.icm;*.icc";
            //
            // saveFileDialog1
            //
            this.saveFileDialog1.Filter = "JPEG Image (*.jpg, *.jpe, *.jpeg, *.jfif)|*.jpg;*.jpe;*.jpeg;*.jfif|TIFF Image (*" +
    ".tif, *.tiff)|*.tif;*.tiff";
            this.saveFileDialog1.FilterIndex = 2;
            //
            // addProofingProfileButton
            //
            this.addProofingProfileButton.Image = global::SoftProofing.Properties.Resources.AddProfileIcon;
            this.addProofingProfileButton.Location = new System.Drawing.Point(275, 178);
            this.addProofingProfileButton.Name = "addProofingProfileButton";
            this.addProofingProfileButton.Size = new System.Drawing.Size(32, 23);
            this.addProofingProfileButton.TabIndex = 5;
            this.toolTip1.SetToolTip(this.addProofingProfileButton, "Add a proofing color profile");
            this.addProofingProfileButton.UseVisualStyleBackColor = true;
            this.addProofingProfileButton.Click += new System.EventHandler(this.addProofingProfileButton_Click);
            //
            // removeProofingProfileButton
            //
            this.removeProofingProfileButton.Image = global::SoftProofing.Properties.Resources.RemoveProfileIcon;
            this.removeProofingProfileButton.Location = new System.Drawing.Point(313, 178);
            this.removeProofingProfileButton.Name = "removeProofingProfileButton";
            this.removeProofingProfileButton.Size = new System.Drawing.Size(32, 23);
            this.removeProofingProfileButton.TabIndex = 6;
            this.toolTip1.SetToolTip(this.removeProofingProfileButton, "Remove the selected color profile");
            this.removeProofingProfileButton.UseVisualStyleBackColor = true;
            this.removeProofingProfileButton.Click += new System.EventHandler(this.removeProofingProfileButton_Click);
            //
            // proofingIntentHeader
            //
            this.proofingIntentHeader.ForeColor = System.Drawing.SystemColors.Highlight;
            this.proofingIntentHeader.Location = new System.Drawing.Point(13, 203);
            this.proofingIntentHeader.Name = "proofingIntentHeader";
            this.proofingIntentHeader.Size = new System.Drawing.Size(337, 14);
            this.proofingIntentHeader.TabIndex = 15;
            this.proofingIntentHeader.TabStop = false;
            this.proofingIntentHeader.Text = "Softproof rendering intent";
            //
            // monitorIntentHeader
            //
            this.monitorIntentHeader.ForeColor = System.Drawing.SystemColors.Highlight;
            this.monitorIntentHeader.Location = new System.Drawing.Point(13, 108);
            this.monitorIntentHeader.Name = "monitorIntentHeader";
            this.monitorIntentHeader.Size = new System.Drawing.Size(347, 14);
            this.monitorIntentHeader.TabIndex = 11;
            this.monitorIntentHeader.TabStop = false;
            this.monitorIntentHeader.Text = "Display rendering intent";
            //
            // proofingProfileHeader
            //
            this.proofingProfileHeader.ForeColor = System.Drawing.SystemColors.Highlight;
            this.proofingProfileHeader.Location = new System.Drawing.Point(13, 155);
            this.proofingProfileHeader.Name = "proofingProfileHeader";
            this.proofingProfileHeader.Size = new System.Drawing.Size(337, 14);
            this.proofingProfileHeader.TabIndex = 7;
            this.proofingProfileHeader.TabStop = false;
            this.proofingProfileHeader.Text = "Softproof target color profile";
            //
            // displayProfileHeader
            //
            this.displayProfileHeader.ForeColor = System.Drawing.SystemColors.Highlight;
            this.displayProfileHeader.Location = new System.Drawing.Point(13, 60);
            this.displayProfileHeader.Name = "displayProfileHeader";
            this.displayProfileHeader.Size = new System.Drawing.Size(337, 14);
            this.displayProfileHeader.TabIndex = 5;
            this.displayProfileHeader.TabStop = false;
            this.displayProfileHeader.Text = "Display color profile";
            //
            // inputProfileHeader
            //
            this.inputProfileHeader.ForeColor = System.Drawing.SystemColors.Highlight;
            this.inputProfileHeader.Location = new System.Drawing.Point(12, 12);
            this.inputProfileHeader.Name = "inputProfileHeader";
            this.inputProfileHeader.Size = new System.Drawing.Size(338, 14);
            this.inputProfileHeader.TabIndex = 3;
            this.inputProfileHeader.TabStop = false;
            this.inputProfileHeader.Text = "Input color profile";
            //
            // gamutWarningColorButton
            //
            this.gamutWarningColorButton.Location = new System.Drawing.Point(177, 268);
            this.gamutWarningColorButton.Name = "gamutWarningColorButton";
            this.gamutWarningColorButton.Size = new System.Drawing.Size(63, 17);
            this.gamutWarningColorButton.TabIndex = 20;
            this.toolTip1.SetToolTip(this.gamutWarningColorButton, "Select the gamut warning color");
            this.gamutWarningColorButton.Value = System.Drawing.Color.Gray;
            this.gamutWarningColorButton.Click += new System.EventHandler(this.gamutWarningColorButton_Click);
            //
            // SoftProofingConfigDialog
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.ClientSize = new System.Drawing.Size(362, 348);
            this.Controls.Add(this.gamutWarningColorButton);
            this.Controls.Add(this.removeProofingProfileButton);
            this.Controls.Add(this.gamutWarningCheckBox);
            this.Controls.Add(this.proofingProfilesCombo);
            this.Controls.Add(this.displayProfileDescription);
            this.Controls.Add(this.inputProfileDescription);
            this.Controls.Add(this.blackPointCheckBox);
            this.Controls.Add(this.proofingIntentCombo);
            this.Controls.Add(this.proofingIntentHeader);
            this.Controls.Add(this.addProofingProfileButton);
            this.Controls.Add(this.displayIntentCombo);
            this.Controls.Add(this.monitorIntentHeader);
            this.Controls.Add(this.chooseMonitorProfileButton);
            this.Controls.Add(this.proofingProfileHeader);
            this.Controls.Add(this.displayProfileHeader);
            this.Controls.Add(this.chooseInputProfileButton);
            this.Controls.Add(this.inputProfileHeader);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "SoftProofingConfigDialog";
            this.Text = "Soft Proofing";
            this.Controls.SetChildIndex(this.saveButton, 0);
            this.Controls.SetChildIndex(this.cancelButton, 0);
            this.Controls.SetChildIndex(this.inputProfileHeader, 0);
            this.Controls.SetChildIndex(this.chooseInputProfileButton, 0);
            this.Controls.SetChildIndex(this.displayProfileHeader, 0);
            this.Controls.SetChildIndex(this.proofingProfileHeader, 0);
            this.Controls.SetChildIndex(this.chooseMonitorProfileButton, 0);
            this.Controls.SetChildIndex(this.monitorIntentHeader, 0);
            this.Controls.SetChildIndex(this.displayIntentCombo, 0);
            this.Controls.SetChildIndex(this.addProofingProfileButton, 0);
            this.Controls.SetChildIndex(this.proofingIntentHeader, 0);
            this.Controls.SetChildIndex(this.proofingIntentCombo, 0);
            this.Controls.SetChildIndex(this.blackPointCheckBox, 0);
            this.Controls.SetChildIndex(this.inputProfileDescription, 0);
            this.Controls.SetChildIndex(this.displayProfileDescription, 0);
            this.Controls.SetChildIndex(this.proofingProfilesCombo, 0);
            this.Controls.SetChildIndex(this.gamutWarningCheckBox, 0);
            this.Controls.SetChildIndex(this.removeProofingProfileButton, 0);
            this.Controls.SetChildIndex(this.gamutWarningColorButton, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void LoadSettings()
        {
            string userDataPath = Services.GetService<PaintDotNet.AppModel.IAppInfoService>().UserDataDirectory;

            if (!Directory.Exists(userDataPath))
            {
                Directory.CreateDirectory(userDataPath);
            }

            string path = Path.Combine(userDataPath, "SoftProofingSettings.xml");
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(XMLSettingsContainer));
                    this.xmlSettings = (XMLSettingsContainer)serializer.Deserialize(fs);
                }

                // Check to make sure that the dialog has not already been initialized from the token.
                if (this.xmlSettings.InputProfile != null && this.inputProfile == null)
                {
                    this.inputProfile = this.xmlSettings.InputProfile;
                }

                if (this.xmlSettings.DisplayProfile != null && this.displayProfile == null)
                {
                    this.displayProfile = this.xmlSettings.DisplayProfile;
                }

                if (this.xmlSettings.ProofingProfiles != null && this.proofingColorProfiles.Count == 0)
                {
                    foreach (var profile in this.xmlSettings.ProofingProfiles)
                    {
                        if (File.Exists(profile.Path))
                        {
                            this.proofingColorProfiles.Add(profile);
                        }
                    }

                    for (int i = 0; i < this.proofingColorProfiles.Count; i++)
                    {
                        this.proofingProfilesCombo.Items.Add(this.proofingColorProfiles[i].Description);
                    }
                }
            }
            catch (FileNotFoundException)
            {
            }
        }

        private void SaveSettings()
        {
            string path = Path.Combine(Services.GetService<PaintDotNet.AppModel.IAppInfoService>().UserDataDirectory, "SoftProofingSettings.xml");

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(XMLSettingsContainer));
                    serializer.Serialize(fs, this.xmlSettings);
                }
            }
            catch (IOException ex)
            {
                ShowErrorMessage(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);

            PluginThemingUtil.UpdateControlBackColor(this);
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);

            PluginThemingUtil.UpdateControlForeColor(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                LoadSettings();
            }
            catch (IOException ex)
            {
                ShowErrorMessage(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                ShowErrorMessage(ex.Message);
            }
            PluginThemingUtil.EnableEffectDialogTheme(this);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            try
            {
                string srgbProfile = ColorProfileHelper.GetSRGBProfilePath();

                if (this.inputProfile == null)
                {
                    this.inputProfile = new ColorProfileInfo(srgbProfile);
                }
                this.inputProfileDescription.Text = this.inputProfile.Description;

                if (this.displayProfile == null)
                {
                    string displayProfilePath = ColorProfileHelper.GetMonitorColorProfilePath(this.Handle);
                    if (string.IsNullOrEmpty(displayProfilePath))
                    {
                        displayProfilePath = srgbProfile;
                    }
                    this.displayProfile = new ColorProfileInfo(displayProfilePath);
                }

                this.displayProfileDescription.Text = this.displayProfile.Description;
                if (this.proofingProfilesCombo.SelectedIndex == -1)
                {
                    this.proofingProfilesCombo.SelectedIndex = 0;
                }
            }
            catch (IOException ex)
            {
                ShowErrorMessage(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                ShowErrorMessage(ex.Message);
            }
            catch (Win32Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        private bool CanSaveInColorSpace(ProfileColorSpace colorSpace)
        {
            if (colorSpace != ProfileColorSpace.RGB &&
                colorSpace != ProfileColorSpace.CMYK &&
                colorSpace != ProfileColorSpace.Gray)
            {
                ShowErrorMessage(Resources.SaveColorSpaceUnsupportedError);
                return false;
            }

            return true;
        }

        private static bool SaveFormatIsTIFF(string path)
        {
            string extension = Path.GetExtension(path);

            return (extension.Equals(".tif", StringComparison.OrdinalIgnoreCase) || extension.Equals(".tiff", StringComparison.OrdinalIgnoreCase));
        }

        private static unsafe bool HasTransparency(Surface surface)
        {
            int height = surface.Height;
            int width = surface.Width;

            for (int y = 0; y < height; y++)
            {
                ColorBgra* ptr = surface.GetRowAddressUnchecked(y);
                ColorBgra* endPtr = ptr + width;

                while (ptr < endPtr)
                {
                    if (ptr->A != 255)
                    {
                        return true;
                    }
                    ptr++;
                }
            }

            return false;
        }

        private BitmapSource GetBitmapSourceFromEffectSourceSurface(bool tiff, SaveOptions options)
        {
            if (tiff && HasTransparency(this.EffectSourceSurface))
            {
                return BitmapSource.Create(
                                    this.EffectSourceSurface.Width,
                                    this.EffectSourceSurface.Height,
                                    options.HorizontalResolution,
                                    options.VerticalResolution,
                                    PixelFormats.Bgra32,
                                    null,
                                    this.EffectSourceSurface.Scan0.Pointer,
                                    (int)this.EffectSourceSurface.Scan0.Length,
                                    this.EffectSourceSurface.Stride
                                    );
            }
            else
            {
                Surface source = this.EffectSourceSurface;

                WriteableBitmap image = new WriteableBitmap(
                    source.Width,
                    source.Height,
                    options.HorizontalResolution,
                    options.VerticalResolution,
                    PixelFormats.Bgr24,
                    null
                    );

                unsafe
                {
                    byte* scan0 = (byte*)image.BackBuffer.ToPointer();
                    int stride = image.BackBufferStride;

                    for (int y = 0; y < source.Height; y++)
                    {
                        ColorBgra* src = source.GetRowAddressUnchecked(y);
                        byte* dst = scan0 + (y * stride);

                        for (int x = 0; x < source.Width; x++)
                        {
                            dst[0] = src->B;
                            dst[1] = src->G;
                            dst[2] = src->R;
                            src++;
                            dst += 3;
                        }
                    }
                }

                return image;
            }
        }

        private PixelFormat GetDestiniationPixelFormat(bool tiff, ProfileColorSpace colorSpace)
        {
            PixelFormat destiniationFormat;
            if (tiff)
            {
                switch (colorSpace)
                {
                    case ProfileColorSpace.CMYK:
                        destiniationFormat = PixelFormats.Cmyk32;
                        break;
                    case ProfileColorSpace.Gray:
                        destiniationFormat = PixelFormats.Gray8;
                        break;
                    case ProfileColorSpace.RGB:
                        destiniationFormat = HasTransparency(this.EffectSourceSurface) ? PixelFormats.Bgra32 : PixelFormats.Bgr24;
                        break;
                    default:
                        throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Resources.UnsupportedColorConversionFormat, colorSpace));
                }
            }
            else
            {
                // WIC can encode JPEG images as CMYK according to http://msdn.microsoft.com/en-us/library/windows/desktop/ee719797(v=vs.85).aspx
                switch (colorSpace)
                {
                    case ProfileColorSpace.CMYK:
                        destiniationFormat = PixelFormats.Cmyk32;
                        break;
                    case ProfileColorSpace.Gray:
                        destiniationFormat = PixelFormats.Gray8;
                        break;
                    case ProfileColorSpace.RGB:
                        destiniationFormat = PixelFormats.Bgr24;
                        break;
                    default:
                        throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Resources.UnsupportedColorConversionFormat, colorSpace));
                }
            }

            return destiniationFormat;
        }

        private bool SaveImage()
        {
            bool result = false;
            ColorProfileInfo profile = this.proofingProfileIndex >= 0 ? this.proofingColorProfiles[this.proofingProfileIndex] : this.inputProfile;

            if (CanSaveInColorSpace(profile.ColorSpace))
            {
                try
                {
                    if (this.saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                    {
                        bool tiff = SaveFormatIsTIFF(this.saveFileDialog1.FileName);

                        using (SaveOptionsDialog optionsDialog = new SaveOptionsDialog(tiff, profile))
                        {
                            optionsDialog.BackColor = BackColor;
                            optionsDialog.ForeColor = ForeColor;

                            if (optionsDialog.ShowDialog(this) == DialogResult.OK)
                            {
                                SaveOptions options = optionsDialog.Options;

                                BitmapSource source = null;
                                Uri destinationProfileUri = null;

                                if (profile.Equals(this.inputProfile))
                                {
                                    // No conversion is necessary when saving as the input color profile.
                                    source = GetBitmapSourceFromEffectSourceSurface(tiff, options);
                                    destinationProfileUri = new Uri(this.inputProfile.Path, UriKind.Absolute);
                                }
                                else
                                {
                                    // Convert the image to the destination color profile.
                                    PixelFormat destinationPixelFormat = GetDestiniationPixelFormat(tiff, profile.ColorSpace);
                                    WriteableBitmap writable = new WriteableBitmap(
                                        this.EffectSourceSurface.Width,
                                        this.EffectSourceSurface.Height,
                                        options.HorizontalResolution,
                                        options.VerticalResolution,
                                        destinationPixelFormat,
                                        null
                                        );

                                    RenderingIntent destinationRenderingIntent = (RenderingIntent)this.proofingIntentCombo.SelectedIndex;

                                    ColorProfileHelper.ConvertToColorProfile(
                                        this.inputProfile.Path,
                                        profile.Path,
                                        destinationRenderingIntent,
                                        this.blackPointCheckBox.Checked,
                                        this.EffectSourceSurface,
                                        writable
                                        );

                                    source = writable;

                                    // Write a new profile with the selected rendering intent, if necessary.
                                    if (ColorProfileHelper.ChangeProfileRenderingIntent(profile.Path, destinationRenderingIntent, out this.destinationProfileTempPath))
                                    {
                                        destinationProfileUri = new Uri(this.destinationProfileTempPath, UriKind.Absolute);
                                    }
                                    else
                                    {
                                        destinationProfileUri = new Uri(profile.Path, UriKind.Absolute);
                                    }
                                }

                                BitmapEncoder encoder;
                                if (tiff)
                                {
                                    TIFFSaveOptions tiffOptions = (TIFFSaveOptions)options;
                                    encoder = new TiffBitmapEncoder()
                                    {
                                        Compression = tiffOptions.LZWCompression ? TiffCompressOption.Lzw : TiffCompressOption.None
                                    };
                                }
                                else
                                {
                                    JPEGSaveOptions jpegOptions = (JPEGSaveOptions)options;
                                    encoder = new JpegBitmapEncoder()
                                    {
                                        QualityLevel = jpegOptions.Quality,
                                    };
                                }

                                ColorContext[] colorContexts = new ColorContext[] { new ColorContext(destinationProfileUri) };

                                encoder.Frames.Add(BitmapFrame.Create(source, null, null, Array.AsReadOnly(colorContexts)));

                                using (FileStream stream = new FileStream(this.saveFileDialog1.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                                {
                                    encoder.Save(stream);
                                }

                                result = true;
                            }
                        }
                    }
                }
                catch (IOException ex)
                {
                    ShowErrorMessage(ex.Message);
                }
                catch (LCMSException ex)
                {
                    ShowErrorMessage(ex.Message);
                }
                catch (OverflowException ex)
                {
                    ShowErrorMessage(ex.Message);
                }
                catch (UnauthorizedAccessException ex)
                {
                    ShowErrorMessage(ex.Message);
                }
            }

            return result;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (SaveImage())
            {
                FinishTokenUpdate();
                this.DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void chooseInputProfileButton_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.inputProfile = new ColorProfileInfo(this.openFileDialog1.FileName);
                this.inputProfileDescription.Text = this.inputProfile.Description;
                this.xmlSettings.InputProfile = this.inputProfile;
                SaveSettings();
                UpdateConfigToken();
            }
        }

        private void chooseMonitorProfileButton_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.displayProfile = new ColorProfileInfo(this.openFileDialog1.FileName);
                this.displayProfileDescription.Text = this.displayProfile.Description;
                this.xmlSettings.DisplayProfile = this.displayProfile;
                SaveSettings();
                UpdateConfigToken();
            }
        }

        private void addProofingProfileButton_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                ColorProfileInfo info = new ColorProfileInfo(this.openFileDialog1.FileName);
                this.proofingColorProfiles.Add(info);
                this.xmlSettings.ProofingProfiles = this.proofingColorProfiles;
                SaveSettings();
                int index = this.proofingProfilesCombo.Items.Add(info.Description);
                this.proofingProfilesCombo.SelectedIndex = index;
            }
        }

        private void removeProofingProfileButton_Click(object sender, EventArgs e)
        {
            int index = this.proofingProfilesCombo.SelectedIndex;

            this.proofingColorProfiles.RemoveAt(index);
            this.xmlSettings.ProofingProfiles = this.proofingColorProfiles;
            SaveSettings();
            this.proofingProfilesCombo.Items.RemoveAt(index);
            this.proofingProfilesCombo.SelectedIndex = 0;
        }

        private void displayIntentCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateConfigToken();
        }

        private void proofingIntentCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateConfigToken();
        }

        private void blackPointCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateConfigToken();
        }

        private void proofingProfilesCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.proofingIntentCombo.SelectedIndex >= 0)
            {
                int index = this.proofingProfilesCombo.SelectedIndex;

                // The first item in the combo box is used when no profile is selected.
                if (index > 0)
                {
                    this.removeProofingProfileButton.Enabled = true;
                    this.proofingProfileIndex = index - 1;
                }
                else
                {
                    this.removeProofingProfileButton.Enabled = false;
                    this.proofingProfileIndex = -1;
                }

                UpdateConfigToken();
            }
        }

        private void gamutWarningCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.saveButton.Enabled = !this.gamutWarningCheckBox.Checked;
            UpdateConfigToken();
        }

        private void gamutWarningColorButton_Click(object sender, EventArgs e)
        {
            using (ColorPickerForm form = new ColorPickerForm("Choose a gamut warning color:"))
            {
                form.BackColor = BackColor;
                form.ForeColor = ForeColor;

                form.SetDefaultColor(this.gamutWarningColorButton.Value);
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    this.gamutWarningColorButton.Value = form.UserPrimaryColor;
                    UpdateConfigToken();
                }
            }
        }
    }
}
