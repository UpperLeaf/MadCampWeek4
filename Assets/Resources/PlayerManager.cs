using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    [Tooltip("The Local Player Instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    [SerializeField]
    private GameObject CinemachineCameraPrefab;

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
        GameObject camera = Instantiate(CinemachineCameraPrefab);
        DontDestroyOnLoad(camera);
        
        CinemachineVirtualCamera virtualCamera = camera.GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            if (photonView.IsMine)
            {
                virtualCamera.Follow = gameObject.transform;
                virtualCamera.LookAt = gameObject.transform;
            }
        }
        else
        {
            Debug.LogError("CameraWork Component on PlayerPrefab");
        }
    }
}
