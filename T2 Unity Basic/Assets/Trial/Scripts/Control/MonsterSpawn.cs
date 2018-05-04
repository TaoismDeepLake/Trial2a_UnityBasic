using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public struct RateController
//{
//    public GameObject prefabToSpawn;
//}

public class MonsterSpawn : MonoBehaviour {

    public bool activated;

    public float minSpawnTime = 2f, maxSpawnTime = 4f;

    public GameObject prefabToSpawn;
    public float spawnChance = 0.5f;
    public int maxCount = 10;

    public float spawnRadius = 8f;

    SphereCollider _c;
    SphereCollider c
    {
        get
        {
            if (!_c)
                _c = GetComponent<SphereCollider>();

            return _c;
        }
        set
        {
            _c = value;
        }
    }

    public List<GameObject> spawnList = new List<GameObject>();

    public void Spawn()
    {
        float spawnR = Random.Range(1f, spawnRadius);
        float spawnTheta = Random.Range(0, 2 * Mathf.PI);

        Vector3 pos = transform.position + new Vector3(spawnR * Mathf.Cos(spawnTheta), 0, spawnR * Mathf.Sin(spawnTheta));

        GameObject g = Instantiate(prefabToSpawn, pos, Quaternion.identity);

        spawnList.Add(g);

    }

    public void CheckList()
    {
        //remove destroyed objects

        for(int i = 0; i < spawnList.Count; i++)
        {
            if (spawnList[i] == null)
            {
                spawnList.RemoveAt(i);
            }
            else
            {
                AttrController attr = spawnList[i].GetComponent<AttrController>();
                if (attr && !attr.isAlive)
                {
                    Destroy(spawnList[i]);
                    spawnList.RemoveAt(i);
                }
            }
        }
    }

    public void SpawnAttempt()
    {
        if (!activated)
            return;

        CheckList();

        if (spawnList.Count >= maxCount)
            return;

        float r = Random.Range(0, 1f);
        if (r < spawnChance)
        {
           
            Spawn();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = activated ? Color.yellow : Color.grey;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, c.radius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ("Unit" == other.tag && other.GetComponent<MotionController>().playerControlled)
        {
            activated = true;
            StartCoroutine(SpawnCycle());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ("Unit" == other.tag && other.GetComponent<MotionController>().playerControlled)
        {
            activated = false;
            //foreach(GameObject g in spawnList)
            //{
            //    Destroy(g);
            //}

            //spawnList.Clear();
        }
    }

    IEnumerator SpawnCycle()
    {
        yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));

        if (activated)
        {
            SpawnAttempt();
            StartCoroutine(SpawnCycle());
        }
    }
    

}
