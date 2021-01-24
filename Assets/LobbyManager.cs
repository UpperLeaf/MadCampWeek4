using Photon.Pun;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject _currentPlayerUI;

    [SerializeField]
    private GameObject _characterSelectUI;

    [SerializeField]
    private GameObject _gameStartButton;

    [SerializeField]
    private GameObject rifleMan;

    [SerializeField]
    private GameObject bombMan;

    [SerializeField]
    private int startPlayers;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _gameStartButton.SetActive(true);
        }
        else
        {
            _gameStartButton.SetActive(false);
        }
        _currentPlayerUI.GetComponent<TMP_Text>().text = "Current Player : " + PhotonNetwork.CurrentRoom.PlayerCount;
    }

    private void Update()
    {
        _currentPlayerUI.GetComponent<TMP_Text>().text = "Current Player : " + PhotonNetwork.CurrentRoom.PlayerCount;
    }

    public void RiflemanClick()
    {
        PhotonNetwork.Instantiate(rifleMan.name, Vector3.zero, Quaternion.identity, 0);
        TurnoffCharacterSelectUI();
    }

    public void BombmanClick()
    {
        PhotonNetwork.Instantiate(bombMan.name, Vector3.zero, Quaternion.identity, 0);
        TurnoffCharacterSelectUI();
    }

    public void GameStart()
    {
        Debug.Log("Start Player : " + startPlayers);
        Debug.Log("Current Player : " + PhotonNetwork.CurrentRoom.PlayerCount);
        if (PhotonNetwork.CurrentRoom.PlayerCount >= startPlayers)
        {
            _gameStartButton.SetActive(false);
            PhotonNetwork.LoadLevel("Game");
        }
    }

    private void TurnoffCharacterSelectUI()
    {
        _characterSelectUI.SetActive(false);
    }
}
