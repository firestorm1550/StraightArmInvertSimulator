using System;
using System.Collections;
using System.Collections.Generic;
using DAS_Unity_Framework.ExtensionMethods;
using UnityEngine;
using UnityEngine.Animations;

public class SASL3DVisualizer : MonoBehaviour
{
    public SASLInvertCalculator data;

    public Transform skeleton;
    public Camera camera;
    public float camDistance = 4;
    public float verticalOffset = 1;

    public Transform rightShoulderJoint;
    public Transform leftShoulderJoint;

    public Transform rightHipJoint;
    public Transform leftHipJoint;

    public Transform rightHand;
    public Transform leftHand;
    
    
    private Vector3 HandAxis => (rightHand.position - leftHand.position).normalized;
    private Vector3 ShoulderAxis => (rightShoulderJoint.position - leftShoulderJoint.position).normalized;
    private Vector3 HipAxis => (rightHipJoint.position - leftHipJoint.position).normalized;
    

    private Vector3 leftHandStartPos;
    private Vector3 leftHandStartUp;
    private Dictionary<Transform, Quaternion> startRotations;
    private void Start()
    {
        leftHandStartPos = leftHand.transform.position;
        leftHandStartUp = leftHand.transform.up;
        
        startRotations = new Dictionary<Transform, Quaternion>();
        startRotations.Add(rightShoulderJoint, rightShoulderJoint.localRotation);
        startRotations.Add(leftShoulderJoint, leftShoulderJoint.localRotation);
        startRotations.Add(rightHipJoint, rightHipJoint.localRotation);
        startRotations.Add(leftHipJoint, leftHipJoint.localRotation);
    }

    // Update is called once per frame
    void Update()
    {
        ApplySASLData();

        transform.rotation = Quaternion.identity;
        transform.RotateAround(transform.position, rightHand.position - leftHand.position, data.shoulderToTorsoAngleDegrees - 180);
        transform.position = leftHandStartPos + (transform.position - leftHand.position);
        


        //This works but is jittery

        // transform.Reset();
        // skeleton.transform.parent = null;
        //
        //
        // transform.position = leftHand.position;
        // skeleton.transform.parent = transform;
        //
        // transform.RotateAround(transform.position, rightHand.position - leftHand.position, data.shoulderToTorsoAngleDegrees - 180);



        // camera.transform.Reset();
        //
        // camera.transform.position = leftHand.position;
        // camera.transform.position += camDistance * Vector3.back;
        //
        // camera.transform.LookAt(leftHand.position);
        // camera.transform.Rotate(0,0, data.shoulderToTorsoAngleDegrees - 180);
        //
        // camera.transform.position -= camera.transform.up * verticalOffset;
    }

    private void ApplySASLData()
    {
        foreach (Transform joint in startRotations.Keys)
        {
            joint.localRotation = startRotations[joint];
        }

        float theta = 180 - data.shoulderToTorsoAngleDegrees;
        rightShoulderJoint.RotateAround(rightShoulderJoint.position, ShoulderAxis, theta);
        leftShoulderJoint.RotateAround(rightShoulderJoint.position, ShoulderAxis, theta);
        
        float alpha = 180 - data.torsoToLegsAngleDegrees;
        rightHipJoint.RotateAround(rightHipJoint.position, HipAxis, alpha);
        leftHipJoint.RotateAround(rightHipJoint.position, HipAxis, alpha);
    }
}
