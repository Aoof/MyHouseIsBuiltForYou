using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI References")]
    [SerializeField] private GameObject playerUi;
    [SerializeField] private GameObject taskMenu;
    [SerializeField] private TaskMenu taskManager;
    [SerializeField] private GameObject dialogue;
    [SerializeField] private GameObject judgement;
    [SerializeField] private TMPro.TextMeshProUGUI dayText;

    private Canvas playerCanvas;
    private Canvas taskCanvas;
    private Canvas dialogueCanvas;
    private Canvas judgementCanvas;

    public bool isInUI { get; private set; }

    private InputAction interactAction;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction escapeAction;

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
        if (taskManager == null) taskManager = FindFirstObjectByType<TaskMenu>();
        if (taskMenu == null && taskManager != null) taskMenu = taskManager.gameObject;
        if (dialogue == null) dialogue = FindFirstObjectByType<DialogueManager>()?.dialoguePanel;
        if (judgement == null) judgement = FindFirstObjectByType<JudgementCanvas>()?.gameObject;
        // Get canvases
        playerCanvas = playerUi?.GetComponent<Canvas>();
        taskCanvas = taskMenu?.GetComponent<Canvas>();
        dialogueCanvas = dialogue?.GetComponent<Canvas>();
        judgementCanvas = judgement?.GetComponent<Canvas>();
        // Find input actions
        interactAction = InputSystem.actions.FindAction("Interact");
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        escapeAction = InputSystem.actions.FindAction("Escape");
        HideAll();
    }

    public void ShowTaskMenu()
    {
        HideAll();
        taskManager.isReadOnly = false;
        if (taskMenu != null) taskMenu.SetActive(true);
        isInUI = true;
        UnlockMouse();
    }

    public void ShowTaskMenuReadOnly()
    {
        HideAll();
        if (taskManager != null)
        {
            taskMenu.SetActive(true);
            taskManager.isReadOnly = true;
            taskManager.UpdateButtons();
        }
        isInUI = true;
        UnlockMouse();
    }

    public void ShowDayText(int dayNumber)
    {
        HideAll();
        if (dayText != null)
        {
            dayText.gameObject.SetActive(true);
            dayText.text = $"Day {dayNumber + 1}";
        }
        isInUI = true;
        UnlockMouse();
    }

    public void StartDaySequence(int dayNumber)
    {
        StartCoroutine(DaySequenceCoroutine(dayNumber));
    }

    private IEnumerator DaySequenceCoroutine(int dayNumber)
    {
        ShowDayText(dayNumber);
        yield return new WaitForSeconds(2f); // Show day for 2 seconds
        ShowTaskMenuReadOnly();
        float timer = 0f;
        while (timer < 5f) // Show for 5 seconds or until escape
        {
            timer += Time.deltaTime;
            if (escapeAction != null && escapeAction.WasPressedThisFrame())
            {
                break;
            }
            yield return null;
        }
        if (taskMenu.activeSelf && taskManager.isReadOnly)
        {
            HideAll();
        }
    }

    public void ShowJudgement()
    {
        HideAll();
        if (judgement != null)
        {
            judgement.SetActive(true);
            Debug.Log("Judgement UI shown, unlocking mouse");
        }
        else
        {
            Debug.LogError("Judgement UI not assigned!");
        }
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
        if (judgement != null) judgement.SetActive(false);
        if (dayText != null) dayText.gameObject.SetActive(false);
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
        if (escapeAction != null) escapeAction.Enable();
    }

    private void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (interactAction != null) interactAction.Enable();
        if (moveAction != null) moveAction.Enable();
        if (lookAction != null) lookAction.Enable();
        if (escapeAction != null) escapeAction.Disable();
    }
}