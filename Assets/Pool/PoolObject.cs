using UnityEngine;
using System.Collections;

public class PoolObject : MonoBehaviour
{
    // Private Instance Variables
    private bool isActive = false;
    private float startTime = 0f;
    private float minLifeSpan = 1f;
    private float maxLifeSpan = 7.5f;
    private float lifeSpan = 5f;
    private System.Action<PoolObject> OnDeathCallback;

    // Use this for initialization
    public void Init(System.Action<PoolObject> callback)
    {
        isActive = true;
        startTime = Time.time;
        OnDeathCallback = callback;
        lifeSpan = Random.Range(minLifeSpan, maxLifeSpan);
    }

    // Update is called once per frame
    private void Update()
    {
        if (isActive)
        {
            if (Time.time - startTime > lifeSpan)
            {
                isActive = false;
                if (OnDeathCallback != null)
                {
                    System.Action<PoolObject> callback = OnDeathCallback;
                    OnDeathCallback = null;
                    callback(this);
                }
            }
        }
    }
}
