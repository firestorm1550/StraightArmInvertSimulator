using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum InterpolationType
{
    Linear,Quadratic,Cubic,Quartic,
    Sqrt,CubeRoot,FourthRoot
}
public static class Interpolation
{

    public static float Interpolate(float start, float end, float portion, InterpolationType type = InterpolationType.Linear)
    {
        portion = Mathf.Clamp01(portion);

        float delta = (end - start);
        
        switch (type)
        {
            case InterpolationType.Linear:
                return Mathf.Lerp(start, end, portion);
            case InterpolationType.Quadratic:
                return start + delta * portion * portion;
            case InterpolationType.Cubic:
                return start + delta * portion * portion * portion;
            case InterpolationType.Quartic:
                return start + delta * portion * portion * portion * portion;


            case InterpolationType.Sqrt:
                return start + delta * Mathf.Sqrt(portion);
            case InterpolationType.CubeRoot:
                return start + delta * Mathf.Pow(portion, 1 / 3f);
            case InterpolationType.FourthRoot:
                return start + delta * Mathf.Pow(portion, 1 / 4f);
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
    
    
    public static Vector3 Interpolate(Vector3 startPoint, Vector3 endPoint, float portion, InterpolationType type = InterpolationType.Linear)
    {
        return new Vector3(
         Interpolate(startPoint.x, endPoint.x, portion, type),
         Interpolate(startPoint.y, endPoint.y, portion, type),
         Interpolate(startPoint.z, endPoint.z, portion, type));
    }

}
