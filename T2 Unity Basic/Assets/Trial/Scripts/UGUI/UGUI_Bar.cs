using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UGUI_Bar : MonoBehaviour {

    public float maxVal = 100f;
    public float val = 100f;

    [SerializeField] Image barImage;

	public void SetVal(float _val)
    {
        val = _val > 0? _val : 0;
        barImage.fillAmount = val / maxVal;
    }
}
