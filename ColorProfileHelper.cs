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
using SoftProofing.LCMSInterop;
using SoftProofing.Properties;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SoftProofing
{
    internal static class ColorProfileHelper
    {
        /// <summary>
        /// Gets the path of the color profile for the monitor containing the specified window handle.
        /// </summary>
        /// <param name="hwnd">The window handle for which to retrieve the color profile.</param>
        /// <returns>
        /// A string containing the color profile for the monitor; or null if no color profile is assigned.
        /// </returns>
        internal static string GetMonitorColorProfilePath(IntPtr hwnd)
        {
            string profile = null;

            IntPtr hMonitor = SafeNativeMethods.MonitorFromWindow(hwnd, NativeConstants.MONITOR_DEFAULTTONEAREST);

            NativeStructs.MONITORINFOEX monitorInfo = new NativeStructs.MONITORINFOEX();
            monitorInfo.cbSize = (uint)Marshal.SizeOf(typeof(NativeStructs.MONITORINFOEX));

            if (SafeNativeMethods.GetMonitorInfo(hMonitor, ref monitorInfo))
            {
                using (SafeDCHandle hdc = SafeNativeMethods.CreateDC(monitorInfo.szDeviceName, monitorInfo.szDeviceName, null, IntPtr.Zero))
                {
                    if (!hdc.IsInvalid)
                    {
                        uint size = 0;
                        SafeNativeMethods.GetICMProfile(hdc, ref size, null);

                        if (size > 0)
                        {
                            StringBuilder builder = new StringBuilder((int)size);

                            if (SafeNativeMethods.GetICMProfile(hdc, ref size, builder))
                            {
                                profile = builder.ToString();
                            }
                        }
                    }
                }
            }

            return profile;
        }

        /// <summary>
        /// Gets the path to the installed sRGB profile.
        /// </summary>
        /// <returns>The path of the sRGB profile.</returns>
        /// <exception cref="Win32Exception">An error occurred when retrieving the path.</exception>
        internal static string GetSRGBProfilePath()
        {
            StringBuilder builder = new StringBuilder(NativeConstants.MAX_PATH);
            uint bufferSize = NativeConstants.MAX_PATH;

            if (!SafeNativeMethods.GetStandardColorSpaceProfile(null, NativeConstants.LCS_sRGB, builder, ref bufferSize))
            {
                throw new Win32Exception();
            }

            string path = null;

            string profile = builder.ToString();
            if (Path.IsPathRooted(profile))
            {
                path = profile;
            }
            else
            {            
                // GetStandardColorSpace may return a relative path to the system color directory.
                bufferSize = NativeConstants.MAX_PATH;
                if (!SafeNativeMethods.GetColorDirectory(null, builder, ref bufferSize))
                {
                    throw new Win32Exception();
                }
                path = Path.Combine(builder.ToString(), profile);
            }

            return path;
        }

        /// <summary>
        /// Gets the description and color space from the specified color profile.
        /// </summary>
        /// <param name="fileName">The path of the color profile.</param>
        internal static void GetProfileInfo(string fileName, out string description, out ProfileColorSpace colorSpace)
        {
            description = "Unknown profile";
            colorSpace = ProfileColorSpace.Unknown;

            using (LCMSProfileHandle hProfile = LCMSHelper.OpenColorProfile(fileName))
            {
                if (!hProfile.IsInvalid)
                {
                    colorSpace = LCMSHelper.GetProfileColorSpace(hProfile);

                    uint descriptionSize = LCMSHelper.GetProfileInfoSize(hProfile, LCMSEnums.ProfileInfoType.Description);

                    if (descriptionSize > 0U)
                    {
                        description = LCMSHelper.GetProfileInfo(hProfile, LCMSEnums.ProfileInfoType.Description, descriptionSize);
                    }
                }
            }
        }

        private static LCMSEnums.BitmapFormat BitmapFormatFromWICPixelFormat(PixelFormat format)
        {
            if (format == PixelFormats.Bgr24)
            {
                return LCMSEnums.BitmapFormat.Bgr8;
            }
            else if (format == PixelFormats.Cmyk32)
            {
                return LCMSEnums.BitmapFormat.Cmyk8;
            }
            else if (format == PixelFormats.Gray8)
            {
                return LCMSEnums.BitmapFormat.Gray8;
            }
            else
            {
                return LCMSEnums.BitmapFormat.Bgra8;
            }
        }

        /// <summary>
        /// Converts the image to the specified output color profile.
        /// </summary>
        /// <param name="inputProfilePath">The path of the input color profile.</param>
        /// <param name="outputProfilePath">The path of the output color profile.</param>
        /// <param name="intent">The rendering intent to use for the conversion.</param>
        /// <param name="blackPointCompensation"><c>true</c> if black point compensation should be used; otherwise, <c>false</c>.</param>
        /// <param name="source">The source image to convert.</param>
        /// <param name="destination">The destination image to place the converted pixels.</param>
        /// <exception cref="LCMSException">
        /// The input profile could not be opened.
        /// -or-
        /// The output profile could not be opened.
        /// -or-
        /// The color transform could not be created.
        /// -or-
        /// The source and destination images are not the same size.
        /// </exception>
        internal static void ConvertToColorProfile(
            string inputProfilePath,
            string outputProfilePath,
            RenderingIntent intent,
            bool blackPointCompensation,
            Surface source,
            WriteableBitmap destination
            )
        {
            using (LCMSProfileHandle inputProfile = LCMSHelper.OpenColorProfile(inputProfilePath))
            {
                if (inputProfile.IsInvalid)
                {
                    throw new LCMSException(Resources.OpenSourceProfileError);
                }

                using (LCMSProfileHandle outputProfile = LCMSHelper.OpenColorProfile(outputProfilePath))
                {
                    if (outputProfile.IsInvalid)
                    {
                        throw new LCMSException(Resources.OpenDestiniationProfileError);
                    }

                    LCMSStructs.BitmapData sourceBitmapData = new LCMSStructs.BitmapData
                    {
                        width = (uint)source.Width,
                        height = (uint)source.Height,
                        format = LCMSEnums.BitmapFormat.Bgra8,
                        stride = (uint)source.Stride,
                        scan0 = source.Scan0.Pointer
                    };

                    LCMSStructs.BitmapData destinationBitmapData = new LCMSStructs.BitmapData
                    {
                        width = (uint)destination.PixelWidth,
                        height = (uint)destination.PixelHeight,
                        format = BitmapFormatFromWICPixelFormat(destination.Format),
                        stride = (uint)destination.BackBufferStride,
                        scan0 = destination.BackBuffer
                    };

                    LCMSEnums.TransformFlags flags = LCMSEnums.TransformFlags.None;
                    
                    if (LCMSHelper.UseBlackPointCompensation(blackPointCompensation, intent))
                    {
                        flags |= LCMSEnums.TransformFlags.BlackPointCompensation;
                    }

                    LCMSEnums.ConvertProfileStatus status = LCMSHelper.ConvertToProfile(
                        inputProfile,
                        outputProfile,
                        intent,
                        flags,
                        ref sourceBitmapData,
                        ref destinationBitmapData
                        );

                    if (status != LCMSEnums.ConvertProfileStatus.Ok)
                    {
                        switch (status)
                        {
                            case LCMSEnums.ConvertProfileStatus.InvalidParameter:
                                throw new LCMSException(Resources.ConvertProfileInvalidParameter);
                            case LCMSEnums.ConvertProfileStatus.DifferentImageDimensions:
                                throw new LCMSException(Resources.DifferentImageDimensions);
                            case LCMSEnums.ConvertProfileStatus.CreateTransformFailed:
                                throw new LCMSException(Resources.CreateTransformError);
                            default:
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Changes the rendering intent of the color profile to the specified <see cref="RenderingIntent" /> and saves the profile to a new file.
        /// </summary>
        /// <param name="profilePath">The path of the color profile.</param>
        /// <param name="renderingIntent">The rendering intent to use for the color profile.</param>
        /// <param name="newProfileFileName">The path of the color profile with the new rendering intent.</param>
        /// <returns><c>true</c> if the rendering intent was changed; otherwise, <c>false</c>.</returns>
        /// <exception cref="LCMSException">
        /// The color profile could not be opened.
        /// -or-
        /// An error occurred saving the profile with the new rendering intent.
        /// </exception>
        internal static bool ChangeProfileRenderingIntent(string profilePath, RenderingIntent renderingIntent, out string newProfileFileName)
        {
            newProfileFileName = null;
            bool renderingIntentChanged = false;

            using (LCMSProfileHandle profile = LCMSHelper.OpenColorProfile(profilePath))
            {
                if (profile.IsInvalid)
                {
                    throw new LCMSException(Resources.OpenDestiniationProfileError);
                }

                RenderingIntent profileRenderingIntent = LCMSHelper.GetProfileRenderingIntent(profile);

                if (profileRenderingIntent != renderingIntent)
                {
                    LCMSHelper.SetProfileRenderingIntent(profile, renderingIntent);

                    newProfileFileName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

                    if (!LCMSHelper.SaveColorProfile(profile, newProfileFileName))
                    {
                        throw new LCMSException(Resources.ChangeRenderingIntentError);
                    }

                    renderingIntentChanged = true;
                }
            }

            return renderingIntentChanged;
        }

    }
}
