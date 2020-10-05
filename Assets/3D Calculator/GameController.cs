using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _3D_Calculator
{
    public class GameController : MonoBehaviour
    {
        public SASLModelManager modelManagerPrefab;
        

        public Text shoulderTorqueText;
        public Text hipsTorqueText;
        public TextMeshProUGUI sequenceMoveLabel;
        public LabelledSlider shoulderFlexionSlider;
        public LabelledSlider anteriorHipFlexionSlider;
        public LabelledSlider lateralHipFlexionSlider;
        
        [HideInInspector] public SASLModelManager modelManager;
        [HideInInspector] public SequenceCreator sequenceCreator;
        
        public Button startSequenceButton;
        public Button despawnGuyButton;


        private bool cameraNeedsRefocus;
        private float timeSinceUpdateRequested;

        private void Awake()
        {
            SetActiveSASLUI(false);
            // cameraController.Init(modelManager.gameObject);
            // cameraController.Focus(modelManager.gameObject);
            // cameraController.desiredDegreeOffsetY = -14;
        }
        
        private void Update()
        {
            startSequenceButton.interactable = sequenceCreator?.SequenceInProgress == false;
            sequenceCreator?.moveLabel.gameObject.SetActive(sequenceCreator.SequenceInProgress);
        }

        public void OnInputSliderChanged()
        {
            cameraNeedsRefocus = true;
        }

        public GameObject SpawnGuy()
        {
             modelManager = Instantiate(modelManagerPrefab);
             modelManager.InitializeFields(shoulderTorqueText, hipsTorqueText, 
                 shoulderFlexionSlider, anteriorHipFlexionSlider, lateralHipFlexionSlider);
             
             sequenceCreator = modelManager.GetComponent<SequenceCreator>();
             if(sequenceCreator) sequenceCreator.moveLabel = sequenceMoveLabel;

             SetActiveSASLUI(true);
             
             
             return modelManager.gameObject;
        }

        public void DespawnGuy()
        {
            Destroy(modelManager.gameObject);
            SetActiveSASLUI(false);
        }

        private void SetActiveSASLUI(bool value)
        {
            shoulderTorqueText.transform.parent.gameObject.SetActive(value);
            hipsTorqueText.transform.parent.gameObject.SetActive(value);
            sequenceMoveLabel.gameObject.SetActive(value);
            
            shoulderFlexionSlider.gameObject.SetActive(value);
            anteriorHipFlexionSlider.gameObject.SetActive(value);
            lateralHipFlexionSlider.gameObject.SetActive(value);

            startSequenceButton.gameObject.SetActive(value);
            despawnGuyButton.gameObject.SetActive(value);
        }
        
        public void OnClick_StartSequence()
        {
            if(sequenceCreator)
                sequenceCreator.StartSequence();
        }
        
    }
}