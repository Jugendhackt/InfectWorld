using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIScript : MonoBehaviour
{
        public GameObject patientView;
        public Image infectedProgress;
        public TMP_Text infectedProgressText;
        public GameObject pauseCanvas;
        private InputMaster _input;
        public NetworkPlayer currentPlayer;

        public void ChangePatientView()
        { 
                patientView.SetActive(true);
        }

        private void Awake()
        {
                _input = new InputMaster();
                print("!!!");
        }

        public void TogglePause()
        {
                print("test");
                currentPlayer.movement = !currentPlayer.movement;
                pauseCanvas.SetActive(currentPlayer.movement);
                Cursor.visible = currentPlayer.movement;
                Cursor.lockState = currentPlayer.movement ? CursorLockMode.Locked : CursorLockMode.None;

        }

        public void ChangeInfectedTime(float timePercent)
        {
                infectedProgress.fillAmount = timePercent;
                infectedProgressText.text = $"{Math.Round(timePercent * 100)}% infected";
        }
}