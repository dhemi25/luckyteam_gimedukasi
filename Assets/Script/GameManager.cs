using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Pengaturan Syarat Level")]
    public int totalMateriDiLevel = 2; // Berapa materi yang harus dibaca sebelum soal terbuka?
    public int totalSoalDiLevel = 3;   // Berapa soal yang harus dijawab?

    private int materiSelesai = 0;
    private int soalSelesai = 0;

    [Header("UI Global")]
    public TextMeshProUGUI txt_skorGlobal;

    [Header("Pengaturan Perpindahan Scene")]
    public ScreenChanger screenChanger;     // Taruh GameObject yang punya skrip ScreenChanger di sini
    public string namaSceneBerikutnya;     // Ketik nama scene tujuan di Inspector

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateUI();
    }

    // Dipanggil oleh UiMateri saat materi BARU selesai dibaca
    public void TambahMateriSelesai()
    {
        materiSelesai++;
        UpdateUI();
    }

    // Dipanggil oleh UiSoal saat soal dijawab benar semua
    public void TambahSoalSelesai()
    {
        soalSelesai++;
        UpdateUI();

        if (soalSelesai >= totalSoalDiLevel)
        {
            LevelSelesai();
        }
    }

    // Fungsi untuk mengecek apakah pemain sudah boleh membuka Soal
    public bool ApakahMateriSudahCukup()
    {
        return materiSelesai >= totalMateriDiLevel;
    }

    void UpdateUI()
    {
        if (txt_skorGlobal != null)
        {
            // Menampilkan gabungan skor Materi dan Soal
            int totalSkor = materiSelesai + soalSelesai;
            int totalTarget = totalMateriDiLevel + totalSoalDiLevel;
            txt_skorGlobal.text = "Progres: " + totalSkor + " / " + totalTarget;
        }
    }

    void LevelSelesai()
    {
        Debug.Log("Semua soal selesai! Memanggil ScreenChanger untuk pindah halaman.");

        // Memastikan ScreenChanger dan Nama Scene sudah diatur di Inspector
        if (screenChanger != null && !string.IsNullOrEmpty(namaSceneBerikutnya))
        {
            screenChanger.PindahKeScene(namaSceneBerikutnya);
        }
        else
        {
            Debug.LogError("Gagal pindah scene! Pastikan 'Screen Changer' dan 'Nama Scene Berikutnya' sudah diisi di Inspector GameManager.");
        }
    }
}