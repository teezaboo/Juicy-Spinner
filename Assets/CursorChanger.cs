using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    public Texture2D cursorTexture; // ให้กำหนด Texture2D ใน Inspector
    public Vector2 hotSpot = Vector2.zero; // จุดที่คลิกใน cursor

    void Start()
    {
        // เปลี่ยนรูป cursor ในเริ่มต้นเกม
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }

    void OnDisable()
    {
        // เปลี่ยนกลับเป็น cursor เริ่มต้นเมื่อปิดสคริปต์
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
