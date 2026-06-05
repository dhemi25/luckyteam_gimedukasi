using UnityEngine;

// Class untuk mendeteksi area interaksi menggunakan trigger collider
public class Interact : MonoBehaviour
{
    // Referensi ke script PlayerContrrollerAdventure
    // Digunakan untuk mengubah status interaksi pada player
    public PlayerContrrollerAdventure player;

    // Awake dipanggil saat object pertama kali diinisialisasi (sebelum Start)
    private void Awake()
    {
        // Saat game mulai, pastikan player TIDAK berada di area interaksi
        // Jadi UI interaksi dimatikan terlebih dahulu
        player.SetOnInteractionArea(false);

    }


    // Dipanggil saat collider lain masuk ke trigger area object ini
    private void OnTriggerEnter(Collider other)
    {
        // Saat ada object masuk ke area trigger,
        // player dianggap berada di area interaksi
        // Biasanya digunakan untuk menampilkan UI (misalnya "Press E")
        player.SetOnInteractionArea(true);
    }

    // Dipanggil saat collider keluar dari trigger area
    private void OnTriggerExit(Collider other)
    {
        // Saat object keluar dari area trigger,
        // player dianggap tidak lagi bisa berinteraksi
        // UI interaksi akan disembunyikan
        player.SetOnInteractionArea(false);

    }
}