using UnityEngine;

namespace Project.Utilities
{
    public static class LayerMaskUtilities
    {
        // Adapted from https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html
        public static bool Contains(this LayerMask layerMask, int layer)
        {
            return layerMask == (layerMask | (1 << layer));
        }
    }
}
