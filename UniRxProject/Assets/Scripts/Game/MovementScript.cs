using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Operators;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MovementScript : MonoBehaviour
{
    public MovementDir moveDir { get; private set; }
    public UnityEvent UpInput, DownInput, RightInput, LeftInput;
    bool stopped = false;
    public static float mapWidth = 14.5f, mapHeight = 9.5f;
    public UnityEvent PointsEvent;
    public static UnityEvent StartGameEvent = new UnityEvent();
    public float baseMovementSpeed = 1f;
    public float movementSpeedMultiplier = 1f;
    private float movementSpeedIncrement = 0.05f;

    public enum MovementDir
    {
        up, down, left, right
    }

    void Start()
    {
        stopped = true;

        MenuController.LoseEvent.AddListener(() =>
        {
            ResetplayerObject();
        });
        StartGameEvent.AddListener(() => stopped = false);

        var input = this.gameObject.AddComponent<InputScript>();
        input.InputScriptInit(UpInput, DownInput, RightInput, LeftInput);

        UpInput.AsObservable().Where(_ => moveDir != MovementDir.down).Subscribe(_ =>
        {
            transform.LookAt(transform.position + Vector3.forward);
            moveDir = MovementDir.up;
        });
        DownInput.AsObservable().Where(_ => moveDir != MovementDir.up).Subscribe(_ =>
        {
            transform.LookAt(transform.position + -Vector3.forward);
            moveDir = MovementDir.down;
        });
        RightInput.AsObservable().Where(_ => moveDir != MovementDir.left).Subscribe(_ =>
        {
            transform.LookAt(transform.position + Vector3.right);
            moveDir = MovementDir.right;
        });
        LeftInput.AsObservable().Where(_ => moveDir != MovementDir.right).Subscribe(_ =>
        {
            transform.LookAt(transform.position + -Vector3.right);
            moveDir = MovementDir.left;
        });

        transform.UpdateAsObservable().Where(_ => stopped == false)
            .Subscribe(x => transform.position += transform.forward * Time.deltaTime * baseMovementSpeed * movementSpeedMultiplier);
        MenuController.LoseEvent.AddListener(() => movementSpeedMultiplier = 1);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Point"))
        {
            PointsEvent.Invoke();
            collision.gameObject.transform.position = new Vector3(Random.Range(-mapWidth, mapWidth), 0, Random.Range(-mapHeight, mapHeight));
            movementSpeedMultiplier += movementSpeedIncrement;

        }
        if (collision.gameObject.CompareTag("Tail"))
        {
            MenuController.LoseEvent.Invoke();
        }
    }
    void ResetplayerObject()
    {
        stopped = true;
        movementSpeedMultiplier = 1;
        transform.position = new Vector3(0, 0, 0);
        transform.LookAt(transform.position + Vector3.forward);
        moveDir = MovementDir.up;
    }


}
