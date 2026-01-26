using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    public Texture2D defaultCursor;
    public Texture2D hoverCursor;

    private int hoverCount = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    public void OnHoverEnter()
    {
        hoverCount++;
        Cursor.SetCursor(hoverCursor, Vector2.zero, CursorMode.Auto);
    }

    public void OnHoverExit()
    {
        hoverCount = Mathf.Max(hoverCount - 1, 0);

        if (hoverCount == 0)
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }
}