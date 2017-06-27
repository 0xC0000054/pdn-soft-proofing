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

using PaintDotNet;
using System;
using System.Reflection;

namespace SoftProofing
{
    public sealed class PluginSupportInfo : IPluginSupportInfo
    {
        public string Author
        {
            get
            {
                return "null54";
            }
        }

        public string Copyright
        {
            get
            {
                return ((AssemblyCopyrightAttribute)typeof(SoftProofingEffect).Assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright;
            }
        }

        public string DisplayName
        {
            get
            {
                return SoftProofingEffect.StaticName;
            }
        }

        public Version Version
        {
            get
            {
                return typeof(SoftProofingEffect).Assembly.GetName().Version;
            }
        }

        public Uri WebsiteUri
        {
            get
            {
                return new Uri("http://www.getpaint.net/redirect/plugins.html");
            }
        }
    }
}
