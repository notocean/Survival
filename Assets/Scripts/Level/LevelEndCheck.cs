using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        Main_Infor main = collision.GetComponent<Main_Infor>();
        if (main != null) {
            NotifyManager.Instance.ShowNotify(NotifyManager.NotifyType.LevelEnd, true);
        }
    }
}
