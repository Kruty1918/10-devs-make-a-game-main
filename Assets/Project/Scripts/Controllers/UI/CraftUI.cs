using Bonjoura.Craft;
using System.Collections.Generic;
using UnityEngine;

namespace Bonjoura.UI.Craft
{
    public class CraftUI : MonoBehaviour
    {
        [SerializeField] private List<CraftOption> craftList;
        [SerializeField] private Transform craftListTransform;
        [SerializeField] private CraftOptionUI craftOptionPrefab;

        private void OnEnable()
        {
            foreach (var craft in craftList)
            {
                var craftOption = Instantiate(craftOptionPrefab, craftListTransform);
                craftOption.InitializateOption(craft);
            }
        }
    }
}