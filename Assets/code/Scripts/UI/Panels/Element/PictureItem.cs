using UI.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.Element
{
    public enum EnumTachieStatus
    {
        Normal,
        Talk,
        Darken,
    }
    public class PictureItem : UIElementBase
    {
        public string PictureId
        {
            get => m_pictureID;
        }

        private int SpriteCount
        {
            get => m_spriteCount;
            set
            {
                m_spriteCount = value;
                if (m_spriteCount == m_spriteMaxCount)
                {
                    ActiveItem(true);
                }
            }
        }

        public void Initialize(string picID)
        {
            m_pictureID = picID;
            SpriteCount = 0;
            ActiveItem(false);
        }

        public void SetBody(Sprite body)
        {
            m_body.sprite = body;
            SpriteCount++;
        }
        
        public void SetMouse(Sprite mouse)
        {
            m_mouse.sprite = mouse;
            SpriteCount++;
        }
        public void SetEyes(Sprite eyes)
        {
            m_eyes.sprite = eyes;
            SpriteCount++;
        }
        public void SetEyebrow(Sprite eyebrow)
        {
            m_eyebrow.sprite = eyebrow;
            SpriteCount++;
        }

        public void SetTachieStatus(EnumTachieStatus status,bool force = false)
        {
            if (m_status == status && !force)
            {
                return;
            }

            m_status = status;
            switch (status)
            {
                case EnumTachieStatus.Darken:
                    m_animator.Play(AnimatorString.TachieDarken);
//                    m_makeChildrenGray.MakeGray(true);
                    break;
                case EnumTachieStatus.Talk:
                case EnumTachieStatus.Normal:
                    m_animator.Play(AnimatorString.TachieNormal);
//                    m_makeChildrenGray.MakeGray(false);
                    break;
            }
        }

        public string GetMyCharactorName()
        {
            if (string.IsNullOrEmpty(m_pictureID))
            {
                Debug.LogError("立绘ID未初始化！！！！");
                return null;
            }
            if (string.IsNullOrEmpty(m_charatorName))
            {
                m_charatorName = ConfigData.Instance.characterTable.GetCharacterName(m_pictureID);
            }

            return m_charatorName;
        }

        public void Release()
        {
            gameObject.SetActive(false);
            SpriteCount = 0;
            m_charatorName = "";
        }

        private void ActiveItem(bool status)
        {
            gameObject.SetActive(status);
            SetTachieStatus(EnumTachieStatus.Normal,true);
        }

        private int m_spriteCount = 0;
        private int m_spriteMaxCount = 1;
        private string m_pictureID;
        private string m_charatorName;
        private EnumTachieStatus m_status;

        [SerializeField] private Image m_body;
        [SerializeField] private Image m_mouse;
        [SerializeField] private Image m_eyes;
        [SerializeField] private Image m_eyebrow;
        [SerializeField] private Animator m_animator;
    }
}