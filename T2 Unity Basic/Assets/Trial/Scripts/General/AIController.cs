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

        mc.ac.Death += StopAI;
        

        leftHanded = 0 == Random.Range(0, 2);

        StartCoroutine(FlipHand());
    }



    Vector3 destination;
    // Update is called once per frame
    void Update () {
        CheckTarget();

        if (target != null)
        {
            Vector3 otherPos = target.transform.position;
            Vector3 myPos = transform.position;

            destination = otherPos + (myPos - otherPos).normalized * (range - 1f);
            //attack
            if (Vector3.Distance(otherPos, myPos) <= range)
            {
                mc.atkDirection = (otherPos - myPos).normalized;
                mc.AttackAttempt();
            }

            //walking around
            if (Vector3.Distance(destination, myPos) < 1f)
            {
                //close enough. attack!
                //Debug.Log(string.Format("{0} attacking", name, destination));
              
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
        if (enabled && target != null)
        {
            Gizmos.DrawLine(transform.position, destination);

            Gizmos.DrawSphere(destination, 0.3f);
        }
    }

    public void StopAI()
    {
        enabled = false;
    }


    void CheckTarget()
    {
        while (targetList.Count > 0)
        {
            if (target == null)
            {
                targetList.Remove(target);
                continue;
            }

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

        //Debug.Log("AI noticed something.");

        if (other.tag == "Unit" && otherMC && otherMC.teamIndex != teamIndex && otherMC.enabled)
        {
            Debug.Log("AI found a Target.");
            targetList.Add(otherMC);
        }
    }

    float minFlip = 1f;
    float maxFlip = 3f;

    IEnumerator FlipHand()
    {
        yield return new WaitForSeconds(Random.Range(minFlip, maxFlip));
        leftHanded = !leftHanded;

        StartCoroutine(FlipHand());
    }
}
