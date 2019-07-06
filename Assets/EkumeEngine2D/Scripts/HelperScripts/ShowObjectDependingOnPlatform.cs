using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowObjectDependingOnPlatform : MonoBehaviour
{
    enum WhatToDo { Destroy, Disable, Enable}
    enum Condition { IfIsInTheList, IfIsNotInTheList }
    [Space()]
    [SerializeField] GameObject gameObjectTarget;
    [Header("Add the platforms that can see this object.")]
    [SerializeField] List<RuntimePlatform> platformToShow = new List<RuntimePlatform>();
    [Space()]
    [SerializeField] Condition condition;
    [SerializeField] WhatToDo whatToDo;

    void Awake()
    {
        if((condition == Condition.IfIsNotInTheList) ?
            !platformToShow.Contains(Application.platform) 
            : platformToShow.Contains(Application.platform))
        {
            if (whatToDo == WhatToDo.Destroy)
                Destroy(gameObjectTarget);
            else if (whatToDo == WhatToDo.Disable)
                gameObjectTarget.SetActive(false);
            else if (whatToDo == WhatToDo.Enable)
                gameObjectTarget.SetActive(true);
        }
    }
	
}
