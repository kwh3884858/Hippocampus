using UnityEngine;
using Evidence;
using Tips;
using StarPlatinum;
using StarPlatinum.Base;

/// <summary>
/// 核心配置数据类
/// </summary>
public class ConfigData : Singleton<ConfigData>
{

	/// <summary>配置表路径</summary>
	public const string LocalTextFilePath = "Storys/ExhibitTable";
	public const string LocalTipsFilePath = "Storys/TipsTable";
    public const string LocalCharacterFilePath = "Storys/CharacterTable";
	/// <summary>由Json转换后的对象体</summary>
	GameConfig configPackage = null;

	/// <summary>证据表配置数据</summary>
	public EvidenceConfig evidenceConfig { get; private set; }
	/// <summary>tips表配置数据</summary>
	public TipsConfig tipsConfig { get; private set; }
    /// <summary>Character Table</summary>
    public CharacterTable characterTable { get; private set; }
	/// <summary>是否就绪</summary>
	public bool isInitialized { get; private set; }

	public ConfigData ()
	{
		Init ("");
	}

	/// <summary>
	/// 配置数据初始化
	/// </summary>
	public void Init (string vConfigText)
	{
		// 总配置转换
		TextAsset vConfigAsset = null;
		if (string.IsNullOrEmpty (vConfigText)) {
			vConfigAsset = Resources.Load (LocalTextFilePath) as TextAsset;// 从本地取
			vConfigText = vConfigAsset.text;
		}
		configPackage = JsonUtility.FromJson<GameConfig> (vConfigText);
		evidenceConfig = new EvidenceConfig (configPackage.value);
		// 读取tips
		vConfigAsset = Resources.Load (LocalTipsFilePath) as TextAsset;// 从本地取
		vConfigText = vConfigAsset.text;
		tipsConfig = new TipsConfig (JsonUtility.FromJson<GameTipsConfig> (vConfigText).value);

        // Read Character Table
        vConfigAsset = Resources.Load(LocalCharacterFilePath) as TextAsset;
        characterTable = new CharacterTable(vConfigAsset.text);

        // 内存释放
        if (vConfigAsset != null) {
			Resources.UnloadAsset (vConfigAsset);
		}
		isInitialized = true;
	}

	/// <summary>
	/// JSON内容对应的数据结构
	/// </summary>
	public class GameConfig
	{

		public EvidenceConfig.Detail [] value;
	}

	/// <summary>
	/// tips js内容对于的数据结构
	/// </summary>
	public class GameTipsConfig
	{
		public TipsConfig.Detail [] value;
	}
}
