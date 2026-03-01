using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dynamite : MonoBehaviour
{
    public float radius;
    [Tooltip("Layers that can be destroyed by the explosion (e.g. dirt/blocks).")]
    public LayerMask explosionLayerMask;
    [Tooltip("VFX prefab to spawn at explosion position. Optional; can destroy itself or use a duration.")]
    public GameObject explode_VFX;

    void Start()
    {
        StartCoroutine(countdown());
    }

    IEnumerator countdown()
    {
        yield return new WaitForSeconds(5);
        explode();
    }

    void explode()
    {
        Vector2 center = transform.position;

        // Spawn explosion VFX at this position
        if (explode_VFX != null)
        {
            GameObject vfx = Instantiate(explode_VFX, center, Quaternion.identity);
            // Remove clone after 5 seconds if it doesn't destroy itself
            Destroy(vfx, 5f);
        }

        // Use OverlapCircle to find all colliders in radius on the explosion layers
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius, explosionLayerMask);
        HashSet<GameObject> toDestroy = new HashSet<GameObject>();
        foreach (Collider2D col in hits)
        {
            if (col != null && col.gameObject != gameObject)
                toDestroy.Add(col.gameObject);
        }
        foreach (GameObject obj in toDestroy)
            Destroy(obj);

        Destroy(gameObject);
    }

    // Draw a gizmo for the explosion radius in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
