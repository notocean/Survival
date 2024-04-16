using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Movement : MonoBehaviour
{
    float speed;
    float horBody, verBody;
    Main_Infor main_Infor;

    Rigidbody2D rb2d;
    Animator animBody;

    bool isRotate = false;

    enum dirBC {
        basicDir, cornerDir
    }
    dirBC dirBCBody;

    private void Awake() {
        animBody = GetComponent<Animator>();
        main_Infor = GetComponent<Main_Infor>();

        rb2d = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start() {
        horBody = 0;
        verBody = -1;

        dirBCBody = dirBC.basicDir;
        speed = GetComponent<Main_Infor>().infor.speed;
    }

    private void Update() {
        if (GameManager.Instance.gameState == GameManager.GameState.Pause)
            return;

        if (main_Infor.infor.isDestroy) {
            animBody.SetFloat("horBody", 0);
            animBody.SetFloat("verBody", 0);
            return;
        }

        horBody = Input.GetAxisRaw("HorizontalCharacter");
        verBody = Input.GetAxisRaw("VerticalCharacter");

        if (Mathf.Approximately(horBody, 0) && Mathf.Approximately(verBody, 0)) {
            rb2d.velocity = new Vector2(0, 0);
        }

        Vector2 tmp = new Vector2(horBody, verBody);
        if (tmp.magnitude > 0.6f) {
            AnimationClip clip = animBody.GetCurrentAnimatorClipInfo(0)[0].clip;
            if (Mathf.Approximately(tmp.x, 0) || Mathf.Approximately(tmp.y, 0)) {
                if (!clip.name.Contains("Walk_Rotate")) {
                    if (dirBCBody == dirBC.cornerDir) {
                        animBody.SetTrigger("basicMove");
                    }
                    dirBCBody = dirBC.basicDir;
                }
            }
            else {
                if (!clip.name.Contains("Walk_Rotate")) {
                    if (dirBCBody == dirBC.basicDir) {
                        animBody.SetTrigger("cornerMove");
                    }
                    dirBCBody = dirBC.cornerDir;
                }
            }

            if (clip.name.Contains("Walk_Rotate")) {
                isRotate = true;
            }
            else isRotate = false;
        }
        else {
            isRotate = true;
        }

        animBody.SetFloat("horBody", horBody);
        animBody.SetFloat("verBody", verBody);

    }
    // Update is called once per frame
    void FixedUpdate() {
        if (GameManager.Instance.gameState == GameManager.GameState.Pause)
            return;

        if (!isRotate && !main_Infor.infor.isDestroy) {
            Vector2 pos = rb2d.position;
            Vector2 dirBody = new Vector2(horBody, verBody);
            dirBody.Normalize();

            pos.x += speed * dirBody.x * Time.deltaTime;
            pos.y += speed * dirBody.y * Time.deltaTime;
            rb2d.MovePosition(pos);
        }
    }

}
