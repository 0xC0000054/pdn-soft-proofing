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

using Microsoft.Win32.SafeHandles;

namespace SoftProofing
{
    /// <summary>
    /// Represents a handle to a device context
    /// </summary>
    internal sealed class SafeDCHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafeDCHandle() : base(true)
        {
        }

        protected override bool ReleaseHandle()
        {
            return SafeNativeMethods.DeleteDC(this.handle);
        }
    }
}
