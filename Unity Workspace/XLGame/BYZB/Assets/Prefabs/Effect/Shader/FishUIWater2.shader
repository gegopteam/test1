// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:1,cusa:True,bamd:0,cgin:,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:1,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:5095,x:34043,y:31997,varname:node_5095,prsc:2|diff-2361-OUT,spec-37-OUT,gloss-610-OUT,normal-8191-RGB,emission-8346-OUT,alpha-1047-OUT;n:type:ShaderForge.SFN_Time,id:6997,x:30523,y:32613,varname:WaveTime,prsc:2;n:type:ShaderForge.SFN_Multiply,id:2698,x:30809,y:32668,varname:WaveMult,prsc:2|A-6997-T,B-5495-OUT;n:type:ShaderForge.SFN_Vector1,id:2109,x:30438,y:32973,varname:node_2109,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Slider,id:5955,x:30299,y:32828,ptovrint:False,ptlb:WaveSpeed,ptin:_WaveSpeed,varname:_WaveSpeed,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0.75,max:5;n:type:ShaderForge.SFN_Multiply,id:5495,x:30621,y:32852,varname:node_5495,prsc:2|A-5955-OUT,B-2109-OUT;n:type:ShaderForge.SFN_TexCoord,id:4572,x:30768,y:32217,varname:node_4572,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:8472,x:31048,y:32240,varname:node_8472,prsc:2|A-4572-UVOUT,B-7070-OUT;n:type:ShaderForge.SFN_Vector1,id:7070,x:30768,y:32411,varname:node_7070,prsc:2,v1:-20;n:type:ShaderForge.SFN_Slider,id:3693,x:30397,y:32471,ptovrint:False,ptlb:TileValue,ptin:_TileValue,varname:_TileValue,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.9,max:1;n:type:ShaderForge.SFN_OneMinus,id:5168,x:30767,y:32479,varname:node_5168,prsc:2|IN-3693-OUT;n:type:ShaderForge.SFN_Multiply,id:7912,x:31260,y:32157,varname:TileMult,prsc:2|A-8472-OUT,B-5168-OUT;n:type:ShaderForge.SFN_Panner,id:5802,x:31560,y:32157,varname:PannerTex,prsc:0,spu:0,spv:1|UVIN-7912-OUT,DIST-2698-OUT;n:type:ShaderForge.SFN_Panner,id:6047,x:31531,y:32421,varname:PannerNormal,prsc:0,spu:1,spv:0|UVIN-7912-OUT,DIST-2698-OUT;n:type:ShaderForge.SFN_Tex2d,id:5668,x:32010,y:32247,ptovrint:False,ptlb:WaterMap,ptin:_WaterMap,varname:_WaterMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-2255-OUT;n:type:ShaderForge.SFN_Tex2d,id:8191,x:32013,y:32532,ptovrint:False,ptlb:NormalMap,ptin:_NormalMap,varname:_NormalMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True|UVIN-444-OUT;n:type:ShaderForge.SFN_Vector1,id:37,x:33740,y:31991,varname:node_37,prsc:2,v1:0;n:type:ShaderForge.SFN_Multiply,id:5706,x:32247,y:32185,varname:WaterColor_Mult,prsc:2|A-522-A,B-5668-B;n:type:ShaderForge.SFN_Color,id:522,x:32010,y:32008,ptovrint:False,ptlb:WaterColor,ptin:_WaterColor,varname:_WaterColor,prsc:1,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Lerp,id:232,x:33236,y:31962,varname:node_232,prsc:2|A-522-RGB,B-9096-OUT,T-9026-OUT;n:type:ShaderForge.SFN_OneMinus,id:5354,x:32502,y:32185,varname:node_5354,prsc:2|IN-5706-OUT;n:type:ShaderForge.SFN_Vector1,id:9096,x:32864,y:32052,varname:node_9096,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:610,x:32834,y:32578,varname:node_610,prsc:2|A-8191-B,B-5476-OUT;n:type:ShaderForge.SFN_Slider,id:5476,x:32462,y:32661,ptovrint:False,ptlb:Roughness,ptin:_Roughness,varname:_Roughness,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.7606838,max:1;n:type:ShaderForge.SFN_Clamp01,id:9026,x:32790,y:32188,varname:node_9026,prsc:2|IN-5354-OUT;n:type:ShaderForge.SFN_Multiply,id:1047,x:33785,y:32457,varname:node_1047,prsc:2|A-415-A,B-929-OUT;n:type:ShaderForge.SFN_Slider,id:929,x:33438,y:32542,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:_Opacity,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.7094017,max:1;n:type:ShaderForge.SFN_VertexColor,id:415,x:33568,y:32380,varname:node_415,prsc:2;n:type:ShaderForge.SFN_Time,id:1363,x:30467,y:31611,varname:PannerUV_Time,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:856,x:30289,y:32050,ptovrint:False,ptlb:UVPanner_U,ptin:_UVPanner_U,varname:_UVPanner_U,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:8793,x:30476,y:32094,ptovrint:False,ptlb:UVPanner_V,ptin:_UVPanner_V,varname:_UVPanner_V,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Tex2d,id:3615,x:32405,y:31484,ptovrint:False,ptlb:WaterMap_Emission,ptin:_WaterMap_Emission,varname:_WaterMap_Emission,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-2255-OUT;n:type:ShaderForge.SFN_Multiply,id:9062,x:30750,y:31820,varname:node_9062,prsc:2|A-1363-T,B-696-OUT,C-237-OUT;n:type:ShaderForge.SFN_Multiply,id:7804,x:30781,y:32020,varname:node_7804,prsc:2|A-1363-T,B-8793-OUT,C-237-OUT;n:type:ShaderForge.SFN_Vector1,id:237,x:30476,y:32238,varname:PannerUV_Value,prsc:0,v1:0.1;n:type:ShaderForge.SFN_Append,id:5458,x:30948,y:31880,varname:node_5458,prsc:2|A-9062-OUT,B-7804-OUT;n:type:ShaderForge.SFN_Add,id:8717,x:31137,y:31880,varname:UVPanner_Add,prsc:2|A-5458-OUT,B-4572-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:6631,x:31331,y:31805,ptovrint:False,ptlb:FlowMap,ptin:_FlowMap,varname:_FlowMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-8717-OUT;n:type:ShaderForge.SFN_Append,id:2996,x:31533,y:31888,varname:FlowMap_AppendUV,prsc:2|A-6631-R,B-6631-R;n:type:ShaderForge.SFN_Add,id:444,x:31775,y:32377,varname:NormalMap_Add,prsc:2|A-2996-OUT,B-6047-UVOUT;n:type:ShaderForge.SFN_Add,id:2255,x:31797,y:32008,varname:FlowMap_Add,prsc:2|A-2996-OUT,B-5802-UVOUT;n:type:ShaderForge.SFN_Add,id:1244,x:33294,y:31478,varname:node_1244,prsc:2|A-4608-OUT,B-8346-OUT;n:type:ShaderForge.SFN_Multiply,id:823,x:32600,y:31729,varname:node_823,prsc:2|A-6271-OUT,B-6505-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6505,x:32401,y:31904,ptovrint:False,ptlb:FlowMap_HighLight_Mult,ptin:_FlowMap_HighLight_Mult,varname:_FlowMap_HighLight_Mult,prsc:1,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Power,id:6271,x:32390,y:31729,varname:node_6271,prsc:2|VAL-6631-R,EXP-4270-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4270,x:32169,y:31829,ptovrint:False,ptlb:Power,ptin:_Power,varname:_Power,prsc:1,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:8346,x:32998,y:31772,varname:node_8346,prsc:2|A-823-OUT,B-8362-R;n:type:ShaderForge.SFN_Tex2d,id:8362,x:32762,y:31786,ptovrint:False,ptlb:FlowMaskMap,ptin:_FlowMaskMap,varname:_FlowMaskMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:8255,x:30289,y:31814,ptovrint:False,ptlb:PannerUV_UMask,ptin:_PannerUV_UMask,varname:_PannerUV_UMask,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:696,x:30476,y:31843,varname:node_696,prsc:2|A-8255-R,B-856-OUT;n:type:ShaderForge.SFN_Multiply,id:4608,x:33023,y:31478,varname:node_4608,prsc:2|A-3615-RGB,B-6273-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6273,x:32678,y:31602,ptovrint:False,ptlb:Emissive_Mul,ptin:_Emissive_Mul,varname:_Emissive_Mul,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:2361,x:33523,y:31703,varname:node_2361,prsc:2|A-1244-OUT,B-232-OUT;proporder:5955-3693-8191-522-5668-6631-8362-3615-5476-929-8255-856-8793-6505-4270-6273;pass:END;sub:END;*/

