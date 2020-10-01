﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SASLModelManager : MonoBehaviour
{
    public Text shoulderTorqueText;
    public Text hipsTorqueText;
    public LabelledSlider shoulderAngle;
    public LabelledSlider hipAngle;

    public MassSystem massSystem;
    
    public Transform rightShoulderJoint;
    public Transform leftShoulderJoint;

    public Transform rightHipJoint;
    public Transform leftHipJoint;

    public Transform rightHand;
    public Transform leftHand;
    
    private Vector3 HandAxis => (rightHand.position - leftHand.position).normalized;
    private Vector3 ShoulderAxis => (rightShoulderJoint.position - leftShoulderJoint.position).normalized;
    private Vector3 HipAxis => (rightHipJoint.position - leftHipJoint.position).normalized;
    
    private Vector3 _leftHandStartPos;
    private Dictionary<Transform, Quaternion> startRotations;
    
    
    
    private void Awake()
    {
        shoulderAngle.Init(0,180,180);
        hipAngle.Init(20, 180, 180);
    }

    private void Start()
    {
        _leftHandStartPos = leftHand.transform.position;

        startRotations = new Dictionary<Transform, Quaternion>();
        startRotations.Add(rightShoulderJoint, rightShoulderJoint.localRotation);
        startRotations.Add(leftShoulderJoint, leftShoulderJoint.localRotation);
        startRotations.Add(rightHipJoint, rightHipJoint.localRotation);
        startRotations.Add(leftHipJoint, leftHipJoint.localRotation);
    }
    
    void Update()
    {
     
        foreach (Transform joint in startRotations.Keys)
        {
            joint.localRotation = startRotations[joint];
        }

        float A = 180 - shoulderAngle.slider.value;
        rightShoulderJoint.RotateAround(rightShoulderJoint.position, ShoulderAxis, A);
        leftShoulderJoint.RotateAround(rightShoulderJoint.position, ShoulderAxis, A);

        float alpha = hipAngle.slider.value - 180;
        rightHipJoint.RotateAround(rightHipJoint.position, HipAxis, alpha);
        leftHipJoint.RotateAround(rightHipJoint.position, HipAxis, alpha);
        // Vector3 shoulderAxis = Vector3.up;
        // rightShoulderJoint.localRotation = startRotations[rightShoulderJoint] *
        //     Quaternion.AngleAxis(-(180 - shoulderAngle.slider.value), shoulderAxis);
        // leftShoulderJoint.localRotation = startRotations[leftShoulderJoint] *
        //     Quaternion.AngleAxis(180 - shoulderAngle.slider.value, shoulderAxis);
        //
        //
        // Vector3 hipAxis = Vector3.forward;
        // rightHipJoint.localRotation = startRotations[rightHipJoint] *
        //                               Quaternion.AngleAxis(180-hipAngle.slider.value, hipAxis);
        // leftHipJoint.localRotation = startRotations[leftHipJoint] *
        //     Quaternion.AngleAxis(180-hipAngle.slider.value, hipAxis);
        
        
        //Set arms angle
        Vector3 handsToCoG = massSystem.LocalCenterOfGravity - transform.InverseTransformPoint(leftHand.transform.position) ;
        handsToCoG = new Vector3(0,-1, handsToCoG.z);
        
        float armAngle = Vector3.SignedAngle(handsToCoG, Vector3.down, HandAxis);
        Debug.Log(handsToCoG + " to " + Vector3.down + " = " + armAngle);
        
        
        
        //lock hands position
        transform.rotation = Quaternion.identity;
        transform.RotateAround(transform.position, HandAxis, shoulderAngle.slider.value - 180 - armAngle);
        transform.position = _leftHandStartPos + (transform.position - leftHand.position);
        
        
    }
}
