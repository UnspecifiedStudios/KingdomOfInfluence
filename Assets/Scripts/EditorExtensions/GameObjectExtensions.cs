using UnityEngine;

public static class GameObjectExtensions
{
    // does not work on GameObjects without any parent
    public static GameObject FindSiblingByName(this GameObject obj, string nameToFind)
    {
        Transform[] allSiblingTransforms = obj.transform.parent.GetComponentsInChildren<Transform>();

        foreach (Transform sibTransform in allSiblingTransforms)
        {   
            // if the obj is the name we're looking for and it's not the object that called it
            if (sibTransform.gameObject.name == nameToFind && sibTransform.gameObject.name != obj.name)
            {
                return sibTransform.gameObject;
            }
        }
        return null;
    }

    public static GameObject FindChildByName(this GameObject obj, string nameToFind)
    {
        Transform[] allChildrenTransforms = obj.GetComponentsInChildren<Transform>();

        foreach (Transform childTransform in allChildrenTransforms)
        {   
            // if the obj is the name we're looking for and it's not the object that called it
            if (childTransform.gameObject.name == nameToFind)
            {
                return childTransform.gameObject;
            }
        }
        return null;
    }
}
