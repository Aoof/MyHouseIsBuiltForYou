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

    public struct DayHistory // List of Days
    {
        public List<ActionStruct> Day; // List of Actions
    }

    public List<DayHistory> stateHistory = new();

    [Header("Game State")]
    public int currentDay = 0;
    public int tasksQuantity => availableTasks.Count;
    public int currentActions = 0;

    [Header("Tasks")]
    [SerializeField] private DayTasks[] dayTasks;
    public List<string> availableTasks => GetAvailableTasks();

    [Header("Judgement")]
    [SerializeField] private JudgementResult[] judgementResults;

    private List<string> GetAvailableTasks()
    {
        if (dayTasks == null || currentDay >= dayTasks.Length || dayTasks[currentDay] == null)
        {
            return new List<string>();
        }
        return dayTasks[currentDay].tasks;
    }

    public GameObject[] interactables;

    public JudgementCanvas judgementCanvas;

    public Dictionary<string, bool> dailyTaskResults;
    public int dayScore;
    public int totalScore;

    void Awake()
    {
        interactables = GameObject.FindGameObjectsWithTag("Interactable");
        if (judgementCanvas == null) judgementCanvas = FindFirstObjectByType<JudgementCanvas>();
    }

    void Start()
    {
        stateHistory.Add(new DayHistory { Day = new List<ActionStruct>() });
        currentDay = 0;
        currentActions = 0;
        UIManager.instance.StartDaySequence(currentDay);
    }

    public void StartNewDay()
    {
        stateHistory.Add(new DayHistory { Day = new List<ActionStruct>() });
        currentDay++;
        currentActions = 0;
        UIManager.instance.StartDaySequence(currentDay);
    }

    public void ActionPerformed()
    {
        currentActions++;
        if (currentActions >= tasksQuantity)
        {
            judgementCanvas.StartJudgement();
        }
    }

    public void CheckResults()
    {
        foreach (string taskName in availableTasks)
        {
            foreach (ActionStruct action in stateHistory[currentDay].Day)
            {
                if (action.TaskName == taskName)
                {
                    
                }
            }
        }
    }

    public string GetJudgementText()
    {
        if (judgementResults == null || judgementResults.Length == 0) return "No judgement available.";

        foreach (JudgementResult result in judgementResults)
        {
            if (IsJudgementSatisfied(result))
            {
                return result.partnerText;
            }
        }
        return "Default judgement text."; // Or something
    }

    private bool IsJudgementSatisfied(JudgementResult result)
    {
        // Check if all necessary actions are in the day's history
        var actions = new (string obj, string task, ObjectState state)[]
        {
            (result.objectName_1, result.taskName_1, result.objectState_1),
            (result.objectName_2, result.taskName_2, result.objectState_2),
            (result.objectName_3, result.taskName_3, result.objectState_3),
            (result.objectName_4, result.taskName_4, result.objectState_4),
            (result.objectName_5, result.taskName_5, result.objectState_5)
        };

        foreach (var (obj, task, state) in actions)
        {
            if (!string.IsNullOrEmpty(obj))
            {
                bool found = false;
                foreach (ActionStruct action in stateHistory[currentDay].Day)
                {
                    if (action.ObjectName == obj && action.TaskName == task && action.ObjectState == state)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found) return false;
            }
        }
        return true;
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
