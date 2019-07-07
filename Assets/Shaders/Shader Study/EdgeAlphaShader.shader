// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/EdgeAlphaShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_AlphaNum("AlphaNum", Range(0,2)) = 1
    }
    SubShader
    {
		Tags{ "RenderType" = "Transparent" "IgnoreProjector" = "True" "Queue" = "Transparent" }
        LOD 100

        Pass
        {
			//ZTest Less	
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			//Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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
				float dot : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _AlphaNum;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				float3 viewDir = mul(unity_WorldToObject, WorldSpaceViewDir(v.vertex));
				o.dot = max(0,dot(normalize(viewDir), v.normal) * _AlphaNum);
				//o.dot = lerp(0, 1, dot(normalize(viewDir), v.normal) * _AlphaNum);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return fixed4(col.rgb,col.a * i.dot);
            }
            ENDCG
        }
    }
}
