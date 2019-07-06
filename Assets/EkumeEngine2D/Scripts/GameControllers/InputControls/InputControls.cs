using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InputControls : MonoBehaviour
{
    InputControlsManager inputControlsManager;

    static bool[] inputsDownPC;
    static bool[] inputsUpPC;
    static bool[] inputsPressedPC;

    //Joystick buttons
    static bool[] inputsDownJoystickButton;
    static bool[] inputsUpJoystickButton;
    static bool[] inputsPressedJoystickButton;

    // Joystick axis
    static bool[] inputsDownJoystickAxisPositive;
    static bool[] inputsUpJoystickAxisPositive;
    static bool[] inputsPressedJoystickAxisPositive;

    static bool[] inputsDownJoystickAxisNegative;
    static bool[] inputsUpJoystickAxisNegative;
    static bool[] inputsPressedJoystickAxisNegative;

    bool[] enterInDownJoystickAxisNegative;
    bool[] enterInDownJoystickAxisPositive;
    bool[] enterInUpJoystickAxisNegative;
    bool[] enterInUpJoystickAxisPositive;
    //---------------

    //Mobile controls in UI
    public static bool[] inputDownMobile;
    public static bool[] inputUpMobile;
    public static bool[] inputPressedMobile;

    //Mobile controls in UI (Virtual Joystick)
    public static bool[] inputDownVirtualJoystickXPositive;
    public static bool[] inputDownVirtualJoystickXNegative;
    public static bool[] inputDownVirtualJoystickYNegative;
    public static bool[] inputDownVirtualJoystickYPositive;

    public static bool[] inputUpVirtualJoystickXPositive;
    public static bool[] inputUpVirtualJoystickXNegative;
    public static bool[] inputUpVirtualJoystickYNegative;
    public static bool[] inputUpVirtualJoystickYPositive;

    public static bool[] inputPressedVirtualJoystickXPositive;
    public static bool[] inputPressedVirtualJoystickXNegative;
    public static bool[] inputPressedVirtualJoystickYNegative;
    public static bool[] inputPressedVirtualJoystickYPositive;

    // Swipe inputs
    static bool[] inputDownSwipeUp;
    static bool[] inputDownSwipeDown;
    static bool[] inputDownSwipeRight;
    static bool[] inputDownSwipeLeft;

    static bool[] inputUpSwipeUp;
    static bool[] inputUpSwipeDown;
    static bool[] inputUpSwipeRight;
    static bool[] inputUpSwipeLeft;

    static bool[] inputPressedSwipeUp;
    static bool[] inputPressedSwipeDown;
    static bool[] inputPressedSwipeRight;
    static bool[] inputPressedSwipeLeft;

    //Timers for keep pressed the "swipe" inputs
    static float timerPressedSwipeUp;
    static float timerPressedSwipeDown;
    static float timerPressedSwipeRight;
    static float timerPressedSwipeLeft;

    static bool startTimerPressedSwipeUp;
    static bool startTimerPressedSwipeDown;
    static bool startTimerPressedSwipeRight;
    static bool startTimerPressedSwipeLeft;

    //---------------

    //Click/Touch inputs

    public static bool[] inputDownClick;
    public static bool[] inputUpClick;
    public static bool[] inputPressedClick;

    //---------------

    Vector2 firstCursorPosition; // first finger position
    Vector2 secondCursorPosition; // last finger position
#if !UNITY_EDITOR && !UNITY_STANDALONE && !UNITY_WEBPLAYER && !UNITY_WEBGL
    bool swiped;
#endif

    int numberOfInputs;

    void OnEnable()
    {

        inputControlsManager = InputControlsManager.instance;
        numberOfInputs = inputControlsManager.inputNames.Count;

        inputsDownPC = new bool[numberOfInputs];
        inputsUpPC = new bool[numberOfInputs];
        inputsPressedPC = new bool[numberOfInputs];

        inputsDownJoystickButton = new bool[numberOfInputs];
        inputsUpJoystickButton = new bool[numberOfInputs];
        inputsPressedJoystickButton = new bool[numberOfInputs];

        inputsDownJoystickAxisPositive = new bool[numberOfInputs];
        inputsUpJoystickAxisPositive = new bool[numberOfInputs];
        inputsPressedJoystickAxisPositive = new bool[numberOfInputs];

        inputsDownJoystickAxisNegative = new bool[numberOfInputs];
        inputsUpJoystickAxisNegative = new bool[numberOfInputs];
        inputsPressedJoystickAxisNegative = new bool[numberOfInputs];

        enterInDownJoystickAxisNegative = new bool[numberOfInputs];
        enterInDownJoystickAxisPositive = new bool[numberOfInputs];
        enterInUpJoystickAxisNegative = new bool[numberOfInputs];
        enterInUpJoystickAxisPositive = new bool[numberOfInputs];

        inputDownMobile = new bool[numberOfInputs];
        inputUpMobile = new bool[numberOfInputs];
        inputPressedMobile = new bool[numberOfInputs];

        //Virtual joystick

        inputDownVirtualJoystickXPositive = new bool[numberOfInputs];
        inputDownVirtualJoystickXNegative = new bool[numberOfInputs];
        inputDownVirtualJoystickYNegative = new bool[numberOfInputs];
        inputDownVirtualJoystickYPositive = new bool[numberOfInputs];

        inputUpVirtualJoystickXPositive = new bool[numberOfInputs];
        inputUpVirtualJoystickXNegative = new bool[numberOfInputs];
        inputUpVirtualJoystickYNegative = new bool[numberOfInputs];
        inputUpVirtualJoystickYPositive = new bool[numberOfInputs];

        inputPressedVirtualJoystickXPositive = new bool[numberOfInputs];
        inputPressedVirtualJoystickXNegative = new bool[numberOfInputs];
        inputPressedVirtualJoystickYNegative = new bool[numberOfInputs];
        inputPressedVirtualJoystickYPositive = new bool[numberOfInputs];

        //Touch events

        inputDownSwipeUp = new bool[numberOfInputs];
        inputDownSwipeDown = new bool[numberOfInputs];
        inputDownSwipeRight = new bool[numberOfInputs];
        inputDownSwipeLeft = new bool[numberOfInputs];

        inputUpSwipeUp = new bool[numberOfInputs];
        inputUpSwipeDown = new bool[numberOfInputs];
        inputUpSwipeRight = new bool[numberOfInputs];
        inputUpSwipeLeft = new bool[numberOfInputs];

        inputPressedSwipeUp = new bool[numberOfInputs];
        inputPressedSwipeDown = new bool[numberOfInputs];
        inputPressedSwipeRight = new bool[numberOfInputs];
        inputPressedSwipeLeft = new bool[numberOfInputs];

        //Touch or click in screen
        inputDownClick = new bool[numberOfInputs];
        inputUpClick = new bool[numberOfInputs];
        inputPressedClick = new bool[numberOfInputs];

        if (FindObjectOfType<EventSystem>() == null)
        {
            Debug.LogError("The scene does not have an object with the component EventSystem. Maybe do you have the UI disabled?");
        }
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL
        if (inputControlsManager.useInPC)
        {
            PCInputDetection();
        }
#endif
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_PS3 || UNITY_PS4 || UNITY_XBOX360 || UNITY_XBOXONE || UNITY_WEBGL
        if (inputControlsManager.useInJoystick)
        {
            JoystickInputDetection();
        }
#endif
        if (inputControlsManager.useTouchOrClickEvents)
        {
            CursorEventDetection();
            TimersForKeepSwipe();
        }
    }

    void PCInputDetection()
    {
        for (int i = 0; i < numberOfInputs; i++)
        {
            if (Input.GetKeyDown(InputControlsManager.CustomKeyCodesPC[inputControlsManager.keyCodePC[i]]))
            {
                inputsDownPC[i] = true;
                inputsPressedPC[i] = true;
            }

            if (Input.GetKeyUp(InputControlsManager.CustomKeyCodesPC[inputControlsManager.keyCodePC[i]]))
            {
                inputsPressedPC[i] = false;
                inputsUpPC[i] = true;
            }
        }
    }

    void JoystickInputDetection()
    {   
        for (int i = 0; i < numberOfInputs; i++) //Count between all the names of the inputs
        {
            if (!inputControlsManager.useJoystickAxis[i]) // If is not checked the option "Use Axis"
            {
                if (Input.GetKeyDown(InputControlsManager.CustomKeyCodesJoystick[inputControlsManager.keyCodeJoystick[i]]))
                {
                    inputsDownJoystickButton[i] = true;
                    inputsPressedJoystickButton[i] = true;
                }

                if (Input.GetKeyUp(InputControlsManager.CustomKeyCodesJoystick[inputControlsManager.keyCodeJoystick[i]]))
                {
                    inputsPressedJoystickButton[i] = false;
                    inputsUpJoystickButton[i] = true;
                }
            }
            else
            {
                //If it is axis
                if (inputControlsManager.positiveAxisValue[i] && Input.GetAxis(InputControlsManager.CustomKeyCodesJoystickAxis[inputControlsManager.keyCodeJoystick[i]]) > 0)
                {
                    if (!enterInDownJoystickAxisPositive[i])
                    {
                        inputsDownJoystickAxisPositive[i] = true;

                        enterInDownJoystickAxisPositive[i] = true;

                        enterInUpJoystickAxisPositive[i] = false;
                    }

                    inputsPressedJoystickAxisPositive[i] = true;
                }
                else if (!inputControlsManager.positiveAxisValue[i] && Input.GetAxis(InputControlsManager.CustomKeyCodesJoystickAxis[inputControlsManager.keyCodeJoystick[i]]) < 0)
                {
                    if (!enterInDownJoystickAxisNegative[i])
                    {
                        inputsDownJoystickAxisNegative[i] = true;

                        enterInDownJoystickAxisNegative[i] = true;

                        enterInUpJoystickAxisNegative[i] = false;
                    }

                    inputsPressedJoystickAxisNegative[i] = true;
                }
                else
                {
                    if (inputsPressedJoystickAxisPositive[i])
                        inputsPressedJoystickAxisPositive[i] = false;

                    if (inputsPressedJoystickAxisNegative[i])
                        inputsPressedJoystickAxisNegative[i] = false;

                    if (inputsDownJoystickAxisPositive[i])
                        inputsDownJoystickAxisPositive[i] = false;

                    if (inputsDownJoystickAxisNegative[i])
                        inputsDownJoystickAxisNegative[i] = false;
                }
            }

            if ((inputControlsManager.positiveAxisValue[i]) && Input.GetAxis(InputControlsManager.CustomKeyCodesJoystickAxis[inputControlsManager.keyCodeJoystick[i]]) <= 0)
            {
                if (enterInDownJoystickAxisPositive[i])
                {
                    enterInDownJoystickAxisPositive[i] = false;
                    inputsUpJoystickAxisPositive[i] = true;

                    if (!enterInUpJoystickAxisPositive[i])
                    {
                        inputsUpJoystickAxisPositive[i] = true;
                        enterInUpJoystickAxisPositive[i] = true;
                    }
                }
            }

            if ((!inputControlsManager.positiveAxisValue[i]) && Input.GetAxis(InputControlsManager.CustomKeyCodesJoystickAxis[inputControlsManager.keyCodeJoystick[i]]) >= 0)
            {
                if (enterInDownJoystickAxisNegative[i])
                {
                    enterInDownJoystickAxisNegative[i] = false;

                    if (!enterInUpJoystickAxisNegative[i])
                    {
                        inputsUpJoystickAxisNegative[i] = true;
                        enterInUpJoystickAxisNegative[i] = true;
                    }
                }
            }
        }
    }

    void CursorEventDetection()
    {
        bool touchOrClickDown = false;
        bool touchOrClickUp = false;

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL
        touchOrClickDown = Input.GetMouseButtonDown(0);
        touchOrClickUp = Input.GetMouseButtonUp(0);
#else
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                touchOrClickDown = true;
            }
            else
            {
                touchOrClickDown = false;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                 touchOrClickUp = true;
            }
            else
            {
                touchOrClickUp = false;
            }
        }
