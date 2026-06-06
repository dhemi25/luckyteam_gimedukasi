using UnityEngine;
using TMPro; // Untuk teks UI

public class GameManager : MonoBehaviour
{
    // Singleton agar mudah diakses dari script mana saja tanpa perlu drag-and-drop
    public static GameManager instance;

    [Header("Pengaturan Level")]
    public int totalObjekDiLevel = 3; // Isi di inspector, berapa total objek yang harus dijawab
    private int objekSelesai = 0; // Menghitung berapa objek yang sudah dijawab benar semua

    [Header("UI Global")]
    public TextMeshProUGUI txt_skorGlobal; // Teks untuk "Task Selesai: 0 / 3"
    public GameObject canvasLevelSelesai; // Canvas yang berisi tombol Next Level / Main Menu

    private void Awake()
    {
        // Setup Singleton
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Pastikan canvas kemenangan mati saat game dimulai
        if (canvasLevelSelesai != null) canvasLevelSelesai.SetActive(false);
        UpdateUI();
    }

    // Fungsi ini akan dipanggil oleh UiSoal ketika pemain menjawab benar semua di satu objek
    public void TambahObjekSelesai()
    {
        objekSelesai++;
        UpdateUI();

        // Cek apakah semua objek sudah diselesaikan
        if (objekSelesai >= totalObjekDiLevel)
        {
            LevelSelesai();
        }
    }

    void UpdateUI()
    {
        if (txt_skorGlobal != null)
        {
            txt_skorGlobal.text = "Task Selesai: " + objekSelesai + " / " + totalObjekDiLevel;
        }
    }

    void LevelSelesai()
    {
        Debug.Log("Semua task selesai! Membuka gerbang / tombol Next Level.");
        // Munculkan Canvas yang berisi tombol Next Level atau Exit
        if (canvasLevelSelesai != null)
        {
            canvasLevelSelesai.SetActive(true);
        }
    }
}