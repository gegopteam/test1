// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.19 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.19;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:3138,x:33009,y:32657,varname:node_3138,prsc:2|emission-6151-OUT,custl-117-OUT,alpha-6644-OUT;n:type:ShaderForge.SFN_Tex2d,id:3755,x:30864,y:32999,ptovrint:False,ptlb:Mask Texture,ptin:_MaskTexture,varname:node_3755,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_If,id:5746,x:31505,y:32951,varname:node_5746,prsc:2|A-654-OUT,B-3755-A,GT-6272-OUT,EQ-6272-OUT,LT-3073-OUT;n:type:ShaderForge.SFN_Vector1,id:6272,x:31244,y:33041,varname:node_6272,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:3073,x:31244,y:33221,varname:node_3073,prsc:2,v1:0;n:type:ShaderForge.SFN_Tex2d,id:5620,x:31697,y:32545,ptovrint:False,ptlb:Diffuse Texture,ptin:_DiffuseTexture,varname:_node_3755_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:6731,x:32185,y:32854,varname:node_6731,prsc:2|A-5620-A,B-6450-OUT;n:type:ShaderForge.SFN_Color,id:7519,x:31697,y:32332,ptovrint:False,ptlb:Diffuse Color,ptin:_DiffuseColor,varname:node_7519,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.6985294,c2:0.6985294,c3:0.6985294,c4:1;n:type:ShaderForge.SFN_Multiply,id:3808,x:32158,y:32434,varname:node_3808,prsc:2|A-7519-RGB,B-5620-RGB;n:type:ShaderForge.SFN_Multiply,id:6644,x:32386,y:32755,varname:node_6644,prsc:2|A-7519-A,B-6731-OUT;n:type:ShaderForge.SFN_If,id:5217,x:31505,y:33152,varname:node_5217,prsc:2|A-654-OUT,B-2022-OUT,GT-6272-OUT,EQ-6272-OUT,LT-3073-OUT;n:type:ShaderForge.SFN_Add,id:2022,x:31104,y:33084,varname:node_2022,prsc:2|A-3755-B,B-5828-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5828,x:30864,y:33230,ptovrint:False,ptlb:N_BY_KD,ptin:_N_BY_KD,varname:node_5828,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.01;n:type:ShaderForge.SFN_Subtract,id:1274,x:31720,y:33015,varname:node_1274,prsc:2|A-5746-OUT,B-5217-OUT;n:type:ShaderForge.SFN_Color,id:9508,x:31720,y:33216,ptovrint:False,ptlb:C_BYcolor,ptin:_C_BYcolor,varname:node_9508,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:7346,x:31925,y:33049,varname:node_7346,prsc:2|A-1274-OUT,B-9508-RGB;n:type:ShaderForge.SFN_ValueProperty,id:8447,x:31720,y:33412,ptovrint:False,ptlb:N_BY_QD,ptin:_N_BY_QD,varname:node_8447,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_Multiply,id:7666,x:32245,y:33021,varname:node_7666,prsc:2|A-7346-OUT,B-8447-OUT;n:type:ShaderForge.SFN_Add,id:6450,x:31892,y:32859,varname:node_6450,prsc:2|A-5746-OUT,B-1274-OUT;n:type:ShaderForge.SFN_Multiply,id:4059,x:32358,y:32510,varname:node_4059,prsc:2|A-3808-OUT,B-6450-OUT;n:type:ShaderForge.SFN_Add,id:1014,x:32542,y:32653,varname:node_1014,prsc:2|A-4059-OUT,B-7666-OUT;n:type:ShaderForge.SFN_VertexColor,id:8945,x:30864,y:32679,varname:node_8945,prsc:2;n:type:ShaderForge.SFN_Multiply,id:654,x:31233,y:32835,varname:node_654,prsc:2|A-8945-A,B-8395-OUT;n:type:ShaderForge.SFN_Multiply,id:6151,x:32718,y:32562,varname:node_6151,prsc:2|A-7519-A,B-1014-OUT;n:type:ShaderForge.SFN_Multiply,id:117,x:32658,y:32902,varname:node_117,prsc:2|A-7519-A,B-7666-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8395,x:30864,y:32875,ptovrint:False,ptlb:node_8395,ptin:_node_8395,varname:node_8395,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;proporder:7519-5620-3755-9508-8447-5828-8395;pass:END;sub:END;*/

Shader "Fish/Dissolution_Add" {
    Properties {
        _DiffuseColor ("Diffuse Color", Color) = (0.6985294,0.6985294,0.6985294,1)
        _DiffuseTexture ("Diffuse Texture", 2D) = "white" {}
        _MaskTexture ("Mask Texture", 2D) = "white" {}
        _C_BYcolor ("C_BYcolor", Color) = (1,0,0,1)
        _N_BY_QD ("N_BY_QD", Float ) = 3
        _N_BY_KD ("N_BY_KD", Float ) = 0.01
        _node_8395 ("node_8395", Float ) = 1
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
            #pragma exclude_renderers xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _MaskTexture; uniform float4 _MaskTexture_ST;
            uniform sampler2D _DiffuseTexture; uniform float4 _DiffuseTexture_ST;
            uniform float4 _DiffuseColor;
            uniform float _N_BY_KD;
            uniform float4 _C_BYcolor;
            uniform float _N_BY_QD;
            uniform float _node_8395;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
/////// Vectors:
////// Lighting:
////// Emissive:
                float4 _DiffuseTexture_var = tex2D(_DiffuseTexture,TRANSFORM_TEX(i.uv0, _DiffuseTexture));
                float node_654 = (i.vertexColor.a*_node_8395);
                float4 _MaskTexture_var = tex2D(_MaskTexture,TRANSFORM_TEX(i.uv0, _MaskTexture));
                float node_5746_if_leA = step(node_654,_MaskTexture_var.a);
                float node_5746_if_leB = step(_MaskTexture_var.a,node_654);
                float node_3073 = 0.0;
                float node_6272 = 1.0;
                float node_5746 = lerp((node_5746_if_leA*node_3073)+(node_5746_if_leB*node_6272),node_6272,node_5746_if_leA*node_5746_if_leB);
                float node_5217_if_leA = step(node_654,(_MaskTexture_var.b+_N_BY_KD));
                float node_5217_if_leB = step((_MaskTexture_var.b+_N_BY_KD),node_654);
                float node_1274 = (node_5746-lerp((node_5217_if_leA*node_3073)+(node_5217_if_leB*node_6272),node_6272,node_5217_if_leA*node_5217_if_leB));
                float node_6450 = (node_5746+node_1274);
                float3 node_7666 = ((node_1274*_C_BYcolor.rgb)*_N_BY_QD);
                float3 emissive = (_DiffuseColor.a*(((_DiffuseColor.rgb*_DiffuseTexture_var.rgb)*node_6450)+node_7666));
                float3 finalColor = emissive + (_DiffuseColor.a*node_7666);
                return fixed4(finalColor,(_DiffuseColor.a*(_DiffuseTexture_var.a*node_6450)));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
