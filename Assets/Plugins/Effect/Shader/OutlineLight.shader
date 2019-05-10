// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Skylight / 2D / OutlineLight" {

	Properties{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_BlendTex("Blend Texture",2D) = "white"{}
		_Opacity("Blend Opacity",Range(0,1)) = 1
	}
	
	SubShader{
			ZTest Always
			ZWrite Off
			Cull Off

		Pass
		{

		CGPROGRAM
		#pragma vertex vert_img
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest
		#include "UnityCG.cginc"
		uniform sampler2D _MainTex;
		uniform sampler2D _BlendTex;
		fixed _Opacity;


			struct v2f {
				float4 pos : SV_POSITION;
				half2 uv: TEXCOORD0;
			};
		
			v2f vert(appdata_img v) {
				v2f o;
		
				o.pos = UnityObjectToClipPos(v.vertex);
		
				o.uv = v.texcoord;
		
				return o;
			}
		

		fixed4 frag(v2f i) : SV_Target
		{
			fixed4 renderTex = tex2D(_MainTex,i.uv);
			fixed4 blendTex = tex2D(_BlendTex,i.uv);

		//	blendedMultiply = renderTex * blendTex;
			fixed4 blendedScreen = (1.0 - ((1.0 - renderTex) * (1.0 - blendTex)));//这里是颜色计算核心

			//renderTex = renderTex* _Opacity;
			renderTex = lerp(renderTex, blendedScreen,_Opacity);

			return renderTex;
		}
		ENDCG
		}

	}

			Fallback Off
}
//Shader "Skylight / 2D / OutlineLight" {
//	Properties{
//		_MainTex("Base (RGB)", 2D) = "white" {}
//		_Brightness("Brightness", Float) = 1
//		_Saturation("Saturation", Float) = 1
//		_Contrast("Contrast", Float) = 1
//	}
//		 
//		ZTest Always Cull Off ZWrite Off
//
//		CGPROGRAM
//#pragma vertex vert  
//#pragma fragment frag  
//
//#include "UnityCG.cginc"  
//
//		sampler2D _MainTex;
//	half _Brightness;
//	half _Saturation;
//	half _Contrast;
//
//	struct v2f {
//		float4 pos : SV_POSITION;
//		half2 uv: TEXCOORD0;
//	};
//
//	v2f vert(appdata_img v) {
//		v2f o;
//
//		o.pos = UnityObjectToClipPos(v.vertex);
//
//		o.uv = v.texcoord;
//
//		return o;
//	}
//
//	fixed4 frag(v2f i) : SV_Target{
//		fixed4 renderTex = tex2D(_MainTex, i.uv);
//
//	 Apply brightness
//	fixed3 finalColor = renderTex.rgb * _Brightness;
//
//	 Apply saturation
//	fixed luminance = 0.2125 * renderTex.r + 0.7154 * renderTex.g + 0.0721 * renderTex.b;
//	fixed3 luminanceColor = fixed3(luminance, luminance, luminance);
//	finalColor = lerp(luminanceColor, finalColor, _Saturation);
//
//	 Apply contrast
//	fixed3 avgColor = fixed3(0.5, 0.5, 0.5);
//	finalColor = lerp(avgColor, finalColor, _Contrast);
//
//	return fixed4(finalColor, renderTex.a);
//	}
//
//		ENDCG
//	}
//	}
//
//		Fallback Off
//}