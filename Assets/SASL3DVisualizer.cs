using System;
using System.Collections;
using System.Collections.Generic;
using DAS_Unity_Framework.ExtensionMethods;
using UnityEngine;
using UnityEngine.Animations;

public class SASL3DVisualizer : MonoBehaviour
{
    public SASLInvertCalculator data;

    public Transform skeletonRoot;
    
    public Transform rightShoulderJoint;
    public Transform leftShoulderJoint;

    public Transform rightHipJoint;
    public Transform leftHipJoint;

    public Transform rightHand;
    public Transform leftHand;
    
    
    private Dictionary<Transform, Quaternion> startRotations;
    private void Start()
    {
        startRotations = new Dictionary<Transform, Quaternion>();
        startRotations.Add(rightShoulderJoint, rightShoulderJoint.localRotation);
        startRotations.Add(leftShoulderJoint, leftShoulderJoint.localRotation);
        startRotations.Add(rightHipJoint, rightHipJoint.localRotation);
        startRotations.Add(leftHipJoint, leftHipJoint.localRotation);
        startRotations.Add(skeletonRoot, skeletonRoot.localRotation);
    }

    // Update is called once per frame
    void Update()
    {
        ApplySASLData();
    }

    private void ApplySASLData()
    {
        Vector3 shoulderAxis = rightShoulderJoint.position - leftShoulderJoint.position;
        shoulderAxis.Normalize();

        Vector3 hipAxis = rightHipJoint.position - leftHipJoint.position;
        hipAxis.Normalize();

        foreach (Transform joint in startRotations.Keys)
        {
            joint.localRotation = startRotations[joint];
        }
        
        float theta = 180 - data.shoulderToTorsoAngleDegrees;
        rightShoulderJoint.RotateAround(rightShoulderJoint.position, shoulderAxis, theta);
        leftShoulderJoint.RotateAround(rightShoulderJoint.position, shoulderAxis, theta);


        float alpha = 180 - data.torsoToLegsAngleDegrees;
        rightHipJoint.Rotate(hipAxis, alpha);
        leftHipJoint.Rotate(hipAxis, alpha);

    }
}
