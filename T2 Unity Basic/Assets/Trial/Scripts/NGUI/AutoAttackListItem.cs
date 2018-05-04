using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttackListItem : MonoBehaviour {

    [SerializeField] UITexture sprite;
    [SerializeField] UILabel unitName;
    [SerializeField] UIButton button;

    public AttrController attrController;
    public MotionController mc;

    public void SetContent(MotionController _mc)
    {
        attrController = _mc.ac;
        sprite.mainTexture = _mc.ac.icon;

        unitName.text = _mc.ac.unitName;
        mc = _mc;

        mc.ac.Death += Destruct;
        mc.ac.Death += GeneralController.playerMC.GetComponent<PlayerAutomation>().CheckStopAI;
    }

    public void Destruct()
    {
        Destroy(gameObject);
    }

    public void PlayerAttack()
    {
        MotionController playerMC = GeneralController.playerMC;
        PlayerAutomation auto = playerMC.GetComponent<PlayerAutomation>();
        auto.enabled = true;
        auto.targetList.Insert(0, mc);
    }

    public Single OnDestruct;

    private void OnDestroy()
    {
        if (OnDestruct != null)
            OnDestruct();
    }
}
