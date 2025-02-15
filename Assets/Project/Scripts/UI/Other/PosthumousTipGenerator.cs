using System.Collections.Generic;
using UnityEngine;

public class PosthumousTipGenerator : MonoBehaviour
{
    private static PosthumousTipGenerator _instance;
    private Dictionary<string, List<string>> possibleTips;

    [TextArea(2, 10)] [SerializeField] private List<string> starveTips;
    [TextArea(2, 10)] [SerializeField] private List<string> hypothermiaTips;
    [TextArea(2, 10)] [SerializeField] private List<string> hyperthermiaTips;
    [TextArea(2, 10)] [SerializeField] private List<string> wolfTips;
    [TextArea(2, 10)] [SerializeField] private List<string> bobyTips;
    [TextArea(2, 10)] [SerializeField] private List<string> mimicTips;

    public static PosthumousTipGenerator Instance => _instance;
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }
        _instance = this;

        possibleTips = new Dictionary<string, List<string>>()
        {
            {"hunger", starveTips},
            {"cold", hypothermiaTips},
            {"heat", hyperthermiaTips},
            {"Wolf", wolfTips},
            {"Bolf", bobyTips},
            {"mimic", mimicTips},
        };
    }
    public string GenerateQuickTip(string reason)
    {
        if (possibleTips.TryGetValue(reason, out List<string> list))
        {
            string tip = list[Random.Range(0, list.Count)];
            return tip;
        }
        return "I dunno, pal, figure somethin' out yourself.";

    }
}
