using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Pengaturan Syarat Level")]
    public int totalMateriDiLevel = 2;
    public int totalSoalDiLevel = 3;

    private int materiSelesai = 0;
    private int soalSelesai = 0;

    [Header("UI Global")]
    public TextMeshProUGUI txt_skorGlobal;

    [Header("Pengaturan Perpindahan Scene")]
    public ScreenChanger screenChanger;
    public string namaSceneBerikutnya; // Scene Win
    public string namaSceneKalah;      // <-- TAMBAHAN: Ketik nama scene Lose di Inspector

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateUI();
    }

    public void TambahMateriSelesai()
    {
        materiSelesai++;
        UpdateUI();
    }

    public void TambahSoalSelesai()
    {
        soalSelesai++;
        UpdateUI();

        if (soalSelesai >= totalSoalDiLevel)
        {
            LevelSelesai(); // Memanggil WIN
        }
    }

    public bool ApakahMateriSudahCukup()
    {
        return materiSelesai >= totalMateriDiLevel;
    }

    void UpdateUI()
    {
        if (txt_skorGlobal != null)
        {
            int totalSkor = materiSelesai + soalSelesai;
            int totalTarget = totalMateriDiLevel + totalSoalDiLevel;
            txt_skorGlobal.text = "Progres: " + totalSkor + " / " + totalTarget;
        }
    }

    void LevelSelesai()
    {
        Debug.Log("Semua soal selesai! Pindah ke scene Win.");

        if (screenChanger != null && !string.IsNullOrEmpty(namaSceneBerikutnya))
        {
            screenChanger.PindahKeScene(namaSceneBerikutnya);
        }
        else
        {
            Debug.LogError("Gagal pindah scene Win!");
        }
    }

    // <-- TAMBAHAN: Fungsi untuk memanggil scene LOSE
    public void LevelKalah()
    {
        Debug.Log("Gagal memenuhi syarat minimum soal! Pindah ke scene Lose.");

        if (screenChanger != null && !string.IsNullOrEmpty(namaSceneKalah))
        {
            screenChanger.PindahKeScene(namaSceneKalah);
        }
        else
        {
            Debug.LogError("Gagal pindah scene Lose! Pastikan 'Nama Scene Kalah' sudah diisi di GameManager.");
        }
    }
}