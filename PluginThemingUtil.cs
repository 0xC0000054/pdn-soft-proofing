﻿/////////////////////////////////////////////////////////////////////////////////
//
// Soft Proofing Effect Plugin for Paint.NET
//
// This software is provided under the MIT License:
//   Copyright (C) 2017-2018 Nicholas Hayes
//
// See LICENSE.txt for complete licensing and attribution information.
//
/////////////////////////////////////////////////////////////////////////////////

using PaintDotNet;
using PaintDotNet.Effects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace SoftProofing
{
    internal static class PluginThemingUtil
    {
        // Paint.NET added theming support for plug-ins in 4.20.
        private static readonly Version PluginThemingMinVersion = new Version("4.20");

        private static MethodInfo useAppThemeSetter = null;
        private static bool initAppThemeSetter = false;

        public static void EnableEffectDialogTheme(EffectConfigDialog dialog)
        {
            try
            {
                Version pdnVersion = dialog.Services.GetService<PaintDotNet.AppModel.IAppInfoService>().AppVersion;

                if (pdnVersion >= PluginThemingMinVersion)
                {
                    if (!initAppThemeSetter)
                    {
                        initAppThemeSetter = true;

                        PropertyInfo propertyInfo = typeof(EffectConfigDialog).GetProperty("UseAppThemeColors");
                        if (propertyInfo != null)
                        {
                            useAppThemeSetter = propertyInfo.GetSetMethod();
                        }
                    }


                    if (useAppThemeSetter != null)
                    {
                        useAppThemeSetter.Invoke(dialog, new object[] { true });
                    }
                }
            }
            catch
            {
                // Ignore any exceptions that are thrown when trying to enable the dialog theming.
                // The dialog should be shown to the user even if theming could not be enabled.
            }
        }

        public static void UpdateControlBackColor(Control root)
        {
            Color backColor = root.BackColor;

            Stack<Control> stack = new Stack<Control>();
            stack.Push(root);

            while (stack.Count > 0)
            {
                Control parent = stack.Pop();

                var controls = parent.Controls;

                for (int i = 0; i < controls.Count; i++)
                {
                    Control control = controls[i];

                    if (control is Button button)
                    {
                        // Reset the BackColor of all Button controls.
                        button.UseVisualStyleBackColor = true;
                    }
                    else
                    {
                        // Update the BackColor for all child controls as some controls
                        // do not change the BackColor when the parent control does.

                        control.BackColor = backColor;

                        if (control.HasChildren)
                        {
                            stack.Push(control);
                        }
                    }
                }
            }
        }

        public static void UpdateControlForeColor(Control root)
        {
            Color foreColor = root.ForeColor;

            Stack<Control> stack = new Stack<Control>();
            stack.Push(root);

            while (stack.Count > 0)
            {
                Control parent = stack.Pop();

                var controls = parent.Controls;

                for (int i = 0; i < controls.Count; i++)
                {
                    Control control = controls[i];

                    if (control is Button button)
                    {
                        // Reset the ForeColor of all Button controls.
                        button.ForeColor = SystemColors.ControlText;
                    }
                    else if (control is LinkLabel link)
                    {
                        link.LinkColor = foreColor;
                    }
                    else
                    {
                        // Update the ForeColor for all child controls as some controls
                        // do not change the ForeColor when the parent control does.

                        control.ForeColor = foreColor;

                        if (control.HasChildren)
                        {
                            stack.Push(control);
                        }
                    }
                }
            }
        }
    }
}
