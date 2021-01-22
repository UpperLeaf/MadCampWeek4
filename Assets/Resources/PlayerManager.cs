using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks, IPlayer, IPunObservable
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

        if (Health <= 0 )
        {
            gameObject.SetActive(false);

            if (photonView.IsMine)
            {
                FindObjectOfType<Camera>().backgroundColor = Color.gray;

                Renderer[] renderers = FindObjectsOfType<Renderer>();

                foreach (Renderer renderer in renderers)
                {
                    renderer.material.SetColor("_Color", Color.gray);
                }
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.Health);
        }
        else
        {
            this.Health = (float)stream.ReceiveNext();
        }
    }
}
