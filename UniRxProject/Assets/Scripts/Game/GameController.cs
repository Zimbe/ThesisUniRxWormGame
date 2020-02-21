using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Operators;
using UniRx.Triggers;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject playerObjectPrefab;
    [SerializeField] GameObject PointObjectPrefab;

    public static UnityEvent PointsEvent = new UnityEvent();


    List<GameObject> TailObjects = new List<GameObject>();
    CompositeDisposable disposables;

    private float PlayerFollowDistance = 1.5f;
   private float TailFollowDistance = 0.5f;

    Pooling Pool;

    GameObject playerObject;
    GameObject PointObject;

    private void Start()
    {
        Pool = GetComponent<Pooling>();
        PointObject = Instantiate(PointObjectPrefab, new Vector3(Random.Range(-MovementScript.mapWidth, MovementScript.mapWidth), 0,
            Random.Range(-MovementScript.mapHeight, MovementScript.mapHeight)), Quaternion.identity);
        playerObject = Instantiate(playerObjectPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        PointsEvent.AddListener(()=>AddToTail(playerObject));
        MenuController.LoseEvent.AddListener(FailGame);
    }
    public void StartGame()
    {
        disposables = new CompositeDisposable();
        var stream = playerObject.transform.UpdateAsObservable().Where(_ =>
           playerObject.transform.position.x < -MovementScript.mapWidth
        || playerObject.transform.position.x > MovementScript.mapWidth
        || playerObject.transform.position.z > MovementScript.mapHeight
        || playerObject.transform.position.z < -MovementScript.mapHeight).Subscribe(x => {
            MenuController.LoseEvent.Invoke();
            disposables.Dispose();
            }).AddTo(disposables);

        playerObject.GetComponent<MovementScript>().PointsEvent = PointsEvent;

        MovementScript.StartGameEvent.Invoke();
    }
    public void FailGame()
    {
        ClearTail();
    }

    private void ClearTail()
    {
        foreach (var obj in TailObjects)
        {
            Pool.ReturnToPool(obj);
            obj.GetComponent<TailScript>().DisposeDisposables();
        }
        TailObjects.Clear();
    }
    void AddToTail(GameObject player)
    {
        var obj = Pool.FromPool();
        GameObject frontObj;
        if(TailObjects.Count == 0)
        {
            frontObj = player;
            obj.GetComponent<TailScript>().FollowDistance = PlayerFollowDistance;
            obj.transform.position = frontObj.transform.position-frontObj.transform.forward*5;

        }
        else
        {
            frontObj = TailObjects[TailObjects.Count - 1];
            obj.GetComponent<TailScript>().FollowDistance = TailFollowDistance;
            obj.transform.position = frontObj.transform.position;

        }
        TailObjects.Add(obj);
        obj.GetComponent<TailScript>().target = frontObj;
    }
}

