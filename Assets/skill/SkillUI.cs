using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public Image skillFilter;
    [SerializeField]
    private Image icon; 
    public TMP_Text coolTimeCounter;
    public TMP_Text ammoText;
    private float coolTime;

    

    void Start()
    {
        skillFilter.fillAmount = 0;
        
        coolTimeCounter = GetComponent<TMP_Text>();
    }

    public void UseSkill(float coolTime)
    {
        skillFilter.fillAmount = 1;
        this.coolTime = coolTime;
        StartCoroutine("CoolTime");
    }

    public void updateAmmo(int ammo)
    {
        ammoText.text = ammo.ToString();
    }

    public void DisableAmmo()
    {
        ammoText.text = "";
    }

    public void SetImage(Sprite sprite)
    {
        icon.sprite =sprite;
    }

    public void iconInit(Vector3 position, Vector2 size, float rotation)
    {
        icon.rectTransform.localPosition = position;
        icon.rectTransform.localEulerAngles = new Vector3(0,0,rotation);
        icon.rectTransform.sizeDelta = size;
    }

    IEnumerator CoolTime()
    {
        while (skillFilter.fillAmount > 0)
        {
            skillFilter.fillAmount -= 1 * Time.smoothDeltaTime / coolTime;
            yield return null;
        }
        yield break;
    }
}
