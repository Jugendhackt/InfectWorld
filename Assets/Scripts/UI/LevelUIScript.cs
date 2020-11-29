using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    }

    public void ChangePatientView()
    {
        patientView.SetActive(true);
    }

    public void TogglePause()
    {
        var current = currentPlayer.movement;
        currentPlayer.movement = !current;
        pauseCanvas.SetActive(current);
        Cursor.visible = current;
        Cursor.lockState = current ? CursorLockMode.None: CursorLockMode.Locked;
    }

    public void ChangeInfectedTime(float timePercent)
    {
        infectedProgress.fillAmount = timePercent;
        infectedProgressText.text = $"{Math.Round(timePercent * 100)}% infected";
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
    }
}