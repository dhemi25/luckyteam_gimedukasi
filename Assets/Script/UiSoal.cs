using System.Collections.Generic;
using TMPro; // Library untuk menggunakan TextMeshPro
using UnityEngine;
using UnityEngine.UI; // Library untuk menggunakan UI bawaan Unity (seperti Button dan Text biasa)

public class UiSoal : MonoBehaviour
{
    [Header("Referensi Sistem")]
    public PlayerContrrollerAdventure player; // Mengambil data pemain
    public GameObject canvasInteract; // Menyimpan referensi layar UI

    [Header("UI Kuis")]
    public TextMeshProUGUI txt_soal; // Tempat munculnya teks pertanyaan
    public TextMeshProUGUI txt_notif; // Tempat munculnya teks "BENAR!" atau "SALAH!"
    public Button[] btnJawaban; // Kumpulan tombol jawaban (A, B, C, D)

    [Header("Data Soal")]
    public List<DataSoal> listSoal; // Daftar soal yang diambil dari script/class DataSoal

    [Header("Aturan Kelulusan")]
    public int minimalBenarUntukLulus = 2; // <-- TAMBAHAN: Atur minimum jawaban benar di Inspector

    // --- PENJELASAN VARIABEL INTERNAL ---
    // Variabel 'private' berarti nilainya hanya bisa diakses dan diubah oleh script ini saja.
    private int currentSoalIndex; // Mengingat kita sedang di soal nomor berapa (Dimulai dari 0)
    private int skorBenar; // Jumlah tebakan benar
    private int skorSalah; // Jumlah tebakan salah

    void Start()
    {
        // --- PENJELASAN: LOOPING & LISTENER TOMBOL ---
        // Kita menggunakan 'for' untuk mengatur semua tombol jawaban secara otomatis 
        // tanpa harus memasukkannya satu per satu di Unity Inspector.
        for (int i = 0; i < btnJawaban.Length; i++)
        {
            // TRIK PENTING UNTUK PEMULA (Closure): 
            // Kenapa kita harus memindahkan 'i' ke 'indexTombol'? 
            // Jika kita langsung memasukkan 'i' ke dalam AddListener, semua tombol akan 
            // mengirimkan angka terakhir dari loop (misal 4). Dengan 'indexTombol', 
            // tombol pertama akan ingat dia nomor 0, tombol kedua nomor 1, dst.
            int indexTombol = i;

            // Hapus fungsi klik yang mungkin sudah ada sebelumnya biar tidak dobel
            btnJawaban[i].onClick.RemoveAllListeners();

            // Tambahkan perintah: "Kalau tombol ini diklik, jalankan fungsi PilihJawaban(nomor tombolnya)"
            btnJawaban[i].onClick.AddListener(() => PilihJawaban(indexTombol));
        }
    }

    public void MulaiQuiz()
    {
        // Reset semua angka saat kuis baru dimulai
        currentSoalIndex = 0;
        skorBenar = 0;
        skorSalah = 0;

        // Panggil fungsi UpdateNotif untuk memunculkan teks awal berwarna putih
        UpdateNotif("Pilih jawaban yang benar!", Color.white);

        // Kalau daftar soalnya ada (tidak kosong), atur tulisan soalnya
        if (listSoal != null && listSoal.Count > 0)
        {
            SetUpTextSoal();
        }
    }

    void UpdateNotif(string status, Color warna)
    {
        if (txt_notif != null)
        {
            txt_notif.gameObject.SetActive(true); // Munculkan teks notifikasi

            // --- PENJELASAN: STRING INTERPOLATION ($) ---
            // Simbol '$' di depan tanda kutip memungkinkan kita memasukkan variabel 
            // (seperti skorBenar) langsung ke dalam teks menggunakan kurung kurawal {}.
            // \n berfungsi seperti tombol 'Enter' untuk membuat baris baru.
            // <size=80%> adalah tag kaya teks (Rich Text) milik TextMeshPro untuk mengecilkan huruf.
            txt_notif.text = $"{status}\n<size=80%>Jawaban Benar: {skorBenar} | Salah: {skorSalah}</size>";
            txt_notif.color = warna; // Ubah warnanya (misal merah untuk salah, hijau untuk benar)
        }
    }

