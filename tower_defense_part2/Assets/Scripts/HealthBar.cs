using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider _slider;
    public Gradient gradient;
    private Image _fill;

    public void Start()
    {
        _slider = GetComponent<Slider>();
        _fill = transform.GetChild(0).GetComponent<Image>();
    }

    public void SetHealth(int health)
    {
        _slider.value = health;
        _fill.color = gradient.Evaluate(_slider.normalizedValue);
    }

    /*public void SetMaxHealth(int health)
    {
        _slider.maxValue = health;
        _slider.value = health;
        _fill.color = gradient.Evaluate(1f);
    }*/
}
 