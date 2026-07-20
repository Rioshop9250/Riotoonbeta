Shader "Rios /Riotoon"
{
    Properties
    {
        // --- [MAIN SETTINGS] ---
        [Enum(Opaque, 0, Cutout, 1, Transparent, 2)] _RenderMode("Render Mode", Float) = 0
        [HideInInspector] _SrcBlend("Src Blend", Float) = 1
        [HideInInspector] _DstBlend("Dst Blend", Float) = 0
        [HideInInspector] _ZWrite("Z Write", Float) = 1

        _MainTex("Main Texture 1", 2D) = "white" {}
        _MainTex2("Main Texture 2", 2D) = "white" {}
        _MainTex3("Main Texture 3", 2D) = "white" {}
        _MainTexMask2("Texture 2 Mask", 2D) = "black" {}
        _MainTexMask3("Texture 3 Mask", 2D) = "black" {}

        _Color("Main Color", Color) = (1,1,1,1)
        _BumpMap("Normal Map", 2D) = "bump" {}
        _BumpScale("Normal Scale", Range(0, 2)) = 1.0
        _Cutoff("Alpha Cutoff (For Cutout)", Range(0, 1)) = 0.5

        // --- [PROCEDURAL SKIN PORES] ---
        [Toggle(_SKIN_PORES_ON)] _UseSkinPores("Enable Skin Pores", Float) = 0
        _PoreScale("Pore Scale", Float) = 200.0
        _PoreStrength("Pore Strength", Range(0, 1)) = 0.2

        // --- [PHYSICAL PRESS: MAX] ---
        [Toggle(_DEFORM_ON)] _UseDeform("Enable Press Deform", Float) = 0
        _MaskTex ("Deform Mask (R)", 2D) = "white" {}
        _FlattenStrength ("吸着強度 (1.0で完全固定)", Range(0,2)) = 1.0
        _Radius ("Smoothing Radius", Range(0,5)) = 0.5
        _MaxPush ("Max Push Depth", Range(0,1)) = 0.2
        _PressShadowIten("Press Area Shadow", Range(0, 1)) = 0.5
        [Toggle] _HideOutlineOnPress("Hide Outline On Press", Float) = 1

        _SocketPos("Socket Pos", Vector) = (0,0,0,0)
        _SocketNormal("Socket Norm", Vector) = (0,0,1,0)
        _SocketRight("Socket Right", Vector) = (1,0,0,0)
        _SocketUp("Socket Up", Vector) = (0,1,0,0)
        _SocketWidth("Socket Width", Float) = 0.5
        _SocketHeight("Socket Height", Float) = 0.5

        // --- [AUDIOLINK] ---
        [Toggle(_AUDIOLINK_ON)] _UseAudioLink("Enable AudioLink", Float) = 0
        _AudioMask("AudioLink Mask", 2D) = "white" {}
        _ALBand("Audio Band (0-3)", Int) = 0
        _ALEEmissionFreq("Emission Power", Range(0, 10)) = 0.0
        _ALVertexScale("Vertex Shake", Range(0, 1)) = 0.0

        // --- [SHADING & HIGH HARFTONE] ---
        _ShadowColor("1st Shadow Color", Color) = (0.7, 0.7, 0.8, 1)
        _ShadowColor2("2nd Shadow Color", Color) = (0.5, 0.5, 0.6, 1)
        _ShadowThreshold("1st Threshold", Range(0, 1)) = 0.5
        _ShadowThreshold2("2nd Threshold", Range(0, 1)) = 0.3
        _ShadowFeather("Shadow Feather", Range(0, 0.5)) = 0.05
        _MinBrightness("Min Brightness", Range(0, 1)) = 0.0
        _MaxBrightness("Max Brightness", Range(0, 2)) = 1.0
        _ReceiveShadow("Receive Other Shadows", Range(0, 1)) = 1.0
        
        [Toggle(_HALFTONE_ON)] _UseHalftone("Use Dynamic Halftone", Float) = 0
        _HalftoneSize("Halftone Base Size", Float) = 100.0
        _HalftoneAmount("Halftone Sharpness", Range(0, 1)) = 0.5

        // --- [VISUAL EFFECTS (MATCAP x3)] ---
        [Toggle(_MATCAP_ON)] _UseMatCap("Enable MatCap", Float) = 0
        [Enum(Add,0,Multiply,1)] _MatCapBlendMode("Blend Mode", Float) = 0
        _MatCapBlur("MatCap Blur", Range(0, 10)) = 0.0
        _MatCapNormal("Normal Influence", Range(0, 1)) = 1.0

        _MatCapTex("MatCap 1 Texture", 2D) = "black" {}
        _MatCapMask("MatCap 1 Mask", 2D) = "white" {}
        _MatCapColor("MatCap 1 Color", Color) = (1,1,1,1)
        _MatCapStrength("MatCap 1 Intensity", Range(0, 5)) = 1.0

        _MatCapTex2("MatCap 2 Texture", 2D) = "black" {}
        _MatCapMask2("MatCap 2 Mask", 2D) = "black" {}
        _MatCapColor2("MatCap 2 Color", Color) = (1,1,1,1)
        _MatCapStrength2("MatCap 2 Intensity", Range(0, 5)) = 1.0

        _MatCapTex3("MatCap 3 Texture", 2D) = "black" {}
        _MatCapMask3("MatCap 3 Mask", 2D) = "black" {}
        _MatCapColor3("MatCap 3 Color", Color) = (1,1,1,1)
        _MatCapStrength3("MatCap 3 Intensity", Range(0, 5)) = 1.0

        [Toggle(_RIM_ON)] _UseRim("Enable Rim Light", Float) = 0
        [Enum(Add,0,Mix,1)] _RimBlendMode("Blend Mode", Float) = 0
        _RimMask("Rim Mask", 2D) = "white" {}
        _RimColor("Rim Color", Color) = (1,1,1,1)
        _RimPower("Rim Power", Range(0.1, 10)) = 3.0
        _RimWidth("Rim Width", Range(0, 1)) = 0.5
        _RimFeather("Rim Feather", Range(0, 1)) = 0.1

        [Toggle(_GLITTER_ON)] _UseGlitter("Enable Glitter", Float) = 0
        _GlitterColor("Glitter Color", Color) = (1,1,1,1)
        _GlitterSize("Glitter Scale", Float) = 100.0
        _GlitterThreshold("Glitter Density", Range(0.9, 0.999)) = 0.98

        // --- [STENCIL & OUTLINE] ---
        _StencilRef("Stencil Ref", Int) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp("Stencil Comp", Float) = 8
        [Enum(UnityEngine.Rendering.CompareFunction)] _StencilPass("Stencil Pass", Float) = 0

        [Toggle(_OUTLINE_ON)] _UseOutline("Enable Outline", Float) = 0
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth("Outline Width", Range(0, 1)) = 0.1
        _OutlineMask("Outline Mask (R=Width)", 2D) = "white" {}

        // --- [DPS] ---
        [Toggle(_DPS_ON)] _UseDPS("Enable DPS", Float) = 0
        [HideInInspector] _PenetratorEnabled("Penetrator Enabled", Float) = 0
        [HideInInspector] _PenetratorLength("Penetrator Length", Float) = 0
        [HideInInspector] _PenetratorRadius("Penetrator Radius", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" "VRCFallback"="Toon" }
        Blend [_SrcBlend] [_DstBlend]
        ZWrite [_ZWrite]
        Stencil { Ref [_StencilRef] Comp [_StencilComp] Pass [_StencilPass] }

        CGINCLUDE
        #include "UnityCG.cginc"
        #include "AutoLight.cginc"
        #include "Lighting.cginc"

        uniform sampler2D _AudioData;
        
        sampler2D _MainTex, _MainTex2, _MainTex3, _MainTexMask2, _MainTexMask3, _BumpMap, _MaskTex, _AudioMask;
        sampler2D _MatCapTex, _MatCapMask, _MatCapTex2, _MatCapMask2, _MatCapTex3, _MatCapMask3, _RimMask;
        sampler2D _OutlineMask;
        float4 _MainTex_ST;
        
        float4 _Color, _ShadowColor, _ShadowColor2, _OutlineColor, _GlitterColor, _RimColor;
        float4 _MatCapColor, _MatCapColor2, _MatCapColor3;
        float3 _SocketPos, _SocketNormal, _SocketRight, _SocketUp;
        
        float _RenderMode, _BumpScale, _FlattenStrength, _Radius, _MaxPush, _SocketWidth, _SocketHeight;
        float _ShadowThreshold, _ShadowThreshold2, _ShadowFeather, _MinBrightness, _MaxBrightness, _Cutoff;
        float _ReceiveShadow, _UseAudioLink, _ALEEmissionFreq, _ALVertexScale, _HalftoneSize, _HalftoneAmount, _PressShadowIten, _HideOutlineOnPress;
        float _MatCapStrength, _MatCapStrength2, _MatCapStrength3, _MatCapBlur, _MatCapNormal, _MatCapBlendMode;
        float _RimPower, _RimWidth, _RimFeather, _RimBlendMode;
        float _GlitterSize, _GlitterThreshold, _OutlineWidth;
        float _PoreScale, _PoreStrength;
        int _ALBand;

        float GetAL(int band) {
            #ifdef _AUDIOLINK_ON
            return tex2Dlod(_AudioData, float4(float2(0.125 + 0.25 * band, 0.0625), 0, 0)).r;
            #endif
            return 0.0;
        }

        float ApplyPress(inout float3 wp, inout float3 wn, float2 uv) {
            float weight = 0.0;
            #ifdef _DEFORM_ON
            float m = tex2Dlod(_MaskTex, float4(uv, 0.0, 0.0)).r;
            float3 sn = normalize(_SocketNormal);
            float3 rel = wp - _SocketPos;
            float dist = dot(rel, sn);
            
            float wX = smoothstep(_SocketWidth * 0.5 + _Radius, _SocketWidth * 0.5, abs(dot(rel, normalize(_SocketRight))));
            float wY = smoothstep(_SocketHeight * 0.5 + _Radius, _SocketHeight * 0.5, abs(dot(rel, normalize(_SocketUp))));
            weight = wX * wY * smoothstep(0.01, -_Radius, dist) * m;

            wp -= sn * dist * weight * _FlattenStrength;
            wn = normalize(lerp(wn, sn, weight * _FlattenStrength));
            #endif
            return weight;
        }

        struct v2f {
            float4 pos : SV_POSITION;
            float2 uv : TEXCOORD0;
            float3 worldNormal : TEXCOORD1;
            float3 origNormal : TEXCOORD2;
            float3 worldPos : TEXCOORD3;
            float4 screenPos : TEXCOORD4;
            float3 viewDir : TEXCOORD5;
            float4 tangent : TEXCOORD6;
            float alPower : TEXCOORD7;
            SHADOW_COORDS(8)
        };
        ENDCG

        // ==========================================
        // 1. FORWARD PASS (メイン描画)
        // ==========================================
        Pass
        {
            Name "FORWARD"
            Tags { "LightMode"="ForwardBase" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma shader_feature_local _DEFORM_ON
            #pragma shader_feature_local _AUDIOLINK_ON
            #pragma shader_feature_local _HALFTONE_ON
            #pragma shader_feature_local _MATCAP_ON
            #pragma shader_feature_local _RIM_ON
            #pragma shader_feature_local _GLITTER_ON
            #pragma shader_feature_local _SKIN_PORES_ON
            #pragma shader_feature_local _DPS_ON

            v2f vert (appdata_full v) {
                v2f o;
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

                #ifdef _DPS_ON
                // #include "Assets/DynamicPenetrationSystem/Plugins/RalivPenetration.cginc"
                // if (_PenetratorEnabled) { v.vertex.xyz = applyPenetratorDistortion(v.vertex.xyz); }
                #endif

                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.origNormal = o.worldNormal;
                o.tangent = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w);
                
                o.alPower = GetAL(_ALBand);
                float alMask = tex2Dlod(_AudioMask, float4(o.uv, 0.0, 0.0)).r;
                o.worldPos += o.worldNormal * o.alPower * _ALVertexScale * alMask;
                
                ApplyPress(o.worldPos, o.worldNormal, o.uv);

                o.pos = UnityWorldToClipPos(float4(o.worldPos, 1.0));
                o.screenPos = ComputeScreenPos(o.pos);
                o.viewDir = normalize(_WorldSpaceCameraPos - o.worldPos);
                TRANSFER_SHADOW(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // Main Textures
                fixed4 base = tex2D(_MainTex, i.uv);
                fixed4 base2 = tex2D(_MainTex2, i.uv);
                fixed4 base3 = tex2D(_MainTex3, i.uv);
                float mask2 = tex2D(_MainTexMask2, i.uv).r;
                float mask3 = tex2D(_MainTexMask3, i.uv).r;
                
                // アルファと色の両方を正しく合成
                base.rgb = lerp(base.rgb, base2.rgb, mask2 * base2.a);
                base.a = lerp(base.a, base2.a, mask2);
                base.rgb = lerp(base.rgb, base3.rgb, mask3 * base3.a);
                base.a = lerp(base.a, base3.a, mask3);
                base *= _Color;

                if (_RenderMode == 1 && base.a < _Cutoff) discard;

                float3 origN = normalize(i.origNormal);
                float3 viewDir = normalize(i.viewDir);
                float3 binormal = cross(origN, i.tangent.xyz) * i.tangent.w;
                float3x3 tbn = float3x3(i.tangent.xyz, binormal, origN);
                
                float3 texNormal = UnpackNormal(tex2D(_BumpMap, i.uv)) * float3(_BumpScale, _BumpScale, 1.0);
                
                #ifdef _SKIN_PORES_ON
                float2 poreUV = i.uv * _PoreScale;
                float pore = smoothstep(0.8, 1.0, abs(sin(poreUV.x) * sin(poreUV.y)));
                texNormal += float3(pore * _PoreStrength, pore * _PoreStrength, 0.0);
                #endif

                float3 n = normalize(mul(texNormal, tbn));

                float atten = lerp(1.0, SHADOW_ATTENUATION(i), _ReceiveShadow);
                float ndl = dot(n, _WorldSpaceLightPos0.xyz) * 0.5 + 0.5;
                float3 dummyP = i.worldPos; float3 dummyN = i.worldNormal;
                float pW = ApplyPress(dummyP, dummyN, i.uv);
                
                float shadowVal = ndl * atten * lerp(1.0, (1.0 - _PressShadowIten), pW);
                float sm1 = smoothstep(_ShadowThreshold - _ShadowFeather, _ShadowThreshold + _ShadowFeather, shadowVal);
                float sm2 = smoothstep(_ShadowThreshold2 - _ShadowFeather, _ShadowThreshold2 + _ShadowFeather, shadowVal);
                
                float shadingCol = sm1 * 0.6 + sm2 * 0.4;
                float finalShadow = clamp(shadingCol, _MinBrightness, _MaxBrightness);

                #ifdef _HALFTONE_ON
                float2 hUV = i.screenPos.xy / i.screenPos.w;
                hUV.x *= _ScreenParams.x / _ScreenParams.y;
                hUV *= _HalftoneSize;
                float2 grid = frac(hUV) - 0.5;
                float dist = length(grid);
                float dotSize = lerp(1.5, 0.0, saturate(shadowVal * 1.2)); 
                float blur = (1.0 - _HalftoneAmount) * 0.2 + 0.01;
                finalShadow = clamp(smoothstep(dotSize, dotSize + blur, dist), _MinBrightness, _MaxBrightness);
                #endif

                float3 lightColor = _LightColor0.rgb;
                float3 ambient = ShadeSH9(float4(n, 1.0));
                float3 envLight = saturate(lightColor + ambient + 0.3);

                float3 shadow1Col = base.rgb * _ShadowColor.rgb * envLight;
                float3 shadow2Col = base.rgb * _ShadowColor2.rgb * envLight;
                float3 currentShadowColor = lerp(shadow2Col, shadow1Col, sm1);
                float3 litColor = base.rgb * saturate(lightColor + ambient + 0.2);
                float3 finalCol = lerp(currentShadowColor, litColor, finalShadow);

                #ifdef _AUDIOLINK_ON
                finalCol += base.rgb * i.alPower * _ALEEmissionFreq * tex2D(_AudioMask, i.uv).r;
                #endif

                #ifdef _RIM_ON
                float rimDot = 1.0 - saturate(dot(n, viewDir));
                float rimIntensity = pow(smoothstep(1.0 - _RimWidth - _RimFeather, 1.0 - _RimWidth + _RimFeather, rimDot), _RimPower);
                float3 rim = rimIntensity * _RimColor.rgb * tex2D(_RimMask, i.uv).r * finalShadow;
                if (_RimBlendMode < 0.5) finalCol += rim;
                else finalCol = lerp(finalCol, _RimColor.rgb, rimIntensity * tex2D(_RimMask, i.uv).r * finalShadow);
                #endif

                #ifdef _MATCAP_ON
                float3 mcNormal = lerp(origN, n, _MatCapNormal);
                float3 viewNormal = mul((float3x3)UNITY_MATRIX_V, mcNormal);
                float2 matcapUV = viewNormal.xy * 0.5 + 0.5;
                
                float3 mc1 = tex2Dlod(_MatCapTex, float4(matcapUV, 0.0, _MatCapBlur)).rgb * _MatCapColor.rgb;
                float3 mc2 = tex2Dlod(_MatCapTex2, float4(matcapUV, 0.0, _MatCapBlur)).rgb * _MatCapColor2.rgb;
                float3 mc3 = tex2Dlod(_MatCapTex3, float4(matcapUV, 0.0, _MatCapBlur)).rgb * _MatCapColor3.rgb;
                
                float m1 = tex2D(_MatCapMask, i.uv).r * _MatCapStrength;
                float m2 = tex2D(_MatCapMask2, i.uv).r * _MatCapStrength2;
                float m3 = tex2D(_MatCapMask3, i.uv).r * _MatCapStrength3;

                if (_MatCapBlendMode < 0.5) {
                    finalCol += (mc1 * m1) + (mc2 * m2) + (mc3 * m3);
                } else {
                    finalCol = lerp(finalCol, finalCol * mc1, m1);
                    finalCol = lerp(finalCol, finalCol * mc2, m2);
                    finalCol = lerp(finalCol, finalCol * mc3, m3);
                }
                #endif

                #ifdef _GLITTER_ON
                float3 glitterPos = i.worldPos * _GlitterSize;
                float noise = frac(sin(dot(glitterPos, float3(12.9898, 78.233, 45.164))) * 43758.5453);
                finalCol += step(_GlitterThreshold, noise) * _GlitterColor.rgb * finalShadow;
                #endif

                return fixed4(finalCol, base.a);
            }
            ENDCG
        }

        // ==========================================
        // 2. OUTLINE PASS (アウトライン)
        // ==========================================
        Pass
        {
            Name "OUTLINE"
            Tags { "LightMode"="Always" }
            Cull Front
            CGPROGRAM
            #pragma vertex vert_out
            #pragma fragment frag_out
            #pragma shader_feature_local _DEFORM_ON
            #pragma shader_feature_local _AUDIOLINK_ON
            #pragma shader_feature_local _OUTLINE_ON

            struct v2f_o { 
                float4 pos : SV_POSITION; 
                float2 uv : TEXCOORD0; 
                float pressWeight : TEXCOORD1; // 潰れ具合をフラグメントに渡す
            };

            v2f_o vert_out (appdata_full v) {
                v2f_o o;
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                #ifndef _OUTLINE_ON
                    o.pos = float4(0.0, 0.0, 0.0, 0.0);
                    o.pressWeight = 0;
                    return o;
                #endif

                float3 wp = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 wn = UnityObjectToWorldNormal(v.normal);

                #ifdef _AUDIOLINK_ON
                wp += wn * GetAL(0) * _ALVertexScale * tex2Dlod(_AudioMask, float4(o.uv, 0.0, 0.0)).r;
                #endif

                float pW = ApplyPress(wp, wn, o.uv);
                o.pressWeight = pW; // フラグメントシェーダーに送る

                float outlineMask = tex2Dlod(_OutlineMask, float4(o.uv, 0.0, 0.0)).r;
                float width = _OutlineWidth * outlineMask * saturate(1.0 - (pW * _HideOutlineOnPress)) * 0.01;
                wp += wn * width;

                o.pos = UnityWorldToClipPos(float4(wp, 1.0));
                return o;
            }

            fixed4 frag_out (v2f_o i) : SV_Target {
                fixed4 base = tex2D(_MainTex, i.uv);

                // Cutoutモードの時に透明部分のアウトラインを消す
                if (_RenderMode == 1 && base.a < _Cutoff) discard;

                // 【対策1】潰れた時、メッシュ表面と輪郭線(裏面)が全く同じ平面になりZファイティング
                // （輪郭線の色が重なったり、薄くチラつく現象）を防ぐため、潰れている箇所は輪郭線を削除
                #ifdef _DEFORM_ON
                if (_HideOutlineOnPress > 0.5) {
                    clip(0.95 - i.pressWeight); // 95%以上潰れていたら描画自体を完全に破棄
                }
                #endif

                fixed4 outCol = _OutlineColor;

                // 【対策2】Transparent(半透明)モードの時、裏にある輪郭線が透けて本体を塗りつぶしてしまう問題対策
                // メインテクスチャのアルファ値を輪郭線のアルファにも掛けることで透明に合わせる
                if (_RenderMode == 2) {
                    outCol.a *= base.a;
                }

                return outCol;
            }
            ENDCG
        }

        // ==========================================
        // 3. SHADOWCASTER PASS (影を正しく落とす)
        // ==========================================
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode"="ShadowCaster" }
            ZWrite On
            CGPROGRAM
            #pragma vertex vertShadow
            #pragma fragment fragShadow
            #pragma multi_compile_shadowcaster
            #pragma shader_feature_local _DEFORM_ON
            #pragma shader_feature_local _AUDIOLINK_ON
            #pragma shader_feature_local _DPS_ON

            struct v2f_shadow {
                V2F_SHADOW_CASTER;
                float2 uv : TEXCOORD1;
            };

            v2f_shadow vertShadow(appdata_full v) {
                v2f_shadow o;
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

                #ifdef _DPS_ON
                // #include "Assets/DynamicPenetrationSystem/Plugins/RalivPenetration.cginc"
                // if (_PenetratorEnabled) { v.vertex.xyz = applyPenetratorDistortion(v.vertex.xyz); }
                #endif

                float3 wp = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 wn = UnityObjectToWorldNormal(v.normal);

                #ifdef _AUDIOLINK_ON
                wp += wn * GetAL(0) * _ALVertexScale * tex2Dlod(_AudioMask, float4(o.uv, 0.0, 0.0)).r;
                #endif

                ApplyPress(wp, wn, o.uv);
                v.vertex = mul(unity_WorldToObject, float4(wp, 1.0));

                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }

            fixed4 fragShadow(v2f_shadow i) : SV_Target {
                if (_RenderMode == 1 && tex2D(_MainTex, i.uv).a < _Cutoff) discard;
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    Fallback "Legacy Shaders/VertexLit"
    CustomEditor "RiotoonEditor"
}