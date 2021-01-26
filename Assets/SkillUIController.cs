using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUIController : MonoBehaviour
{
    [SerializeField]
    private SkillUI[] skillUi;

    public void UseSkill(SkillType type, float coolTime)
    {
        int target = (int)type;
        skillUi[target].UseSkill(coolTime);
    }

    public enum SkillType
    {
        Attack = 0,
        Skill = 1,
        Dig = 2,
        MakeWall = 3,
        Trap = 4,
        Temp = 5
    }

}
