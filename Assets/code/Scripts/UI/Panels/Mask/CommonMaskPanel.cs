using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using DG.Tweening;
using UnityEngine.UI;

namespace UI.Panels
{
    public partial class CommonMaskPanel : UIPanel<UIDataProvider, DataProvider>
    {
        #region UI template method
        public override void Initialize(UIDataProvider uiDataProvider, UIPanelSettings settings)
        {
            base.Initialize(uiDataProvider, settings);
            m_model.Initialize(this);
        }

        public override void DeInitialize()
        {
            m_model.Deactivate();
            base.DeInitialize();
        }

        public override void Hide()
        {
            m_model.Hide();
            base.Hide();
        }

        public override void Deactivate()
        {
            m_model.Deactivate();
            base.Deactivate();
        }

        public override void ShowData(DataProvider data)
        {
            m_model.ShowData(data);
            base.ShowData(data);
            maskData = data as MaskDataProvider;
            ShowAnim();
        }

        public override void UpdateData(DataProvider data)
        {
            m_model.UpdateData(data);
            base.UpdateData(data);
        }

        public override void Tick()
        {
            m_model.Tick();
            base.Tick();
        }

        public override void LateTick()
        {
            m_model.LateTick();
            base.LateTick();
        }

        public override void SubpanelChanged(UIPanelType type, DataProvider data = null)
        {
            m_model.SubpanelChanged(type, data);
            base.SubpanelChanged(type, data);
        }

        public override void SubpanelDataChanged(UIPanelType type, DataProvider data)
        {
            m_model.SubpanelDataChanged(type, data);
            base.SubpanelDataChanged(type, data);
        }
        #endregion

        #region Member
        [SerializeField]
        private Image fadeImage = null;
        [SerializeField]
        private RawImage fenceImage = null;
        [SerializeField]
        private Image diffusionImage = null;

        private void ShowAnim()
        {
            switch (maskData.MyType)
            {
                case MaskType.Normal:
                    DoNormalFade();
                    break;
                case MaskType.TimeShape:
                    DoTimeShapeFade();
                    break;
                case MaskType.DiffusionShape:
                    DoDiffusionShape();
                    break;
                case MaskType.FenceShape:
                    DoFenceShapeFade();
                    break;
                case MaskType.ShuttleShape:
                    DoNormalFade();
                    break;
                default:
                    DoNormalFade();
                    break;
            }
        }

        private void DoNormalFade()
        {
            if (fadeImage != null)
            {
                var startColor = fadeImage.color;
                startColor.a = 0f;
                fadeImage.color = startColor;
                mySequence = DOTween.Sequence();
                mySequence.Append(fadeImage.DOFade(1f, maskData.InTime));
                mySequence.AppendInterval(maskData.StayTime);
                mySequence.Append(fadeImage.DOFade(0f, maskData.OutTime));
                mySequence.OnComplete(() =>
                {
                    UI.UIManager.Instance().HideStaticPanel(UIPanelType.UICommonMaskPanel);
                });
            }
        }

        private void DoTimeShapeFade()
        {
            if (fadeImage != null)
            {
                fadeImage.fillAmount = 0f;
                mySequence = DOTween.Sequence();
                mySequence.Append(fadeImage.DOFillAmount(1f, maskData.InTime));
                mySequence.AppendInterval(maskData.StayTime);
                mySequence.Append(fadeImage.DOFillAmount(0f, maskData.OutTime));
                mySequence.OnComplete(() =>
                {
                    UI.UIManager.Instance().HideStaticPanel(UIPanelType.UICommonMaskPanel);
                });
            }
        }

        private void DoDiffusionShape()
        {
            isDiffusion = true;
            if (fadeImage != null)
            {
                fadeImage.enabled = false;
            }
            if (diffusionImage != null)
            {
                diffusionImage.gameObject.SetActive(true);
                material = new Material(diffusionImage.material);
                diffusionImage.material = material;
                float resouceRadius = material.GetFloat(DIFFUSION_RADIUS);
                mySequence = DOTween.Sequence();
                mySequence.Append(material.DOFloat(0f, DIFFUSION_RADIUS, maskData.InTime));
                mySequence.AppendInterval(maskData.StayTime);
                mySequence.Append(material.DOFloat(resouceRadius, DIFFUSION_RADIUS, maskData.OutTime));
                mySequence.OnComplete(() =>
                {
                    UI.UIManager.Instance().HideStaticPanel(UIPanelType.UICommonMaskPanel);
                });
            }
        }

        private void DoFenceShapeFade()
        {
            if (fadeImage != null)
            {
                fadeImage.enabled = false;
            }
            if (fenceImage != null)
            {
                fenceImage.gameObject.SetActive(true);
                material = new Material(fenceImage.material);
                fenceImage.material = material;
                material.SetFloat(START_FLAG, 1);
                float col = material.GetFloat(COLUMN);
                mySequence = DOTween.Sequence();
                mySequence.Append(material.DOFloat(1f / col, START_TIME, maskData.InTime));
                mySequence.AppendInterval(maskData.StayTime);
                mySequence.Append(material.DOFloat(0f, START_TIME, maskData.OutTime));
                mySequence.OnComplete(() =>
                {
                    UI.UIManager.Instance().HideStaticPanel(UIPanelType.UICommonMaskPanel);
                });
            }
        }

        private void OnDestroy()
        {
            if (mySequence != null && mySequence.IsActive())
            {
                mySequence.Kill();
            }
        }
        private Sequence mySequence = null;
        private MaskDataProvider maskData = null;
        private readonly string START_FLAG = "_StartFlag";
        private readonly string START_TIME = "_StartTime";
        private readonly string COLUMN = "_Column";
        private Material material = null;
        private readonly string DIFFUSION_RADIUS = "_Radius";
        private bool isDiffusion = false;
        #endregion
    }
}