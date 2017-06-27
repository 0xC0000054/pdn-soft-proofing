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
using SoftProofing.LCMSInterop;
using System.Drawing;
using PaintDotNet;

namespace SoftProofing
{
    internal sealed class ColorManagement : IDisposable
    {
        private LCMSProfileHandle inputProfile;
        private LCMSProfileHandle displayProfile;
        private LCMSProfileHandle proofingProfile;
        private LCMSTransformHandle proofingTransform;
        private bool proofingTransformIsValid;
        private bool disposed;

        private string inputProfilePath;
        private string displayProfilePath;
        private string proofingProfilePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorManagement"/> class.
        /// </summary>
        public ColorManagement()
        {
            this.inputProfile = null;
            this.displayProfile = null;
            this.proofingProfile = null;
            this.proofingTransform = null;
            this.inputProfilePath = null;
            this.displayProfilePath = null;
            this.proofingProfilePath = null;
            this.proofingTransformIsValid = false;
            this.disposed = false;
        }

        /// <summary>
        /// Gets a value indicating whether the proofing transform is valid.
        /// </summary>
        /// <value>
        /// <c>true</c> if the proofing transform is valid; otherwise, <c>false</c>.
        /// </value>
        public bool ProofingTransformIsValid
        {
            get
            {
                return this.proofingTransformIsValid;
            }
        }

        private void Reset()
        {
            if (this.inputProfile != null)
            {
                this.inputProfile.Dispose();
                this.inputProfile = null;
            }

            if (this.displayProfile != null)
            {
                this.displayProfile.Dispose();
                this.displayProfile = null;
            }

            if (this.proofingProfile != null)
            {
                this.proofingProfile.Dispose();
                this.proofingProfile = null;
            }

            if (this.proofingTransform != null)
            {
                this.proofingTransform.Dispose();
                this.proofingTransform = null;
            }
            this.inputProfilePath = null;
            this.displayProfilePath = null;
            this.proofingProfilePath = null;
            this.proofingTransformIsValid = false;
        }

        /// <summary>
        /// Initializes the color profiles.
        /// </summary>
        /// <param name="inputPath">The path of the input profile.</param>
        /// <param name="displayPath">The path of the display profile.</param>
        /// <param name="proofingPath">The path of the proofing profile.</param>
        /// <returns>
        ///     <c>true</c> if the color profiles were successfully initialized; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="inputPath"/> is null.
        /// or
        /// <paramref name="displayPath"/> is null.
        /// or
        /// <paramref name="proofingPath"/> is null.
        /// </exception>
        private bool InitializeColorProfiles(string inputPath, string displayPath, string proofingPath)
        {
            if (inputPath == null)
            {
                throw new ArgumentNullException("inputPath");
            }
            if (displayPath == null)
            {
                throw new ArgumentNullException("displayPath");
            }
            if (proofingPath == null)
            {
                throw new ArgumentNullException("proofingPath");
            }

            // If the profiles have not changed exit early.
            if (inputPath.Equals(this.inputProfilePath, StringComparison.OrdinalIgnoreCase) &&
                displayPath.Equals(this.displayProfilePath, StringComparison.OrdinalIgnoreCase) &&
                proofingPath.Equals(this.proofingProfilePath, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            Reset();
            bool result = false;

            this.inputProfile = LCMSHelper.OpenColorProfile(inputPath);
            this.displayProfile = LCMSHelper.OpenColorProfile(displayPath);
            this.proofingProfile = LCMSHelper.OpenColorProfile(proofingPath);

            if (!this.inputProfile.IsInvalid && !this.displayProfile.IsInvalid && !this.proofingProfile.IsInvalid)
            {
                this.inputProfilePath = inputPath;
                this.displayProfilePath = displayPath;
                this.proofingProfilePath = proofingPath;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Initializes the color transform with the options from the specified <see cref="SoftProofingConfigToken"/>.
        /// </summary>
        /// <param name="token">The token containing the transform options.</param>
        /// <returns>
        ///     <c>true</c> if the proofing transform was successfully initialized; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="token"/> is null.</exception>
        private bool InitializeProofingTransform(SoftProofingConfigToken token)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }

            if (this.proofingTransform != null)
            {
                this.proofingTransform.Dispose();
                this.proofingTransform = null;
            }

            this.proofingTransform = LCMSHelper.CreateProofingTransformBGRA8(
                this.inputProfile,
                this.displayProfile,
                token.DisplayIntent,
                this.proofingProfile,
                token.ProofingIntent,
                token.ShowGamutWarning,
                token.GamutWarningColor,
                token.BlackPointCompensation
                );

            return !this.proofingTransform.IsInvalid;
        }


        /// <summary>
        /// Creates the proofing transform from the specified <see cref="SoftProofingConfigToken"/>.
        /// </summary>
        /// <param name="token">The token containing the settings transform.</param>
        /// <exception cref="ArgumentNullException"><paramref name="token" /> is null.</exception>
        /// <exception cref="ObjectDisposedException">The object has been disposed.</exception>
        public void CreateProofingTransform(SoftProofingConfigToken token)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }

            if (this.disposed)
            {
                throw new ObjectDisposedException("ColorManagement");
            }

            if (token.InputColorProfile != null &&
                token.DisplayColorProfile != null &&
                token.ProofingColorProfiles != null &&
                token.ProofingProfileIndex >= 0)
            {
                if (InitializeColorProfiles(token.InputColorProfile.Path, token.DisplayColorProfile.Path, token.ProofingColorProfiles[token.ProofingProfileIndex].Path))
                {
                    this.proofingTransformIsValid = InitializeProofingTransform(token);
                }
            }
            else
            {
                if (this.inputProfile != null)
                {
                    Reset();
                }
            }
        }

