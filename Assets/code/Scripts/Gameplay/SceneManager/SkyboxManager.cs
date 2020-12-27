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
    public class SkyboxConfig
    {
        public SkyboxEnum m_skyboxEnum;

        public Material m_material;

        public Color32 m_environmentColor;
    }

    public SkyboxEnum[] m_skyboxEnums;
    public Material[] m_materials;
    public Color32[] m_environmentColors;

    [SerializeField]
    public List<SkyboxConfig> m_skyboxs = new List<SkyboxConfig>();

    //public List<SkyboxConfig> m_skyboxs = new List<SkyboxConfig>();

    public override void SingletonInit()
    {
        //m_skyboxAssetDic.Add(SkyboxEnum.Default, skyboxs[0] != null ? skyboxs[0] : null);
        //m_skyboxAssetDic.Add(SkyboxEnum.Rainday, skyboxs[1] != null ? skyboxs[1] : null);
        //m_skyboxAssetDic.Add(SkyboxEnum.Sunday, skyboxs[2] != null ? skyboxs[2] : null);
        //m_skyboxAssetDic.Add(SkyboxEnum.Twinight_01, skyboxs[3] != null ? skyboxs[3] : null);
        //m_skyboxAssetDic.Add(SkyboxEnum.Twinight_02, skyboxs[4] != null ? skyboxs[4] : null);
        //m_skyboxAssetDic.Add(SkyboxEnum.Twinight_03, skyboxs[5] != null ? skyboxs[5] : null);
        for (int i = 0; i < m_skyboxEnums.Length; i++)
        {
            m_skyboxs.Add(new SkyboxConfig() { m_skyboxEnum = m_skyboxEnums[i], m_environmentColor = m_environmentColors[i], m_material = m_materials[i] });

        }
    }
    public void UpdateSkybox(SkyboxEnum skyEnum)
    {
        Debug.Log("Update Skybox:  " + skyEnum);

        if (skyEnum == SkyboxEnum.None)
        {
            RevertSkybox();
            return;
        }

        foreach (var item in m_skyboxs)
        {
            if (skyEnum == item.m_skyboxEnum)
            {
                m_currentSkyboxEnum = item.m_skyboxEnum;
                RenderSettings.skybox = item.m_material;
                RenderSettings.ambientLight = item.m_environmentColor;
                DynamicGI.UpdateEnvironment();

                return;
            }
        }
        Debug.LogWarning("no sky materials");

        //m_skyboxs.TryGetValue(skyEnum, out var skyboxMaterial);
        //if (skyboxMaterial)
        //{
        //    m_currentSkybox = skyEnum;
        //    RenderSettings.skybox = skyboxMaterial;
        //    DynamicGI.UpdateEnvironment();
        //}
        //else
        //{
        //    Debug.LogWarning("no sky materials");
        //}

        //if (skyEnum == SkyboxEnum.None)
        //{
        //    RevertSkybox();
        //}
    }

    public void RefreshSkyboxSettingAfterNewGameSceneIsLoaded(Material skyboxMaterial, Color ambientColor)
    {
        m_originalSkybox = skyboxMaterial;
        m_originalEnvironmentColor = ambientColor;

        foreach (var item in m_skyboxs)
        {
            if (m_originalSkybox == item.m_material)
            {
                m_currentSkyboxEnum = item.m_skyboxEnum;
            }
        }
        //foreach (var material in m_skyboxAssetDic)
        //{
        //    if (m_originalSkybox==material.Value)
        //    {
        //        m_currentSkyboxEnum = material.Key;
        //    }
        //}
        if (m_currentSkyboxEnum != SkyboxEnum.None)
        {
            UpdateSkybox(m_currentSkyboxEnum);
        }
        else
        {
            Debug.LogError("can not find the correct skybox when loading the scene");
        }
    }

    public bool IsSkyboxOverrided()
    {
        return m_currentSkyboxEnum != SkyboxEnum.None; 
    }

    private void RevertSkybox()
    {
        RenderSettings.skybox = m_originalSkybox;
        RenderSettings.ambientLight = m_originalEnvironmentColor;
        DynamicGI.UpdateEnvironment();
    }

    //private List<SkyboxConfig> m_skyboxAssetDic = new List<SkyboxConfig>();
    private SkyboxEnum m_currentSkyboxEnum = SkyboxEnum.None;
    private Material m_originalSkybox = null;
    private Color m_originalEnvironmentColor = Color.gray;
}
