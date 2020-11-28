using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIScript : MonoBehaviour
{
        public GameObject patientView;
        public Image infectedProgress;
        public TMP_Text infectedProgressText;
        
        public void ChangePatientView()
        { 
                patientView.SetActive(true);
        }

        public void ChangeInfectedTime(float timePercent)
        {
                infectedProgress.fillAmount = timePercent;
                infectedProgressText.text = $"{Math.Round(timePercent * 100)}% infected";
        }
}