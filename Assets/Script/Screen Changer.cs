using UnityEngine;
// --- PENJELASAN: PUSTAKA SCENE ---
// Wajib ditambahkan karena kita menggunakan perintah SceneManager di bawah.
using UnityEngine.SceneManagement;

public class ScreenChanger : MonoBehaviour
{
    // --- PENJELASAN: PUBLIC VOID ---
    // Kata kunci 'public' di depan 'void' ini SANGAT PENTING.
    // Jika kita tidak menuliskan 'public', tombol (Button) UI yang ada di Unity 
    // tidak akan bisa mendeteksi atau mengklik fungsi ini. 
    // 'public' mengizinkan fungsi ini dilihat oleh objek lain di luar script.
    public void PindahKeScene(string namaSceneTujuan)
    {
        // Perintah untuk menghancurkan memori level saat ini dan memuat level baru sesuai namanya.
        // Ingat: Nama scene yang dituju harus sudah didaftarkan di File -> Build Settings.
        SceneManager.LoadScene(namaSceneTujuan);
    }

    // Fungsi BARU untuk keluar dari game (Bisa dihubungkan ke tombol "Exit" di Main Menu)
    public void KeluarGame()
    {
        // Menampilkan pesan di tab Console programmer agar kita tahu tombolnya terhubung dan berfungsi.
        Debug.Log("Game ditutup!");

        // --- PENJELASAN PENTING: APPLICATION.QUIT ---
        // Ini adalah perintah mutlak untuk menutup aplikasi game sepenuhnya ke layar HP/Desktop.
        // TAPI INGAT: Perintah Application.Quit() TIDAK AKAN TERLIHAT BEKERJA saat kamu 
        // mengujinya menggunakan tombol 'Play' di dalam Unity Editor. 
        // Jangan panik jika kamu klik tombol Exit di editor tapi gamenya tidak mati. 
        // Perintah ini BARU AKAN BERFUNGSI NYATA setelah game kamu di-Build 
        // (dijadikan file .exe untuk PC atau .apk untuk Android).
        Application.Quit();
    }
}