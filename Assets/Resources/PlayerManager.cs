﻿using Cinemachine;
using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviourPunCallbacks, IPlayer, IPunObservable
{
    [Tooltip("The Local Player Instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    [SerializeField]
    private GameObject CinemachineCameraPrefab;

    private GameObject _camera;

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

    private AbstractPlayerScript playerScript;

    private bool isGameStart;

    private bool isDied;
    private void Start()
    {
        if (photonView.IsMine)
        {
            _camera = Instantiate(CinemachineCameraPrefab);
            CinemachineVirtualCamera virtualCamera = _camera.GetComponent<CinemachineVirtualCamera>();

            virtualCamera.Follow = gameObject.transform;
            virtualCamera.m_Lens.Orthographic = true;
            virtualCamera.m_Lens.OrthographicSize = 9;

            CinemachineConfiner confiner = virtualCamera.GetComponent<CinemachineConfiner>();
            GameObject map = GameObject.Find("Map");
            confiner.m_BoundingShape2D = map.GetComponent<MapManager>().GetMapCollider();
            virtualCamera.AddExtension(confiner);

            CinemachineBasicMultiChannelPerlin perlin =
                virtualCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            NoiseSettings shake = Resources.Load("6DShake") as NoiseSettings;
            perlin.m_NoiseProfile = shake;
            perlin.m_AmplitudeGain = 0;

            playerScript = GetComponent<AbstractPlayerScript>();
            playerScript.SetCinemachineBasicMultiChannelPerlin(perlin);

            isGameStart = false;
        }

        if (SceneManager.GetActiveScene().name.Equals("Game"))
        {
            GameManager.playerManagers.Add(this);
            GameStart();
        }
    }
    public void OnDestroy()
    {
        Destroy(_camera);
    }

    public void GameStart()
    {
        isGameStart = true;
        isDied = false;

        if (photonView.IsMine)
        {
            bloodScreen = GameObject.Find("BloodScreen");
            SkillUIController controller = GameObject.Find("SkillUI").GetComponent<SkillUIController>();
            playerScript.SetSkillUiController(controller);

            EnableLight();
            CreatePlayerUI();
        }
        else
            DisableLight();
    }

    private void EnableLight()
    {
        GetComponent<Light2D>().enabled = true;
        foreach (Transform transform in gameObject.transform)
        {
            if (transform.name == "sight")
            {
                transform.gameObject.GetComponent<Light2D>().enabled = true;
                break;
            }
        }
    }


    private void DisableLight()
    {
        GetComponent<Light2D>().enabled = false;
        foreach (Transform transform in transform)
        {
            if (transform.name == "sight")
            {
                Destroy(transform.gameObject);
                break;
            }
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
        StartCoroutine("ShowBloodScreen");
        
        if (Health <= 0) {
            photonView.RPC("Dead", PhotonTargets.All);
            GameManager.Instance.GameLose();
        }
    }

    public void heal(float healamount)
    {
        if (!isGameStart || !photonView.IsMine)
            return;

        if (Health >= 1f)
        {
            Health = 1f;
        }
        else
        {
            Health += healamount;
        }
               
                
    }


    [PunRPC]
    public void Dead()
    {
        isDied = true;
        gameObject.SetActive(false);
        GameManager.playerManagers.Remove(this);
        if (photonView.IsMine)
        {
            FindObjectOfType<Camera>().backgroundColor = Color.gray;
            Renderer[] renderers = FindObjectsOfType<Renderer>();
            foreach (Renderer renderer in renderers)
                renderer.material.SetColor("_Color", Color.gray);
        }
        else
        {
            GameManager.Instance.CheckGameOver();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
            stream.SendNext(this.Health);
        else
            Health = (float)stream.ReceiveNext();
    }

    IEnumerator ShowBloodScreen()
    {
        Debug.Log("ShowBlood Screen" + bloodScreen);
        bloodScreen.GetComponent<Image>().color = new Color(1, 0, 0, Random.Range(0.3f, 0.4f));
        yield return new WaitForSeconds(0.2f);
        bloodScreen.GetComponent<Image>().color = Color.clear;
    }

    public bool IsDied()
    {
        return isDied;
    }

}
