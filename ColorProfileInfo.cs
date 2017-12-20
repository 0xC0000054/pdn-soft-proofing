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
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SoftProofing
{
    [Serializable]
    public sealed class ColorProfileInfo : IEquatable<ColorProfileInfo>, IXmlSerializable
    {
        private string path;
        private string description;
        private ProfileColorSpace colorSpace;

        internal const string XMLArrayItemName = "Profile";

        public string Path
        {
            get
            {
                return this.path;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
        }

        public ProfileColorSpace ColorSpace
        {
            get
            {
                return this.colorSpace;
            }
        }

        // Required for XML serialization.
        private ColorProfileInfo()
        {
        }

        public ColorProfileInfo(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            this.path = fileName;
            ColorProfileHelper.GetProfileInfo(fileName, out this.description, out this.colorSpace);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            ColorProfileInfo info = obj as ColorProfileInfo;
            if (info != null)
            {
                return Equals(info);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hash = 31;
            unchecked
            {
                hash = (hash * 127) + (this.path != null ? this.path.GetHashCode() : 0);
                hash = (hash * 127) + (this.description != null ? this.description.GetHashCode() : 0);
                hash = (hash * 127) + this.colorSpace.GetHashCode();
            }
            return hash;
        }

        public bool Equals(ColorProfileInfo other)
        {
            if (other == null)
            {
                return false;
            }

            return (this.path == other.path && this.description == other.description && this.colorSpace == other.colorSpace);
        }

        public static bool operator ==(ColorProfileInfo c1, ColorProfileInfo c2)
        {
            if (((object)c1) == null || ((object)c2) == null)
            {
                return object.Equals(c1, c2);
            }

            return c1.Equals(c2);
        }

        public static bool operator !=(ColorProfileInfo c1, ColorProfileInfo c2)
        {
            return !(c1 == c2);
        }

        public override string ToString()
        {
            return this.description;
        }

        #region IXmlSerializable
        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader.MoveToContent() == XmlNodeType.Element)
            {
                this.path = reader.GetAttribute("Path");
                this.description = reader.GetAttribute("Description");
                this.colorSpace = (ProfileColorSpace)Enum.Parse(typeof(ProfileColorSpace), reader.GetAttribute("ColorSpace"));

                // If the item is part of an array, advance to the next element.
                if (reader.LocalName == XMLArrayItemName)
                {
                    reader.Read();
                }
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Path", this.path);
            writer.WriteAttributeString("Description", this.description);
            writer.WriteAttributeString("ColorSpace", this.colorSpace.ToString("G"));
        }
        #endregion
    }
}
