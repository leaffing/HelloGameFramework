Shader "Custom/ShaderOptions"
{
    Properties
    {
		_Color("Color", Color) = (1,0,0,1)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "IgnoreProjector" = "True" "Queue"="Transparent"}
		
        LOD 600

		Pass{
			ZWrite On
			ColorMask 0
		}

        Pass
        {
			//ZTest Less	
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return fixed4(UNITY_LIGHTMODEL_AMBIENT.rgb ,_Color.a)+ col * _Color;
            }
            ENDCG
        }
    }
	SubShader
	{
			Tags{ "RenderType" = "Opaque" }
			LOD 500

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
		fixed4 _Color;

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			// sample the texture
			fixed4 col = tex2D(_MainTex, i.uv);
		return fixed4(0,1,0,1);
		}
			ENDCG
		}
		}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 300

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
	fixed4 _Color;

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		// sample the texture
		fixed4 col = tex2D(_MainTex, i.uv);
	return fixed4(0,0,1,1);
	}
		ENDCG
	}
	}
}
