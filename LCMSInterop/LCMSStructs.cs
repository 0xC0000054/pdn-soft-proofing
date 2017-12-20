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

namespace SoftProofing.LCMSInterop
{
    internal static class LCMSStructs
    {
        internal struct BitmapData
        {
            public uint width;
            public uint height;
            public LCMSEnums.BitmapFormat format;
            public uint stride;
            public IntPtr scan0;
        }
    }
}
