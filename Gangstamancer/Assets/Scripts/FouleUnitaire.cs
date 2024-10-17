using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class FouleUnitaire : MonoBehaviour
{
    private static FouleUnitaire _instance;
    public static FouleUnitaire Instance => _instance;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    [SerializeField] List<GameObject> PrefabPersoFoule = new List<GameObject>();

    [SerializeField] int NBFouleStart = 5;
    [SerializeField] int MaxFoule = 10;


    [SerializeField] int FouleLeft = 0;
    [SerializeField] int FouleRight = 0;




    [SerializeField] List<Transform> posLeft;
    [SerializeField] List<Transform> posRight;

    [SerializeField] Transform behindLeft;
    [SerializeField] Transform behindRight;

    GameObject goBehindLeft;
    GameObject goBehindRight;

    // Start is called before the first frame update
    void Start()
    {
        FouleLeft = NBFouleStart;
        FouleRight = NBFouleStart;
        for (int i = 0; i < NBFouleStart; i++)
        {
            GameObject go = Instantiate(PrefabPersoFoule[0], posLeft[i].position, Quaternion.identity);
        }

        for (int i = 0; i < NBFouleStart; i++)
        {
            GameObject go = Instantiate(PrefabPersoFoule[0], posRight[i].position, Quaternion.identity); 
        }

        goBehindLeft = Instantiate(PrefabPersoFoule[0], behindLeft.position, Quaternion.identity);
        goBehindRight = Instantiate(PrefabPersoFoule[0], behindRight.position, Quaternion.identity);

    }


    [Button]
    public void AddLeftFan()
    {

        FouleLeft++;
        StartCoroutine(goToPos(goBehindLeft, posLeft[FouleLeft]));
        goBehindLeft = Instantiate(PrefabPersoFoule[0], behindLeft.position, Quaternion.identity);

        CheckCam();
    }

    [Button]
    public void AddRightFan()
    {

        FouleRight++;
        StartCoroutine(goToPos(goBehindRight, posRight[FouleRight]));
        goBehindRight = Instantiate(PrefabPersoFoule[0], behindRight.position, Quaternion.identity);

        CheckCam();
    }

    IEnumerator goToPos(GameObject go, Transform pos)
    {
        int i = 0;
        Vector3 startPos = go.transform.position;
        while(i< 50)
        {
            go.transform.position = Vector3.Lerp(startPos, pos.position, i / 50f);
            i++;
            yield return null;
        }
    }

    void CheckCam()
    {
        int diff = FouleLeft - FouleRight;
        if (Mathf.Abs(diff) >= 2)
        {
            StartCoroutine(goToRot(diff * 1.5f));
        }
        else
        {
            StartCoroutine(goToRot(0f));
        }
    }

    IEnumerator goToRot(float diff)
    {
        float oldAngle = Camera.main.transform.eulerAngles.z;
        int i = 0;
        while (i < 10)
        {
            Camera.main.transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(oldAngle, diff, i / 50f));
            i++;
            yield return null;
        }
    }
}
