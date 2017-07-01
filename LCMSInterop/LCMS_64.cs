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
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;

namespace SoftProofing.LCMSInterop
{
    [System.Security.SuppressUnmanagedCodeSecurity]
    internal static class LCMS_64
    {
        private const string DllName = "SoftProofingLCMS_x64.dll";

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        internal static unsafe extern LCMSProfileHandleX64 OpenColorProfileFromFile([MarshalAs(UnmanagedType.LPWStr)] string fileName);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SaveColorProfileToFile(LCMSProfileHandle hProfile, [MarshalAs(UnmanagedType.LPWStr)] string fileName);

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseColorProfile(IntPtr hProfile);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern uint GetColorProfileInfoSize(LCMSProfileHandle hProfile, LCMSEnums.ProfileInfoType infoType);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        internal static extern uint GetColorProfileInfo(
            LCMSProfileHandle hProfile,
            LCMSEnums.ProfileInfoType infoType,
            [Out()] StringBuilder buffer,
            uint bufferSize
            );

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern ProfileColorSpace GetProfileColorSpace(LCMSProfileHandle hProfile);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern RenderingIntent GetProfileRenderingIntent(LCMSProfileHandle hProfile);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern void SetProfileRenderingIntent(LCMSProfileHandle hProfile, RenderingIntent intent);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern void SetGamutWarningColor(byte red, byte green, byte blue);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern LCMSTransformHandleX64 CreateProofingTransformBGRA8(
            LCMSProfileHandle input,
            LCMSProfileHandle display,
            RenderingIntent displayIntent,
            LCMSProfileHandle proofing,
            RenderingIntent proofingIntent,
            LCMSEnums.TransformFlags flags
            );

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        internal static unsafe extern void ApplyProofingTransform(
            LCMSTransformHandle hTransform, 
            [In()] ref LCMSStructs.BitmapData source,
            [In()] ref LCMSStructs.BitmapData dest,
            Rectangle* rois,
            int length
            );

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern void DeleteTransform(IntPtr hTransform);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern LCMSEnums.ConvertProfileStatus ConvertToProfile(
            LCMSProfileHandle inputProfile,
            LCMSProfileHandle outputProfile,
            RenderingIntent renderingIntent,
            LCMSEnums.TransformFlags flags,
            [In()] ref LCMSStructs.BitmapData input,
            [In()] ref LCMSStructs.BitmapData output
            );
    }
}
