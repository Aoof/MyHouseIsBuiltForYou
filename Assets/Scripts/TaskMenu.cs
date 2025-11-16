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

    float maxButtonWidth;

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
        maxButtonWidth = taskButtons[0].GetComponent<RectTransform>().sizeDelta.x;
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
            ResizeButton(taskButtons[i].gameObject);
        }
        // Disable unused task buttons
        for (int i = pointsSystem.availableTasks.Count; i < taskButtons.Length; i++)
        {
            taskButtons[i].gameObject.SetActive(false);
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

    private void ResizeButton(GameObject button)
    {
        var textComp = button.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (textComp != null)
        {
            var textRect = textComp.GetComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(maxButtonWidth, textRect.sizeDelta.y);
            textComp.textWrappingMode = TMPro.TextWrappingModes.Normal;
            textComp.ForceMeshUpdate();
            var buttonRect = button.GetComponent<RectTransform>();
            buttonRect.sizeDelta = new Vector2(buttonRect.sizeDelta.x, textComp.preferredHeight);
        }
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
        else
        {
            // No task selected, return to game
            UIManager.instance.HideAll();
        }
    }
}