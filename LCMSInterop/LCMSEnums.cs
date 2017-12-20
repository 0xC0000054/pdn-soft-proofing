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
    internal static class LCMSEnums
    {
        internal enum ProfileInfoType : int
        {
            Description = 0,
            Manufacturer = 1,
            Model = 2,
            Copyright = 3
        }

        internal enum ConvertProfileStatus : int
        {
            Ok = 0,
            InvalidParameter = -1,
            DifferentImageDimensions = -2,
            CreateTransformFailed = -3
        }

        internal enum BitmapFormat : int
        {
            Bgra8 = 0,
            Bgr8,
            Gray8,
            Cmyk8
        }

        [Flags()]
        internal enum TransformFlags : uint
        {
            None = 0,
            /// <summary>
            /// Inhibit 1‐pixel cache.
            /// </summary>
            NoCache = 0x0040,
            /// <summary>
            /// Inhibit optimizations.
            /// </summary>
            NoOptimize = 0x0100,
            /// <summary>
            /// Do not apply the transform.
            /// </summary>
            NullTransform = 0x0200,
            /// <summary>
            /// Warn when colors are out of gamut for the target profile, only used with proofing transforms
            /// </summary>
            GamutCheck = 0x1000,
            /// <summary>
            /// Perform soft proofing on the target profiles, only used with proofing transforms
            /// </summary>
            SoftProofing = 0x4000,
            /// <summary>
            /// Adjust for differences in the darkest colors the input and target profiles support.
            /// </summary>
            BlackPointCompensation = 0x2000,
            /// <summary>
            /// Do not fix scum dot.
            /// </summary>
            NoWhiteOnWhiteFixup = 0x0004,
            /// <summary>
            /// Use more memory to give better accuracy, for linear XYZ.
            /// </summary>
            HighResPreCalc = 0x0400,
            /// <summary>
            /// Use less memory to minimize resources
            /// </summary>
            LowResPreCalc = 0x0800,
            /// <summary>
            /// Force CLUT optimization.
            /// </summary>
            ForceCLUT = 0x0002,
            /// <summary>
            /// Create CLUT post-linearization tables
            /// </summary>
            CLUTPostLinearization = 0x0001,
            /// <summary>
            /// Create CLUT pre-linearization tables
            /// </summary>
            CLUTPreLinearization = 0x0010
        }
    }
}
