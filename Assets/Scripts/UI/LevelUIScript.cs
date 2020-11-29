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
    public NetworkPlayer currentPlayer;
    private InputMaster _input;

    private void Awake()
    {
        _input = new InputMaster();
        print("!!!");
    }

    public void ChangePatientView()
    {
        patientView.SetActive(true);
    }

    public void TogglePause()
    {
        print("test");
        var current = currentPlayer.movement;
        currentPlayer.movement = !current;
        pauseCanvas.SetActive(current);
        Cursor.visible = current;
        Cursor.lockState = current ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void ChangeInfectedTime(float timePercent)
    {
        infectedProgress.fillAmount = timePercent;
        infectedProgressText.text = $"{Math.Round(timePercent * 100)}% infected";
    }
}