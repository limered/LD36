using UnityEngine;

public static class MathExtensions
{
    /// <summary>
    /// 
    /// <para>
    /// Compares two floating point values if they are similar.
    /// </para>
    /// 
    /// </summary>
    /// <param name="v1"/><param name="v2"/>
    public static bool Approx(this float v1, float v2)
    {
        return Mathf.Approximately(v1, v2);
    }
}
