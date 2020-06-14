// Upgrade NOTE: replaced 'UNITY_PASS_TEXCUBE(unity_SpecCube1)' with 'UNITY_PASS_TEXCUBE_SAMPLER(unity_SpecCube1,unity_SpecCube0)'

// FT Shader EditorV1.0
// (c) 2019-2020

Shader "FT Shader/Role/Equip10"
{
	Properties
	{
	[FTHeaderHelp(BASE, Base Properties)]
		//TOONY COLORS
		_Color ("Color", Color) = (1,1,1,1)
		_HColor ("Highlight Color", Color) = (0.785,0.785,0.785,1.0)
		_SColor ("Shadow Color", Color) = (0.195,0.195,0.195,1.0)
		_STexture ("Shadow Color Texture", 2D) = "white" {}
		_HighlightMultiplier ("Highlight Multiplier", Range(0,4)) = 1
		_ShadowMultiplier ("Shadow Multiplier", Range(0,4)) = 1
		_WrapFactor ("Light Wrapping", Range(-1,3)) = 1.0

		//DIFFUSE
		_MainTex ("Main Texture", 2D) = "white" {}

		[Header(Texture Blending (Texture Map))]
		_TexBlendMap ("Texture Blending Map", 2D) = "black" {}
		[Space]
		_BlendTex1 ("Texture 1 (r)", 2D) = "white" {}
		_DiffTint ("Diffuse Tint", Color) = (0.7,0.8,1,1)
	[FTSeparator]

		//TOONY COLORS RAMP
		[FTHeader(RAMP SETTINGS)]

		_RampThreshold ("Ramp Threshold", Range(0,1)) = 0.5
		_RampSmooth ("Ramp Smoothing", Range(0.001,1)) = 0.1
	[FTSeparator]

	[Header(Masks)]
		[NoScaleOffset]
		_Mask1 ("Mask 1 (Specular)", 2D) = "black" {}
		[NoScaleOffset]
		_Mask2 ("Mask 2 (Reflection)", 2D) = "black" {}
		[NoScaleOffset]
		_Mask3 ("Mask 3 (Emission)", 2D) = "black" {}
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
		_SpecSmooth ("Toon Smoothness", Range(0,1)) = 0.05
	[FTSeparator]

	[FTHeaderHelp(REFLECTION, Reflection)]
		//REFLECTION
		//[NoScaleOffset] _Cube ("Reflection Cubemap", Cube) = "_Skybox" {}

		_ReflSmoothness ("Reflection Smoothness", Range(0.0,1.0)) = 1
		//_ReflectRoughness ("Reflection Roughness", Range(0,9)) = 0
		_ReflectColor ("Reflection Color (RGB) Strength (Alpha)", Color) = (1,1,1,0.5)
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
		[HDR] _OutlineColor ("Outline Color", Color) = (0.2, 0.2, 0.2, 1.0)
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
		//Blending
		[FTHeader(OUTLINE BLENDING)]
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlendOutline ("Blending Source", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlendOutline ("Blending Dest", Float) = 10
	[FTSeparator]

	[FTHeaderHelp(TRANSPARENCY)]
		//Blending
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlendFT ("Blending Source", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlendFT ("Blending Dest", Float) = 10
		//Alpha Testing
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	[FTSeparator]


		//Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}

	SubShader
	{
		//================================================================
		// OUTLINE INCLUDE

		CGINCLUDE

		#include "UnityCG.cginc"

		struct a2v
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
	#if FT_OUTLINE_TEXTURED
			float3 texcoord : TEXCOORD0;
	#endif
		#if FT_COLORS_AS_NORMALS
			float4 color : COLOR;
		#endif
	#if FT_UV2_AS_NORMALS
			float2 uv2 : TEXCOORD1;
	#endif
	#if FT_TANGENT_AS_NORMALS
			float4 tangent : TANGENT;
	#endif
	
			UNITY_VERTEX_INPUT_INSTANCE_ID
	
		};

		struct v2f
		{
			float4 pos : SV_POSITION;
	#if FT_OUTLINE_TEXTURED
			float3 texlod : TEXCOORD1;
	#endif
		};

		float _Outline;
		float _ZSmooth;
		half4 _OutlineColor;

	#if FT_OUTLINE_TEXTURED
		sampler2D _MainTex;
		float4 _MainTex_ST;
		float _TexLod;
	#endif

		#define OUTLINE_WIDTH _Outline

		v2f FT_Outline_Vert(a2v v)
		{
			v2f o;	
			//GPU instancing support
			UNITY_SETUP_INSTANCE_ID(v);


	#if FT_ZSMOOTH_ON
			float4 pos = float4(UnityObjectToViewPos(v.vertex), 1.0);
	#endif

	#ifdef FT_COLORS_AS_NORMALS
			//Vertex Color for Normals
			float3 normal = (v.color.xyz*2) - 1;
	#elif FT_TANGENT_AS_NORMALS
			//Tangent for Normals
			float3 normal = v.tangent.xyz;
	#elif FT_UV2_AS_NORMALS
			//UV2 for Normals
			float3 n;
			//unpack uv2
			v.uv2.x = v.uv2.x * 255.0/16.0;
			n.x = floor(v.uv2.x) / 15.0;
			n.y = frac(v.uv2.x) * 16.0 / 15.0;
			//get z
			n.z = v.uv2.y;
			//transform
			n = n*2 - 1;
			float3 normal = n;
	#else
			float3 normal = v.normal;
	#endif

	#if FT_ZSMOOTH_ON
			//Correct Z artefacts
			normal = UnityObjectToViewPos(normal);
			normal.z = -_ZSmooth;
	#endif

	#ifdef FT_OUTLINE_CONST_SIZE
			//Camera-independent outline size
			float dist = distance(_WorldSpaceCameraPos, mul(unity_ObjectToWorld, v.vertex));
			#define SIZE	dist
	#else
			#define SIZE	1.0
	#endif

	#if FT_ZSMOOTH_ON
			o.pos = mul(UNITY_MATRIX_P, pos + float4(normalize(normal),0) * OUTLINE_WIDTH * 0.01 * SIZE);
	#else
			o.pos = UnityObjectToClipPos(v.vertex + float4(normal,0) * OUTLINE_WIDTH * 0.01 * SIZE);
	#endif

	#if FT_OUTLINE_TEXTURED
			half2 uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.texlod = tex2Dlod(_MainTex, float4(uv, 0, _TexLod)).rgb;
	#endif

			return o;
		}

		#define OUTLINE_COLOR _OutlineColor

		float4 FT_Outline_Frag (v2f IN) : SV_Target
		{
	#if FT_OUTLINE_TEXTURED
			return float4(IN.texlod, 1) * OUTLINE_COLOR;
	#else
			return OUTLINE_COLOR;
	#endif
		}

		ENDCG

		// OUTLINE INCLUDE END
		//================================================================

		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Blend [_SrcBlendFT] [_DstBlendFT]
		Cull Off
		CGPROGRAM

		#pragma surface surf ToonyColorsCustom addshadow fullforwardshadows vertex:vert keepalpha exclude_path:deferred exclude_path:prepass
		#pragma target 3.5

		//================================================================
		// VARIABLES

		fixed4 _Color;
		sampler2D _MainTex;
		sampler2D _STexture;
		sampler2D _Mask1;
		sampler2D _Mask2;
		sampler2D _Mask3;
		sampler2D _TexBlendMap;
		sampler2D _BlendTex1;
		float4 _BlendTex1_ST;
		fixed _ReflSmoothness;
		//fixed _ReflectRoughness;
		fixed4 _ReflectColor;
		sampler2D _BumpMap;
		half _BumpScale;
		fixed _Smoothness;
		fixed4 _RimColor;
		fixed _RimMin;
		fixed _RimMax;
		float4 _RimDir;
		fixed _Cutoff;

		#define UV_MAINTEX uv_MainTex

		struct Input
		{
			half2 uv_MainTex;
			half2 uv_TexBlendMap;
			half2 uv_BumpMap;
			float3 worldPos;
			float3 worldRefl;
			float3 worldNormal;
			INTERNAL_DATA
			float3 bViewDir;
		};

		//================================================================
		// CUSTOM LIGHTING

		//Lighting-related variables
		fixed4 _HColor;
		fixed4 _SColor;
		half _WrapFactor;
		fixed _HighlightMultiplier;
		fixed _ShadowMultiplier;
		half _RampThreshold;
		half _RampSmooth;
		fixed _SpecSmooth;
		fixed4 _DiffTint;

		//Specular help functions (from UnityStandardBRDF.cginc)
		inline half3 SafeNormalize(half3 inVec)
		{
			half dp3 = max(0.001f, dot(inVec, inVec));
			return inVec * rsqrt(dp3);
		}


		//PBR Blinn-Phong
		inline half PercRoughnessToSpecPower(half roughness)
		{
			half sq = max(1e-4f, roughness*roughness);
			half n = (2.0 / sq) - 2.0;
			n = max(n, 1e-4f);
			return n;
		}
		inline half NDFBlinnPhong(half NdotH, half n)
		{
			// norm = (n+2)/(2*pi)
			half normTerm = (n + 2.0) * (0.5/UNITY_PI);

			half specTerm = pow (NdotH, n);
			return specTerm * normTerm;
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
		fixed ndl = max(0, dot(IN_NORMAL, lightDir));
			 ndl = max(0, (dot(IN_NORMAL, lightDir) + _WrapFactor) / (1+_WrapFactor));
			#define NDL ndl

			#define		RAMP_THRESHOLD	_RampThreshold
			#define		RAMP_SMOOTH		_RampSmooth

			fixed3 ramp = smoothstep(RAMP_THRESHOLD - RAMP_SMOOTH*0.5, RAMP_THRESHOLD + RAMP_SMOOTH*0.5, NDL);
		#if !(POINT) && !(SPOT)
			ramp *= atten;
		#endif
			//Shadow Color Texture
			s.Albedo = lerp(s.ShadowColorTex.rgb, s.Albedo, ramp);
		#if !defined(UNITY_PASS_FORWARDBASE)
			_SColor = fixed4(0,0,0,1);
		#endif
			_SColor = lerp(_HColor, _SColor, _SColor.a * _ShadowMultiplier);	//Shadows intensity through alpha
			_HColor *= _HighlightMultiplier;
			ramp = lerp(_SColor.rgb, _HColor.rgb, ramp);
			fixed3 wrappedLight = saturate(_DiffTint.rgb + saturate(dot(IN_NORMAL, lightDir)));
			ramp *= wrappedLight;
			//Specular: PBR Blinn-Phong
			half3 halfDir = SafeNormalize(lightDir + viewDir);
			half roughness = s.Specular*s.Specular;
			half nh = saturate(dot(IN_NORMAL, halfDir));
			half spec = NDFBlinnPhong(nh, PercRoughnessToSpecPower(roughness)) * s.Gloss;
			spec = smoothstep(0.5-_SpecSmooth*0.5, 0.5+_SpecSmooth*0.5, spec);
			spec *= atten;

			//s.Specular = spec;

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

		#if defined(UNITY_PASS_FORWARDADD)
			//multiply RGB with alpha for additive lights for proper transparency behavior
			c.rgb *= c.a;
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
			fixed4 mask2 = tex2D(_Mask2, IN.UV_MAINTEX);
			fixed4 mask3 = tex2D(_Mask3, IN.UV_MAINTEX);

			//Texture Blending
			#define MAIN_UV IN.UV_MAINTEX
			float4 texblend_map = tex2D(_TexBlendMap, IN.uv_TexBlendMap);
			#define BLEND_SOURCE texblend_map

			fixed4 tex1 = tex2D(_BlendTex1, MAIN_UV * _BlendTex1_ST.xy + _BlendTex1_ST.zw);

			mainTex = lerp(mainTex, tex1, BLEND_SOURCE.r);

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

			//Reflection
			half3 eyeVec = IN.worldPos.xyz - _WorldSpaceCameraPos.xyz;
			half3 worldNormal = reflect(eyeVec, WorldNormalVector(IN, o.Normal));
			float oneMinusRoughness = _ReflSmoothness;
			fixed3 reflColor = fixed3(0,0,0);
		#if UNITY_SPECCUBE_BOX_PROJECTION
			half3 worldNormal0 = BoxProjectedCubemapDirection (worldNormal, IN.worldPos, unity_SpecCube0_ProbePosition, unity_SpecCube0_BoxMin, unity_SpecCube0_BoxMax);
		#else
			half3 worldNormal0 = worldNormal;
		#endif
			half3 env0 = Unity_GlossyEnvironment (UNITY_PASS_TEXCUBE(unity_SpecCube0), unity_SpecCube0_HDR, worldNormal0, oneMinusRoughness);

		#if UNITY_SPECCUBE_BLENDING
			const float kBlendFactor = 0.99999;
			float blendLerp = unity_SpecCube0_BoxMin.w;
			UNITY_BRANCH
			if (blendLerp < kBlendFactor)
			{
			#if UNITY_SPECCUBE_BOX_PROJECTION
				half3 worldNormal1 = BoxProjectedCubemapDirection (worldNormal, IN.worldPos, unity_SpecCube1_ProbePosition, unity_SpecCube1_BoxMin, unity_SpecCube1_BoxMax);
			#else
				half3 worldNormal1 = worldNormal;
			#endif

				half3 env1 = Unity_GlossyEnvironment (UNITY_PASS_TEXCUBE_SAMPLER(unity_SpecCube1,unity_SpecCube0), unity_SpecCube1_HDR, worldNormal1, oneMinusRoughness);
				reflColor = lerp(env1, env0, blendLerp);//_ReflectRoughness
			}
			else
			{
				reflColor = env0;
			}
		#else
			reflColor = env0;
		#endif
		//	reflColor *= 0.5;
			reflColor *= mask2.r;
			reflColor.rgb *= _ReflectColor.rgb * _ReflectColor.a;
			o.Emission += reflColor.rgb;

			//Rim
			float3 viewDir = normalize(IN.bViewDir);
			half rim = 1.0f - saturate( dot(viewDir, o.Normal) );
			rim = smoothstep(_RimMin, _RimMax, rim);
			o.Albedo = lerp(o.Albedo.rgb, _RimColor.rgb, rim * _RimColor.a);
			o.ShadowColorTex = lerp(o.ShadowColorTex.rgb, _RimColor.rgb, rim * _RimColor.a);

			//Emission
			half3 emissiveColor = half3(1,1,1);
			emissiveColor *= mainTex.rgb * mask3.r;
			o.Emission += emissiveColor;
		}

		ENDCG

		//Outline
		Pass
		{
			Cull Front
			Offset [_Offset1],[_Offset2]

			Tags { "LightMode"="ForwardBase" "Queue"="Transparent" "IgnoreProjectors"="True" "RenderType"="Transparent" }
			Blend [_SrcBlendOutline] [_DstBlendOutline]

			CGPROGRAM

			#pragma vertex FT_Outline_Vert
			#pragma fragment FT_Outline_Frag

			#pragma multi_compile FT_NONE FT_ZSMOOTH_ON
			#pragma multi_compile FT_NONE FT_OUTLINE_CONST_SIZE
			#pragma multi_compile FT_NONE FT_COLORS_AS_NORMALS FT_TANGENT_AS_NORMALS FT_UV2_AS_NORMALS
			#pragma multi_compile FT_NONE FT_OUTLINE_TEXTURED			
			#pragma multi_compile_instancing


			#pragma target 3.5

			ENDCG
		}

		//Shadow Caster (for shadows and depth texture)
		Pass
		{
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }

			CGPROGRAM

			#include "UnityCG.cginc"
			#pragma vertex vertShadowCaster
			#pragma fragment fragShadowCaster
			#pragma multi_compile_shadowcaster
			#pragma multi_compile_instancing


			half4		_Color;
			half		_Cutoff;
			sampler2D	_MainTex;
			float4		_MainTex_ST;
			sampler3D	_DitherMaskLOD;

			struct VertexInput
			{
				float4 vertex	: POSITION;
				float3 normal	: NORMAL;
				float2 uv0		: TEXCOORD0;
		
				UNITY_VERTEX_INPUT_INSTANCE_ID
		
			};

			struct VertexOutputShadowCaster
			{
				V2F_SHADOW_CASTER_NOPOS
				float2 tex : TEXCOORD1;
			};

			void vertShadowCaster(VertexInput v, out VertexOutputShadowCaster o, out float4 opos : SV_POSITION)
			{
				
				TRANSFER_SHADOW_CASTER_NOPOS(o,opos)
				o.tex = TRANSFORM_TEX(v.uv0, _MainTex);
			}

			half4 fragShadowCaster(VertexOutputShadowCaster i, UNITY_VPOS_TYPE vpos : VPOS) : SV_Target
			{
				half alpha = tex2D(_MainTex, i.tex).a * _Color.a;
				// Use dither mask for alpha blended shadows, based on pixel position xy
				// and alpha level. Our dither texture is 4x4x16.
				half alphaRef = tex3D(_DitherMaskLOD, float3(vpos.xy*0.25,alpha*0.9375)).a;
				clip (alphaRef - 0.01);

				SHADOW_CASTER_FRAGMENT(i)
			}

			ENDCG
		}
Pass
		{		
			Blend SrcAlpha  OneMinusSrcAlpha
			ZWrite Off
			Cull Back
			//ColorMask RGB
			
			Stencil
			{
				Ref 0			
				Comp Equal			
				WriteMask 255		
				ReadMask 255
				//Pass IncrSat
				Pass Invert
				Fail Keep
				ZFail Keep
			}
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag

			float4 _ShadowPlane;
			float4 _ShadowProjDir;
			float4 _WorldPos;
			float _ShadowInvLen;
			float4 _ShadowFadeParams;
			float _ShadowFalloff;
			
			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2ff
			{
				float4 vertex : SV_POSITION;
				float3 xlv_TEXCOORD0 : TEXCOORD0;
				float3 xlv_TEXCOORD1 : TEXCOORD1;
			};

			v2ff vert(appdata v)
			{
				v2ff o;

				float3 lightdir = normalize(_ShadowProjDir);
				float3 worldpos = mul(unity_ObjectToWorld, v.vertex).xyz;
				// _ShadowPlane.w = p0 * n  // 平面的w分量就是p0 * n
				float distance = (_ShadowPlane.w - dot(_ShadowPlane.xyz, worldpos)) / dot(_ShadowPlane.xyz, lightdir.xyz);
				worldpos = worldpos + distance * lightdir.xyz;
				o.vertex = mul(unity_MatrixVP, float4(worldpos, 1.0));
				o.xlv_TEXCOORD0 = _WorldPos.xyz;
				o.xlv_TEXCOORD1 = worldpos;

				return o;
			}
			
			float4 frag(v2ff i) : SV_Target
			{
				float3 posToPlane_2 = (i.xlv_TEXCOORD0 - i.xlv_TEXCOORD1);
				float4 color;
				color.xyz = float3(0.0, 0.0, 0.0);

				color.w = (pow((1.0 - clamp(((sqrt(dot(posToPlane_2, posToPlane_2)) * _ShadowInvLen) - _ShadowFadeParams.x), 0.0, 1.0)), _ShadowFadeParams.y) * _ShadowFadeParams.z);

				return color;
			}
			
			ENDCG
		}
Pass
		{		
			Blend SrcAlpha  OneMinusSrcAlpha
			ZWrite Off
			Cull Back
			//ColorMask RGB
			
			Stencil
			{
				Ref 0			
				Comp Equal			
				WriteMask 255		
				ReadMask 255
				//Pass IncrSat
				Pass Invert
				Fail Keep
				ZFail Keep
			}
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag

			float4 _ShadowPlane;
			float4 _ShadowProjDir;
			float4 _WorldPos;
			float _ShadowInvLen;
			float4 _ShadowFadeParams;
			float _ShadowFalloff;
			
			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2ff
			{
				float4 vertex : SV_POSITION;
				float3 xlv_TEXCOORD0 : TEXCOORD0;
				float3 xlv_TEXCOORD1 : TEXCOORD1;
			};

			v2ff vert(appdata v)
			{
				v2ff o;

				float3 lightdir = normalize(_ShadowProjDir);
				float3 worldpos = mul(unity_ObjectToWorld, v.vertex).xyz;
				// _ShadowPlane.w = p0 * n  // 平面的w分量就是p0 * n
				float distance = (_ShadowPlane.w - dot(_ShadowPlane.xyz, worldpos)) / dot(_ShadowPlane.xyz, lightdir.xyz);
				worldpos = worldpos + distance * lightdir.xyz;
				o.vertex = mul(unity_MatrixVP, float4(worldpos, 1.0));
				o.xlv_TEXCOORD0 = _WorldPos.xyz;
				o.xlv_TEXCOORD1 = worldpos;

				return o;
			}
			
			float4 frag(v2ff i) : SV_Target
			{
				float3 posToPlane_2 = (i.xlv_TEXCOORD0 - i.xlv_TEXCOORD1);
				float4 color;
				color.xyz = float3(0.0, 0.0, 0.0);

				color.w = (pow((1.0 - clamp(((sqrt(dot(posToPlane_2, posToPlane_2)) * _ShadowInvLen) - _ShadowFadeParams.x), 0.0, 1.0)), _ShadowFadeParams.y) * _ShadowFadeParams.z);

				return color;
			}
			
			ENDCG
		}
	}

	Fallback "Diffuse"
	CustomEditor "FT_MaterialInspector_SG"
}
