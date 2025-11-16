using TMPro;
using UnityEngine;
using System.Collections;

public class JudgementCanvas : MonoBehaviour
{
    public PointsSystem pointsSystem;

    public TextMeshProUGUI partnerText;

    public float judgementDuration = 5f;

    void Awake()
    {
        if (pointsSystem == null) pointsSystem = FindFirstObjectByType<PointsSystem>();
    }

    public void StartJudgement()
    {
        UIManager.instance.ShowJudgement();
        partnerText.text = pointsSystem.GetJudgementText();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(WaitAndNextDay());
    }

    private IEnumerator WaitAndNextDay()
    {
        yield return new WaitForSeconds(judgementDuration);
        StartNextDay();
    }

    public void StartNextDay()
    {
        UIManager.instance.HideAll();
        pointsSystem.StartNewDay();
    }
}
