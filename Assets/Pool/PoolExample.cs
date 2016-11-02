using UnityEngine;
using System.Collections;

public class PoolExample : MonoBehaviour
{
    // Unity Editor Variables
    [SerializeField] private GameObject prefab;

    // Private Instance Variables
    private const int POOL_WARMUP_SIZE = 200;
    private Pool<PoolObject> pool;
    private int spawnedObjectCount = 0;
    private int spaceBarPressedCounter = 0;

    // Use this for initialization
    private void Start()
    {
        SetupPool();
    }

    // Update is called once per frame
    private void Update ()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            spaceBarPressedCounter++;
            for (int i = 0; i < 50; i++)
            {
                spawnedObjectCount++;
                PoolObject pObj = pool.New();
                pObj.gameObject.SetActive(true);
                pObj.name = prefab.name + "_" + spaceBarPressedCounter + "_" + i;
                Vector3 pos = Random.insideUnitSphere;
                pos.x *= Random.Range(1f, 40f);
                pos.y *= Random.Range(1f, 20f);
                pos.z *= Random.Range(1f, 40f);
                pObj.transform.position = pos;
                pObj.Init(OnPoolObjectDeath);
            }
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 150, 20), "Pool Objects: " + pool.Count);
        GUI.Label(new Rect(10, 30, 150, 20), "Object in scene: " + spawnedObjectCount);
    }

    private void SetupPool()
    {
        pool = new Pool<PoolObject>(
            () => 
            {
                GameObject obj = Instantiate<GameObject>(prefab);
                obj.SetActive(false);
                PoolObject pObj = obj.GetComponent<PoolObject>();
                return pObj;
            }
        );
        pool.WarmUp(POOL_WARMUP_SIZE);
    }

    private void OnPoolObjectDeath(PoolObject pObj)
    {
        spawnedObjectCount--;
        pObj.gameObject.SetActive(false);
    }
}
