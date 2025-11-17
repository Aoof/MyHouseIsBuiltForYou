using System;
using System.Collections.Generic;
using UnityEngine;

public class PointsSystem : MonoBehaviour
{
    public enum ObjectState
    {
        Unchanged,
        Destroyed,
        Changed
    }

    public struct ActionStruct
    {
        public string ObjectName;
        public string TaskName;
        public ObjectState ObjectState;
    }

    [System.Serializable]
    public struct CorrectAction
    {
        public string taskName;
        public string objectName;
        public ObjectState objectState;
    }

    public struct DayHistory // List of Days
    {
        public List<ActionStruct> Day; // List of Actions
    }

    public List<DayHistory> stateHistory = new();

    [Header("Game State")]
    public int currentDay = 0;
    public int currentActions = 0;

    [Header("Tasks")]
    [SerializeField] private DayTasks[] dayTasks;
    public List<string> availableTasks => GetAvailableTasks();

    private List<string> GetAvailableTasks()
    {
        if (dayTasks == null || currentDay >= dayTasks.Length || dayTasks[currentDay] == null)
        {
            return new List<string>();
        }
        return dayTasks[currentDay].tasks;
    }

    public GameObject[] interactables;

    public JudgementManager judgementManager;


    void Awake()
    {
        interactables = GameObject.FindGameObjectsWithTag("Interactable");
        if (judgementManager == null) judgementManager = FindFirstObjectByType<JudgementManager>();
    }

    void Start()
    {
        stateHistory.Add(new DayHistory { Day = new List<ActionStruct>() });
        currentDay = 0;
        currentActions = 0;
        UIManager.instance.StartDaySequence(currentDay);
        ResetInteractables();
        FindFirstObjectByType<TaskMenu>().UpdateButtons();
    }

    public void StartNewDay()
    {
        stateHistory.Add(new DayHistory { Day = new List<ActionStruct>() });
        currentDay++;
        currentActions = 0;
        UIManager.instance.StartDaySequence(currentDay);
        ResetInteractables();
        FindFirstObjectByType<TaskMenu>().UpdateButtons();
    }

    public void ActionPerformed()
    {
        currentActions++;
        if (currentActions >= dayTasks[currentDay].tasks.Count)
        {
            judgementManager.StartJudgement();
        }
    }
    
    public string GetJudgementText()
    {
        if (dayTasks == null || currentDay >= dayTasks.Length || dayTasks[currentDay] == null)
        {
            return "No tasks available.";
        }
        var correctActions = dayTasks[currentDay].correctActions;
        int count = 0;
        foreach (var correct in correctActions)
        {
            bool found = false;
            foreach (ActionStruct action in stateHistory[currentDay].Day)
            {
                if (action.ObjectName == correct.objectName && action.TaskName == correct.taskName && action.ObjectState == correct.objectState)
                {
                    found = true;
                    break;
                }
            }
            if (found) count++;
        }
        int total = correctActions.Count;
        string message = $"{count} out of {total} tasks done correctly.\n";
        if (count <= 1)
        {
            message += "You disappoint me, darling.";
        }
        else if (count == 2)
        {
            message += "Well… You can do better, right?";
        }
        else
        {
            message += "Thank you. I love you.";
        }
        return message;
    }

    public void ResetInteractables()
    {
        InteractableObject[] interactables = FindObjectsByType<InteractableObject>(FindObjectsSortMode.None);
        foreach (InteractableObject io in interactables)
        {
            io.ResetState();
        }
    }
}
