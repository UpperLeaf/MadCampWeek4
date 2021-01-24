using Cinemachine;
using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

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

    [SerializeField]
    private int cameraSize = 7;

    private GameObject _uiObject;

    [SerializeField]
    private GameObject bloodScreen;

    private bool isGameStart;


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
                    virtualCamera.m_Lens.Orthographic = true;
                    virtualCamera.m_Lens.OrthographicSize = 7;
                    isGameStart = false;
                }
            }
            else
            {
                Debug.LogError("CameraWork Component on PlayerPrefab");
            }
        }
    }

    public void GameStart()
    {
        if (photonView.IsMine)
        {
            isGameStart = true;
            gameObject.GetComponent<Light2D>().enabled = true;
            bloodScreen = GameObject.Find("BloodScreen");
            foreach (Transform transform in gameObject.transform)
            {
                if (transform.name == "sight")
                {
                    transform.gameObject.GetComponent<Light2D>().enabled = true;
                    break;
                }
            }
            CreatePlayerUI();
        }
    }

    private void CreatePlayerUI()
    {
        if (playerUiPrefab != null)
        {
            _uiObject = Instantiate(playerUiPrefab);
            _uiObject.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
    }

    public void Damaged(float damage)
    {
        if (!isGameStart || !photonView.IsMine)
            return;
        Health -= damage;
        Debug.Log("Damaged!");
        StartCoroutine("ShowBloodScreen");
        if (Health <= 0)
            photonView.RPC("Dead", PhotonTargets.All);       
    }

    [PunRPC]
    public void Dead()
    {
        gameObject.SetActive(false);
        if (photonView.IsMine)
        {
            FindObjectOfType<Camera>().backgroundColor = Color.gray;
            Renderer[] renderers = FindObjectsOfType<Renderer>();
            foreach (Renderer renderer in renderers)
                renderer.material.SetColor("_Color", Color.gray);
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
            Health = (float)stream.ReceiveNext();
        }
    }

    IEnumerator ShowBloodScreen()
    {
        Debug.Log("ShowBlood Screen" + bloodScreen);
        bloodScreen.GetComponent<Image>().color = new Color(1, 0, 0, Random.Range(0.2f, 0.3f));
        yield return new WaitForSeconds(0.1f);
        bloodScreen.GetComponent<Image>().color = Color.clear;
    }

}
