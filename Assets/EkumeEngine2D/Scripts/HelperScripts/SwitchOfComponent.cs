using UnityEngine;
using System.Collections;
using EkumeEnumerations;
using System.Collections.Generic;

public class SwitchOfComponent : MonoBehaviour
{
    public enum ActionsForComponentEnum { Enable, Disable, Destroy }
    public enum TypeOfComponentEnum { Script, Collider2D, SpriteRenderer, OtherGameObject, ThisGameObject, otherComponentType, GameObjectThatCollides }

    public enum ActivatorOfAction { OnTriggerEnter2D, OnCollisionEnter2D, WhenTheObjectStart }
    public ActionsForComponentEnum actionToDo;
    public List<string> tagsThatActivate = new List<string>();
    public bool anyTag;
    public ActivatorOfAction activatorOfAction;
    public TypeOfComponentEnum typeOfComponent;
    public float secondsToActivateAction;
    public Behaviour behaviourComponent;
    public Component component;
    public GameObject gameObjectComp;
    public SpriteRenderer spriteRendererComp;
    GameObject collidedOject;

    //When the object start in the game (GameObject enable)
    void OnEnable()
    {
        if (activatorOfAction == ActivatorOfAction.WhenTheObjectStart)
            StartCoroutine(WaitForActionActivation());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (anyTag || tagsThatActivate.Contains(other.tag))
        {
            if (activatorOfAction == ActivatorOfAction.OnTriggerEnter2D)
            {
                StartCoroutine(WaitForActionActivation());
                collidedOject = other.gameObject;
            }
        }
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (anyTag || tagsThatActivate.Contains(other.collider.tag))
        {
            if (activatorOfAction == ActivatorOfAction.OnCollisionEnter2D)
            {
                StartCoroutine(WaitForActionActivation());
                collidedOject = other.gameObject;
            }
        }
    }

    IEnumerator WaitForActionActivation ()
    {
        yield return new WaitForSeconds(secondsToActivateAction);
        ActivateAction();
    }

    void ActivateAction()
    {
        switch (actionToDo)
        {
            case ActionsForComponentEnum.Destroy:
                switch (typeOfComponent)
                {
                    case TypeOfComponentEnum.Collider2D:
                        Destroy(behaviourComponent);
                        break;
                    case TypeOfComponentEnum.Script:
                        Destroy(behaviourComponent);
                        break;
                    case TypeOfComponentEnum.SpriteRenderer:
                        Destroy(spriteRendererComp);
                        break;
                    case TypeOfComponentEnum.otherComponentType:
                        Destroy(component);
                        break;
                    case TypeOfComponentEnum.ThisGameObject:
                        Destroy(this.gameObject);
                        break;
                    case TypeOfComponentEnum.OtherGameObject:
                        Destroy(gameObjectComp);
                        break;
                    case TypeOfComponentEnum.GameObjectThatCollides:
                        Destroy(collidedOject);
                        break;
                }
                break;
            case ActionsForComponentEnum.Disable:
                switch (typeOfComponent)
                {
                    case TypeOfComponentEnum.Collider2D:
                        behaviourComponent.enabled = false;
                        break;
                    case TypeOfComponentEnum.Script:
                        behaviourComponent.enabled = false;
                        break;
                    case TypeOfComponentEnum.SpriteRenderer:
                        spriteRendererComp.enabled = false;
                        break;
                    case TypeOfComponentEnum.otherComponentType:
                        Debug.LogWarning("You can't disable or enable the component attached in the SwitchOfComponent. You can destroy it but not disable it or enable it.");
                        break;
                    case TypeOfComponentEnum.ThisGameObject:
                        this.gameObject.SetActive(false);
                        break;
                    case TypeOfComponentEnum.OtherGameObject:
                        gameObjectComp.SetActive(false);
                        break;
                    case TypeOfComponentEnum.GameObjectThatCollides:
                        collidedOject.SetActive(false);
                        break;
                }
                break;
            case ActionsForComponentEnum.Enable:
                switch (typeOfComponent)
                {
                    case TypeOfComponentEnum.Collider2D:
                        behaviourComponent.enabled = true;
                        break;
                    case TypeOfComponentEnum.Script:
                        behaviourComponent.enabled = true;
                        break;
                    case TypeOfComponentEnum.SpriteRenderer:
                        spriteRendererComp.enabled = true;
                        break;
                    case TypeOfComponentEnum.otherComponentType:
                        Debug.LogWarning("You cannot disable or enable the component attached in the SwitchOfComponent. You can destroy it but not disable it or enable it.");
                        break;
                    case TypeOfComponentEnum.ThisGameObject:
                        this.gameObject.SetActive(true);
                        break;
                    case TypeOfComponentEnum.OtherGameObject:
                        gameObjectComp.SetActive(true);
                        break;
                    case TypeOfComponentEnum.GameObjectThatCollides:
                        collidedOject.SetActive(true);
                        break;
                }
                break;
        }
    }
}
