using UnityEngine;
// --- PENJELASAN: NAMESPACE (PUSTAKA TAMBAHAN) ---
// Secara default, Unity tidak memuat semua fitur ke dalam memori agar game tidak berat.
// Karena kita ingin mengurus perpindahan layar (Scene) dan Video, 
// kita harus "meminjam" buku panduan khusus SceneManagement dan Video dari Unity.
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class splashvideocontroler : MonoBehaviour
{
    [Header("Pengaturan Video")]
    public VideoPlayer videoPlayer; // Kotak untuk menaruh komponen pemutar video

    [Header("Pengaturan Waktu (Detik)")]
    [Tooltip("Waktu tunggu sebelum video mulai diputar")]
    public float jedaAwal = 0.5f;

    [Tooltip("Waktu tunggu setelah video selesai diputar sebelum pindah scene")]
    public float jedaAkhir = 2.0f;

    [Header("Tujuan Scene")]
    [Tooltip("Nama scene setelah splash video selesai (misal: MainMenu)")]
    public string namaSceneTujuan;

    // --- PENJELASAN: FLAG (PENANDA STATUS) ---
    // Variabel bool (true/false) sering digunakan sebagai "Gembok" atau "Penanda"
    // agar sebuah perintah tidak dieksekusi dua kali secara tidak sengaja.
    private bool isVideoStarted = false; // Penanda apakah video sudah mulai jalan?
    private bool isMovingToNextScene = false; // Penanda apakah game sedang proses pindah scene?

    void Start()
    {
        // Kalau kotak videoPlayer di Inspector masih kosong (belum di-drag oleh kita),
        // suruh script ini mencari sendiri komponen VideoPlayer yang menempel di objek yang sama.
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // Matikan Play On Awake di VideoPlayer agar kita bisa mengatur jeda awalnya lewat kode
        // (Biar videonya nggak langsung meledak nyala pas game baru dibuka).
        videoPlayer.playOnAwake = false;

        // --- PENJELASAN: SISTEM EVENT / LISTENER TANDA TAMBAH SAMA DENGAN (+=) ---
        // 'loopPointReached' adalah alarm bawaan Unity yang akan berbunyi saat video tamat.
        // Tanda '+=' berarti kita mendaftarkan diri ke alarm tersebut:
        // "Hei Unity, kalau videonya nanti sudah tamat, tolong panggil fungsi OnVideoFinished milikku ya!"
        videoPlayer.loopPointReached += OnVideoFinished;

        // Mulai menghitung jeda awal menggunakan Invoke (Sistem Timer)
        // Tunggu selama 'jedaAwal' (0.5 detik), lalu jalankan fungsi "MulaiPutarVideo"
        Invoke("MulaiPutarVideo", jedaAwal);
    }

    void MulaiPutarVideo()
    {
        // Pastikan pemutar videonya benar-benar ada
        if (videoPlayer != null)
        {
            videoPlayer.Play(); // Mainkan videonya!
            isVideoStarted = true; // Nyalakan penanda bahwa video sudah mulai
        }
    }

    // --- PENJELASAN: FUNGSI CALLBACK (DIPANGGIL OTOMATIS) ---
    // Fungsi ini tidak dipanggil oleh kita secara manual, melainkan dipanggil otomatis 
    // oleh Unity karena kita sudah mendaftarkannya di Start() tadi.
    // (VideoPlayer vp) di dalam kurung wajib ditulis sebagai syarat dari Unity, 
    // meskipun variabel 'vp' nya tidak kita pakai di dalam fungsinya.
    void OnVideoFinished(VideoPlayer vp)
    {
        // Jalankan perpindahan scene dengan jeda akhir yang ditentukan.
        // Pengecekan (!isMovingToNextScene) ini penting sebagai gembok! 
        // Biar kalau videonya nge-bug dan mengirim sinyal tamat 2 kali, kita nggak akan mindahin scene 2 kali.
        if (!isMovingToNextScene)
        {
            isMovingToNextScene = true; // Kunci gemboknya

            // Tunggu selama 'jedaAkhir' (2 detik), lalu jalankan fungsi "PindahKeSceneSelanjutnya"
            Invoke("PindahKeSceneSelanjutnya", jedaAkhir);
        }
    }

    void PindahKeSceneSelanjutnya()
    {
        // Cek dulu, apakah nama scene tujuan sudah kita ketik di Unity Inspector?
        if (!string.IsNullOrEmpty(namaSceneTujuan))
        {
            // --- PENJELASAN: LOAD SCENE ---
            // Ini adalah mantra utama untuk memuat layar baru.
            // CATATAN PENTING: Scene yang mau dituju WAJIB sudah didaftarkan di 
            // menu File -> Build Settings -> Scenes in Build. Kalau belum, akan error!
            SceneManager.LoadScene(namaSceneTujuan);
        }
        else
        {
            // Munculkan tulisan merah di Console programmer kalau kita lupa ngetik nama scenenya
            Debug.LogError("Nama Scene Tujuan belum diisi di Inspector!");
        }
    }
}