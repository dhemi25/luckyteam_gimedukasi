using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class splashvideocontroler : MonoBehaviour
{
    [Header("Pengaturan Video")]
    public VideoPlayer videoPlayer; // Taruh komponen VideoPlayer di sini

    [Header("Pengaturan Waktu (Detik)")]
    [Tooltip("Waktu tunggu sebelum video mulai diputar")]
    public float jedaAwal = 0.5f;

    [Tooltip("Waktu tunggu setelah video selesai diputar sebelum pindah scene")]
    public float jedaAkhir = 2.0f;

    [Header("Tujuan Scene")]
    [Tooltip("Nama scene setelah splash video selesai (misal: MainMenu)")]
    public string namaSceneTujuan;

    private bool isVideoStarted = false;
    private bool isMovingToNextScene = false;

    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // Matikan Play On Awake di VideoPlayer agar kita bisa mengatur jeda awalnya lewat kode
        videoPlayer.playOnAwake = false;

        // Daftarkan fungsi saat video selesai diputar
        videoPlayer.loopPointReached += OnVideoFinished;

        // Mulai menghitung jeda awal
        Invoke("MulaiPutarVideo", jedaAwal);
    }

    void MulaiPutarVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Play();
            isVideoStarted = true;
        }
    }

    // Fungsi otomatis yang dipanggil saat video habis/selesai
    void OnVideoFinished(VideoPlayer vp)
    {
        // Jalankan perpindahan scene dengan jeda akhir yang ditentukan
        if (!isMovingToNextScene)
        {
            isMovingToNextScene = true;
            Invoke("PindahKeSceneSelanjutnya", jedaAkhir);
        }
    }

    void PindahKeSceneSelanjutnya()
    {
        if (!string.IsNullOrEmpty(namaSceneTujuan))
        {
            SceneManager.LoadScene(namaSceneTujuan);
        }
        else
        {
            Debug.LogError("Nama Scene Tujuan belum diisi di Inspector!");
        }
    }
}
