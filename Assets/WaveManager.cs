using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class WaveManager : MonoBehaviour
    {
        private GameState gameState;

        private void Awake()
        {
            gameState = GameState.Waiting;
        }

        private void Update()
        {
            switch (gameState)
            {
                case GameState.Waiting:
                    break;
                case GameState.Play:
                    break;
            }
        }
        enum GameState
        {
            Waiting,
            Play,
        }
    }

}