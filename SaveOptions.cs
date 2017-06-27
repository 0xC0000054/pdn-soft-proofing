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

namespace SoftProofing
{
    internal class SaveOptions
    {
        public double HorizontalResolution
        {
            get;
            private set;
        }

        public double VerticalResolution
        {
            get;
            private set;
        }

        public SaveOptions(double horizontalResolution, double verticalResolution)
        {
            this.HorizontalResolution = horizontalResolution;
            this.VerticalResolution = verticalResolution;
        }
    }

    internal sealed class JPEGSaveOptions : SaveOptions
    {
        public int Quality
        {
            get;
            private set;
        }

        public JPEGSaveOptions(int quality, double horizontalResolution, double verticalResolution) : base(horizontalResolution, verticalResolution)
        {
            this.Quality = quality;
        }
    }

    internal sealed class TIFFSaveOptions : SaveOptions
    {
        public bool LZWCompression
        {
            get;
            private set;
        }

        public TIFFSaveOptions(bool lzwCompression, double horizontalResolution, double verticalResolution) : base(horizontalResolution, verticalResolution)
        {
            this.LZWCompression = lzwCompression;
        }
    }
}
