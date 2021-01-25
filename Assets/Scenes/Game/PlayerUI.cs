using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Tooltip("Pixel offset from the Player Target")]
    private Vector3 screenOffset = new Vector3(0f, 120f, 0f);

    [Tooltip("UI Text to display Player's name")]
    [SerializeField]
    private TMP_Text playerNameText;

    [Tooltip("UI Slider to display Player's Health")]
    [SerializeField]
    private Slider playerHealthSlider;

    Transform targetTransform;
    Vector3 targetPosition;

    private PlayerManager target;

    private void Awake()
    {
        GameObject canvas = GameObject.Find("Canvas");
        transform.SetParent(canvas.GetComponent<Transform>(), false);
        
        //DontDestroyOnLoad(canvas);
        //DontDestroyOnLoad(gameObject);
    }

    public void SetTarget(PlayerManager _target)
    {
        if(_target == null)
            Debug.LogError("PlayerMakerManager target for PlayerUI");
        
        target = _target;
        targetTransform = target.transform;
        if(playerNameText != null)
        {
            playerNameText.text = target.photonView.Owner.NickName;
        }

    }

    private void Update()
    {
        if (playerHealthSlider != null)
        {
            playerHealthSlider.value = target.Health;
        }    

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void LateUpdate()
    {
        if(targetTransform != null)
        {
            targetPosition = targetTransform.position;
            transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
        }       
    }
}
