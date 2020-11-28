using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class NetworkingLauncher : MonoBehaviourPunCallbacks
{
    private const string GameVersion = "1";

    public MainMenuObjects mainMenuObjects;

    [Serializable]
    public class MainMenuObjects
    {
        public TMP_InputField nameField;
        public TMP_InputField codeField;
        public GameObject canvas;
    }

    public RoomMenuObjects roomMenuObjects;
    [Serializable]
    public class RoomMenuObjects
    {
        public TMP_Text textField;
        public GameObject canvas;
        public TMP_Text codeField;
    }

    public GameObject loadingCanvas;

    // Store the PlayerPref Key to avoid typos
    private const string PlayerNamePrefKey = "PlayerName";
    
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(PlayerNamePrefKey))
        {
            mainMenuObjects.nameField.text = PlayerPrefs.GetString(PlayerNamePrefKey);
            UpdateNickname();
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Connect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateNickname()
    {
        var nick = mainMenuObjects.nameField.text;
        PhotonNetwork.NickName = nick;
        PlayerPrefs.SetString(PlayerNamePrefKey, nick);
    }

    public void Join()
    {
        var code = mainMenuObjects.codeField.text;
        if (string.IsNullOrEmpty(code))
            return;
        PhotonNetwork.JoinRoom(code);
    }
    private static Random _random = new Random();
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }

    public void Create()
    {
        var code = mainMenuObjects.codeField.text;
        if (string.IsNullOrEmpty(code))
            code = RandomString(5);
        PhotonNetwork.CreateRoom(code, new RoomOptions
        {
            IsVisible = false,
            MaxPlayers = 10
        });
    }

    public override void OnJoinedRoom()
    {
        mainMenuObjects.canvas.SetActive(false);
        roomMenuObjects.canvas.SetActive(true);
        roomMenuObjects.codeField.text = PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // not seen if you're the player connecting
        UpdatePlayerList();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        StartCoroutine(Reconnect());
    }

    private void UpdatePlayerList()
    {
        var value = "";
        foreach (var pair in PhotonNetwork.CurrentRoom.Players)
        {
            value += pair.Value.NickName;
            if (pair.Value.IsMasterClient)
                value += " (Master)";
            if (pair.Value.IsLocal)
                value += " (You)";
            value += "\r\n";
        }
        roomMenuObjects.textField.text = value;
    }

    private IEnumerator Reconnect()
    {
        yield return new WaitForFixedUpdate();
        Connect();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        roomMenuObjects.canvas.SetActive(false);
        mainMenuObjects.canvas.SetActive(true);
    }

    public void CloseGame()
    {
        PhotonNetwork.Disconnect();
        Application.Quit();
    }

    public void Connect()
    {
        Debug.Log("Connecting to master...");
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = GameVersion;
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master!");
    }

    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        PhotonNetwork.LoadLevel("Level");
        loadingCanvas.SetActive(true);
        roomMenuObjects.canvas.SetActive(false);
    }

    public override void OnLeftRoom()
    {
        SceneManager.UnloadSceneAsync("Level");
    }
}