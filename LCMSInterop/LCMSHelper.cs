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

using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace SoftProofing.LCMSInterop
{
    internal static class LCMSHelper
    {
        internal static LCMSProfileHandle OpenColorProfile(string fileName)
        {
            byte[] buffer = File.ReadAllBytes(fileName);
            return OpenColorProfile(buffer);
        }

        internal static LCMSProfileHandle OpenColorProfile(byte[] buffer)
        {
            unsafe
            {
                fixed (byte* ptr = buffer)
                {
                    if (IntPtr.Size == 8)
                    {
                        return LCMS_64.OpenColorProfileFromMemory((void*)ptr, (uint)buffer.Length);
                    }
                    else
                    {
                        return LCMS_86.OpenColorProfileFromMemory((void*)ptr, (uint)buffer.Length);
                    }
                } 
            }
        }

        internal static bool SaveColorProfileToMemory(LCMSProfileHandle hProfile, IntPtr buffer, ref uint bufferSize)
        {
            if (IntPtr.Size == 8)
            {
                return LCMS_64.SaveColorProfileToMemory(hProfile, buffer, ref bufferSize);
            }
            else
            {
                return LCMS_86.SaveColorProfileToMemory(hProfile, buffer, ref bufferSize);
            } 
        }

        private static void SetGamutWarningColor(Color gamutWarningColor)
        {
            if (IntPtr.Size == 8)
            {
                LCMS_64.SetGamutWarningColor(gamutWarningColor.R, gamutWarningColor.G, gamutWarningColor.B);
            }
            else
            {
                LCMS_86.SetGamutWarningColor(gamutWarningColor.R, gamutWarningColor.G, gamutWarningColor.B);
            }
        }

        internal static bool UseBlackPointCompensation(bool blackPointCompensation, RenderingIntent intent)
        {
            // Black point compensation should not be used when rendering in Absolute Colorimetric according to
            // https://www.adobe.com/content/dam/Adobe/en/devnet/photoshop/sdk/AdobeBPC.pdf

            return (blackPointCompensation && intent != RenderingIntent.AbsoluteColorimetric);
        }

        internal static LCMSTransformHandle CreateProofingTransformBGRA8(
            LCMSProfileHandle input,
            LCMSProfileHandle display,
            RenderingIntent displayIntent,
            LCMSProfileHandle proofing,
            RenderingIntent proofingIntent,
            bool checkGamut,
            Color gamutWarningColor,
            bool blackPointCompensation
            )
        {
            LCMSEnums.TransformFlags flags = LCMSEnums.TransformFlags.None;

            if (checkGamut)
            {
                flags |= LCMSEnums.TransformFlags.GamutCheck;
                SetGamutWarningColor(gamutWarningColor);
            }
            else
            {
                flags |= LCMSEnums.TransformFlags.SoftProofing;
            }
            
            if (UseBlackPointCompensation(blackPointCompensation, proofingIntent))
            {
                flags |= LCMSEnums.TransformFlags.BlackPointCompensation;
            }

            if (IntPtr.Size == 8)
            {
                return LCMS_64.CreateProofingTransformBGRA8(
                    input,
                    display,
                    displayIntent,
                    proofing,
                    proofingIntent,
                    flags
                    );
            }
            else
            {
                return LCMS_86.CreateProofingTransformBGRA8(
                    input,
                    display,
                    displayIntent,
                    proofing,
                    proofingIntent,
                    flags
                    );
            }
        }

        internal static uint GetProfileInfoSize(LCMSProfileHandle hProfile, LCMSEnums.ProfileInfoType infoType)
        {
            if (IntPtr.Size == 8)
            {
                return LCMS_64.GetColorProfileInfoSize(hProfile, infoType);
            }
            else
            {
                return LCMS_86.GetColorProfileInfoSize(hProfile, infoType);
            }
        }

        internal static string GetProfileInfo(LCMSProfileHandle hProfile, LCMSEnums.ProfileInfoType infoType, uint bufferSize)
        {
            if (bufferSize > 0U)
            {
                uint result;
                StringBuilder buffer = new StringBuilder((int)bufferSize);

                if (IntPtr.Size == 8)
                {
                    result = LCMS_64.GetColorProfileInfo(hProfile, infoType, buffer, bufferSize);
                }
                else
                {
                    result = LCMS_86.GetColorProfileInfo(hProfile, infoType, buffer, bufferSize);
                }

                if (result > 0U)
                {
                    return buffer.ToString();
                }
            }

            return string.Empty;
        }

        internal static ProfileColorSpace GetProfileColorSpace(LCMSProfileHandle hProfile)
        {
            if (IntPtr.Size == 8)
            {
                return LCMS_64.GetProfileColorSpace(hProfile);
            }
            else
            {
                return LCMS_86.GetProfileColorSpace(hProfile);
            }
        }

        internal static RenderingIntent GetProfileRenderingIntent(LCMSProfileHandle hProfile)
        {
            if (IntPtr.Size == 8)
            {
                return LCMS_64.GetProfileRenderingIntent(hProfile);
            }
            else
            {
                return LCMS_86.GetProfileRenderingIntent(hProfile);
            }
        }

        internal static void SetProfileRenderingIntent(LCMSProfileHandle hProfile, RenderingIntent newRenderingIntent)
        {
            if (IntPtr.Size == 8)
            {
                LCMS_64.SetProfileRenderingIntent(hProfile, newRenderingIntent);
            }
            else
            {
                LCMS_86.SetProfileRenderingIntent(hProfile, newRenderingIntent);
            }
        }

        internal static LCMSEnums.ConvertProfileStatus ConvertToProfile(
            LCMSProfileHandle inputProfile,
            LCMSProfileHandle outputProfile,
            RenderingIntent renderingIntent,
            LCMSEnums.TransformFlags flags,
            ref LCMSStructs.BitmapData input,
            ref LCMSStructs.BitmapData output
            )
        {
            if (IntPtr.Size == 8)
            {
                return LCMS_64.ConvertToProfile(
                    inputProfile,
                    outputProfile,
                    renderingIntent,
                    flags,
                    ref input,
                    ref output
                    );
            }
            else
            {
                return LCMS_86.ConvertToProfile(
                    inputProfile,
                    outputProfile,
                    renderingIntent,
                    flags,
                    ref input,
                    ref output
                    );
            }
        }
    }
}
