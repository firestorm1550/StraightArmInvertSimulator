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


    //mass percent values from http://robslink.com/SAS/democd79/body_part_weights.htm


    private float totalMass = 80;
    
    float headMassPercent = 8.26f;
    float headLengthMeters = .4f;
    float headCenterOfGravity = .55f;
    
    float armsMassPercent = 11.4f;
    float armsLengthMeters = .66f;
    float armsCenterOfGravity = .4f;

    float torsoMassPercent = 46.84f;
    float torsoLengthMeters = .46f;
    float torsoCenterOfGravity = .4f;

    float legsMassPercent = 33.4f;
    float legsLengthMeters = .9f;
    float legsCenterOfGravity = .4f;



    public float ArmsMassKg => armsMassPercent * totalMass / 100;
    public float HeadMassKg => headMassPercent * totalMass / 100;
    public float TorsoMassKg => torsoMassPercent * totalMass / 100;
    public float LegsMassKg => legsMassPercent * totalMass / 100;
    public float CombinedMass => totalMass;





    [HideInInspector] public float armsElevationAngle;
    [HideInInspector] public float shoulderToTorsoAngleDegrees; //D
    [HideInInspector] public float torsoToLegsAngleDegrees; //B
    public float handsAngle = 0;
    public float A => 180 - shoulderToTorsoAngleDegrees - armsElevationAngle;
    public float C => 180 - (90 - A) - torsoToLegsAngleDegrees;


    public float x1 => torsoLengthMeters * Mathf.Sin(A * Mathf.Deg2Rad);
    public float y1 => torsoLengthMeters * Mathf.Cos(A * Mathf.Deg2Rad);
    public float x2 => legsLengthMeters * Mathf.Cos(C * Mathf.Deg2Rad);
    public float y2 => legsLengthMeters * Mathf.Sin(C * Mathf.Deg2Rad);

    
    
    public Vector2 HeadCG => new Vector2(headLengthMeters * Mathf.Sin(-A * Mathf.Deg2Rad) * headCenterOfGravity,
                                        headLengthMeters * Mathf.Cos(-A * Mathf.Deg2Rad) * headCenterOfGravity);
    public Vector2 ArmsCG => new Vector2(armsLengthMeters * Mathf.Sin(armsElevationAngle) * armsCenterOfGravity,
                                        armsLengthMeters * Mathf.Cos(armsElevationAngle) * armsCenterOfGravity);
    public Vector2 LegsCG => new Vector2(x1 + x2 * legsCenterOfGravity, -y1 + y2 *legsCenterOfGravity);
    public Vector2 TorsoCG => new Vector2(x1 * torsoCenterOfGravity, -y1 * torsoCenterOfGravity);

    public Vector2 CombinedCG =>
        (LegsCG * LegsMassKg + 
         TorsoCG * TorsoMassKg + 
         HeadCG * HeadMassKg + 
         ArmsCG * ArmsMassKg) / totalMass;



    
    public float TorqueOnHips => (LegsCG.x - x1) * LegsMassKg;
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
