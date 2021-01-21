using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks, IPlayer
{
    [Tooltip("The Local Player Instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    [SerializeField]
    private GameObject CinemachineCameraPrefab;

    [Tooltip("The Player's UI GameObject Prefab")]
    [SerializeField]
    private GameObject playerUiPrefab;

    [Tooltip("The Current Health of Our Player")]
    [SerializeField]
    public float Health = 1f;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            PlayerManager.LocalPlayerInstance = gameObject;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            GameObject camera = Instantiate(CinemachineCameraPrefab);
            DontDestroyOnLoad(camera);

            CinemachineVirtualCamera virtualCamera = camera.GetComponent<CinemachineVirtualCamera>();
            if (virtualCamera != null)
            {
                if (photonView.IsMine)
                {
                    virtualCamera.Follow = gameObject.transform;
                    //virtualCamera.LookAt = gameObject.transform;
                }
            }
            else
            {
                Debug.LogError("CameraWork Component on PlayerPrefab");
            }
        }

        if (playerUiPrefab != null)
        {
            GameObject _uiObject = Instantiate(playerUiPrefab);
            _uiObject.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
    }

    public void Damaged(float damage)
    {
        Health -= damage;
    }
}
