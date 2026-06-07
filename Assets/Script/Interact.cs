using UnityEngine;

public enum TipeInteract { Soal, Materi }

public class Interact : MonoBehaviour
{
    public PlayerContrrollerAdventure player;

    [Header("Pengaturan Objek")]
    public TipeInteract tipeObjek;
    public string namaObjek = "Nama Objek";

    [Header("Khusus Tipe Materi")]
    [Tooltip("Masukkan gambar-gambar materi khusus untuk objek ini saja")]
    public Sprite[] halamanMateri; // <-- SEKARANG GAMBAR DISIMPAN DI SINI

    [HideInInspector] public bool sudahSelesai = false;

    private void Awake()
    {
        if (player != null) player.SetOnInteractionArea(false, null);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && player != null)
        {
            player.SetOnInteractionArea(true, this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && player != null)
        {
            player.SetOnInteractionArea(false, null);
        }
    }
}