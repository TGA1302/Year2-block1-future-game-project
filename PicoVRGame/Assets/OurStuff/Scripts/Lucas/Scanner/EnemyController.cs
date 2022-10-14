using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private bool spotted = false;
    public bool Spotted
    {
        get { return spotted; }
        set { spotted = value; }
    }


    [SerializeField]
    [Range(1f, 3f)]
    private float fadeRate;

    [SerializeField]
    private Image img;
    private Color color;
    private float targetAlpha = 0;

    public void SetDot()
    {
        color = new Color(img.color.r, img.color.g, img.color.b, 1);
        img.color = color;
        Invoke("StartFade", 2f);
    }

    private void StartFade()
    {
        spotted = false;
        StartCoroutine(FadeDot());
    }

    private IEnumerator FadeDot()
    {
        while (Mathf.Abs(color.a - targetAlpha) > 0.001f)
        {
            if (spotted)
                StopAllCoroutines();

            color.a = Mathf.Lerp(color.a, targetAlpha, fadeRate * Time.deltaTime);
            img.color = color;
            yield return null;
        }
        Debug.Log("Done Fading");
    }
}