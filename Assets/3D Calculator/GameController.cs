using System;
using UnityEngine;
using UnityEngine.UI;

namespace _3D_Calculator
{
    public class GameController : MonoBehaviour
    {
        public SASLModelManager modelManager;
        public SequenceCreator sequenceCreator;
        public MaxCamera cameraController;


        public Button startSequenceButton;


        private bool cameraNeedsRefocus;
        private float timeSinceUpdateRequested;

        private void Awake()
        {
            cameraController.Init(modelManager.gameObject);
            cameraController.Focus(modelManager.gameObject);
            cameraController.desiredDegreeOffsetY = -14;
        }
        
        private void Update()
        {
            startSequenceButton.interactable = sequenceCreator.SequenceInProgress == false;
            sequenceCreator.moveLabel.gameObject.SetActive(sequenceCreator.SequenceInProgress);

            if (cameraNeedsRefocus)
            {
                timeSinceUpdateRequested+= Time.deltaTime;
                if (timeSinceUpdateRequested > 2f)
                {
                    cameraController.Focus(modelManager.gameObject);
                    timeSinceUpdateRequested = 0;
                    cameraNeedsRefocus = false;
                }
            }
        }

        public void OnInputSliderChanged()
        {
            cameraNeedsRefocus = true;
        }
    }
}