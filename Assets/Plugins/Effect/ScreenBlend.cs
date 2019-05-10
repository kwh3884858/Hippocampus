using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBlend : PostEffectsBase
{



	public Shader m_shader;
	private Material m_material;

	[Range (0.0f, 3.0f)]
	public float m_opacity;
	public Texture2D m_blendTexture;


	public Material material {
		get {
			m_material = CheckShaderAndCreateMaterial (m_shader, m_material);

			return m_material;
		}
	}

	private void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		if (material != null) {
			material.SetFloat ("_Opacity", m_opacity);
			material.SetTexture ("_BlendTex", m_blendTexture);

			Graphics.Blit (source, destination, material);
		} else {
			Graphics.Blit (source, destination);
		}
	}
}
