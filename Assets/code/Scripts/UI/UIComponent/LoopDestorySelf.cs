using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopDestroySelf : DestroySelf
{
    [HideInInspector]
    public float Duration;
    
    private ParticleSystem[] _particles;
    private List<ParticleSystem> _stopParticles = new List<ParticleSystem>();
    private List<ParticleSystem> _clearParticles = new List<ParticleSystem>();
    private MeshRenderer[] _meshs;

    private void Awake()
    {
//        Universal.PETDebug.LogError($"[Awake]LoopDestroySelf特效Start:{transform.name}");
        _particles = GetComponentsInChildren<ParticleSystem>();
        foreach (var particle in _particles)
        {
            if (!particle.main.loop)
            {
                continue;
            }

            if (particle.name.Contains("_PM"))
            {
                _clearParticles.Add(particle);
            }
            else
            {
                _stopParticles.Add(particle);
            }
        }

        _meshs = GetComponentsInChildren<MeshRenderer>();
        if (!_isPlaying)
        {
            StopAll();
        }
    }

//    private void Start()
//    {
////        Universal.PETDebug.LogError($"[Start]LoopDestroySelf特效Start:{transform.name}");
//        
//    }
    
    public override void RePlay()
    {
        _isPlaying = true;
        if (Duration > 0)
        {
//            Universal.PETDebug.LogError($"開始：{gameObject.name}");
            StartCoroutine(Stop());
        }
        else
        {
//            Universal.PETDebug.LogError($"[Error]Effect :{gameObject.name},Duration: 0");
        }
    }

    private IEnumerator Stop()
    {
        yield return new WaitForSeconds(Duration);
        StopEmission();
        _nowTime = 0;
        _isRelease = false;
    }

    private void StopEmission()
    {
//        Universal.PETDebug.LogError($"停止：{gameObject.name}");
        if (_particles == null)
        {
            return;
        }

        foreach (var particle in _stopParticles)
        {
            if (particle != null)
            {
//                Universal.PETDebug.LogError($"停止粒子:{gameObject.name}--{particle}");
                particle.Stop();
            }
        }

        foreach (var particle in _clearParticles)
        {
            if (particle != null)
            {
                particle.Clear();
//                Universal.PETDebug.LogError($"清除粒子:{gameObject.name}--{particle}");
            }
        }

        foreach (var mesh in _meshs)
        {
            mesh.enabled = false;
//            Universal.PETDebug.LogError($"隐藏mesh:{gameObject.name}--{mesh}");
        }
    }

    public void StopAll()
    {
        _isPlaying = false;
//        Universal.PETDebug.LogError($"[StopAll]LoopDestroySelf特效Start:{transform.name}");
        foreach (var particle in _stopParticles)
        {
            if (particle != null)
            {
                particle.Stop();
            }
        }

        foreach (var particle in _clearParticles)
        {
            if (particle != null)
            {
                particle.Stop();
            }
        }
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        foreach (var mesh in _meshs)
        {
            mesh.enabled = true;
        }
//        Universal.PETDebug.LogError($"释放：{gameObject.name}");
    }
}