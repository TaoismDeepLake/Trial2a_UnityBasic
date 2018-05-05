using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttackListItem : MonoBehaviour {

    [SerializeField] UITexture sprite;
    [SerializeField] UILabel unitName;
    [SerializeField] UIButton button;

    public AttrController attrController;
    public MotionController mc;

    public bool selected = false;

    private void Update()
    {
        if (selected)
        {
            Flashing();
        }
        else
        {
            sprite.alpha = 1.0f;
            //sprite.color = Color.white;
        }
    }

    bool increasing;
    [SerializeField] float flashSpeed = 2.0f;

    void Flashing()
    {
        if (increasing)
        {
            sprite.alpha += flashSpeed * Time.deltaTime;
            if (sprite.alpha > 1f)
                increasing = false;
        }
        else
        {
            sprite.alpha -= flashSpeed * Time.deltaTime;
            if (sprite.alpha < 0.2f)
                increasing = true;
        }
    }

    public void SetContent(MotionController _mc)
    {
        attrController = _mc.ac;
        sprite.mainTexture = _mc.ac.icon;

        unitName.text = _mc.ac.unitName;
        mc = _mc;

        mc.listItem = this;

        mc.ac.Death += Destruct;
        mc.ac.Death += GeneralController.playerMC.GetComponent<PlayerAutomation>().CheckStopAI;
    }

    public void Destruct()
    {
        Destroy(gameObject);
    }

    public void PlayerAttack()
    {
        GeneralController.instance.HaltPlayer();

        MotionController playerMC = GeneralController.playerMC;
        PlayerAutomation auto = playerMC.GetComponent<PlayerAutomation>();
        auto.enabled = true;
        auto.targetList.Insert(0, mc);
        selected = true;

        transform.SetSiblingIndex(1);
        AutoAttackList.instance.Refresh();
        auto.OnStopAI += StopFlashing;
    }

    public Single OnDestruct;

    public void StopFlashing()
    {
        selected = false;
    }

    private void OnDestroy()
    {
        if (OnDestruct != null)
            OnDestruct();
    }
}
