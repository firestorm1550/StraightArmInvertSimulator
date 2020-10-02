using System;
using System.Collections;
using System.Collections.Generic;
using DAS_Unity_Framework;
using TMPro;
using UnityEngine;

namespace _3D_Calculator
{
    [RequireComponent(typeof(SASLModelManager))]
    public class SequenceCreator : MonoBehaviour
    {
        public TextMeshProUGUI moveLabel;
        
        private SASLModelManager model;
        private void Start()
        {
            model = GetComponent<SASLModelManager>();
            StartCoroutine(ExecuteSequence(new List<SequenceElement>
            {
                new SequenceElement(0, 180, 180, 1),
                new SequenceElement(1, 180, 70, .25f),
                new SequenceElement(3, 10, 70, .75f,"Straight Arm Straight Leg Invert"),
                new SequenceElement(3f, -75, 70, 1, "Skin the Cat"),
                new SequenceElement(1.5f, -45, 180, 2, "Back Planche"),
                new SequenceElement(1, 3.5f, 180, 1),
                new SequenceElement(3, 55, 180, 2, "Front Planche"),
                new SequenceElement(2, 180, 180, 1),

            }));
        }

        private IEnumerator ExecuteSequence(List<SequenceElement> sequenceElements)
        {
            
            foreach (SequenceElement sequenceElement in sequenceElements)
            {
                if (moveLabel)
                    moveLabel.text = sequenceElement.name;
                //create a dummy sequence element to lerp from
                SequenceElement start =
                    new SequenceElement(0, model.shoulderAngle.slider.value,
                        model.hipAngle.slider.value, 0);
                float transitionTimeElapsed = 0;
                float holdDurationElapsed = 0;
                
                
                while(transitionTimeElapsed < sequenceElement.transitionTime)
                {
                    InterpToPose(start, sequenceElement, transitionTimeElapsed / sequenceElement.transitionTime);
                    transitionTimeElapsed += Time.deltaTime;
                    yield return null;
                }

                model.shoulderAngle.slider.value = sequenceElement.shoulderAngle;
                model.hipAngle.slider.value = sequenceElement.hipsAngle;

                while (holdDurationElapsed < sequenceElement.holdDuration)
                {
                    holdDurationElapsed += Time.deltaTime;
                    yield return null;
                }
            }
        }

        private void InterpToPose(SequenceElement start, SequenceElement end, float t)
        {
            model.shoulderAngle.slider.value = Interpolation.Interpolate(start.shoulderAngle, end.shoulderAngle, t, InterpolationType.EaseInOutSine);
            model.hipAngle.slider.value = Interpolation.Interpolate(start.hipsAngle, end.hipsAngle, t, InterpolationType.EaseInOutSine);
            if (moveLabel)
            {
                Color c = moveLabel.color;
                moveLabel.color = new Color(c.r,c.g,c.b, Interpolation.Interpolate(0, 1, t, InterpolationType.EaseInOutSine));
            }
        }
    }

    public class SequenceElement
    {
        public float transitionTime;//time to reach this pose
        public float shoulderAngle;
        public float hipsAngle;
        public float holdDuration;
        public string name;

        public SequenceElement(float transitionTime, float shoulderAngle, float hipsAngle, float holdDuration, string name = "")
        {
            this.transitionTime = transitionTime;
            this.shoulderAngle = shoulderAngle;
            this.hipsAngle = hipsAngle;
            this.holdDuration = holdDuration;
            this.name = name;
        }
    }
}