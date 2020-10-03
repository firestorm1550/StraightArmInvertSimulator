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
        public bool SequenceInProgress { get; private set; }
        public TextMeshProUGUI moveLabel;
        private SASLModelManager model;

        public void StartSequence()
        {
            
            model = GetComponent<SASLModelManager>();
            
            List<SequenceElement> sequence = new List<SequenceElement>();
            
            sequence.Add(SequenceElements.DeadHang(0,.5f));
            sequence.AddRange(SequenceElements.SASLInvert());
            sequence.Add(SequenceElements.SkinTheCat());
            sequence.Add(SequenceElements.BackPlanche(30));
            sequence.Add(SequenceElements.BackPlanche(0));
            sequence.Add(SequenceElements.Inverted());
            sequence.Add(SequenceElements.FrontPlanche(30));
            sequence.Add(SequenceElements.FrontPlanche(0));
            sequence.Add(SequenceElements.DeadHang(2, 1));


            StartCoroutine(ExecuteSequence(sequence));
        }

        private IEnumerator ExecuteSequence(List<SequenceElement> sequenceElements)
        {
            SequenceInProgress = true;
            foreach (SequenceElement sequenceElement in sequenceElements)
            {
                if (moveLabel)
                    moveLabel.text = sequenceElement.name;
                //create a dummy sequence element to lerp from
                SequenceElement start =
                    new SequenceElement{shoulderFlexionAngle = model.shoulderFlexionSlider.slider.value,
                                        anteriorHipFlexionAngle = model.anteriorHipFlexionSlider.slider.value,
                                        lateralHipFlexionAngle = model.lateralHipFlexionSlider.slider.value
                    };
                float transitionTimeElapsed = 0;
                float holdDurationElapsed = 0;
                
                
                while(transitionTimeElapsed < sequenceElement.transitionTime)
                {
                    InterpToPose(start, sequenceElement, transitionTimeElapsed / sequenceElement.transitionTime);
                    transitionTimeElapsed += Time.deltaTime;
                    yield return null;
                }

                model.shoulderFlexionSlider.slider.value = sequenceElement.shoulderFlexionAngle;
                model.anteriorHipFlexionSlider.slider.value = sequenceElement.anteriorHipFlexionAngle;
                model.lateralHipFlexionSlider.slider.value = sequenceElement.lateralHipFlexionAngle;

                while (holdDurationElapsed < sequenceElement.holdDuration)
                {
                    holdDurationElapsed += Time.deltaTime;
                    yield return null;
                }
            }

            SequenceInProgress = false;
        }

        private void InterpToPose(SequenceElement start, SequenceElement end, float t)
        {
            model.shoulderFlexionSlider.slider.value = Interpolation.Interpolate(start.shoulderFlexionAngle, end.shoulderFlexionAngle, t, InterpolationType.EaseInOutSine);
            model.anteriorHipFlexionSlider.slider.value = Interpolation.Interpolate(start.anteriorHipFlexionAngle, end.anteriorHipFlexionAngle, t, InterpolationType.EaseInOutSine);
            model.lateralHipFlexionSlider.slider.value = Interpolation.Interpolate(start.lateralHipFlexionAngle, end.lateralHipFlexionAngle, t, InterpolationType.EaseInOutSine);
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
        public float shoulderFlexionAngle;
        public float anteriorHipFlexionAngle;
        public float lateralHipFlexionAngle;
        public float holdDuration;
        public string name;
    }
}