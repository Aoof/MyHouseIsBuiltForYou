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
    public int tasksQuantity = 3;
    public int currentActions = 0;

    [Header("Tasks")]
    public List<string> availableTasks = new() { "Clean the apartment", "Fix the laptop", "Organize the fridge", "Change the boot", "Destroy condoms" };

    public GameObject[] interactables;

    void Awake()
    {
        interactables = GameObject.FindGameObjectsWithTag("Interactable");
    }

    void Start()
    {
        stateHistory.Add(new DayHistory { Day = new List<ActionStruct>() });
        currentDay = 0;
        currentActions = 0;
    }

    public void ActionPerformed()
    {
        currentActions++;
        if (currentActions >= tasksQuantity)
        {
            // End day, start new day
            stateHistory.Add(new DayHistory { Day = new List<ActionStruct>() });
            currentDay++;
            currentActions = 0;
        }
    }
}
