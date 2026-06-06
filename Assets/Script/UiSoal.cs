using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiSoal : MonoBehaviour
{
    public PlayerContrrollerAdventure player;
    public GameObject canvasInteract;

    [Header("UI Kuis")]
    public TextMeshProUGUI txt_soal;
    public TextMeshProUGUI txt_notif;
    public Button[] btnJawaban;

    [Header("Data Soal")]
    public List<DataSoal> listSoal;

    private int currentSoalIndex;
    private int skorBenar; // Ganti nama dari skorKuis agar lebih jelas
    private int skorSalah; // Variabel baru untuk menghitung jawaban salah

    void Start()
    {
        for (int i = 0; i < btnJawaban.Length; i++)
        {
            int indexTombol = i;
            btnJawaban[i].onClick.RemoveAllListeners();
            btnJawaban[i].onClick.AddListener(() => PilihJawaban(indexTombol));
        }
    }

    public void MulaiQuiz()
    {
        currentSoalIndex = 0;
        skorBenar = 0;
        skorSalah = 0; // Reset skor salah

        // Tampilkan notif awal dengan skor 0
        UpdateNotif("Pilih jawaban yang benar!", Color.white);

        if (listSoal != null && listSoal.Count > 0)
        {
            SetUpTextSoal();
        }
    }

    // Fungsi pembantu untuk memperbarui teks notif dengan skor
    void UpdateNotif(string status, Color warna)
    {
        if (txt_notif != null)
        {
            txt_notif.gameObject.SetActive(true);
            txt_notif.text = $"{status}\n<size=80%>Jawaban Benar: {skorBenar} | Salah: {skorSalah}</size>";
            txt_notif.color = warna;
        }
    }

    public void PilihJawaban(int indexDipilih)
    {
        int indexBenar = listSoal[currentSoalIndex].indexJawabanBenar;

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

        foreach (Button btn in btnJawaban) btn.interactable = false;
        Invoke(nameof(LanjutSoal), 1.5f);
    }

    void LanjutSoal()
    {
        currentSoalIndex++;
        if (currentSoalIndex >= listSoal.Count)
        {
            SelesaikanQuiz();
        }
        else
        {
            SetUpTextSoal();
            // Kembalikan notif ke pesan instruksi saat soal baru muncul
            UpdateNotif("Pilih jawaban yang benar!", Color.white);
        }
    }

    void SelesaikanQuiz()
    {
        // Cek jika benar semua
        if (skorBenar == listSoal.Count)
        {
            if (player.objekInteraksiSaatIni != null && !player.objekInteraksiSaatIni.sudahSelesai)
            {
                player.objekInteraksiSaatIni.sudahSelesai = true;

                if (GameManager.instance != null)
                {
                    GameManager.instance.TambahObjekSelesai();
                }

                // Hancurkan objek 3D
                Destroy(player.objekInteraksiSaatIni.gameObject);

                // TAMBAHAN PENTING: Kosongkan variabel agar player tidak 'teringat' objek lama
                player.objekInteraksiSaatIni = null;
            }
        }

        // Tutup canvas
        if (canvasInteract != null) canvasInteract.SetActive(false);

        // Izinkan jalan kembali
        player.can_move = true;

        // TAMBAHAN PENTING: Panggil fungsi ini untuk memastikan UI "Press E" dimatikan
        player.SetOnInteractionArea(false, null);
    }

    // ... (Fungsi SetUpTextSoal biarkan tetap sama seperti sebelumnya)
    public void SetUpTextSoal()
    {
        DataSoal soal = listSoal[currentSoalIndex];
        txt_soal.text = soal.Soal;
        for (int i = 0; i < btnJawaban.Length; i++)
        {
            if (i < soal.jawaban.Count)
            {
                Text textLegacy = btnJawaban[i].GetComponentInChildren<Text>();
                TextMeshProUGUI textTMP = btnJawaban[i].GetComponentInChildren<TextMeshProUGUI>();
                if (textLegacy != null) textLegacy.text = soal.jawaban[i];
                else if (textTMP != null) textTMP.text = soal.jawaban[i];

                btnJawaban[i].interactable = true;
                btnJawaban[i].gameObject.SetActive(true);
            }
            else btnJawaban[i].gameObject.SetActive(false);
        }
    }
}