// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Depth" {
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
	
	/*SubShader
	{
		Tags { "RenderType" = "Opaque" }
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
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return fixed4(i.vertex.z, i.vertex.z, i.vertex.z, 1);
			}
			ENDCG
		}
	}*/
	//SubShader
	//{
	//	Tags{ "RenderType" = "Opaque" }

	//	Pass
	//	{
	//		ZTest Always Cull Off ZWrite Off

	//		CGPROGRAM
	//		// Use shader model 3.0 target, to get nicer looking lighting
	//		#pragma glsl
	//		#pragma fragmentoption ARB_precision_hint_fastest
	//		#pragma target 3.0
	//		#pragma vertex vert
	//		#pragma fragment frag
	//		#include "unityCG.cginc"
	//		sampler2D _CameraDepthTexture;

	//		struct v2f {
	//			float4 pos : SV_POSITION;
	//			float4 scrPos:TEXCOORD0;
	//		};
	//		//Vertex Shader
	//		v2f vert(appdata_base v) {
	//			v2f o;
	//			o.pos = UnityObjectToClipPos(v.vertex);
	//			o.scrPos = ComputeScreenPos(o.pos);
	//			return o;
	//		}

	//		//Fragment Shader
	//		float4 frag(v2f i) :COLOR{
	//		float depthValue = 1 - Linear01Depth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r);
	//		//float4 depthValue = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos));
	//		return float4(depthValue, depthValue, depthValue, 1.0f);
	//		//return depthValue;
	//	}
	//	ENDCG
	//}
	//}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 depth: TEXCOORD0;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.depth = o.vertex.zw;
				return o;
			}
			fixed4 frag(v2f i) : SV_Target
			{
				float depth = i.depth.x / i.depth.y;
				return EncodeFloatRGBA(depth);
			}
			ENDCG
		}
	}
		FallBack "Diffuse"
}