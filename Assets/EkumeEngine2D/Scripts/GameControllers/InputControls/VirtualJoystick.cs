using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    Image bgImg;
    Image joystickImg;
    Vector3 inputVector;

    public float joystickDeadZone;
    public bool useAxisXPositive;
    public bool useAxisXNegative;
    public bool useAxisYPositive;
    public bool useAxisYNegative;

    public int inputControlUp;
    public int inputControlDown;
    public int inputControlRight;
    public int inputControlLeft;

    bool enterInDownJoystickAxisXPositive = true;
    bool enterInDownJoystickAxisXNegative = true;

    bool enterInDownJoystickAxisYPositive = true;
    bool enterInDownJoystickAxisYNegative = true;

    public InputControlsManager inputControlsManager;

    void Start()
    {
        bgImg = GetComponent<Image>();

        if(transform.childCount == 0)
        {
            Debug.LogError("The GameObject: "+gameObject.name+" don't have a child. The script VirtualJoystick should be added to some GameObject with a child.");
            return;
        }

        joystickImg = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        if (InputControlsManager.instance.useControlsInUI)
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos))
            {
                pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
                pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

                inputVector = new Vector3(pos.x * 2, 0, pos.y * 2);
                inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

                //Move Joystick Img
                if (useAxisXNegative || useAxisXPositive)
                    joystickImg.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 2),
                                                                            joystickImg.rectTransform.anchoredPosition.y);
                if (useAxisYNegative || useAxisYPositive)
                    joystickImg.rectTransform.anchoredPosition = new Vector3(joystickImg.rectTransform.anchoredPosition.x,
                                                                            inputVector.z * (bgImg.rectTransform.sizeDelta.y / 2));

            }
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        if (InputControlsManager.instance.useControlsInUI)
        {
            OnDrag(ped);
        }
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        if (InputControlsManager.instance.useControlsInUI)
        {
            inputVector = Vector3.zero;
            joystickImg.rectTransform.anchoredPosition = Vector3.zero;

            InputControls.inputPressedVirtualJoystickXPositive[inputControlRight] = false;
            InputControls.inputPressedVirtualJoystickXNegative[inputControlLeft] = false;
            InputControls.inputPressedVirtualJoystickYPositive[inputControlUp] = false;
            InputControls.inputPressedVirtualJoystickYNegative[inputControlDown] = false;

            if (!enterInDownJoystickAxisXNegative)
            {
                enterInDownJoystickAxisXNegative = true;
                InputControls.inputUpVirtualJoystickXNegative[inputControlLeft] = true;
            }

            if (!enterInDownJoystickAxisXPositive)
            {
                enterInDownJoystickAxisXPositive = true;
                InputControls.inputUpVirtualJoystickXPositive[inputControlRight] = true;
            }

            if (!enterInDownJoystickAxisYNegative)
            {
                enterInDownJoystickAxisYNegative = true;
                InputControls.inputUpVirtualJoystickYNegative[inputControlDown] = true;
            }

            if (!enterInDownJoystickAxisYPositive)
            {
                enterInDownJoystickAxisYPositive = true;
                InputControls.inputUpVirtualJoystickYPositive[inputControlUp] = true;
            }
        }
    }

    void Update()
    {
        if (InputControlsManager.instance.useControlsInUI)
        {
            //X Axis
            if (useAxisXPositive && inputVector.x > joystickDeadZone)
            {
                if (enterInDownJoystickAxisXPositive)
                {
                    InputControls.inputDownVirtualJoystickXPositive[inputControlRight] = true; //Turned false in InputControl.cs in LateUpdate

                    enterInDownJoystickAxisXPositive = false;

                    if (!enterInDownJoystickAxisXNegative)
                    {
                        enterInDownJoystickAxisXNegative = true;
                        InputControls.inputUpVirtualJoystickXPositive[inputControlRight] = true;
                    }
                }

                InputControls.inputPressedVirtualJoystickXPositive[inputControlRight] = true;
                InputControls.inputPressedVirtualJoystickXNegative[inputControlLeft] = false;
            }
            else if (useAxisXNegative && inputVector.x < -joystickDeadZone)
            {
                if (enterInDownJoystickAxisXNegative)
                {
                    InputControls.inputDownVirtualJoystickXNegative[inputControlLeft] = true; //Turned false in InputControl.cs in LateUpdate

                    enterInDownJoystickAxisXNegative = false;

                    if (!enterInDownJoystickAxisXPositive)
                    {
                        enterInDownJoystickAxisXPositive = true;

                        InputControls.inputUpVirtualJoystickXNegative[inputControlLeft] = true;
                    }
                }

                InputControls.inputPressedVirtualJoystickXNegative[inputControlLeft] = true;
                InputControls.inputPressedVirtualJoystickXPositive[inputControlRight] = false;

            }

            //Y Axis
            if (useAxisYPositive && inputVector.z > joystickDeadZone)
            {
                if (enterInDownJoystickAxisYPositive)
                {
                    InputControls.inputDownVirtualJoystickYPositive[inputControlUp] = true; //Turned false in InputControl.cs in LateUpdate

                    enterInDownJoystickAxisYPositive = false;

                    if (!enterInDownJoystickAxisYNegative)
                    {
                        enterInDownJoystickAxisYNegative = true;
                        InputControls.inputUpVirtualJoystickYPositive[inputControlUp] = true; //Turned false in InputControl.cs in LateUpdate
                    }
                }

                InputControls.inputPressedVirtualJoystickYPositive[inputControlUp] = true;
                InputControls.inputPressedVirtualJoystickYNegative[inputControlDown] = false;
            }
            else if (useAxisYNegative && inputVector.z < -joystickDeadZone)
            {
                if (enterInDownJoystickAxisYNegative)
                {
                    InputControls.inputDownVirtualJoystickYNegative[inputControlDown] = true; //Turned false in InputControl.cs in LateUpdate

                    enterInDownJoystickAxisYNegative = false;

                    if (!enterInDownJoystickAxisYPositive)
                    {
                        enterInDownJoystickAxisYPositive = true;

                        InputControls.inputUpVirtualJoystickYNegative[inputControlDown] = true; //Turned false in InputControl.cs in LateUpdate
                    }
                }

                InputControls.inputPressedVirtualJoystickYNegative[inputControlDown] = true;
                InputControls.inputPressedVirtualJoystickYPositive[inputControlUp] = false;
            }
        }
    }
}
