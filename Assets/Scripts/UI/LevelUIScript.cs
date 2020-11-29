using System;
using System.Collections;
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
    public GameObject patientTitle;
    public MeetingMenuObjects meetingMenuObjects;

    public class MeetingMenuObjects
    {
        public GameObject canvas;
        public GameObject player;
        public GameObject playerList;
        public TMP_Text countdownText;
    }

    public IEnumerator ChangePatientView()
    {
        patientView.SetActive(true);
        patientTitle.SetActive(true);
        yield return new WaitForSeconds(3);
        patientTitle.SetActive(false);
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