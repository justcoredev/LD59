using TMPro;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public float currentValue = 31;
    public float maxValue = 50;

    public TMP_Text valueText;
    public GameObject bar;
    public SpriteRenderer barSpriteRenderer;

    float startBarScaleY;

    public string id;

    void Start()
    {
        startBarScaleY = bar.transform.localScale.y;
    }

    void Update()
    {
        currentValue = Mathf.Sin(Time.time) * Mathf.Sin(Time.time) * 50;
        UpdateValues();
    }

    void UpdateValues()
    {
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);

        valueText.text = currentValue.ToString("F1");

        float t = currentValue / maxValue;

        bar.transform.localScale = new Vector3(
            bar.transform.localScale.x, 
            Mathf.Lerp(0, startBarScaleY, t), 
            bar.transform.localScale.z
        );
    }

    public static Sensor FindByID(string id)
    {
        Sensor[] sensors = FindObjectsByType<Sensor>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var s in sensors)
            if (s.id == id)
                return s;
        return null;
    }
}