using UnityEngine;

// Class untuk mengontrol pergerakan player (mode adventure)
public class PlayerContrrollerAdventure : MonoBehaviour
{
    // Kecepatan gerak player
    public float speed = 5f;

    // Referensi ke kamera (agar arah gerak mengikuti arah kamera)
    public Transform cameraTransform;

    // Komponen CharacterController untuk menggerakkan player tanpa Rigidbody
    CharacterController controller;

    // Menyimpan apakah player sedang berada di area interaksi
    private bool onInteractArea;

    // Objek UI atau indikator interaksi (misalnya ikon atau teks)
    public GameObject objInfoInteract;

    //mengambil canvas
    public GameObject canvasInteract;


    // Fungsi Start dijalankan sekali saat game mulai
    void Start()
    {
        // Mengambil komponen CharacterController dari GameObject ini
        controller = GetComponent<CharacterController>();

        // Mengambil child ke-2 sebagai objek interaksi
        // PERHATIAN: index dimulai dari 0, jadi ini adalah child ke-3
        // Jika urutan berubah di Hierarchy, bisa menyebabkan error/salah object
        objInfoInteract = transform.GetChild(2).gameObject;

        // Pastikan canvas mati di awal
        canvasInteract.SetActive(false);
    }

    // Fungsi Update dijalankan setiap frame
    void Update()
    {
        // Mengambil input horizontal (A/D atau panah kiri/kanan)
        float h = Input.GetAxis("Horizontal");

        // Mengambil input vertical (W/S atau panah atas/bawah)
        float v = Input.GetAxis("Vertical");

        // Mengambil arah depan dari kamera
        Vector3 forward = cameraTransform.forward;

        // Mengambil arah kanan dari kamera
        Vector3 right = cameraTransform.right;

        // Menghilangkan pengaruh sumbu Y agar player tidak naik/turun
        forward.y = 0;
        right.y = 0;

        // Menghitung arah gerak berdasarkan input dan orientasi kamera
        Vector3 moveDirection = forward * v + right * h;

        // Menggerakkan player menggunakan CharacterController
        // normalized digunakan agar kecepatan tetap konsisten (tidak lebih cepat diagonal)
        controller.Move(moveDirection.normalized * speed * Time.deltaTime);

        // Jika ada input gerakan (player bergerak)
        if (moveDirection != Vector3.zero)
        {
            // Mengubah arah hadap player mengikuti arah gerakan
            transform.forward = moveDirection;

        }

        // Memutar objek interaksi terus-menerus di sumbu Y
        // Biasanya digunakan untuk efek visual (misalnya ikon berputar)
        objInfoInteract.transform.Rotate(0f, 120f * Time.deltaTime, 0f);

        if (onInteractArea && Input.GetKeyDown(KeyCode.E))
        {
            canvasInteract.SetActive(true);
            Debug.Log("E ditekan di area interaksi!");
        }
    }

    // Fungsi untuk mengatur apakah player berada di area interaksi
    public void SetOnInteractionArea(bool value)
    {
        // Menyimpan status apakah player berada di area interaksi
        onInteractArea = value;

        // Menampilkan atau menyembunyikan objek interaksi sesuai kondisi
        objInfoInteract.SetActive(value);

    }

}