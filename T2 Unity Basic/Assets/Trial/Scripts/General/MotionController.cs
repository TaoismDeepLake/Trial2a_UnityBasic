using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Single();

public class MotionController : MonoBehaviour {

    [SerializeField] Animator anim;
    [SerializeField] CharacterController cc;
    [SerializeField] SoundController sc;
    [SerializeField] VocalController vc;
    public int attackTypeCount = 2;
    public float moveSpeed;
    public float atkType = 0;
    public bool attacking;
    public float attackCD = 0f;//attack cool down dynamic


  

    public float attackInterval = 1f;

    protected float moveX = 0, moveZ = 0;

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

        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Upkeep();

        HandleInput();
        HandleMovement();
        HandleAnimation();

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

    protected void HandleInput()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        float threshold = 0.1f;

        if (Mathf.Abs(moveX) > threshold || Mathf.Abs(moveZ) > threshold)
            return;//can not attack while moving

        if (attackCD <= 0 && false == attacking && Input.GetAxis("Fire1") > threshold)
        {
            atkType = Random.Range(0.5f, attackTypeCount);
            attacking = true;
            sc.Attack(0.5f);
            if (vc)
                vc.Attack();

            if (Attack != null)
                Attack();

            StartCoroutine(ResetAtk());
        }

        if (Input.GetAxis("Fire1") < threshold)
        {
            attacking = false;
        }
    }

    IEnumerator ResetAtk()
    {
        attackCD = attackInterval;
        yield return new WaitForSeconds(attackInterval);
        attacking = false;
    }


    protected void HandleMovement()
    {
        cc.SimpleMove(transform.localToWorldMatrix * new Vector3(moveX * moveSpeed, 0, moveZ * moveSpeed));
    }

    protected void HandleAnimation()
    {
        anim.SetFloat("SpeedX", moveX);
        anim.SetFloat("SpeedZ", moveZ);

        if (attacking)
            anim.SetFloat("AttackVer", atkType);
        else
            anim.SetFloat("AttackVer", 0, 0.2f, Time.deltaTime);
    }


}
