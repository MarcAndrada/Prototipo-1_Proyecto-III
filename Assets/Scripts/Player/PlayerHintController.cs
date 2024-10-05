using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHintController : MonoBehaviour
{
    public enum DeviceType { KEYBOARD = 0, GAMEPAD = 1, MOUSE = 2}
    public enum ActionType { NONE, GRAB, USE }



    [field: SerializeField]
    public DeviceType deviceType {  get; private set; }

    [Space, SerializedDictionary("Action", "Device Sprites"), Tooltip("Esto relaciona la posicion del modulo con el objeto que habra en la casilla")]
    public SerializedDictionary<ActionType, List<Sprite>> ActionSprites;
    [Space, SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Image hintImage;
    [SerializeField]
    private float hintOffset;


    private void Start()
    {
        canvas.worldCamera = Camera.main;

        PlayerInput input = GetComponent<PlayerInput>(); 
        InputDevice device = input.devices[0];  // En este caso, tomamos el primer dispositivo de la lista


        if (device is Gamepad)
            deviceType = DeviceType.GAMEPAD;
        else if (device is Keyboard)
            deviceType = DeviceType.KEYBOARD;
        else if (device is Mouse)
            deviceType = DeviceType.MOUSE;
        else
            Debug.Log($"Input no reconocido, el dispositivo es: {device.displayName}");


        UpdateActionType(ActionType.GRAB);
    }

    private void Update()
    {
        hintImage.transform.position = transform.position + new Vector3(0, hintOffset, 0);
    }

    public void UpdateActionType(ActionType _action)
    {
        if (_action == ActionType.NONE)
        {
            //Ocultar la UI de inputs
            hintImage.gameObject.SetActive(false);
            return;
        }

        //Mostrar la UI de inputs
        hintImage.gameObject.SetActive(true);


        Sprite currentSprite = ActionSprites[_action][(int)deviceType];
        hintImage.sprite = currentSprite;

    }


}
