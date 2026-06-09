using UnityEngine;
using TMPro; // Diperlukan untuk menampilkan teks menggunakan TextMeshPro
using System.Collections;

public class PlayerContrrollerAdventure : MonoBehaviour
{
    [Header("Pengaturan Pemain")]
    public float speed = 5f; // Kecepatan jalan karakter
    public Transform cameraTransform; // Posisi kamera, agar jalan karakter bisa menyesuaikan arah kamera
    CharacterController controller; // Komponen bawaan Unity untuk menggerakkan karakter
    public Animator playerAnimator; // Untuk mengatur animasi jalan/diam
    public bool can_move = true; // Tombol on/off apakah pemain boleh bergerak atau tidak

    [Header("Sistem Interaksi")]
    private bool onInteractArea; // Penanda apakah pemain sedang berada di dekat objek yang bisa diinteraksi
    [HideInInspector] public Interact objekInteraksiSaatIni; // Menyimpan data objek apa yang sedang ada di depan pemain

    [Header("UI Interaksi (Tekan E)")]
    public GameObject objInfoInteract; // Munculin tulisan "Tekan E"
    public TextMeshProUGUI txt_namaObjek; // Munculin nama objeknya (contoh: "Papan Tulis")

    [Header("UI Peringatan")]
    public TextMeshProUGUI txt_peringatan; // Tulisan merah/peringatan (contoh: "Selesaikan materi dulu!")

    [Header("Canvas & System (Tampilan Layar)")]
    public GameObject canvasSoal; // Layar untuk kuis/soal
    public UiSoal uiSoal; // Script pengatur kuis
    public GameObject canvasMateri; // Layar untuk baca materi
    public UiMateri uiMateri; // Script pengatur materi

    void Start()
    {
        // Mengambil komponen CharacterController yang menempel pada pemain
        controller = GetComponent<CharacterController>();

        // PERSIAPAN AWAL: Matikan semua tampilan UI (layar) saat game baru dimulai
        if (canvasSoal != null) canvasSoal.SetActive(false);
        if (canvasMateri != null) canvasMateri.SetActive(false);
        if (objInfoInteract != null) objInfoInteract.SetActive(false);
        if (txt_peringatan != null) txt_peringatan.gameObject.SetActive(false);

        // Kosongkan tulisan nama objek di awal game
        if (txt_namaObjek != null) txt_namaObjek.text = "";
    }

