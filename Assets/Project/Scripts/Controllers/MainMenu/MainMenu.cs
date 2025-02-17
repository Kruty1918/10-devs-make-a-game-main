using System.Collections;
using System.Threading.Tasks;
using SGS29.Utilities;
using UnityEngine;

namespace Bonjoura.UI
{
    public class MainMenu : MonoSingleton<MainMenu>
    {
        [SerializeField] private Animator playAnimator;
        [SerializeField] private string animationPlayName = "GamePlay";
        [SerializeField] private string gameScene = "MainScene";


        public void Load()
        {
            playAnimator.Play(animationPlayName);
            StartCoroutine(LoadCoroutine());
        }

        public void Quit()
        {
            Application.Quit();
        }

        private IEnumerator LoadCoroutine()
        {
            yield return SceneLoader.LoadSceneAsync(gameScene, 1, 2);
        }
    }
}