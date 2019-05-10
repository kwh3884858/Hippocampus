using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Skylight
{
	public class AnimationService : GameModule<AnimationService>
	{

		public void RotateToAnotherSide (GameObject go)
		{
			iTween.RotateBy (go, iTween.Hash ("x", 0.25f, "easeType", "easeOutElastic", "loopType", "loop", "delay", .4));
		}

		/// <summary>
		/// 3D物件以y轴左右抖动，带有缓动效果
		/// </summary>
		/// <param name="tran"></param>
		public void Shake (Transform tran)
		{
			StartCoroutine (ElasticAnimation (tran, 0.5f));

		}

		/// <summary>
		/// 大小缩放到零
		/// </summary>
		/// <param name="go"></param>
		public void SceleToZero (GameObject go)
		{
			iTween.ScaleTo (go, iTween.Hash ("y", 0, "z", 0, "easeType", "easeOutSine", "time", 0.5f));
		}

		/// <summary>
		/// 精灵左右抖动
		/// </summary>
		/// <param name="transform"></param>
		public void SpriteShake (Transform transform)
		{
			StartCoroutine (SpriteElasticAnimation (transform, 1f));
		}

		/// <summary>
		/// 2D物体闪烁
		/// </summary>
		/// <param name="tran"></param>
		public void Flash (Transform tran, float duration = 0.3f)
		{

			StartCoroutine (SpriteFlash (tran, duration));
		}

		/// <summary>
		/// 蒙皮的2D物体闪烁
		/// </summary>
		/// <param name="tran"></param>
		/// <param name="duration"></param>
		public void SkeletalFlash (Transform tran, float duration = 0.3f)
		{
			StartCoroutine (ChildrenSpriteFlash (tran, duration));

		}


		/// <summary>
		/// 2D物体闪烁和晃动
		/// </summary>
		/// <param name="tran"></param>
		public void FlashAndShake (Transform tran, float delay = 0f)
		{
			m_readyForFlashAndShake.Enqueue (tran);
			TimerService.Instance ().AddTimer (delay, TimerCallbackFlashAndShake);


		}
		public Queue<Transform> m_readyForFlashAndShake = new Queue<Transform> ();
		private void TimerCallbackFlashAndShake ()
		{
			Transform tran = m_readyForFlashAndShake.Dequeue ();
			StartCoroutine (SpriteFlashAndShake (tran, 0.3f));
		}

		/// <summary>
		/// 物件的左右抖动，循环性地修改x轴坐标，延迟和持续时间二选一
		/// </summary>
		/// <param name="tran"></param>
		public void HorizontalShake (Transform tran, float delay = 0f, float duration = 0.3f)
		{

			if (delay < 0.01f) {
				StartCoroutine (ShakeHorizontal (tran, duration));

			} else {
				m_readyForHorizontalShake.Enqueue (tran);
				TimerService.Instance ().AddTimer (delay, TimerCallbackHorizontalShake);
			}


		}
		public Queue<Transform> m_readyForHorizontalShake = new Queue<Transform> ();

		private void TimerCallbackHorizontalShake ()
		{
			Transform tran = m_readyForHorizontalShake.Dequeue ();
			StartCoroutine (ShakeHorizontal (tran, 0.2f));
		}


		/// <summary>
		/// 物件的上下抖动，循环性地修改y轴坐标,延迟和持续时间二选一
		/// </summary>
		/// <param name="tran"></param>
		public void VerticalShake (Transform tran, float delay = 0f, float duration = 0.3f)
		{
			if (delay < 0.01f) {
				StartCoroutine (ShakeVertical (tran, duration));

			} else {
				m_readyForVerticalFlash.Enqueue (tran);
				TimerService.Instance ().AddTimer (delay, TimerCallbackVerticalShake);
			}

		}
		public Queue<Transform> m_readyForVerticalFlash = new Queue<Transform> ();

		private void TimerCallbackVerticalShake ()
		{
			Transform tran = m_readyForVerticalFlash.Dequeue ();
			StartCoroutine (ShakeVertical (tran, 0.2f));

		}

		/// <summary>
		/// 游戏物件逐渐消失
		/// </summary>
		/// <param name="go"></param>
		/// <param name="duration"></param>
		public void GameObejctToFadeItween (GameObject go, float duration)
		{
			iTween.ColorTo (go, new Color (1, 1, 1, 0), duration);
		}

		public void GameObejctToFade (GameObject go, float duration = 0.3f)
		{
			StartCoroutine (SpriteFade (go.transform, duration));

		}

		/// <summary>
		/// UI逐渐消失
		/// </summary>
		/// <param name="tran"></param>
		public void UIToFade (Transform tran)
		{
			Component [] comps = tran.GetComponentsInChildren<Component> ();
			foreach (Component c in comps) {
				if (c is Graphic) {
					(c as Graphic).CrossFadeAlpha (0, 1.5f, true);
				}

			}
		}

		/// <summary>
		/// UI逐渐显示
		/// </summary>
		/// <param name="tran"></param>
		public void UIFromFade (Transform tran, float duration = 1.5f)
		{
			Component [] comps = tran.GetComponentsInChildren<Component> ();
			foreach (Component c in comps) {
				if (c is Graphic) {
					(c as Graphic).CrossFadeAlpha (1, duration, true);
				}

			}
		}

		public void UIFromFadeEnumrator<T> (T source, float duration = 1.5f) where T : Graphic
		{
			StartCoroutine (ImageFadeOut (source, duration));
		}

		IEnumerator DelayToInvokeDo (Action action, float delaySeconds)
		{
			yield return new WaitForSeconds (delaySeconds);
			action ();
		}

		IEnumerator DelaySomeTime (float delaySeconds)
		{
			yield return new WaitForSeconds (delaySeconds);
		}

		IEnumerator SpriteElasticAnimation (Transform ElasticTran, float duration)
		{
			//Debug.LogError("怪物抖动:" + gameObject.transform);
			float time = Time.time;
			Vector3 oldPos = ElasticTran.position;
			while (true) {

				yield return new WaitForSeconds (0.03f);
				time += Time.fixedDeltaTime;
				if (time > duration) {
					time = 0;
					break;
				} else {
					//float y = Mathf.Sin(time);

					ElasticTran.transform.position = new Vector3 (Mathf.Sin (250 * time) + oldPos.x,
						oldPos.y, oldPos.z);
				}
			}
			ElasticTran.transform.position = oldPos;
			yield break;
		}

		IEnumerator ElasticAnimation (Transform ElasticTran, float duration)
		{
			float time = Time.time;

			while (true) {

				yield return new WaitForSeconds (0.03f);
				time += Time.fixedDeltaTime;
				if (time > duration) {
					time = 0;
					break;
				} else {
					float y = (1 / 3 * time - 0.2f) * Mathf.Sin (200 * time * time);
					if (Mathf.Abs (y) < 0.1) {
						y = 0;
					}

					ElasticTran.transform.rotation = Quaternion.AngleAxis (y * 10, Vector3.up);
				}
			}
			ElasticTran.transform.rotation = new Quaternion (0, 0, 0, 0);
			yield break;
		}

		IEnumerator SpriteFlashAndShake (Transform tran, float duration)
		{
			float time = 0;

			SpriteRenderer spriteRender = tran.GetComponent<SpriteRenderer> ();
			Vector3 tranPos = tran.position;
			while (true) {
				yield return new WaitForSeconds (0.03f);

				//if (spriteRender == null || spriteRender.material == null)
				// {
				//   yield return null;
				//}
				time += Time.fixedDeltaTime;
				if (time > duration) {
					time = 0;
					break;
				} else {
					float y = Mathf.Sin (70 * time);
					if (tran != null && spriteRender != null && spriteRender.material != null) {
						tran.position = new Vector3 (tranPos.x + y / 8, tranPos.y, tranPos.z);
						//Debug.L4og("spriteRender.size " + spriteRender.size);
						spriteRender.material.color = new Color (1, 1, 1, (y + 1) / 2);
					}

				}
			}

			if (tran != null && spriteRender != null && spriteRender.material != null) {
				spriteRender.material.color = new Color (1, 1, 1, 1);
				tran.position = tranPos;
			}

			yield break;
		}

		public void spriteMoveInAndOut (GameObject sp, Vector2 from, Vector2 to)
		{
			StartCoroutine (spriteMove (sp, from, to, 0.5f, 1f));
		}

		IEnumerator spriteMove (GameObject sp, Vector2 from, Vector2 to, float time, float speed)
		{
			//yield return null;
			bool isMoveRight = true;
			float distance = 0;
			Vector3 speedVal = new Vector3 (speed, speed, 0f);
			GameObject ggo;
			ggo = GameObject.Instantiate (sp, new Vector3 (from.x, from.y, -3f), Quaternion.identity);
			ggo.GetComponent<SpriteRenderer> ().sortingOrder = 100;
			Debug.Log ("from:" + from + " " + "to::" + to);
			while (true) {
				yield return new WaitForSeconds (0.01f);

				if (ggo.transform.position.x + 10 < from.x) {
					break;
				}
				distance = 0.01f * (to.x - ggo.transform.position.x) * (to.x - ggo.transform.position.x);
				if (distance < 0.3f) {
					distance = 0.3f;
				}
				if (isMoveRight) {
					ggo.transform.position += speedVal * distance;
				} else {
					distance = Mathf.Abs (to.x - ggo.transform.position.x);
					ggo.transform.position -= speedVal;
				}
				if (ggo.transform.position.x > to.x - 1) {
					isMoveRight = false;
					yield return new WaitForSeconds (time);
				}
			}




			Destroy (ggo);
		}

		IEnumerator ShakeHorizontal (Transform tran, float duration)
		{
			float time = 0;
			if (tran == null || tran.transform == null || tran.gameObject == null) { yield break; }
			//Vector3 tranPos = tran.position;
			while (true) {
				yield return new WaitForSeconds (0.03f);

				time += Time.fixedDeltaTime;
				if (time > duration) {
					time = 0;
					break;
				} else {
					float y = Mathf.Sin (70 * time);
					if (tran == null) { yield break; }

					tran.position = new Vector3 (tran.position.x + y / 8, tran.position.y, tran.position.z);

				}
			}
			//if (tran == null) { yield break; }

			//tran.position = tranPos;
			yield break; ;
		}

		IEnumerator ShakeVertical (Transform tran, float duration)
		{
			float time = 0;

			//SpriteRenderer spriteRender = tran.GetComponent<SpriteRenderer> ();
			//Vector3 tranPos = tran.position;
			while (true) {
				yield return new WaitForSeconds (0.03f);


				time += Time.fixedDeltaTime;
				if (time > duration) {
					time = 0;
					break;
				} else {
					float y = Mathf.Sin (70 * time);
					if (tran == null) { yield break; }
					tran.position = new Vector3 (tran.position.x, tran.position.y + y / 8, tran.position.z);

				}
			}
			//if (tran == null) { yield break; }

			//tran.position = tran.position;
			yield break;
		}

		IEnumerator SpriteFlash (Transform tran, float duration)
		{
			float time = 0;
			if (tran == null) { yield break; }
			SpriteRenderer spriteRender = tran.GetComponent<SpriteRenderer> ();
			if (spriteRender == null) {
				Debug.Log ("Animation Service Warining: Sprite Flash Cant Find Sprite Render In Gameobject!");
				yield break;
			}
			while (true) {
				yield return new WaitForSeconds (0.03f);

				//if (spriteRender == null || spriteRender.material == null)
				// {
				//   yield return null;
				//}
				time += Time.fixedDeltaTime;
				if (time > duration) {
					time = 0;
					break;
				} else {
					float y = Mathf.Sin (70 * time);
					if (tran != null && spriteRender != null && spriteRender.material != null) {
						spriteRender.material.color = new Color (1, 1, 1, (y + 1) / 2);
					}
				}
			}
			if (tran != null && spriteRender != null && spriteRender.material != null) {
				spriteRender.material.color = new Color (1, 1, 1, 1);
			}

			yield return null;
		}


		IEnumerator ChildrenSpriteFlash (Transform tran, float duration)
		{
			float time = 0;

			bool nowShow = true;
			bool oldShow = nowShow;
			if (tran == null) { yield break; }
			SkinnedMeshRenderer [] meshs = tran.GetComponentsInChildren<SkinnedMeshRenderer> ();
			if (meshs == null) {
				Debug.Log ("Animation Service Warining: Sprite Flash Cant Find Sprite Render In Gameobject!");
				yield break;
			}
			while (true) {
				yield return new WaitForSeconds (0.03f);

				//if (spriteRender == null || spriteRender.material == null)
				// {
				//   yield return null;
				//}
				time += Time.fixedDeltaTime;
				if (time > duration) {
					time = 0;
					break;
				} else {
					float y = Mathf.Sin (70 * time);
					if (y >= 0) {
						nowShow = true;
					} else {
						nowShow = false;
					}
					if (nowShow != oldShow) {
						for (int i = 0; i < meshs.Length; i++) {
							if (tran != null && meshs [i] != null) {
								meshs [i].enabled = !meshs [i].enabled;
							}
						}
						oldShow = nowShow;
					}


				}
			}

			for (int i = 0; i < meshs.Length; i++) {
				if (tran != null && meshs [i] != null) {
					meshs [i].enabled = true;
				}
			}
			//meshs.material.color = new Color(1, 1, 1, 1);


			yield return null;
		}

		IEnumerator SpriteFade (Transform gameObj, float duration)
		{
			float time = 0;
			float interval = 0.03f;
			float increment = 1 / (duration / interval);

			SpriteRenderer spriteRender = gameObj.GetComponent<SpriteRenderer> ();
			if (gameObj == null || spriteRender == null || spriteRender.material == null) { yield break; }
			//Vector3 tranPos = gameObj.position;
			while (true) {
				yield return new WaitForSeconds (interval);
				time += Time.fixedDeltaTime;
				if (time > duration) {
					time = 0;
					break;
				} else {
					//float y = Mathf.Sin(70 * time);
					if (gameObj != null && spriteRender != null && spriteRender.material != null) {
						//gameObj.position = new Vector3(tranPos.x + y / 8, tranPos.y, tranPos.z);
						//Debug.L4og("spriteRender.size " + spriteRender.size);
						spriteRender.material.color -= new Color (0, 0, 0, increment);
					}

				}
			}

			if (gameObj != null && spriteRender != null && spriteRender.material != null) {
				spriteRender.material.color = new Color (1, 1, 1, 0);
				//gameObj.position = tranPos;
			}

			yield return null;
		}

		IEnumerator SpriteFadeOut (Transform gameObj, float duration)
		{
			float time = 0;
			float interval = 0.03f;
			float increment = 1 / (duration / interval);

			SpriteRenderer spriteRender = gameObj.GetComponent<SpriteRenderer> ();
			if (gameObj == null || spriteRender == null || spriteRender.material == null) { yield break; }
			//Vector3 tranPos = gameObj.position;
			while (true) {
				yield return new WaitForSeconds (interval);
				time += Time.fixedDeltaTime;
				if (time > duration) {
					time = 0;
					break;
				} else {
					//float y = Mathf.Sin(70 * time);
					if (gameObj != null && spriteRender != null && spriteRender.material != null) {
						//gameObj.position = new Vector3(tranPos.x + y / 8, tranPos.y, tranPos.z);
						//Debug.L4og("spriteRender.size " + spriteRender.size);
						spriteRender.material.color += new Color (0, 0, 0, increment);
					}

				}
			}

			if (gameObj != null && spriteRender != null && spriteRender.material != null) {
				spriteRender.material.color = new Color (1, 1, 1, 1);
				//gameObj.position = tranPos;
			}

			yield return null;
		}

		IEnumerator ImageFadeOut<T> (T source, float duration) where T : Graphic
		{
			float time = 0;
			float interval = 0.03f;
			float increment = 1 / (duration / interval);


			source.color = new Color (1, 1, 1, 0);
			if (source == null || source.material == null) { yield break; }
			//Vector3 tranPos = gameObj.position;
			while (true) {
				yield return new WaitForSeconds (interval);
				time += Time.fixedDeltaTime;
				if (time > duration) {
					time = 0;
					break;
				} else {
					//float y = Mathf.Sin(70 * time);
					if (source != null && source.material != null) {
						//gameObj.position = new Vector3(tranPos.x + y / 8, tranPos.y, tranPos.z);
						//Debug.L4og("spriteRender.size " + spriteRender.size);
						source.color += new Color (0, 0, 0, increment);
					}

				}
			}

			if (source != null && source.material != null) {
				source.color = new Color (1, 1, 1, 1);
				//gameObj.position = tranPos;
			}

			yield return null;
		}

	}

}
