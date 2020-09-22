using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using StarPlatinum.Base;
using UnityEngine.Assertions;

namespace StarPlatinum
{
	public class SoundService : Singleton<SoundService>
	{
		GameObject m_music;
		GameObject m_effect;

		private bool m_isPlayMusic;
		private bool m_isPlayEffect;

		private int m_volume;
		private int m_volumePercentage;

		public AudioSource m_backgoundSound = null;
		private Dictionary<string, AudioClip> m_bgmCache = new Dictionary<string, AudioClip> ();
		private Dictionary<string, AudioSource> m_effectCache = new Dictionary<string, AudioSource> ();

		public SoundService ()
		{

			Transform root = GameObject.Find ("GameRoot").transform;
			Assert.IsTrue (root != null, "Game Root must always exist.");
			if (root == null) return;

			m_music = new GameObject ("Bgm");
			m_music.transform.SetParent (root.transform);
			m_backgoundSound = m_music.AddComponent<AudioSource> ();

			m_effect = new GameObject ("Effect");
			m_effect.transform.SetParent (root.transform);

			m_isPlayEffect = true;
			m_isPlayMusic = true;
			SetVolume(50);
			SetVolumePercentage(100);
		}

		/// <summary>
		/// Play the music.
		/// </summary>
		/// <param name="name">music name.</param>
		/// <param name="isloop">If set to <c>true</c> is loop.</param>
		public void PlayBgm (string name, bool isloop = true)
		{
			if (this.m_backgoundSound.clip != null && name.IndexOf (this.m_backgoundSound.clip.name) > -1) {
				return;
			}
			if (!m_isPlayMusic) {
				m_backgoundSound.volume = 0f;
			}
			m_backgoundSound.loop = isloop;
			var clip = LoadAudioClip ("Bgm/" + name);
			if (clip) {
				m_backgoundSound.clip = clip;
				if (m_backgoundSound.clip != null) {
					m_backgoundSound.Play ();
				}
			}
		}

		public void StopMusic ()
		{
			m_backgoundSound.Stop ();
			m_backgoundSound.clip = null;
		}

		public AudioClip LoadAudioClip (string name)
		{
			AudioClip ac = null;
			m_bgmCache.TryGetValue (name, out ac);
			if (ac == null) {
				ac = AssetsManager.LoadPrefabFromResources<AudioClip> ("Sound/" + name);

				//if (Application.platform == RuntimePlatform.OSXPlayer) {
				//	ac = AssetsManager.Load<AudioClip> ("Sound/" + name);
				//}

				if (ac != null) {
					m_bgmCache.Add (name, ac);
				} else {
					Debug.Log ("Can`t load sound " + name);
				}
			}

			return ac;
		}

		/// <summary>
		/// 播放音效
		/// </summary>
		public void PlayEffect (string name, bool isLoop = false, float volume = 0.5f, bool isCover = false)
		{
			string _name = name;
			if (isCover) {
				//允许重复播放
				while (true) {
					name += UnityEngine.Random.Range (0, 9999).ToString ();
					if (m_effectCache.ContainsKey (name)) {
						continue;
					} else {
						break;
					}
				}
			} else {
				if (m_effectCache.ContainsKey (name)) {
					//杜绝重复播放
					return;
				}
			}

			if (!m_isPlayEffect) {
				volume = 0;
			}

			GameObject effectGameObject = new GameObject (name);
			effectGameObject.transform.SetParent (m_effect.transform);

			AudioSource audioSource = (AudioSource)effectGameObject.AddComponent<AudioSource> ();
			var clip = LoadAudioClip ("Effect/" + _name);
			audioSource.clip = clip;
			//audioSource.spatialBlend = 1f;
			//audioSource.volume = 1;
			audioSource.volume = volume;
			audioSource.loop = isLoop;
			audioSource.Play ();

			m_effectCache.Add (name, audioSource);

			if (!isLoop) {
				if (clip != null) {
					var clearTime = clip.length * ((Time.timeScale >= 0.01f) ? Time.timeScale : 0.01f);

					CoroutineComponent coroutine = effectGameObject.AddComponent<CoroutineComponent> ();
					coroutine.InvokeCoroutine (DelayToInvokeDo (() => {
						GameObject.Destroy (effectGameObject);
						m_effectCache.Remove (name);
					},
					clearTime));
				} else {
					Debug.LogError ("Cannot find Effect/" + _name + ". clip is null");
				}
			}

		}

