using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskMenu : MonoBehaviour
{
    [Header("References")]
    public PointsSystem pointsSystem;
    [Header("UI Elements")]
    [SerializeField] private Button[] taskButtons;
    [SerializeField] private Button confirmButton;

    [NonSerialized]
    public string selectedTask = "";
    public Action onTaskSelected;

    void Start()
    {
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
        // Setup task buttons
        for (int i = 0; i < taskButtons.Length && i < pointsSystem.availableTasks.Count; i++)
        {
            int index = i;
            var textComponent = taskButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = pointsSystem.availableTasks[i];
            }
            taskButtons[i].onClick.AddListener(() => OnTaskButtonClicked(index));
        }
        confirmButton.GetComponentInChildren<TextMeshProUGUI>().text = "Confirm";
        confirmButton.onClick.AddListener(OnConfirm);
        UpdateButtons();
    }

    void OnEnable()
    {
        selectedTask = "";
        UpdateButtons();
    }

    void UpdateButtons()
    {
        for (int i = 0; i < taskButtons.Length && i < pointsSystem.availableTasks.Count; i++)
        {
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
        }
    }

    void OnTaskButtonClicked(int index)
    {
        Debug.Log("Clicked on Task Button " + index);
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
        Debug.Log("Clicked on Confirm");
        if (!string.IsNullOrEmpty(selectedTask))
        {
            onTaskSelected?.Invoke();
        }
    }
}