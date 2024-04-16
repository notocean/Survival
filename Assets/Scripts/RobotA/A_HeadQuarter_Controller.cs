using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_HeadQuarter_Controller : MonoBehaviour
{
    private static A_HeadQuarter_Controller instance;
    public static A_HeadQuarter_Controller Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<A_HeadQuarter_Controller>();
                if (instance == null) {
                    GameObject obj = new GameObject("RobotA_Controller");
                    instance = obj.AddComponent<A_HeadQuarter_Controller>();
                }
            }
            return instance;
        }
    }

    float timeToLaunch = 6f;
    float timerLaunch;
    bool hasLauch;

    float timeToFall;
    float timerFall = 2;

    public List<Rocket_Controller> rocket = new List<Rocket_Controller>();
    void Start()
    {
        timerLaunch = 0;
        timerFall = 0;
    }

    // Update is called once per frame
    void Update()
    {
        hasLauch = false;
        timerLaunch += Time.deltaTime;
        if (timerLaunch > timeToLaunch) {
            hasLauch = true;
            timerLaunch = 0;
        }

        if (rocket.Count > 0) {
            timerFall += Time.deltaTime;
            if (timerFall > timeToFall) {
                Fall();
            }
        }
    }

    public bool HasLauch() {
        return hasLauch;
    }

    public void AddRocket(Rocket_Controller tmp) {
        rocket.Add(tmp);
    }

    void Fall() {
        GameObject main = GameObject.FindGameObjectWithTag("Player");
        if (main != null) {
            Vector2 mainPos = main.transform.position;
            for (int i = 0; i < rocket.Count; i++) {
                int y;
                if (i <= 3)
                    y = 3;
                else if (i <= 6)
                    y = 6;
                else if (i <= 9)
                    y = 9;
                else y = 12;
                Vector3 vec3 = new Vector3(0, y, 0);
                vec3 = Quaternion.Euler(0, 0, Random.Range(0, 361)) * vec3;
                Vector2 vec2 = vec3;
                Vector2 tmp = mainPos + vec2;
                rocket[i].Fall(tmp);
            }
        }
        else {
            for (int i = 0; i < rocket.Count; i++) {
                rocket[i].Fall(new Vector2(0, 0));
            }
        }
        rocket.Clear();
    }
}
