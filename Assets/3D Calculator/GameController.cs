using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            modelManager = FindObjectOfType<SASLModelManager>();
            if (modelManager)
                modelManager.gc = this;
            else
                SetActiveSASLUI(false);
            
            shoulderFlexionSlider.Init(-90, 180, 180);
            anteriorHipFlexionSlider.Init(20, 180, 180);
            lateralHipFlexionSlider.Init(0, 90, 0);
            }
        
        private void Update()
        {
            if (sequenceCreator)
            {
                startSequenceButton.interactable = sequenceCreator.SequenceInProgress == false;
                sequenceMoveLabel.gameObject.SetActive(sequenceCreator.SequenceInProgress);
            }
            else
            {
                startSequenceButton.gameObject.SetActive(false);
                sequenceMoveLabel.gameObject.SetActive(false);
            }
        }

        public void OnInputSliderChanged()
        {
            cameraNeedsRefocus = true;
        }

        public GameObject SpawnGuy()
        {
             modelManager = Instantiate(modelManagerPrefab);
             modelManager.gc = this;
             sequenceCreator = modelManager.GetComponentInChildren<SequenceCreator>();
             
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
            if(SceneManager.GetActiveScene().name != "Calculator3D_Standalone")
                despawnGuyButton.gameObject.SetActive(value);
        }
        
        public void OnClick_StartSequence()
        {
            if(sequenceCreator)
                sequenceCreator.StartSequence();
        }

        public void OnClickExit()
        {
            Application.Quit();
        }
    }
}