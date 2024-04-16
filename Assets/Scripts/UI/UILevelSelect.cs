using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILevelSelect : MonoBehaviour
{
    [SerializeField] int levelIndex;

    public void SelectLevel() {
        NotifyManager.Instance.HideNotify(GameManager.GameState.Start);
        GameManager.Instance.SetLevelIndex(levelIndex);
        GameManager.Instance.ChangeScene(GameManager.SceneState.PlayScene);
    }
}