    public void PilihJawaban(int indexDipilih)
    {
        // Mengambil kunci jawaban yang benar dari data soal saat ini
        int indexBenar = listSoal[currentSoalIndex].indexJawabanBenar;

        // Kalau tombol yang diklik sama dengan kunci jawaban
        if (indexDipilih == indexBenar)
        {
            skorBenar++;
            UpdateNotif("BENAR!", Color.green);
        }
        else
        {
            skorSalah++;
            UpdateNotif("SALAH!", Color.red);
        }

        // --- PENJELASAN: MEMATIKAN TOMBOL SEMENTARA ---
        // Foreach ini akan mengecek semua tombol.
        // btn.interactable = false membuat tombol jadi redup dan tidak bisa diklik lagi (mencegah spam klik).
        foreach (Button btn in btnJawaban) btn.interactable = false;

        // --- PENJELASAN: INVOKE (SISTEM TIMER) ---
        // Invoke mirip dengan Coroutine, fungsinya untuk menunda eksekusi kode.
        // Kode di bawah ini berarti: "Tunggu 1.5 detik, lalu jalankan fungsi LanjutSoal".
        // nameof() sangat dianjurkan untuk mencegah salah ketik nama fungsi.
        Invoke(nameof(LanjutSoal), 1.5f);
    }

    void LanjutSoal()
    {
        // Tambah 1 ke nomor soal saat ini (lanjut ke soal berikutnya)
        currentSoalIndex++;

        // Kalau nomor soal sudah melebihi atau sama dengan jumlah total soal, artinya kuis tamat
        if (currentSoalIndex >= listSoal.Count)
        {
            SelesaikanQuiz();
        }
        else
        {
            // Kalau masih ada soal, atur lagi teksnya dan kembalikan notifikasi jadi putih
            SetUpTextSoal();
            UpdateNotif("Pilih jawaban yang benar!", Color.white);
        }
    }

    void SelesaikanQuiz()
    {
        // <-- MODIFIKASI: Cek apakah skor mencapai minimum untuk lulus
        if (skorBenar >= minimalBenarUntukLulus)
        {
            // Kalau lulus dan objek soal di dunia 3D masih ada
            if (player.objekInteraksiSaatIni != null && !player.objekInteraksiSaatIni.sudahSelesai)
            {
                player.objekInteraksiSaatIni.sudahSelesai = true; // Tandai sudah selesai

                if (GameManager.instance != null)
                {
                    // Menambahkan progres soal. Jika progres terpenuhi, GameManager memanggil scene WIN
                    GameManager.instance.TambahSoalSelesai();
                }

                // Hancurkan kotak objek soal di dunia 3D agar pemain tidak bisa mengkliknya lagi
                Destroy(player.objekInteraksiSaatIni.gameObject);
                player.objekInteraksiSaatIni = null;
            }
        }
        else
        {
            // <-- TAMBAHAN: Jika gagal memenuhi target, panggil scene LOSE
            if (GameManager.instance != null)
            {
                GameManager.instance.LevelKalah();
            }
        }

        // Tutup layar kuis, izinkan pemain jalan lagi, dan matikan sistem interaksi
        if (canvasInteract != null) canvasInteract.SetActive(false);
        player.can_move = true;
        player.SetOnInteractionArea(false, null);
    }

    public void SetUpTextSoal()
    {
        // Ambil data soal pada urutan saat ini
        DataSoal soal = listSoal[currentSoalIndex];

        txt_soal.text = soal.Soal; // Tampilkan teks pertanyaannya

        // Looping untuk mengatur teks pada setiap tombol jawaban
        for (int i = 0; i < btnJawaban.Length; i++)
        {
            // Cek apakah data jawaban untuk tombol ini tersedia
            if (i < soal.jawaban.Count)
            {
                // --- PENJELASAN: GET COMPONENT IN CHILDREN ---
                // Tombol di Unity biasanya memiliki "Anak" (Child) berupa objek Text.
                // Kode ini mencari komponen teks apa pun yang ada di dalam tombol tersebut, 
                // baik itu teks biasa (Legacy) maupun TextMeshPro.
                Text textLegacy = btnJawaban[i].GetComponentInChildren<Text>();
                TextMeshProUGUI textTMP = btnJawaban[i].GetComponentInChildren<TextMeshProUGUI>();

                if (textLegacy != null) textLegacy.text = soal.jawaban[i];
                else if (textTMP != null) textTMP.text = soal.jawaban[i];

                btnJawaban[i].interactable = true; // Nyalakan tombol agar bisa diklik lagi
                btnJawaban[i].gameObject.SetActive(true); // Munculkan tombolnya
            }
            else
            {
                // Kalau opsinya cuma A, B, C (3 jawaban), tombol ke-4 (D) disembunyikan
                btnJawaban[i].gameObject.SetActive(false);
            }
        }
    }
}