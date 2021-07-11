using System;
using UnityEditor;
using UnityEngine;

namespace Wolf3D.ReadyPlayerMe.AvatarSDK
{
    public class EditorWindowBase : EditorWindow
    {
        public const string Version = "v1.5.1";

        private const string BannerPath = "Assets/Plugins/Wolf3D Ready Player Me SDK/Editor/RPM_EditorImage_Banner.png";
        private const string LovePath = "Assets/Plugins/Wolf3D Ready Player Me SDK/Editor/RPM_EditorImage_Love.png";

        private const string RpmUrl = "https://readyplayer.me";
        private const string BlogUrl = "https://readyplayer.me/blog";
        private const string DocsUrl = "https://readyplayer.me/docs";
        private const string DiscordUrl = "https://discord.gg/UCvRaM2Hm9";
        private const string WolfUrl = "https://wolf3d.io";

        private const int headerTop = 110;
        private readonly Vector2 headerSize = new Vector2(350, 10);

        private static Texture2D banner = null;
        private static Texture2D love = null;

        private GUIStyle headerTextStyle = null;
        private GUIStyle footerTextStyle = null;
        private GUIStyle webButtonStyle = null;

        private GUILayoutOption windowWidth = GUILayout.Width(512);

        private void LoadAssets()
        {
            if (banner == null) banner = AssetDatabase.LoadAssetAtPath<Texture2D>(BannerPath);
            if (love == null) love = AssetDatabase.LoadAssetAtPath<Texture2D>(LovePath);

            if (headerTextStyle == null)
            {
                headerTextStyle = new GUIStyle();
                headerTextStyle.fontSize = 18;
                headerTextStyle.richText = true;
                headerTextStyle.fontStyle = FontStyle.Bold;
                headerTextStyle.normal.textColor = Color.white;
            }

            if (footerTextStyle == null)
            {
                footerTextStyle = new GUIStyle();
                footerTextStyle.richText = true;
                footerTextStyle.fontStyle = FontStyle.Bold;
                footerTextStyle.normal.textColor = GUI.skin.label.normal.textColor;
            }

            if (webButtonStyle == null)
            {
                webButtonStyle = new GUIStyle(GUI.skin.button);
                webButtonStyle.fontSize = 10;
                webButtonStyle.fixedWidth = 110;
            }
        }

        public void DrawContent(Action content, int windowHeight)
        {
            LoadAssets();

            Horizontal(() =>
            {
                GUILayout.FlexibleSpace();
                Vertical(() => {
                    DrawBanner();
                    content?.Invoke();
                    EditorGUILayout.Space();
                    DrawExternalLinks();
                    DrawFooter(windowHeight);
                }, windowWidth);
                GUILayout.FlexibleSpace();
            });
        }

        private void DrawBanner()
        {
            if (banner != null)
            {
                GUI.DrawTexture(new Rect((position.size.x - banner.width) / 2, 0, banner.width, banner.height), banner);
            }

            EditorGUI.DropShadowLabel(new Rect((position.size.x - headerSize.x) / 2, headerTop, headerSize.y, banner.height), $"Ready Player Me Unity SDK { Version }", headerTextStyle);

            GUILayout.Space(142);
        }

        private void DrawExternalLinks()
        {
            EditorGUILayout.BeginHorizontal("Box");
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Ready Player Me", webButtonStyle))
            {
                Application.OpenURL(RpmUrl);
            }
            if (GUILayout.Button("Blog", webButtonStyle))
            {
                Application.OpenURL(BlogUrl);
            }
            if (GUILayout.Button("Docs", webButtonStyle))
            {
                Application.OpenURL(DocsUrl);
            }
            if (GUILayout.Button("Discord", webButtonStyle))
            {
                Application.OpenURL(DiscordUrl);
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawFooter(int windowHeight)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Built with  --  by Wolf3D", footerTextStyle, GUILayout.Width(160)))
            {
                Application.OpenURL(WolfUrl);
            }

            if (love != null)
            {
#if UNITY_2019_1_OR_NEWER
                GUI.DrawTexture(new Rect((position.size.x - 44f) / 2f, windowHeight - 20, 16, 16), love);
#else
                GUI.DrawTexture(new Rect((position.size.x - 28f) / 2f, windowHeight - 15, 16, 16), love);
#endif
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        #region Horizontal and Vertical Layouts
        public void Vertical(Action content, bool isBox = false)
        {
            EditorGUILayout.BeginVertical(isBox ? "Box" : GUIStyle.none);
            content?.Invoke();
            EditorGUILayout.EndVertical();
        }

        public void Vertical(Action content, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginVertical(options);
            content?.Invoke();
            EditorGUILayout.EndVertical();
        }

        public void Horizontal(Action content, bool isBox = false)
        {
            EditorGUILayout.BeginHorizontal(isBox ? "Box" : GUIStyle.none);
            content?.Invoke();
            EditorGUILayout.EndHorizontal();
        }

        public void Horizontal(Action content, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal(options);
            content?.Invoke();
            EditorGUILayout.EndHorizontal();
        }
        #endregion
    }
}
