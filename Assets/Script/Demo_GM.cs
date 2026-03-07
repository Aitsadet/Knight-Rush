using UnityEngine;
using UnityEngine.UI;

public class Demo_GM : MonoBehaviour
{
    public static Demo_GM Gm;

    [Header("UI Settings")]
    [Tooltip("ต้องใส่ Image ทั้งหมด 8 ช่อง (0-7)")]
    public Image[] UIImage;

    // กำหนดสีไว้ตรงนี้จะได้แก้ที่เดียวจบ
    private Color32 normalColor = new Color32(255, 255, 255, 255); // สีปกติ (ขาว)
    private Color32 pressedColor = new Color32(180, 180, 180, 255); // สีตอนกด (เทา)

    void Awake()
    {
        Screen.fullScreen = false;
        Gm = this;
    }

    void Update()
    {
        KeyUPDownchange();
    }

    // ฟังก์ชันช่วยเปลี่ยนสีแบบปลอดภัย เช็กก่อนว่ามีของใน Array ไหม
    void SetKeyColor(int index, bool isPressed)
    {
        // เช็กว่า Index นี้มีอยู่ใน Array และมีการลาก Object มาใส่จริงๆ
        if (UIImage != null && index < UIImage.Length && UIImage[index] != null)
        {
            UIImage[index].color = isPressed ? pressedColor : normalColor;
        }
    }

    public void KeyUPDownchange()
    {
        // --- WSAD ---
        if (Input.GetKeyDown(KeyCode.W)) SetKeyColor(0, true);
        if (Input.GetKeyUp(KeyCode.W)) SetKeyColor(0, false);

        if (Input.GetKeyDown(KeyCode.S)) SetKeyColor(1, true);
        if (Input.GetKeyUp(KeyCode.S)) SetKeyColor(1, false);

        if (Input.GetKeyDown(KeyCode.A)) SetKeyColor(2, true);
        if (Input.GetKeyUp(KeyCode.A)) SetKeyColor(2, false);

        if (Input.GetKeyDown(KeyCode.D)) SetKeyColor(3, true);
        if (Input.GetKeyUp(KeyCode.D)) SetKeyColor(3, false);

        // --- Mouse ---
        if (Input.GetKeyDown(KeyCode.Mouse0)) SetKeyColor(4, true);
        if (Input.GetKeyUp(KeyCode.Mouse0)) SetKeyColor(4, false);

        if (Input.GetKeyDown(KeyCode.Mouse1)) SetKeyColor(5, true);
        if (Input.GetKeyUp(KeyCode.Mouse1)) SetKeyColor(5, false);

        // --- Other Keys ---
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetKeyColor(6, true);
        if (Input.GetKeyUp(KeyCode.Alpha1)) SetKeyColor(6, false);

        if (Input.GetKeyDown(KeyCode.Space)) SetKeyColor(7, true);
        if (Input.GetKeyUp(KeyCode.Space)) SetKeyColor(7, false);
    }

    // ฟังก์ชันรีเซ็ตสีทั้งหมด (ถ้าต้องการเรียกใช้)
    void InitColor()
    {
        if (UIImage == null) return;
        for (int i = 0; i < UIImage.Length; i++)
        {
            if (UIImage[i] != null) UIImage[i].color = normalColor;
        }
    }
}