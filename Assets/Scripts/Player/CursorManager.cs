using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void LockCursor(bool revealCursor=false)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = revealCursor;
    }

    public void UnlockCursor(bool revealCursor=true)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = revealCursor;
    }

    public void SetCursorVisibility(bool cursorVis)
    {
        Cursor.visible = cursorVis;
    }
}
