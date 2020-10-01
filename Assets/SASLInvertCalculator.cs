using System;
using System.Collections;
using System.Collections.Generic;
using DAS_Unity_Framework.ExtensionMethods;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SASLInvertCalculator : MonoBehaviour
{
    public Text shoulderTorqueText;
    public Text hipsTorqueText;
    public LabelledSlider shoulderAngle;
    public LabelledSlider hipAngle;
    public Text variablesText;
    
    public float headAndArmsMassKg = 18;
    public float headAndArmsLengthMeters = .6f;
    public float headAndArmsCenterOfGravity = .3f;

    public float torsoMassKg = 42;
    public float torsoLengthMeters = .6f;
    public float torsoCenterOfGravity = .4f;
 
    public float legsMassKg = 40;
    public float legsLengthMeters = .9f;
    public float legsCenterOfGravity = .4f;
    
    
    [HideInInspector] public float shoulderToTorsoAngleDegrees; //D
    [HideInInspector] public float torsoToLegsAngleDegrees; //B
    public float handsAngle = 0;
    public float A => 180 - shoulderToTorsoAngleDegrees;
    public float C => 180 - (90 - A) - torsoToLegsAngleDegrees;


    public float x1 => torsoLengthMeters * Mathf.Sin(A * Mathf.Deg2Rad);
    public float y1 => torsoLengthMeters * Mathf.Cos(A * Mathf.Deg2Rad);
    public float x2 => legsLengthMeters * Mathf.Cos(C * Mathf.Deg2Rad);
    public float y2 => legsLengthMeters * Mathf.Sin(C * Mathf.Deg2Rad);

    
    public Vector2 LegsCG => new Vector2(x1 + x2 * legsCenterOfGravity, -y1 + y2 *legsCenterOfGravity);
    public Vector2 TorsoCG => new Vector2(x1 * torsoCenterOfGravity, -y1 * torsoCenterOfGravity);
    public Vector2 CombinedCG => (LegsCG * legsMassKg + TorsoCG * torsoMassKg) / (torsoMassKg + legsMassKg);



    public float CombinedMass => legsMassKg + torsoMassKg;

    public float TorqueOnHips => (LegsCG.x - x1) * legsMassKg;
    public float TorqueOnShoulders => CombinedCG.x * CombinedMass;
    
    


    private void Awake()
    {
        shoulderAngle.Init(0,180,180);
        hipAngle.Init(20, 180, 180);
    }

    private void Update()
    {
        shoulderTorqueText.text = "Torque on shoulders:\n" + TorqueOnShoulders.RoundToNearest(.01f) + " KN*M";
        hipsTorqueText.text = "Torque on hips:\n" + TorqueOnHips.RoundToNearest(.01f) + " KN*M";
        
        shoulderToTorsoAngleDegrees = shoulderAngle.slider.value;
        torsoToLegsAngleDegrees = hipAngle.slider.value;

        variablesText.text = "x1: " + x1.RoundToNearest(.1f) + "\ny1: " + y1.RoundToNearest(.1f) + 
                             "\nx2: " + x2.RoundToNearest(.1f) + "\ny2: " + y2.RoundToNearest(.1f);
    }
}
