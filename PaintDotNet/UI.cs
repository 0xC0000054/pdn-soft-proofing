/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See src/Resources/Files/License.txt for full licensing and attribution      //
// details.                                                                    //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using PaintDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace PaintDotNet
{
    /// <summary>
    /// Contains static methods related to the user interface.
    /// </summary>
    internal static class UI
    {
        private static bool initScales = false;
        private static float xScale;
        private static float yScale;

        private static void InitScaleFactors(Control c)
        {
            if (c == null)
            {
                xScale = 1.0f;
                yScale = 1.0f;
            }
            else
            {
                using (Graphics g = c.CreateGraphics())
                {
                    xScale = g.DpiX / 96.0f;
                    yScale = g.DpiY / 96.0f;
                }
            }

            initScales = true;
        }

        public static void InitScaling(Control c)
        {
            if (!initScales)
            {
                InitScaleFactors(c);
            }
        }

        public static float ScaleWidth(float width)
        {
            return (float)Math.Round(width * GetXScaleFactor());
        }

        public static int ScaleWidth(int width)
        {
            return (int)Math.Round((float)width * GetXScaleFactor());
        }

        public static int ScaleHeight(int height)
        {
            return (int)Math.Round((float)height * GetYScaleFactor());
        }

        public static float ScaleHeight(float height)
        {
            return (float)Math.Round(height * GetYScaleFactor());
        }

        public static Size ScaleSize(Size size)
        {
            return new Size(ScaleWidth(size.Width), ScaleHeight(size.Height));
        }

        public static Point ScalePoint(Point pt)
        {
            return new Point(ScaleWidth(pt.X), ScaleHeight(pt.Y));
        }

        public static float GetXScaleFactor()
        {
            if (!initScales)
            {
                throw new InvalidOperationException("Must call InitScaling() first");
            }

            return xScale;
        }

        public static float GetYScaleFactor()
        {
            if (!initScales)
            {
                throw new InvalidOperationException("Must call InitScaling() first");
            }

            return yScale;
        }

        private static VisualStyleClass DetermineVisualStyleClass()
        {
            VisualStyleClass style = VisualStyleClass.Other;
            try
            {
                style = DetermineVisualStyleClassImpl();
            }
            catch (Exception)
            {
            }
            return style;
        }

        private static VisualStyleClass DetermineVisualStyleClassImpl()
        {
            VisualStyleClass vsClass;

            if (!VisualStyleInformation.IsSupportedByOS)
            {
                vsClass = VisualStyleClass.Classic;
            }
            else if (!VisualStyleInformation.IsEnabledByUser)
            {
                vsClass = VisualStyleClass.Classic;
            }
            else if (0 == string.Compare(VisualStyleInformation.Author, "MSX", StringComparison.InvariantCulture) &&
                     0 == string.Compare(VisualStyleInformation.DisplayName, "Aero style", StringComparison.InvariantCulture))
            {
                vsClass = VisualStyleClass.Aero;
            }
            else if (0 == string.Compare(VisualStyleInformation.Company, "Microsoft Corporation", StringComparison.InvariantCulture) &&
                     0 == string.Compare(VisualStyleInformation.Author, "Microsoft Design Team", StringComparison.InvariantCulture))
            {
                if (0 == string.Compare(VisualStyleInformation.DisplayName, "Windows XP style", StringComparison.InvariantCulture) ||  // Luna
                    0 == string.Compare(VisualStyleInformation.DisplayName, "Zune Style", StringComparison.InvariantCulture) ||        // Zune
                    0 == string.Compare(VisualStyleInformation.DisplayName, "Media Center style", StringComparison.InvariantCulture))  // Royale
                {
                    vsClass = VisualStyleClass.Luna;
                }
                else
                {
                    vsClass = VisualStyleClass.Other;
                }
            }
            else
            {
                vsClass = VisualStyleClass.Other;
            }

            return vsClass;
        }

        public static VisualStyleClass VisualStyleClass
        {
            get
            {
                return DetermineVisualStyleClass();
            }
        }
    }
}
