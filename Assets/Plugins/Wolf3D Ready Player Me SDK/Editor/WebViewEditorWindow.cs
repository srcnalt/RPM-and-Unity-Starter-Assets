using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using System;

namespace Wolf3D.ReadyPlayerMe.AvatarSDK
{
    public class WebViewEditorWindow : EditorWindowBase
    {
        private const string IFrameSourceText = "src=\'https://readyplayer.me/avatar?webview=true\'";
        private const string WebViewFileName = "webview.html";
        private const string UrlRegexPattern = "src=\\'https:\\/\\/([a-z]+[.])?readyplayer.me\\/avatar\\?webview=true\\'";
        private const string WebViewPartnerSaveKey = "WebViewPartnerSubdomainName";
        private const string BecomeAPartnerUrl = "http://bit.ly/RPMBecomePartner";

        private const string HelpText = "If you are a Ready Player Me partner who owns a subdomain, you can enter it here to change your WebView start url. If you want to use the default website please leave this blank.";
        private const string BecomePartnerText = "Would you like to become a Ready Player Me partner to have your a own subdomain with your logo and custom theme? Click on the button and fill in the form to get in touch with us!";

        private string WebViewFilePath => $"{ Application.streamingAssetsPath }/{ WebViewFileName }";
        private string PartnerSubdomainUrl => $"src=\'https://{partnerSubdomain}.readyplayer.me/avatar?webview=true\'";
        private string SavedPartnerUrl => string.IsNullOrEmpty(partnerSubdomain) ? IFrameSourceText : PartnerSubdomainUrl;

        private string partnerSubdomain = "";
        private bool initialized = false;

        private string text = "";
        private string currentSource = "";

        private bool saveButtonDirty = true;
        private string SaveButtonText => saveButtonDirty ? "Save" : "Subdomain Saved!";

        private static Vector2Int windowSize = new Vector2Int(512, 395);

        private GUIStyle textFieldStyle = null;
        private GUIStyle textLabelStyle = null;

        [MenuItem("Ready Player Me/WebView Partner Editor")]
        private static void ShowWindowMenu()
        {
            WebViewEditorWindow window = GetWindow(typeof(WebViewEditorWindow)) as WebViewEditorWindow;
            window.titleContent = new GUIContent("WebView Partner Editor");
            window.minSize = window.maxSize = windowSize;
            window.ShowUtility();
        }

        private void Initialize()
        {
            if (File.Exists(WebViewFilePath))
            {
                text = File.ReadAllText(WebViewFilePath);
                Regex regex = new Regex(UrlRegexPattern);
                Match match = regex.Match(text);

                if (match.Success)
                {
                    string subdomain = EditorPrefs.GetString(WebViewPartnerSaveKey);

                    if (string.IsNullOrEmpty(subdomain) || match.Value == $"src=\'https://{subdomain}.readyplayer.me/avatar?webview=true\'")
                    {
                        partnerSubdomain = subdomain;
                        currentSource = match.Value;
                    }
                    else
                    {
                        text.Replace(match.Value, IFrameSourceText);
                        currentSource = IFrameSourceText;

                        File.WriteAllText(WebViewFilePath, text);
                        EditorPrefs.SetString(WebViewPartnerSaveKey, "");

                        Debug.LogWarning("WebView Subdomain Mismatch!");
                    }
                }
                else
                {
                    Debug.LogError("WebView File is Corrupted!");
                }
            }
            else
            {
                Debug.LogError("WebView File Missing!");
            }

            initialized = true;
        }

        private void OnGUI()
        {
            if (!initialized) Initialize();

            LoadStyles();

            DrawContent(()=>
            {
                DrawContent();
            }, windowSize.y);
        }

        private void LoadStyles()
        {
            if(textFieldStyle == null)
            {
                textFieldStyle = new GUIStyle(GUI.skin.textField);
                textFieldStyle.alignment = TextAnchor.MiddleCenter;
                textFieldStyle.fontSize = 16;
            }

            if (textLabelStyle == null)
            {
                textLabelStyle = new GUIStyle(GUI.skin.label);
                textLabelStyle.alignment = TextAnchor.MiddleLeft;
                textLabelStyle.fontStyle = FontStyle.Bold;
                textLabelStyle.fontSize = 16;
            }
        }

        private void DrawContent()
        {
            Vertical(() => {
                EditorGUILayout.HelpBox(HelpText, MessageType.Info);

                EditorGUILayout.Space();

                Horizontal(() => { 
                    EditorGUILayout.LabelField("https://", textLabelStyle, GUILayout.Width(80), GUILayout.Height(30));
                    string oldValue = partnerSubdomain;
                    partnerSubdomain = EditorGUILayout.TextField(oldValue, textFieldStyle, GUILayout.Width(252), GUILayout.Height(30));
                    EditorGUILayout.LabelField(".readyplayer.me", textLabelStyle, GUILayout.Width(150), GUILayout.Height(30));

                    if(oldValue != partnerSubdomain)
                    {
                        saveButtonDirty = true;
                    }
                });

                EditorGUILayout.Space();

                if (GUILayout.Button(SaveButtonText, GUILayout.Height(30)) && ValidateSubdomain())
                {
                    saveButtonDirty = false;
                    EditorPrefs.SetString(WebViewPartnerSaveKey, partnerSubdomain);
                    text = text.Replace(currentSource, SavedPartnerUrl);
                    currentSource = SavedPartnerUrl;
                    File.WriteAllText(WebViewFilePath, text);
                }
            }, true);

            Vertical(() => {
                EditorGUILayout.HelpBox(BecomePartnerText, MessageType.Info);

                if (GUILayout.Button("Become a partner"))
                {
                    Application.OpenURL(BecomeAPartnerUrl);
                }
            }, true);
        }

        private bool ValidateSubdomain()
        {
            if (!partnerSubdomain.All(c => char.IsLetterOrDigit(c)))
            {
                EditorUtility.DisplayDialog("Subdomain Format Error", $"Partner subdomain cannot contain white space and special characters. Only alpha-numeric characters are allowed. Value you enteres is '{ partnerSubdomain }'.", "ok");
                return false;
            }

            return true;
        }
    }
}
