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

namespace SoftProofing.LCMSInterop
{
    internal abstract class LCMSTransformHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        protected LCMSTransformHandle(bool ownsHandle) : base(ownsHandle)
        {
        } 
    }

    internal sealed class LCMSTransformHandleX86 : LCMSTransformHandle
    {
        private LCMSTransformHandleX86() : base(true)
        {
        }

        protected override bool ReleaseHandle()
        {
            LCMS_86.DeleteTransform(this.handle);
            return true;
        }
    }

    internal sealed class LCMSTransformHandleX64 : LCMSTransformHandle
    {
        private LCMSTransformHandleX64() : base(true)
        {
        }

        protected override bool ReleaseHandle()
        {
            LCMS_64.DeleteTransform(this.handle);
            return true;
        }
    }
}
