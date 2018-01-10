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

using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace SoftProofing
{
    [SuppressUnmanagedCodeSecurity()]
    internal static class SafeNativeMethods
    {
        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        internal static extern SafeDCHandle CreateDC(
            string pwszDriver,
            string pwszDevice,
            string pszPort,
            IntPtr pdm
            );

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [DllImport("gdi32.dll", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetICMProfile(SafeDCHandle hDC, ref uint bufferSize, [Out()] StringBuilder buffer);

        [DllImport("user32.dll", ExactSpelling = true)]
        internal static extern IntPtr MonitorFromWindow(IntPtr hWnd, uint dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, ref NativeStructs.MONITORINFOEX lpmi);

        [DllImport("mscms.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetStandardColorSpaceProfile(
            string pMachineName,
            uint dwProfileID,
            [Out()] StringBuilder pProfileName,
            ref uint pdwSize
            );

        [DllImport("mscms.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetColorDirectory(string pMachineName, [Out()] StringBuilder pBuffer, ref uint pdwSize);
    }
}
