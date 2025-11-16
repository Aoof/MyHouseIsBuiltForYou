using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    //public Dictionary<int, Dictionary<string, ObjectState>> stateHistory = new();
    //public List<Dictionary<string, Tuple<string, ObjectState>>> stateHistory = new();
    public int currentDay = 0;
    public int tasksQuantity;
    public int tasksDone;

    public GameObject[] interactables;

    void Awake()
    {
        interactables = GameObject.FindGameObjectsWithTag("Interactable");
    }

    // Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
    void Start()
    {
        DayHistory dh = new();
        dh.Day = new List<ActionStruct>();

        stateHistory.Add(dh);
    }
}
