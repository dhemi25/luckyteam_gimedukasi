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

    [Header("Aturan Kelulusan")]
    public int minimalBenarUntukLulus = 2; // <-- TAMBAHAN: Atur minimum jawaban benar di Inspector

    private int currentSoalIndex;
    private int skorBenar;
    private int skorSalah;

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
        skorSalah = 0;

        UpdateNotif("Pilih jawaban yang benar!", Color.white);

        if (listSoal != null && listSoal.Count > 0)
        {
            SetUpTextSoal();
        }
    }

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
            UpdateNotif("Pilih jawaban yang benar!", Color.white);
        }
    }

    void SelesaikanQuiz()
    {
        // <-- MODIFIKASI: Cek apakah skor mencapai minimum
        if (skorBenar >= minimalBenarUntukLulus)
        {
            if (player.objekInteraksiSaatIni != null && !player.objekInteraksiSaatIni.sudahSelesai)
            {
                player.objekInteraksiSaatIni.sudahSelesai = true;

                if (GameManager.instance != null)
                {
                    // Menambahkan progres soal. Jika progres terpenuhi, GameManager memanggil scene WIN
                    GameManager.instance.TambahSoalSelesai();
                }

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

        if (canvasInteract != null) canvasInteract.SetActive(false);
        player.can_move = true;
        player.SetOnInteractionArea(false, null);
    }

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