using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.Element
{
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

        public void Release()
        {
            gameObject.SetActive(false);
            SpriteCount = 0;
        }

        private void ActiveItem(bool status)
        {
            gameObject.SetActive(status);
        }

        private int m_spriteCount = 0;
        private int m_spriteMaxCount = 1;
        private string m_pictureID;

        [SerializeField] private Image m_body;
        [SerializeField] private Image m_mouse;
        [SerializeField] private Image m_eyes;
        [SerializeField] private Image m_eyebrow;
    }
}