using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class MassPoint : CoGMarker
{
    public float mass;

    public void Initialize(float totalMass)
    {
        transform.localScale = Mathf.Sqrt(mass) / (totalMass) * Vector3.one;
    }
}