#endif

        //Touch or click in screen
        if (touchOrClickDown)
        {
            PointerEventData cursor = new PointerEventData(EventSystem.current); // This section prepares a list for all objects hit with the raycast

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL
            cursor.position = Input.mousePosition;
#else
            cursor.position = Input.GetTouch(0).position;
#endif
            List<RaycastResult> objectsHit = new List<RaycastResult>();
            EventSystem.current.RaycastAll(cursor, objectsHit);

            if (objectsHit.Count == 0)
            {
                for (int i = 0; i < numberOfInputs; i++)
                {
                    if (inputControlsManager.keyCodeCursorEvent[i] == 1) // If  input selected is "Touch or click in screen" (1)
                    {
                        inputDownClick[i] = true;
                        inputPressedClick[i] = true;
                    }
                }
            }
        }
        else if (touchOrClickUp)
        {
            for (int i = 0; i < numberOfInputs; i++)
            {
                if (inputControlsManager.keyCodeCursorEvent[i] == 1) // If input selected is "Touch or click in screen" (1)
                {
                    inputUpClick[i] = true;

                    if (inputPressedClick[i])
                        inputPressedClick[i] = false;
                }
            }
        }

        //Swipe
#if !UNITY_EDITOR && !UNITY_STANDALONE && !UNITY_WEBPLAYER && !UNITY_WEBGL
        foreach (Touch touch in Input.touches)
        {
            if (!swiped)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    firstCursorPosition = touch.position;
                    secondCursorPosition = touch.position;
                }
                if (touch.phase == TouchPhase.Moved)
                {
                    secondCursorPosition = touch.position;
                    SwipeFunctions();
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                swiped = false;
            }
        }
