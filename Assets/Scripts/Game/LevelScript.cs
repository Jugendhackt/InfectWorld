using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public LevelUIScript levelUIScript;
    public int range = 4;
    private void Awake()
    {
        Debug.LogFormat("PhotonNetwork : Loading Level");
        Instantiate(levelUIScript.gameObject);
        
        var position = transform.position;
        var x = Random.Range(position.x - range, position.x + range);
        var y = Random.Range(position.y - range, position.y + range);
        var z = Random.Range(position.z - range, position.z + range);
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(x, y, z), Quaternion.identity, 0);
        
        var players = PhotonNetwork.PlayerList;
    }
}