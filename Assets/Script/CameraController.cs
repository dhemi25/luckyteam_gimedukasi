using UnityEngine;

// Class untuk mengontrol kamera (third person camera)
public class CameraController : MonoBehaviour
{
    // Target yang akan diikuti kamera (biasanya player)
    public Transform target;

    // Sensitivitas mouse untuk rotasi kamera
    public float mouseSensitivity = 150f;

    // Offset posisi kamera terhadap target (posisi relatif kamera)
    public Vector3 offset = new Vector3(0, 2, -5);

    // Rotasi horizontal (kiri/kanan)
    float yaw;

    // Rotasi vertical (atas/bawah), default sedikit melihat ke bawah
    float pitch = 20f;

    public PlayerContrrollerAdventure player;

    // LateUpdate dipanggil setelah semua Update selesai
    // Cocok untuk kamera agar mengikuti object dengan lebih smooth
    void LateUpdate()
    {

        if (!player.can_move) return;
        // Mengubah yaw (rotasi horizontal) berdasarkan pergerakan mouse X
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        // Mengubah pitch (rotasi vertical) berdasarkan pergerakan mouse Y
        // Dikurangi (-) agar arah gerak mouse terasa natural
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Membatasi rotasi vertical agar kamera tidak terlalu atas/bawah
        pitch = Mathf.Clamp(pitch, -30f, 60f);

        // Membuat rotasi berdasarkan pitch (X) dan yaw (Y)
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Mengatur posisi kamera:
        // posisi target + offset yang sudah diputar (rotation)
        // hasilnya kamera akan mengorbit mengelilingi target
        transform.position = target.position + rotation * offset;

        // Membuat kamera selalu melihat ke arah target
        // Vector3.up * 1.5f agar fokus ke bagian atas (misalnya kepala karakter)
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}