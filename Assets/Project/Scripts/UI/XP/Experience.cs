using TMPro;
using UnityEngine;

public class Experience : MonoBehaviour
{
    public int experience { get; private set; }
    private int textValue;

    [SerializeField] private float _textAnimSpeed;

    [SerializeField] TMP_Text XP_text;

    private void Start()
    {
        experience = 0;
    }
    public void AddXP(int value)
    {
        experience += value;
    }
    public void RemoveXP(int value)
    {
        experience -= value;
    }
    private void Update()
    {
        if (textValue < experience)
        {
            textValue += (int)_textAnimSpeed;
        }
        else if (textValue > experience)
        {
            textValue -= (int)_textAnimSpeed;
        }
        XP_text.text = textValue.ToString();
    }
}
