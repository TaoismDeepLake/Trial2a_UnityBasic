using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Single();

public class MotionController : MonoBehaviour {

    [SerializeField] Animator anim;
    [SerializeField] CharacterController cc;
    [SerializeField] SoundController sc;
    [SerializeField] VocalController vc;
    [SerializeField] AttrController ac;

    EasyJoystick sz;
    /// <summary>
    /// 0 = player
    /// </summary>
    public int teamIndex = 0;
    public int attackTypeCount = 2;
    public float moveSpeed = 3;
    public float atkType = 0;
    public bool attacking;
    public float attackCD = 0f;//attack cool down dynamic

    public bool playerControlled = false;

    public Vector3 atkDirection;

    public float attackInterval = 1f;

    protected float moveX = 0, moveZ = 0;

    /// <summary>
    /// Try calling AttackAttempt to achieve cd.
    /// </summary>
    public event Single Attack;

    private void Awake()
    {
        if (null == anim)
        {
            anim = GetComponentInChildren<Animator>();
        }

        if (null == cc)
        {
            cc = GetComponentInChildren<CharacterController>();
        }

        if (null == sc)
        {
            sc = GetComponentInChildren<SoundController>();
        }

        if (null == ac)
        {
            ac = GetComponentInChildren<AttrController>();
        }
    }

    // Use this for initialization
    void Start () {
        ac.Death += Death;
        ac.OnTakeDamge += Hurt;
        Attack += AttackBasic;
	}
	
    public void SetEZControl(EasyJoystick joystick)
    {
        sz = joystick;
        playerControlled = true;
    }

	// Update is called once per frame
	void Update () {
        Upkeep();

        if (playerControlled)
            HandleInput();

        anim.SetFloat("Attacking", attackCD > 0 ? 1 : 0);
        anim.SetFloat("Moving", moveX != 0 || moveZ != 0 ? 1 : 0);

        HandleMovement();
        HandleAnimation();

        moveX = 0;
        moveZ = 0;
	}

    void Upkeep()
    {
        if (attackCD > 0)
            attackCD -= Time.deltaTime;

        if (false == attacking)
        {
            atkType = 0;
        }

        
    }

    public void MoveTowards(Vector3 direction)
    {
        //transform.forward = direction;
        direction.Normalize();
        moveX = direction.x;
        moveZ = direction.z;

        anim.SetFloat("Moving", moveX != 0 || moveZ != 0 ? 1 : 0);
    }

    protected void HandleInput()
    {
        float fire;


        if (GeneralController.instance.useEasyTouch)
        {
            fire = Input.GetAxis("Fire2");
        }
        else
        {
            fire = Input.GetAxis("Fire1");
        }

        

        moveX = sz.JoystickValue.x / sz.speed.x;
        moveZ = sz.JoystickValue.y;
        //moveX = Input.GetAxis("Horizontal");
        //moveZ = Input.GetAxis("Vertical");



        float threshold = 0.1f;

        //if (Mathf.Abs(moveX) > threshold || Mathf.Abs(moveZ) > threshold)
        //    return;//can not attack while moving

        if (attackCD <= 0 && false == attacking && fire > threshold)
        {
            Attack();
        }

        Vector3 world = transform.localToWorldMatrix * new Vector3(moveX, 0, moveZ);

        moveX = world.x;
        moveZ = world.z;

        atkDirection = transform.forward;
    }

    void AttackBasic()
    {
        atkType = Random.Range(0.5f, attackTypeCount);
        attacking = true;
        sc.Attack(0.5f);
        if (vc)
            vc.Attack();

        StartCoroutine(ResetAtk());
    }

    IEnumerator ResetAtk()
    {
        attackCD = attackInterval;
        yield return new WaitForSeconds(attackInterval);
        attacking = false;
    }


    protected void HandleMovement()
    {
        cc.SimpleMove( new Vector3(moveX * moveSpeed, 0, moveZ * moveSpeed));
    }

    protected void HandleAnimation()
    {
        Vector3 local = transform.worldToLocalMatrix *(new Vector3(moveX, 0, moveZ));


        anim.SetFloat("SpeedX", local.x);
        anim.SetFloat("SpeedZ", local.z);

        if (attacking)
            anim.SetFloat("AttackVer", atkType);
        
    }

    public void Hurt()
    {
        vc.Hurt();
    }

    public void Death()
    {
        anim.SetBool("Dead", true);
        vc.Death();
        cc.enabled = false;
        enabled = false;
    }

    public void AttackAttempt()
    {
        if (attackCD <= 0)
            Attack();
    }
}
