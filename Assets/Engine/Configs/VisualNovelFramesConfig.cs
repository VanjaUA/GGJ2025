using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Engine
{
    [CreateAssetMenu(fileName = "FramesConfig", menuName = "ScriptableObjects/VisualNovelFramesConfig")]
    public class VisualNovelFramesConfig : ScriptableObject
    {
        public AllFramesData Data;

        [Serializable]
        public class AllFramesData
        {
            public List<FrameData> AllFrames = new List<FrameData>();
        }

        [Serializable]
        public class FrameData
        {
            public Sprite CharacterSprite;
            public Sprite BackgroundSprite;
            public string LabelText;
            public string DialogText;
            public AudioClip SoundClip;
            public AudioClip MusicClip;
        }
    }


}
