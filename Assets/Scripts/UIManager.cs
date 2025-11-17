using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI References")]
    [SerializeField] private GameObject playerUi;
    [SerializeField] private GameObject taskMenu;
    [SerializeField] private TaskMenu taskManager;
    [SerializeField] private GameObject dialogue;
    [SerializeField] private GameObject judgement;
    [SerializeField] private GameObject dayTeller;
    private CanvasGroup dayCanvasGroup;

    private float dayTimer = 0f;
    private float taskTimer = 0f;
    private float fadeTimer = 0f;
    private bool isShowingDay = false;
    private bool isFadingIn = false;
    private bool isFadingOut = false;
    private bool isShowingTask = false;
    private float fadeDuration = 1f;

    public bool isInUI { get; private set; }

    private InputAction interactAction;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction escapeAction;

    void Awake()
    {
        instance = this;

        // Ensure playerUi is always active
        if (playerUi != null) playerUi.SetActive(true);
        // Find UI references if not assigned
        if (taskMenu == null) taskMenu = FindFirstObjectByType<TaskMenu>().gameObject;
        if (taskManager == null) taskManager = FindFirstObjectByType<TaskMenu>();
        if (taskMenu == null && taskManager != null) taskMenu = taskManager.gameObject;
        if (dialogue == null) dialogue = FindFirstObjectByType<DialogueManager>().dialoguePanel;
        if (judgement == null) judgement = FindFirstObjectByType<JudgementManager>().gameObject;
        // Get canvases
        dayCanvasGroup = dayTeller?.GetComponent<CanvasGroup>();
        if (dayCanvasGroup == null && dayTeller != null)
        {
            dayCanvasGroup = dayTeller.AddComponent<CanvasGroup>();
        }
        // Find input actions
        interactAction = InputSystem.actions.FindAction("Interact");
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        escapeAction = InputSystem.actions.FindAction("Escape");
    }

    void Update()
    {
        if (isFadingIn)
        {
            fadeTimer += Time.deltaTime;
            if (dayCanvasGroup != null)
            {
                dayCanvasGroup.alpha = Mathf.Lerp(0f, 1f, fadeTimer / fadeDuration);
            }
            if (fadeTimer >= fadeDuration)
            {
                isFadingIn = false;
                isShowingDay = true;
                dayTimer = 0f;
            }
        }
        else if (isShowingDay)
        {
            dayTimer += Time.deltaTime;
            if (dayTimer >= 2f || (escapeAction != null && escapeAction.WasPressedThisFrame()))
            {
                isShowingDay = false;
                isFadingOut = true;
                fadeTimer = 0f;
            }
        }
        else if (isFadingOut)
        {
            fadeTimer += Time.deltaTime;
            if (dayCanvasGroup != null)
            {
                dayCanvasGroup.alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);
            }
            if (fadeTimer >= fadeDuration)
            {
                isFadingOut = false;
                dayTeller.SetActive(false);
                FindFirstObjectByType<TaskMenu>().ShowTaskMenu(true);
                taskTimer = 0f;
                isShowingTask = true;
            }
        }
        else if (isShowingTask)
        {
            taskTimer += Time.deltaTime;
            if (taskTimer >= 5f || (escapeAction != null && escapeAction.WasPressedThisFrame()))
            {
                isShowingTask = false;
                HideAll();
            }
        }
    }

    public void ShowTaskMenu()
    {
        HideAll();
        if (taskMenu != null) taskMenu.SetActive(true);
        isInUI = true;
        UnlockMouse();
    }

    public void ShowDayText(int dayNumber)
    {
        HideAll();
        TextMeshProUGUI dayText = dayTeller.GetComponentInChildren<TextMeshProUGUI>();
        dayText.text = $"Day {dayNumber + 1}";
        dayTeller.SetActive(true);
        dayCanvasGroup.alpha = 0f;
        isFadingIn = true;
        fadeTimer = 0f;
        isInUI = true;
        UnlockMouse();
    }

    public void StartDaySequence(int dayNumber)
    {
        UnlockMouse();
        ShowDayText(dayNumber);
    }

    public void ShowJudgement()
    {
        HideAll();
        if (judgement != null) judgement.SetActive(true);
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
        if (dayTeller != null) dayTeller.SetActive(false);
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