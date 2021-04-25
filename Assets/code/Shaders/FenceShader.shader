Shader "Custom/FenceShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_MainTex2("Texture2", 2D) = "white" {}
		_StartFlag("开始标记", float) = 0
		_SpeedFactor("速度",Range(0.01,10)) = 0.1
		_StartTime("时间初始标记，不要手动设置",float) = 1
		_Column("百叶窗的列数",float) = 5
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				// make fog work
				#pragma multi_compile_fog

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;

				sampler2D _MainTex2;
				float4 _MainTex2_ST;

				float _StartFlag;
				float _Column;
				float _SpeedFactor;
				float _StartTime;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = 0;
				//_StartFlag 通过C#监听键盘事件，设置为1，作为开始动画的标记
				//step(a,b) => if(b>=a) return 1 else return 0
				//i.uv.x % (1/_Column) i.uv.x范围是0-1,分成_Column份 每份（1/_Column）
				//fixed result = _StartFlag * step(i.uv.x % (1 / _Column) ,(_Time.y - _StartTime) * _SpeedFactor);
				fixed result = _StartFlag * step(i.uv.x % (1 / _Column) , _StartTime * _SpeedFactor);

				if (result == 0)
				{
					col = tex2D(_MainTex, i.uv);
				}
				else
				{
					col = tex2D(_MainTex2, i.uv);
				}

				return col;
			}
			ENDCG
		}
		}
}