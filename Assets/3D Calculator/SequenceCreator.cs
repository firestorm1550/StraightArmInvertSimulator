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
            StartCoroutine(ExecuteSequence(new List<SequenceElement>
            { 
                new SequenceElement{transitionTime = 0   , shoulderFlexionAngle =  180, anteriorHipFlexionAngle = 180, lateralHipFlexionAngle =  0, holdDuration =    1},
                new SequenceElement{transitionTime = 1   , shoulderFlexionAngle =  180, anteriorHipFlexionAngle =  70, lateralHipFlexionAngle =  0, holdDuration = .25f},
                new SequenceElement{transitionTime = 3   , shoulderFlexionAngle =   10, anteriorHipFlexionAngle =  70, lateralHipFlexionAngle =  0, holdDuration = .75f, name = "Straight Arm Straight Leg Invert"},
                new SequenceElement{transitionTime = 3f  , shoulderFlexionAngle =  -75, anteriorHipFlexionAngle =  70, lateralHipFlexionAngle =  0, holdDuration =    1, name = "Skin the Cat"},
                new SequenceElement{transitionTime = 1.5f, shoulderFlexionAngle =  -45, anteriorHipFlexionAngle = 180, lateralHipFlexionAngle = 60, holdDuration =    2, name = "Back Planche"},
                new SequenceElement{transitionTime = 1.5f, shoulderFlexionAngle =  -45, anteriorHipFlexionAngle = 180, lateralHipFlexionAngle =  0, holdDuration =    2, name = "Harder Back Planche"},
                new SequenceElement{transitionTime = 1   , shoulderFlexionAngle = 3.5f, anteriorHipFlexionAngle = 180, lateralHipFlexionAngle =  0, holdDuration =    1},
                new SequenceElement{transitionTime = 3   , shoulderFlexionAngle =   55, anteriorHipFlexionAngle = 180, lateralHipFlexionAngle = 60, holdDuration =    2, name = "Front Planche"},
                new SequenceElement{transitionTime = 3   , shoulderFlexionAngle =   55, anteriorHipFlexionAngle = 180, lateralHipFlexionAngle =  0, holdDuration =    2, name = "Harder Front Planche"},
                new SequenceElement{transitionTime = 2   , shoulderFlexionAngle =  180, anteriorHipFlexionAngle = 180, lateralHipFlexionAngle =  0, holdDuration =    1},

            }));
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