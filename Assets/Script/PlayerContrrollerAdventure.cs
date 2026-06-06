using UnityEngine;

public class PlayerContrrollerAdventure : MonoBehaviour
{
    public float speed = 5f;
    public Transform cameraTransform;
    CharacterController controller;

    private bool onInteractArea;

    // Menyimpan SCRIPT Interact dari objek yang ditabrak
    [HideInInspector] public Interact objekInteraksiSaatIni;

    public GameObject objInfoInteract; // UI "Press E"
    public GameObject canvasInteract; // Canvas Quiz
    public Animator playerAnimator;
    public bool can_move = true;

    public UiSoal uiSoal;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        canvasInteract.SetActive(false);
        if (objInfoInteract != null) objInfoInteract.SetActive(false);
    }

    void Update()
    {
        if (!can_move) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

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

        if (onInteractArea && objInfoInteract != null)
        {
            objInfoInteract.transform.Rotate(0f, 120f * Time.deltaTime, 0f);
        }

        if (onInteractArea && Input.GetKeyDown(KeyCode.E))
        {
            canvasInteract.SetActive(true);
            can_move = false;

            uiSoal.MulaiQuiz();

            // Matikan UI "Press E" sementara canvas kuis terbuka
            if (objInfoInteract != null) objInfoInteract.SetActive(false);
        }
    }

    // Fungsi dimodifikasi menerima tipe Interact
    public void SetOnInteractionArea(bool isInteract, Interact objInteract)
    {
        onInteractArea = isInteract;
        objekInteraksiSaatIni = objInteract;

        if (objInfoInteract != null)
        {
            objInfoInteract.SetActive(isInteract);
        }
    }
}