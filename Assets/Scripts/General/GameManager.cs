using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<GameManager>();
                if (instance == null) {
                    GameObject obj = new GameObject("Game Manager");
                    instance = obj.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    public enum SceneState {
        StartScene, PlayScene
    }

    public enum GameState {
        Start, Play, Pause
    }

    public SceneState sceneState { get; set; }
    int levelIndex;
    public bool isPlaying { get; set; }
    public GameState gameState { get; set; }

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        sceneState = SceneState.StartScene;

        if (instance != null)
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        isPlaying = false;
        gameState = GameState.Start;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.Pause) {
            Time.timeScale = 0;
        }
        else {
            Time.timeScale = 1;
        }

        if (isPlaying) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                NotifyManager.Instance.ShowNotify(NotifyManager.NotifyType.Pause, true);
                isPlaying = false;
            }
        }
    }

    public bool SetNextLevel() {
        return ++levelIndex <= 2;
    }

    public void SetLevelIndex(int index) {
        levelIndex = index;
    }

    public void ChangeScene(SceneState scene) {
        sceneState = scene;
        if (sceneState == SceneState.PlayScene) {
            LevelManager.Instance.setLevelInfor(levelIndex);
            NotifyManager.Instance.HideNotify(GameState.Play);
            SceneManager.LoadScene(levelIndex + 1);
        }
        else {
            SceneManager.LoadScene(0);
            isPlaying = false;
            NotifyManager.Instance.HideNotify(GameState.Start);
        } 
    }
}
