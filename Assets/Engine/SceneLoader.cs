using UnityEngine.SceneManagement;

namespace Engine
{
    public static class SceneLoader
    {
        public enum Scene
        {
            MainMenuScene,
            LoadingScene,
            TestScene
        }

        private static Scene targetScene;

        public static void Load(Scene sceneToLoad) 
        {
            targetScene = sceneToLoad;
            SceneManager.LoadScene(Scene.LoadingScene.ToString());
        }

        public static void LoaderCallback()
        {
            SceneManager.LoadScene(targetScene.ToString());
        }
    }
}
