using System;
using System.Collections.Generic;
using System.Timers;
using code.StarPlatinum.EventManager;
using Config.Data;
using StarPlatinum;
using StarPlatinum.EventManager;
using UI.Panels.Providers.DataProviders;
using UI.Panels.Providers.DataProviders.GameScene;
using UI.Panels.Providers.DataProviders.StaticBoard;
using UnityEngine;

namespace UI.Panels
{
    public enum EnumControversyStage
    {
        Begin,
        BeforeEntrance,
        Entrance,
        StageOne,
        StageOneLose,
        StageOneWin,
        StageTwo,
        StageTwoLose,
        Win,
        AfterWin,
        MissSpecial,
        Wrong,
    }
    public class BarrageItem
    {
        public int ID;
        public List<BarrageTextItem> Items;
        public bool IsSpecial;
        public float BornTime;
        public int CorrectIndex;
        public bool IsMoving;
    }

    public class BarrageTextItem
    {
        public int ID;
        public string Text;
        public bool IsHighLight;
        public int Index;
        public BarrageItem BarrageInfo;
    }
    public class JudgmentControversyModel: UIModel
    {
        #region template method
        public override void Initialize(IUiPanel uiPanel )
        {
            base.Initialize(uiPanel);
            var config = CommonConfig.Data;
            m_panel = (JudgmentControversyPanel) uiPanel;
            m_stageOneClearPoint = config.ControversyStageOnePoint;
            m_stageTwoClearPoint = config.ControversyStageTwoPoint1;
            m_heavyAttackInterval = config.ControversyHeavyAttackInterval;
            m_heavyAttackRecover = config.ControversyHeavyAttackRecover;
            m_normalAttackInterval = config.ControversyNormalAttackInterval;
        }

        public override void DeInitialize()
        {
            base.DeInitialize();
        }

