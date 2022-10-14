using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainDot : MonoBehaviour
{
    private Color color;
    [SerializeField]
    private Image img;
    [SerializeField]
    [Range(0.01f, 1f)]
    private float imgScale;

    [SerializeField]
    private float fadeRate = 0.9f;
    [SerializeField]
    private float targetAlpha = 0;

    private void Start()
    {
        color = new Color(img.color.r, img.color.g, img.color.b, 1);
        img.color = color;
        img.transform.localScale = new Vector2(imgScale, imgScale);
        Invoke("StartFade", 1f);
    }

    private void StartFade()
    {
        StartCoroutine(FadeDot());
    }
    
    private IEnumerator FadeDot()
    {
        while (Mathf.Abs(color.a - targetAlpha) > 0.01f)
        {
            color.a = Mathf.Lerp(color.a, targetAlpha, fadeRate * Time.deltaTime);
            img.color = color;
            yield return null;
        }
        Destroy(gameObject);
    }
}