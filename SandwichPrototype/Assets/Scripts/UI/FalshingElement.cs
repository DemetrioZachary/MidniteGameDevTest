using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FalshingElement : MonoBehaviour
{
    [SerializeField] Graphic targetGraphic;
    [SerializeField] Color color = Color.yellow;
    [SerializeField] AnimationCurve intensityCurve;
    [SerializeField] float loopDuration = 1f;

    bool active;
    public bool Active
    {
        set
        {
            active = value;
            if (active)
            {
                StopAllCoroutines();
                StartCoroutine(Flashing());
            }
        }
    }

    IEnumerator Flashing()
    {
        float time = 0f;
        Color originalColor = targetGraphic.color;

        while (active)
        {
            float t = time / loopDuration;
            targetGraphic.color = Color.Lerp(originalColor, color, intensityCurve.Evaluate(t));
            yield return null;
            time += Time.deltaTime;
            if (time >= loopDuration)
            {
                time -= loopDuration;
            }
        }

        targetGraphic.color = originalColor;
    }
}
