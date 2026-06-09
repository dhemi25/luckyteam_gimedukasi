using UnityEngine;

// --- PENJELASAN: ENUM (ENUMERATION) ---
// Enum adalah cara untuk membuat "kategori" atau "pilihan dropdown" buatan kita sendiri.
// Di sini kita membuat kategori bernama TipeInteract yang isinya cuma ada 2 pilihan: Soal atau Materi.
// Ini sangat memudahkan saat kita mengatur objek di Unity Inspector karena tidak perlu mengetik manual (mencegah typo).
public enum TipeInteract { Soal, Materi }

public class Interact : MonoBehaviour
{
    // Mengambil referensi dari script pemain agar objek ini bisa "ngobrol" dengan si pemain
    public PlayerContrrollerAdventure player;

    [Header("Pengaturan Objek")]
    public TipeInteract tipeObjek; // Akan muncul sebagai dropdown (Soal / Materi) di Unity
    public string namaObjek = "Nama Objek"; // Nama yang akan muncul di layar (contoh: "Buku IPA")

    [Header("Khusus Tipe Materi")]
    [Tooltip("Masukkan gambar-gambar materi khusus untuk objek ini saja")] // Tooltip memunculkan teks bantuan saat mouse diarahkan ke variabel ini di Unity
    public Sprite[] halamanMateri; // Array (kumpulan) gambar yang akan ditampilkan jika tipenya Materi

    // HideInInspector membuat variabel ini tidak terlihat di Unity (karena diatur oleh kode),
    // tapi tetap public agar script lain (seperti UiSoal) bisa membaca dan mengubah statusnya.
    [HideInInspector] public bool sudahSelesai = false;

    // --- PENJELASAN: AWAKE VS START ---
    // Awake dipanggil paling pertama kali saat objek hidup, bahkan sebelum fungsi Start() dijalankan.
    private void Awake()
    {
        // Pastikan saat objek ini baru muncul di dunia game, tidak ada interaksi yang nyangkut/terbuka.
        if (player != null) player.SetOnInteractionArea(false, null);
    }

    // --- PENJELASAN: ON TRIGGER ENTER (SISTEM SENSOR MASUK) ---
    // Fungsi bawaan Unity yang otomatis berjalan saat ada objek menembus area Collider objek ini.
    // SYARAT: Objek ini harus memiliki komponen BoxCollider (atau sejenisnya) dan dicentang "Is Trigger"-nya.
    // 'other' adalah data dari objek apa pun yang menabrak/masuk ke area sensor ini.
    private void OnTriggerEnter(Collider other)
    {
        // CompareTag mengecek: "Apakah yang menabrak saya memiliki Tag bernama 'Player'?"
        // Ini penting agar objek tidak bereaksi kalau yang nabrak adalah musuh, peluru, atau tembok.
        if (other.CompareTag("Player") && player != null)
        {
            // --- PENJELASAN: KATA KUNCI 'this' ---
            // 'this' artinya adalah "diri saya sendiri" atau "script Interact yang menempel di objek ini".
            // Jadi perintah ini membisikkan ke pemain: 
            // "Hei Pemain, kamu masuk ke area interaksi (true), dan objek yang ada di depanmu adalah SAYA (this)."
            player.SetOnInteractionArea(true, this);
        }
    }

    // --- PENJELASAN: ON TRIGGER EXIT (SISTEM SENSOR KELUAR) ---
    // Otomatis berjalan saat objek yang tadi menabrak akhirnya keluar atau menjauh dari area Collider.
    private void OnTriggerExit(Collider other)
    {
        // Kalau yang keluar dari area adalah si Pemain...
        if (other.CompareTag("Player") && player != null)
        {
            // Beritahu pemain: 
            // "Hei Pemain, kamu sudah keluar dari area interaksi (false), jadi kosongkan data objek di depanmu (null)."
            player.SetOnInteractionArea(false, null);
        }
    }
}