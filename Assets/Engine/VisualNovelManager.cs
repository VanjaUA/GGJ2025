using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Engine
{
    public class VisualNovelManager : MonoBehaviour
    {
        public static VisualNovelManager Instance { get; private set; }

        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image characterImage;
        [SerializeField] private TextMeshProUGUI characterLabelText;
        [SerializeField] private TextMeshProUGUI dialogText;

        [SerializeField] private AudioSource soundSource;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioClip clickSound;

        [SerializeField] private List<VisualNovelFramesConfig> allFramesData;

        private int currentDayId;
        private int currentFrameId;

        private void Awake()
        {
            SingletonInit();

            currentFrameId = 0;
            SetFrame(allFramesData[0].Data.AllFrames[0]);
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

        private void Start()
        {
            InputManager.Instance.OnLeftMouseButtonPressed += OnLeftMouseButtonPressed;
            InputManager.Instance.OnSpaceButtonPressed += OnSpaceButtonPressed;
        }

        private void OnSpaceButtonPressed()
        {
            MoveToNextFrame();
        }

        private void OnLeftMouseButtonPressed()
        {
            MoveToNextFrame();
        }

        private void MoveToNextFrame() 
        {
            soundSource.PlayOneShot(clickSound);
            currentFrameId++;
            if (currentFrameId >= allFramesData[currentDayId].Data.AllFrames.Count)
            {
                Debug.LogError("Set monig to next scene");
                return;
            }
            SetFrame(allFramesData[currentDayId].Data.AllFrames[currentFrameId]);
        }

        private void SetFrame(VisualNovelFramesConfig.FrameData frameData) 
        {
            if (frameData.CharacterSprite != null)
            {
                characterImage.sprite = frameData.CharacterSprite;
            }
            if (frameData.BackgroundSprite != null)
            {
                backgroundImage.sprite = frameData.BackgroundSprite;
            }
            if (string.IsNullOrEmpty(frameData.LabelText) == false)
            {
                characterLabelText.text = frameData.LabelText;
            }
            if (string.IsNullOrEmpty(frameData.DialogText) == false)
            {
                dialogText.text = frameData.DialogText;
            }
            if (frameData.MusicClip != null)
            {
                musicSource.clip = frameData.MusicClip;
            }
            if (frameData.SoundClip != null)
            {
                soundSource.PlayOneShot(frameData.SoundClip);
            }
        }
    }
}