#else
        if (Input.GetMouseButtonDown(0))
        {
            firstCursorPosition = Input.mousePosition;
            secondCursorPosition = Input.mousePosition;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            secondCursorPosition = Input.mousePosition;
            SwipeFunctions();
        }
#endif

    }

    void SwipeFunctions ()
    {
        if ((firstCursorPosition.x - secondCursorPosition.x) > 25) // left swipe
        {
            for (int i = 0; i < numberOfInputs; i++)
            {
                if (inputControlsManager.keyCodeCursorEvent[i] == 5) // If  input selected is "Swipe down" (5)
                {
                    inputDownSwipeLeft[i] = true; //It will be turned false in late update

                    inputPressedSwipeLeft[i] = true;
                    startTimerPressedSwipeLeft = true;
                }
            }
#if !UNITY_EDITOR && !UNITY_STANDALONE && !UNITY_WEBPLAYER && !UNITY_WEBGL
            swiped = true;
#endif
        }
        else if ((firstCursorPosition.x - secondCursorPosition.x) < -25) // right swipe
        {

            for (int i = 0; i < numberOfInputs; i++)
            {
                if (inputControlsManager.keyCodeCursorEvent[i] == 4) // If  input selected is "Swipe right" (4)
                {
                    inputDownSwipeRight[i] = true; //It will be turned false in late update

                    inputPressedSwipeRight[i] = true;
                    startTimerPressedSwipeRight = true;
                }
            }
#if !UNITY_EDITOR && !UNITY_STANDALONE && !UNITY_WEBPLAYER && !UNITY_WEBGL
            swiped = true;
#endif
        }
        else if ((firstCursorPosition.y - secondCursorPosition.y) < -25) // up swipe
        {

            for (int i = 0; i < numberOfInputs; i++)
            {
                if (inputControlsManager.keyCodeCursorEvent[i] == 2) // If  input selected is "Swipe up" (2)
                {
                    inputDownSwipeUp[i] = true; //It will be turned false in late update

                    inputPressedSwipeUp[i] = true;
                    startTimerPressedSwipeUp = true;
                }
            }
#if !UNITY_EDITOR && !UNITY_STANDALONE && !UNITY_WEBPLAYER && !UNITY_WEBGL
            swiped = true;
#endif
        }
        else if ((firstCursorPosition.y - secondCursorPosition.y) > 25) // down swipe
        {

            for (int i = 0; i < numberOfInputs; i++)
            {
                if (inputControlsManager.keyCodeCursorEvent[i] == 3) // If  input selected is "Swipe down" (3)
                {
                    inputDownSwipeDown[i] = true; //It will be turned false in late update

                    inputPressedSwipeDown[i] = true;
                    startTimerPressedSwipeDown = true;
                }
            }
#if !UNITY_EDITOR && !UNITY_STANDALONE && !UNITY_WEBPLAYER && !UNITY_WEBGL
            swiped = true;
#endif
        }
    }

    void TimersForKeepSwipe()
    {
        if (startTimerPressedSwipeLeft)
        {
            timerPressedSwipeLeft += Time.deltaTime;

            if (timerPressedSwipeLeft >= inputControlsManager.timeToKeepSwipeLeft)
            {
                for (int i = 0; i < numberOfInputs; i++)
                {
                    inputPressedSwipeLeft[i] = false;
                    inputUpSwipeLeft[i] = true;
                }
                timerPressedSwipeLeft = 0;
                startTimerPressedSwipeLeft = false;
            }
        }

        if (startTimerPressedSwipeRight)
        {
            timerPressedSwipeRight += Time.deltaTime;

            if (timerPressedSwipeRight >= inputControlsManager.timeToKeepSwipeRight)
            {
                for (int i = 0; i < numberOfInputs; i++)
                {
                    inputPressedSwipeRight[i] = false;
                    inputUpSwipeRight[i] = true;
                }
                timerPressedSwipeRight = 0;
                startTimerPressedSwipeRight = false;
            }
        }

        if (startTimerPressedSwipeUp)
        {
            timerPressedSwipeUp += Time.deltaTime;

            if (timerPressedSwipeUp >= inputControlsManager.timeToKeepSwipeUp)
            {
                for (int i = 0; i < numberOfInputs; i++)
                {
                    inputPressedSwipeUp[i] = false;
                    inputUpSwipeUp[i] = true;
                }
                timerPressedSwipeUp = 0;
                startTimerPressedSwipeUp = false;
            }
        }

        if (startTimerPressedSwipeDown)
        {
            timerPressedSwipeDown += Time.deltaTime;

            if (timerPressedSwipeDown >= inputControlsManager.timeToKeepSwipeDown)
            {
                for (int i = 0; i < numberOfInputs; i++)
                {
                    inputPressedSwipeDown[i] = false;
                    inputUpSwipeDown[i] = true;
                }
                timerPressedSwipeDown = 0;
                startTimerPressedSwipeDown = false;
            }
        }
    }

    void LateUpdate()
    {
        for (int i = 0; i < numberOfInputs; i++)
        {
            if (inputControlsManager.useInPC)
            {
                if (inputsDownPC[i])
                    inputsDownPC[i] = false;

                if (inputsUpPC[i])
                    inputsUpPC[i] = false;
            }

            if (inputControlsManager.useInJoystick)
            {
                if (inputsDownJoystickButton[i])
                    inputsDownJoystickButton[i] = false;

                if (inputsUpJoystickButton[i])
                    inputsUpJoystickButton[i] = false;

                if (inputsDownJoystickAxisPositive[i])
                    inputsDownJoystickAxisPositive[i] = false;

                if (inputsDownJoystickAxisNegative[i])
                    inputsDownJoystickAxisNegative[i] = false;

                if (inputsUpJoystickAxisPositive[i])
                    inputsUpJoystickAxisPositive[i] = false;

                if (inputsUpJoystickAxisNegative[i])
                    inputsUpJoystickAxisNegative[i] = false;
            }

            if (inputControlsManager.useControlsInUI)
            {
                if (inputUpMobile[i])
                    inputUpMobile[i] = false;

                if (inputDownMobile[i])
                    inputDownMobile[i] = false;

                //Joystick axis of mobile
                //X Axis
                if (inputDownVirtualJoystickXNegative[i])
                    inputDownVirtualJoystickXNegative[i] = false;

                if (inputDownVirtualJoystickXPositive[i])
                    inputDownVirtualJoystickXPositive[i] = false;

                if (inputUpVirtualJoystickXNegative[i])
                    inputUpVirtualJoystickXNegative[i] = false;

                if (inputUpVirtualJoystickXPositive[i])
                    inputUpVirtualJoystickXPositive[i] = false;

                //Y Axis
                if (inputDownVirtualJoystickYNegative[i])
                    inputDownVirtualJoystickYNegative[i] = false;

                if (inputDownVirtualJoystickYPositive[i])
                    inputDownVirtualJoystickYPositive[i] = false;

                if (inputUpVirtualJoystickYNegative[i])
                    inputUpVirtualJoystickYNegative[i] = false;

                if (inputUpVirtualJoystickYPositive[i])
                    inputUpVirtualJoystickYPositive[i] = false;

            }

            if(inputControlsManager.useTouchOrClickEvents)
            {
                //Swipe
                if (inputUpSwipeRight[i])
                    inputUpSwipeRight[i] = false;

                if (inputDownSwipeRight[i])
                    inputDownSwipeRight[i] = false;

                if (inputUpSwipeLeft[i])
                    inputUpSwipeLeft[i] = false;

                if (inputDownSwipeLeft[i])
                    inputDownSwipeLeft[i] = false;

                if (inputUpSwipeUp[i])
                    inputUpSwipeUp[i] = false;

                if (inputDownSwipeUp[i])
                    inputDownSwipeUp[i] = false;

                if (inputUpSwipeDown[i])
                    inputUpSwipeDown[i] = false;

                if (inputDownSwipeDown[i])
                    inputDownSwipeDown[i] = false;

                //Touch or click in screen

                if (inputDownClick[i])
                    inputDownClick[i] = false;

                if (inputUpClick[i])
                    inputUpClick[i] = false;
            }
        }
    }

    public static bool GetControlDown(int inputNumber)
    {
        return (inputsDownPC[inputNumber] || inputsDownJoystickButton[inputNumber]
               || inputsDownJoystickAxisPositive[inputNumber] || inputsDownJoystickAxisNegative[inputNumber]
               || inputDownMobile[inputNumber])
               || inputDownVirtualJoystickXNegative[inputNumber] || inputDownVirtualJoystickYNegative[inputNumber] || inputDownVirtualJoystickXPositive[inputNumber] || inputDownVirtualJoystickYPositive[inputNumber]
               || inputDownSwipeUp[inputNumber] || inputDownSwipeLeft[inputNumber] || inputDownSwipeRight[inputNumber] || inputDownSwipeDown[inputNumber]
               || inputDownClick[inputNumber]
            ? true : false;
    }

    public static bool GetControlUp(int inputNumber)
    {
        return (inputsUpPC[inputNumber] || inputsUpJoystickButton[inputNumber]
               || inputsUpJoystickAxisPositive[inputNumber] || inputsUpJoystickAxisNegative[inputNumber]
               || inputUpMobile[inputNumber])
               || inputUpVirtualJoystickXNegative[inputNumber] || inputUpVirtualJoystickYNegative[inputNumber] || inputUpVirtualJoystickXPositive[inputNumber] || inputUpVirtualJoystickYPositive[inputNumber]
               || inputUpSwipeUp[inputNumber] || inputUpSwipeLeft[inputNumber] || inputUpSwipeRight[inputNumber] || inputUpSwipeDown[inputNumber]
               || inputUpClick[inputNumber]
            ? true : false;
    }

    public static bool GetControl(int inputNumber)
    {
        return (inputsPressedPC[inputNumber] || inputsPressedJoystickButton[inputNumber]
               || inputsPressedJoystickAxisPositive[inputNumber] || inputsPressedJoystickAxisNegative[inputNumber])
               || inputPressedMobile[inputNumber]
               || inputPressedVirtualJoystickXNegative[inputNumber] || inputPressedVirtualJoystickYNegative[inputNumber] || inputPressedVirtualJoystickXPositive[inputNumber] || inputPressedVirtualJoystickYPositive[inputNumber]
               || inputPressedSwipeUp[inputNumber] || inputPressedSwipeLeft[inputNumber] || inputPressedSwipeRight[inputNumber] || inputPressedSwipeDown[inputNumber]
               || inputPressedClick[inputNumber]
            ? true : false;
    }
}