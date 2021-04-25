using UnityEngine;

public static class ExtensionMethods
{
    public static bool InBetween(this int number, int min, int max, bool inclusive = true)
    {
        if(inclusive)
            return number >= min && number <= max;
        else
            return number > min && number < max;
    }

    public static bool MatchLayer(this LayerMask layerMask, in int layer)
    {
        return ((1 << layer) & layerMask) != 0;
    }
}
