using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotifyManager : MonoBehaviour
{
    private static NotifyManager instance;
    public static NotifyManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<NotifyManager>();
                if (instance == null) {
                    GameObject obj = new("Notify Canvas");
                    instance = obj.AddComponent<NotifyManager>();
                }
            }
            return instance;
        }
    }

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        if (instance != null)
            Destroy(gameObject);
    }

    public enum NotifyType {
        LevelComplete, LevelStart, LevelEnd, LevelSelect, Pause, Guide
    }

    [SerializeField] GameObject levelComplete;
    [SerializeField] GameObject levelStart;
    [SerializeField] GameObject levelEnd;
    [SerializeField] GameObject levelSelector;
    [SerializeField] GameObject Star1;
    [SerializeField] GameObject Star2;
    [SerializeField] GameObject Star3;
    [SerializeField] GameObject loseImage;
    [SerializeField] GameObject winButton;
    [SerializeField] GameObject loseButton;
    [SerializeField] GameObject pauseNotify;

    GameObject notifyObject;

    public void ShowNotify(NotifyType type, bool pause) {
        if (pause)
            GameManager.Instance.gameState = GameManager.GameState.Pause;

        switch (type) {
            case NotifyType.LevelComplete:
                notifyObject = Instantiate(levelComplete, transform);
                break;
            case NotifyType.LevelStart:
                notifyObject = Instantiate(levelStart, transform);
                break;
            case NotifyType.LevelEnd:
                notifyObject = Instantiate(levelEnd, transform);
                int cStar = LevelManager.Instance.GetCountStar();
                if (cStar > 0)
                    Instantiate(winButton, notifyObject.transform);
                else Instantiate(loseButton, notifyObject.transform);
                if (cStar == 1) {
                    Instantiate(Star1, notifyObject.transform);
                }
                else if (cStar == 2) {
                    Instantiate(Star2, notifyObject.transform);
                }
                else if (cStar == 3) {
                    Instantiate(Star3, notifyObject.transform);
                }
                else {
                    Instantiate(loseImage, notifyObject.transform);
                    notifyObject.transform.Find("Notify").GetComponent<TextMeshProUGUI>().SetText("Level Failed");
                }
                break;
            case NotifyType.LevelSelect:
                notifyObject = Instantiate(levelSelector, transform);
                break;
            case NotifyType.Pause:
                notifyObject = Instantiate(pauseNotify, transform);
                break;
        }
    }

    public void HideNotify(GameManager.GameState state) {
        GameManager.Instance.gameState = state;
        if (notifyObject != null) {
            notifyObject.GetComponent<Animator>().SetTrigger("Disable");
        }
    }
}
