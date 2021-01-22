using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    //public Image skillImage;

    
    public Image skillFilter;
    public TMP_Text coolTimeCounter;

    public float coolTime;

    private float currentCoolTime;

    private bool canUseSkill = true;

    // Start is called before the first frame update
    void Start()
    {
        skillFilter.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UseSkill()
    {
        if (canUseSkill)
        {
            Debug.Log("스킬 시전");

            skillFilter.fillAmount = 1;
            StartCoroutine("CoolTime");

            currentCoolTime = coolTime * 10f;
            coolTimeCounter.text = "" + currentCoolTime;

            StartCoroutine("CoolTimeCounter");

            canUseSkill = false;
        }
        else
        {
            Debug.Log("cooltime");
        }
        
    }

    IEnumerator CoolTime()
    {
        while (skillFilter.fillAmount > 0)
        {
            skillFilter.fillAmount -= 1 * Time.smoothDeltaTime / coolTime;

            yield return null;
        }

        canUseSkill = true;

        yield break;
    }

    IEnumerator CoolTimeCounter()
    {
        while (currentCoolTime > 0)
        {
            yield return new WaitForSeconds(0.1f);

            currentCoolTime = Mathf.Round(currentCoolTime - 1f);

            
            coolTimeCounter.text = "" + currentCoolTime * 0.1f;
        }

        yield break;
    }

    
}
