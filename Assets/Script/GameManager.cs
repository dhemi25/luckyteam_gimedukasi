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
    public GameObject canvasLevelSelesai;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (canvasLevelSelesai != null) canvasLevelSelesai.SetActive(false);
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
        Debug.Log("Semua soal selesai! Membuka gerbang / tombol Next Level.");
        if (canvasLevelSelesai != null)
        {
            canvasLevelSelesai.SetActive(true);
        }
    }
}