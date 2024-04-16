using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Controller : MonoBehaviour
{
    Robot infor;
    Rigidbody2D rb2d;
    Animator animBody, animCannon;
    Vector2 enemyPos;
    Vector2 pos;
    Vector2 moveDir;
    float speed;
    float hor, ver;
    float distanceSafe = 10f;
    [SerializeField] GameObject brokenPrefab;
    [SerializeField] GameObject brokenEffectPrefab;
    [SerializeField] GameObject explosionPrefab;
    UIBar healthBar;
    enum State {
        move, safe, load, launch, beforeExplosion
    }
    State currState;
    // Start is called before the first frame update
    private void Awake() {
        healthBar = GetComponentInChildren<UIBar>();
        infor = GetComponent<Robot>();
        infor.Init(LevelManager.Instance.maxHealthA, LevelManager.Instance.speedA, LevelManager.Instance.damageA, healthBar);

        rb2d = GetComponent<Rigidbody2D>();
        animBody = GetComponent<Animator>();
        animCannon = transform.GetChild(0).gameObject.GetComponent<Animator>();
    }
    void Start()
    {
        speed = infor.speed;
        hor = 0; ver = 0;
        moveDir = new Vector2(0, 0);
        currState = State.move;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Pause)
            return;

        GameObject enemyObject = GameObject.FindGameObjectWithTag("Player");
        if (enemyObject != null && (int)currState <= 0) {
            if (Vector2.Distance(enemyObject.transform.position, transform.position) < distanceSafe) {
                enemyPos = enemyObject.transform.position;
                pos = gameObject.transform.position;
                moveDir = pos - enemyPos;
                moveDir.Normalize();
                currState = State.move;
            }
            else currState = State.safe;
        }

        if (A_HeadQuarter_Controller.Instance.HasLauch()) {
            currState = State.load;
        }

        if (infor.isDestroy) {
            currState = State.beforeExplosion;
            if (!animBody.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("A_Explosion")) {
                animBody.SetTrigger("a_Explosion");
            }
        }

        switch (currState) {
            case State.move:
                hor = moveDir.x;
                ver = moveDir.y;
                if (Mathf.Abs(moveDir.x) > 0 || Mathf.Abs(moveDir.y) > 0) {
                    hor = moveDir.x;
                    ver = moveDir.y;
                }
                animBody.SetFloat("horBody", hor);
                animBody.SetFloat("verBody", ver);
                break;
            case State.load:
                animCannon.SetTrigger("a_Load");
                currState = State.launch;
                break;
            case State.launch:
                if (animCannon.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                    currState = State.move;
                break;
            default:
                break;
        }
    }
    private void FixedUpdate() {
        if (GameManager.Instance.gameState == GameManager.GameState.Pause)
            return;

        if (currState != State.move)
            return;
        Vector2 pos = rb2d.position;
        Vector2 dirBody = new Vector2(hor, ver);
        dirBody.Normalize();

        pos.x += speed * dirBody.x * Time.deltaTime;
        pos.y += speed * dirBody.y * Time.deltaTime;
        rb2d.MovePosition(pos);
    }

    public void Explosion() {
        if (LevelManager.Instance.HasGenerateScrapA()) {
            Instantiate(brokenEffectPrefab, transform.position, Quaternion.identity);
            Instantiate(brokenPrefab, transform.position, Quaternion.identity);
        }
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        LevelManager.Instance.DestroyA();
        Destroy(gameObject);
    }
}
