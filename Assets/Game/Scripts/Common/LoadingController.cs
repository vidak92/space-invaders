using SpaceInvaders.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceInvaders.Common
{
    public class LoadingController : MonoBehaviour
    {
        // Unity Events
        private void Start()
        {
            SceneManager.LoadScene(SceneBuildIndices.GameScene);
        }
    }
}