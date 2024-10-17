using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FouleUnitaire : MonoBehaviour
{
    private static FouleUnitaire _instance;
    public static FouleUnitaire Instance => _instance;


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
    void AddLeftFan()
    {

        FouleLeft++;
    }

    [Button]
    void AddRightFan()
    {

    }
}
