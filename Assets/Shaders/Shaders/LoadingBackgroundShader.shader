Shader "Custom/LoadingBackgroundShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
	    _Noise ("NoiseMask", 2D) = "white" {}
		_Thresold("Thresold", Range(0,2)) = 1
		_ScrollingSpeed("Scrolling speed", Vector) = (1,0,0,0)
		_MaskScrollingSpeed("Mask Speed", Vector) = (0,0,0,0)
		_MaskThresold("Mask Thresold", Range(0,2)) = 1
	}
	SubShader
	{
		Tags {  "Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True" }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

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
				float2 maskuv : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _Noise;
			float4 _Noise_ST;
			float _Thresold;
			float4 _ScrollingSpeed;
			float4 _MaskScrollingSpeed;
			float _MaskThresold;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv + _Time.x * _ScrollingSpeed, _MainTex);
				o.maskuv = TRANSFORM_TEX(v.uv + _Time.x * _MaskScrollingSpeed, _Noise);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 maskCol = tex2D(_Noise, i.maskuv * _Time.x);
				return fixed4(col.xyz, col.w * (saturate(sin(_Time.w) + 0.5) + _Thresold) * (dot(fixed3(0.2154, 0.7154, 0.0721), maskCol)+ _MaskThresold));
				return fixed4(col.xyz, col.w * dot(fixed3(0.2154, 0.7154, 0.0721), maskCol) * (saturate(_SinTime.w) + _Thresold));
			}
			ENDCG
		}
	}
}
