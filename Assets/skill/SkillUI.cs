using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public Image skillFilter;
    public TMP_Text coolTimeCounter;
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
