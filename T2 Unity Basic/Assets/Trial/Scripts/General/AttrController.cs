﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttrController : MonoBehaviour {

    public event Single Death;
    public event Single OnTakeDamge;

    [SerializeField] bool startFull = true;
    [SerializeField] UGUI_Bar HPBar = null;


    public bool isAlive = true;

    public float maxHP = 100f, HP;
    public float maxMP = 100f, MP;

    public float atk = 10f;
    public float regenHP = 0.1f;
    public float regenMP = 0.1f;

    private void Start()
    {
        if (startFull)
        {
            HP = maxHP;
            MP = maxMP;
        }

        if (HPBar)
        {
            HPBar.maxVal = maxHP;
            HPBar.SetVal(HP);
        }

        Death += Die;
    }

    // Update is called once per frame
    void LateUpdate() {
        if (isAlive)
            Upkeep();

    }

    void Upkeep()
    {
        if (HP <= 0)
            Death();

        #region REGEN
        HP += regenHP * Time.deltaTime;
        if (HP > maxHP)
            HP = maxHP;

        if (HPBar)
            HPBar.SetVal(HP);

        MP += regenMP * Time.deltaTime;
        if (MP > maxMP)
            MP = maxMP;

        #endregion
    }

    void Die()
    {
        isAlive = false;
    }

    public void TakeDamage(float dmg, MotionController src = null)
    {
        HP -= dmg;
        if (HPBar)
            HPBar.SetVal(HP);

        if (OnTakeDamge != null)
            OnTakeDamge();
    }
}