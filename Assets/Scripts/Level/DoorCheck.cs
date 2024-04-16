using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCheck : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] GameObject[] enemy;
    
    public enum DoorType {
        Start, End
    }

    [SerializeField] DoorType doorType;
    private void OnTriggerEnter2D(Collider2D collision) {
        Main_Infor main = collision.GetComponent<Main_Infor>();
        if (main != null) {
            if (!GameManager.Instance.isPlaying) {
                if (doorType == DoorType.Start) {
                    GameManager.Instance.isPlaying = true;
                    foreach (GameObject obj in enemy) {
                        obj.SetActive(true);
                    }
                    NotifyManager.Instance.ShowNotify(NotifyManager.NotifyType.LevelStart, false);
                }
                door.GetComponent<Animator>().enabled = true;
                door.GetComponent<AudioSource>().Play();
                Destroy(gameObject);
            }
        }
    }
}
