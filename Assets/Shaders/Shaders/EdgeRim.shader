// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Effects/EdgeRim"
{
	//����
	Properties{
		_Diffuse("Diffuse", Color) = (1,1,1,1)
		_RimColor("RimColor", Color) = (1,1,1,1)
		_RimPower("RimPower", Range(0.000001, 3.0)) = 0.1
		_MainTex("Base 2D", 2D) = "white"{}
	}

		//����ɫ��	
		SubShader
	{
		Pass
		{
		//����Tags
		Tags{ "RenderType" = "Opaque" }

		CGPROGRAM
		//����ͷ�ļ�
		#include "Lighting.cginc"
		//����Properties�еı���
		fixed4 _Diffuse;
		sampler2D _MainTex;
		//ʹ����TRANSFROM_TEX�����Ҫ����XXX_ST
		float4 _MainTex_ST;
		fixed4 _RimColor;
		float _RimPower;

		//����ṹ�壺vertex shader�׶����������
		struct v2f
		{
			float4 pos : SV_POSITION;
			float3 worldNormal : TEXCOORD0;
			float2 uv : TEXCOORD1;
			//��vertex shader�м���۲췽�򴫵ݸ�fragment shader
			float3 worldViewDir : TEXCOORD2;
		};

		//���嶥��shader,����ֱ��ʹ��appdata_base������position, noramal, texcoord��
		v2f vert(appdata_base v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			//ͨ��TRANSFORM_TEX��ת���������꣬��Ҫ������Offset��Tiling�ĸı�,Ĭ��ʱ��ͬ��o.uv = v.texcoord.xy;
			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
			//����ת��������ռ�
			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			//���԰Ѽ������ViewDir�Ĳ�������vertex shader�׶Σ��Ͼ��𶥵����Ƚ�ʡ
			o.worldViewDir = _WorldSpaceCameraPos.xyz - worldPos;
			return o;
		}

		//����ƬԪshader
		fixed4 frag(v2f i) : SV_Target
		{
			//unity�����diffuseҲ�Ǵ��˻����⣬��������Ҳ����һ�»�����
			fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * _Diffuse.xyz;
			//��һ�����ߣ���ʹ��vert��һ��Ҳ���У���vert��frag�׶��в�ֵ��������ķ��߷��򲢲���vertex shaderֱ�Ӵ�����
			fixed3 worldNormal = normalize(i.worldNormal);
			//�ѹ��շ����һ��
			fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
			//���ݰ�������ģ�ͼ������صĹ�����Ϣ
			fixed3 lambert = 0.5 * dot(worldNormal, worldLightDir) + 0.5;
			//���������ɫΪlambert��ǿ*����diffuse��ɫ*����ɫ
			fixed3 diffuse = lambert * _Diffuse.xyz * _LightColor0.xyz + ambient;
			//�����������
			fixed4 color = tex2D(_MainTex, i.uv);

			//����Ϊ��ƪ���⣺����RimLight
			//�����߷����һ��
			float3 worldViewDir = normalize(i.worldViewDir);
			//�������߷����뷨�߷���ļнǣ��н�Խ��dotֵԽ�ӽ�0��˵�����߷���Խƫ��õ㣬Ҳ����ƽ�ӣ��õ�Խ�ӽ���Ե
			float rim = 1 - max(0, dot(worldViewDir, worldNormal));
			//����rimLight
			fixed3 rimColor = _RimColor * pow(rim, 1 / _RimPower);
			//�����ɫ+��Ե����ɫ
			color.rgb = color.rgb * diffuse + rimColor;

			return fixed4(color);
		}

			//ʹ��vert������frag����
			#pragma vertex vert
			#pragma fragment frag	

			ENDCG
		}
	}
		//ǰ���ShaderʧЧ�Ļ���ʹ��Ĭ�ϵ�Diffuse
		FallBack "Diffuse"
}