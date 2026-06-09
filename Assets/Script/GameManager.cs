using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // --- PENJELASAN: SINGLETON PATTERN (POLA SINGLETON) ---
    // 'public static' membuat variabel ini menjadi "Milik Bersama" alias global.
    // Ini membuat script GameManager bertindak seperti "Bos Utama" di dalam game.
    // Script lain (seperti Player atau UiSoal) bisa langsung memanggil Bos ini 
    // dengan cara mengetik: GameManager.instance.NamaFungsinya() 
    // tanpa harus repot-repot menarik (drag & drop) objek GameManager di Inspector.
    public static GameManager instance;

    [Header("Pengaturan Syarat Level")]
    public int totalMateriDiLevel = 2; // Target jumlah materi yang harus dibaca
    public int totalSoalDiLevel = 3;   // Target jumlah soal yang harus diselesaikan

    // Variabel private untuk mencatat progres pemain saat ini
    private int materiSelesai = 0;
    private int soalSelesai = 0;

    [Header("UI Global")]
    public TextMeshProUGUI txt_skorGlobal; // Teks di pojok layar untuk menampilkan progres (misal: "Progres: 1 / 5")

    [Header("Pengaturan Perpindahan Scene")]
    public ScreenChanger screenChanger; // Script bantuan buatanmu untuk mengurus efek pindah layar/scene
    public string namaSceneBerikutnya; // Nama scene jika MENANG (Misal: "SceneWin")
    public string namaSceneKalah;      // <-- TAMBAHAN: Nama scene jika KALAH (Misal: "SceneLose")

    // --- PENJELASAN: AWAKE & PENCEGAHAN DUPLIKAT ---
    private void Awake()
    {
        // Karena GameManager ini adalah "Bos Utama" (Singleton), dia HANYA BOLEH ADA SATU di dalam game.
        // Jika kursinya masih kosong (null), maka isi dengan diri saya sendiri (this).
        if (instance == null) instance = this;

        // Tapi, jika ternyata sudah ada GameManager lain yang menjabat (misal dari scene sebelumnya),
        // maka hancurkan diri saya sendiri (Destroy) agar tidak ada dua Bos (duplikat) yang bikin error.
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Saat game baru mulai, langsung perbarui tulisan teks di layar (biar tertulis "Progres: 0 / 5")
        UpdateUI();
    }

    // Fungsi ini dipanggil oleh script materi ketika pemain selesai membaca
    public void TambahMateriSelesai()
    {
        materiSelesai++; // Tambah angka materi selesai sebanyak 1
        UpdateUI(); // Perbarui teks di layar
    }

    // Fungsi ini dipanggil oleh script UiSoal ketika pemain berhasil menjawab soal
    public void TambahSoalSelesai()
    {
        soalSelesai++; // Tambah angka soal selesai sebanyak 1
        UpdateUI(); // Perbarui teks di layar

        // Cek apakah jumlah soal yang sudah dijawab sama dengan atau melebihi target?
        if (soalSelesai >= totalSoalDiLevel)
        {
            // Kalau ya, panggil fungsi menang
            LevelSelesai();
        }
    }

    // Fungsi ini dipakai oleh script Player untuk ngecek: "Eh Bos, si pemain udah baca semua materi belum?"
    // Fungsi yang pakai tipe 'bool' akan mengembalikan nilai 'true' (benar) atau 'false' (salah).
    public bool ApakahMateriSudahCukup()
    {
        return materiSelesai >= totalMateriDiLevel;
    }

    void UpdateUI()
    {
        // Kalau teks di layar tidak kosong (ada komponennya)
        if (txt_skorGlobal != null)
        {
            int totalSkor = materiSelesai + soalSelesai; // Hitung progres saat ini
            int totalTarget = totalMateriDiLevel + totalSoalDiLevel; // Hitung total syarat kemenangan

            // Gabungkan kata-kata menjadi satu kalimat utuh
            txt_skorGlobal.text = "Progres: " + totalSkor + " / " + totalTarget;
        }
    }

    void LevelSelesai()
    {
        // --- PENJELASAN: DEBUG LOG ---
        // Debug.Log hanya akan muncul di layar 'Console' khusus programmer. 
        // Pemain biasa tidak akan melihat tulisan ini. Sangat berguna untuk melacak apakah kode kita berjalan.
        Debug.Log("Semua soal selesai! Pindah ke scene Win.");

        // Cek 2 hal: 
        // 1. Apakah script pemindah layarnya (ScreenChanger) sudah dimasukkan?
        // 2. string.IsNullOrEmpty mengecek: Apakah kotak 'Nama Scene Berikutnya' di Inspector sudah diisi huruf? (Tidakkosong)
        if (screenChanger != null && !string.IsNullOrEmpty(namaSceneBerikutnya))
        {
            // Kalau dua syarat aman, perintahkan pindah scene!
            screenChanger.PindahKeScene(namaSceneBerikutnya);
        }
        else
        {
            // Debug.LogError memunculkan pesan peringatan berwarna merah terang di Console programmer.
            Debug.LogError("Gagal pindah scene Win!");
        }
    }

    // <-- TAMBAHAN: Fungsi untuk memanggil scene LOSE (Dipanggil oleh UiSoal kalau jawaban banyak yang salah)
    public void LevelKalah()
    {
        Debug.Log("Gagal memenuhi syarat minimum soal! Pindah ke scene Lose.");

        // Logika pencegahan error yang sama dengan saat menang
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