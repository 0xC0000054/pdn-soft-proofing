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
using System.Xml.Serialization;

namespace SoftProofing
{
    [Serializable]
    [XmlRoot("SoftProofingSettings")]
    public sealed class XMLSettingsContainer
    {
        public ColorProfileInfo InputProfile
        {
            get;
            set;
        }

        public ColorProfileInfo DisplayProfile
        {
            get;
            set;
        }

        [XmlArray("ProofingProfiles")]
        [XmlArrayItem(ColorProfileInfo.XMLArrayItemName)]
        public ColorProfileInfoCollection ProofingProfiles
        {
            get;
            set;
        }

        public XMLSettingsContainer()
        {
            this.InputProfile = null;
            this.DisplayProfile = null;
            this.ProofingProfiles = null;
        }
    }
}
