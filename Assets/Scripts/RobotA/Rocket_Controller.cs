using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Rocket_Controller : MonoBehaviour
{
    [SerializeField] float maxDistanceMove = 250f;
    [SerializeField] float speedY;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] GameObject focusPrefab;
    Transform focus;
    Vector2 start;
    float distanceToAt;
    float maxSizeY = 2.2f;
    float maxSizeX = 1.8f;
    Vector2 atPosition;
    public enum State {
        launch, delay, fall
    }
    State currState;
    // Start is called before the first frame update
    private void Awake() {

    }
    void Start() {
        currState = State.launch;
        start = transform.position;
    }

    // Update is called once per frame
    void Update() {
        switch (currState) {
            case State.launch:
                if (transform.position.y - start.y < maxDistanceMove) {
                    Vector2 pos = transform.position;
                    pos.y += speedY * Time.deltaTime;
                    transform.position = pos;
                }
                else {
                    currState = State.delay;
                    A_HeadQuarter_Controller.Instance.AddRocket(this);
                }
                break;
            case State.delay:
                break;
            case State.fall:
                Vector2 tmp = transform.position;
                tmp.y -= speedY * Time.deltaTime;
                transform.position = tmp;
                Vector3 scale = new Vector3(maxSizeX * ((distanceToAt - (tmp.y - atPosition.y)) / distanceToAt), maxSizeY * ((distanceToAt - (tmp.y - atPosition.y)) / distanceToAt), 1);
                focus.localScale = scale;
                if (transform.position.y <= atPosition.y) {
                    Explosion_Controller explosion_Controller = Instantiate(explosionPrefab, transform.position, Quaternion.identity).GetComponent<Explosion_Controller>();
                    explosion_Controller.damage = LevelManager.Instance.damageA;
                    Destroy(focus.gameObject);
                    Destroy(gameObject);
                }
                break;
            default:
                break;
        }
    }

    public void Launch(Vector2 start) {
        this.start = start;
    }

    public void Fall(Vector2 at) {
        atPosition = at;
        distanceToAt = transform.position.y - atPosition.y;
        focus = Instantiate(focusPrefab, new Vector2(atPosition.x, atPosition.y), Quaternion.identity).transform;

        transform.Rotate(Vector3.forward, 180);
        Vector2 pos = new Vector2(at.x, transform.position.y);
        transform.position = pos;

        currState = State.fall;
    }
}
