using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenChanger : MonoBehaviour
{
    // Fungsi untuk pindah Scene (yang sudah kamu buat)
    public void PindahKeScene(string namaSceneTujuan)
    {
        SceneManager.LoadScene(namaSceneTujuan);
    }

    // Fungsi BARU untuk keluar dari game
    public void KeluarGame()
    {
        // Menampilkan pesan di tab Console agar kita tahu tombolnya berfungsi
        Debug.Log("Game ditutup!");

        // Perintah utama untuk menutup aplikasi
        Application.Quit();
    }
}