Shader "Hologram/Hologram_B"
{
	Properties
	{
		_Noise_Texture("Noise_Texture", 2D) = "white" {}
		_BaseTexture("BaseTexture", 2D) = "white" {}
		[HDR]_Tint("Tint", Color) = (0,0,0,0)
		_Noise_Speed("Noise_Speed", Vector) = (0,0,0,0)
		_Opacity_Intensity("Opacity_Intensity", Range( 0 , 5)) = 2
		[Toggle(_CUSTOMBASETEXTTINT_ON)] _CustomBaseTextTint("CustomBaseTextTint", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Off
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _CUSTOMBASETEXTTINT_ON
		struct Input
		{
			float2 uv_texcoord;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float _Opacity_Intensity;
		uniform sampler2D _Noise_Texture;
		uniform float2 _Noise_Speed;
		uniform float4 _Noise_Texture_ST;
		uniform sampler2D _BaseTexture;
		uniform float4 _BaseTexture_ST;
		uniform float4 _Tint;

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float mrliunxj_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			mrliunxj_lightAtten = 0;
			#else
			float3 mrliunxj_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float mrliunxj_lightAtten = max( max( mrliunxj_lightAttenRGB.r, mrliunxj_lightAttenRGB.g ), mrliunxj_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			mrliunxj_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float2 uv_Noise_Texture = i.uv_texcoord * _Noise_Texture_ST.xy + _Noise_Texture_ST.zw;
			float2 panner_temp = ( 1.0 * _Time.y * _Noise_Speed + uv_Noise_Texture);
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 
			float4 mrliunxj_lightColor = 0;
			#else 
			float4 mrliunxj_lightColor = _LightColor0;
			#endif 
			float4 temp_output = saturate( ( _WorldSpaceLightPos0.w * ( mrliunxj_lightAtten * mrliunxj_lightColor ) ) );
			float2 uv_BaseTexture = i.uv_texcoord * _BaseTexture_ST.xy + _BaseTexture_ST.zw;
			float4 blenderTex = tex2D( _BaseTexture, uv_BaseTexture );
			float4 temp_cast_0 = (blenderTex.r).xxxx;
			#ifdef _CUSTOMBASETEXTTINT_ON
				float4 customTint = temp_cast_0;
			#else
				float4 customTint = blenderTex;
			#endif
			float4 temp_output_1 = ( ( tex2D( _Noise_Texture, panner_temp ) * temp_output ) * ( customTint * temp_output ) );
			float grayColor = Luminance(temp_output_1.rgb);
			c.rgb = ( _Tint * temp_output_1 ).rgb;
			c.a = ( _Opacity_Intensity * grayColor );
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting alpha:fade keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				UnityGI gi;
				UNITY_INITIALIZE_OUTPUT( UnityGI, gi );
				o.Alpha = LightingStandardCustomLighting( o, worldViewDir, gi ).a;
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	
}
