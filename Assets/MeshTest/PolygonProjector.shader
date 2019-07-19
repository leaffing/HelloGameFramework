Shader "Custom/PolygonProjector" {
	Properties{
		//调色
		_Color("Color", Color) = (1,1,1,1)
		_PointColor("PointColor", Color) = (1,0,0,0.5)
		_PointSize("PointSize", Range(0, 2)) = 0.5
		_LineColor("LineColor", Color) = (0,1,0,0.5)
		_LineSize("LineSize", Range(0, 2)) = 0.5
		//投影图片
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		//根据投影仪视距渐变的图片
		_FalloffTex("Falloff",2D) = "white"{}
	}
	SubShader{
		Pass{
			ZWrite Off
			//解决ZFighting现象
			Offset -1, -1
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vertg
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4 _Color;
			//将投影仪剪辑空间的X和Y轴映射到U和V坐标，这些坐标通常用于对径向衰减纹理进行采样。
			float4x4 unity_Projector;
			//将投影仪视图空间的Z轴映射到U坐标（可能在V中复制它），该坐标可用于采样渐变纹理，该纹理定义投影仪随距离衰减。u值在投影仪近平面处为0，在投影仪远平面处为1。
			float4x4 unity_ProjectorClip;

			sampler2D _MainTex;
			sampler2D _FalloffTex;

			fixed4 _PointColor;
			float _PointSize;
			fixed4 _LineColor;
			float _LineSize;
			
			//定义与脚本进行通信的变量 
			vector Value[6];
			int PointNum = 0;

			struct v2f {
				float4 uvDecal:TEXCOORD0;
				float4 uvFalloff:TEXCOORD1;
				float4 pos:SV_POSITION;
				float4 worldPos:TEXCOOD2;
			};

			//计算两点间的距离的函数 
			float Dis(float4 v1, float4 v2)
			{
				return sqrt(pow((v1.x - v2.x), 2) + pow((v1.y - v2.y), 2) + pow((v1.z - v2.z), 2));
			}

			//绘制线段 
			bool DrawLineSegment(float4 p1, float4 p2, float lineWidth, v2f i)
			{
				//p1 = mul(unity_Projector, p1);
				//p2 = mul(unity_Projector, p2);
				float4 center = float4((p1.x + p2.x) / 2, 0, (p1.z + p2.z) / 2, 0);
				//计算点到直线的距离   
				float d = abs((p2.z- p1.z) * i.worldPos.x + (p1.x - p2.x) * i.worldPos.z + p2.x * p1.z - p2.z * p1.x) / sqrt(pow(p2.z - p1.z, 2) + pow(p1.x - p2.x, 2));
				//小于或者等于线宽的一半时，属于直线范围   
				float lineLength = sqrt(pow(p1.x - p2.x, 2) + pow(p1.z - p2.z, 2));
				if (d <= lineWidth / 2 && Dis(float4(i.worldPos.x, 0, i.worldPos.z, 0), center) < lineLength / 2)
				{
					return true;
				}
				return false;
			}

			//绘制多边形，这里限制了顶点数不超过6。可以自己根据需要更改。 
			bool pnpoly(int nvert, float4 vert[6], float testx, float testy)
			{
				int i, j;
				bool c = false;
				float vertx[6];
				float verty[6];
				for (int n = 0;n < nvert;n++)
				{
					vertx[n] = vert[n].x;
					verty[n] = vert[n].z;
				}

				for (i = 0, j = nvert - 1; i < nvert; j = i++) {
					if (((verty[i] > testy) != (verty[j] > testy)) && (testx < (vertx[j] - vertx[i]) * (testy - verty[i]) / (verty[j] - verty[i]) + vertx[i]))
						c = !c;
				}
				return c;
			}

			v2f vertg(appdata_img v){
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0));
				//四元纹理坐标给UNITY_PROJ_COORD读取					
				o.uvDecal = mul(unity_Projector, v.vertex);
				o.uvFalloff = mul(unity_ProjectorClip, v.vertex);
				//处理顶点到投射空间 
				//for (int j = 0;j < PointNum;j++)
				//{
				//	vector temp = Value[j];
				//	//Value[j] = mul(unity_Projector, temp);
				//}
				return o;
			}

			float4 frag(v2f i) :SV_Target{
				//绘制多边形顶点 
				for (int j = 0;j < PointNum;j++)
				{
					if (Dis(i.worldPos, Value[j]) < _PointSize)
					{
						return _PointColor;
					}
				}

				//绘制多边形的边 
				for (int k = 0;k < PointNum;k++)
				{
					if (k == PointNum - 1)
					{
						if (DrawLineSegment(Value[k],Value[0], _LineSize,i))
						{
							return _LineColor;
						}
					}
					else
					{
						if (DrawLineSegment(Value[k],Value[k + 1], _LineSize,i))
						{
							return _LineColor;
						}
					}
				}

				//填充多边形内部 
				if (pnpoly(PointNum, Value, i.worldPos.x ,i.worldPos.z))
				{
					float4 decal;
					//解决图片四周拖影
					if (i.uvDecal.x / i.uvDecal.w<0.0001 || i.uvDecal.x / i.uvDecal.w>0.9999 || i.uvDecal.y / i.uvDecal.w<0.0001 || i.uvDecal.y / i.uvDecal.w>0.9999)
					{
						decal = float4(0, 0, 0, 0);
					}
					else
					{
						//采样齐次uv，分量都除以了w
						decal = tex2Dproj(_MainTex, UNITY_PROJ_COORD(i.uvDecal));
					}
					float falloff = tex2Dproj(_FalloffTex, UNITY_PROJ_COORD(i.uvFalloff)).r;
					return float4(decal.rgb* _Color.rgb, decal.a* falloff* _Color.a);
				}
				return fixed4(0,0,0,0);

				float4 decal;
				//解决图片四周拖影
				if (i.uvDecal.x / i.uvDecal.w<0.0001 || i.uvDecal.x / i.uvDecal.w>0.9999 || i.uvDecal.y / i.uvDecal.w<0.0001 || i.uvDecal.y / i.uvDecal.w>0.9999)
				{
					decal = float4(0, 0, 0, 0);
				}
				else
				{
					//采样齐次uv，分量都除以了w
					decal = tex2Dproj(_MainTex, UNITY_PROJ_COORD(i.uvDecal));
				}
				float falloff = tex2Dproj(_FalloffTex, UNITY_PROJ_COORD(i.uvFalloff)).r;
				return float4(decal.rgb* _Color.rgb,decal.a* falloff* _Color.a);
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}