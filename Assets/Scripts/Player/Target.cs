using UnityEngine;
using UnityEngine.Events;

public class Target
{
    public GameObject gameObject;
    private readonly TargetPool pool;

    public UnityEvent OnHit = new UnityEvent();

    public Target(GameObject prefab, Transform parent, TargetPool pool)
    {
        this.gameObject = Object.Instantiate(prefab, parent);
        this.pool = pool;

        var targetCollider = this.gameObject.GetComponent<TargetCollider>();
        if (targetCollider == null)
        {
            Debug.LogError("The Target Prefab is missing the 'TargetCollider' script component.", this.gameObject);
            return;
        }
        targetCollider.target = this;
        this.gameObject.SetActive(false);
    }

    public void Hit()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        OnHit.Invoke();
        pool.ReturnTarget(this);
    }

    public void Activate(Vector3 position, Quaternion rotation)
    {
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}