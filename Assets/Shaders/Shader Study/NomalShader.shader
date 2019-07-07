Shader "Custom/NomalShader"
{
    Properties
    {

		_Color("Color", Color) = (1,0,0,1)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			#include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 albedo : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.albedo = dot(UnityObjectToWorldNormal(v.normal), normalize(UnityWorldSpaceLightDir(mul(unity_ObjectToWorld, v.vertex))));
				o.albedo = max(0, o.albedo) * _LightColor0.rgb;
				return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
				// 反射率
				fixed3 albedo = col.rgb * _Color.rgb;
				// 环境光
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb * albedo;
				// 漫反射
				fixed3 diffuse = i.albedo * albedo;

				return fixed4(diffuse + ambient, col.a * _Color.a);
            }
            ENDCG
        }
    }
}
