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

using PaintDotNet;
using PaintDotNet.Effects;
using System.Drawing;

namespace SoftProofing
{
    [PluginSupportInfo(typeof(PluginSupportInfo))]
    public sealed class SoftProofingEffect : Effect
    {
        private ColorManagement colorManagement;
        private bool renderProofingTransform;

        public static string StaticName
        {
            get
            {
                return "Soft Proofing";
            }
        }

        public static Bitmap StaticImage
        {
            get
            {
                return new Bitmap(typeof(SoftProofingEffect), "SoftProofIcon.png");
            }
        }

        public static string StaticSubMenuName
        {
            get
            {
                return "Tools";
            }
        }

        public SoftProofingEffect() : base(StaticName, StaticImage, StaticSubMenuName, EffectFlags.Configurable)
        {
            this.colorManagement = null;
            this.renderProofingTransform = false;
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing && this.colorManagement != null)
            {
                this.colorManagement.Dispose();
                this.colorManagement = null;
            }

            base.OnDispose(disposing);
        }

        public override EffectConfigDialog CreateConfigDialog()
        {
            return new SoftProofingConfigDialog();
        }

        protected override void OnSetRenderInfo(EffectConfigToken parameters, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            SoftProofingConfigToken token = (SoftProofingConfigToken)parameters;
            if (this.colorManagement == null)
            {
                this.colorManagement = new ColorManagement();
            }

            this.colorManagement.CreateProofingTransform(token);

            this.renderProofingTransform = this.colorManagement.ProofingTransformIsValid;

            base.OnSetRenderInfo(parameters, dstArgs, srcArgs);
        }

        public override void Render(EffectConfigToken parameters, RenderArgs dstArgs, RenderArgs srcArgs, Rectangle[] rois, int startIndex, int length)
        {
            if (this.renderProofingTransform)
            {
                this.colorManagement.ApplyProofingTransformBGRA8(srcArgs.Surface, dstArgs.Surface, rois, startIndex, length);
            }
            else
            {
                dstArgs.Surface.CopySurface(srcArgs.Surface, rois, startIndex, length);
            }
        }
    }
}
