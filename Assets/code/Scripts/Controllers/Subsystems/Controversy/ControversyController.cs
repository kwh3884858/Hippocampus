using System.Collections.Generic;
using Config.Data;
using LocalCache;
using StarPlatinum;
using StarPlatinum.EventManager;
using UnityEngine;

namespace Controllers.Subsystems
{

    public class ControversyController: ControllerBase
    {
        public override void Initialize(IControllerProvider args)
        {
            base.Initialize(args);
            InitBarrageInfo();
        }

        public override void Tick()
        {
            base.Tick();
        }

        public override void Terminate()
        {
            base.Terminate();
        }

        private void InitBarrageInfo()
        {
            var barrageInfos = ControversyBarrageConfig.GetAllConfig();

            foreach (var barrageInfo in barrageInfos)
            {
                if (!m_normalBarrageInfo.ContainsKey(barrageInfo.Value.groupID))
                {
                    m_normalBarrageInfo.Add(barrageInfo.Value.groupID,new List<ControversyBarrageConfig>());
                }
                m_normalBarrageInfo[barrageInfo.Value.groupID].Add(barrageInfo.Value);
            }
        }

        public List<ControversyBarrageConfig> GetBarrageInfoByGroup(int groupID)
        {
            if (!m_normalBarrageInfo.ContainsKey(groupID))
            {
                Debug.LogError($"找不到普通弹幕组ID ：{groupID}");
                return null;
            }
            return m_normalBarrageInfo[groupID];

        }

        private Dictionary<int, List<ControversyBarrageConfig>> m_normalBarrageInfo =new Dictionary<int, List<ControversyBarrageConfig>>();
    }
}