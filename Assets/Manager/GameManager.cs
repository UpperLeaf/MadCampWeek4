using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Tooltip("The Prefab to use for representing the player")]
    [SerializeField]
    private GameObject playerPrefab;

    #region Photon Callbacks
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
    #endregion

    #region MonoBehaviour Callbacks

    private void Start()
    {
        if (playerPrefab == null)
            Debug.LogError("Missing Prefab");
        else
        {
            if (PlayerManager.LocalPlayerInstance == null)
            {
                GameObject _player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(-5f, 5f, 0f), Quaternion.identity, 0);
            }
        }
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PlayerManagerGameStart();
    }
    #endregion

    #region Public Methods
    public void LeaveRoom()
    {
        Destroy(GameObject.Find("Leave Button"));
        PhotonNetwork.LeaveRoom();
    }
    #endregion

    #region Private Methods
    private void PlayerManagerGameStart()
    {
        foreach (PhotonView view in PhotonNetwork.PhotonViewCollection)
        {
            PlayerManager playerManager = view.gameObject.GetComponent<PlayerManager>();
            if (playerManager != null)
            {
                playerManager.GameStart();
            }

            if (!view.IsMine)
            {
                Destroy(view.gameObject.GetComponent<Light2D>());
                foreach (Transform transform in view.gameObject.transform)
                {
                    if (transform.name == "sight")
                    {
                        Destroy(transform.gameObject);
                        break;
                    }
                }
            }
        }
    }
    #endregion
}