    void Update()
    {
        // Kalau pemain lagi nggak boleh gerak (misal lagi baca materi/kuis), hentikan kode di bawahnya
        if (!can_move) return;

        // --- PENJELASAN: INPUT PEMAIN ---
        // GetAxis menghasilkan angka dari -1 sampai 1.
        // Tekan 'A' (kiri) = -1. Tekan 'D' (kanan) = 1. Dilepas = 0.
        // Tekan 'S' (mundur) = -1. Tekan 'W' (maju) = 1. Dilepas = 0.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // --- PENJELASAN: MATEMATIKA ARAH (VECTOR) ---
        // Kita ingin pemain maju ke arah mana kamera sedang melihat.
        // cameraTransform.forward mengambil arah "depan" dari kamera.
        // cameraTransform.right mengambil arah "kanan" dari kamera.
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // MENGAPA Y = 0? 
        // Sumbu Y adalah Atas/Bawah. Jika kamera menunduk, kita tidak mau karakter ikut 
        // tembus ke dalam tanah. Jadi kita paksa sumbu atas/bawah (Y) menjadi 0.
        forward.y = 0; right.y = 0;

        // Menggabungkan arah berdasarkan tombol yang ditekan (h dan v).
        Vector3 moveDirection = forward * v + right * h;

        // --- PENJELASAN: NORMALIZED & TIME.DELTATIME ---
        // .normalized: Mencegah pemain jalan lebih cepat saat bergerak diagonal (tekan W dan D bersamaan).
        // Time.deltaTime: Membuat kecepatan jalan sama rata di semua komputer (baik yang spek dewa maupun kentang).
        controller.Move(moveDirection.normalized * speed * Time.deltaTime);

        // --- PENJELASAN: ANIMASI DAN ARAH BADAN ---
        // Vector3.zero artinya (0,0,0) atau pemain sedang diam.
        if (moveDirection != Vector3.zero)
        {
            // transform.forward mengubah arah dada karakter agar menghadap ke arah dia berjalan.
            transform.forward = moveDirection;
            playerAnimator.enabled = true; // Nyalakan animasi jalan
        }
        else
        {
            playerAnimator.enabled = false; // Matikan animasi jalan (kembali berdiri diam)
        }

        // --- PENJELASAN: LOGIKA INTERAKSI ---
        // Input.GetKeyDown berarti aksi dieksekusi 1x persis saat tombol 'E' ditekan, tidak akan berulang walau ditahan.
        if (onInteractArea && Input.GetKeyDown(KeyCode.E))
        {
            // Kalau nggak ada objeknya, batalin
            if (objekInteraksiSaatIni == null) return;

            // CEK TIPE OBJEK: Apakah ini kuis atau materi?
            if (objekInteraksiSaatIni.tipeObjek == TipeInteract.Soal)
            {
                // Mengambil data dari GameManager tanpa harus memasukkannya manual ke script ini (Pola Singleton).
                if (!GameManager.instance.ApakahMateriSudahCukup())
                {
                    // Memanggil fungsi waktu (Coroutine) untuk memunculkan peringatan
                    StartCoroutine(TampilkanPeringatan("Baca semua materi terlebih dahulu!"));
                    return;
                }

                // Kalau materi udah cukup, buka layar kuis
                canvasSoal.SetActive(true);
                uiSoal.MulaiQuiz();
            }
            else if (objekInteraksiSaatIni.tipeObjek == TipeInteract.Materi)
            {
                // Kalau tipenya materi, buka layar materi
                canvasMateri.SetActive(true);
                uiMateri.BukaMateri(objekInteraksiSaatIni);
            }

            // Kunci pergerakan pemain (biar nggak bisa jalan pas layar kuis/materi terbuka)
            can_move = false;

            // Sembunyikan tulisan "Tekan E" dan "Nama Objek" saat pemain lagi buka UI
            if (objInfoInteract != null) objInfoInteract.SetActive(false);
            if (txt_namaObjek != null) txt_namaObjek.gameObject.SetActive(false);
        }
    }

    // --- FUNGSI TRIGGER AREA INTERAKSI ---
    // Dipanggil saat pemain menabrak (masuk/keluar) area objek.
    public void SetOnInteractionArea(bool isInteract, Interact objInteract)
    {
        onInteractArea = isInteract;
        objekInteraksiSaatIni = objInteract;

        // Pencegahan error: Kalau disuruh matiin interaksi atau objeknya hilang, pastikan statusnya false
        if (!isInteract || objInteract == null)
        {
            isInteract = false;
        }

        // Tampilkan atau sembunyikan kotak tulisan "Tekan E"
        if (objInfoInteract != null)
        {
            objInfoInteract.SetActive(isInteract);
        }

        // Atur kemunculan tulisan Nama Objek
        if (txt_namaObjek != null)
        {
            if (isInteract && objInteract != null)
            {
                txt_namaObjek.text = objInteract.namaObjek;
                txt_namaObjek.gameObject.SetActive(true);
            }
            else
            {
                txt_namaObjek.text = "";
                txt_namaObjek.gameObject.SetActive(false);
            }
        }
    }

    // --- PENJELASAN: IENUMERATOR (COROUTINE) ---
    // IEnumerator adalah fungsi khusus untuk membuat sistem Timer tanpa membekukan keseluruhan game.
    IEnumerator TampilkanPeringatan(string pesan)
    {
        if (txt_peringatan != null)
        {
            txt_peringatan.text = pesan; // Isi teks peringatannya apa
            txt_peringatan.gameObject.SetActive(true); // Munculin di layar

            // Perintah ini memberitahu Unity: "Tahan fungsi ini di sini, biarkan game tetap berjalan normal, 
            // lalu lanjutkan sisa kode di bawah ini setelah 2.5 detik berlalu."
            yield return new WaitForSeconds(2.5f);

            txt_peringatan.gameObject.SetActive(false); // Sembunyikan lagi setelah 2.5 detik
        }
    }
}