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
        uIManager.ShowJudgement();
        partnerText.text = pointsSystem.GetJudgementText();
    }

    public void StartNextDay()
    {
        uIManager.HideAll();
        pointsSystem.StartNewDay();
    }
}
