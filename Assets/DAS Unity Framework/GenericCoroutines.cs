using System;
using System.Collections;
using DAS_Unity_Framework;
using UnityEngine;

public static class GenericCoroutines
{
    public delegate bool Condition();

    public static IEnumerator DoAfterCondition(Action action, Condition condition)
    {
        if (!condition())
        {
            yield return new WaitUntil(() => condition());
        }

        action();
    }

    public static IEnumerator DoAfterSeconds(Action action, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }

    public static IEnumerator DoAfterFrames(Action action, int numFrames)
    {
        int framesWaited = 0;
        while (framesWaited < numFrames)
        {
            yield return new WaitForEndOfFrame();
            framesWaited++;
        }

        action();
    }

    public static IEnumerator MoveAwayFrom(Transform objectToMove, Vector3 myCenterPoint, Vector3 awayFrom,  
        float distance, float seconds, InterpolationType interpolationType = InterpolationType.Linear, Action doAfter = null)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        Vector3 endPos = startingPos + (myCenterPoint - awayFrom).normalized * distance;
        while (elapsedTime < seconds)
        {
            objectToMove.position = Vector3.Lerp(startingPos, endPos, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.position = endPos;

        doAfter?.Invoke();
    }
    
    public static IEnumerator MoveAndRotateOverSeconds(GameObject objectToMove, Vector3 end, Quaternion endRot, float seconds, Action onFinish = null)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        Quaternion startingRot = objectToMove.transform.rotation;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            objectToMove.transform.rotation = Quaternion.Lerp(startingRot, endRot, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
        objectToMove.transform.rotation = endRot;

        if (onFinish != null)
            onFinish();
    }

}