// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33304,y:32317,varname:node_9361,prsc:2|emission-1192-OUT,custl-2546-OUT,alpha-1415-OUT;n:type:ShaderForge.SFN_TexCoord,id:6160,x:31349,y:32593,varname:node_6160,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Panner,id:7756,x:31783,y:32591,varname:UVSpeed,prsc:1,spu:0.01,spv:0|UVIN-6160-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:6734,x:31859,y:32903,ptovrint:False,ptlb:FlowMap,ptin:_FlowMap,varname:_FlowMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-3706-OUT;n:type:ShaderForge.SFN_Append,id:3370,x:32061,y:32903,varname:node_3370,prsc:2|A-6734-R,B-6734-R;n:type:ShaderForge.SFN_Add,id:74,x:32243,y:32594,varname:UV_MainTex,prsc:1|A-7756-UVOUT,B-1586-OUT;n:type:ShaderForge.SFN_Tex2d,id:3281,x:32587,y:32734,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-74-OUT;n:type:ShaderForge.SFN_Multiply,id:1586,x:32243,y:32944,varname:FlowMap_Mul,prsc:2|A-3370-OUT,B-1289-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1289,x:32061,y:33122,ptovrint:False,ptlb:FlowValue,ptin:_FlowValue,varname:_FlowValue,prsc:1,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Multiply,id:2546,x:32898,y:32661,varname:node_2546,prsc:2|A-3281-RGB,B-3642-RGB;n:type:ShaderForge.SFN_Color,id:3642,x:32585,y:32995,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:1415,x:32905,y:32852,varname:node_1415,prsc:2|A-3281-A,B-3642-A;n:type:ShaderForge.SFN_Time,id:7993,x:30891,y:32758,varname:Time,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:9333,x:30891,y:32982,ptovrint:False,ptlb:USpeed,ptin:_USpeed,varname:_USpeed,prsc:1,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:4471,x:30891,y:33124,ptovrint:False,ptlb:VSpeed,ptin:_VSpeed,varname:_VSpeed,prsc:1,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:1751,x:31136,y:32827,varname:node_1751,prsc:2|A-7993-T,B-9333-OUT;n:type:ShaderForge.SFN_Multiply,id:306,x:31136,y:33008,varname:node_306,prsc:2|A-7993-T,B-4471-OUT;n:type:ShaderForge.SFN_Append,id:3540,x:31346,y:32915,varname:node_3540,prsc:2|A-1751-OUT,B-306-OUT;n:type:ShaderForge.SFN_Add,id:3706,x:31593,y:32894,varname:Time_UV,prsc:2|A-6160-UVOUT,B-3540-OUT;n:type:ShaderForge.SFN_Tex2d,id:4993,x:32587,y:32220,ptovrint:False,ptlb:Emission,ptin:_Emission,varname:_Emission,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-1431-OUT;n:type:ShaderForge.SFN_Multiply,id:1192,x:32898,y:32421,varname:node_1192,prsc:2|A-4398-OUT,B-4993-RGB,C-7523-RGB,D-7523-A;n:type:ShaderForge.SFN_Vector1,id:4398,x:32587,y:32121,varname:node_4398,prsc:2,v1:15;n:type:ShaderForge.SFN_Panner,id:9022,x:31783,y:32401,varname:node_9022,prsc:2,spu:0,spv:-0.03|UVIN-6160-UVOUT;n:type:ShaderForge.SFN_Add,id:1431,x:32083,y:32404,varname:UV_Emission,prsc:1|A-9022-UVOUT,B-1586-OUT;n:type:ShaderForge.SFN_Color,id:7523,x:32587,y:32470,ptovrint:False,ptlb:EmissionColor,ptin:_EmissionColor,varname:_EmissionColor,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;proporder:3642-3281-6734-4993-1289-9333-4471-7523;pass:END;sub:END;*/

Shader "Fish/FishUIWater" {
    Properties {
        _Color ("Color", Color) = (0,0.5,0.5,1)
        _MainTex ("MainTex", 2D) = "white" {}
        _FlowMap ("FlowMap", 2D) = "white" {}
        _Emission ("Emission", 2D) = "white" {}
        _FlowValue ("FlowValue", Float ) = 0.1
        _USpeed ("USpeed", Float ) = 0
        _VSpeed ("VSpeed", Float ) = 0
        _EmissionColor ("EmissionColor", Color) = (0.5,0.5,0.5,1)
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
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform sampler2D _FlowMap; uniform float4 _FlowMap_ST;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform half _FlowValue;
            uniform fixed4 _Color;
            uniform half _USpeed;
            uniform half _VSpeed;
            uniform sampler2D _Emission; uniform float4 _Emission_ST;
            uniform fixed4 _EmissionColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_4263 = _Time;
                float4 Time = _Time;
                float2 Time_UV = (i.uv0+float2((Time.g*_USpeed),(Time.g*_VSpeed)));
                float4 _FlowMap_var = tex2D(_FlowMap,TRANSFORM_TEX(Time_UV, _FlowMap));
                float2 FlowMap_Mul = (float2(_FlowMap_var.r,_FlowMap_var.r)*_FlowValue);
                half2 UV_Emission = ((i.uv0+node_4263.g*float2(0,-0.03))+FlowMap_Mul);
                float4 _Emission_var = tex2D(_Emission,TRANSFORM_TEX(UV_Emission, _Emission));
                float3 emissive = (15.0*_Emission_var.rgb*_EmissionColor.rgb*_EmissionColor.a);
                half2 UV_MainTex = ((i.uv0+node_4263.g*float2(0.01,0))+FlowMap_Mul);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(UV_MainTex, _MainTex));
                float3 finalColor = emissive + (_MainTex_var.rgb*_Color.rgb);
                return fixed4(finalColor,(_MainTex_var.a*_Color.a));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
