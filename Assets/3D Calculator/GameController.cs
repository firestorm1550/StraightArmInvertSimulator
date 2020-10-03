using System;
using UnityEngine;
using UnityEngine.UI;

namespace _3D_Calculator
{
    public class GameController : MonoBehaviour
    {
        public SASLModelManager modelManager;
        public SequenceCreator sequenceCreator;


        public Button startSequenceButton;


        private void Update()
        {
            startSequenceButton.interactable = sequenceCreator.SequenceInProgress == false;
            sequenceCreator.moveLabel.gameObject.SetActive(sequenceCreator.SequenceInProgress);
        }
    }
}