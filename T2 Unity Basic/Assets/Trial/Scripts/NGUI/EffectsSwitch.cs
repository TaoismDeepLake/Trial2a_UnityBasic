using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class EffectsSwitch : MonoBehaviour {

    [SerializeField] PostProcessingBehaviour tool;

	public void ToggleEffects()
    {
        tool.enabled = !tool.enabled;
    }
}
