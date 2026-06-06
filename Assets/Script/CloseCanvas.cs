using UnityEngine;

public class CloseCanvas : MonoBehaviour
{
    
    public GameObject canvasInteract;
    public PlayerContrrollerAdventure playerContrrollerAdventure;
    public void TutupCanvas()
    {
        canvasInteract.SetActive(false);
        playerContrrollerAdventure.can_move = true;
    }
}
