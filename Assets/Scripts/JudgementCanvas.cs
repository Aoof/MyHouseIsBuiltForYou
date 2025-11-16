using TMPro;
using UnityEngine;

public class JudgementCanvas : MonoBehaviour
{
    public PointsSystem pointsSystem;
    public UIManager uIManager;

    public TextMeshProUGUI partnerText;

    void Awake()
    {
        if (pointsSystem == null) pointsSystem = FindFirstObjectByType<PointsSystem>();
        if (uIManager == null) uIManager = FindFirstObjectByType<UIManager>();
    }

    public void StartJudgement()
    {
        //partnerText.text = 
        uIManager.ShowJudgement();
    }

    public void StartNextDay()
    {
        pointsSystem.StartNewDay();
    }
}
