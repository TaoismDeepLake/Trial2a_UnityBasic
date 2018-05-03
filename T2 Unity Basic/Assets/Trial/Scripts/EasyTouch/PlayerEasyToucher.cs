using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEasyToucher : MonoBehaviour {

    public static PlayerEasyToucher instance;

    [SerializeField] EasyButton fireButton;
    [SerializeField] EasyJoystick moveJoystick;

    private void Awake()
    {
        instance = this;
    }

    public void InitEasyTouch(GameObject player)
    {
        fireButton.receiverGameObject = player;
        moveJoystick.YAxisTransform = player.transform;
        moveJoystick.XAxisTransform = player.transform;
        player.GetComponent<MotionController>().SetEZControl(moveJoystick);
    }
}
