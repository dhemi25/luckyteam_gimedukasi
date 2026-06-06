using UnityEngine;

public class CloseCanvas : MonoBehaviour
{
    
    public GameObject canvasInteract;

    public void TutupCanvas()
    {
        canvasInteract.SetActive(false);
    }
}
