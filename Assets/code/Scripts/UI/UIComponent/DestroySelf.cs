using StarPlatinum;
using UnityEngine;

public class DestroySelf : MonoBehaviour {

    public float DeletTime;


    protected bool _isRelease = true;
    protected float _nowTime;
    protected bool _isPlaying;


    public virtual void RePlay()
    {
        _isPlaying = true;
        _nowTime = 0;
        _isRelease = false;
    }
 
    void Update ()
    {
        if (_isRelease)
        {
            return;
        }

        _nowTime += Time.deltaTime;
        if (_nowTime > DeletTime)
        {
            PrefabManager.Instance.UnloadAsset(this.gameObject);

            OnDispose();
            _isRelease = true;
        }
    }

    protected virtual void OnDispose()
    {
        _isPlaying = false;
    }
}