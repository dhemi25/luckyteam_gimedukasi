using UnityEngine;

public class Interact : MonoBehaviour
{
    public PlayerContrrollerAdventure player;

    // Penanda agar objek tidak dihitung / disentuh dua kali
    [HideInInspector] public bool sudahSelesai = false;

    private void Awake()
    {
        player.SetOnInteractionArea(false, null);
    }

    private void OnTriggerEnter(Collider other)
    {
        // TAMBAHAN: Jika sudah selesai, abaikan trigger ini sama sekali
        if (sudahSelesai) return;

        if (other.CompareTag("Player"))
        {
            player.SetOnInteractionArea(true, this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.SetOnInteractionArea(false, null);
        }
    }
}