using System.Collections;
using System.Collections.Generic;
using code.StarPlatinum.EventManager;
using Config.Data;
using StarPlatinum;
using StarPlatinum.EventManager;
using UI.Panels.Element;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UI.Panels.Providers.DataProviders.StaticBoard;

namespace UI.Panels
{
	public partial class JudgmentControversyPanel : UIPanel<UIDataProvider, DataProvider>
	{
		#region UI template method
		public override void Initialize (UIDataProvider uiDataProvider, UIPanelSettings settings)
		{
			base.Initialize (uiDataProvider, settings);
			m_model.Initialize(this);
			
			InitMoveInfo();
			m_UI_Judgment_ControversyCharactor_Item.Init(m_UI_Judgment_ControversyCharactor_Item.GetComponent<RectTransform>());
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
			EventManager.Instance.AddEventListener<ControversyBarrageSlashEvent>(OnSlashed);

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
			EventManager.Instance.AddEventListener<ControversyBarrageSlashEvent>(OnSlashed);
			SetInfo();
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
			if (!m_model.IsBeginBarrage)
			{
				return;
			}

			foreach (var barrageSubView in m_barrageSubViews)
			{
				barrageSubView.Value.transform.Translate(m_speed * Time.deltaTime*Vector3.left);
				if (barrageSubView.Value.MovingTime >= m_totalMoveTime * 4)
				{
					CallbackTime(0.01f, () =>
					{
						RecycleBarrageSubview(barrageSubView.Key);
					});
				}else if (barrageSubView.Value.MovingTime >= m_totalMoveTime &&
				          !m_model.IsSlashed(barrageSubView.Key) && barrageSubView.Value.IsPassed(m_go_cordon.position))
				{
					if (m_model.BarragePassed(barrageSubView.Value.Info))
					{
						MoveCordon(false);
					}
				}

				barrageSubView.Value.MovingTime += Time.deltaTime;
			}
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

		public void SetInfo()
		{
			PrefabManager.Instance.SetImage(m_img_enemy_Image,m_model.EnemyConfig.IdleTachieKey);
			m_model.ChangeStage(EnumControversyStage.Begin);
			m_UI_Judgment_ControversyCharactor_Item.SetStatus(EnumCharacterStatus.Idle);
		}
		
		public void ChangeStage()
		{
			switch (m_model.CurStage)
			{
				case EnumControversyStage.Begin:
					ShowScreenAnimation();
					break;
				case EnumControversyStage.Entrance:
					ShowScreenAnimation();
					break;
				case EnumControversyStage.StageOne:
					m_model.IsBeginBarrage = true;
					m_cordonMoveDisdance = m_dictance / 2 / m_model.TotalBarrageAmount;
					ResetCordon();
					break;
				case EnumControversyStage.StageOneLose:
					ClearStage();
					m_model.IsBeginBarrage = false;
					ShowTalkPanel(m_model.ControversyConfig.stageOneFailStoryID);
					break;
				case EnumControversyStage.StageOneWin:
					ClearStage();
					m_model.IsBeginBarrage = false;
					break;
				case EnumControversyStage.StageTwo:
					m_model.IsBeginBarrage = true;
					m_cordonMoveDisdance = m_dictance / 2 / m_model.TotalBarrageAmount;
					ResetCordon();
					break;
				case EnumControversyStage.Wrong:
					ClearStage();
					string storyID;
					switch (m_model.SlashedSpecialIndex)
					{
						case 1:
							storyID = m_model.SpecialBarrageConfig.wrongStoryID1;
							break;
						case 2:
							storyID = m_model.SpecialBarrageConfig.wrongStoryID2;
							break;
						case 3:
							storyID = m_model.SpecialBarrageConfig.wrongStoryID3;
							break;
						case 4:
							storyID = m_model.SpecialBarrageConfig.wrongStoryID4;
							break;
						default:
							storyID = m_model.SpecialBarrageConfig.wrongStoryID1;
							break;
					}
					ShowTalkPanel(storyID);
					break;
				case EnumControversyStage.MissSpecial:
					ClearStage();
					ShowTalkPanel(m_model.SpecialBarrageConfig.missStoryID);
					break;
				case EnumControversyStage.Win:
					ClearStage();
					ShowTalkPanel(m_model.ControversyConfig.winStoryID);
					break;
			}
		}

		private void ShowTalkPanel(string id)
		{
			UIManager.Instance().ShowStaticPanel(UIPanelType.TalkPanel,new TalkDataProvider(){ID = id, OnTalkEnd = TalkCallback});
		}

		private void TalkCallback()
		{
			switch (m_model.CurStage)
			{
				case EnumControversyStage.Wrong:
				case EnumControversyStage.MissSpecial:
				case EnumControversyStage.StageOneLose:
				case EnumControversyStage.StageOneWin:
					ShowScreenAnimation();
					break;
				case EnumControversyStage.Win:
					//TODO:破论 回调ShowScreenAnimation
					break;
			}
		}
		
		public void CheckCharge()
		{
			if (m_model.IsCharging)
			{
				m_UI_Judgment_ControversyCharactor_Item.SetStatus(EnumCharacterStatus.Charge);
			}
			else
			{
				m_UI_Judgment_ControversyCharactor_Item.SetStatus(EnumCharacterStatus.Idle);
			}
		}

		private void MoveCordon(bool isRight)
		{
			Vector3 direction;
			if (isRight)
			{
				direction = Vector3.right;
				m_curCordonMoveValue++;
			}
			else
			{
				direction = Vector3.left;
				m_curCordonMoveValue--;
			}
			var scale = (m_model.TotalBarrageAmount - m_curCordonMoveValue) / (float) m_model.TotalBarrageAmount;
			if (scale < 0.5f)
			{
				return;
			}else if (scale > 1.5f)
			{
				return;
			}
			var distance = direction * m_cordonMoveDisdance;
			m_pl_line.transform.Translate(distance);
			if (m_curCordonMoveValue > 0)
			{
				m_go_enemy.localScale =Vector3.one*scale;
			}
		}

		private void ResetCordon()
		{
			m_curCordonMoveValue = 0;
			m_pl_line.anchoredPosition = Vector2.zero;
			m_go_enemy.localScale =Vector3.one;
		}
		
		//显示屏风特效
		private void ShowScreenAnimation()
		{
			switch (m_model.CurStage)
			{
				case EnumControversyStage.Begin:
					gameObject.SetActive(false);
					//TODO:破论
					CallbackTime(4, () =>
					{
						m_model.ChangeStage(EnumControversyStage.Entrance);
					});
					break;
				case EnumControversyStage.Entrance:
					gameObject.SetActive(true);
					PrefabManager.Instance.SetImage(m_img_screenLeft_Image,UIRes.ScreenLeftBegin);
					PrefabManager.Instance.SetImage(m_img_screenRight_Image,m_model.EnemyConfig.entranceScreenKey);
					CallbackTime(4, () =>
					{
						m_model.ChangeStage(EnumControversyStage.StageOne);
					});
					break;
				case EnumControversyStage.StageOneLose:
					PrefabManager.Instance.SetImage(m_img_screenLeft_Image,UIRes.ScreenLeftLose);
					PrefabManager.Instance.SetImage(m_img_screenRight_Image,m_model.EnemyConfig.winScreenKey);
					CallbackTime(4, () =>
					{
						m_model.ChangeStage(EnumControversyStage.StageOne);
					});
					break;
				case EnumControversyStage.Wrong:
				case EnumControversyStage.MissSpecial:
					PrefabManager.Instance.SetImage(m_img_screenLeft_Image,UIRes.ScreenLeftLose);
					PrefabManager.Instance.SetImage(m_img_screenRight_Image,m_model.EnemyConfig.winScreenKey);
					CallbackTime(4, () =>
					{
						m_model.ChangeStage(EnumControversyStage.StageTwo);
					});
					break;
				case EnumControversyStage.Win:
					PrefabManager.Instance.SetImage(m_img_screenLeft_Image,UIRes.ScreenLeftWin);
					PrefabManager.Instance.SetImage(m_img_screenRight_Image,UIRes.ScreenRightWin);
					CallbackTime(4, () =>
					{
						InvokeHidePanel();
					});
					break;
			}
		}

		private void InitMoveInfo()
		{
			m_totalMoveTime = CommonConfig.Data.ControversyBarrageMoveSpeed;
			m_dictance = m_go_cordon.transform.position.x - m_go_beginLine1.position.x;
			m_speed = m_dictance / m_totalMoveTime;

			m_cordonMoveDisdance = m_dictance / 2;
		}

		private void RecycleBarrageSubview(int barrageID)
		{
			if (m_barrageSubViews.ContainsKey(barrageID))
			{
				m_barrageSubViews[barrageID].gameObject.SetActive(false);
				m_freeSubViews.Enqueue(m_barrageSubViews[barrageID]);
				m_barrageSubViews.Remove(barrageID);
			}
		}
		
		public void SendBarrage(BarrageItem itemInfo)
		{
			if (m_freeSubViews.Count > 0)
			{
				var subview = m_freeSubViews.Dequeue();
				SendBarrage(subview,itemInfo);
			}
			else
			{
				PrefabManager.Instance.InstantiateAsync<UI_Judgment_ControversyBarrage_Item_SubView>(
					UI_Judgment_ControversyBarrage_Item_SubView.VIEW_NAME,
					(result) =>
					{
						if (result.status == RequestStatus.FAIL)
						{
							return;
						}

						var subview = result.result as UI_Judgment_ControversyBarrage_Item_SubView;
						subview.Init(subview.GetComponent<RectTransform>());
						SendBarrage(subview,itemInfo);
					});
			}
			
		}

		private void SendBarrage(UI_Judgment_ControversyBarrage_Item_SubView subView,BarrageItem itemInfo)
		{
			Transform parent;
			int track;
			if (itemInfo.IsSpecial)
			{
				track = ControversySpecialBarrageConfig.GetConfigByKey(itemInfo.ID).track;
			}
			else
			{
				track = ControversyBarrageConfig.GetConfigByKey(itemInfo.ID).track;
			}

			switch (track)
			{
				case 1:
					parent = m_go_beginLine1;
					break;
				case 2:
					parent = m_go_beginLine2;
					break;
				case 3:
					parent = m_go_beginLine3;
					break;
				case 4:
					parent = m_go_beginLine4;
					break;
				default:
					parent = m_go_beginLine1;
					break;
			}
			subView.transform.SetParent(parent);
			subView.transform.localScale = Vector3.one;
			subView.transform.localPosition = Vector3.zero;
			subView.SetInfo(itemInfo);
			if (!m_barrageSubViews.ContainsKey(itemInfo.ID))
			{
				m_barrageSubViews.Add(itemInfo.ID,subView);
			}
		}

		private void ClearStage()
		{
			foreach (var barrageSubView in m_barrageSubViews)
			{
				barrageSubView.Value.gameObject.SetActive(false);
				m_freeSubViews.Enqueue(barrageSubView.Value);
			}
			m_barrageSubViews.Clear();
			m_model.Clear();
		}

		private void OnSlashed(object sender, ControversyBarrageSlashEvent e)
		{
			var info = e.SubView.BarrageInfo;

			if (m_model.IsSlashed(info.ID))
			{
				return;
			}
			var isSuccess = m_model.SlashBarrage(info);
			m_barrageSubViews[info.ID].Slash();

			if (isSuccess)
			{
				MoveCordon(true);
			}
			//TODO:播放斩击动画
			
			if (m_model.IsHeavyAttack)
			{
				m_model.IsHeavyAttack = false;
			}
		}

		
		#region Member
		
		private Dictionary<int,UI_Judgment_ControversyBarrage_Item_SubView> m_barrageSubViews = new Dictionary<int,UI_Judgment_ControversyBarrage_Item_SubView>();

		private Queue<UI_Judgment_ControversyBarrage_Item_SubView> m_freeSubViews = new Queue<UI_Judgment_ControversyBarrage_Item_SubView>();
		private float m_totalMoveTime;
		private float m_dictance;
		private float m_speed;
		private float m_cordonMoveDisdance;
		private int m_curCordonMoveValue;

		#endregion
	}
}