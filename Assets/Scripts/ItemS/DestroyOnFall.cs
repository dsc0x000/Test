using UnityEngine;

public class DestroyOnFall : MonoBehaviour
{
    [SerializeField] private float minHeight;

    private void FixedUpdate()
    {
        if (transform.position.y < minHeight)
            Destroy(gameObject);
    }
}
