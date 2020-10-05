using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DAS_Unity_Framework.ExtensionMethods;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SASLModelManager : MonoBehaviour
{
    public bool applyArmAngle = true;
    
    public Text shoulderTorqueText;
    public Text hipsTorqueText;
    public LabelledSlider shoulderFlexionSlider;
    public LabelledSlider anteriorHipFlexionSlider;
    public LabelledSlider lateralHipFlexionSlider;

    public MassSystem massSystem;
    
    public Transform rightShoulderJoint;
    public Transform leftShoulderJoint;

    public Transform rightUpLegJoint;
    public Transform leftUpLegJoint;

    public Transform hips;

    public Transform rightHand;
    public Transform leftHand;
    
    private Vector3 HandAxis => (rightHand.position - leftHand.position).normalized;
    private Vector3 ShoulderAxis => (rightShoulderJoint.position - leftShoulderJoint.position).normalized;
    private Vector3 AnteriorHipAxis => (rightUpLegJoint.position - leftUpLegJoint.position).normalized;
    private Vector3 LateralHipAxis => hips.up;
    
    private float TorqueOnShoulders => (massSystem.CenterOfGravity.z - leftShoulderJoint.position.z) * massSystem.TotalMass;

    private float TorqueOnHips => (massSystem.CalculateCoG(massSystem.subsystem).z - leftUpLegJoint.position.z) *
                                  massSystem.subsystem.Sum(m => m.mass);
    
    
    private Vector3 _leftHandStartPos;
    private Dictionary<Transform, Quaternion> startRotations;



    public void InitializeFields(Text shoulderTorque, Text hipsTorque, LabelledSlider shoulderFlexion,
        LabelledSlider anteriorHipFlexion, LabelledSlider lateralHipFlexion)
    {
        shoulderTorqueText = shoulderTorque;
        hipsTorqueText = hipsTorque;

        shoulderFlexionSlider = shoulderFlexion;
        anteriorHipFlexionSlider = anteriorHipFlexion;
        lateralHipFlexionSlider = lateralHipFlexion;
        
        shoulderFlexionSlider.Init(-90,180,180);
        anteriorHipFlexionSlider.Init(20, 180, 180);
        lateralHipFlexionSlider.Init(0, 90, 0);
    }
    

    private void Start()
    {
        _leftHandStartPos = leftHand.transform.position;

        startRotations = new Dictionary<Transform, Quaternion>();
        startRotations.Add(rightShoulderJoint, rightShoulderJoint.localRotation);
        startRotations.Add(leftShoulderJoint, leftShoulderJoint.localRotation);
        startRotations.Add(rightUpLegJoint, rightUpLegJoint.localRotation);
        startRotations.Add(leftUpLegJoint, leftUpLegJoint.localRotation);
    }
    
    void Update()
    {
        transform.rotation = Quaternion.identity;
     
        foreach (Transform joint in startRotations.Keys)
        {
            joint.localRotation = startRotations[joint];
        }

        float A = 180 - shoulderFlexionSlider.slider.value;
        rightShoulderJoint.RotateAround(rightShoulderJoint.position, ShoulderAxis, A);
        leftShoulderJoint.RotateAround(rightShoulderJoint.position, ShoulderAxis, A);

        float alpha = anteriorHipFlexionSlider.slider.value - 180;
        rightUpLegJoint.RotateAround(rightUpLegJoint.position, AnteriorHipAxis, alpha);
        leftUpLegJoint.RotateAround(leftUpLegJoint.position, AnteriorHipAxis, alpha);

        float lateralHipAngle = lateralHipFlexionSlider.slider.value;
        rightUpLegJoint.Rotate(Vector3.up, -lateralHipAngle, Space.Self);
        leftUpLegJoint.Rotate(Vector3.up, lateralHipAngle, Space.Self);
        

        //lock hands position
        transform.RotateAround(transform.position, HandAxis, shoulderFlexionSlider.slider.value - 180);
        transform.position = _leftHandStartPos + (transform.position - leftHand.position);
        
        
        
        Vector3 handsToCoG = massSystem.CenterOfGravity - leftHand.transform.position;
        handsToCoG = new Vector3(0, -1, handsToCoG.z);

        float armAngle = Vector3.SignedAngle(handsToCoG, Vector3.down, HandAxis);


        if (applyArmAngle)
        {
            transform.RotateAround(leftHand.position, HandAxis, 2*armAngle);
            transform.position = _leftHandStartPos + (transform.position - leftHand.position);
            
        }


        shoulderTorqueText.text = "Torque on shoulders:\n" + TorqueOnShoulders.RoundToNearest(.01f) + " KN*M";
        hipsTorqueText.text = "Torque on hips:\n" + TorqueOnHips.RoundToNearest(.01f) + " KN*M";
        
        
    }
}
