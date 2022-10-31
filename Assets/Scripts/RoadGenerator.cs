using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//public class RoadGenerator : SingletonBehaviour<RoadGenerator>

public class RoadGenerator : Singletone<RoadGenerator>
{
    //public static RoadGenerator Instance { get; private set; }

    public GameObject RoadPrefab;
    private List<GameObject> roads = new List<GameObject>();
    public float maxSpeed = 10;
    public float speed = 0;
    public int maxRoadCount = 5;

    //---------------------------------------
    public Transform startPos;
    int nextStep = 0;

    public GameObject PlanePrefab;////!!!!!!!!!!!!!!

    /// </summary>

    /* <summary>
    public int Generate()
    {
        /*int rand;
        Random random = Random(15);
        rand = random();
        
        //int min = Request.QueryString("min");
        //int max = Request.QueryString("max");

        //min = 1;
        //max = 15;

        int rand = Random.Range(0, 15);
        return rand;
        /*Random.Range rand = new Random.Range();
        int value = rand.Next(1,15);
        return value;
        


        /// </summary>
    }*/
    /*public void Awake()
    {
        InitializeSingleton();
    }*/
    //------------------------------------------
        /*
    private void Update()
    {

        //!!!!! idea, dont realisation, field in class!!!
        public Transform startPos;
        int nextStep = 0;
        public GameObject[] PrefabRoad;

    //------

        nextStep+= 15; //count plane Road
        Instantiate (PrefabRoad[Random.Range(0, PrefabRoad.Length)], 
            new Vector3(startPos.position.x,
            startPos.position.y, transform.position.z + nextStep), 
        Quaternion.identity);
    }
    */
    //------------------------------------------

    void Start()
    {
        
        PoolManager.Instance.Preload(RoadPrefab, 15);//create rive road awake


        ResetLevel();
        //StartLevel();//
    }

    
    void Update()
    {
        if (speed == 0) 
            return;

        foreach (GameObject road in roads)
        {
            road.transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
        }

        if(roads[0].transform.position.z < -15)
        {
            PoolManager.Instance.Despawn(roads[0]);//Destroy
            roads.RemoveAt(0);

            CreateNextRoad();
        }
    }
   

    private void CreateNextRoad()
    {
        Vector3 pos = Vector3.zero;

        if (roads.Count > 0) 
        {
            pos = roads[roads.Count - 1].transform.position + new Vector3(0, 0, 15);
        }

        /*nextStep += 15; //count plane Road
        Instantiate(roads[Random.Range(0, roads.Count)],
            new Vector3(startPos.position.x,
            startPos.position.y, transform.position.z + nextStep),
        Quaternion.identity);*/


       // Instantiate(RoadPrefab, new Vector3(Vector3.zero);



        //
        //
        /*if (roads.Count > Random.Range(2,15))
        {
            pos = roads[roads.Count - 1].transform.position + new Vector3(0, 0, 15);

            if (roads.Count > 0)
            {
                //pos = roads[roads.Count - 1].transform.position + new Vector3(0, 0, 15);
                Instantiate(PlanePrefab, transform, false);
                CreateNextRoad();
                
                //But.transform.SetParent(ObstacleFull);
            }*/
        //взято с примера, не вникать в суть
        //CreateNextRoad();
        /*
         (((private int a; //количество элементов надо задать
         private string PrefabName; //название кнопки надо задать
         public GameObject Parent; //Родительский объект на сцене, должен находиться в Canvas

        void Start() {
        for (int i = 0; i < a; i++) {
        PosX = 232f + i*60f; //тут сами подгоняйте это размер смещения
        GameObject But = Instantiate(Resources.Load<GameObject>(PrefabName), transform, false); //загружаем копию префаба из ресурсов.
        But.transform.SetParent(Parent); //Помещаем кнопку к родителю
        But.transform.localPosition = new Vector3(PosX, -25f, 0f); //смещаем кнопки в моем случае по Х
        int j = i + 1;
        But.name = j.ToString(); //Дополняем кнопки нумерацией, чтобы потом можно скажем через EventSystem получать имя нажатой кнопки.
                    But.GetComponentInChildren<Text>().text = j.ToString(); //меняем текст на кнопке)))
         */
        //}
        //pos = roads.transform.rotation.x += 90;


        //
        GameObject go = PoolManager.Instance.Spawn(RoadPrefab, pos, Quaternion.identity);//Instantiate
        go.transform.SetParent(transform);
        roads.Add(go);
    }

    public void StartLevel()
    {
       speed = maxSpeed;
       SwipeManager.Instance.enabled = true;//

    }

    public void ResetLevel()
    {
        speed = 0;
        while(roads.Count > 0)
        {
            Destroy(roads[0]);
            roads.RemoveAt(0);
        }
        for(int i = 0; i < maxRoadCount; i++)
        {
            CreateNextRoad();
        }
        SwipeManager.Instance.enabled = false;//
        MapGenerator.Instance.ResetMaps();///I
    }
}