using TMPro;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public float currentValue = 31;
    public float maxValue = 50;
    [HideInInspector]public float desiredValue;

    public TMP_Text valueText;
    public GameObject bar;
    public SpriteRenderer barSpriteRenderer;

    float startBarScaleY;
    string startText;

    public string id;

    public bool activated;

    void Start()
    {
        startBarScaleY = bar.transform.localScale.y;
        startText = valueText.text; // T, P, etc
    }

    void Update()
    {
        if (activated)
        {
            currentValue = Mathf.Lerp(currentValue, desiredValue, Time.deltaTime * 5.0f);
            valueText.text = currentValue < 0.01 ? "" : currentValue.ToString("F1");
        }
        else
        {
            currentValue = 0;
            valueText.text = "";
        }

        float t = currentValue / maxValue;
        bar.transform.localScale = new Vector3(
            bar.transform.localScale.x, 
            Mathf.Lerp(0, startBarScaleY, t), 
            bar.transform.localScale.z
        );
        
        UpdateValues();
    }

    void UpdateValues()
    {
        
    }

    public void Activate()
    {
        activated = true;
    }

    public void SetDesiredValue(float value)
    {
        desiredValue = value;
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