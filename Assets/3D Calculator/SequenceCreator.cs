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
                if (model.gc.sequenceMoveLabel)
                    model.gc.sequenceMoveLabel.text = sequenceElement.name;
                //create a dummy sequence element to lerp from
                SequenceElement start =
                    new SequenceElement{shoulderFlexionAngle = model.gc.shoulderFlexionSlider.slider.value,
                                        anteriorHipFlexionAngle = model.gc.anteriorHipFlexionSlider.slider.value,
                                        lateralHipFlexionAngle = model.gc.lateralHipFlexionSlider.slider.value
                    };
                float transitionTimeElapsed = 0;
                float holdDurationElapsed = 0;
                
                
                while(transitionTimeElapsed < sequenceElement.transitionTime)
                {
                    InterpToPose(start, sequenceElement, transitionTimeElapsed / sequenceElement.transitionTime);
                    transitionTimeElapsed += Time.deltaTime;
                    yield return null;
                }

                model.gc.shoulderFlexionSlider.slider.value = sequenceElement.shoulderFlexionAngle;
                model.gc.anteriorHipFlexionSlider.slider.value = sequenceElement.anteriorHipFlexionAngle;
                model.gc.lateralHipFlexionSlider.slider.value = sequenceElement.lateralHipFlexionAngle;

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
            model.gc.shoulderFlexionSlider.slider.value = Interpolation.Interpolate(start.shoulderFlexionAngle, end.shoulderFlexionAngle, t, InterpolationType.EaseInOutSine);
            model.gc.anteriorHipFlexionSlider.slider.value = Interpolation.Interpolate(start.anteriorHipFlexionAngle, end.anteriorHipFlexionAngle, t, InterpolationType.EaseInOutSine);
            model.gc.lateralHipFlexionSlider.slider.value = Interpolation.Interpolate(start.lateralHipFlexionAngle, end.lateralHipFlexionAngle, t, InterpolationType.EaseInOutSine);
            if (model.gc.sequenceMoveLabel)
            {
                Color c = model.gc.sequenceMoveLabel.color;
                model.gc.sequenceMoveLabel.color = new Color(c.r,c.g,c.b, 
                    Interpolation.Interpolate(0, 1, t, InterpolationType.EaseInOutSine));
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