        /// <summary>
        /// Apply the proofing transform to the specified 8 bit-per-channel BGRA image data.
        /// </summary>
        /// <param name="source">The source surface.</param>
        /// <param name="destination">The destination surface.</param>
        /// <param name="rois">The array of rectangles describing the region to be processed.</param>
        /// <param name="startIndex">The start index in the rectangle array.</param>
        /// <param name="length">The number of rectangles to process.</param>
        /// <exception cref="ObjectDisposedException">The object has been disposed.</exception>
        /// <exception cref="InvalidOperationException">The proofing transform is not valid.</exception>
        public void ApplyProofingTransformBGRA8(Surface source, Surface destination, Rectangle[] rois, int startIndex, int length)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("ColorManagement");
            }

            if (!this.proofingTransformIsValid)
            {
                throw new InvalidOperationException("This method must only be called with a valid proofing transform.");
            }

            if (length == 0)
            {
                return;
            }

            LCMSStructs.BitmapData sourceData = new LCMSStructs.BitmapData
            {
                width = (uint)source.Width,
                height = (uint)source.Height,
                format = LCMSEnums.BitmapFormat.Bgra8,
                stride = (uint)source.Stride,
                scan0 = source.Scan0.Pointer
            };

            LCMSStructs.BitmapData destData = new LCMSStructs.BitmapData
            {
                width = (uint)destination.Width,
                height = (uint)destination.Height,
                format = LCMSEnums.BitmapFormat.Bgra8,
                stride = (uint)destination.Stride,
                scan0 = destination.Scan0.Pointer
            };

            unsafe
            {
                fixed (Rectangle* pROIS = &rois[startIndex])
                {
                    if (IntPtr.Size == 8)
                    {
                        LCMS_64.ApplyProofingTransform(this.proofingTransform, ref sourceData, ref destData, pROIS, length);
                    }
                    else
                    {
                        LCMS_86.ApplyProofingTransform(this.proofingTransform, ref sourceData, ref destData, pROIS, length);
                    }
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!this.disposed)
            {
                if (this.inputProfile != null)
                {
                    this.inputProfile.Dispose();
                    this.inputProfile = null;
                }

                if (this.displayProfile != null)
                {
                    this.displayProfile.Dispose();
                    this.displayProfile = null;
                }

                if (this.proofingProfile != null)
                {
                    this.proofingProfile.Dispose();
                    this.proofingProfile = null;
                }

                if (this.proofingTransform != null)
                {
                    this.proofingTransform.Dispose();
                    this.proofingTransform = null;
                }
                this.proofingTransformIsValid = false;
                this.disposed = true;
            }
        }
    }
}