		//public void PlayEffectCover (string name)
		//{


		//	GameObject gameObject = new GameObject (gameName);
		//	gameObject.transform.SetParent (effect.transform);

		//	AudioSource audioSource = (AudioSource)gameObject.AddComponent<AudioSource> ();
		//	var clip = LoadAudioClip (name);
		//	audioSource.clip = clip;
		//	//audioSource.spatialBlend = 1f;
		//	//audioSource.volume = 1;
		//	audioSource.Play ();
		//	audioSource.volume = 0.5f;
		//	effectsounCache.Add (gameName, audioSource);
		//	var clearTime = clip.length * ((Time.timeScale >= 0.01f) ? Time.timeScale : 0.01f);

		//StartCoroutine(DelayToInvokeDo (() => {
		//    GameObject.Destroy(gameObject);
		//    effectsounCache.Remove(gameName);
		//}, clearTime));
		//}
		/// <summary>
		/// 播放音效
		/// </summary>
		/// <param name="name">片段名.</param>
		/// <param name="isLoop">是否重复.</param>
		//public void PlayEffect (string name, bool isLoop)
		//{
		//	if (effectsounCache.ContainsKey (name)) {
		//		//杜绝重复播放
		//		return;
		//	}

		//	GameObject gameObject = new GameObject (name);
		//	gameObject.transform.SetParent (effect.transform);

		//	AudioSource audioSource = (AudioSource)gameObject.AddComponent<AudioSource> ();
		//	var clip = LoadAudioClip (name);
		//	audioSource.clip = clip;
		//	audioSource.loop = isLoop;
		//	//audioSource.spatialBlend = 1f;
		//	//audioSource.volume = 1;
		//	audioSource.Play ();
		//	effectsounCache.Add (name, audioSource);

		//	//var clearTime = clip.length * ((Time.timeScale >= 0.01f) ? Time.timeScale : 0.01f);

		//}

		public void StopEffect (string name)
		{
			if (m_effectCache.ContainsKey (name)) {
				//杜绝重复播放
				GameObject.Destroy (m_effectCache [name].gameObject);
				m_effectCache.Remove (name);

			} else {
				return;
			}


		}

		IEnumerator DelayToInvokeDo (Action action, float delaySeconds)
		{
			yield return new WaitForSeconds (delaySeconds);
			action ();
		}

		public void Play (string name)
		{
			AudioSource.PlayClipAtPoint (LoadAudioClip (name), new Vector3 (), 1);
		}

		public void SetVolumePercentage(int value)
		{
			m_volumePercentage = value;
			SetVolume();
		}

		public void SetVolume(int value)
		{
			m_volume = value;
			SetVolume();
		}

		private void SetVolume()
		{
			m_backgoundSound.volume = (float)m_volume * m_volumePercentage / 10000;
		}
	}

	//	public void OpenPlayMusic ()
	//	{
	//		AdjustMusicVolume (0.5f);
	//		m_isPlayMusic = true;
	//	}

	//	public void ClosePlayMusic ()
	//	{
	//		AdjustMusicVolume (0);
	//		m_isPlayMusic = false;
	//	}

	//	public void OpenPlayEffect ()
	//	{
	//		foreach (AudioSource audio in m_effectCache.Values) {
	//			audio.volume = 0;
	//		}
	//		m_isPlayEffect = true;
	//	}

	//	public void ClosePlayEffect ()
	//	{
	//		foreach (AudioSource audio in m_effectCache.Values) {
	//			audio.volume = 0.5f;
	//		}
	//		m_isPlayEffect = false;
	//	}
	//}
}