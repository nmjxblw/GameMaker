using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace GameMaker
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField]
        private float _normalTimeScale = 1f;
        public float NormalTimeScale => _normalTimeScale;
        public UnityEvent OnGamePaused;
        public UnityEvent OnGameResumed;
        [SerializeField]
        private bool _gamePaused = false;
        public bool GamePaused
        {
            get
            {
                return _gamePaused;
            }
            set
            {
                if (_gamePaused != value)
                {
                    _gamePaused = value;
                    if (_gamePaused)
                        OnGamePaused?.Invoke();
                    else
                        OnGameResumed?.Invoke();
                }
            }
        }
        protected virtual void OnEnable()
        {
            OnGamePaused.AddListener(() => SetTimeScale(0f));
            OnGamePaused.AddListener(() => SetCursorLockState(false));
            OnGamePaused.AddListener(() => SetCursorVisible(true));
            OnGameResumed.AddListener(() => SetTimeScale(NormalTimeScale));
            OnGameResumed.AddListener(() => SetCursorLockState(true));
            OnGameResumed.AddListener(() => SetCursorVisible(false));
        }
        protected virtual void OnDisable()
        {
            OnGamePaused.RemoveAllListeners();
            OnGameResumed.RemoveAllListeners();
        }
        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                GamePaused = !GamePaused;
        }
        public static void SetTimeScale(float timeScale) => Time.timeScale = timeScale;
        public static void SetCursorLockState(bool value) => Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
        public static void SetCursorVisible(bool value) => Cursor.visible = value;
        public static void Quit() => Application.Quit();
    }
}