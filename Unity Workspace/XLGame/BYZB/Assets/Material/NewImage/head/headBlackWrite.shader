// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/headBlackWrite" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

	}
	SubShader {
		Tags { "Queue"="Transparent+1000" }




		pass{

		Offset -1, -1
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		sampler2D _MainTex;

		struct Input {
			float4 vertex : POSITION;
			float4 color : COLOR;
			float2 texcoord : TEXCOORD0;//一级纹理坐标
		};
		struct Output{
			float4 vertex : POSITION;
			float4 color : COLOR;
			half2 texcoord : TEXCOORD0;
		};
		fixed4 _Color;



		Output vert (Input IN) {
			Output Out;
				//输出的顶点位置为模型视图投影矩阵乘以顶点位置，也就是将三维空间中的坐标投影到了二维窗口
				Out.vertex = UnityObjectToClipPos(IN.vertex);
				//输出的纹理坐标也就是输入的纹理坐标
				Out.texcoord = IN.texcoord;
				//输出的颜色值也就是输入的颜色值
				Out.color = IN.color;
				//【3】返回此输出结构对象
				return Out;

		}
		fixed4 frag(Output output): COLOR{
			 fixed4 col;  
     		     col = tex2D(_MainTex, output.texcoord); 
      		 	 float grey = dot(col.rgb, float3(0.299, 0.587, 0.114));  
       			 col.rgb = float3(grey, grey, grey);  
   				 
   			return col;
		}

		ENDCG


		Stencil
			{
  			  Ref [_Stencil]
   			 Comp [_StencilComp]
   			 Pass [_StencilOp] 
   			 ReadMask [_StencilReadMask]
   			 WriteMask [_StencilWriteMask]
			}
	}





	}
	FallBack "Diffuse"
}
