using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TaskMenu : MonoBehaviour
{
    [Header("References")]
    public PointsSystem pointsSystem;
    public PlayerInteractions playerInteractions;
    [Header("UI Elements")]
    [SerializeField] private Button[] taskButtons;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TextMeshProUGUI objectTitle;


    [NonSerialized]
    public string selectedTask = "";
    public Action onTaskSelected;
    public static bool isReadOnly = false;

    private InputAction escapeAction;

    void Awake()
    {
        if (playerInteractions == null) playerInteractions = FindFirstObjectByType<PlayerInteractions>();
        escapeAction = InputSystem.actions.FindAction("Escape");

        pointsSystem = FindFirstObjectByType<PointsSystem>();
        if (pointsSystem == null)
        {
            Debug.LogError("PointsSystem not found!");
            return;
        }
        if (taskButtons == null || taskButtons.Length == 0)
        {
            Debug.LogError("Task buttons not assigned!");
            return;
        }
        UpdateButtons();
    }

    void OnEnable()
    {
        selectedTask = "";
        isReadOnly = false;
        UpdateButtons();
        escapeAction.Enable();
    }

    void OnDisable()
    {
        escapeAction.Disable();
    }

    void Update()
    {
        if (!UIManager.instance.isInUI && escapeAction != null && escapeAction.WasPressedThisFrame())
        {
            ShowTaskMenu(true);
        }
    }

    public void UpdateButtons()
    {
        for (int i = 0; i < taskButtons.Length; i++)
        {
            taskButtons[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < taskButtons.Length && i < pointsSystem.availableTasks.Count; i++)
        {
            var textComponent = taskButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = pointsSystem.availableTasks[i];
            }
            int index = i;
            taskButtons[i].onClick.RemoveAllListeners();
            taskButtons[i].onClick.AddListener(() => OnTaskButtonClicked(index));


            var imageComponent = taskButtons[i].GetComponent<Image>();
            if (imageComponent != null)
            {
                if (pointsSystem.availableTasks[i] == selectedTask)
                {
                    imageComponent.color = Color.green;
                }
                else
                {
                    imageComponent.color = Color.white;
                }
            }
            taskButtons[i].interactable = !isReadOnly;

            if (pointsSystem.currentDay >= 0 && pointsSystem.currentDay < pointsSystem.stateHistory.Count)
            {
                for (int j = 0; j < pointsSystem.stateHistory[pointsSystem.currentDay].Day.Count; j++)
                {
                    PointsSystem.ActionStruct dayAction = pointsSystem.stateHistory[pointsSystem.currentDay].Day[j];
                    if (dayAction.TaskName == pointsSystem.availableTasks[i])
                    {
                        // taskButtons[i].gameObject.SetActive(false);
                        taskButtons[i].interactable = false;
                    }
                }
            }
        }

        for (int i = pointsSystem.availableTasks.Count; i < taskButtons.Length; i++)
        {
            taskButtons[i].gameObject.SetActive(false);
        }

        confirmButton.gameObject.SetActive(!isReadOnly);
        confirmButton.interactable = !isReadOnly;
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(OnConfirm);
    }

    void OnTaskButtonClicked(int index)
    {
        string task = pointsSystem.availableTasks[index];
        if (selectedTask == task)
        {
            selectedTask = "";
        }
        else
        {
            selectedTask = task;
        }
        UpdateButtons();
    }

    void OnConfirm()
    {
        if (!string.IsNullOrEmpty(selectedTask))
        {
            onTaskSelected?.Invoke();
        }
        else
        {
            // No task selected, return to game
            UIManager.instance.HideAll();
        }
    }

    public void ShowTaskMenu(bool readOnly = false)
    {
        UIManager.instance.ShowTaskMenu();
        selectedTask = "";
        isReadOnly = readOnly;
        if (isReadOnly)
        {
            objectTitle.text = "TASKS FOR TODAY: PRESS ESC TO GET THIS MENU";
        } else
        {
            objectTitle.text = "SELECT TASK";
        }
        UpdateButtons();
    }

}