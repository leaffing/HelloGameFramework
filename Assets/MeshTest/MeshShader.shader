// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/depth_render"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
	SubShader
	{

		Tags{"RenderType" = "Opaque"  "Queue" = "Geometry"}
		// No culling or depth
		Cull Off ZWrite Off ZTest off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			sampler2D _CameraDepthTexture;
			sampler2D _MainTex;

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

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float4 col = float4(1, 1, 1, 1);

				// 内置 深度获取
				float depth = tex2D(_CameraDepthTexture, i.uv).r;
				col = float4(depth, 0, 0, 1);
				return col;
			}
			ENDCG

			//CGPROGRAM
			//// Use shader model 3.0 target, to get nicer looking lighting
			//#pragma glsl
			//#pragma fragmentoption ARB_precision_hint_fastest
			//#pragma target 3.0
			//#pragma vertex vert
			//#pragma fragment frag
			//#include "unityCG.cginc"
			//sampler2D _CameraDepthTexture;

			//struct v2f {
			//	float4 pos : SV_POSITION;
			//	float4 scrPos:TEXCOORD0;
			//};
			////Vertex Shader
			//v2f vert(appdata_base v) {
			//	v2f o;
			//	o.pos = UnityObjectToClipPos(v.vertex);
			//	o.scrPos = ComputeScreenPos(o.pos);
			//	return o;
			//}

			////Fragment Shader
			//float4 frag(v2f i) :COLOR{
			//float depthValue = 1 - Linear01Depth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r);
			//return float4(depthValue, depthValue, depthValue, 0.5f);
			//}
			//ENDCG
		}
	}
}