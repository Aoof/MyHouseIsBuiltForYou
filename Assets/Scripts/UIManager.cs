using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI References")]
    [SerializeField] private GameObject playerUi;
    [SerializeField] private GameObject taskMenu;
    [SerializeField] private GameObject dialogue;

    private Canvas playerCanvas;
    private Canvas taskCanvas;
    private Canvas dialogueCanvas;

    public bool isInUI { get; private set; }

    private InputAction interactAction;
    private InputAction moveAction;
    private InputAction lookAction;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Ensure playerUi is always active
        if (playerUi != null) playerUi.SetActive(true);
        // Find UI references if not assigned
        if (taskMenu == null) taskMenu = FindFirstObjectByType<TaskMenu>()?.gameObject;
        if (dialogue == null) dialogue = FindFirstObjectByType<DialogueManager>()?.dialoguePanel;
        // Get canvases
        playerCanvas = playerUi?.GetComponent<Canvas>();
        taskCanvas = taskMenu?.GetComponent<Canvas>();
        dialogueCanvas = dialogue?.GetComponent<Canvas>();
        // Find input actions
        interactAction = InputSystem.actions.FindAction("Interact");
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        HideAll();
    }

    public void ShowTaskMenu()
    {
        HideAll();
        if (taskMenu != null) taskMenu.SetActive(true);
        isInUI = true;
        UnlockMouse();
    }

    public void ShowDialogue()
    {
        HideAll();
        if (dialogue != null) dialogue.SetActive(true);
        isInUI = true;
        UnlockMouse();
    }

    public void HideAll()
    {
        if (taskMenu != null) taskMenu.SetActive(false);
        if (dialogue != null) dialogue.SetActive(false);
        isInUI = false;
        LockMouse();
    }

    private void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (interactAction != null) interactAction.Disable();
        if (moveAction != null) moveAction.Disable();
        if (lookAction != null) lookAction.Disable();
    }

    private void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (interactAction != null) interactAction.Enable();
        if (moveAction != null) moveAction.Enable();
        if (lookAction != null) lookAction.Enable();
    }
}