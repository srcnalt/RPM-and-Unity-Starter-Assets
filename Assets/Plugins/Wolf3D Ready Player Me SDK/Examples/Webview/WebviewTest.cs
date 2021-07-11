using UnityEngine;

namespace Wolf3D.ReadyPlayerMe.AvatarSDK
{
    public class WebviewTest : MonoBehaviour
    {
        private Webview webview;
        private GameObject avatar;

        [SerializeField] private GameObject loadingLabel = null;

        // Create and initialize WebView
        private void Start()
        {
            webview = new Webview();
            webview.OnAvatarCreated = OnAvatarCreated;
            webview.SetScreenPadding(0, 0, 0, 0);
            webview.CreateWebview(this);
        }

        // Set WebView visible.
        public void DisplayWebView()
        {
            webview.SetVisible(true);
        }

        // WebView callback for retrieving avatar url
        private void OnAvatarCreated(string url)
        {
            if (avatar) Destroy(avatar);

            loadingLabel.SetActive(true);
            webview.SetVisible(false);

            AvatarLoader avatarLoader = new AvatarLoader();
            avatarLoader.LoadAvatar(url, OnAvatarLoaded);
        }

        // AvatarLoader callback for retrieving loaded avatar game object
        private void OnAvatarLoaded(GameObject avatar, AvatarMetaData metaData)
        {
            this.avatar = avatar;
            loadingLabel.SetActive(false);

            Debug.Log("Loaded");
        }
    }
}
