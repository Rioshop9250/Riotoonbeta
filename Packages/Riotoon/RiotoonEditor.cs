#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class RiotoonEditor : ShaderGUI
{
    private static bool isJP = true;
    bool s_Main = true, s_Press = false, s_Audio = false, s_Shading = false;
    bool s_MatCap = false, s_Rim = false, s_Glitter = false, s_Outline = false, s_DPS = false;
    bool s_MainTex1 = true, s_MainTex2 = false, s_MainTex3 = false;
    bool s_URL = true;
    bool s_matcap1 = true,s_matcap2 = false, s_matcap3 = false;
    bool s_rim = true;

    // 単一項目コピー用の静的変数
    private static object copiedValue = null;
    private static MaterialProperty.PropType copiedType;

    // カテゴリ一括コピー用の静的変数（プロパティ名と値のペア）
    private static Dictionary<string, object> copiedSectionProps = null;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {
        Material mat = (Material)materialEditor.target;

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Toggle(isJP, "日本語 (JP)", "Button")) isJP = true;
        if (GUILayout.Toggle(!isJP, "English (EN)", "Button")) isJP = false;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        // --- プロパティの配列定義（一括コピペ用） ---
        string[] mainAllProps = { "_RenderMode", "_Cutoff", "_Color", "_MainTex", "_BumpMap", "_BumpScale", "_MainTex2", "_MainTexMask2", "_MainTex3", "_MainTexMask3", "_UseSkinPores", "_PoreScale", "_PoreStrength" };
        string[] tex1Props = { "_Color", "_MainTex", "_BumpMap", "_BumpScale" };
        string[] tex2Props = { "_MainTex2", "_MainTexMask2" };
        string[] tex3Props = { "_MainTex3", "_MainTexMask3" };
        string[] pressProps = { "_UseDeform", "_MaskTex", "_FlattenStrength", "_Radius", "_MaxPush", "_PressShadowIten", "_HideOutlineOnPress", "_SocketPos", "_SocketNormal", "_SocketRight", "_SocketUp", "_SocketWidth", "_SocketHeight" };
        string[] audioProps = { "_UseAudioLink", "_AudioMask", "_ALBand", "_ALEEmissionFreq", "_ALVertexScale" };
        string[] shadingProps = { "_ShadowColor", "_ShadowColor2", "_ShadowThreshold", "_ShadowThreshold2", "_ShadowFeather", "_MinBrightness", "_MaxBrightness", "_ReceiveShadow", "_UseHalftone", "_HalftoneSize", "_HalftoneAmount" };
        string[] effectProps = { "_UseMatCap", "_MatCapBlendMode", "_MatCapNormal", "_MatCapBlur", "_MatCapTex", "_MatCapMask", "_MatCapColor", "_MatCapStrength", "_MatCapTex2", "_MatCapMask2", "_MatCapColor2", "_MatCapStrength2", "_MatCapTex3", "_MatCapMask3", "_MatCapColor3", "_MatCapStrength3", "_UseRim", "_RimBlendMode", "_RimMask", "_RimColor", "_RimPower", "_RimWidth", "_RimFeather", "_UseGlitter", "_GlitterColor", "_GlitterSize", "_GlitterThreshold" };
        string[] outlineProps = { "_StencilRef", "_StencilComp", "_StencilPass", "_UseOutline", "_OutlineColor", "_OutlineWidth", "_OutlineMask" };
        string[] dpsProps = { "_UseDPS" };

        // 1. MAIN
        s_URL = DrawHeader(isJP ? "公式URL" : "Official URL", s_URL, mainAllProps, props, materialEditor);
        EditorGUILayout.Space();
        GUILayout.Label(isJP ? "URLをクリックするとページが開けます" : "Click the URL to open the page.", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        if (s_URL)
            if (GUILayout.Button("公式X(Twitter)=https://x.com/riogames925", EditorStyles.linkLabel))
        {
            Application.OpenURL("https://x.com/riogames925");
        }
        EditorGUILayout.Space();
        if (s_URL)
            if (GUILayout.Button("公式booth=https://rio2.booth.pm/", EditorStyles.linkLabel))
            {
                Application.OpenURL("https://rio2.booth.pm/?_gl=1*tcb5sn*_ga*MTU2MjIwNzU1NS4xNzY2MTQwNjY3*_ga_RWT2QKJLDC*czE3ODQ1MjM1MTQkbzIkZzEkdDE3ODQ1MjQ4OTgkajkkbDAkaDA.");
            }
        EditorGUILayout.Space();
        s_Main = DrawHeader(isJP ? "基本設定 & テクスチャ (最大3枚)" : "Main Settings", s_Main, mainAllProps, props, materialEditor);
        if (s_Main)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                EditorGUI.BeginChangeCheck();
                var renderMode = FindPropSafe("_RenderMode", props);
                if (renderMode != null)
                {
                    materialEditor.ShaderProperty(renderMode, "Render Mode");
                    HandleContextMenu(GUILayoutUtility.GetLastRect(), renderMode, materialEditor);
                }

                if (EditorGUI.EndChangeCheck())
                {
                    int mode = (int)renderMode.floatValue;
                    if (mode == 0) // Opaque
                    {
                        mat.SetOverrideTag("RenderType", "Opaque");
                        mat.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
                        mat.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.Zero);
                        mat.SetFloat("_ZWrite", 1f);
                        mat.renderQueue = -1;
                    }
                    else if (mode == 1) // Cutout
                    {
                        mat.SetOverrideTag("RenderType", "TransparentCutout");
                        mat.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
                        mat.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.Zero);
                        mat.SetFloat("_ZWrite", 1f);
                        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
                    }
                    else if (mode == 2) // Transparent
                    {
                        mat.SetOverrideTag("RenderType", "Transparent");
                        mat.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        mat.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        mat.SetFloat("_ZWrite", 0f);
                        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    }
                }

                if (mat.HasProperty("_RenderMode") && mat.GetFloat("_RenderMode") == 1f)
                    DrawProp(materialEditor, props, "_Cutoff", "Alpha Cutoff");

                s_MainTex1 = DrawHeader(isJP ? "メインテクスチャー" : "Main Texture", s_MainTex1, tex1Props, props, materialEditor);
                if (s_MainTex1)
                {
                    EditorGUILayout.BeginVertical("box");
                    DrawProp(materialEditor, props, "_Color", "Main Color");
                    DrawTex(materialEditor, props, "_MainTex", "Main Texture 1");
                    DrawTex(materialEditor, props, "_BumpMap", "Normal Map");
                    DrawProp(materialEditor, props, "_BumpScale", "Normal Scale");
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.Space();

                s_MainTex2 = DrawHeader(isJP ? "2テクスチャー" : "Texture 2", s_MainTex2, tex2Props, props, materialEditor);
                if (s_MainTex2)
                {
                    DrawTex(materialEditor, props, "_MainTex2", "Main Texture 2");
                    DrawTex(materialEditor, props, "_MainTexMask2", "Mask for Tex 2");
                }
                EditorGUILayout.Space();
                s_MainTex3 = DrawHeader(isJP ? "3テクスチャー" : "Texture 3", s_MainTex3, tex3Props, props, materialEditor);
                if (s_MainTex3)
                {
                    DrawTex(materialEditor, props, "_MainTex3", "Main Texture 3");
                    DrawTex(materialEditor, props, "_MainTexMask3", "Mask for Tex 3");
                }
                EditorGUILayout.Space();
                GUILayout.Label(isJP ? "プロシージャル毛穴 (テクスチャ不要)" : "Procedural Skin Pores", EditorStyles.boldLabel);
                DrawProp(materialEditor, props, "_UseSkinPores", "Enable Pores");
                if (mat.HasProperty("_UseSkinPores") && mat.GetFloat("_UseSkinPores") > 0.5f)
                {
                    DrawProp(materialEditor, props, "_PoreScale", "Pore Scale (Detail Size)");
                    DrawProp(materialEditor, props, "_PoreStrength", "Pore Depth/Strength");
                }
            }
        }

        // 2. PHYSICAL PRESS
        s_Press = DrawHeader(isJP ? "物理プレス変形 (座標設定)" : "Physical Press", s_Press, pressProps, props, materialEditor);
        if (s_Press)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                DrawProp(materialEditor, props, "_UseDeform", "Enable Press");
                DrawTex(materialEditor, props, "_MaskTex", "Deform Mask (R)");
                DrawProp(materialEditor, props, "_FlattenStrength", isJP ? "吸着強度 (1.0で完全固定)" : "Press Strength");
                DrawProp(materialEditor, props, "_Radius", "Smoothing Radius");
                DrawProp(materialEditor, props, "_MaxPush", "Max Depth");
                DrawProp(materialEditor, props, "_PressShadowIten", "Press Shadow Intensity");
                DrawProp(materialEditor, props, "_HideOutlineOnPress", "Hide Outline On Press");
                EditorGUILayout.Space();
                DrawProp(materialEditor, props, "_SocketPos", "Socket Position");
                DrawProp(materialEditor, props, "_SocketNormal", "Socket Normal");
                DrawProp(materialEditor, props, "_SocketRight", "Socket Right");
                DrawProp(materialEditor, props, "_SocketUp", "Socket Up");
                DrawProp(materialEditor, props, "_SocketWidth", "Socket Width");
                DrawProp(materialEditor, props, "_SocketHeight", "Socket Height");
            }
        }

        // 3. AUDIOLINK
        s_Audio = DrawHeader(isJP ? "オーディオリンク (AudioLink)" : "AudioLink Settings", s_Audio, audioProps, props, materialEditor);
        if (s_Audio)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                DrawProp(materialEditor, props, "_UseAudioLink", "Enable AudioLink");
                DrawTex(materialEditor, props, "_AudioMask", "Impact Mask (R)");
                DrawProp(materialEditor, props, "_ALBand", "Band (0:Bass - 3:High)");
                DrawProp(materialEditor, props, "_ALEEmissionFreq", "Emission Power");
                DrawProp(materialEditor, props, "_ALVertexScale", "Vertex Shake");
            }
        }

        // 4. SHADING
        s_Shading = DrawHeader(isJP ? "陰影・ハーフトーン" : "Advanced Shading", s_Shading, shadingProps, props, materialEditor);
        if (s_Shading)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                DrawProp(materialEditor, props, "_ShadowColor", "1st Shadow Color");
                DrawProp(materialEditor, props, "_ShadowColor2", "2nd Shadow Color");
                DrawProp(materialEditor, props, "_ShadowThreshold", "1st Threshold");
                DrawProp(materialEditor, props, "_ShadowThreshold2", "2nd Threshold");
                DrawProp(materialEditor, props, "_ShadowFeather", "Shadow Feather");
                DrawProp(materialEditor, props, "_MinBrightness", "Min Brightness");
                DrawProp(materialEditor, props, "_MaxBrightness", "Max Brightness");
                DrawProp(materialEditor, props, "_ReceiveShadow", "Receive Shadow Influcence");
                EditorGUILayout.Space();
                DrawProp(materialEditor, props, "_UseHalftone", isJP ? "ハーフトーン (動的サイズ)" : "Dynamic Halftone");
                if (mat.HasProperty("_UseHalftone") && mat.GetFloat("_UseHalftone") > 0.5f)
                {
                    DrawProp(materialEditor, props, "_HalftoneSize", "Base Dot Size");
                    DrawProp(materialEditor, props, "_HalftoneAmount", "Dot Density/Sharpness");
                }

                EditorGUILayout.Space();
                //GUI.backgroundColor = new Color(0.7f, 0.9f, 1f);
                //if (GUILayout.Button(isJP ? "メインテクスチャから影マスクを生成 (Bake)" : "Bake Shadow Mask from MainTex", GUILayout.Height(30)))
               // {
               //     BakeShadowMask(mat);
               // }
                GUI.backgroundColor = Color.white;
            }
        }

        // 5. VISUAL EFFECTS
        s_MatCap = DrawHeader(isJP ? "特効 (MatCap x3 / Rim / Glitter)" : "Visual Effects", s_MatCap, effectProps, props, materialEditor);
        if (s_MatCap)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                GUILayout.Label("--- MatCap Settings ---", EditorStyles.boldLabel);
                DrawProp(materialEditor, props, "_UseMatCap", "Enable MatCap");
                DrawProp(materialEditor, props, "_MatCapBlendMode", "MatCap Blend Mode");
                DrawProp(materialEditor, props, "_MatCapNormal", "Normal Influence");
                DrawProp(materialEditor, props, "_MatCapBlur", "MatCap Blur");

                EditorGUILayout.Space();
                s_matcap1 = DrawHeader(isJP ? "マットキャップ1" : "matcap1", s_matcap1, mainAllProps, props, materialEditor);
                if (s_matcap1)
                {
                    DrawTex(materialEditor, props, "_MatCapTex", "MatCap 1 Texture");
                    DrawTex(materialEditor, props, "_MatCapMask", "MatCap 1 Mask");
                    DrawProp(materialEditor, props, "_MatCapColor", "MatCap 1 Color");
                    DrawProp(materialEditor, props, "_MatCapStrength", "MatCap 1 Intensity");
                }
                EditorGUILayout.Space();
                s_matcap2 = DrawHeader(isJP ? "マットキャップ2" : "matcap2", s_matcap2, mainAllProps, props, materialEditor);
                if (s_matcap2)
                {
                    DrawTex(materialEditor, props, "_MatCapTex2", "MatCap 2 Texture");
                    DrawTex(materialEditor, props, "_MatCapMask2", "MatCap 2 Mask");
                    DrawProp(materialEditor, props, "_MatCapColor2", "MatCap 2 Color");
                    DrawProp(materialEditor, props, "_MatCapStrength2", "MatCap 2 Intensity");
                }
                EditorGUILayout.Space();
                s_matcap3 = DrawHeader(isJP ? "マットキャップ3" : "matcap3", s_matcap3, mainAllProps, props, materialEditor);
                if (s_matcap3)
                {
                
                    DrawTex(materialEditor, props, "_MatCapTex3", "MatCap 3 Texture");
                    DrawTex(materialEditor, props, "_MatCapMask3", "MatCap 3 Mask");
                    DrawProp(materialEditor, props, "_MatCapColor3", "MatCap 3 Color");
                    DrawProp(materialEditor, props, "_MatCapStrength3", "MatCap 3 Intensity");
                }
                EditorGUILayout.Space();
                s_rim = DrawHeader(isJP ? "リムライト" : "rimlight", s_rim, mainAllProps, props, materialEditor);
                if (s_rim)
                {
                    GUILayout.Label("--- RimLight & Glitter ---", EditorStyles.boldLabel);
                    DrawProp(materialEditor, props, "_UseRim", "Enable RimLight");
                    DrawProp(materialEditor, props, "_RimBlendMode", "Rim Blend Mode");
                    DrawTex(materialEditor, props, "_RimMask", "Rim Mask");
                    DrawProp(materialEditor, props, "_RimColor", "Rim Color");
                    DrawProp(materialEditor, props, "_RimPower", "Rim Power");
                    DrawProp(materialEditor, props, "_RimWidth", "Rim Width");
                    DrawProp(materialEditor, props, "_RimFeather", "Rim Feather");
                }

                EditorGUILayout.Space();
                DrawProp(materialEditor, props, "_UseGlitter", "Enable Glitter");
                DrawProp(materialEditor, props, "_GlitterColor", "Color");
                DrawProp(materialEditor, props, "_GlitterSize", "Scale");
                DrawProp(materialEditor, props, "_GlitterThreshold", "Density");
            }
        }

        // 6. STENCIL & OUTLINE
        s_Outline = DrawHeader(isJP ? "ステンシル & 輪郭線 (マスク対応)" : "Stencil & Outline", s_Outline, outlineProps, props, materialEditor);
        if (s_Outline)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                DrawProp(materialEditor, props, "_StencilRef", "Stencil Ref");
                DrawProp(materialEditor, props, "_StencilComp", "Stencil Comp");
                DrawProp(materialEditor, props, "_StencilPass", "Stencil Pass");
                EditorGUILayout.Space();
                DrawProp(materialEditor, props, "_UseOutline", "Enable Outline");
                DrawProp(materialEditor, props, "_OutlineColor", "Outline Color");
                DrawProp(materialEditor, props, "_OutlineWidth", "Outline Width");
                DrawTex(materialEditor, props, "_OutlineMask", "Outline Width Mask (R)");
            }
        }

        // 7. DPS
        s_DPS = DrawHeader(isJP ? "DPS 対応設定(開発段階中)" : "DPS Settings", s_DPS, dpsProps, props, materialEditor);
        if (s_DPS)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                DrawProp(materialEditor, props, "_UseDPS", "Enable DPS (Requires Package)");
                EditorGUILayout.HelpBox(isJP ? "※Raliv等のDPSパッケージが必要です。\nコード内の `#include` コメントを外してご利用ください。" : "Requires Raliv DPS package installed. Uncomment #include in shader file.", MessageType.Info);
            }
        }

        materialEditor.RenderQueueField();
    }

    // --- ヘッダー描画 ＆ カテゴリ一括コピペ処理 ---
    bool DrawHeader(string title, bool state, string[] propNames = null, MaterialProperty[] allProps = null, MaterialEditor editor = null)
    {
        Rect r = EditorGUILayout.GetControlRect(true, 24f);
        GUI.Box(r, "", "Button");

        // 右クリックメニュー（カテゴリ一括）
        Event e = Event.current;
        if (e.type == EventType.ContextClick && r.Contains(e.mousePosition))
        {
            if (propNames != null && propNames.Length > 0 && allProps != null)
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent(isJP ? "この項目の設定をコピー" : "Copy Section Settings"), false, () => CopySection(propNames, allProps));

                if (copiedSectionProps != null && copiedSectionProps.Count > 0)
                {
                    menu.AddItem(new GUIContent(isJP ? "設定を貼り付け" : "Paste Section Settings"), false, () => PasteSection(propNames, allProps, editor));
                }
                else
                {
                    menu.AddDisabledItem(new GUIContent(isJP ? "設定を貼り付け" : "Paste Section Settings"));
                }
                menu.ShowAsContext();
                e.Use();
            }
        }

        return EditorGUI.Foldout(r, state, title, true, new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold });
    }

    // --- 一括コピー＆ペースト関数 ---
    private void CopySection(string[] propNames, MaterialProperty[] allProps)
    {
        copiedSectionProps = new Dictionary<string, object>();
        foreach (string name in propNames)
        {
            var prop = FindPropSafe(name, allProps);
            if (prop != null)
            {
                switch (prop.type)
                {
                    case MaterialProperty.PropType.Float:
                    case MaterialProperty.PropType.Range: copiedSectionProps[name] = prop.floatValue; break;
                    case MaterialProperty.PropType.Color: copiedSectionProps[name] = prop.colorValue; break;
                    case MaterialProperty.PropType.Vector: copiedSectionProps[name] = prop.vectorValue; break;
                    case MaterialProperty.PropType.Texture: copiedSectionProps[name] = prop.textureValue; break;
                }
            }
        }
    }

    private void PasteSection(string[] propNames, MaterialProperty[] allProps, MaterialEditor editor)
    {
        if (copiedSectionProps == null) return;

        editor.RegisterPropertyChangeUndo("Paste Section Properties");
        foreach (string name in propNames)
        {
            if (copiedSectionProps.ContainsKey(name))
            {
                var prop = FindPropSafe(name, allProps);
                if (prop != null)
                {
                    switch (prop.type)
                    {
                        case MaterialProperty.PropType.Float:
                        case MaterialProperty.PropType.Range: prop.floatValue = (float)copiedSectionProps[name]; break;
                        case MaterialProperty.PropType.Color: prop.colorValue = (Color)copiedSectionProps[name]; break;
                        case MaterialProperty.PropType.Vector: prop.vectorValue = (Vector4)copiedSectionProps[name]; break;
                        case MaterialProperty.PropType.Texture: prop.textureValue = (Texture)copiedSectionProps[name]; break;
                    }
                }
            }
        }
    }

    // 安全にプロパティを取得
    MaterialProperty FindPropSafe(string name, MaterialProperty[] props)
    {
        return FindProperty(name, props, false);
    }

    void DrawProp(MaterialEditor e, MaterialProperty[] p, string n, string l)
    {
        var prop = FindPropSafe(n, p);
        if (prop != null)
        {
            e.ShaderProperty(prop, l);
            HandleContextMenu(GUILayoutUtility.GetLastRect(), prop, e);
        }
    }

    void DrawTex(MaterialEditor e, MaterialProperty[] p, string n, string l)
    {
        var prop = FindPropSafe(n, p);
        if (prop != null)
        {
            e.TexturePropertySingleLine(new GUIContent(l), prop);
            HandleContextMenu(GUILayoutUtility.GetLastRect(), prop, e);
        }
    }

    // --- 右クリック（単一項目）処理群 ---
    private void HandleContextMenu(Rect rect, MaterialProperty prop, MaterialEditor materialEditor)
    {
        Event e = Event.current;
        if (e.type == EventType.ContextClick && rect.Contains(e.mousePosition))
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent(isJP ? "値をコピー (Copy)" : "Copy Value"), false, () => CopyProperty(prop));

            if (copiedValue != null && copiedType == prop.type)
            {
                menu.AddItem(new GUIContent(isJP ? "値を貼り付け (Paste)" : "Paste Value"), false, () => PasteProperty(prop, materialEditor));
            }
            else
            {
                menu.AddDisabledItem(new GUIContent(isJP ? "値を貼り付け (Paste)" : "Paste Value"));
            }
            menu.ShowAsContext();
            e.Use();
        }
    }

    private void CopyProperty(MaterialProperty prop)
    {
        copiedType = prop.type;
        switch (prop.type)
        {
            case MaterialProperty.PropType.Float:
            case MaterialProperty.PropType.Range: copiedValue = prop.floatValue; break;
            case MaterialProperty.PropType.Color: copiedValue = prop.colorValue; break;
            case MaterialProperty.PropType.Vector: copiedValue = prop.vectorValue; break;
            case MaterialProperty.PropType.Texture: copiedValue = prop.textureValue; break;
        }
    }

    private void PasteProperty(MaterialProperty prop, MaterialEditor materialEditor)
    {
        materialEditor.RegisterPropertyChangeUndo("Paste Material Property");
        switch (prop.type)
        {
            case MaterialProperty.PropType.Float:
            case MaterialProperty.PropType.Range: prop.floatValue = (float)copiedValue; break;
            case MaterialProperty.PropType.Color: prop.colorValue = (Color)copiedValue; break;
            case MaterialProperty.PropType.Vector: prop.vectorValue = (Vector4)copiedValue; break;
            case MaterialProperty.PropType.Texture: prop.textureValue = (Texture)copiedValue; break;
        }
    }

    // --- 影マスク生成（ベイク）処理 ---
    private void BakeShadowMask(Material mat)
    {
        Texture sourceTex = mat.GetTexture("_MainTex");
        if (sourceTex == null)
        {
            EditorUtility.DisplayDialog("エラー", "Main Texture 1 (_MainTex) が設定されていません。\nベイク元となるテクスチャをセットしてください。", "OK");
            return;
        }

        string path = EditorUtility.SaveFilePanelInProject("影マスクを保存", sourceTex.name + "_ShadowMask", "png", "保存先を選択してください");
        if (string.IsNullOrEmpty(path)) return;

        float threshold = mat.HasProperty("_ShadowThreshold") ? mat.GetFloat("_ShadowThreshold") : 0.5f;
        float feather = mat.HasProperty("_ShadowFeather") ? mat.GetFloat("_ShadowFeather") : 0.1f;

        RenderTexture rt = RenderTexture.GetTemporary(sourceTex.width, sourceTex.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        Graphics.Blit(sourceTex, rt);
        RenderTexture.active = rt;

        Texture2D readTex = new Texture2D(sourceTex.width, sourceTex.height, TextureFormat.RGBA32, false);
        readTex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        readTex.Apply();

        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);

        Color[] pixels = readTex.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            Color c = pixels[i];
            float luminance = c.r * 0.299f + c.g * 0.587f + c.b * 0.114f;

            float min = threshold - feather;
            float max = threshold + feather;
            float maskValue = Mathf.Clamp01((luminance - min) / (max - min));

            pixels[i] = new Color(maskValue, maskValue, maskValue, 1f);
        }

        readTex.SetPixels(pixels);
        readTex.Apply();

        byte[] bytes = readTex.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
        AssetDatabase.Refresh();

        Object.DestroyImmediate(readTex);

        EditorUtility.DisplayDialog("ベイク完了", $"影マスクの生成が完了しました。\n保存先: {path}", "OK");
    }
}
#endif