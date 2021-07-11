using System;
using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Webview
{
    private const string WebViewFileName = "webview.html";

    private MonoBehaviour parent;
    private WebviewWindowBase webViewObject;
    private int left, top, right, bottom;

    // Event to call when webview starts, receives message.
    public Action<string> OnWebviewStarted;

    // Event to call when avatar is created, receives GLB url.
    public Action<string> OnAvatarCreated;

    /// <summary>
    ///     Create webview object attached to a MonoBehaviour object
    /// </summary>
    /// <param name="parent">Parent game object.</param>
    public void CreateWebview(MonoBehaviour parent)
    {
        this.parent = parent;

        if (SetWebviewWindow())
        {
            parent.StartCoroutine(LoadWebviewURL());
            SetScreenPadding(left, top, right, bottom);
        }
    }

    /// <summary>
    ///     Set webview screen padding in pixels.
    /// </summary>
    public void SetScreenPadding(int left, int top, int right, int bottom)
    {
        this.left = left;
        this.top = top;
        this.right = right;
        this.bottom = bottom;

        if (webViewObject)
        {
            webViewObject.SetMargins(left, top, right, bottom);
        }
    }

    public void SetVisible(bool visible)
    {
        webViewObject.IsVisible = visible;
    }

    private bool SetWebviewWindow()
    {
        bool hasNetwork = Application.internetReachability != NetworkReachability.NotReachable;
        bool supported = true;

        WebviewOptions options = new WebviewOptions();

        if (hasNetwork)
        {
        #if !UNITY_EDITOR && UNITY_ANDROID
            webViewObject = parent.gameObject.AddComponent<AndroidWebViewWindow>();
        #elif !UNITY_EDITOR && UNITY_IOS
            webViewObject = parent.gameObject.AddComponent<IOSWebViewWindow>();
        #else
            webViewObject = parent.gameObject.AddComponent<NotSupportedWebviewWindow>();
            supported = false;
        #endif
        }
        else
        {
            webViewObject = parent.gameObject.AddComponent<NotSupportedWebviewWindow>();
            supported = false;
        }

        webViewObject.OnLoaded = OnLoaded;
        webViewObject.OnJS = OnWebMessageReceived;

        webViewObject.Init(options);

        return supported;
    }

    // TODO: Dont write again if exists
    private IEnumerator LoadWebviewURL()
    {
        string source = Path.Combine(Application.streamingAssetsPath, WebViewFileName);
        string destination = Path.Combine(Application.persistentDataPath, WebViewFileName);
        byte[] result = null;

#if UNITY_ANDROID
        using(UnityWebRequest request = UnityWebRequest.Get(source))
        {
            yield return request.SendWebRequest();
            result = request.downloadHandler.data;
        }
#elif UNITY_IOS
        result = File.ReadAllBytes(source);
#endif

        File.WriteAllBytes(destination, result);
        yield return null;

        webViewObject.LoadURL($"file://{ destination}");
    }

    private void OnWebMessageReceived(string message)
    {
        Debug.Log(message);

        if (message.Contains(".glb"))
        {
            webViewObject.IsVisible = false;
            OnAvatarCreated?.Invoke(message);
        }
    }

    private void OnLoaded(string message)
    {
        webViewObject.EvaluateJS(@"
            if (window && window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.unityControl) {
                window.Unity = {
                    call: function(msg) { 
                        window.webkit.messageHandlers.unityControl.postMessage(msg); 
                    }
                }
            } 
            else {
                window.Unity = {
                    call: function(msg) {
                        window.location = 'unity:' + msg;
                    }
                }
            }

            document.getElementById('loading').innerHTML = '<center>Failed to load on current browser.<br/>Please update your Operating System.</center>'
        ");
    }
}
