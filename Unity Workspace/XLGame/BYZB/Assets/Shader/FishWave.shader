// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.28 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.28;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:687,x:32798,y:32164,varname:node_687,prsc:2|emission-6027-OUT,custl-7558-OUT,alpha-8121-OUT;n:type:ShaderForge.SFN_Tex2d,id:2054,x:31849,y:31947,ptovrint:False,ptlb:WaveLight,ptin:_WaveLight,varname:_WaveLight,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:34a4f1b1c38bf8c40bc3bcb8095e37cf,ntxv:0,isnm:False|UVIN-2232-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:3777,x:32070,y:32641,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:540e3f74e4f914c2fbdef38ae542c1b5,ntxv:0,isnm:False|UVIN-9934-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:373,x:31351,y:31933,varname:node_373,prsc:2,uv:1;n:type:ShaderForge.SFN_ValueProperty,id:5565,x:32059,y:32945,ptovrint:False,ptlb:Light,ptin:_Light,varname:_Light,prsc:1,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:7558,x:32409,y:32514,varname:node_7558,prsc:2|A-3777-RGB,B-5565-OUT,C-7475-RGB;n:type:ShaderForge.SFN_Multiply,id:8121,x:32352,y:32820,varname:node_8121,prsc:2|A-3777-A,B-7475-A;n:type:ShaderForge.SFN_Color,id:7475,x:32059,y:33096,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Panner,id:2232,x:31564,y:31933,varname:UV_Speed,prsc:1,spu:-0.2,spv:0|UVIN-373-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:9934,x:31847,y:32680,varname:node_9934,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:8454,x:32147,y:32153,varname:node_8454,prsc:2|A-2054-RGB,B-19-OUT;n:type:ShaderForge.SFN_Color,id:6621,x:31847,y:32435,ptovrint:False,ptlb:WaveLightColor,ptin:_WaveLightColor,varname:_WaveLightColor,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:19,x:31847,y:32292,ptovrint:False,ptlb:LightWaveValue,ptin:_LightWaveValue,varname:_LightWaveValue,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_Multiply,id:6027,x:32439,y:32263,varname:node_6027,prsc:2|A-8454-OUT,B-6621-RGB,C-6621-A;proporder:7475-3777-6621-2054-5565-19;pass:END;sub:END;*/

Shader "Fish/FishWaveLight(All)" {
    Properties {
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _MainTex ("MainTex", 2D) = "white" {}
        _WaveLightColor ("WaveLightColor", Color) = (0.5,0.5,0.5,1)
        _WaveLight ("WaveLight", 2D) = "white" {}
        _Light ("Light", Float ) = 1
        _LightWaveValue ("LightWaveValue", Float ) = 3
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _WaveLight; uniform float4 _WaveLight_ST;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform half _Light;
            uniform fixed4 _Color;
            uniform fixed4 _WaveLightColor;
            uniform float _LightWaveValue;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_8525 = _Time + _TimeEditor;
                half2 UV_Speed = (i.uv1+node_8525.g*float2(-0.2,0));
                fixed4 _WaveLight_var = tex2D(_WaveLight,TRANSFORM_TEX(UV_Speed, _WaveLight));
                float3 emissive = ((_WaveLight_var.rgb*_LightWaveValue)*_WaveLightColor.rgb*_WaveLightColor.a);
                fixed4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 finalColor = emissive + (_MainTex_var.rgb*_Light*_Color.rgb);
                return fixed4(finalColor,(_MainTex_var.a*_Color.a));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
