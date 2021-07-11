using UnityEngine;
using UnityEngine.UI;

public class MessageCanvas : MonoBehaviour
{
    [SerializeField] private RectTransform panel = null;
    [SerializeField] private Text messageLabel = null;

    public void SetMessage(string message)
    {
        messageLabel.text = message;
    }

    public void SetMargins(int left, int top, int right, int bottom)
    {
        panel.offsetMax = new Vector2(-right, -top);
        panel.offsetMin = new Vector2(left, bottom);
    }
}
