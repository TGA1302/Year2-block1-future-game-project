using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AreaScan : MonoBehaviour
{
    private Engine engine;

    [SerializeField]
    [Range(10, 150)]
    private float ScanLength;

    [SerializeField]
    [Range(15, 150)]
    private float scanRotSpeed;
    public float CalculateRotSpeed
    {
        get { return (scanRotSpeed - engine.CalculateLowPressure) * Time.deltaTime; }
    }

    [SerializeField]
    private GameObject TerrainDot;


    [SerializeField]
    [Range(0.01f, 1f)]
    private float ignoreTerrainTime;
    private bool ignoring = false;

    private void Start()
    {
        engine = FindObjectOfType<Engine>();
    }

    private int frames;
    private void Update()
    {
        //frames++;
        //if (frames % 5 == 0)
        //{
        //    frames = 0;
        Raycasting();
        //}
        transform.Rotate(Vector3.up * CalculateRotSpeed);
    }

    private void Raycasting()
    {
        int layermask = (1 << 7);
        layermask |= (1 << 9);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, ScanLength, layermask))
        {
            switch (hit.collider.gameObject.layer)
            {
                case 7: // Hit Terrain
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.blue);
                    if (!ignoring)
                    {
                        ignoring = true;
                        DrawTerrainOBJs(hit.point);
                    }
                    break;

                case 9: // Hit Enemy
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                    if (!hit.collider.GetComponent<EnemyController>().Spotted)
                    {
                        hit.collider.GetComponent<EnemyController>().Spotted = true;
                        hit.collider.GetComponent<EnemyController>().SetDot();
                    }
                    break;

                default: // hit something else

                    break;
            }
        }
        else
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * ScanLength, Color.white);
    }

    //private Color color;
    //private float fadeSpeed

    private void DrawTerrainOBJs(Vector3 pos)
    {
        GameObject obj = Instantiate(TerrainDot, pos, TerrainDot.transform.rotation);
        StartCoroutine(IgnoreTime());

        //Destroy(obj, 3f);
    }
    private IEnumerator IgnoreTime()
    {
        yield return new WaitForSecondsRealtime(ignoreTerrainTime);
        ignoring = false;
    }
}