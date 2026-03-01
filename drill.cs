using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    public Transform player;
    public float radius = 0.6f;
    public float drillPullPushSpeed;
    public float offset;

    public GameObject[] interactedObjs;
    public LayerMask drillLayer;
    public camFollow cam;
    Vector2 depo;
    public GameObject depoObj;
    public ParticleSystem dirt_VFX;
    public GameObject dynamite;

    [Header("Drill shake when mining")]
    public float drillShakeAmount = 0.04f;

    [Header("Mining time by tag. Set same length for both arrays. Unknown tags use Default Mine Time.")]
    public float defaultMineTime = 0.2f;
    public string[] mineTimeTags = new string[0];
    public float[] mineTimeSeconds = new float[0];

    // Current mining target and progress
    GameObject _currentMiningTarget;
    float _miningProgress;
    float _mineTimeRequired;

    void Start()
    {
        // OnTriggerEnter2D requires at least one side to have a Rigidbody2D - ensure drill has one (kinematic)
        if (GetComponent<Rigidbody2D>() == null)
        {
            var rb = gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;
            rb.simulated = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = player.position;

        Vector2 dir = (mouse - playerPos).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + offset;

        // Use local position/rotation when drill is a child (e.g. of player) so parent Rigidbody doesn't override
        if (transform.parent != null)
        {
            Vector2 localOffset = (Vector2)transform.parent.InverseTransformDirection(dir) * radius;
            transform.localPosition = new Vector3(localOffset.x, localOffset.y, 0f);
            float parentZ = transform.parent.eulerAngles.z;
            transform.localRotation = Quaternion.Euler(0f, 0f, angle - parentZ);
        }
        else
        {
            transform.position = playerPos + (Vector2)(dir * radius);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        if (Input.GetMouseButton(0))
        {
            bool isDrilling = drill();
            if (isDrilling)
            {
                // Shake the drill while mining (in local space when parented)
                if (transform.parent != null)
                    transform.localPosition += (Vector3)(Random.insideUnitCircle * drillShakeAmount);
                else
                    transform.position += (Vector3)(Random.insideUnitCircle * drillShakeAmount);
                dirt_VFX.Play();
            }
        }
        else
        {
            if (dirt_VFX != null)
            {
                dirt_VFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            _currentMiningTarget = null;
        }
        if (Input.GetMouseButtonDown(2))
        {
            Instantiate(dynamite, transform.position, Quaternion.identity);
        }
        // Remove any missing or destroyed GameObjects from interactedObjs
        if (interactedObjs != null && interactedObjs.Length > 0)
        {
            var validList = new List<GameObject>();
            foreach (var obj in interactedObjs)
            {
                if (obj != null)
                {
                    validList.Add(obj);
                }
            }
            if (validList.Count != interactedObjs.Length)
            {
                interactedObjs = validList.ToArray();
            }
        }


        radius = Mathf.Clamp(radius, 1, 2);

        
            if (Input.GetKey("e"))
            {
                radius += drillPullPushSpeed * Time.deltaTime;
            }
       
            if (Input.GetKey("q"))
            {
                radius -= drillPullPushSpeed * Time.deltaTime;
            }
       
        
    }


    bool drill()
    {
        GameObject closestObj = null;
        float closestDist = float.MaxValue;

        if (interactedObjs != null && interactedObjs.Length > 0)
        {
            foreach (GameObject obj in interactedObjs)
            {
                if (obj == null) continue;
                float dist = Vector2.Distance(transform.position, obj.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestObj = obj;
                }
            }
        }

        if (closestObj == null)
        {
            if (dirt_VFX != null)
                dirt_VFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _currentMiningTarget = null;
            return false;
        }

        if (closestObj != _currentMiningTarget)
        {
            _currentMiningTarget = closestObj;
            _miningProgress = 0f;
            _mineTimeRequired = GetMineTimeForTag(closestObj.tag);
            var sr = closestObj.GetComponent<SpriteRenderer>();
            if (sr != null && dirt_VFX != null)
            {
                var main = dirt_VFX.main;
                main.startColor = sr.color;
            }
        }

        if (dirt_VFX != null && !dirt_VFX.isPlaying)
        {
            dirt_VFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            dirt_VFX.Play();
        }

        if (_mineTimeRequired <= 0f)
        {
            Destroy(closestObj);
            cam.ShakeCamera(.01f, .1f, 1);
            _currentMiningTarget = null;
            return true;
        }

        _miningProgress += Time.deltaTime;
        if (_miningProgress >= _mineTimeRequired)
        {
            Destroy(closestObj);
            cam.ShakeCamera(.01f, .1f, 1);
            _currentMiningTarget = null;
        }

        return true;
    }

    float GetMineTimeForTag(string tag)
    {
        if (mineTimeTags == null || mineTimeSeconds == null) return defaultMineTime;
        int count = Mathf.Min(mineTimeTags.Length, mineTimeSeconds.Length);
        for (int i = 0; i < count; i++)
        {
            if (mineTimeTags[i] == tag)
                return mineTimeSeconds[i];
        }
        return defaultMineTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;
        if (((1 << layer) & drillLayer) != 0)
        {
            var tempList = interactedObjs != null ? new List<GameObject>(interactedObjs) : new List<GameObject>();
            if (!tempList.Contains(collision.gameObject))
            {
                tempList.Add(collision.gameObject);
                interactedObjs = tempList.ToArray();
            }
        }
        if (collision.gameObject.CompareTag("depo"))
        {
            depo = collision.ClosestPoint(transform.position);
            depoObj = collision.gameObject;
        }
    }

    public Vector2 returnDepo()
    {
        if(depo != null)
        {
            return depo;
        }
        else
        {
            return Vector2.zero;
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;
        if (((1 << layer) & drillLayer) != 0 && interactedObjs != null && interactedObjs.Length > 0)
        {
            var tempList = new List<GameObject>(interactedObjs);
            if (tempList.Remove(collision.gameObject))
            {
                interactedObjs = tempList.ToArray();
            }
            if (_currentMiningTarget == collision.gameObject)
                _currentMiningTarget = null;
            if (dirt_VFX != null)
                dirt_VFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
