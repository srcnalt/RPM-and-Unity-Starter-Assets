using UnityEngine;

public class NotSupportedWebviewWindow : WebviewWindowBase
{
    public static bool IsWebViewAvailable => false;

    public override void Init(WebviewOptions options = default)
    {
        bool hasNetwork = Application.internetReachability != NetworkReachability.NotReachable;
        string message = hasNetwork ? "Current platform\ndoes not support Webview." : "Network is not reachable.";

        MessageCanvas canvasPrefab = Resources.Load<MessageCanvas>("NotSupportedCanvas");
        messageCanvas = Instantiate(canvasPrefab);
        messageCanvas.SetMessage(message);
    }

    public override void SetMargins(int left, int top, int right, int bottom)
    {
        messageCanvas.SetMargins(left, top, right, bottom);
    }

    public override bool IsVisible
    {
        get
        {
            return isVisible;
        }
        set
        {
            isVisible = value;
        }
    }

    public override bool IsKeyboardVisible
    {
        get
        {
            return iskeyboardVisible;
        }
        set
        {
            iskeyboardVisible = value;
            SetMargins(marginLeft, marginTop, marginRight, marginBottom);
        }
    }

    public override bool AlertDialogEnabled
    {
        get
        {
            return alertDialogEnabled;
        }
        set
        {
            alertDialogEnabled = value;
        }
    }

    public override bool ScrollBounceEnabled
    {
        get
        {
            return scrollBounceEnabled;
        }
        set
        {
            scrollBounceEnabled = value;
        }
    }

    public override void LoadURL(string url) { }

    public override void LoadHTML(string html, string baseUrl) { }

    public override void EvaluateJS(string js) { }

    public override int Progress => 0;

    #region Navigation Methods
    public override bool CanGoBack() => false;

    public override bool CanGoForward() => false;

    public override void GoBack() { }

    public override void GoForward() { }

    public override void Reload() { }
    #endregion

    #region Session Related Methods
    public override void AddCustomHeader(string headerKey, string headerValue) { }

    public override string GetCustomHeaderValue(string headerKey) => string.Empty;

    public override void RemoveCustomHeader(string headerKey) { }

    public override void ClearCustomHeader() { }

    public override void ClearCookies() { }

    public override void SaveCookies() { }

    public override string GetCookies(string url) => string.Empty;

    public override void SetBasicAuthInfo(string userName, string password) { }

    public override void ClearCache(bool includeDiskFiles) { }
    #endregion

    private void OnDestroy()
    {
        Destroy(messageCanvas);
    }
}
