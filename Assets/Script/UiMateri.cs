using UnityEngine;
using UnityEngine.UI;

public class UiMateri : MonoBehaviour
{
    public PlayerContrrollerAdventure player;
    public GameObject canvasMateri;

    [Header("UI Materi")]
    public Image wadahGambar;
    public Button btnNext;
    public Button btnPrev;
    public Button btnTutup;

    // Sekarang diubah menjadi private karena datanya akan diisi otomatis lewat kode
    private Sprite[] daftarHalaman;
    private int indeksHalaman = 0;
    private Interact materiSaatIni;

    void Start()
    {
        btnNext.onClick.AddListener(HalamanSelanjutnya);
        btnPrev.onClick.AddListener(HalamanSebelumnya);
        btnTutup.onClick.AddListener(TutupMateri);
    }

    public void BukaMateri(Interact objekMateri)
    {
        materiSaatIni = objekMateri;
        indeksHalaman = 0;

        // TAHAPAN PENTING: Ambil gambar dari objek materi yang sedang ditabrak player
        daftarHalaman = objekMateri.halamanMateri;

        TampilkanHalaman();
    }

    void TampilkanHalaman()
    {
        // Cek dulu apakah objek materi tersebut ada gambarnya atau kosong
        if (daftarHalaman != null && daftarHalaman.Length > 0)
        {
            wadahGambar.sprite = daftarHalaman[indeksHalaman];

            // Atur tombol aktif atau tidak
            btnPrev.interactable = (indeksHalaman > 0);
            btnNext.interactable = (indeksHalaman < daftarHalaman.Length - 1);
        }
        else
        {
            Debug.LogWarning("Objek " + materiSaatIni.namaObjek + " belum dimasukkan gambar materinya di Inspector!");
            btnPrev.interactable = false;
            btnNext.interactable = false;
        }
    }

    void HalamanSelanjutnya()
    {
        if (daftarHalaman != null && indeksHalaman < daftarHalaman.Length - 1)
        {
            indeksHalaman++;
            TampilkanHalaman();
        }
    }

    void HalamanSebelumnya()
    {
        if (indeksHalaman > 0)
        {
            indeksHalaman--;
            TampilkanHalaman();
        }
    }

    void TutupMateri()
    {
        if (materiSaatIni != null && !materiSaatIni.sudahSelesai)
        {
            materiSaatIni.sudahSelesai = true;

            if (GameManager.instance != null)
            {
                GameManager.instance.TambahMateriSelesai();
            }
        }

        if (canvasMateri != null) canvasMateri.SetActive(false);

        if (player != null)
        {
            player.can_move = true;
            if (materiSaatIni != null)
            {
                player.SetOnInteractionArea(true, materiSaatIni);
            }
        }
    }
}