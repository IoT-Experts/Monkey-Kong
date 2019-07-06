using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputControlsManager : ScriptableObject
{
    public static string[] CustomKeyCodesPC = new string[]
    {
        "backspace", "tab", "clear", "return", "pause", "escape", "space", "exclaim", "double quote", "hash", "dollar", "ampersand", "quote", "left paren", "right paren", "asterisk", "plus", "comma", "minus", "period", "slash", "alpha 0", "alpha 1", "alpha 2", "alpha 3", "alpha 4", "alpha 5", "alpha 6", "alpha 7", "alpha 8", "alpha 9", "colon", "semicolon", "less", "equals", "greater", "question", "at", "left bracket", "backslash", "right bracket", "caret", "underscore", "back quote", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "delete", "keypad 0", "keypad 1", "keypad 2", "keypad 3", "keypad 4", "keypad 5", "keypad 6", "keypad 7", "keypad 8", "keypad 9", "keypad period", "keypad divide", "keypad multiply", "keypad minus", "keypad plus", "keypad enter", "keypad equals", "up", "down", "right", "left", "insert", "home", "end", "page up", "page down", "f1", "f2", "f3", "f4", "f5", "f6", "f7", "f8", "f9", "f10", "f11", "f12", "f13", "f14", "f15", "numlock", "caps lock", "scroll lock", "right shift", "left shift", "right control", "left control", "right alt", "left alt", "right command", "right apple", "left command", "left apple", "left windows", "right windows", "alt gr", "help", "print", "sys req", "break", "menu", "mouse 0", "mouse 1", "mouse 2", "mouse 3", "mouse 4", "mouse 5", "mouse 6"
    };

    public static string[] CustomKeyCodesJoystick = new string[]
    {
        "joystick button 0", "joystick button 1", "joystick button 2", "joystick button 3", "joystick button 4", "joystick button 5", "joystick button 6", "joystick button 7", "joystick button 8", "joystick button 9", "joystick button 10", "joystick button 11", "joystick button 12", "joystick button 13", "joystick button 14", "joystick button 15", "joystick button 16", "joystick button 17", "joystick button 18", "joystick button 19", "joystick 1 button 0", "joystick 1 button 1", "joystick 1 button 2", "joystick 1 button 3", "joystick 1 button 4", "joystick 1 button 5", "joystick 1 button 6", "joystick 1 button 7", "joystick 1 button 8", "joystick 1 button 9", "joystick 1 button 10", "joystick 1 button 11", "joystick 1 button 12", "joystick 1 button 13", "joystick 1 button 14", "joystick 1 button 15", "joystick 1 button 16", "joystick 1 button 17", "joystick 1 button 18", "joystick 1 button 19", "joystick 2 button 0", "joystick 2 button 1", "joystick 2 button 2", "joystick 2 button 3", "joystick 2 button 4", "joystick 2 button 5", "joystick 2 button 6", "joystick 2 button 7", "joystick 2 button 8", "joystick 2 button 9", "joystick 2 button 10", "joystick 2 button 11", "joystick 2 button 12", "joystick 2 button 13", "joystick 2 button 14", "joystick 2 button 15", "joystick 2 button 16", "joystick 2 button 17", "joystick 2 button 18", "joystick 2 button 19", "joystick 3 button 0", "joystick 3 button 1", "joystick 3 button 2", "joystick 3 button 3", "joystick 3 button 4", "joystick 3 button 5", "joystick 3 button 6", "joystick 3 button 7", "joystick 3 button 8", "joystick 3 button 9", "joystick 3 button 10", "joystick 3 button 11", "joystick 3 button 12", "joystick 3 button 13", "joystick 3 button 14", "joystick 3 button 15", "joystick 3 button 16", "joystick 3 button 17", "joystick 3 button 18", "joystick 3 button 19", "joystick 4 button 0", "joystick 4 button 1", "joystick 4 button 2", "joystick 4 button 3", "joystick 4 button 4", "joystick 4 button 5", "joystick 4 button 6", "joystick 4 button 7", "joystick 4 button 8", "joystick 4 button 9", "joystick 4 button 10", "joystick 4 button 11", "joystick 4 button 12", "joystick 4 button 13", "joystick 4 button 14", "joystick 4 button 15", "joystick 4 button 16", "joystick 4 button 17", "joystick 4 button 18", "joystick 4 button 19"
    };

    public static string[] CustomKeyCodesJoystickAxis = new string[]
    {
        "X axis", "Y axis", "3rd axis", "4th axis", "5th axis", "6th axis", "7th axis", "8th axis", "9th axis", "10th axis", "11th axis", "12th axis", "13th axis", "14th axis", "15th axis", "16th axis", "17th axis", "18th axis", "19th axis", "20th axis", "21st axis", "22nd axis", "23rd axis", "24th axis", "25th axis", "26th axis", "27th axis", "28th axis",
    };

    public static string[] CursorEvents = new string[]
    {
        "None", "Touch or click in screen", "Swipe up", "Swipe down", "Swipe right", "Swipe left"
    };

    public bool useInPC;
    public bool useControlsInUI;
    public bool useInJoystick;
    
    public List<string> inputNames = new List<string>();
    public List<int> keyCodePC = new List<int>();
    public List<int> keyCodeJoystick = new List<int>();

    public List<int> keyCodeCursorEvent = new List<int>();
    public float timeToKeepSwipeDown;
    public float timeToKeepSwipeUp;
    public float timeToKeepSwipeLeft;
    public float timeToKeepSwipeRight;

    //JoystickAxis
    public List<bool> useJoystickAxis = new List<bool>();
    public List<bool> positiveAxisValue = new List<bool>();

    //Mobile controls
    public string nameOfParentMobileControls;
    public bool useTouchOrClickEvents;
    

    static InputControlsManager reference;

    public static InputControlsManager instance
    {
        get
        {
            if (reference == null)
            {
                reference = (InputControlsManager)Resources.Load("Data/InputControlsManager", typeof(InputControlsManager));
                return reference;
            }
            else
                return reference;
        }
    }
}