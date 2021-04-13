// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.30 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.30;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:2944,x:32843,y:32738,varname:node_2944,prsc:2|custl-7549-OUT,alpha-8375-A;n:type:ShaderForge.SFN_Tex2d,id:8375,x:32219,y:32767,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_TexCoord,id:903,x:30535,y:32624,varname:node_903,prsc:2,uv:0;n:type:ShaderForge.SFN_RemapRange,id:7880,x:30988,y:32732,varname:node_7880,prsc:2,frmn:0,frmx:1,tomn:0,tomx:3.14|IN-9087-OUT;n:type:ShaderForge.SFN_Sin,id:2486,x:31534,y:32928,varname:node_2486,prsc:2|IN-7207-OUT;n:type:ShaderForge.SFN_Slider,id:5043,x:30893,y:33205,ptovrint:False,ptlb:width,ptin:_width,varname:_width,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1.335804,max:10;n:type:ShaderForge.SFN_Exp,id:3313,x:31495,y:33187,varname:node_3313,prsc:2,et:0|IN-4813-OUT;n:type:ShaderForge.SFN_Power,id:3525,x:31702,y:32968,varname:Power,prsc:0|VAL-2486-OUT,EXP-3313-OUT;n:type:ShaderForge.SFN_RemapRange,id:4813,x:31279,y:33222,varname:node_4813,prsc:2,frmn:0,frmx:10,tomn:10,tomx:1|IN-5043-OUT;n:type:ShaderForge.SFN_Color,id:1684,x:32019,y:33330,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Vector1,id:4421,x:32019,y:33213,varname:node_4421,prsc:2,v1:2;n:type:ShaderForge.SFN_Add,id:7549,x:32416,y:32929,varname:node_7549,prsc:2|A-8375-RGB,B-9794-OUT;n:type:ShaderForge.SFN_Multiply,id:9794,x:32219,y:33160,varname:node_9794,prsc:2|A-9917-OUT,B-4421-OUT,C-1684-RGB;n:type:ShaderForge.SFN_Slider,id:2063,x:30701,y:32933,ptovrint:False,ptlb:Offect,ptin:_Offect,varname:_Offect,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-3.2,cur:3.2,max:3.2;n:type:ShaderForge.SFN_Add,id:7207,x:31290,y:32855,varname:node_7207,prsc:2|A-7880-OUT,B-4011-OUT,C-2063-OUT;n:type:ShaderForge.SFN_Slider,id:1755,x:30152,y:32810,ptovrint:False,ptlb:Angle,ptin:_Angle,varname:_Angle,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:90;n:type:ShaderForge.SFN_RemapRange,id:9573,x:30535,y:32808,varname:node_9573,prsc:2,frmn:0,frmx:90,tomn:0,tomx:1|IN-1755-OUT;n:type:ShaderForge.SFN_Lerp,id:9087,x:30780,y:32722,varname:node_9087,prsc:2|A-903-U,B-903-V,T-9573-OUT;n:type:ShaderForge.SFN_Clamp,id:9917,x:31932,y:33031,varname:node_9917,prsc:2|IN-3525-OUT,MIN-1573-OUT,MAX-6668-OUT;n:type:ShaderForge.SFN_Vector1,id:1573,x:31702,y:33145,varname:node_1573,prsc:2,v1:0.01;n:type:ShaderForge.SFN_Vector1,id:6668,x:31702,y:33250,varname:node_6668,prsc:2,v1:1;n:type:ShaderForge.SFN_Time,id:1874,x:30469,y:33026,varname:Time,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:4078,x:30469,y:33224,ptovrint:False,ptlb:Offect1,ptin:_Offect1,varname:_Offect1,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:4011,x:30670,y:33105,varname:node_4011,prsc:2|A-1874-T,B-4078-OUT;proporder:1684-8375-5043-2063-1755-4078;pass:END;sub:END;*/

Shader "Fish/UV_Panner" {
    Properties {
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _MainTex ("MainTex", 2D) = "white" {}
        _width ("width", Range(0, 10)) = 1.335804
        _Offect ("Offect", Range(-3.2, 3.2)) = 3.2
        _Angle ("Angle", Range(0, 90)) = 1
        _Offect1 ("Offect1", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
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
            #pragma multi_compile_fog
            #pragma exclude_renderers metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _width;
            uniform fixed4 _Color;
            uniform fixed _Offect;
            uniform float _Angle;
            uniform float _Offect1;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
                fixed4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 Time = _Time + _TimeEditor;
                float3 finalColor = (_MainTex_var.rgb+(clamp(pow(sin(((lerp(i.uv0.r,i.uv0.g,(_Angle*0.01111111+0.0))*3.14+0.0)+(Time.g*_Offect1)+_Offect)),exp((_width*-0.9+10.0))),0.01,1.0)*2.0*_Color.rgb));
                fixed4 finalRGBA = fixed4(finalColor,_MainTex_var.a);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
