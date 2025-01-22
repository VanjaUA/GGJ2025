using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Engine 
{
    public class MainMenuManager : MonoBehaviour
    {
        public static MainMenuManager Instance { get; private set; }

        [SerializeField] private GameObject buttonsPanel;
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button continueGameButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitGameButton;
        [SerializeField] private GameObject settingPanel;
        [SerializeField] private Button backToMenuButton;

        private void Awake()
        {
            SingletonInit();

            startGameButton.onClick.AddListener(StartNewGame);
            continueGameButton.onClick.AddListener(ContinueGame);
            settingsButton.onClick.AddListener(OpenSettings);
            exitGameButton.onClick.AddListener(ExitGame);
            backToMenuButton.onClick.AddListener(CloseSettings);

            SettingsPanelSetActive(false);
        }

        private void SingletonInit()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        private void StartNewGame() 
        {
            SceneLoader.Load(SceneLoader.Scene.TestScene);
        }

        private void ContinueGame()
        {

        }

        private void OpenSettings()
        {
            SettingsPanelSetActive(true);
        }

        private void ExitGame()
        {
            Application.Quit();
        }

        private void CloseSettings() 
        {
            SettingsPanelSetActive(false);
        }

        private void SettingsPanelSetActive(bool active) 
        {
            buttonsPanel.gameObject.SetActive(!active);
            settingPanel.gameObject.SetActive(active);
        }
    }
} 
