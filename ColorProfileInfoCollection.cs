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
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SoftProofing
{
    [Serializable]
    public sealed class ColorProfileInfoCollection : Collection<ColorProfileInfo>
    {
        internal ColorProfileInfoCollection() : base(new List<ColorProfileInfo>())
        {
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the <see cref="ColorProfileInfoCollection"/>.
        /// </summary>
        /// <param name="collection">The collection if items to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is null.</exception>
        internal void AddRange(IEnumerable<ColorProfileInfo> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            ((List<ColorProfileInfo>)this.Items).AddRange(collection);
        }
    }
}
