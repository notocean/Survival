using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonManager : MonoBehaviour
{
    public enum ButtonType {
        Start, CloseLevelSelector, Setting, Exit, Restart, Home, Next, Continue
    }
    [SerializeField] ButtonType uiType;

    public void clicked() {
        switch (uiType) {
            case ButtonType.Start:
                NotifyManager.Instance.ShowNotify(NotifyManager.NotifyType.LevelSelect, false);
                break;
            case ButtonType.CloseLevelSelector:
                NotifyManager.Instance.HideNotify(GameManager.GameState.Start);
                break;
            case ButtonType.Setting:
                break;
            case ButtonType.Exit:
                Application.Quit();
                break;
            case ButtonType.Restart:
                GameManager.Instance.isPlaying = false;
                NotifyManager.Instance.HideNotify(GameManager.GameState.Play);
                GameManager.Instance.ChangeScene(GameManager.SceneState.PlayScene);
                break;
            case ButtonType.Home:
                GameManager.Instance.ChangeScene(GameManager.SceneState.StartScene);
                break;
            case ButtonType.Next:
                if (GameManager.Instance.SetNextLevel())
                    GameManager.Instance.ChangeScene(GameManager.SceneState.PlayScene);
                else GameManager.Instance.ChangeScene(GameManager.SceneState.StartScene);
                break;
            case ButtonType.Continue:
                NotifyManager.Instance.HideNotify(GameManager.GameState.Play);
                GameManager.Instance.isPlaying = true;
                break;
            default:
                break;
        }
    }
}
