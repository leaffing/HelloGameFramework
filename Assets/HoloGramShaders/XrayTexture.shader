Shader "Hologram/XrayTexture" {
	Properties
	{
		_RimColor("RimColor", Color) = (0, 0, 1, 1)
		_RimIntensity("Intensity", Range(0, 2)) = 1
		_Diffuse("Diffuse", 2D) = "white" {}
	}
		SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType" = "Opaque" }

		LOD 200
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Front
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 texcoord0 : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv0 : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv0 = v.texcoord0;
				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				float4 _Diffuse_var = tex2D(_Diffuse, TRANSFORM_TEX(i.uv0, _Diffuse));
				return _Diffuse_var;
			}
			ENDCG
		}
		Pass
		{
			Blend SrcAlpha  OneMinusSrcAlpha
			ZWrite off
			Cull Back
			Lighting off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
					

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal:Normal;
				float2 texcoord0 : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				fixed4 color : COLOR;
				float2 uv0 : TEXCOORD0;
			};

			fixed4 _RimColor;
			float _RimIntensity;

			v2f vert(appdata v)
			{
				v2f o;
				o.uv0 = v.texcoord0;
				o.pos = UnityObjectToClipPos(v.vertex);
				float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));//计算出顶点到相机的向量
				float val = 1 - saturate(dot(v.normal, viewDir));//计算点乘值

				o.color = _RimColor * val * (0.8 + _RimIntensity);//计算强度
				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				return i.color;
			}
			ENDCG
		}
	}
}