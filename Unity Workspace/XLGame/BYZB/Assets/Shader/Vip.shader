// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.28 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.28;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:826,x:33173,y:32028,varname:node_826,prsc:2|emission-7699-OUT,custl-1410-OUT;n:type:ShaderForge.SFN_Tex2d,id:8096,x:32131,y:32312,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:bf2ca24ffb51641b98881b08f832c014,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:760,x:32131,y:32575,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:26,x:32512,y:32471,varname:MainTex_Color,prsc:2|A-8096-RGB,B-760-RGB;n:type:ShaderForge.SFN_Tex2d,id:1565,x:32119,y:32026,ptovrint:False,ptlb:Emission,ptin:_Emission,varname:_Emission,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:9e64bb657e48f437ca491ed537e17413,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:7166,x:32386,y:32046,varname:node_7166,prsc:2|A-1565-RGB,B-760-RGB;n:type:ShaderForge.SFN_Multiply,id:5986,x:32779,y:32101,varname:node_5986,prsc:2|A-7166-OUT,B-7568-OUT;n:type:ShaderForge.SFN_Fresnel,id:912,x:32330,y:32902,varname:node_912,prsc:0;n:type:ShaderForge.SFN_Multiply,id:9521,x:32671,y:32690,varname:node_9521,prsc:0|A-7453-OUT,B-7226-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7453,x:32330,y:32704,ptovrint:False,ptlb:Fresnel,ptin:_Fresnel,varname:_Fresnel,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_ValueProperty,id:7568,x:32512,y:32268,ptovrint:False,ptlb:Emiss,ptin:_Emiss,varname:_Emiss,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1.3;n:type:ShaderForge.SFN_Multiply,id:1410,x:32804,y:32470,varname:node_1410,prsc:2|A-26-OUT,B-9521-OUT;n:type:ShaderForge.SFN_Add,id:7699,x:32912,y:32231,varname:node_7699,prsc:2|A-5986-OUT,B-26-OUT;n:type:ShaderForge.SFN_OneMinus,id:7226,x:32511,y:32889,varname:node_7226,prsc:2|IN-912-OUT;proporder:760-8096-1565-7568-7453;pass:END;sub:END;*/

Shader "Fish/Vip" {
    Properties {
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _MainTex ("MainTex", 2D) = "white" {}
        _Emission ("Emission", 2D) = "white" {}
        _Emiss ("Emiss", Float ) = 1.3
        _Fresnel ("Fresnel", Float ) = 0.1
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform fixed4 _Color;
            uniform sampler2D _Emission; uniform float4 _Emission_ST;
            uniform fixed _Fresnel;
            uniform fixed _Emiss;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                fixed4 _Emission_var = tex2D(_Emission,TRANSFORM_TEX(i.uv0, _Emission));
                fixed4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 MainTex_Color = (_MainTex_var.rgb*_Color.rgb);
                float3 emissive = (((_Emission_var.rgb*_Color.rgb)*_Emiss)+MainTex_Color);
                float3 finalColor = emissive + (MainTex_Color*(_Fresnel*(1.0 - (1.0-max(0,dot(normalDirection, viewDirection))))));
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
