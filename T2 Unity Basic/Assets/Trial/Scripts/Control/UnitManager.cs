using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {

    public static UnitManager instance;

    List<MotionController> units = new List<MotionController>();

    private void Awake()
    {
        instance = this;
    }


    void CheckList()
    {
        for(int i = 0; i < units.Count; i++)
        {
            MotionController mc = units[i];

            if (mc)
            {
                if (!mc.ac.isAlive)
                {
                    Destroy(mc.gameObject);
                    units.RemoveAt(i);
                }
            }
            else
            {
                units.RemoveAt(i);
            }
        }
    }
}
