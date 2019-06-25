Shader "Hologram/HologramUI"{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
	    _RimIntensity("Intensity", Range(0, 10)) = 1
		_Threshold("Threshold", Range(5, 10)) = 5
	}
		SubShader
		{
			Tags { "Queue" = "Transparent" "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
			//Blend SrcAlpha One//打开混合模式
			Blend SrcAlpha  One
			ZWrite off
			Lighting off

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
			fixed4 _Color;
			fixed _RimIntensity;
			float _Threshold;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed result = min(exp2(col.r * _Threshold - _Threshold), exp2(col.g * _Threshold - _Threshold));
				result = min(result, exp2(col.b * _Threshold - _Threshold));
				fixed3 c = _Color.rgb;
				col = fixed4(c, result * _RimIntensity);
				col = _Color * result * _RimIntensity;
				return col;
			}
			ENDCG
		}
		}
}
