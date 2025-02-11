using UnityEngine;

namespace Engine
{
    public class SceneLoaderCallback : MonoBehaviour
    {
        private bool isFirstUpdate = true;

        private void Update()
        {
            if (isFirstUpdate)
            {
                isFirstUpdate = false;
                SceneLoader.LoaderCallback();
            }
        }
    }
}