        public override void Hide()
        {
            base.Hide();
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void ShowData(DataProvider data)
        {
            base.ShowData(data);
            var uiData = data as ControversyDataProvider;
            if (uiData == null)
            {
                Debug.LogError("争锋相对传入数据错误！！！！");
                return;
            }
            SetInfo(uiData.ID);
        }

        public override void UpdateData(DataProvider data)
        {
            base.UpdateData(data);
        }

        public override void Tick()
        {
            base.Tick();
            if (!IsBeginBarrage)
            {
                return;
            }

            if (IsHeavyAttack)
            {
                IsHeavyAttack = false;
                m_heavyAttackColdTime = m_heavyAttackInterval;
                Debug.LogError("========Quick Heavy");
                CheckCharge();
            }

            if (IsNormalAttack)
            {
                IsNormalAttack = false;
                m_normalAttackColdTime = m_normalAttackInterval;
            }
            
            if (StarPlatinum.Services.InputService.Instance.Input.Controversy.LightAttack.triggered && IsCharging)
            {
                IsHeavyAttack = true;
            }

            if (StarPlatinum.Services.InputService.Instance.Input.Controversy.HeavyAttack.triggered && m_normalAttackColdTime <= 0)
            {
                IsNormalAttack = true;
            }

            if (m_heavyAttackColdTime > 0)
            {
                m_heavyAttackColdTime -= Time.deltaTime;
                if (m_heavyAttackColdTime <= 0)
                {
                    m_heavyAttackColdTime = 0;
                    CheckCharge();
                }
                m_panel.m_lbl_heavyAttackCounter_TextMeshProUGUI.text = $"重击冷却:{m_heavyAttackColdTime:F1}秒";
            }

            if (m_normalAttackColdTime > 0)
            {
                m_normalAttackColdTime -= Time.deltaTime;
            }
            
            if (CurStage == EnumControversyStage.StageOne || CurStage == EnumControversyStage.StageTwo)
            {
                for (int i = 0; i < NormalBarrageInfos.Count; i++)
                {
                    if (NormalBarrageInfos[i].BornTime <= m_time && NormalBarrageInfos[i].IsMoving==false)
                    {
                        m_panel.SendBarrage(NormalBarrageInfos[i]);
                        NormalBarrageInfos[i].IsMoving = true;
                    }
                    else if(NormalBarrageInfos[i].IsMoving && !m_panel.m_barrageSubViews.ContainsKey(NormalBarrageInfos[i].ID))
                    {
                        NormalBarrageInfos.RemoveAt(i);
                        i--;
                    }
                }

                if (CurStage == EnumControversyStage.StageTwo)
                {
                    if (SpecialBarrageInfo.BornTime <= m_time && SpecialBarrageInfo.IsMoving==false)
                    {
                        m_panel.SendBarrage(SpecialBarrageInfo);
                        SpecialBarrageInfo.IsMoving = true;
                    }
                }
            }

            m_time += Time.deltaTime;
        }

        public override void LateTick()
        {
            base.LateTick();
        }

        public override void SubpanelChanged(UIPanelType type, DataProvider data = null)
        {
            base.SubpanelChanged(type,data);
        }

        public override void SubpanelDataChanged(UIPanelType type, DataProvider data)
        {
            base.SubpanelDataChanged(type,data);
        }
        #endregion
        
        public void ChangeStage(EnumControversyStage stage)
        {
            Debug.LogError($"ChangeStage {stage}");
            CurStage = stage;
            ResetData();
            switch (CurStage)
            {
                case EnumControversyStage.StageOne:
                    InitStageOneData();
                    break;
                case EnumControversyStage.StageTwo:
                    InitStageTwoData();
                    break;
                default:
                    break;
            }
            TimerService.Instance.AddTimer(0.01f, () =>
            {
                m_panel.ChangeStage();
            });
        }
        
        public bool IsStageClear()
        {
            int point ;
            switch (CurStage)
            {
                case EnumControversyStage.StageOne:
                    point = m_slashedBarrageAmount * 100 / TotalBarrageAmount;
                    return point >= m_stageOneClearPoint;
                case EnumControversyStage.StageTwo:
                    point = 100 - m_missBarrageCount * 100 / TotalBarrageAmount;
                    return point >= m_stageTwoClearPoint;
                default:
                    return true;
            }
        }

        public bool SlashBarrage(BarrageTextItem barrage)
        {
            if (barrage.BarrageInfo == SpecialBarrageInfo)
            {
                if (!IsHeavyAttack)
                {
                    return false;
                }

                if (barrage.Index == barrage.BarrageInfo.CorrectIndex)
                {
                    ChangeStage(EnumControversyStage.Win);
                    return true;
                }
                else
                {
                    SlashedSpecialIndex = barrage.Index;
                    ChangeStage(EnumControversyStage.Wrong);
                    return false;
                }
            }
            if (IsHeavyAttack)
            {
                SlashBarrageWithoutCheck(barrage);
                return true;
            }

            if (IsSlashed(barrage.ID))
            {
                return false;
            }
            if ( barrage.Index == barrage.BarrageInfo.CorrectIndex)
            {
                SlashBarrage(barrage.BarrageInfo);
                return true;
            }
            return false;
        }

        private void SlashBarrageWithoutCheck(BarrageTextItem barrage)
        {
            if (IsSlashed(barrage.ID))
            {
                return;
            }

            SlashBarrage(barrage.BarrageInfo);
        }

        private void SlashBarrage(BarrageItem barrage)
        {
            NormalBarrageInfos.Remove(barrage);
            Debug.LogError("SlashBarrage ");
            m_heavyAttackColdTime -= m_heavyAttackRecover;
            m_slashedBarrageAmount++;
            CheckCharge();
            m_slashedBarrageLst.Add(barrage.ID);
            if (CurStage != EnumControversyStage.StageTwo)
            {
                CheckStage();
            }
        }

        public bool BarragePassed(BarrageItem barrage)
        {
            if (IsSlashed(barrage.ID))
            {
                if (!NormalBarrageInfos.Contains(barrage))
                {
                    return false;
                }
                Debug.LogError($"BarragePassed IsSlashed but in normalBarrage {barrage.ID}");
            }
            else
            {
                m_slashedBarrageLst.Add(barrage.ID);
            }

            if (barrage.IsSpecial)
            {
                Debug.LogError("BarragePassed IsSpecial");

                ChangeStage(EnumControversyStage.MissSpecial);
            }
            else
            {
                m_missBarrageCount++;
                NormalBarrageInfos.Remove(barrage);
                if (NormalBarrageInfos.Count == 0)
                {
                    Debug.LogError("NormalBarrageInfos == 0 CheckStage");
                    CheckStage();
                }
                else
                {
                    Debug.LogError("NormalBarrageInfos != 0 CheckStage");

                    if (CurStage == EnumControversyStage.StageTwo)
                    {
                        if (IsStageClear() == false)
                        {
                            ChangeStage(EnumControversyStage.StageTwoLose);
                        }
                    }
                }
                
            }

            return true;
        }
        
        public bool IsSlashed(int barrageID)
        {
            return m_slashedBarrageLst.Contains(barrageID);
        }

        private void CheckCharge()
        {
            IsCharging = m_heavyAttackColdTime <= 0;
            m_panel.CheckCharge();
        }

        private void SetInfo(string controversyID)
        {
            m_controversyID = controversyID;
            ControversyConfig = ControversyConfig.GetConfigByKey(controversyID);
            if (!string.IsNullOrEmpty(ControversyConfig.storyFileName))
            {
                UiDataProvider.ControllerManager.StoryController.LoadStoryFileByName(ControversyConfig.storyFileName);
            }
            EnemyConfig = ControversyCharacterConfig.GetConfigByKey(ControversyConfig.leftCharacterID);

            var specialBarrageID = ControversyConfig.specialBarrageID;
            SpecialBarrageInfo = GetSpecialBarrageItem(specialBarrageID);
            SpecialBarrageConfig = ControversySpecialBarrageConfig.GetConfigByKey(SpecialBarrageInfo.ID);
        }

        private void CheckStage()
        {
            switch (CurStage)
            {
                case EnumControversyStage.StageOne:
                    if (NormalBarrageInfos.Count > 0)
                    {
                        return;
                    }
                    if (IsStageClear())
                    {
                        ChangeStage(EnumControversyStage.StageOneWin);
                    }
                    else
                    {
                        ChangeStage(EnumControversyStage.StageOneLose);
                    }
                    break;
                case EnumControversyStage.StageTwo:
                    if (SlashedSpecialIndex != 0)
                    {
                        ChangeStage(EnumControversyStage.Wrong);
                    }
                    else
                    {
                        ChangeStage(EnumControversyStage.MissSpecial);
                    }
                    break;
            }
        }

        private void InitStageOneData()
        {
            ResetData();
            var barrageInfos = GameRunTimeData.Instance.ControllerManager.ControversyController.GetBarrageInfoByGroup(ControversyConfig
                .stageOneBarrageGroupID);
            foreach (var barrageInfo in barrageInfos)
            {
                NormalBarrageInfos.Add(GetNormalBarrageItem(barrageInfo));
            }

            TotalBarrageAmount = NormalBarrageInfos.Count;
        }

        private void InitStageTwoData()
        {
            ResetData();

            var barrageInfos = GameRunTimeData.Instance.ControllerManager.ControversyController.GetBarrageInfoByGroup(ControversyConfig
                .stageTwoBarrageGroupID);
            foreach (var barrageInfo in barrageInfos)
            {
                NormalBarrageInfos.Add(GetNormalBarrageItem(barrageInfo));
            }
            TotalBarrageAmount = NormalBarrageInfos.Count;
        }

        private void ResetData()
        {            
            m_slashedBarrageLst.Clear();
            NormalBarrageInfos.Clear();
            m_slashedBarrageAmount = 0;
            m_time = 0;
            m_heavyAttackColdTime = m_heavyAttackInterval;
            m_normalAttackColdTime = m_normalAttackInterval;
            IsHeavyAttack = false;
            IsNormalAttack = false;
            IsCharging = false;
            SlashedSpecialIndex = 0;
            SpecialBarrageInfo.IsMoving = false;
            m_missBarrageCount = 0;
            IsBeginBarrage = false;
            m_panel.m_img_heavyAttackCounter_Image.gameObject.SetActive(false);

            Debug.LogError("Clear Data");
        }

        private BarrageItem GetSpecialBarrageItem(int specialBarrageID)
        {
            BarrageItem item = new BarrageItem(){ID = specialBarrageID,IsSpecial = true};
            var specialBarrageConfig = ControversySpecialBarrageConfig.GetConfigByKey(specialBarrageID);
            item.Items = ProcessBarrageText(item, specialBarrageConfig.text);
            item.CorrectIndex = specialBarrageConfig.correctIndex;
            item.BornTime = specialBarrageConfig.bornTime;
            return item;
        }

        private BarrageItem GetNormalBarrageItem(ControversyBarrageConfig barrageConfig)
        {
            BarrageItem item = new BarrageItem(){ID = barrageConfig.ID,IsSpecial = false};

            item.Items = ProcessBarrageText(item, barrageConfig.text);
            item.CorrectIndex = barrageConfig.correctIndex;
            item.BornTime = barrageConfig.bornTime;
            return item;
        }
        
        private List<BarrageTextItem> ProcessBarrageText(BarrageItem barrageItem, string text)
        {
            List<BarrageTextItem> items = new List<BarrageTextItem>();
            bool isBeginWithHighlight = text.StartsWith("HightLight_{");
            string[] strs = text.Split(new string[] {"HightLight_{"},StringSplitOptions.RemoveEmptyEntries);
            int index = 1;
            for (int i = 0; i < strs.Length; i++)
            {
                if (i == 0 && !isBeginWithHighlight)
                {
                    items.Add(new BarrageTextItem(){ID = barrageItem.ID,Index = 0,IsHighLight = false, Text = strs[i],BarrageInfo = barrageItem});
                    continue;
                }
                var specialStr = strs[i].Substring(0,strs[i].IndexOf('}'));
                items.Add(new BarrageTextItem(){ID = barrageItem.ID,Index = index++,IsHighLight = true, Text = specialStr,BarrageInfo = barrageItem});
                
                var normalStr = strs[i].Substring(strs[i].IndexOf('}')+1);
                if (normalStr.Length > 0)
                {
                    items.Add(new BarrageTextItem() {ID = barrageItem.ID, Index = 0, IsHighLight = false, Text = normalStr,BarrageInfo = barrageItem});
                }
                
            }

            return items;
        }
        
        #region Member
        public List<BarrageItem> NormalBarrageInfos { get; private set; } = new List<BarrageItem>();
        public BarrageItem SpecialBarrageInfo;
        public ControversySpecialBarrageConfig SpecialBarrageConfig;
        public ControversyConfig ControversyConfig;
        public ControversyCharacterConfig EnemyConfig;
        public bool IsBeginBarrage;
        public int TotalBarrageAmount;


        public EnumControversyStage CurStage;
        public bool IsCharging { get; private set; }
        public bool IsHeavyAttack;
        public bool IsNormalAttack;
        public int SlashedSpecialIndex;


        private JudgmentControversyPanel m_panel;
        private string m_controversyID;
        private int m_slashedBarrageAmount;
        private int m_stageOneClearPoint;
        private int m_stageTwoClearPoint;
        private int m_missBarrageCount;
        private List<int> m_slashedBarrageLst = new List<int>();
        private List<BarrageItem> SentBarrageInfos = new List<BarrageItem>();
        private float m_time=0;
        private float m_normalAttackInterval;
        private float m_heavyAttackInterval;
        private float m_heavyAttackRecover;

        public float m_normalAttackColdTime;
        public float m_heavyAttackColdTime;

        #endregion
    }
}