// FT Shader EditorV1.0
// (c) 2019-2020

Shader "FT Shader/Role/Skin"
{
	Properties
	{
	[FTHeaderHelp(BASE, Base Properties)]
		//TOONY COLORS
		_Color ("Color", Color) = (1,1,1,1)
		_HColor ("Highlight Color", Color) = (0.785,0.785,0.785,1.0)
		_SColor ("Shadow Color", Color) = (0.195,0.195,0.195,1.0)
		_STexture ("Shadow Color Texture", 2D) = "white" {}

		//DIFFUSE
		_MainTex ("Main Texture", 2D) = "white" {}
	[FTSeparator]

		//TOONY COLORS RAMP
		[FTHeader(RAMP SETTINGS)]

		[FTGradient] _Ramp			("Toon Ramp (RGB)", 2D) = "gray" {}
	[FTSeparator]

	[Header(Masks)]
		[NoScaleOffset]
		_Mask1 ("Mask 1 (Specular)", 2D) = "black" {}
	[FTSeparator]

	[FTHeaderHelp(NORMAL MAPPING, Normal Bump Map)]
		//BUMP
		_BumpMap ("Normal map (RGB)", 2D) = "bump" {}
		_BumpScale ("Scale", Float) = 1.0
	[FTSeparator]

	[FTHeaderHelp(SPECULAR, Specular)]
		//SPECULAR
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Smoothness ("Smoothness", Range(0,1)) = 0.5
	[FTSeparator]

	[FTHeaderHelp(RIM, Rim)]
		//RIM LIGHT
		_RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.6)
		_RimMin ("Rim Min", Range(0,2)) = 0.5
		_RimMax ("Rim Max", Range(0,2)) = 1.0
		//RIM DIRECTION
		_RimDir ("Rim Direction", Vector) = (0.0,0.0,1.0,0.0)
	[FTSeparator]

	[FTHeaderHelp(OUTLINE, Outline)]
		//OUTLINE
		_OutlineColor ("Outline Color", Color) = (0.2, 0.2, 0.2, 1.0)
		_Outline ("Outline Width", Float) = 1

		//Outline Textured
		[Toggle(FT_OUTLINE_TEXTURED)] _EnableTexturedOutline ("Color from Texture", Float) = 0
		[FTKeywordFilter(FT_OUTLINE_TEXTURED)] _TexLod ("Texture LOD", Range(0,10)) = 5

		//Constant-size outline
		[Toggle(FT_OUTLINE_CONST_SIZE)] _EnableConstSizeOutline ("Constant Size Outline", Float) = 0

		//ZSmooth
		[Toggle(FT_ZSMOOTH_ON)] _EnableZSmooth ("Correct Z Artefacts", Float) = 0
		//Z Correction & Offset
		[FTKeywordFilter(FT_ZSMOOTH_ON)] _ZSmooth ("Z Correction", Range(-3.0,3.0)) = -0.5
		[FTKeywordFilter(FT_ZSMOOTH_ON)] _Offset1 ("Z Offset 1", Float) = 0
		[FTKeywordFilter(FT_ZSMOOTH_ON)] _Offset2 ("Z Offset 2", Float) = 0

		//This property will be ignored and will draw the custom normals GUI instead
		[FTOutlineNormalsGUI] __outline_gui_dummy__ ("_unused_", Float) = 0
	[FTSeparator]


		//Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}

	SubShader
	{

		Tags { "RenderType"="Opaque" }

		CGPROGRAM

		#pragma surface surf ToonyColorsCustom addshadow fullforwardshadows vertex:vert exclude_path:deferred exclude_path:prepass
		#pragma target 3.0

		//================================================================
		// VARIABLES

		fixed4 _Color;
		sampler2D _MainTex;
		sampler2D _STexture;
		sampler2D _Mask1;
		sampler2D _BumpMap;
		half _BumpScale;
		fixed _Smoothness;
		fixed4 _RimColor;
		fixed _RimMin;
		fixed _RimMax;
		float4 _RimDir;

		#define UV_MAINTEX uv_MainTex

		struct Input
		{
			half2 uv_MainTex;
			half2 uv_BumpMap;
			float3 bViewDir;
		};

		//================================================================
		// CUSTOM LIGHTING

		//Lighting-related variables
		fixed4 _HColor;
		fixed4 _SColor;
		sampler2D _Ramp;

		//Specular help functions (from UnityStandardBRDF.cginc)
		inline half3 SafeNormalize(half3 inVec)
		{
			half dp3 = max(0.001f, dot(inVec, inVec));
			return inVec * rsqrt(dp3);
		}

	  //Fresnel
		float CalcFresnel(float3 viewDir, float3 halfVec, float fresnelValue)
		{
			float fresnel = pow(1.0 - dot(viewDir, halfVec), 5.0);
			fresnel += fresnelValue * (1.0 - fresnel);
			return fresnel;
		}

		// Instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		//Custom SurfaceOutput
		struct SurfaceOutputCustom
		{
			half atten;
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			half Specular;
			fixed Gloss;
			fixed Alpha;
			fixed3 ShadowColorTex;
		};

		inline half4 LightingToonyColorsCustom (inout SurfaceOutputCustom s, half3 viewDir, UnityGI gi)
		{
		#define IN_NORMAL s.Normal
	
			half3 lightDir = gi.light.dir;
		#if defined(UNITY_PASS_FORWARDBASE)
			half3 lightColor = _LightColor0.rgb;
			half atten = s.atten;
		#else
			half3 lightColor = gi.light.color.rgb;
			half atten = 1;
		#endif

			IN_NORMAL = normalize(IN_NORMAL);
			fixed ndl = max(0, dot(IN_NORMAL, lightDir) * 0.5 + 0.5);
			#define NDL ndl


			#define		RAMP_TEXTURE	_Ramp

			fixed3 ramp = tex2D(RAMP_TEXTURE, fixed2(NDL,NDL)).rgb;
		#if !(POINT) && !(SPOT)
			ramp *= atten;
		#endif
			//Shadow Color Texture
			s.Albedo = lerp(s.ShadowColorTex.rgb, s.Albedo, ramp);
		#if !defined(UNITY_PASS_FORWARDBASE)
			_SColor = fixed4(0,0,0,1);
		#endif
			_SColor = lerp(_HColor, _SColor, _SColor.a);	//Shadows intensity through alpha
			ramp = lerp(_SColor.rgb, _HColor.rgb, ramp);
			//Blinn-Phong Specular (legacy)
			half3 h = normalize(lightDir + viewDir);
			float ndh = max(0, dot (IN_NORMAL, h));
			float spec = pow(ndh, s.Specular*128.0) * s.Gloss * 2.0;
			spec *= atten;
			fixed4 c;
			c.rgb = s.Albedo * lightColor.rgb * ramp;
		#if (POINT || SPOT)
			c.rgb *= atten;
		#endif

			#define SPEC_COLOR	_SpecColor.rgb
			c.rgb += lightColor.rgb * SPEC_COLOR * spec;
			c.a = s.Alpha;

		#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
			c.rgb += s.Albedo * gi.indirect.diffuse;
		#endif

			return c;
		}

		void LightingToonyColorsCustom_GI(inout SurfaceOutputCustom s, UnityGIInput data, inout UnityGI gi)
		{
			gi = UnityGlobalIllumination(data, 1.0, IN_NORMAL);

			s.atten = data.atten;	//transfer attenuation to lighting function
			gi.light.color = _LightColor0.rgb;	//remove attenuation
		}

		//Vertex input
		struct appdata_ft
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
			float4 tangent : TANGENT;
	
			UNITY_VERTEX_INPUT_INSTANCE_ID
	
		};

		//================================================================
		// VERTEX FUNCTION

		inline float3 FT_ObjSpaceViewDir( in float4 v )
		{
			float3 camPos = _WorldSpaceCameraPos;
			camPos += mul(_RimDir, UNITY_MATRIX_V).xyz;
			float3 objSpaceCameraPos = mul(unity_WorldToObject, float4(camPos, 1)).xyz;
			return objSpaceCameraPos - v.xyz;
		}

		void vert(inout appdata_ft v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			TANGENT_SPACE_ROTATION;
			o.bViewDir = mul(rotation, FT_ObjSpaceViewDir(v.vertex));
		}

		//================================================================
		// SURFACE FUNCTION

		void surf(Input IN, inout SurfaceOutputCustom o)
		{
			fixed4 mainTex = tex2D(_MainTex, IN.UV_MAINTEX);

			//Masks
			fixed4 mask1 = tex2D(_Mask1, IN.UV_MAINTEX);

			//Shadow Color Texture
			fixed4 shadowTex = tex2D(_STexture, IN.UV_MAINTEX);
			o.ShadowColorTex = shadowTex.rgb;
			o.Albedo = mainTex.rgb * _Color.rgb;
			o.Alpha = mainTex.a * _Color.a;

			//Specular
			_Smoothness = 1 - _Smoothness;	//smoothness to roughness
			o.Gloss = mask1.r;
			o.Specular = _Smoothness;

			//Normal map
			half4 normalMap = tex2D(_BumpMap, IN.uv_BumpMap.xy);
			o.Normal = UnpackScaleNormal(normalMap, _BumpScale);

			//Rim
			float3 viewDir = normalize(IN.bViewDir);
			half rim = 1.0f - saturate( dot(viewDir, o.Normal) );
			rim = smoothstep(_RimMin, _RimMax, rim);
			o.Emission += (_RimColor.rgb * rim) * _RimColor.a;
		}

		ENDCG
		//Outline
		UsePass "FT Shader/Outline Only/OUTLINE"
	}

	Fallback "Diffuse"
	CustomEditor "FT_MaterialInspector_SG"
}
