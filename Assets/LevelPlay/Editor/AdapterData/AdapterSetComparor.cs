using System;
using System.Collections.Generic;

namespace Unity.Services.LevelPlay.Editor.AdapterData
{
    class AdapterSetComparor : IComparer<Adapter>
    {
        public int Compare(Adapter x, Adapter y)
        {
            // Sort in reverse alphabetical order if both x and y are IsRecommended
            if (x != null && x.IsRecommended && y != null && y.IsRecommended)
            {
                return StringComparer.OrdinalIgnoreCase.Compare(y.AdapterName, x.AdapterName);
            }

            // Sort in alphabetical order if both x and y are not IsRecommended
            if (x != null && !x.IsRecommended && y != null && !y.IsRecommended)
            {
                return StringComparer.OrdinalIgnoreCase.Compare(x.AdapterName, y.AdapterName);
            }

            // Place IsRecommended provider before the not IsRecommended provider
            return x != null && x.IsRecommended ? -1 : 1;
        }
    }
}
