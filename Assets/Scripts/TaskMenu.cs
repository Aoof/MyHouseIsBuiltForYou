using System;
using TMPro;
using UnityEngine;
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

    float maxButtonWidth;

    [NonSerialized]
    public string selectedTask = "";
    public Action onTaskSelected;
    public bool isReadOnly = false;

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
        confirmButton.gameObject.SetActive(!isReadOnly);
        UpdateButtons();
    }

    void Awake()
    {
        if (playerInteractions == null) playerInteractions = FindFirstObjectByType<PlayerInteractions>();
    }

    void OnEnable()
    {
        selectedTask = "";
        UpdateButtons();
        
        // Update ObjectTitle
        objectTitle.text = playerInteractions.latestInteractable.gameObject.name;
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
            taskButtons[i].interactable = !isReadOnly;
        }
        confirmButton.interactable = !isReadOnly;
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
}