using UnityEngine;

public class WindowController : MonoBehaviour
{
    public ShowWindowImage imageWindow;
    public WindowDialogueManager dialogueWindow;

    public void OnClick()
    {
        imageWindow.OpenImage();
        dialogueWindow.OpenWindow();
    }
}