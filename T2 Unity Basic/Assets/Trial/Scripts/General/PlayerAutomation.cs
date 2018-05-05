using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutomation : MonoBehaviour {

    [SerializeField] MotionController mc;
    [SerializeField] CharacterController cc;

    public List<MotionController> targetList = new List<MotionController>();

    public event Single OnStopAI;

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
    public float forgiveDistance = 1000f;


    private void Awake()
    {
        if (null == mc)
            mc = GetComponent<MotionController>();

        if (null == cc)
            cc = GetComponent<CharacterController>();

        
    }



    Vector3 destination;
    // Update is called once per frame
    void Update()
    {
        if (mc.hasAnyInput())
            StopAI();

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

    public void CheckStopAI()
    {
        CheckTarget();
        Debug.Log("target = " + target);
        if (!target)
            StopAI();
    }

    public void StopAI()
    {
        targetList.Clear();
        enabled = false;
        mc.playerControlled = true;
        if (OnStopAI!= null)
        {
            OnStopAI();
        }
    }

    public void OnEnable()
    {
        mc.playerControlled = false;
    }


    void CheckTarget()
    {
        while (target != null)
        {
            Transform t = target.transform;
            if (Vector3.Distance(t.position, transform.position) > forgiveDistance ||
                false == target.enabled || target.teamIndex == teamIndex || !target.ac.isAlive)
            {
                targetList.Remove(target);
            }
            else
            {
                break;//find a valid target, ignore the others.
            }

        }
    }



    float minFlip = 1f;
    float maxFlip = 3f;
}
