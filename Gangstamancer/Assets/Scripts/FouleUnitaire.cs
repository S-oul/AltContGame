using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FouleUnitaire : MonoBehaviour
{
    private static FouleUnitaire _instance;
    public static FouleUnitaire Instance => _instance;

    public int FouleLeft { get => _fouleLeft; set => _fouleLeft = value; }
    public int FouleRight { get => _fouleRight; set => _fouleRight = value; }

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


    [SerializeField] int _fouleLeft = 0;
    [SerializeField] int _fouleRight = 0;


    [Range(0,15)]
    [SerializeField] float _CamMaxDegre = 0;

    [SerializeField] List<Transform> posLeft;
    [SerializeField] List<Transform> posRight;

    [SerializeField] Transform behindLeft;
    [SerializeField] Transform behindRight;

    GameObject goBehindLeft;
    GameObject goBehindRight;

    List<GameObject> _LastLeftJoined = new List<GameObject>();
    List<GameObject> _LastRightJoined = new List<GameObject>();
    // Start is called before the first frame update
    public void OnStart()
    {
        for (int i = 0; i < NBFouleStart; i++)
        {
            AddLeftFan();
            AddRightFan();
        }
    }


    [Button]
    public void AddLeftFan()
    {
        if (FouleLeft == MaxFoule) return;

        goBehindLeft = Instantiate(PrefabPersoFoule[0], behindLeft.position, Quaternion.identity);
        StartCoroutine(goToPos(goBehindLeft, posLeft[FouleLeft], false));
        FouleLeft++;
        _LastLeftJoined.Add(goBehindLeft);

        CheckCam();
    }

    [Button]
    public void AddRightFan()
    {
        if (FouleRight == MaxFoule) return;

        goBehindRight = Instantiate(PrefabPersoFoule[1], behindRight.position, Quaternion.identity);
        StartCoroutine(goToPos(goBehindRight, posRight[FouleRight], false));
        FouleRight++;
        _LastRightJoined.Add(goBehindRight);

        CheckCam();
    }
    [Button]
    public void RemoveLeftFan()
    {
        if (FouleLeft == 0) return;

        FouleLeft--;
        StartCoroutine(goToPos(_LastLeftJoined[_LastLeftJoined.Count - 1], behindLeft, true));
        _LastLeftJoined.Remove(goBehindLeft);
        if (FouleLeft > 0) goBehindLeft = _LastLeftJoined[_LastLeftJoined.Count - 1];

        CheckCam();
    }
    [Button]
    public void RemoveRightFan()
    {
        if (FouleRight == 0) return;
        FouleRight--;
        StartCoroutine(goToPos(_LastRightJoined[_LastRightJoined.Count - 1], behindRight, true));
        _LastRightJoined.Remove(goBehindRight);
        if (FouleRight > 0) goBehindRight = _LastRightJoined[_LastRightJoined.Count - 1];

        CheckCam();
    }
    IEnumerator goToPos(GameObject go, Transform pos, bool destroy)
    {
        int i = 0;
        Vector3 startPos = go.transform.position;
        while (i < 50)
        {
            go.transform.position = Vector3.Lerp(startPos, pos.position, i / 50f);
            i++;
            yield return null;
        }
        if (destroy) Destroy(go);
    }

    void CheckCam()
    {
        int diff = FouleLeft - FouleRight;
        if (Mathf.Abs(diff) >= 2)
        {
            //StartCoroutine(goToRot(diff * (_CamMaxDegre / 10f)));
            Camera.main.transform.eulerAngles = new Vector3(0,0,diff * (_CamMaxDegre / 10f));
        }
        else
        {
            Camera.main.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    IEnumerator goToRot(float angle)
    {
        float oldAngle = Camera.main.transform.eulerAngles.z;
        int i = 0;
        while (i < 10)
        {
            Camera.main.transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(oldAngle, angle, i / 50f));
            i++;
            yield return null;
        }
    }
}
