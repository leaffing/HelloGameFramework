Shader "Custom/CustomMixShader" 
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _ProjectTexture;
			float4x4 _MainLightProjection;
			float4x4 _MainCameraProjection;
			float3 _MainLeftBottomPos;
			float3 _MainLeftLeftTopPos;
			float3 _MainLeftRightBottomPos;

			sampler2D _MainTex;
			float4 _MainTex_ST;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 worldPos : TEXCOORD1;
			};

			//是否在投射之外
			inline half BesideCamera(float3 pos)
			{				
				half result = 0;
				/*对于GPU来讲，各个顶点各个像素都在进行大量的并行运算，每个片段着色器都在同步运行，
				边缘地带像素的片段着色器虽然率先return，但是它依然要等待最后一个return的像素。
				只有所有像素全部完成计算，才会进行下一次运算， 在片段着色器中，每个片段处理器每条指令操作上百个像素.
				如果有些片段（像素）采取一个分支而有些片段不采用另一个分支，则所有片段都会执行两个分支，
				但只在每个片段应该采取的分支上写入寄存器。另外，if/endif等流程控制操作有较高的开销（4个时钟周期,Geforce6）。
				因此在GPU编程中，if else ，switch case等条件语句和太复杂的逻辑是不推荐的.
				相应的，可以用step（）等函数进行替换，用阶梯函数的思维来构建条件语句。
				这样，所有的线程都执行完全一样的代码，在很多方面对GPU都是有益的。*/
				result += step(1, pos.x);
				result += step(pos.x, 0);
				result += step(1, pos.y);
				result += step(pos.y, 0);
				result += step(1, pos.z);
				result += step(pos.z, 0);
				return result;
			}

			inline float4 GetCameraProjectPoint(float4x4 CameraProjection, float4 WorldPos) 
			{
				//进行MVP转换后得到投影齐次坐标
				float4 projectPos = mul(_MainCameraProjection, WorldPos);
				//转成NDC坐标系
				projectPos = projectPos / projectPos.w;
				return projectPos;
			}

			inline float2 GetCameraUVPoint(float2 Point, float2 LB, float2 LT, float2 RB)
			{
				float2 P = Point - LB;
				float2 XL = RB - LB;
				float2 YL = LT - LB;
				float x = dot(normalize(P), normalize(XL)) * length(P) / length(XL);
				float y = dot(normalize(P), normalize(YL)) * length(P) / length(YL);
				return float2(x, y);
			}

			inline fixed4 GetCameraProjectionColor(float4 WorldPos, float4x4 CameraProjection) {
				float2 Point = GetCameraProjectPoint(CameraProjection, WorldPos);
				float2 LB = GetCameraProjectPoint(CameraProjection, float4(_MainLeftBottomPos,1));
				float2 LT = GetCameraProjectPoint(CameraProjection, float4(_MainLeftLeftTopPos, 1));
				float2 RB = GetCameraProjectPoint(CameraProjection, float4(_MainLeftRightBottomPos, 1));
				float2 uvPos = GetCameraUVPoint(Point, LB, LT, RB);
				
				float2 videoUV = uvPos;
				videoUV.y = 1 - videoUV.y;
				fixed4 cameracolor = tex2D(_ProjectTexture, videoUV.xy);
				fixed ismain = BesideCamera(float3(uvPos,0.5));
				return (1 - ismain) * cameracolor + fixed4(1,1,1,0);
			}

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				
				//完成MVP中的M（local -> worl）
				//与该计算功能一致：float4 worldPos = mul(UNITY_MATRIX_M, v.vertex);
				float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.worldPos.xyz = worldPos.xyz;
				//点到投影矩阵再到UV去对比深度时需要与correction矩阵运算
				//而要使这个矩阵成立，该vector4的w分量必须是1
				o.worldPos.w = 1;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				//获取该片元原始颜色值
				fixed4 col = tex2D(_MainTex, i.uv);
				
				//进行MVP转换后得到投影齐次坐标
				fixed4 projectPos = mul(_MainLightProjection, i.worldPos);
				//转成NDC坐标系
				projectPos = projectPos / projectPos.w;

				//计算投影点是否在投射区域内
				fixed ismain = step(1, BesideCamera(projectPos));
				
				//通过UV获取投射图像对应点颜色
				float3 videoUV = projectPos;
				videoUV.y = 1 - videoUV.y;
				fixed4 cameracolor = tex2D(_ProjectTexture, videoUV.xy);

				fixed4 finalcol =  (1 - ismain) * cameracolor + ismain * col;
				return fixed4(finalcol.xyz, 1); //是否需要透明处理？
				//return (1 - ismain) * GetCameraProjectionColor(i.worldPos, _MainCameraProjection) + ismain * col;
			}
			ENDCG
		}
	}
}