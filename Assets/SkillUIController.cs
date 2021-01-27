using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIController : MonoBehaviour
{
    [SerializeField]
    private SkillUI[] skillUi;

    

    public void UseSkill(SkillType type, float coolTime)
    {
        int target = (int)type;
        skillUi[target].UseSkill(coolTime);
    }

    public void updateAmmo(SkillType type, int ammo)
    {
        int target = (int)type;
        skillUi[target].updateAmmo(ammo);
    }

    public void DisableAmmoText(SkillType type)
    {
        int target = (int)type;
        skillUi[target].DisableAmmo();
    }

    public void SetImage(SkillType type, Sprite sprite)
    {
        int target = (int)type;
        skillUi[target].SetImage(sprite);
    }

    public void rifleInit()
    {
        skillUi[0].iconInit(new Vector3(20, 5, 0), new Vector2(200, 200), -20);
        skillUi[1].iconInit(new Vector3(5, 15, 0), new Vector2(150, 150), 0);
    }

    public void bombInit()
    {
        skillUi[0].iconInit(Vector3.zero, new Vector2(180, 180), 0);
        skillUi[1].iconInit(new Vector3(0, 5, 0), new Vector2(250, 250), 0);
    }

    public enum SkillType
    {
        Attack = 0,
        Skill = 1,
        MakeWall = 2,
        Trap = 3
    }

}
