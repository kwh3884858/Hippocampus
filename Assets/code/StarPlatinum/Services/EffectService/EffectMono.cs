using System.Collections.Generic;
using Entitas.Unity;
using UnityEngine;

namespace StarPlatinum.Services.EffectService
{
    public class EffectMono: MonoBehaviour
    {
        public void SetInfo(EffectConfig config)
        {
            m_config = config;
            if (config.Parent != null)
            {
                transform.SetParent(config.Parent);
            }
            
            if (m_config.IsFollow && m_config.Parent != null)
            {
                transform.SetParent(m_config.Parent, m_config.LayerType != EnumEffectLayerType.UI);
            }

            if (m_config.IsFollow)
            {
                transform.localPosition = m_config.Pos;
                transform.localEulerAngles = m_config.EulerAngle;
            }
            else
            {
                transform.position = m_config.Pos;
                transform.eulerAngles = m_config.EulerAngle;
            }

            SetGlobalScale(m_config.Scale);

            SetLayer(m_config);
            RePlayEffect();

            InitDestroyScript(m_config);

        }
        
         private void SetGlobalScale(Vector3 scale)
        {
            if (m_config.LayerType != EnumEffectLayerType.UI)
            {
                    transform.localScale = Vector3.one;
                    var lossyScale = transform.lossyScale;
                    var newScale = new Vector3(scale.x / lossyScale.x, scale.y / lossyScale.y, scale.z / lossyScale.z);
                    transform.localScale = newScale;
            }
            else
            {
                transform.localScale = scale;
            }

            SetTrailRendererScale(scale);
            SetLineRendererScale(scale);
        }

        private void SetTrailRendererScale(Vector3 scale)
        {
            var trailRenders = transform.GetComponentsInChildren<TrailRenderer>();
            foreach (var render in trailRenders)
            {
                m_originTrailRenderScaleDic.Add(render, render.startWidth);
                render.startWidth *= scale.x;
//                Universal.PETDebug.LogError($"render:{render},scale:{scale}");
            }
        }
        
        private void SetLineRendererScale(Vector3 scale)
        {
            var trailRenders = transform.GetComponentsInChildren<LineRenderer>();
            foreach (var render in trailRenders)
            {
                m_originLineRenderScaleDic.Add(render, render.startWidth);
                render.startWidth *= scale.x;
            }
        }

        private void InitDestroyScript(EffectConfig config)
        {
            m_destroyLoopControl = gameObject.GetComponent<LoopDestroySelf>();
            if (m_destroyLoopControl)
            {
                m_destroyLoopControl.Duration = config.Duration;
                m_destroyLoopControl.RePlay();
            }
            else
            {
                m_destroyControl = gameObject.GetComponent<DestroySelf>();
                if (m_destroyControl)
                {
                    m_destroyControl.RePlay();
                }
            }
        }

        private void SetLayer(EffectConfig config)
        {

        }

        private void RePlayEffect()
        {

            var animator = transform.GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                animator.Play(0,0,0);
            }
            
            var anim = transform.GetComponentInChildren<Animation>();
            if (anim != null)
            {
                anim.cullingType = AnimationCullingType.AlwaysAnimate;
                anim.Rewind();
                anim.Play();
            }

            var particles = transform.GetComponentsInChildren<ParticleSystem>();
            foreach (var particle in particles)
            {
                particle.Play();
            }

            var trails = transform.GetComponentsInChildren<TrailRenderer>(); 
            foreach (var trail in trails)
            {
                trail.Clear();
            }

            EnableRenderer(true);
        }

        private void EnableRenderer(bool value)
        {
            var trails = transform.GetComponentsInChildren<TrailRenderer>();
            foreach (var trail in trails)
            {
                trail.enabled = value;
            }
            var lines = transform.GetComponentsInChildren<LineRenderer>();
            foreach (var line in lines)
            {
                line.enabled = value;
            }
        }
        
        
        
        private void OnDestroy()
        {
            if (gameObject != null)
            {
                CullingAnimator();
                EnableRenderer(false);
                ResetTrailRendererScale();
                ResetLineRendererScale();
                if (m_destroyLoopControl)
                {
                    m_destroyLoopControl.StopAll();
                }
                EffectService.Instance.ReleaseEffectAsset(this.gameObject);
            }
        }

        private void CullingAnimator()
        {
            var animator = transform.GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
            }
            
            var anim = transform.GetComponentInChildren<Animation>();
            if (anim != null)
            {
                anim.cullingType = AnimationCullingType.BasedOnRenderers;
            }
        }
        
        private void ResetTrailRendererScale()
        {
            var trailRenders = transform.GetComponentsInChildren<TrailRenderer>();
            foreach (var render in trailRenders)
            {
                if (m_originTrailRenderScaleDic.ContainsKey(render))
                {
                    render.startWidth = m_originTrailRenderScaleDic[render];
                    m_originTrailRenderScaleDic.Remove(render);
//                    Universal.PETDebug.LogError($"[reset]render:{render},scale:{render.startWidth}");
                }
            }
        }
        
        private void ResetLineRendererScale()
        {
            var trailRenders = transform.GetComponentsInChildren<LineRenderer>();
            foreach (var render in trailRenders)
            {
                if (m_originLineRenderScaleDic.ContainsKey(render))
                {
                    render.startWidth = m_originLineRenderScaleDic[render];
                    m_originLineRenderScaleDic.Remove(render);
                }
            }
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
        
        private EffectConfig m_config;
        
        private DestroySelf m_destroyControl;
        private LoopDestroySelf m_destroyLoopControl;
        
        private float m_originScale;
        private Dictionary<TrailRenderer, float> m_originTrailRenderScaleDic = new Dictionary<TrailRenderer, float>();
        private Dictionary<LineRenderer, float> m_originLineRenderScaleDic = new Dictionary<LineRenderer, float>();


    }
}