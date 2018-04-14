using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

    [SerializeField] MotionController mc;
    [SerializeField] CharacterController cc;

    public List<MotionController> targetList = new List<MotionController>();

    public MotionController target
    {
        get
        {
            if (0 == targetList.Count)
                return null;

            return targetList[0];
        }
    }

    public int teamIndex
    {
        get { return mc.teamIndex; }
    }

    //public float detectRadius = 5f;
    public float range = 3f;
    public float forgiveDistance = 10f;

    public bool leftHanded = true;
    public bool strafer = true;


    private void Awake()
    {
        if (null == mc)
            mc = GetComponent<MotionController>();

        if (null == cc)
            cc = GetComponent<CharacterController>();

        GetComponent<AttrController>().Death += StopAI;
        

        leftHanded = 0 == Random.Range(0, 2);
    }

    Vector3 destination;
    // Update is called once per frame
    void Update () {
        CheckTarget();

        if (target != null)
        {
            Vector3 otherPos = target.transform.position;
            Vector3 myPos = transform.position;

            destination = otherPos + (myPos - otherPos).normalized * range;

            if (Vector3.Distance(destination, myPos) < 0.5f)
            {
                //close enough. attack!
                //Debug.Log(string.Format("{0} attacking", name, destination));
                mc.atkDirection = (otherPos - myPos).normalized;
                mc.AttackAttempt();

                //strafing
                if (strafer)
                {
                    if (leftHanded)
                    {
                        mc.MoveTowards(-transform.right);
                    }
                    else
                    {
                        mc.MoveTowards(transform.right);
                    }
                }
            }
            else
            {
                //Debug.Log(string.Format("{0} moving to {1}", name, destination));
                //move towards destination
                mc.MoveTowards(destination - myPos);
            }

            mc.transform.LookAt(target.transform);
        }


	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (target != null)
            Gizmos.DrawLine(transform.position, destination);
    }

    public void StopAI()
    {
        enabled = false;
    }


    void CheckTarget()
    {
        while (target != null)
        {
            Transform t = target.transform;
            if (Vector3.Distance(t.position, transform.position) > forgiveDistance ||
                false == target.enabled || target.teamIndex == teamIndex)
            {
                targetList.Remove(target);
            }
            else
            {
                break;//find a valid target, ignore the others.
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        MotionController otherMC = other.GetComponent<MotionController>();

        Debug.Log("AI noticed something.");

        if (other.tag == "Unit" && otherMC && otherMC.teamIndex != teamIndex && otherMC.enabled)
        {
            Debug.Log("AI found a Target.");
            targetList.Add(otherMC);
        }
    }
}
