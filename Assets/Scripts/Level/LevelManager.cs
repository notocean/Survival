using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    private static LevelManager instance;
    public static LevelManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<LevelManager>();
                if (instance == null) {
                    GameObject obj = new GameObject("Level Manager");
                    instance = obj.AddComponent<LevelManager>();
                }
            }
            return instance;
        }
    }

    int[] maxHealthCInLevel = {30, 45, 60};
    float[] speedCInLevel = { 1f, 1.5f, 2f };
    int[] damageCInLevel = { 30, 45, 60 };
    int[] maxScrapCInLevel = { 2, 5, 2 };
    int[] countCInLevel = { 5, 6, 4 };

    int[] maxHealthAInLevel = { 90, 120, 150 };
    float[] speedAInLevel = { 1f, 1.5f, 2f };
    int[] damageAInLevel = { 40, 60, 80 };
    int[] maxScrapAInLevel = { 4, 4, 2 };
    int[] countAInLevel = { 8, 10, 4 };

    float[] timeInLevel = { 20, 30, 40 };

    public int maxHealthC;
    public float speedC;
    public int damageC;
    int maxScrapC;
    int countC;
    int currScrapC;


    public int maxHealthA;
    public float speedA;
    public int damageA;
    int maxScrapA;
    int countA;
    int currScrapA;

    public int maxHealthM = 200;
    public float speedM = 3;
    public int damageM = 30;

    float timer;
    float maxTime;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        if (instance != null)
            Destroy(gameObject);
        timer = 0;
    }

    private void Update() {
        if (countA + countC == 0 && GameManager.Instance.isPlaying && GameManager.Instance.sceneState == GameManager.SceneState.PlayScene) {
            GameManager.Instance.isPlaying = false;
            NotifyManager.Instance.ShowNotify(NotifyManager.NotifyType.LevelComplete, false);
        }

        if (GameManager.Instance.isPlaying) {
            timer += Time.deltaTime;
        }
    }

    public void setLevelInfor(int levelIndex) {
        maxHealthC = maxHealthCInLevel[levelIndex];
        speedC = speedCInLevel[levelIndex];
        damageC = damageCInLevel[levelIndex];
        maxScrapC = maxScrapCInLevel[levelIndex];
        countC = countCInLevel[levelIndex];
        currScrapC = 0;

        maxHealthA = maxHealthAInLevel[levelIndex];
        speedA = speedAInLevel[levelIndex];
        damageA = damageAInLevel[levelIndex];
        maxScrapA = maxScrapAInLevel[levelIndex];
        countA = countAInLevel[levelIndex];
        currScrapA = 0;

        maxTime = timeInLevel[levelIndex];
    }

    public bool HasGenerateScrapC() {
        if (countC == 0)
            return false;
        int random = Random.Range(1, 101);
        if (random <= (maxScrapC - currScrapC) / countC * 100)
            return true;
        return false;
    }
    public void DestroyC() {
        countC--;
    }

    public bool HasGenerateScrapA() {
        if (countA == 0)
            return false;
        int random = Random.Range(1, 101);
        if (random <= (maxScrapA - currScrapA) / countA * 100)
            return true;
        return false;
    }
    public void DestroyA() {
        countA--;
    }
    public int CountCurrEnemy() {
        return countC + countA;
    }

    public int GetCountStar() {
        if (countA + countC != 0)
            return 0;
        float p = maxTime / timer;
        if (p < 0.33f)
            return 1;
        else if (p < 0.66f)
            return 2;
        else return 3;
    }
}
