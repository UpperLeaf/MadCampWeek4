using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    [Tooltip("UI Text to display Player's name")]
    [SerializeField]
    private TMP_Text playerNameText;

    [Tooltip("UI Slider to display Player's Health")]
    [SerializeField]
    private Slider playerHealthSlider;


    private PlayerManager target;


    public void SetTarget(PlayerManager _target)
    {
        if(_target == null)
        {
            Debug.LogError("PlayerMakerManager target for PlayerUI");
        }
        target = _target;

        if(playerNameText != null)
        {
            playerNameText.text = target.photonView.Owner.NickName;
        }
    }
}
