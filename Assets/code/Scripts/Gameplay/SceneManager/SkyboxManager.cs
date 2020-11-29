using System.Collections;
using System.Collections.Generic;
using StarPlatinum.Base;
using UnityEngine;

[System.Serializable]
public enum SkyboxEnum
{
    None,
    Default,
    Rainday,
    Sunday,
    Twinight_01,
    Twinight_02,
    Twinight_03,

};

public class SkyboxManager : MonoSingleton<SkyboxManager>
{
    [SerializeField]
    public Material[] skyboxs;
    public override void SingletonInit()
    {
        m_skyboxAssetDic.Add(SkyboxEnum.Default, skyboxs[0] != null ? skyboxs[0] : null);
        m_skyboxAssetDic.Add(SkyboxEnum.Rainday, skyboxs[1] != null ? skyboxs[1] : null);
        m_skyboxAssetDic.Add(SkyboxEnum.Sunday, skyboxs[2] != null ? skyboxs[2] : null);
        m_skyboxAssetDic.Add(SkyboxEnum.Twinight_01, skyboxs[3] != null ? skyboxs[3] : null);
        m_skyboxAssetDic.Add(SkyboxEnum.Twinight_02, skyboxs[4] != null ? skyboxs[4] : null);
        m_skyboxAssetDic.Add(SkyboxEnum.Twinight_03, skyboxs[5] != null ? skyboxs[5] : null);
    }
    public void UpdateSkybox(SkyboxEnum skyEnum)
    {
        m_skyboxAssetDic.TryGetValue(skyEnum, out var skyboxMaterial);
        if (skyboxMaterial)
        {
            m_currentSkybox = skyEnum;
            RenderSettings.skybox = skyboxMaterial;
            DynamicGI.UpdateEnvironment();
        }
        else
        {
            Debug.LogWarning("no sky materials");
        }

        if (skyEnum == SkyboxEnum.None)
        {
            RevertSkybox();
        }
    }

    public void RefreshSkyboxSettingAfterNewGameSceneIsLoaded(Material skyboxMaterial)
    {
        m_originalSkybox = skyboxMaterial;
        if (m_currentSkybox != SkyboxEnum.None)
        {
            UpdateSkybox(m_currentSkybox);
        }
    }

    public bool IsSkyboxOverrided()
    {
        return m_currentSkybox != SkyboxEnum.None; 
    }

    private void RevertSkybox()
    {
        RenderSettings.skybox = m_originalSkybox;
        DynamicGI.UpdateEnvironment();
    }

    private Dictionary<SkyboxEnum, Material> m_skyboxAssetDic = new Dictionary<SkyboxEnum, Material>();
    private SkyboxEnum m_currentSkybox = SkyboxEnum.None;
    private Material m_originalSkybox = null;
}
