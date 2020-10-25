using System.Collections;
using System.Collections.Generic;
using UI.Utils;
using UnityEngine;

namespace UI.Panels.Element
{
    public enum EnumCharacterStatus
    {
        Idle,
        Charge,
        Hit,
        LightAttack,
        HeavyAttack,
    }
    public partial class UI_Judgment_ControversyCharactor_Item_SubView : UIElementBase
    {
        private EnumCharacterStatus m_status;

        public void SetStatus(EnumCharacterStatus status)
        {
            switch (status)
            {
                case EnumCharacterStatus.Charge:
                    m_status = status;
                    m_UI_Judgment_ControversyCharactor_Item_Animator.SetBool(AnimatorString.CharacterIsCharge,true);
                    break;
                case EnumCharacterStatus.Idle:
                    m_UI_Judgment_ControversyCharactor_Item_Animator.SetBool(AnimatorString.CharacterIsCharge,false);
                    m_status = status;
                    break;
                case EnumCharacterStatus.Hit:
                    m_UI_Judgment_ControversyCharactor_Item_Animator.SetTrigger(AnimatorString.CharacterHit);
                    break;
                case EnumCharacterStatus.HeavyAttack:
                    m_UI_Judgment_ControversyCharactor_Item_Animator.SetTrigger(AnimatorString.CharacterHeavyAttack);
                    break;
                case EnumCharacterStatus.LightAttack:
                    m_UI_Judgment_ControversyCharactor_Item_Animator.SetTrigger(AnimatorString.CharacterLightAttack);
                    break;
            }
        }
    }
}