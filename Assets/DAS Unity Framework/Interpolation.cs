using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAS_Unity_Framework
{
    public enum InterpolationType
    {
        Linear,Quadratic,Cubic,Quartic,
        Sqrt,CubeRoot,FourthRoot,
        EaseOutBack, EaseInOutSine
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


                case InterpolationType.EaseOutBack:
                    return start + delta * EaseOutBack(portion);
                case InterpolationType.EaseInOutSine:
                    return start + delta * EaseInOutSine(portion);
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
        
        private static float EaseOutBack(float t)
        {
            float c1 = 1.70158f;
            float c3 = c1 + 1;

            return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
        }

        private static float EaseInOutSine(float x)
        {
            return -(Mathf.Cos(Mathf.PI * x) - 1) / 2;
        }
    }
}