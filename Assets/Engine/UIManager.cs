using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Engine
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [SerializeField] private Sprite[] faceSprites;
        [SerializeField] private Image faceImage;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI bulletsText;
        [SerializeField] private Image healthBar;

        private void Awake()
        {
            SingletonInit();
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

        #region API

        public void UpdateMoneyText(int moneyValue) 
        {
            moneyText.text = moneyValue.ToString();
        }

        public void UpdateBulletsText(int allBullets,int bulletsInClip) 
        {
            bulletsText.text = allBullets + "/" + bulletsInClip;
        }

        public void UpdateHealthBar(int currentHealth,int maxHealth)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
            UpdateFaceImage(currentHealth, maxHealth);
        }

        private void UpdateFaceImage(int healthValue, int maxHealth) 
        {
            int healthStep = maxHealth / faceSprites.Length;

            int faceIndex = 0;
            while (healthValue > healthStep)
            {
                healthValue -= healthStep;
                faceIndex++;
            }
            faceImage.sprite = faceSprites[faceIndex];
        }

        #endregion
    }
}
