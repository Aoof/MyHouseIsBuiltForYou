using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;

[CreateAssetMenu(fileName = "JudgementResult", menuName = "Scriptable Objects/JudgementResult")]
public class JudgementResult : ScriptableObject
{
    public string partnerText;

    [Header("Necessary Action 1")]
    public string objectName_1;
    public string taskName_1;
    public PointsSystem.ObjectState objectState_1;
    [Header("Necessary Action 2")]
    public string objectName_2;
    public string taskName_2;
    public PointsSystem.ObjectState objectState_2;
    [Header("Necessary Action 3")]
    public string objectName_3;
    public string taskName_3;
    public PointsSystem.ObjectState objectState_3;
    [Header("Necessary Action 4")]
    public string objectName_4;
    public string taskName_4;
    public PointsSystem.ObjectState objectState_4;
    [Header("Necessary Action 5")]
    public string objectName_5;
    public string taskName_5;
    public PointsSystem.ObjectState objectState_5;
}
