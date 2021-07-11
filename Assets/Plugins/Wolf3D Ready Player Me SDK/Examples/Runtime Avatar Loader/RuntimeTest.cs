using UnityEngine;
using Wolf3D.ReadyPlayerMe.AvatarSDK;

public class RuntimeTest : MonoBehaviour
{
    [SerializeField] private string AvatarURL = "https://d1a370nemizbjq.cloudfront.net/294eef48-c00e-4016-9e08-ec3f741129ae.glb";

    private void Start()
    {
        Debug.Log($"Started loading avatar. [{Time.timeSinceLevelLoad:F2}]");
        AvatarLoader avatarLoader = new AvatarLoader();
        avatarLoader.LoadAvatar(AvatarURL, OnAvatarLoaded);
    }

    private void OnAvatarLoaded(GameObject avatar, AvatarMetaData metaData)
    {
        Debug.Log($"Avatar loeded. [{Time.timeSinceLevelLoad:F2}]\n\n{metaData}");
    }
}
