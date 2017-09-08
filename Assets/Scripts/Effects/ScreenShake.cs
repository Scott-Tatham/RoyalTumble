using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake
{
    public static IEnumerator HardShake(float time, float range, Transform shakeObj)
    {
        float timer = 0;
        Vector3 initialPos = shakeObj.position;

        while (timer < time)
        {
            timer += Time.deltaTime;
            Vector3 point = range * Random.insideUnitCircle;
            shakeObj.position = initialPos + point;
            yield return new WaitForEndOfFrame();
        }

        shakeObj.position = initialPos;
    }

    public static IEnumerator HarshShake(float time, float range, float intensity, Transform shakeObj)
    {
        float timer = 0;
        Vector3 point= range * Random.insideUnitSphere;
        Vector3 initialPos = shakeObj.position;

        while (timer < time)
        {
            timer += Time.deltaTime;

            if (shakeObj.position != initialPos + point)
            {
                shakeObj.position = Vector3.MoveTowards(shakeObj.position, initialPos + point, intensity * Time.deltaTime);
            }

            else
            {
                point = range * Random.insideUnitCircle;
            }

            yield return new WaitForEndOfFrame();
        }

        while (shakeObj.position != initialPos)
        {
            shakeObj.position = Vector3.MoveTowards(shakeObj.position, initialPos, intensity * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public static IEnumerator SmoothShake(float time, float range, float intensity, Transform shakeObj)
    {
        float timer = 0;
        Vector3 point = range * Random.insideUnitSphere;
        Vector3 initialPos = shakeObj.position;

        while (timer < time)
        {
            timer += Time.deltaTime;

            if (shakeObj.position != initialPos + point)
            {
                shakeObj.position = Vector3.Lerp(shakeObj.position, initialPos + point, intensity * Time.deltaTime);
            }

            else
            {
                point = range * Random.insideUnitCircle;
            }

            yield return new WaitForEndOfFrame();
        }

        while (shakeObj.position != initialPos)
        {
            shakeObj.position = Vector3.Lerp(shakeObj.position, initialPos, intensity * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}