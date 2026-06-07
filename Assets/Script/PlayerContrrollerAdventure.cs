using UnityEngine;
using TMPro; // Diperlukan untuk TextMeshPro
using System.Collections;

public class PlayerContrrollerAdventure : MonoBehaviour
{
    public float speed = 5f;
    public Transform cameraTransform;
    CharacterController controller;

    private bool onInteractArea;
    [HideInInspector] public Interact objekInteraksiSaatIni;

    [Header("UI Interaksi (Tekan E)")]
    public GameObject objInfoInteract; // UI "Press E"
    public TextMeshProUGUI txt_namaObjek; // Teks untuk menampilkan nama objek

    [Header("UI Peringatan")]
    public TextMeshProUGUI txt_peringatan; // Teks "Selesaikan materi dulu!"

    [Header("Canvas & System")]
    public GameObject canvasSoal;
    public UiSoal uiSoal;
    public GameObject canvasMateri; // Canvas baru untuk materi
    public UiMateri uiMateri; // Script baru untuk materi

    public Animator playerAnimator;
    public bool can_move = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // PERBAIKAN 1: Memastikan semua canvas mati saat game baru dimulai
        if (canvasSoal != null) canvasSoal.SetActive(false);
        if (canvasMateri != null) canvasMateri.SetActive(false);

        if (objInfoInteract != null) objInfoInteract.SetActive(false);
        if (txt_peringatan != null) txt_peringatan.gameObject.SetActive(false);

        // Kosongkan teks nama objek di awal game
        if (txt_namaObjek != null) txt_namaObjek.text = "";
    }

    void Update()
    {
        if (!can_move) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0; right.y = 0;

        Vector3 moveDirection = forward * v + right * h;
        controller.Move(moveDirection.normalized * speed * Time.deltaTime);

        if (moveDirection != Vector3.zero)
        {
            transform.forward = moveDirection;
            playerAnimator.enabled = true;
        }
        else
        {
            playerAnimator.enabled = false;
        }

        if (onInteractArea && Input.GetKeyDown(KeyCode.E))
        {
            if (objekInteraksiSaatIni == null) return;

            // CEK TIPE OBJEK
            if (objekInteraksiSaatIni.tipeObjek == TipeInteract.Soal)
            {
                if (!GameManager.instance.ApakahMateriSudahCukup())
                {
                    StartCoroutine(TampilkanPeringatan("Baca semua materi terlebih dahulu!"));
                    return;
                }

                canvasSoal.SetActive(true);
                uiSoal.MulaiQuiz();
            }
            else if (objekInteraksiSaatIni.tipeObjek == TipeInteract.Materi)
            {
                canvasMateri.SetActive(true);
                uiMateri.BukaMateri(objekInteraksiSaatIni);
            }

            can_move = false;

            // PERBAIKAN: Cukup matikan visual UI-nya saja secara manual, 
            // JANGAN hapus datanya dengan SetOnInteractionArea(false, null)
            if (objInfoInteract != null) objInfoInteract.SetActive(false);
            if (txt_namaObjek != null) txt_namaObjek.gameObject.SetActive(false);
        }
    }

    // PERBAIKAN 2: Logika penanganan teks nama objek saat keluar/masuk area collider
    public void SetOnInteractionArea(bool isInteract, Interact objInteract)
    {
        onInteractArea = isInteract;
        objekInteraksiSaatIni = objInteract;

        // Jika perintahnya adalah mematikan interaksi ATAU datanya kosong, paksa ke kondisi false
        if (!isInteract || objInteract == null)
        {
            isInteract = false;
        }

        // Tampilkan/sembunyikan UI "Press E"
        if (objInfoInteract != null)
        {
            objInfoInteract.SetActive(isInteract);
        }

        // Atur teks nama objek berdasarkan kondisi interaksi
        if (txt_namaObjek != null)
        {
            if (isInteract && objInteract != null)
            {
                txt_namaObjek.text = objInteract.namaObjek;
                txt_namaObjek.gameObject.SetActive(true);
            }
            else
            {
                txt_namaObjek.text = ""; // Kosongkan text agar tidak membekas di layar
                txt_namaObjek.gameObject.SetActive(false); // Sembunyikan komponen teks
            }
        }
    }

    IEnumerator TampilkanPeringatan(string pesan)
    {
        if (txt_peringatan != null)
        {
            txt_peringatan.text = pesan;
            txt_peringatan.gameObject.SetActive(true);
            yield return new WaitForSeconds(2.5f);
            txt_peringatan.gameObject.SetActive(false);
        }
    }
}