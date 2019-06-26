// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Shadow/BaseShadowMapShader"
{
	Properties
	{
		_LightMaps("Camera Tex", 2D) = "white" {}
		//_MainLightMaps("MainLightMaps", 2D) = "white" {}
		//_SecondLightMaps("SecondLightMaps", 2D) = "white" {}
		//_ThirdLightMaps("ThirdLightMaps", 2D) = "white" {}
		_ShadowColor("Shadow Color",Color)=(0,0,0,0)
		_Color ("Color Tint", Color) = (1, 1, 1, 1)
		_MainTex ("Main Tex", 2D) = "white" {}
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_BumpScale ("Bump Scale", Float) = 1.0
		_Specular ("Specular", Color) = (1, 1, 1, 1)
		_Gloss ("Gloss", Range(8.0, 256)) = 20
	}
		SubShader
		{
			Tags { "LightMode" = "ShadowCaster" }
			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
				};


				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					discard;
					return 0;
				}
				ENDCG
			}
		}
	SubShader
	{
		Tags{ "RenderType"="Opaque" }

		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex object_vert
			#pragma fragment object_frag

			#pragma multi_compile MAINCAMERAON MAINCAMERAOFF
			#pragma multi_compile SECONDCAMERAON SECONDCAMERAOFF
			#pragma multi_compile THIRDCAMERAON	THIRDCAMERAOFF 
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BumpMap;
			float4 _BumpMap_ST;
			float _BumpScale;
			fixed4 _Specular;
			float _Gloss;

			sampler2D _LightMaps;
			float4 _LightMaps_ST;

			sampler2D _MainLightMaps;
			sampler2D _SecondLightMaps;
			sampler2D _ThirdLightMaps;
			uniform half4 _MainTex_TexelSize;


			sampler2D _MainCameraDepthTexture;
			sampler2D _SecondCameraDepthTexture;
			sampler2D _ThirdCameraDepthTexture;

			float4x4 _MainLightProjection;
			float4x4 _SecondLightProjection;
			float4x4 _ThirdLightProjection;

			fixed4 _ShadowColor;

			float4 _MainCameraWorldPos;
			float4 _SecondCameraWorldPos;
			float4 _ThirdCameraWorldPos;
			float _TexturePixelWidth;
			float _TexturePixelHeight;
			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 texcoord : TEXCOORD0;
				float4 worldPos : TEXCOORD1;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 worldPos : TEXCOORD0;
				float4 uv : TEXCOORD1;
				float3 lightDir : TEXCOORD2;
				float3 viewDir : TEXCOORD3;
				float3 cameraDir : TEXCOORD4;
				float3 worldnormal : TEXCOORD5;
			};

			float4x4 inverse(float4x4 input) {
				#define minor(a,b,c) determinant(float3x3(input.a, input.b, input.c))

				float4x4 cofactors = float4x4(
					minor(_22_23_24, _32_33_34, _42_43_44),
					-minor(_21_23_24, _31_33_34, _41_43_44),
					minor(_21_22_24, _31_32_34, _41_42_44),
					-minor(_21_22_23, _31_32_33, _41_42_43),

					-minor(_12_13_14, _32_33_34, _42_43_44),
					minor(_11_13_14, _31_33_34, _41_43_44),
					-minor(_11_12_14, _31_32_34, _41_42_44),
					minor(_11_12_13, _31_32_33, _41_42_43),

					minor(_12_13_14, _22_23_24, _42_43_44),
					-minor(_11_13_14, _21_23_24, _41_43_44),
					minor(_11_12_14, _21_22_24, _41_42_44),
					-minor(_11_12_13, _21_22_23, _41_42_43),

					-minor(_12_13_14, _22_23_24, _32_33_34),
					minor(_11_13_14, _21_23_24, _31_33_34),
					-minor(_11_12_14, _21_22_24, _31_32_34),
					minor(_11_12_13, _21_22_23, _31_32_33)
					);
				#undef minor
				return transpose(cofactors) / determinant(input);
			}

			//顶点着色器
			v2f object_vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				float4 worldPos = mul(UNITY_MATRIX_M, v.vertex);
				o.worldPos.xyz = worldPos.xyz;
				o.worldPos.w = 1;

				o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				o.uv.zw = v.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;

				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
				fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				fixed3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w;

				float3x3 worldToTangent = float3x3(worldTangent, worldBinormal, worldNormal);

				o.lightDir = mul(worldToTangent, WorldSpaceLightDir(v.vertex));
				o.viewDir = mul(worldToTangent, WorldSpaceViewDir(v.vertex));

				o.worldnormal = normalize(worldNormal);
				o.cameraDir = normalize(_MainCameraWorldPos.xyz);
				return o;
			}

			inline fixed4 GetNormalColor(v2f i)
			{
				fixed3 tangentLightDir = normalize(i.lightDir);
				fixed3 tangentViewDir = normalize(i.viewDir);

				// Get the texel in the normal map
				fixed4 packedNormal = tex2D(_BumpMap, i.uv.zw);
				fixed3 tangentNormal;
				// If the texture is not marked as "Normal map"
//				tangentNormal.xy = (packedNormal.xy * 2 - 1) * _BumpScale;
//				tangentNormal.z = sqrt(1.0 - saturate(dot(tangentNormal.xy, tangentNormal.xy)));
				// Or mark the texture as "Normal map", and use the built-in funciton
				tangentNormal = UnpackNormal(packedNormal);
				tangentNormal.xy *= _BumpScale;
				tangentNormal.z = sqrt(1.0 - saturate(dot(tangentNormal.xy, tangentNormal.xy)));
				fixed3 albedo = tex2D(_MainTex, i.uv).rgb * _Color.rgb;
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
				//	fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(tangentNormal, tangentLightDir));
				//	fixed3 halfDir = normalize(tangentLightDir + tangentViewDir);
				//	fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(max(0, dot(tangentNormal, halfDir)), _Gloss);
				return fixed4(ambient , 1.0);
				//return fixed4(ambient + diffuse + specular, 1.0);
			}

			inline fixed PCF(sampler2D _CameraDepthTexture,float3 pos,float _Bias,float currentDepth)
			{
				half shadow;
				half2 texelSize = half2(1.0 / _TexturePixelWidth, 1.0 / _TexturePixelHeight);
				for (int x = -1; x <= 1; x++) {
					for (int y = -1; y <= 1; y++) {
						half2 samplePos = pos.xy + half2(x, y) * texelSize;
						fixed4 pcfDepthRGBA = tex2D(_CameraDepthTexture, samplePos);
						fixed pcfDepth = DecodeFloatRGBA(pcfDepthRGBA);
						shadow += currentDepth + _Bias < pcfDepth ? 1.0 : 0.0;
					}
				}
				shadow /= 9.0;
				return shadow;
			}

			inline fixed GetShadowBias(float3 lightDir, float3 normal, fixed maxBias, fixed baseBias)
			{
				half cos_val = saturate(dot(lightDir, normal));
				half sin_val = sqrt(1 - cos_val * cos_val); // sin(acos(L·N))
				half tan_val = sin_val / cos_val;    // tan(acos(L·N))

				fixed bias = baseBias + clamp(tan_val, 0, maxBias);

				return bias;
			}

			//是否在视野之外
			inline half BesideCamera(v2f i,float4x4 _LightProjection)
			{
				fixed4 lightClipPos = mul(_LightProjection , i.worldPos);
				lightClipPos.xyz = lightClipPos.xyz / lightClipPos.w;
				float3 pos = lightClipPos * 0.5 + 0.5;
				half result = 0;

				//The step function returns 0.0 if x is smaller then edge and otherwise 1.0. 
				//The input parameters can be floating scalars or float vectors. 
				//In case of float vectors the operation is done component-wise.
				result += step(1,pos.x);
				result += step(pos.x,0);
				result += step(1,pos.y);
				result += step(pos.y,0);
				result += step(1,pos.z);
				result += step(pos.z,0);
				return result;
			}

			inline fixed4 GetCameraRender(v2f i,sampler2D _LightMap,sampler2D _CameraDepthTexture,float4x4 _LightProjection,float4 CameraPos)
			{

				fixed4 lightClipPos = mul(_LightProjection , i.worldPos);
				lightClipPos.xyz = lightClipPos.xyz / lightClipPos.w;
				float3 pos = lightClipPos * 0.5 + 0.5;
				fixed4 depthRGBA = tex2D(_CameraDepthTexture,pos.xy);
				float3 videoUV = pos;
				videoUV.y = 1 - videoUV.y;
				fixed4 cameracolor = tex2D(_LightMap,videoUV.xy);
				fixed4 ocolor = GetNormalColor(i);
				fixed depth = DecodeFloatRGBA(depthRGBA);
				fixed bias = GetShadowBias(normalize(CameraPos),i.worldnormal,0.0002,0.0001);
				fixed shadow = PCF(_CameraDepthTexture,pos,bias,lightClipPos.z);
				fixed isclip = step(1,BesideCamera(i,_LightProjection));
				//if(result>0)return ocolor;
				fixed4 clipcolor = ocolor;
				fixed4 insidecolor = fixed4(ocolor*shadow + cameracolor*(1 - shadow));
				fixed4 finnalycolor = fixed4((1 - isclip)*insidecolor + (isclip)*clipcolor);
				return fixed4(finnalycolor.xyz,shadow);
			}

			inline fixed4 GetSecondCameraRender(v2f i,fixed ismain,fixed issecond,fixed4 normalcolor)
			{

				fixed4 mainCamColor = (1 - ismain)*GetCameraRender(i,_MainLightMaps,_MainCameraDepthTexture,_MainLightProjection,_MainCameraWorldPos);

				fixed4 secondCamColor = ((1 - issecond))*GetCameraRender(i,_SecondLightMaps,_SecondCameraDepthTexture,_SecondLightProjection,_SecondCameraWorldPos);


				fixed isOnSecond = step(1,mainCamColor.w);


				mainCamColor = (1 - isOnSecond)*mainCamColor + (1 - issecond)*isOnSecond*secondCamColor
				+ issecond*isOnSecond*normalcolor;

				return  mainCamColor +
				secondCamColor*ismain +
				issecond*ismain*normalcolor;
			}
			
			inline fixed4 GetThirdCameraRender(v2f i,fixed ismain,fixed issecond,fixed isthird,fixed4 normalcolor)
			{
				fixed4 mainCamColor = (1 - ismain)*GetCameraRender(i,_MainLightMaps,_MainCameraDepthTexture,_MainLightProjection,_MainCameraWorldPos);
				fixed4 secondCamColor = ((1 - issecond))*GetCameraRender(i,_SecondLightMaps,_SecondCameraDepthTexture,_SecondLightProjection,_SecondCameraWorldPos);
				fixed4 thirdCamColor = (1 - isthird)*GetCameraRender(i,_ThirdLightMaps,_ThirdCameraDepthTexture,_ThirdLightProjection,_ThirdCameraWorldPos);
				fixed isOnSecond = step(1,mainCamColor.w);
				mainCamColor = (1 - isOnSecond)*mainCamColor + (1 - issecond)*isOnSecond*secondCamColor + issecond*isOnSecond*normalcolor;
				fixed isOnThird = step(1,secondCamColor.w);
				mainCamColor = (1 - isthird)*((1 - isOnSecond)*mainCamColor + (1 - isthird)*isOnSecond*thirdCamColor
					+ isthird*isOnSecond*normalcolor) + isthird*mainCamColor;
				secondCamColor = (1 - isOnThird)*secondCamColor + (1 - isthird)*isOnThird*thirdCamColor + isthird*isOnThird*normalcolor;

				return  mainCamColor + secondCamColor * ismain + thirdCamColor*ismain*issecond + isthird*issecond*ismain*normalcolor;
			}
			
			//片元着色器
			fixed4 object_frag(v2f i) : SV_Target
			{
				fixed4 normalcolor = GetNormalColor(i);
				fixed ismain = step(1,BesideCamera(i,_MainLightProjection));
				fixed issecond = step(1,BesideCamera(i,_SecondLightProjection));
				fixed isthird = step(1,BesideCamera(i,_ThirdLightProjection));

				#ifdef MAINCAMERAON
					#ifdef SECONDCAMERAON
						#ifdef THIRDCAMERAON
							return GetThirdCameraRender(i,ismain,issecond,isthird,normalcolor);
						#else 
							return  GetSecondCameraRender(i,ismain,issecond,normalcolor);
						#endif
					#else
						//return (1 - ismain)*GetCameraRender(i,_MainLightMaps,_MainCameraDepthTexture,_MainLightProjection,_MainCameraWorldPos) + ismain*normalcolor;
						return (1 - ismain)*GetCameraRender(i, _MainLightMaps, _MainCameraDepthTexture, _MainLightProjection, _MainCameraWorldPos) + ismain*normalcolor;
					#endif
				#else
					return normalcolor;
				#endif
			}
			
			ENDCG
		}
	}

	
	FallBack "Diffuse"
}
