using Const;
using Controllers.Subsystems.Story;
using DesperateDevs.Utils;
using StarPlatinum;
using TMPro;
using UI.Panels.Element;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.StaticBoard.Element
{

    public class RecordItem: UIElementBase
    {
        public void Init()
        {
            
        }

        public void SetData(RecordData data)
        {
            if (data == null)
            {
                Debug.LogWarning("RecordItem传入数据错误！！！！！");
                return;
            }
            RefreshElement(data.Type);
            switch (data.Type)
            {
                case RecordType.TalkContent:
                    
                    m_nameImg.gameObject.SetActive(false);
                    m_nameTxt.gameObject.SetActive(false);
                    PrefabManager.Instance.SetImage(m_nameImg,RolePictureNameConst.ArtWordName + data.str1, () =>
                    {
                        m_nameImg.gameObject.SetActive(true);
                        m_nameTxt.gameObject.SetActive(true);
                        m_nameTxt.text = data.str1;
                    });
                    m_contentTxt.text = data.str2;
                    break;
                case RecordType.Jump:
                    m_contentTxt.text = data.str1;
                    break;
                default:
                    Debug.LogWarning("RecordItem传入数据错误！！！！！");
                    break;
            }
        }

        private void RefreshElement(RecordType type)
        {
            m_nameObj.gameObject.SetActive(type == RecordType.TalkContent);
            m_contentTxt.gameObject.SetActive(type == RecordType.Jump || type == RecordType.TalkContent);
        }
        

        [SerializeField] private TMP_Text m_nameTxt;
        [SerializeField] private Image m_nameImg;
        [SerializeField] private GameObject m_nameObj;

        [SerializeField] private TMP_Text m_contentTxt;

        private string m_id;

    }
}