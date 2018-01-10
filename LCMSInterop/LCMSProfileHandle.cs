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

using Microsoft.Win32.SafeHandles;

namespace SoftProofing.LCMSInterop
{
    internal abstract class LCMSProfileHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        protected LCMSProfileHandle(bool ownsHandle) : base(ownsHandle)
        {
        }
    }

    internal sealed class LCMSProfileHandleX86 : LCMSProfileHandle
    {
        private LCMSProfileHandleX86() : base(true)
        {
        }

        protected override bool ReleaseHandle()
        {
            return LCMS_86.CloseColorProfile(this.handle);
        }
    }

    internal sealed class LCMSProfileHandleX64 : LCMSProfileHandle
    {
        private LCMSProfileHandleX64() : base(true)
        {
        }

        protected override bool ReleaseHandle()
        {
            return LCMS_64.CloseColorProfile(this.handle);
        }
    }
}
