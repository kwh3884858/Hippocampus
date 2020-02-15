using UnityEngine;
using Evidence;
using StarPlatinum;

/// <summary>
/// 核心配置数据类
/// </summary>
public class ConfigData : Singleton<ConfigData>
{

    /// <summary>配置表路径</summary>
    public const string LocalTextFilePath = "Storys/ExhibitTable";
    /// <summary>由Json转换后的对象体</summary>
    GameConfig configPackage = null;

    /// <summary>证据表配置数据</summary>
    public EvidenceConfig evidenceConfig { get; private set; }
    /// <summary>是否就绪</summary>
    public bool isInitialized { get; private set; }

    public ConfigData()
    {
        Init("");
    }

    /// <summary>
    /// 配置数据初始化
    /// </summary>
    public void Init(string vConfigText)
    {
        // 总配置转换
        TextAsset vConfigAsset = null;
        if (string.IsNullOrEmpty(vConfigText))
        {
            vConfigAsset = Resources.Load(LocalTextFilePath) as TextAsset;// 从本地取
            vConfigText = vConfigAsset.text;
        }
        configPackage = JsonUtility.FromJson<GameConfig>(vConfigText);
        evidenceConfig = new EvidenceConfig(configPackage.value);
        // 内存释放
        if (vConfigAsset != null)
        {
            Resources.UnloadAsset(vConfigAsset);
        }
        isInitialized = true;
    }

    /// <summary>
    /// JSON内容对应的数据结构
    /// </summary>
    public class GameConfig
    {

        public EvidenceConfig.Detail[] value;
    }

}
