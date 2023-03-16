using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    private CubePos nowCube = new CubePos(0,1,0);
    public float cubeChangePlaseSpeed = 0.5f;
    public Transform CubeToPlace;
    private float CamMoveToYPosition, CamMoveSpeed = 2f;

    public GameObject CubeToCreate, AllCubes;
    public GameObject[] CanvasStartPage;
    private Rigidbody AllCubesRB;

    public Color[] BGColors;
    
    
    private bool IsLose, FirstCube;
    private Color toCameraColor;
    


    public List<Vector3> AllCubesPositions = new List<Vector3>
    {
    };


    private int prevCountMaxGorizontal;
    private Coroutine showCubePlase;
    private Transform MainCam;




    private void Start()
    {
        toCameraColor = Camera.main.backgroundColor;
        MainCam = Camera.main.transform;
        CamMoveToYPosition = 5.9f + nowCube.y - 1f;

        AllCubesPositions.Add(new Vector3(0, 1, 0));
        AllCubesRB = AllCubes.GetComponent<Rigidbody>();
        showCubePlase = StartCoroutine(ShowCubePlase());
    }

    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && CubeToPlace != null && AllCubes != null && !EventSystem.current.IsPointerOverGameObject()) //Если не нажать на UI элемент
        {
#if !UNITY_EDITOR
            if (Input.GetTouch(0).phase != TouchPhase.Began) // если мы не только начали касаться экрана
            {
                return;
            }
#endif

            if (!FirstCube) // Удаляем UI объекты
            {
                FirstCube = true;
                foreach (GameObject buttom in CanvasStartPage)
                {
                    Destroy(buttom);
                }
            }

            GameObject newCube = Instantiate(
                CubeToCreate,
                CubeToPlace.position,
                Quaternion.identity) as GameObject;
            newCube.transform.SetParent(AllCubes.transform);
            nowCube.setVector(newCube.transform.position);
            AllCubesPositions.Add(nowCube.getVector());
            AllCubesRB.isKinematic = true;
            AllCubesRB.isKinematic = false;

            SpawnPositions();
            MoveCameraChangeBG();
        }


        if (!IsLose && AllCubesRB.velocity.magnitude > 0.1f) // колебания, скорость движения внутри RB
        {
            Destroy(CubeToPlace.gameObject);
            IsLose = true;

            StopCoroutine(showCubePlase);
            //  Camera.main.transform.position
            MainCam.localPosition -= new Vector3(0, 0, 3f); //  Camera.main.transform.position с этим почему то не работает
        }
        MainCam.localPosition = Vector3.MoveTowards(MainCam.localPosition, new Vector3(MainCam.localPosition.x, CamMoveToYPosition, MainCam.localPosition.z), CamMoveSpeed * Time.deltaTime);

        if (Camera.main.backgroundColor != toCameraColor)
        {
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, toCameraColor, Time.deltaTime / 1.5f);
        }
    }




    IEnumerator ShowCubePlase()
    {
        while (true)
        {
                SpawnPositions();
            yield return new WaitForSeconds(cubeChangePlaseSpeed);// Хуета которая создает задержку на cubeChangePlaseSpeed
        }
    }
    private void SpawnPositions()
    {
        List<Vector3> positions = new List<Vector3>();  // Динамический массив
        if (IsPositionEmply(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z)) 
            && nowCube.x+1 != CubeToPlace.position.x)
        {
            positions.Add(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z));
        }
        if (IsPositionEmply(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z))
            && nowCube.x - 1 != CubeToPlace.position.x)
        {
            positions.Add(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z));
        }
        if (IsPositionEmply(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z))
            && nowCube.y + 1 != CubeToPlace.position.y)
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z));
        }
        if (IsPositionEmply(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z))
            && nowCube.y -1 != CubeToPlace.position.y)
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z));
        }
        if (IsPositionEmply(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1))
            && nowCube.z + 1 != CubeToPlace.position.z)
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1));
        }
        if (IsPositionEmply(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1))
            && nowCube.z - 1 != CubeToPlace.position.z)
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1));
        }
        if (positions.Count > 1)
            CubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)];
        else if (positions.Count == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //  делает перезагрузку игры
            IsLose = true;
        }
        else CubeToPlace.position = positions[0];
    }
        private bool IsPositionEmply(Vector3 targetpos)
        {
            if (targetpos.y == 0 )
            {
              return false; 
            }
            foreach(Vector3 pos in AllCubesPositions)
            {
                if (pos.x == targetpos.x && pos.y == targetpos.y && pos.z == targetpos.z)
                {
                    return false;
                }
            }
            return true;
        }
    private void MoveCameraChangeBG()
    {
        int maxX = 0,maxY = 0,maxZ = 0, maxHor;
        foreach (Vector3 pos in AllCubesPositions)
        {
            if (Mathf.Abs(Convert.ToInt32(pos.x)) > maxX)
                maxX = Convert.ToInt32(pos.x);
            if (Convert.ToInt32(pos.y) > maxY)
                maxY = Convert.ToInt32(pos.y);
            if (Mathf.Abs(Convert.ToInt32(pos.z)) > maxZ)
                maxZ = Convert.ToInt32(pos.z);
        }
        CamMoveToYPosition = 5.9f + nowCube.y - 1f;
        maxHor = maxX > maxZ ? maxX: maxZ;
        if (maxHor % 3 == 0 && prevCountMaxGorizontal != maxHor)
        {
            MainCam.localPosition += new Vector3(0, 0, -3f);
            prevCountMaxGorizontal = maxHor;
        }
        switch (maxY)
        {
            case 2:
                toCameraColor = BGColors[0];
                break;
            case 5:
                toCameraColor = BGColors[1];
                break;
            case 10:
                toCameraColor = BGColors[2];
                break;
            case 20:
                toCameraColor = BGColors[3];
                break;
            default:
                break;
        }
        
    }

}
    struct CubePos
    {
        public int x, y, z;
        public CubePos(int x,int y,int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vector3 getVector()
        {
            return new Vector3(x, y, z);
        }
        public void setVector(Vector3 pos)
        {
            x = Convert.ToInt32(pos.x);
            y = Convert.ToInt32(pos.y);
            z = Convert.ToInt32(pos.z);
        }
    }