Shader "Fish/FishUIWater2" {
    Properties {
        _WaveSpeed ("WaveSpeed", Range(-5, 5)) = 0.75
        _TileValue ("TileValue", Range(0, 1)) = 0.9
        _NormalMap ("NormalMap", 2D) = "bump" {}
        _WaterColor ("WaterColor", Color) = (0.5,0.5,0.5,1)
        _WaterMap ("WaterMap", 2D) = "white" {}
        _FlowMap ("FlowMap", 2D) = "white" {}
        _FlowMaskMap ("FlowMaskMap", 2D) = "white" {}
        _WaterMap_Emission ("WaterMap_Emission", 2D) = "white" {}
        _Roughness ("Roughness", Range(0, 1)) = 0.7606838
        _Opacity ("Opacity", Range(0, 1)) = 0.7094017
        _PannerUV_UMask ("PannerUV_UMask", 2D) = "white" {}
        _UVPanner_U ("UVPanner_U", Float ) = 0
        _UVPanner_V ("UVPanner_V", Float ) = 0
        _FlowMap_HighLight_Mult ("FlowMap_HighLight_Mult", Float ) = 0
        _Power ("Power", Float ) = 0
        _Emissive_Mul ("Emissive_Mul", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
            "PreviewType"="Plane"
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
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 3.0
            uniform float _WaveSpeed;
            uniform float _TileValue;
            uniform sampler2D _WaterMap; uniform float4 _WaterMap_ST;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform half4 _WaterColor;
            uniform fixed _Roughness;
            uniform fixed _Opacity;
            uniform float _UVPanner_U;
            uniform float _UVPanner_V;
            uniform sampler2D _WaterMap_Emission; uniform float4 _WaterMap_Emission_ST;
            uniform sampler2D _FlowMap; uniform float4 _FlowMap_ST;
            uniform half _FlowMap_HighLight_Mult;
            uniform half _Power;
            uniform sampler2D _FlowMaskMap; uniform float4 _FlowMaskMap_ST;
            uniform sampler2D _PannerUV_UMask; uniform float4 _PannerUV_UMask_ST;
            uniform float _Emissive_Mul;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 PannerUV_Time = _Time;
                float4 _PannerUV_UMask_var = tex2D(_PannerUV_UMask,TRANSFORM_TEX(i.uv0, _PannerUV_UMask));
                fixed PannerUV_Value = 0.1;
                float2 UVPanner_Add = (float2((PannerUV_Time.g*(_PannerUV_UMask_var.r+_UVPanner_U)*PannerUV_Value),(PannerUV_Time.g*_UVPanner_V*PannerUV_Value))+i.uv0);
                float4 _FlowMap_var = tex2D(_FlowMap,TRANSFORM_TEX(UVPanner_Add, _FlowMap));
                float2 FlowMap_AppendUV = float2(_FlowMap_var.r,_FlowMap_var.r);
                float4 WaveTime = _Time;
                float WaveMult = (WaveTime.g*(_WaveSpeed*0.1));
                float2 TileMult = ((i.uv0*(-20.0))*(1.0 - _TileValue));
                float2 NormalMap_Add = (FlowMap_AppendUV+(TileMult+WaveMult*float2(1,0)));
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(NormalMap_Add, _NormalMap)));
                float3 normalLocal = _NormalMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = 1.0 - (_NormalMap_var.b*_Roughness); // Convert roughness to gloss
                float perceptualRoughness = (_NormalMap_var.b*_Roughness);
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = 0.0;
                float specularMonochrome;
                float2 FlowMap_Add = (FlowMap_AppendUV+(TileMult+WaveMult*float2(0,1)));
                float4 _WaterMap_Emission_var = tex2D(_WaterMap_Emission,TRANSFORM_TEX(FlowMap_Add, _WaterMap_Emission));
                float4 _FlowMaskMap_var = tex2D(_FlowMaskMap,TRANSFORM_TEX(i.uv0, _FlowMaskMap));
                float node_8346 = ((pow(_FlowMap_var.r,_Power)*_FlowMap_HighLight_Mult)*_FlowMaskMap_var.r);
                float node_9096 = 1.0;
                float4 _WaterMap_var = tex2D(_WaterMap,TRANSFORM_TEX(FlowMap_Add, _WaterMap));
                float3 diffuseColor = (((_WaterMap_Emission_var.rgb*_Emissive_Mul)+node_8346)*lerp(_WaterColor.rgb,float3(node_9096,node_9096,node_9096),saturate((1.0 - (_WaterColor.a*_WaterMap_var.b))))); // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = float3(node_8346,node_8346,node_8346);
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                return fixed4(finalColor,(i.vertexColor.a*_Opacity));
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 3.0
            uniform float _WaveSpeed;
            uniform float _TileValue;
            uniform sampler2D _WaterMap; uniform float4 _WaterMap_ST;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform half4 _WaterColor;
            uniform fixed _Roughness;
            uniform fixed _Opacity;
            uniform float _UVPanner_U;
            uniform float _UVPanner_V;
            uniform sampler2D _WaterMap_Emission; uniform float4 _WaterMap_Emission_ST;
            uniform sampler2D _FlowMap; uniform float4 _FlowMap_ST;
            uniform half _FlowMap_HighLight_Mult;
            uniform half _Power;
            uniform sampler2D _FlowMaskMap; uniform float4 _FlowMaskMap_ST;
            uniform sampler2D _PannerUV_UMask; uniform float4 _PannerUV_UMask_ST;
            uniform float _Emissive_Mul;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 PannerUV_Time = _Time;
                float4 _PannerUV_UMask_var = tex2D(_PannerUV_UMask,TRANSFORM_TEX(i.uv0, _PannerUV_UMask));
                fixed PannerUV_Value = 0.1;
                float2 UVPanner_Add = (float2((PannerUV_Time.g*(_PannerUV_UMask_var.r+_UVPanner_U)*PannerUV_Value),(PannerUV_Time.g*_UVPanner_V*PannerUV_Value))+i.uv0);
                float4 _FlowMap_var = tex2D(_FlowMap,TRANSFORM_TEX(UVPanner_Add, _FlowMap));
                float2 FlowMap_AppendUV = float2(_FlowMap_var.r,_FlowMap_var.r);
                float4 WaveTime = _Time;
                float WaveMult = (WaveTime.g*(_WaveSpeed*0.1));
                float2 TileMult = ((i.uv0*(-20.0))*(1.0 - _TileValue));
                float2 NormalMap_Add = (FlowMap_AppendUV+(TileMult+WaveMult*float2(1,0)));
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(NormalMap_Add, _NormalMap)));
                float3 normalLocal = _NormalMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = 1.0 - (_NormalMap_var.b*_Roughness); // Convert roughness to gloss
                float perceptualRoughness = (_NormalMap_var.b*_Roughness);
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = 0.0;
                float specularMonochrome;
                float2 FlowMap_Add = (FlowMap_AppendUV+(TileMult+WaveMult*float2(0,1)));
                float4 _WaterMap_Emission_var = tex2D(_WaterMap_Emission,TRANSFORM_TEX(FlowMap_Add, _WaterMap_Emission));
                float4 _FlowMaskMap_var = tex2D(_FlowMaskMap,TRANSFORM_TEX(i.uv0, _FlowMaskMap));
                float node_8346 = ((pow(_FlowMap_var.r,_Power)*_FlowMap_HighLight_Mult)*_FlowMaskMap_var.r);
                float node_9096 = 1.0;
                float4 _WaterMap_var = tex2D(_WaterMap,TRANSFORM_TEX(FlowMap_Add, _WaterMap));
                float3 diffuseColor = (((_WaterMap_Emission_var.rgb*_Emissive_Mul)+node_8346)*lerp(_WaterColor.rgb,float3(node_9096,node_9096,node_9096),saturate((1.0 - (_WaterColor.a*_WaterMap_var.b))))); // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                return fixed4(finalColor * (i.vertexColor.a*_Opacity),0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
