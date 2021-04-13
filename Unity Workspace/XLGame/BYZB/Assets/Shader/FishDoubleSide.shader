// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.28 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.28;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:1037,x:33351,y:32578,varname:node_1037,prsc:2|emission-1944-OUT,custl-4336-OUT,alpha-9140-OUT;n:type:ShaderForge.SFN_Tex2d,id:4249,x:32539,y:32749,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:1389,x:32539,y:32987,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:4336,x:32905,y:32781,varname:node_4336,prsc:2|A-4249-RGB,B-1389-RGB,C-327-OUT;n:type:ShaderForge.SFN_Multiply,id:9140,x:32905,y:32916,varname:node_9140,prsc:2|A-4249-A,B-1389-A;n:type:ShaderForge.SFN_ValueProperty,id:327,x:32539,y:32605,ptovrint:False,ptlb:Light,ptin:_Light,varname:_Light,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Tex2d,id:4354,x:32202,y:32142,ptovrint:False,ptlb:WaveLight,ptin:_WaveLight,varname:_WaveLight,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:34a4f1b1c38bf8c40bc3bcb8095e37cf,ntxv:0,isnm:False|UVIN-4419-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:1175,x:31704,y:32128,varname:node_1175,prsc:2,uv:1;n:type:ShaderForge.SFN_Panner,id:4419,x:31917,y:32128,varname:node_4419,prsc:1,spu:-0.2,spv:0|UVIN-1175-UVOUT;n:type:ShaderForge.SFN_Multiply,id:5892,x:32506,y:32315,varname:node_5892,prsc:2|A-4354-RGB,B-9938-OUT;n:type:ShaderForge.SFN_Color,id:8209,x:32217,y:32542,ptovrint:False,ptlb:WaveLightColor,ptin:_WaveLightColor,varname:_WaveLightColor,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:9938,x:32202,y:32381,ptovrint:False,ptlb:LightWaveValue,ptin:_LightWaveValue,varname:_LightWaveValue,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_Multiply,id:1944,x:32822,y:32417,varname:node_1944,prsc:2|A-5892-OUT,B-8209-RGB,C-8209-A;proporder:1389-4249-327-4354-8209-9938;pass:END;sub:END;*/

Shader "Fish/Fish_doubleside" {
    Properties {
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _MainTex ("MainTex", 2D) = "white" {}
        _Light ("Light", Float ) = 0
        _WaveLight ("WaveLight", 2D) = "white" {}
        _WaveLightColor ("WaveLightColor", Color) = (0.5,0.5,0.5,1)
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
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform fixed4 _Color;
            uniform float _Light;
            uniform sampler2D _WaveLight; uniform float4 _WaveLight_ST;
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
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 node_1277 = _Time + _TimeEditor;
                half2 node_4419 = (i.uv1+node_1277.g*float2(-0.2,0));
                fixed4 _WaveLight_var = tex2D(_WaveLight,TRANSFORM_TEX(node_4419, _WaveLight));
                float3 emissive = ((_WaveLight_var.rgb*_LightWaveValue)*_WaveLightColor.rgb*_WaveLightColor.a);
                fixed4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 finalColor = emissive + (_MainTex_var.rgb*_Color.rgb*_Light);
                return fixed4(finalColor,(_MainTex_var.a*_Color.a));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
