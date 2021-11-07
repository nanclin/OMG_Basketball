using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusProjectController : MonoBehaviour {

    // references
    public float MoveSpeed = 10;
    public GameObject BallPrefab;
    public Transform PosDribble;
    public Transform PosOverHead;
    public Transform Arms;
    public Transform Target;


    // variables
    private Transform Ball;
    private bool IsBallInHands = false;
    private bool IsBallFlying = false;
    private float T = 0;
    private int TrashCount = 0;

    void Start() {
        SpawnTrash();
        SpawnTrash();
        SpawnTrash();
    }

    // Update is called once per frame
    void Update() {

        // spawn trash
        if (TrashCount < 50 && Random.value < 0.0075f) {
            SpawnTrash();
        }

        // walking
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.position += direction * MoveSpeed * Time.deltaTime;
        transform.LookAt(transform.position + direction);

        // ball in hands
        if (IsBallInHands) {

            // hold over head
            if (Input.GetKey(KeyCode.Space)) {
                Ball.position = PosOverHead.position;
                Arms.localEulerAngles = Vector3.right * 180;

                // look towards the target
                transform.LookAt(Target.parent.position);
            }

            // dribbling
            else {
                Ball.position = PosDribble.position;
                Arms.localEulerAngles = Vector3.right * -90;
            }

            // throw ball
            if (Input.GetKeyUp(KeyCode.Space)) {
                IsBallInHands = false;
                IsBallFlying = true;
                T = 0;
            }
        }
        else {
            Arms.localEulerAngles = Vector3.right * 0;
        }

        // ball in the air
        if (IsBallFlying) {
            T += Time.deltaTime;
            float duration = 0.66f;
            float t01 = T / duration;

            // move to target
            Vector3 A = PosOverHead.position;
            Vector3 B = Target.position;
            Vector3 pos = Vector3.Lerp(A, B, t01);

            // move in arc
            Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * 3.14f);

            Ball.position = pos + arc;

            // moment when ball arrives at the target
            if (t01 >= 1) {
                IsBallFlying = false;
                Ball.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    private void SpawnTrash() {
        TrashCount++;
        GameObject ball = Instantiate(BallPrefab);
        ball.transform.position = new Vector3(Random.Range(-9, 9), 15, Random.Range(-9, 9));
        ball.transform.rotation = Random.rotation;
    }

    private void OnTriggerEnter(Collider other) {

        if (!IsBallInHands && !IsBallFlying) {

            IsBallInHands = true;
            Ball = other.transform;
            Ball.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
