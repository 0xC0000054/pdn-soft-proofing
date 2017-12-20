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

using PaintDotNet.Effects;
using System.Drawing;

namespace SoftProofing
{
    public sealed class SoftProofingConfigToken : EffectConfigToken
    {
        public ColorProfileInfo InputColorProfile
        {
            get;
            set;
        }

        public ColorProfileInfo DisplayColorProfile
        {
            get;
            set;
        }

        public RenderingIntent DisplayIntent
        {
            get;
            set;
        }

        public ColorProfileInfoCollection ProofingColorProfiles
        {
            get;
            set;
        }

        public int ProofingProfileIndex
        {
            get;
            set;
        }

        public RenderingIntent ProofingIntent
        {
            get;
            set;
        }

        public bool ShowGamutWarning
        {
            get;
            set;
        }

        public Color GamutWarningColor
        {
            get;
            set;
        }

        public bool BlackPointCompensation
        {
            get;
            set;
        }

        public SoftProofingConfigToken(
            ColorProfileInfo inputProfile,
            ColorProfileInfo monitorProfile,
            RenderingIntent displayIntent,
            ColorProfileInfoCollection proofingProfiles,
            int proofingProfileIndex,
            RenderingIntent proofingIntent,
            bool showGamutWarning,
            Color gamutWarningColor,
            bool blackPointCompensation
            )
        {
            this.InputColorProfile = inputProfile;
            this.DisplayColorProfile = monitorProfile;
            this.DisplayIntent = displayIntent;
            this.ProofingColorProfiles = proofingProfiles;
            this.ProofingProfileIndex = proofingProfileIndex;
            this.ProofingIntent = proofingIntent;
            this.ShowGamutWarning = showGamutWarning;
            this.GamutWarningColor = gamutWarningColor;
            this.BlackPointCompensation = blackPointCompensation;
        }

        private SoftProofingConfigToken(SoftProofingConfigToken cloneMe)
        {
            this.InputColorProfile = cloneMe.InputColorProfile;
            this.DisplayColorProfile = cloneMe.DisplayColorProfile;
            this.DisplayIntent = cloneMe.DisplayIntent;
            this.ProofingColorProfiles = cloneMe.ProofingColorProfiles;
            this.ProofingProfileIndex = cloneMe.ProofingProfileIndex;
            this.ProofingIntent = cloneMe.ProofingIntent;
            this.ShowGamutWarning = cloneMe.ShowGamutWarning;
            this.GamutWarningColor = cloneMe.GamutWarningColor;
            this.BlackPointCompensation = cloneMe.BlackPointCompensation;
        }

        public override object Clone()
        {
            return new SoftProofingConfigToken(this);
        }
    }
}
