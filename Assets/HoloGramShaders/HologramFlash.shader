Shader "Hologram/HologramFlash" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_RimColor("Color", Color) = (1,1,1,1)
	}
		SubShader{
			Tags{ "RenderType" = "Opaque" /*"Queue" = "Transparent"*/ }
			LOD 200
			CGPROGRAM
#pragma surface surf Lambert


		sampler2D _MainTex;
		fixed4 _RimColor;
		float _TransVal;

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
			float3 worldNormal;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;


			//边缘发光
			half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
			float sinX = sin(_Time.y * 4);
			_TransVal = ((sinX * sinX + 1) * 4 + (1 - c.r)) * 1;
			o.Emission = _RimColor.rgb * pow(rim, _TransVal * 1.0f);

		}
		ENDCG
		}
			FallBack "Diffuse"
}