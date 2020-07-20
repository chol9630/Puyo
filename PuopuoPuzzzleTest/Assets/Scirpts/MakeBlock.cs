using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class StringInt : SerializableDictionary<int, string> { };

public class MakeBlock : MonoBehaviour
{

    public Transform OptionButton;
    private bool IsOption = false;
     
    public Text TScore; //스코어점수판
    private int Score; //스코어점수

    public AudioClip EndSound;

    private Quaternion oldRot1; //뿌요 자식의 회전값1
    private Quaternion oldRot2;//뿌요 자식의 회전값2


    public AudioClip OneCombo;
    public AudioClip TwoCombo;
    public AudioClip threeCombo;
    public AudioClip FourCombo;
    public AudioClip FiveCombo;
    public AudioClip SixCombo;
    public AudioClip SevenCombo;


    public AudioClip OneMap;
    public AudioClip TwoMap;

    public AudioClip MoveSound;



    private AudioSource AudioSource;
    private AudioSource MapSource;
    private AudioSource PuyoMoveSource;

    public Transform pyou; //내려갈 뿌요
    public Transform MapTile; //맵그림
    public Transform Borad;   //맵에 맞춰서 뿌요 저장할 공간
    public Transform NextPyous;

    public Transform Ghostpyou;


    public int Xsize;
    public int Ysize;

    public GameObject PrefabTile;

    public bool GameOver = false;
    public Text GameOverTimeText;
    public Text GameOverText;
    public GameObject GameOverPanel;
    public float GameOverTime = 10f;

    private int map;
    private int halfXsize;
    private int halfYsize;

    private float ModenTime; //현재시간
    private float DownTime = 1f; //내려갈시간


    private bool IsStart; //시작을 햇는지 체크
    private int rand; //랜덤값
    private int NextPyou; //다음블럭 미리 생성
  
    private int Combo = 0;//콤보
    private float ComboTime;
    private float NextTime = 2f;

   
    private bool isRotate;
    private Vector3 moveDir;
  

    

    private StringInt YyRed = new StringInt();
    private StringInt YyBlue = new StringInt();
    private StringInt YyGreen = new StringInt();
    private StringInt YyYellow = new StringInt();


    private StringInt YyUp = new StringInt(); //삭제할떄 위 블럭
    private StringInt YyDown = new StringInt(); //삭제할떄 아래 블럭

    private bool isBroken = false; // 부셔진게있으면 true

    //string 리스트 정렬해주기위함
    public int compare(string x, string y)
    {
        return x.CompareTo(y);
    }
    //int 리스트 정렬해주기위함
    public int compareI(int x, int y)
    {
        return x.CompareTo(y);
    }

    IEnumerator GameOverTextTime()
    {
       
        while (true)
        {
            

            GameOverTimeText.text = "";   //아무글자도 안뛰워주고 
            GameOverText.text = "";
            yield return new WaitForSeconds(1f);
            GameOverTimeText.text = GameOverTime.ToString();   //글자를 뛰워주면 
            GameOverText.text = "Press Any Key ReStart";
            yield return new WaitForSeconds(1f);


            if (GameOver == true)
            {
                GameOverTime -= 1f;
            }
           

            if(GameOverTime ==-1)
            {
               SceneManager.LoadScene("Tile");
            }

        }
    }

    private void Awake()
    {

        map = MapSection.map;
       if(map ==1)
        {
           
        }
       else if(map ==2)
        {
            MapTile.GetChild(0).gameObject.SetActive(false);
            MapTile.GetChild(1).gameObject.SetActive(true);
        }
    }


    // Start is called before the first frame update
    void Start()
    {


        AudioSource = GetComponent<AudioSource>();

        GameOverTime = 10f;
        halfXsize = Mathf.RoundToInt(Xsize * 0.5f);
        halfYsize = Mathf.RoundToInt(Ysize * 0.5f);

        GameOver = false;
        GameOverPanel.SetActive(false);

        GameOverTimeText = GameOverPanel.transform.Find("Time").GetComponent<Text>();
        GameOverText  = GameOverPanel.transform.Find("GameOver").GetComponent<Text>();

        //Score = GetComponent<Text>();
        TScore.text = "0";
        Score = 0;
        MapSource  = MapTile.gameObject.GetComponentInChildren<AudioSource>();
        PuyoMoveSource = pyou.gameObject.GetComponentInChildren<AudioSource>();



        StartCoroutine(GameOverTextTime());
        
        IsStart = false;

        //위치에 따른 뿌요넣어주기위함
        for (int i = 0; i < Ysize; ++i)
        {
            GameObject col = new GameObject(i.ToString());
            col.transform.position = new Vector3(0, i, 0);
            col.transform.parent = Borad;
        }
        MapSelection();
    }


    //재시작
    private void ReStart()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    }

    //맵선택에 따른 사운드
    private void MapSelection()
    {
        if (map == 1)
        {
            if (!MapSource.isPlaying)
            {
                MapSource.PlayOneShot(OneMap);
            }
        }
        else if (map == 2)
        {
            if (!MapSource.isPlaying)
            {
                MapSource.PlayOneShot(TwoMap);
            }
        }

    }

    //콤보사운드
    public void ComboSonud()
    {
        if (Combo == 1)
        {
            AudioSource.clip = OneCombo;
           
            TScore.text = Score.ToString();
            if (!AudioSource.isPlaying)
            {
                Score += 10;
                AudioSource.Play();

            }
        }
        else if (Combo == 2)
        {
           
            TScore.text = Score.ToString();
            AudioSource.clip = TwoCombo;
            if (!AudioSource.isPlaying)
            {
                Score += 20;
                AudioSource.Play();
            }
        }
        else if (Combo == 3)
        {
            
            TScore.text = Score.ToString();
            AudioSource.clip = threeCombo;
            if (!AudioSource.isPlaying)
            {
                Score += 30;
                AudioSource.Play();
            }
        }

        else if (Combo == 4)
        {
          
            TScore.text = Score.ToString();
            AudioSource.clip = FourCombo;
            if (!AudioSource.isPlaying)
            {
                Score += 40;
                AudioSource.Play();
            }
        }
        else if (Combo == 5)
        {
           
            TScore.text = Score.ToString();
            AudioSource.clip = FiveCombo;
            if (!AudioSource.isPlaying)
            {
                Score += 50;
                AudioSource.Play();
            }
        }
        else if (Combo == 6)
        {
            
            TScore.text = Score.ToString();
            AudioSource.clip = SixCombo;
            if (!AudioSource.isPlaying)
            {
                Score += 60;
                AudioSource.Play();
            }
        }
        else if(Combo >6)
        {
          
            TScore.text = Score.ToString();
            AudioSource.clip = SevenCombo;
            if (!AudioSource.isPlaying)
            {
                Score += 100;
                AudioSource.Play();
            }
        }


    }



    public void Option()
    {
        if (IsOption == false)
        {
            IsOption = true;
            Time.timeScale = 0f;
            OptionButton.GetChild(0).gameObject.SetActive(true);
            OptionButton.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            IsOption = false;
            Time.timeScale = 1f;
            OptionButton.GetChild(0).gameObject.SetActive(false);
            OptionButton.GetChild(1).gameObject.SetActive(false);
        }

    }

    //오른쪽움직임
    public void MoveRight()
    {
        

        //moveDir = Vector3.zero;
        moveDir.x = 1;
        isRotate = false;
        if (moveDir != Vector3.zero || isRotate)
        {

            MovePyous(moveDir, isRotate);
        }
        //moveDir = Vector3.zero;
       
    }
    //왼쪽움직임
    public void MoveLeft()
    {
        isRotate = false;
        moveDir.x = -1;
        if (moveDir != Vector3.zero || isRotate)
        {
            
            MovePyous(moveDir, isRotate);
        }
        //moveDir = Vector3.zero;
      
    }

    //밑으로 내리기
    public void MoveDown()
    {
        while (MovePyous(Vector3.down, false))
        {
        }
    }
    

    //버튼회전
    public void MoveRotation()
    {

        if (!PuyoMoveSource.isPlaying)
        {
            PuyoMoveSource.PlayOneShot(MoveSound);
        }

        moveDir = Vector3.zero;
        isRotate = true;
        if (moveDir != Vector3.zero || isRotate)
        {

            MovePyous(moveDir, isRotate);
        }
    }


    private void NextPyouPyou()
    {

        switch (NextPyou)
        {
            case 1: //레드 레드 
                ObjectPool.Instance.SpawnPool("Red", new Vector3(4f, 9f, 0f), Quaternion.identity).transform.parent = NextPyous;
                ObjectPool.Instance.SpawnPool("Red", new Vector3(4f, 8f, 0f), Quaternion.identity).transform.parent  = NextPyous;
                IsStart = true;
                break;
            case 2://레드 블루 
                ObjectPool.Instance.SpawnPool("Red", new Vector3(4f, 9f, 0f), Quaternion.identity).transform.parent = NextPyous;
                ObjectPool.Instance.SpawnPool("Blue", new Vector3(4f, 8f, 0f), Quaternion.identity).transform.parent = NextPyous;
                IsStart = true;
                break;
            case 3: //레드 그린
                ObjectPool.Instance.SpawnPool("Red", new Vector3(4f, 9f, 0f), Quaternion.identity).transform.parent = NextPyous;
                ObjectPool.Instance.SpawnPool("Green", new Vector3(4f, 8f, 0f), Quaternion.identity).transform.parent = NextPyous;
                IsStart = true;
                break;
            case 4: //레드 옐로우
                ObjectPool.Instance.SpawnPool("Red", new Vector3(4f, 9f, 0f), Quaternion.identity).transform.parent = NextPyous;
                ObjectPool.Instance.SpawnPool("Yellow", new Vector3(4f, 8f, 0f), Quaternion.identity).transform.parent = NextPyous;
                IsStart = true;
                break;
            case 5://블루 레드
                ObjectPool.Instance.SpawnPool("Blue", new Vector3(4f, 9f, 0f), Quaternion.identity).transform.parent = NextPyous;
                ObjectPool.Instance.SpawnPool("Red", new Vector3(4f, 8f, 0f), Quaternion.identity).transform.parent = NextPyous;
                IsStart = true;
                break;
            case 6://블루 블루
                ObjectPool.Instance.SpawnPool("Blue", new Vector3(4f, 9f, 0f), Quaternion.identity).transform.parent = NextPyous;
                ObjectPool.Instance.SpawnPool("Blue", new Vector3(4f, 8f, 0f), Quaternion.identity).transform.parent = NextPyous;
                IsStart = true;
                break;
            case 7://블루 그린
                ObjectPool.Instance.SpawnPool("Blue", new Vector3(4f,9f, 0f), Quaternion.identity).transform.parent = NextPyous;
                ObjectPool.Instance.SpawnPool("Green", new Vector3(4f, 8f, 0f), Quaternion.identity).transform.parent = NextPyous;
                IsStart = true;
                break;
            case 8://블루 옐로우
                ObjectPool.Instance.SpawnPool("Blue", new Vector3(4f, 9f, 0f), Quaternion.identity).transform.parent = NextPyous;
                ObjectPool.Instance.SpawnPool("Yellow", new Vector3(4f, 8f, 0f), Quaternion.identity).transform.parent = NextPyous;
                IsStart = true;
                break;
            case 9: //그린 레드
                ObjectPool.Instance.SpawnPool("Green", new Vector3(4f, 9f, 0f), Quaternion.identity).transform.parent = NextPyous;
                ObjectPool.Instance.SpawnPool("Red", new Vector3(4f, 8f, 0f), Quaternion.identity).transform.parent = NextPyous;
                IsStart = true;
                break;
            case 10: // 그린 블루
                ObjectPool.Instance.SpawnPool("Green", new Vector3(4f, 9f, 0f), Quaternion.identity).transform.parent = NextPyous;
                ObjectPool.Instance.SpawnPool("Blue", new Vector3(4f, 8f, 0f), Quaternion.identity).transform.parent = NextPyous;
                IsStart = true;
                break;
            case 11: // 그린 그린
                ObjectPool.Instance.SpawnPool("Green", new Vector3(4f, 9f, 0f), Quaternion.identity).transform.parent = NextPyous;
                ObjectPool.Instance.SpawnPool("Green", new Vector3(4f, 8f, 0f), Quaternion.identity).transform.parent = NextPyous;
                IsStart = true;
                break;
            case 12: // 그린 옐로우
                ObjectPool.Instance.SpawnPool("Green", new Vector3(4f,9f, 0f), Quaternion.identity).transform.parent = NextPyous;
                ObjectPool.Instance.SpawnPool("Yellow", new Vector3(4f, 8f, 0f), Quaternion.identity).transform.parent = NextPyous;
                IsStart = true;
                break;
            case 13: //옐로우 레드
                ObjectPool.Instance.SpawnPool("Yellow", new Vector3(4f, 9f, 0f), Quaternion.identity).transform.parent = NextPyous;
                ObjectPool.Instance.SpawnPool("Green", new Vector3(4f, 8f, 0f), Quaternion.identity).transform.parent = NextPyous;
                IsStart = true;
                break;
            case 14: // 옐로우 블루
                ObjectPool.Instance.SpawnPool("Yellow", new Vector3(4f, 9f, 0f), Quaternion.identity).transform.parent = NextPyous;
                ObjectPool.Instance.SpawnPool("Blue", new Vector3(4f, 8f, 0f), Quaternion.identity).transform.parent = NextPyous;
                IsStart = true;
                break;
            case 15: // 옐로우 그린
                ObjectPool.Instance.SpawnPool("Yellow", new Vector3(4f, 9f, 0f), Quaternion.identity).transform.parent = NextPyous;
                ObjectPool.Instance.SpawnPool("Green", new Vector3(4f, 8f, 0f), Quaternion.identity).transform.parent = NextPyous;
                IsStart = true;
                break;
            case 16: // 옐로우 옐로우
                ObjectPool.Instance.SpawnPool("Yellow", new Vector3(4f, 9f, 0f), Quaternion.identity).transform.parent = NextPyous;
                ObjectPool.Instance.SpawnPool("Yellow", new Vector3(4f, 8f, 0f), Quaternion.identity).transform.parent = NextPyous;
                IsStart = true;
                break;

        }
    }

    //뿌요만들기
    private void CreatePyouPyou()
    {

        if (IsStart == false)
        {
            rand = Random.Range(1, 17);
            //rand = 1;
        }
        else
        {
            rand = NextPyou;
        }
        NextPyou = Random.Range(1, 17);
        //NextPyou = 1;


        switch (rand)
        {
            case 1: //레드 레드 
                ObjectPool.Instance.SpawnPool("Red", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("Red", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("GhostRed", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                ObjectPool.Instance.SpawnPool("GhostRed", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                IsStart = true;
                break;
            case 2://레드 블루 
                ObjectPool.Instance.SpawnPool("Red", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("Blue", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("GhostRed", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                ObjectPool.Instance.SpawnPool("GhostBlue", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                IsStart = true;
                break;
            case 3: //레드 그린
                ObjectPool.Instance.SpawnPool("Red", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("Green", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("GhostRed", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                ObjectPool.Instance.SpawnPool("GhostGreen", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                IsStart = true;
                break;
            case 4: //레드 옐로우
                ObjectPool.Instance.SpawnPool("Red", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("Yellow", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("GhostRed", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                ObjectPool.Instance.SpawnPool("GhostYellow", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                IsStart = true;
                break;
            case 5://블루 레드
                ObjectPool.Instance.SpawnPool("Blue", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("Red", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("GhostBlue", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                ObjectPool.Instance.SpawnPool("GhostRed", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                IsStart = true;
                break;
            case 6://블루 블루
                ObjectPool.Instance.SpawnPool("Blue", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("Blue", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("GhostBlue", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                ObjectPool.Instance.SpawnPool("GhostBlue", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                IsStart = true;
                break;
            case 7://블루 그린
                ObjectPool.Instance.SpawnPool("Blue", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("Green", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("GhostBlue", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                ObjectPool.Instance.SpawnPool("GhostGreen", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                IsStart = true;
                break;
            case 8://블루 옐로우
                ObjectPool.Instance.SpawnPool("Blue", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("Yellow", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("GhostBlue", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                ObjectPool.Instance.SpawnPool("GhostYellow", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                IsStart = true;
                break;
            case 9: //그린 레드
                ObjectPool.Instance.SpawnPool("Green", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("Red", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("GhostGreen", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                ObjectPool.Instance.SpawnPool("GhostRed", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                IsStart = true;              
                break;
            case 10: // 그린 블루
                ObjectPool.Instance.SpawnPool("Green", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("Blue", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("GhostGreen", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                ObjectPool.Instance.SpawnPool("GhostBlue", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                IsStart = true;
                break;
            case 11: // 그린 그린
                ObjectPool.Instance.SpawnPool("Green", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("Green", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("GhostGreen", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent =Ghostpyou;
                ObjectPool.Instance.SpawnPool("GhostGreen", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                IsStart = true;
                break;
            case 12: // 그린 옐로우
                ObjectPool.Instance.SpawnPool("Green", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("Yellow", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("GhostGreen", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                ObjectPool.Instance.SpawnPool("GhostYellow", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                IsStart = true;
                break;
            case 13: //옐로우 레드
                ObjectPool.Instance.SpawnPool("Yellow", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("Green", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("GhostYellow", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                ObjectPool.Instance.SpawnPool("GhostGreen", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                IsStart = true;
                break;
            case 14: // 옐로우 블루
                ObjectPool.Instance.SpawnPool("Yellow", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("Blue", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("GhostYellow", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                ObjectPool.Instance.SpawnPool("GhostBlue", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                IsStart = true;
                break;
            case 15: // 옐로우 그린
                ObjectPool.Instance.SpawnPool("Yellow", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("Green", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("GhostYellow", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                ObjectPool.Instance.SpawnPool("GhostGreen", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                IsStart = true;
                break;
            case 16: // 옐로우 옐로우
                ObjectPool.Instance.SpawnPool("Yellow", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("Yellow", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = pyou;
                ObjectPool.Instance.SpawnPool("GhostYellow", new Vector3(0.5f, 10f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                ObjectPool.Instance.SpawnPool("GhostYellow", new Vector3(0.5f, 9f, 0f), Quaternion.identity).transform.parent = Ghostpyou;
                IsStart = true;
                break;

        }



    }
    // Update is called once per frame
    void Update()
    {

        
        //처음 시작이면
        if (IsStart == false)
        {
            Time.timeScale = 1f;
            GameOverTime = 10f;
            CreatePyouPyou();
            NextPyouPyou();
           
        }

        
       

        //맵선택한것 맵보이기
        



        if (GameOver==true)
        {
            if (!AudioSource.isPlaying)
            {
                AudioSource.PlayOneShot(EndSound);
            }

            MapSource.Stop();

            for (int i = 0; i < pyou.childCount; i++)
            {
                pyou.GetChild(i).gameObject.SetActive(false);
                if (pyou.GetChild(i).gameObject.activeInHierarchy == false)
                {
                    pyou.GetChild(i).parent = null;

                    i = -1;
                    //xxsize.transform.DetachChildren();
                }
            }
            GameOverPanel.SetActive(true);


            if (Input.anyKeyDown && !Input.GetKey(KeyCode.Escape)  && GameOverTime >= 0f && GameOverTime <10f)
            {
               
                ReStart();

                
               
            }

            else if(Input.GetKey(KeyCode.Escape))
            {
#if UNITY_EDITOR  //유니티 에디터에서 종료
                UnityEditor.EditorApplication.isPlaying = false;
#else  //에디터가 아닌 PC,모바일 빌드 상태다

        Application.Quit();
#endif
            }

        }


        else if (GameOver == false)
        {
            MovePyou();
        

            if (Combo > 0)
            {
                
                ComboTime += 5f * Time.deltaTime;
                CheckPyou();


            }
            else if (Combo == -1)
            {

                BrekenDown();
                pyou.position = new Vector3(0.5f, 9f, 0f); //초기화 해줘야 다시 회전가능
                pyou.rotation = Quaternion.identity;
                Ghostpyou.position = new Vector3(0.5f, 9f, 0f); //초기화 해줘야 다시 회전가능
                Ghostpyou.rotation = Quaternion.identity;
                CreatePyouPyou();
                NextPyouPyou();
              
                Combo = 0;
            }
           
        }

    }
    //뿌요움직임
    private void MovePyou()
    {
        ModenTime += Time.deltaTime;

        Vector3 moveDir = Vector3.zero;
        moveDir = Vector3.zero;
        bool isRotate = false;

        



        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDir.x = -1;
           

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDir.x = 1;
          
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            isRotate = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDir.y = -1;
          
        }

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            while (MovePyous(Vector3.down, false))
            {
            }
        }
        
        if (ModenTime > DownTime)
        {
            ModenTime = 0f;
            moveDir = Vector3.down;
            isRotate = false;
        }

        if (moveDir != Vector3.zero || isRotate)
        {
            

            MovePyous(moveDir, isRotate);
            
            
        }


    }
    

  

     

    private bool MovePyous(Vector3 moveDir, bool isRotate)
    {
        Vector3 oldPos = pyou.position; //회전이 안되면 다시 돌려보내주기위함
        Quaternion oldRot = pyou.rotation;//회전이 안되면 다시 돌려보내주기위함

        Vector3 GoldPos = Ghostpyou.position; //회전이 안되면 다시 돌려보내주기위함
        Quaternion GoldRot = Ghostpyou.rotation;//회전이 안되면 다시 돌려보내주기위함



        if (pyou.childCount != 0)
        {
            oldRot1 = pyou.GetChild(0).rotation;
            oldRot2 = pyou.GetChild(1).rotation;



        }
        //if (pyou.transform.childCount != 0)
        //{
        //    Quaternion oldRot1 = pyou.transform.GetChild(0).rotation;
        //    Quaternion oldRot2 = pyou.transform.GetChild(1).rotation;

        //    if (!CanMoveTo(pyou))
        //    {
        //        pyou.transform.GetChild(0).rotation = oldRot1;
        //        pyou.transform.GetChild(1).rotation = oldRot2;
        //    }f
        //}
        pyou.position += moveDir;
        if (!isRotate)
        {
            Ghostpyou.position = pyou.position;
            for (int i = 0; i < Ghostpyou.childCount; i++)
            {
                Ghostpyou.GetChild(i).position = pyou.GetChild(i).position;
            }
        }
        else
        {


            pyou.rotation *= Quaternion.Euler(0, 0, 90);
            pyou.GetChild(0).rotation *= Quaternion.Euler(0, 0, -90);
            pyou.GetChild(1).rotation *= Quaternion.Euler(0, 0, -90);
            
            
            Ghostpyou.rotation *= Quaternion.Euler(0, 0, 90);
            Ghostpyou.GetChild(0).rotation *= Quaternion.Euler(0, 0, -90);
            Ghostpyou.GetChild(1).rotation *= Quaternion.Euler(0, 0, -90);
            Ghostpyou.position = pyou.position;
            

            if(pyou.rotation.eulerAngles == new Vector3(0, 0, 0) || pyou.rotation.eulerAngles == new Vector3(0, 0, 180))
            {
                for (int i = 0; i < Ghostpyou.childCount; i++)
                {
                    Ghostpyou.GetChild(i).position = pyou.GetChild(i).position;
                }
            }


        }

        while(CanMoveTo(pyou) && GhostCanMove(Ghostpyou))  //둘다 움직일수있을떄 유령뿌요를 내려준다
        {
            
           Ghostpyou.position += Vector3.down;

           if(Ghostpyou.position.y <-1) //무한루프 방지
           {
               break;
           }           


            
        }
        
        //한칸을 더 내려줫을시 한칸올려준다.
        if (Ghostpyou.position.y <0)
        {
            Ghostpyou.position += Vector3.up;
        }

        if (!CanMoveTo(Ghostpyou))
        {
            Ghostpyou.position = GoldPos;

        }

        if (!CanMoveTo(pyou))  
        {

            //만약 회전버튼을 누르고 회전을 햇으나 보드를 벗어난공간일수있으므로 다시 돌려준다
            pyou.position = oldPos;
            pyou.rotation = oldRot;
            pyou.GetChild(0).rotation = oldRot1;
            pyou.GetChild(1).rotation = oldRot2;

            //Ghostpyou.position = oldPos;
            Ghostpyou.rotation = oldRot;
            Ghostpyou.GetChild(0).rotation = oldRot1;
            Ghostpyou.GetChild(1).rotation = oldRot2;

            Combo = 0;
            if ((int)moveDir.y == -1 && (int)moveDir.x == 0 && isRotate == false) //움직일수없으나 한칸내려줘야하면 보드에 넣어주거나 게임종료조건인지 확인
            {

                if(!CanMoveTo(pyou))
                {

                    for (int i = 0; i < pyou.childCount; i++)
                    {
                        pyou.GetChild(i).gameObject.SetActive(false);
                        if (pyou.GetChild(i).gameObject.activeInHierarchy == false)
                        {
                            pyou.GetChild(i).parent = null;

                            i = -1;
                            //xxsize.transform.DetachChildren();
                        }
                    }
                    GameOver = true;
                    
                }


                if (GameOver == false)
                {
                    AddToBoard(pyou);
                    GhostPyouErase(Ghostpyou);

                    NextPyouFalse(NextPyous);

                }
                if (Combo == 0 &&GameOver ==false)
                {
                    CheckPyou();
                }

            }

            return false;
        }

        if (pyou.position.y <-1)
        {
            return false;
        }


        return true;

    }

    bool GhostCanMove(Transform root)
    {
       
        for (int i = 0; i < root.childCount; ++i)
        {
           
            var node = root.GetChild(i);
            int x = Mathf.RoundToInt(node.position.x + halfXsize + 0.5f);
            int y = Mathf.RoundToInt(node.position.y + halfYsize - 5);


            //좌우측 못가게
            if (x < 1 || x > Xsize)
            {
                return false;
            }
            //바닥
            if (y < 0)
            {
                if (pyou.rotation.eulerAngles == new Vector3(0, 0, 180))
                {
                    Ghostpyou.position += Vector3.up;
                }


                return false;
            }


            var column = Borad.Find(y.ToString());

           
           


            if (column != null && column.Find(x.ToString()) != null)
            {
                //if (pyou.rotation.eulerAngles == new Vector3(0, 0, 180))
                //{
                //    Ghostpyou.position += Vector3.up;
                //}

                return false;

            }

            if (column != null)
            {
               
                
                
               if (Borad.Find((y-1).ToString()) !=null)
                {
                    var columndown = Borad.Find((y - 1).ToString());
                    for(int j=0; j<columndown.childCount; j++)
                    {
                        if(int.Parse(columndown.GetChild(j).name) == x)
                        {

                            if (pyou.rotation.eulerAngles == new Vector3(0, 0, 90) || pyou.rotation.eulerAngles == new Vector3(0, 0, 270))
                            {
                                if (i == 0)
                                {
                                    while (true)
                                    {
                                        var nextnode = root.GetChild(1);
                                        x = Mathf.RoundToInt(nextnode.position.x + halfXsize + 0.5f);
                                        y = Mathf.RoundToInt(nextnode.position.y + halfYsize - 5);
                                        column = Borad.Find(y.ToString());
                                        Vector3 breakPostion = nextnode.position;
                                        if (Borad.Find((y - 1).ToString()) != null && column != null)
                                        {
                                            columndown = Borad.Find((y - 1).ToString());
                                            bool down = false;
                                            for (int jj = 0; jj < columndown.childCount; jj++)
                                            {
                                                if (int.Parse(columndown.GetChild(jj).name) != x)
                                                {
                                                    down = true;
                                                  
                                                }
                                                else
                                                {
                                                    down = false;
                                                    break;
                                                }
                                            }

                                            if(down ==true)
                                            {
                                                nextnode.position += Vector3.down;
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }

                                        if (nextnode.position == breakPostion)
                                        {
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    while (true)
                                    {
                                        var nextnode = root.GetChild(0);
                                        x = Mathf.RoundToInt(nextnode.position.x + halfXsize + 0.5f);
                                        y = Mathf.RoundToInt(nextnode.position.y + halfYsize - 5);
                                        column = Borad.Find(y.ToString());
                                        Vector3 breakPostion = nextnode.position;



                                        if (Borad.Find((y - 1).ToString()) != null && column != null)
                                        {
                                            columndown = Borad.Find((y - 1).ToString());
                                            bool down = false;

                                            for (int jj = 0; jj < columndown.childCount; jj++)
                                            {
                                                if (int.Parse(columndown.GetChild(jj).name) != x)
                                                {

                                                   
                                                    down = true;
                                                }
                                                else
                                                {
                                                    down = false;
                                                    break;
                                                }
                                            }
                                            if(down ==true)
                                            {
                                                nextnode.position += Vector3.down;
                                            }

                                        }
                                        else
                                        {
                                            break;
                                        }

                                        if (nextnode.position == breakPostion)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }

                            return false;
                          
                        }
                    }

                }
               
            } 


        }

        return true;
    }


    //바닥이거나 보드 안에 벗어나는지 체크
    bool CanMoveTo(Transform root)
    {
        for (int i = 0; i < root.childCount; ++i)
        {
            var node = root.GetChild(i);
            int x = Mathf.RoundToInt(node.position.x + halfXsize + 0.5f);
            int y = Mathf.RoundToInt(node.position.y + halfYsize - 5);

           
            //좌우측 못가게
            if (x < 1 || x > Xsize)
            {
                return false;
            }
            //바닥
            if (y < 0)
            {
                return false;
            }
            

            var column = Borad.Find(y.ToString());





            if (column != null && column.Find(x.ToString()) != null)
            {


                return false;

            }


            
        }

        return true;
    }


    void NextPyouFalse(Transform root)
    {
        for (int i = 0; i < root.childCount; i++)
        {
            root.GetChild(i).gameObject.SetActive(false);
            if (root.GetChild(i).gameObject.activeInHierarchy == false)
            {
                root.GetChild(i).parent = null;

                i = -1;
                //xxsize.transform.DetachChildren();
            }
        }


    }

    void GhostPyouErase(Transform root)
    {
        while(root.childCount>0)
        {
            var node = root.GetChild(0);
            node.gameObject.SetActive(false);
            node.parent = null;

        }

    }

    


    //뿌요를 보드에 추가
    void AddToBoard(Transform root)
    {

        int count = 0;
        int count1 = 0;
        int downParent = 0;
        int orgParent = 0;

        List<Transform> isdown = new List<Transform>();

        while (root.childCount > 0)
        {

            var node = root.GetChild(0);

            int x = Mathf.RoundToInt(node.position.x + halfXsize + 0.5f);
            int y = Mathf.RoundToInt(node.position.y);


            node.parent = Borad.Find(y.ToString());
            //node.parent.name = node.name;



            //Instantiate(node,node.transform).name = node.name;
            node.name = x.ToString();

            if (count == 1 && downParent > 0)
            {
                if (isdown[0].name != node.name)
                {
                    isdown[0].parent = Borad.Find((orgParent - downParent).ToString());
                    isdown[0].position += new Vector3(0, -downParent, 0);
                }
            }


            else if (count == 1 && downParent == 0)
            {

                while (y + 1 > 1)
                {
                    var Xnode = Borad.Find((y - 1).ToString());//아래
                    int XnodeChilde = Xnode.childCount;

                    List<string> XnodeList = new List<string>();

                    for (int i = 0; i < XnodeChilde; i++)
                    {
                        XnodeList.Add(Xnode.GetChild(i).name); ///아래쪽 자식의 이름 
                    }



                    int down = 0;

                    for (int o = 0; o < XnodeList.Count; o++)
                    {

                        //만약 찾아보다가 같은게 있다면 밑에 있으니까 내려가면안되고 없으면 내려가야함
                        if (node.name == XnodeList[o])
                        {
                            down = 0;
                            break;

                        }
                        else
                        {
                            down += 1;
                        }

                    }
                    if (down > 0) //같은게 없었다면
                    {
                        //node.parent = Borad.Find((y-1).ToString());//부모를 한칸 낮춰주고  
                        if (downParent == 0)
                        {
                            orgParent = y;
                        }
                        downParent += 1;

                        isdown.Add(node);

                        node.position += new Vector3(0, -1, 0);
                        node.parent = Borad.Find((y - 1).ToString());
                        //node.transform.position = new Vector3(node.transform.position.x, node.transform.position.y- 1, node.transform.position.z);
                    }
                    y--;
                }
            }


            while (count1 == 0)
            {
                if (y <= 0)
                {
                    count1 += 1;

                    break;
                }

                //var XnodeUp = Borad.Find((y+1).ToString());//위 
                var Xnode = Borad.Find((y - 1).ToString());//아래
                int XnodeChilde = Xnode.childCount;

                List<string> XnodeList = new List<string>();

                for (int i = 0; i < XnodeChilde; i++)
                {
                    XnodeList.Add(Xnode.GetChild(i).name); ///아래쪽 자식의 이름 
                }



                int down = 0;

                for (int o = 0; o < XnodeList.Count; o++)
                {

                    //만약 찾아보다가 같은게 있다면 밑에 있으니까 내려가면안되고 없으면 내려가야함
                    if (node.name == XnodeList[o])
                    {
                        down = 0;
                        break;

                    }
                    else
                    {
                        down += 1;
                    }

                }
                if (down > 0) //같은게 없었다면
                {
                    //node.parent = Borad.Find((y-1).ToString());//부모를 한칸 낮춰주고  
                    if (downParent == 0)
                    {
                        orgParent = y;
                    }
                    downParent += 1;

                    isdown.Add(node);

                    //node.transform.position += new Vector3(0, -1, 0);
                    //node.transform.position = new Vector3(node.transform.position.x, node.transform.position.y- 1, node.transform.position.z);
                }

                y--;
                //if(node.transform.name 

            }            //더 내려갈수있으면 더 내려줘라


            count += 1;


        }
    }




    void CheckPyou()
    {
        
        //세로로 된걸 어떤식으로 찾아 줘야할까?
        ComboSonud();
        if (Combo == 0)
        {
            isBroken = false;
            Breken();
            //부셔진게있으면 내림 
            if (GameOver == false)
            {
                BrekenDown();
                
                
            }
            ComboTime = 0f;
        }
        else if (Combo > 0)
        {

            if (ComboTime > NextTime)
            {
                Breken();
                //부셔진게있으면 내림 
                BrekenDown();
                
                //AudioSource.Stop();

            }
        }


    }


    void Breken()
    {
        isBroken = false;






        //가로 또는 세로 4개면 삭제



        YyRed.Clear();
        YyBlue.Clear();
        YyGreen.Clear();
        YyYellow.Clear();

        List<int> YyRedKey = new List<int>();    //키값으로 연결되있는지 학인하기 위함
        List<int> YyBlueKey = new List<int>();   //키값으로 연결되있는지 학인하기 위함
        List<int> YyGreenKey = new List<int>();  //키값으로 연결되있는지 학인하기 위함
        List<int> YyYellowKey = new List<int>(); //키값으로 연결되있는지 학인하기 위함
        


        for (int i = 0; i < Ysize; i++)
        {
            Transform xsize = Borad.GetChild(i);//i번쨰줄 체크





            if (xsize.childCount != 0)
            {
                for (int j = 0; j <= xsize.childCount - 1; j++)
                {
                    if (xsize.GetChild(j).CompareTag("Red"))
                    {
                        YyRed.Add(int.Parse(xsize.GetChild(j).name) + i * 10 - 1, xsize.GetChild(j).name);
                        YyRedKey.Add(int.Parse(xsize.GetChild(j).name) + i * 10 - 1);
                    }
                    else if (xsize.GetChild(j).CompareTag("Blue"))
                    {
                        YyBlue.Add(int.Parse(xsize.GetChild(j).name) + i * 10 - 1, xsize.GetChild(j).name);
                        YyBlueKey.Add(int.Parse(xsize.GetChild(j).name) + i * 10 - 1);

                    }
                    else if (xsize.GetChild(j).CompareTag("Green"))
                    {

                        YyGreen.Add(int.Parse(xsize.GetChild(j).name) + i * 10 - 1, xsize.GetChild(j).name);
                        YyGreenKey.Add(int.Parse(xsize.GetChild(j).name) + i * 10 - 1);
                    }
                    else if (xsize.GetChild(j).CompareTag("Yellow"))
                    {
                        YyYellow.Add(int.Parse(xsize.GetChild(j).name) + i * 10 - 1, xsize.GetChild(j).name);
                        YyYellowKey.Add(int.Parse(xsize.GetChild(j).name) + i * 10 - 1);

                    }

                }


            }

          
        }

        #region 삭제 시키기



        
       if (YyRedKey.Count >= 4)
       {
           //int x = 0;
           int cleck = 0;

           List<int> CleckList = new List<int>();

           bool Erase = false;

           for (int x = 0; x < YyRedKey.Count; x++)  //붙어있는게 4개 이상이면 어떻게 해야하지...?  
           {
               if (YyRed.ContainsKey(YyRedKey[x]) && YyRed.ContainsKey(YyRedKey[x] + 1) && YyRed.ContainsKey(YyRedKey[x] + 2) && YyRed.ContainsKey(YyRedKey[x] + 3)  //0123  -
                   || YyRed.ContainsKey(YyRedKey[x]) && YyRed.ContainsKey(YyRedKey[x] + 1) && YyRed.ContainsKey(YyRedKey[x] + 2) && YyRed.ContainsKey(YyRedKey[x] + 10) //0126 ㄴ
                   || YyRed.ContainsKey(YyRedKey[x]) && YyRed.ContainsKey(YyRedKey[x] + 1) && YyRed.ContainsKey(YyRedKey[x] + 10) && YyRed.ContainsKey(YyRedKey[x] + 11) // 0167 ㅁ
                   || YyRed.ContainsKey(YyRedKey[x]) && YyRed.ContainsKey(YyRedKey[x] + 1) && YyRed.ContainsKey(YyRedKey[x] + 2) && YyRed.ContainsKey(YyRedKey[x] + 12) // 0128 ㄴ> 
                    || YyRed.ContainsKey(YyRedKey[x]) && YyRed.ContainsKey(YyRedKey[x] + 1) && YyRed.ContainsKey(YyRedKey[x] + 2) && YyRed.ContainsKey(YyRedKey[x] + 11) // ㅗ 자 모양
                    || YyRed.ContainsKey(YyRedKey[x]) && YyRed.ContainsKey(YyRedKey[x] + 1) && YyRed.ContainsKey(YyRedKey[x] + 9) && YyRed.ContainsKey(YyRedKey[x] + 10) // 계단 모양
                    || YyRed.ContainsKey(YyRedKey[x]) && YyRed.ContainsKey(YyRedKey[x] + 1) && YyRed.ContainsKey(YyRedKey[x] + 11) && YyRed.ContainsKey(YyRedKey[x] + 12) // 반대방향 계단 
                    || YyRed.ContainsKey(YyRedKey[x]) && YyRed.ContainsKey(YyRedKey[x] + 1) && YyRed.ContainsKey(YyRedKey[x] + 10) && YyRed.ContainsKey(YyRedKey[x] + 20) //  ㄴ 
                    || YyRed.ContainsKey(YyRedKey[x]) && YyRed.ContainsKey(YyRedKey[x] + 1) && YyRed.ContainsKey(YyRedKey[x] + 11) && YyRed.ContainsKey(YyRedKey[x] + 21)  // ㄴ>
                   || YyRed.ContainsKey(YyRedKey[x]) && YyRed.ContainsKey(YyRedKey[x] + 9) && YyRed.ContainsKey(YyRedKey[x] + 10) && YyRed.ContainsKey(YyRedKey[x] + 11)  //ㅜ 자 모양 있어야함
                     || YyRed.ContainsKey(YyRedKey[x]) && YyRed.ContainsKey(YyRedKey[x] + 9) && YyRed.ContainsKey(YyRedKey[x] + 10) && YyRed.ContainsKey(YyRedKey[x] + 19) // 계단
                   || YyRed.ContainsKey(YyRedKey[x]) && YyRed.ContainsKey(YyRedKey[x] + 10) && YyRed.ContainsKey(YyRedKey[x] + 20) && YyRed.ContainsKey(YyRedKey[x] + 30) //0102030 |
                   || YyRed.ContainsKey(YyRedKey[x]) && YyRed.ContainsKey(YyRedKey[x] + 10) && YyRed.ContainsKey(YyRedKey[x] + 11) && YyRed.ContainsKey(YyRedKey[x] + 12) // 0678 r
                   || YyRed.ContainsKey(YyRedKey[x]) && YyRed.ContainsKey(YyRedKey[x] + 10) && YyRed.ContainsKey(YyRedKey[x] + 9) && YyRed.ContainsKey(YyRedKey[x] + 8) //2678    ㄱ
                   || YyRed.ContainsKey(YyRedKey[x]) && YyRed.ContainsKey(YyRedKey[x] + 10) && YyRed.ContainsKey(YyRedKey[x] + 11) && YyRed.ContainsKey(YyRedKey[x] + 20) //  ㅏ 
                    || YyRed.ContainsKey(YyRedKey[x]) && YyRed.ContainsKey(YyRedKey[x] + 10) && YyRed.ContainsKey(YyRedKey[x] + 9) && YyRed.ContainsKey(YyRedKey[x] + 20) //  ㅓ 
                    || YyRed.ContainsKey(YyRedKey[x]) && YyRed.ContainsKey(YyRedKey[x] + 10) && YyRed.ContainsKey(YyRedKey[x] + 11) && YyRed.ContainsKey(YyRedKey[x] + 21) //반대계단 
                    || YyRed.ContainsKey(YyRedKey[x]) && YyRed.ContainsKey(YyRedKey[x] + 10) && YyRed.ContainsKey(YyRedKey[x] + 19) && YyRed.ContainsKey(YyRedKey[x] + 20) // ㄱ 
                   || YyRed.ContainsKey(YyRedKey[x]) && YyRed.ContainsKey(YyRedKey[x] + 10) && YyRed.ContainsKey(YyRedKey[x] + 20) && YyRed.ContainsKey(YyRedKey[x] + 21)) // ㄱ> YyRedKey[x] 10 20 21
               {
                   cleck = YyRedKey[x];
                   Erase = true;
                  

                   if (Erase)
                   {
                       //13
                       isBroken = true;
                       #region 삭제할게있으면 찾아서 리스트에 넣음

                       if (YyRed.ContainsKey(cleck) && YyRed.ContainsKey(cleck + 1) && YyRed.ContainsKey(cleck + 2) && YyRed.ContainsKey(cleck + 3))  //0123  -
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 1);
                           CleckList.Add(cleck + 2);
                           CleckList.Add(cleck + 3);
                       }
                       else if (YyRed.ContainsKey(cleck) && YyRed.ContainsKey(cleck + 10) && YyRed.ContainsKey(cleck + 20) && YyRed.ContainsKey(cleck + 30)) //0102030 |
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 20);
                           CleckList.Add(cleck + 30);
                       }
                       else if(YyRed.ContainsKey(cleck) && YyRed.ContainsKey(cleck + 1) && YyRed.ContainsKey(cleck + 2) && YyRed.ContainsKey(cleck + 10)) //0126 ㄴ
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 1);
                           CleckList.Add(cleck + 2);
                           CleckList.Add(cleck + 10);
                       }
                       else if(YyRed.ContainsKey(cleck) && YyRed.ContainsKey(cleck + 10) && YyRed.ContainsKey(cleck + 11) && YyRed.ContainsKey(cleck + 12)) // 0678 ㄱ>
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 11);
                           CleckList.Add(cleck + 12);
                       }
                       else if(YyRed.ContainsKey(cleck) && YyRed.ContainsKey(cleck + 1) && YyRed.ContainsKey(cleck + 10) && YyRed.ContainsKey(cleck + 11)) // 0167 ㅁ
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 1);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 11);
                       }
                       else if(YyRed.ContainsKey(cleck) && YyRed.ContainsKey(cleck + 1) && YyRed.ContainsKey(cleck + 2) && YyRed.ContainsKey(cleck + 12)) // 0128 ㄴ> ㅂ
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 1);
                           CleckList.Add(cleck + 2);
                           CleckList.Add(cleck + 12);
                       }
                       else if(YyRed.ContainsKey(cleck) && YyRed.ContainsKey(cleck + 10) && YyRed.ContainsKey(cleck + 9) && YyRed.ContainsKey(cleck + 8)) //2678
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 9);
                           CleckList.Add(cleck + 8);
                       }
                       else if(YyRed.ContainsKey(cleck) && YyRed.ContainsKey(cleck + 9) && YyRed.ContainsKey(cleck + 10) && YyRed.ContainsKey(cleck + 11))  //ㅜ 자 모양 있어야함
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 9);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 11);
                       }
                       else if(YyRed.ContainsKey(cleck) && YyRed.ContainsKey(cleck + 1) && YyRed.ContainsKey(cleck + 2) && YyRed.ContainsKey(cleck + 11)) // ㅗ 자 모양
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 1);
                           CleckList.Add(cleck + 2);
                           CleckList.Add(cleck + 11);
                       }
                       else if(YyRed.ContainsKey(cleck) && YyRed.ContainsKey(cleck + 1) && YyRed.ContainsKey(cleck + 9) && YyRed.ContainsKey(cleck + 10)) // 계단 모양
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 1);
                           CleckList.Add(cleck + 9);
                           CleckList.Add(cleck + 10);
                       }
                       else if(YyRed.ContainsKey(cleck) && YyRed.ContainsKey(cleck + 1) && YyRed.ContainsKey(cleck + 11) && YyRed.ContainsKey(cleck + 12)) // 반대방향 계단
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 1);
                           CleckList.Add(cleck + 11);
                           CleckList.Add(cleck + 12);
                       }
                       else if(YyRed.ContainsKey(cleck) && YyRed.ContainsKey(cleck + 10) && YyRed.ContainsKey(cleck + 11) && YyRed.ContainsKey(cleck + 20)) //  ㅏ
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 11);
                           CleckList.Add(cleck + 20);
                       }
                       else if(YyRed.ContainsKey(cleck) && YyRed.ContainsKey(cleck + 10) && YyRed.ContainsKey(cleck + 9) && YyRed.ContainsKey(cleck + 20)) //  ㅓ 
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 9);
                           CleckList.Add(cleck + 20);
                       }
                       else if(YyRed.ContainsKey(cleck) && YyRed.ContainsKey(cleck + 1) && YyRed.ContainsKey(cleck + 10) && YyRed.ContainsKey(cleck + 20))
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 1);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 20);
                       }
                       else if(YyRed.ContainsKey(cleck) && YyRed.ContainsKey(cleck + 9) && YyRed.ContainsKey(cleck + 10) && YyRed.ContainsKey(cleck + 19)) // 계단
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 9);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 19);
                       }
                       else if(YyRed.ContainsKey(cleck) && YyRed.ContainsKey(cleck + 10) && YyRed.ContainsKey(cleck + 11) && YyRed.ContainsKey(cleck + 21)) //반대계단 
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 11);
                           CleckList.Add(cleck + 21);
                       }
                       else if (YyRed.ContainsKey(cleck) && YyRed.ContainsKey(cleck + 1) && YyRed.ContainsKey(cleck + 11) && YyRed.ContainsKey(cleck + 21))  // ㄴ>
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 1);
                           CleckList.Add(cleck + 11);
                           CleckList.Add(cleck + 21);
                       }
                       else if(YyRed.ContainsKey(cleck) && YyRed.ContainsKey(cleck + 10) && YyRed.ContainsKey(cleck + 19) && YyRed.ContainsKey(cleck + 20))// ㄱ
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 19);
                           CleckList.Add(cleck + 20);
                       }
                       else if(YyRed.ContainsKey(cleck) && YyRed.ContainsKey(cleck + 10) && YyRed.ContainsKey(cleck + 20) && YyRed.ContainsKey(cleck + 21))
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 20);
                           CleckList.Add(cleck + 21);

                       }
                       //이중에 하나가 맞다면 리스트 넣을떄 중복체크도 해야하네 중복된건 넣지말고 만약 조건안에 있다면 ....

                       CleckList.Sort(compareI); //정렬
                       for (int c = 0; c < CleckList.Count; c++)
                       {
                           if (c + 1 < CleckList.Count)
                           {
                               if (CleckList[c] == CleckList[c + 1])
                               {
                                   CleckList.Remove(CleckList[c]);
                                   c = -1;
                               }
                               else
                               {
                                   continue;
                               }
                           }
                       }

                       //for (int c = 0; c < CleckList.Count; c++)
                       //{
                      
                       //}
                       #endregion


                       Erase = false;
                   }

               }



           }
           
           if (CleckList.Count >= 4)
           {

               for (int c = 0; c < CleckList.Count; c++)
               {
                   int y = 0 + CleckList[c] / 10;
                  
                   Transform xxsize = Borad.GetChild(y);

                   for (int x = 0; x < xxsize.childCount; x++)
                   {
                        int yy = CleckList[c];
                        if (YyRed.ContainsKey(yy))
                        {

                            if (YyRed[CleckList[c]] == xxsize.GetChild(x).name)
                            {
                                xxsize.GetChild(x).gameObject.SetActive(false);

                            }
                        }
                        else
                        {
                            continue;
                        }

                   }

                   int pp = xxsize.childCount;
                   int cleck1 = 0;
                   for (int k = 0; k < xxsize.childCount; k++)
                   {

                       if (xxsize.childCount != 0)
                       {
                           if (xxsize.GetChild(k).gameObject.activeInHierarchy == false)
                           {
                               xxsize.GetChild(k).parent = null;
                               cleck1++;
                               k = -1;
                               //xxsize.transform.DetachChildren();
                           }
                           else
                           {

                               continue;
                           }
                           if (pp == cleck1)
                           {
                               break;
                           }


                       }
                   }


               }


               YyRed.Clear();
           }


       }
       if (YyBlueKey.Count >= 4)
       {
           //int x = 0;
           int cleck = 0;

           List<int> CleckList = new List<int>();
           CleckList.Clear();
           bool Erase = false;

           for (int x = 0; x < YyBlueKey.Count; x++)  //붙어있는게 4개 이상이면 어떻게 해야하지...?  
           {
               if (YyBlue.ContainsKey(YyBlueKey[x]) && YyBlue.ContainsKey(YyBlueKey[x] + 1) && YyBlue.ContainsKey(YyBlueKey[x] + 2) && YyBlue.ContainsKey(YyBlueKey[x] + 3)  //0123  -
                   || YyBlue.ContainsKey(YyBlueKey[x]) && YyBlue.ContainsKey(YyBlueKey[x] + 10) && YyBlue.ContainsKey(YyBlueKey[x] + 20) && YyBlue.ContainsKey(YyBlueKey[x] + 30) //0102030 |
                   || YyBlue.ContainsKey(YyBlueKey[x]) && YyBlue.ContainsKey(YyBlueKey[x] + 1) && YyBlue.ContainsKey(YyBlueKey[x] + 2) && YyBlue.ContainsKey(YyBlueKey[x] + 10) //0126 ㄴ
                   || YyBlue.ContainsKey(YyBlueKey[x]) && YyBlue.ContainsKey(YyBlueKey[x] + 10) && YyBlue.ContainsKey(YyBlueKey[x] + 11) && YyBlue.ContainsKey(YyBlueKey[x] + 12) // 0678 r
                   || YyBlue.ContainsKey(YyBlueKey[x]) && YyBlue.ContainsKey(YyBlueKey[x] + 1) && YyBlue.ContainsKey(YyBlueKey[x] + 10) && YyBlue.ContainsKey(YyBlueKey[x] + 11) // 0167 ㅁ
                   || YyBlue.ContainsKey(YyBlueKey[x]) && YyBlue.ContainsKey(YyBlueKey[x] + 1) && YyBlue.ContainsKey(YyBlueKey[x] + 2) && YyBlue.ContainsKey(YyBlueKey[x] + 12) // 0128 ㄴ> 
                   || YyBlue.ContainsKey(YyBlueKey[x]) && YyBlue.ContainsKey(YyBlueKey[x] + 10) && YyBlue.ContainsKey(YyBlueKey[x] + 9) && YyBlue.ContainsKey(YyBlueKey[x] + 8) //2678    ㄱ
                   || YyBlue.ContainsKey(YyBlueKey[x]) && YyBlue.ContainsKey(YyBlueKey[x] + 9) && YyBlue.ContainsKey(YyBlueKey[x] + 10) && YyBlue.ContainsKey(YyBlueKey[x] + 11)  //ㅜ 자 모양 있어야함
                    || YyBlue.ContainsKey(YyBlueKey[x]) && YyBlue.ContainsKey(YyBlueKey[x] + 1) && YyBlue.ContainsKey(YyBlueKey[x] + 2) && YyBlue.ContainsKey(YyBlueKey[x] + 11) // ㅗ 자 모양
                    || YyBlue.ContainsKey(YyBlueKey[x]) && YyBlue.ContainsKey(YyBlueKey[x] + 1) && YyBlue.ContainsKey(YyBlueKey[x] + 9) && YyBlue.ContainsKey(YyBlueKey[x] + 10) // 계단 모양
                    || YyBlue.ContainsKey(YyBlueKey[x]) && YyBlue.ContainsKey(YyBlueKey[x] + 1) && YyBlue.ContainsKey(YyBlueKey[x] + 11) && YyBlue.ContainsKey(YyBlueKey[x] + 12) // 반대방향 계단 
                   || YyBlue.ContainsKey(YyBlueKey[x]) && YyBlue.ContainsKey(YyBlueKey[x] + 10) && YyBlue.ContainsKey(YyBlueKey[x] + 11) && YyBlue.ContainsKey(YyBlueKey[x] + 20) //  ㅏ  
                    || YyBlue.ContainsKey(YyBlueKey[x]) && YyBlue.ContainsKey(YyBlueKey[x] + 10) && YyBlue.ContainsKey(YyBlueKey[x] + 9) && YyBlue.ContainsKey(YyBlueKey[x] + 20) //  ㅓ 
                    || YyBlue.ContainsKey(YyBlueKey[x]) && YyBlue.ContainsKey(YyBlueKey[x] + 1) && YyBlue.ContainsKey(YyBlueKey[x] + 10) && YyBlue.ContainsKey(YyBlueKey[x] + 20) //  ㄴ   
                    || YyBlue.ContainsKey(YyBlueKey[x]) && YyBlue.ContainsKey(YyBlueKey[x] + 9) && YyBlue.ContainsKey(YyBlueKey[x] + 10) && YyBlue.ContainsKey(YyBlueKey[x] + 19) // 계단
                    || YyBlue.ContainsKey(YyBlueKey[x]) && YyBlue.ContainsKey(YyBlueKey[x] + 10) && YyBlue.ContainsKey(YyBlueKey[x] + 11) && YyBlue.ContainsKey(YyBlueKey[x] + 21) //반대계단   YyBlueKey[x]  YyBlueKey[x]+10 YyBlueKey[x]+11  YyBlueKey[x] +21        
                    || YyBlue.ContainsKey(YyBlueKey[x]) && YyBlue.ContainsKey(YyBlueKey[x] + 1) && YyBlue.ContainsKey(YyBlueKey[x] + 11) && YyBlue.ContainsKey(YyBlueKey[x] + 21)  // ㄴ>
                    || YyBlue.ContainsKey(YyBlueKey[x]) && YyBlue.ContainsKey(YyBlueKey[x] + 10) && YyBlue.ContainsKey(YyBlueKey[x] + 19) && YyBlue.ContainsKey(YyBlueKey[x] + 20) //ㄱ
                     || YyBlue.ContainsKey(YyBlueKey[x]) && YyBlue.ContainsKey(YyBlueKey[x] + 10) && YyBlue.ContainsKey(YyBlueKey[x] + 20) && YyBlue.ContainsKey(YyBlueKey[x] + 21)) //ㄱ>
               {
                   cleck = YyBlueKey[x];
                   Erase = true;
                  
                   if (Erase)
                   {
                       //13
                       isBroken = true;
                       #region 삭제할게있으면 찾아서 리스트에 넣음

                       if (YyBlue.ContainsKey(cleck) && YyBlue.ContainsKey(cleck + 1) && YyBlue.ContainsKey(cleck + 2) && YyBlue.ContainsKey(cleck + 3))  //0123  -
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 1);
                           CleckList.Add(cleck + 2);
                           CleckList.Add(cleck + 3);
                       }
                       if (YyBlue.ContainsKey(cleck) && YyBlue.ContainsKey(cleck + 10) && YyBlue.ContainsKey(cleck + 20) && YyBlue.ContainsKey(cleck + 30)) //0102030 |
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 20);
                           CleckList.Add(cleck + 30);
                       }
                       if (YyBlue.ContainsKey(cleck) && YyBlue.ContainsKey(cleck + 1) && YyBlue.ContainsKey(cleck + 2) && YyBlue.ContainsKey(cleck + 10)) //0126 ㄴ
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 1);
                           CleckList.Add(cleck + 2);
                           CleckList.Add(cleck + 10);
                       }
                       if (YyBlue.ContainsKey(cleck) && YyBlue.ContainsKey(cleck + 10) && YyBlue.ContainsKey(cleck + 11) && YyBlue.ContainsKey(cleck + 12)) // 0678 ㄱ>
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 11);
                           CleckList.Add(cleck + 12);
                       }
                       if (YyBlue.ContainsKey(cleck) && YyBlue.ContainsKey(cleck + 1) && YyBlue.ContainsKey(cleck + 10) && YyBlue.ContainsKey(cleck + 11)) // 0167 ㅁ
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 1);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 11);
                       }
                       if (YyBlue.ContainsKey(cleck) && YyBlue.ContainsKey(cleck + 1) && YyBlue.ContainsKey(cleck + 2) && YyBlue.ContainsKey(cleck + 12)) // 0128 ㄴ> ㅂ
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 1);
                           CleckList.Add(cleck + 2);
                           CleckList.Add(cleck + 12);
                       }
                       if (YyBlue.ContainsKey(cleck) && YyBlue.ContainsKey(cleck + 10) && YyBlue.ContainsKey(cleck + 9) && YyBlue.ContainsKey(cleck + 8)) //2678
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 9);
                           CleckList.Add(cleck + 8);
                       }
                       if (YyBlue.ContainsKey(cleck) && YyBlue.ContainsKey(cleck + 9) && YyBlue.ContainsKey(cleck + 10) && YyBlue.ContainsKey(cleck + 11))  //ㅜ 자 모양 있어야함
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 9);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 11);
                       }
                       if (YyBlue.ContainsKey(cleck) && YyBlue.ContainsKey(cleck + 1) && YyBlue.ContainsKey(cleck + 2) && YyBlue.ContainsKey(cleck + 11)) // ㅗ 자 모양
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 1);
                           CleckList.Add(cleck + 2);
                           CleckList.Add(cleck + 11);
                       }
                       if (YyBlue.ContainsKey(cleck) && YyBlue.ContainsKey(cleck + 1) && YyBlue.ContainsKey(cleck + 9) && YyBlue.ContainsKey(cleck + 10)) // 계단 모양
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 1);
                           CleckList.Add(cleck + 9);
                           CleckList.Add(cleck + 10);
                       }
                       if (YyBlue.ContainsKey(cleck) && YyBlue.ContainsKey(cleck + 1) && YyBlue.ContainsKey(cleck + 11) && YyBlue.ContainsKey(cleck + 12)) // 반대방향 계단
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 1);
                           CleckList.Add(cleck + 11);
                           CleckList.Add(cleck + 12);
                       }
                       if (YyBlue.ContainsKey(cleck) && YyBlue.ContainsKey(cleck + 10) && YyBlue.ContainsKey(cleck + 11) && YyBlue.ContainsKey(cleck + 20)) //  ㅏ
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 11);
                           CleckList.Add(cleck + 20);
                       }
                       if (YyBlue.ContainsKey(cleck) && YyBlue.ContainsKey(cleck + 10) && YyBlue.ContainsKey(cleck + 9) && YyBlue.ContainsKey(cleck + 20)) //  ㅓ 
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 9);
                           CleckList.Add(cleck + 20);
                       }
                       if (YyBlue.ContainsKey(cleck) && YyBlue.ContainsKey(cleck + 1) && YyBlue.ContainsKey(cleck + 10) && YyBlue.ContainsKey(cleck + 20))
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 1);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 20);
                       }

                       if (YyBlue.ContainsKey(cleck) && YyBlue.ContainsKey(cleck + 9) && YyBlue.ContainsKey(cleck + 10) && YyBlue.ContainsKey(cleck + 19))
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 9);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 19);
                       }
                       if (YyBlue.ContainsKey(cleck) && YyBlue.ContainsKey(cleck + 10) && YyBlue.ContainsKey(cleck + 11) && YyBlue.ContainsKey(cleck + 21))
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 11);
                           CleckList.Add(cleck + 21);

                       }
                       if (YyBlue.ContainsKey(cleck) && YyBlue.ContainsKey(cleck + 1) && YyBlue.ContainsKey(cleck + 11) && YyBlue.ContainsKey(cleck + 21))  // ㄴ>
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 1);
                           CleckList.Add(cleck + 11);
                           CleckList.Add(cleck + 21);
                       }
                       if (YyBlue.ContainsKey(cleck) && YyBlue.ContainsKey(cleck + 10) && YyBlue.ContainsKey(cleck + 19) && YyBlue.ContainsKey(cleck + 20))
                       {
                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 19);
                           CleckList.Add(cleck + 20);
                       }
                       if (YyBlue.ContainsKey(cleck) && YyBlue.ContainsKey(cleck + 10) && YyBlue.ContainsKey(cleck + 20) && YyBlue.ContainsKey(cleck + 21)) //ㄱ>
                       {

                           CleckList.Add(cleck);
                           CleckList.Add(cleck + 10);
                           CleckList.Add(cleck + 20);
                           CleckList.Add(cleck + 21);
                       }
                       //이중에 하나가 맞다면 리스트 넣을떄 중복체크도 해야하네 중복된건 넣지말고 만약 조건안에 있다면 ....

                       CleckList.Sort(compareI); //정렬
                       for (int c = 0; c < CleckList.Count; c++)
                       {
                           if (c + 1 < CleckList.Count)
                           {
                               if (CleckList[c] == CleckList[c + 1])
                               {
                                   CleckList.Remove(CleckList[c]);
                                   c = -1;
                               }
                               else
                               {
                                   continue;
                               }
                           }
                       }

                       //for (int c = 0; c < CleckList.Count; c++)
                       //{
                       //

                       //}

                       Erase = false;
                       #endregion
                   }


               }
           }

           if (CleckList.Count >= 4)
           {

               for (int c = 0; c < CleckList.Count; c++)
               {
                   int y = 0 + CleckList[c] / 10;
                   
                   Transform xxsize = Borad.GetChild(y);

                   for (int x = 0; x < xxsize.childCount; x++)
                   {
                        int yy = CleckList[c];
                        if (YyBlue.ContainsKey(yy))
                        {

                            if (YyBlue[CleckList[c]] == xxsize.GetChild(x).name)
                            {
                                xxsize.GetChild(x).gameObject.SetActive(false);

                            }
                        }
                        else
                        {
                            continue;
                        }

                   }

                   int pp = xxsize.childCount;
                   int cleck1 = 0;
                   for (int k = 0; k < xxsize.childCount; k++)
                   {

                       if (xxsize.childCount != 0)
                       {
                           if (xxsize.GetChild(k).gameObject.activeInHierarchy == false)
                           {
                               xxsize.GetChild(k).parent = null;
                               cleck1++;
                               k = -1;
                               //xxsize.transform.DetachChildren();
                           }
                           else
                           {

                               continue;
                           }
                           if (pp == cleck1)
                           {
                               break;
                           }


                       }
                   }


               }


               YyBlue.Clear();
           }

       }
      
        if (YyGreenKey.Count >= 4)
        {
            //int x = 0;
            int cleck = 0;

            List<int> CleckList = new List<int>();

            bool Erase = false;


            for (int x = 0; x < YyGreenKey.Count; x++)
            {

                if (YyGreen.ContainsKey(YyGreenKey[x]) && YyGreen.ContainsKey(YyGreenKey[x] + 1) && YyGreen.ContainsKey(YyGreenKey[x] + 2) && YyGreen.ContainsKey(YyGreenKey[x] + 3)  //0123  -
                       || YyGreen.ContainsKey(YyGreenKey[x]) && YyGreen.ContainsKey(YyGreenKey[x] + 1) && YyGreen.ContainsKey(YyGreenKey[x] + 2) && YyGreen.ContainsKey(YyGreenKey[x] + 10) //0126 ㄴ
                      || YyGreen.ContainsKey(YyGreenKey[x]) && YyGreen.ContainsKey(YyGreenKey[x] + 1) && YyGreen.ContainsKey(YyGreenKey[x] + 2) && YyGreen.ContainsKey(YyGreenKey[x] + 12) // 0128 ㄴ> 
                       || YyGreen.ContainsKey(YyGreenKey[x]) && YyGreen.ContainsKey(YyGreenKey[x] + 1) && YyGreen.ContainsKey(YyGreenKey[x] + 2) && YyGreen.ContainsKey(YyGreenKey[x] + 11) // ㅗ 자 모양
                       || YyGreen.ContainsKey(YyGreenKey[x]) && YyGreen.ContainsKey(YyGreenKey[x] + 1) && YyGreen.ContainsKey(YyGreenKey[x] + 9) && YyGreen.ContainsKey(YyGreenKey[x] + 10) // 계단 모양
                      || YyGreen.ContainsKey(YyGreenKey[x]) && YyGreen.ContainsKey(YyGreenKey[x] + 1) && YyGreen.ContainsKey(YyGreenKey[x] + 10) && YyGreen.ContainsKey(YyGreenKey[x] + 11) // 0167 ㅁ
                       || YyGreen.ContainsKey(YyGreenKey[x]) && YyGreen.ContainsKey(YyGreenKey[x] + 1) && YyGreen.ContainsKey(YyGreenKey[x] + 10) && YyGreen.ContainsKey(YyGreenKey[x] + 20) //  ㄴ 
                       || YyGreen.ContainsKey(YyGreenKey[x]) && YyGreen.ContainsKey(YyGreenKey[x] + 1) && YyGreen.ContainsKey(YyGreenKey[x] + 11) && YyGreen.ContainsKey(YyGreenKey[x] + 12) // 반대방향 계단 
                        || YyGreen.ContainsKey(YyGreenKey[x]) && YyGreen.ContainsKey(YyGreenKey[x] + 1) && YyGreen.ContainsKey(YyGreenKey[x] + 11) && YyGreen.ContainsKey(YyGreenKey[x] + 21)  // ㄴ>
                      || YyGreen.ContainsKey(YyGreenKey[x]) && YyGreen.ContainsKey(YyGreenKey[x] + 9) && YyGreen.ContainsKey(YyGreenKey[x] + 10) && YyGreen.ContainsKey(YyGreenKey[x] + 11)  //ㅜ 자 모양 있어야함
                        || YyGreen.ContainsKey(YyGreenKey[x]) && YyGreen.ContainsKey(YyGreenKey[x] + 9) && YyGreen.ContainsKey(YyGreenKey[x] + 10) && YyGreen.ContainsKey(YyGreenKey[x] + 19) // 계단
                      || YyGreen.ContainsKey(YyGreenKey[x]) && YyGreen.ContainsKey(YyGreenKey[x] + 10) && YyGreen.ContainsKey(YyGreenKey[x] + 9) && YyGreen.ContainsKey(YyGreenKey[x] + 8) //2678    ㄱ
                       || YyGreen.ContainsKey(YyGreenKey[x]) && YyGreen.ContainsKey(YyGreenKey[x] + 10) && YyGreen.ContainsKey(YyGreenKey[x] + 9) && YyGreen.ContainsKey(YyGreenKey[x] + 20) //  ㅓ 
                      || YyGreen.ContainsKey(YyGreenKey[x]) && YyGreen.ContainsKey(YyGreenKey[x] + 10) && YyGreen.ContainsKey(YyGreenKey[x] + 11) && YyGreen.ContainsKey(YyGreenKey[x] + 12) // 0678 r
                      || YyGreen.ContainsKey(YyGreenKey[x]) && YyGreen.ContainsKey(YyGreenKey[x] + 10) && YyGreen.ContainsKey(YyGreenKey[x] + 11) && YyGreen.ContainsKey(YyGreenKey[x] + 20) //  ㅏ 
                       || YyGreen.ContainsKey(YyGreenKey[x]) && YyGreen.ContainsKey(YyGreenKey[x] + 10) && YyGreen.ContainsKey(YyGreenKey[x] + 11) && YyGreen.ContainsKey(YyGreenKey[x] + 21) //반대계단 
                      || YyGreen.ContainsKey(YyGreenKey[x]) && YyGreen.ContainsKey(YyGreenKey[x] + 10) && YyGreen.ContainsKey(YyGreenKey[x] + 20) && YyGreen.ContainsKey(YyGreenKey[x] + 30) //0102030 |
                      || YyGreen.ContainsKey(YyGreenKey[x]) && YyGreen.ContainsKey(YyGreenKey[x] + 10) && YyGreen.ContainsKey(YyGreenKey[x] + 19) && YyGreen.ContainsKey(YyGreenKey[x] + 20) //ㄱ
                       || YyGreen.ContainsKey(YyGreenKey[x]) && YyGreen.ContainsKey(YyGreenKey[x] + 10) && YyGreen.ContainsKey(YyGreenKey[x] + 20) && YyGreen.ContainsKey(YyGreenKey[x] + 21)) //ㄱ>
                {
                    cleck = YyGreenKey[x];
                    Erase = true;
                   

                    if (Erase)
                    {
                        //13
                        isBroken = true;
                        #region 삭제할게있으면 찾아서 리스트에 넣음

                        if (YyGreen.ContainsKey(cleck) && YyGreen.ContainsKey(cleck + 1) && YyGreen.ContainsKey(cleck + 2) && YyGreen.ContainsKey(cleck + 3))  //0123  -
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 1);
                            CleckList.Add(cleck + 2);
                            CleckList.Add(cleck + 3);
                        }
                        if (YyGreen.ContainsKey(cleck) && YyGreen.ContainsKey(cleck + 10) && YyGreen.ContainsKey(cleck + 20) && YyGreen.ContainsKey(cleck + 30)) //0102030 |
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 20);
                            CleckList.Add(cleck + 30);
                        }
                        if (YyGreen.ContainsKey(cleck) && YyGreen.ContainsKey(cleck + 1) && YyGreen.ContainsKey(cleck + 2) && YyGreen.ContainsKey(cleck + 10)) //0126 ㄴ
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 1);
                            CleckList.Add(cleck + 2);
                            CleckList.Add(cleck + 10);
                        }
                        if (YyGreen.ContainsKey(cleck) && YyGreen.ContainsKey(cleck + 10) && YyGreen.ContainsKey(cleck + 11) && YyGreen.ContainsKey(cleck + 12)) // 0678 ㄱ>
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 11);
                            CleckList.Add(cleck + 12);
                        }
                       if (YyGreen.ContainsKey(cleck) && YyGreen.ContainsKey(cleck + 1) && YyGreen.ContainsKey(cleck + 10) && YyGreen.ContainsKey(cleck + 11)) // 0167 ㅁ
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 1);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 11);
                        }
                        if (YyGreen.ContainsKey(cleck) && YyGreen.ContainsKey(cleck + 1) && YyGreen.ContainsKey(cleck + 2) && YyGreen.ContainsKey(cleck + 12)) // 0128 ㄴ> ㅂ
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 1);
                            CleckList.Add(cleck + 2);
                            CleckList.Add(cleck + 12);
                        }
                        if (YyGreen.ContainsKey(cleck) && YyGreen.ContainsKey(cleck + 10) && YyGreen.ContainsKey(cleck + 9) && YyGreen.ContainsKey(cleck + 8)) //2678
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 9);
                            CleckList.Add(cleck + 8);
                        }
                        if (YyGreen.ContainsKey(cleck) && YyGreen.ContainsKey(cleck + 9) && YyGreen.ContainsKey(cleck + 10) && YyGreen.ContainsKey(cleck + 11))  //ㅜ 자 모양 있어야함
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 9);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 11);
                        }
                        if (YyGreen.ContainsKey(cleck) && YyGreen.ContainsKey(cleck + 1) && YyGreen.ContainsKey(cleck + 2) && YyGreen.ContainsKey(cleck + 11)) // ㅗ 자 모양
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 1);
                            CleckList.Add(cleck + 2);
                            CleckList.Add(cleck + 11);
                        }
                        if (YyGreen.ContainsKey(cleck) && YyGreen.ContainsKey(cleck + 1) && YyGreen.ContainsKey(cleck + 9) && YyGreen.ContainsKey(cleck + 10)) // 계단 모양
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 1);
                            CleckList.Add(cleck + 9);
                            CleckList.Add(cleck + 10);
                        }
                        if (YyGreen.ContainsKey(cleck) && YyGreen.ContainsKey(cleck + 1) && YyGreen.ContainsKey(cleck + 11) && YyGreen.ContainsKey(cleck + 12)) // 반대방향 계단
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 1);
                            CleckList.Add(cleck + 11);
                            CleckList.Add(cleck + 12);
                        }
                        if (YyGreen.ContainsKey(cleck) && YyGreen.ContainsKey(cleck + 10) && YyGreen.ContainsKey(cleck + 11) && YyGreen.ContainsKey(cleck + 20)) //  ㅏ
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 11);
                            CleckList.Add(cleck + 20);
                        }
                        if (YyGreen.ContainsKey(cleck) && YyGreen.ContainsKey(cleck + 10) && YyGreen.ContainsKey(cleck + 9) && YyGreen.ContainsKey(cleck + 20)) //  ㅓ 
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 9);
                            CleckList.Add(cleck + 20);
                        }
                        if (YyGreen.ContainsKey(cleck) && YyGreen.ContainsKey(cleck + 1) && YyGreen.ContainsKey(cleck + 10) && YyGreen.ContainsKey(cleck + 20))
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 1);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 20);
                        }
                        if (YyGreen.ContainsKey(cleck) && YyGreen.ContainsKey(cleck + 9) && YyGreen.ContainsKey(cleck + 10) && YyGreen.ContainsKey(cleck + 19)) // 계단
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 9);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 19);
                        }
                        if (YyGreen.ContainsKey(cleck) && YyGreen.ContainsKey(cleck + 10) && YyGreen.ContainsKey(cleck + 11) && YyGreen.ContainsKey(cleck + 21)) //반대계단
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 11);
                            CleckList.Add(cleck + 21);
                        }
                       if (YyGreen.ContainsKey(cleck) && YyGreen.ContainsKey(cleck + 1) && YyGreen.ContainsKey(cleck + 11) && YyGreen.ContainsKey(cleck + 21))  // ㄴ>
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 11);
                            CleckList.Add(cleck + 21);
                        }
                        if (YyGreen.ContainsKey(cleck) && YyGreen.ContainsKey(cleck + 10) && YyGreen.ContainsKey(cleck + 19) && YyGreen.ContainsKey(cleck + 20)) //ㄱ
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 19);
                            CleckList.Add(cleck + 20);
                        }
                        if (YyGreen.ContainsKey(cleck) && YyGreen.ContainsKey(cleck + 10) && YyGreen.ContainsKey(cleck + 20) && YyGreen.ContainsKey(cleck + 21)) //ㄱ>
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 20);
                            CleckList.Add(cleck + 21);
                        }


                        //이중에 하나가 맞다면 리스트 넣을떄 중복체크도 해야하네 중복된건 넣지말고 만약 조건안에 있다면 ....

                        CleckList.Sort(compareI); //정렬
                        for (int c = 0; c < CleckList.Count; c++)
                        {
                            if (c + 1 < CleckList.Count)
                            {
                                if (CleckList[c] == CleckList[c + 1])
                                {
                                    CleckList.Remove(CleckList[c]);
                                    c = -1;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }


                        Erase = false;

                        //for (int c = 0; c < CleckList.Count; c++)
                        //{
                        //
                        //}
                        #endregion
                    }




                }
            }
            if (CleckList.Count >= 4)
            {

                for (int c = 0; c < CleckList.Count; c++)
                {
                    int y = 0 + CleckList[c] / 10;
                  
                    Transform xxsize = Borad.GetChild(y);

                    for (int x = 0; x < xxsize.childCount; x++)
                    {
                        int yy = CleckList[c];
                        if (YyGreen.ContainsKey(yy)) //키가 존재하는지
                        {
                            if (YyGreen[CleckList[c]] == xxsize.GetChild(x).name)
                            {
                                xxsize.GetChild(x).gameObject.SetActive(false);

                            }
                        }
                        else
                        {
                            continue;
                        }

                    }

                    int pp = xxsize.childCount;
                    int cleck1 = 0;
                    for (int k = 0; k < xxsize.childCount; k++)
                    {

                        if (xxsize.childCount != 0)
                        {
                            if (xxsize.GetChild(k).gameObject.activeInHierarchy == false)
                            {
                                xxsize.GetChild(k).parent = null;
                                cleck1++;
                                k = -1;
                                //xxsize.transform.DetachChildren();
                            }
                            else
                            {

                                continue;
                            }
                            if (pp == cleck1)
                            {
                                break;
                            }
                        }
                    }
                }
                YyGreen.Clear();
            }
        }       
        if (YyYellowKey.Count >= 4)
        {
            //int x = 0;
            int cleck = 0;
            List<int> CleckList = new List<int>();
            bool Erase = false;
            for (int x = 0; x < YyYellowKey.Count; x++)  //붙어있는게 4개 이상이면 어떻게 해야하지...?  
            {
                if (YyYellow.ContainsKey(YyYellowKey[x]) && YyYellow.ContainsKey(YyYellowKey[x] + 1) && YyYellow.ContainsKey(YyYellowKey[x] + 2) && YyYellow.ContainsKey(YyYellowKey[x] + 3)  //0123  -
                    || YyYellow.ContainsKey(YyYellowKey[x]) && YyYellow.ContainsKey(YyYellowKey[x] + 10) && YyYellow.ContainsKey(YyYellowKey[x] + 20) && YyYellow.ContainsKey(YyYellowKey[x] + 30) //0102030 |
                    || YyYellow.ContainsKey(YyYellowKey[x]) && YyYellow.ContainsKey(YyYellowKey[x] + 1) && YyYellow.ContainsKey(YyYellowKey[x] + 2) && YyYellow.ContainsKey(YyYellowKey[x] + 10) //0126 ㄴ
                    || YyYellow.ContainsKey(YyYellowKey[x]) && YyYellow.ContainsKey(YyYellowKey[x] + 10) && YyYellow.ContainsKey(YyYellowKey[x] + 11) && YyYellow.ContainsKey(YyYellowKey[x] + 12) // 0678 r
                    || YyYellow.ContainsKey(YyYellowKey[x]) && YyYellow.ContainsKey(YyYellowKey[x] + 1) && YyYellow.ContainsKey(YyYellowKey[x] + 10) && YyYellow.ContainsKey(YyYellowKey[x] + 11) // 0167 ㅁ
                    || YyYellow.ContainsKey(YyYellowKey[x]) && YyYellow.ContainsKey(YyYellowKey[x] + 1) && YyYellow.ContainsKey(YyYellowKey[x] + 2) && YyYellow.ContainsKey(YyYellowKey[x] + 12) // 0128 ㄴ> 
                    || YyYellow.ContainsKey(YyYellowKey[x]) && YyYellow.ContainsKey(YyYellowKey[x] + 10) && YyYellow.ContainsKey(YyYellowKey[x] + 9) && YyYellow.ContainsKey(YyYellowKey[x] + 8) //2678    ㄱ
                    || YyYellow.ContainsKey(YyYellowKey[x]) && YyYellow.ContainsKey(YyYellowKey[x] + 9) && YyYellow.ContainsKey(YyYellowKey[x] + 10) && YyYellow.ContainsKey(YyYellowKey[x] + 11)  //ㅜ 자 모양 있어야함
                     || YyYellow.ContainsKey(YyYellowKey[x]) && YyYellow.ContainsKey(YyYellowKey[x] + 1) && YyYellow.ContainsKey(YyYellowKey[x] + 2) && YyYellow.ContainsKey(YyYellowKey[x] + 11) // ㅗ 자 모양
                     || YyYellow.ContainsKey(YyYellowKey[x]) && YyYellow.ContainsKey(YyYellowKey[x] + 1) && YyYellow.ContainsKey(YyYellowKey[x] + 9) && YyYellow.ContainsKey(YyYellowKey[x] + 10) // 계단 모양
                     || YyYellow.ContainsKey(YyYellowKey[x]) && YyYellow.ContainsKey(YyYellowKey[x] + 1) && YyYellow.ContainsKey(YyYellowKey[x] + 11) && YyYellow.ContainsKey(YyYellowKey[x] + 12) // 반대방향 계단 
                    || YyYellow.ContainsKey(YyYellowKey[x]) && YyYellow.ContainsKey(YyYellowKey[x] + 10) && YyYellow.ContainsKey(YyYellowKey[x] + 11) && YyYellow.ContainsKey(YyYellowKey[x] + 20) //  ㅏ 
                     || YyYellow.ContainsKey(YyYellowKey[x]) && YyYellow.ContainsKey(YyYellowKey[x] + 10) && YyYellow.ContainsKey(YyYellowKey[x] + 9) && YyYellow.ContainsKey(YyYellowKey[x] + 20) //  ㅓ 
                     || YyYellow.ContainsKey(YyYellowKey[x]) && YyYellow.ContainsKey(YyYellowKey[x] + 1) && YyYellow.ContainsKey(YyYellowKey[x] + 10) && YyYellow.ContainsKey(YyYellowKey[x] + 20) //  ㄴ 
                     || YyYellow.ContainsKey(YyYellowKey[x]) && YyYellow.ContainsKey(YyYellowKey[x] + 9) && YyYellow.ContainsKey(YyYellowKey[x] + 10) && YyYellow.ContainsKey(YyYellowKey[x] + 19) // 계단
                     || YyYellow.ContainsKey(YyYellowKey[x]) && YyYellow.ContainsKey(YyYellowKey[x] + 10) && YyYellow.ContainsKey(YyYellowKey[x] + 11) && YyYellow.ContainsKey(YyYellowKey[x] + 21) //반대계단 
                     || YyYellow.ContainsKey(YyYellowKey[x]) && YyYellow.ContainsKey(YyYellowKey[x] + 1) && YyYellow.ContainsKey(YyYellowKey[x] + 11) && YyYellow.ContainsKey(YyYellowKey[x] + 21)  // ㄴ>
                     || YyYellow.ContainsKey(YyYellowKey[x]) && YyYellow.ContainsKey(YyYellowKey[x] + 10) && YyYellow.ContainsKey(YyYellowKey[x] + 19) && YyYellow.ContainsKey(YyYellowKey[x] + 20) //ㄱ
                      || YyYellow.ContainsKey(YyYellowKey[x]) && YyYellow.ContainsKey(YyYellowKey[x] + 10) && YyYellow.ContainsKey(YyYellowKey[x] + 20) && YyYellow.ContainsKey(YyYellowKey[x] + 21)) //ㄱ>
                {
                    cleck = YyYellowKey[x];
                    Erase = true;                   
                    if (Erase)
                    {
                        //13
                        isBroken = true;
                        #region 삭제할게있으면 찾아서 리스트에 넣음

                        if (YyYellow.ContainsKey(cleck) && YyYellow.ContainsKey(cleck + 1) && YyYellow.ContainsKey(cleck + 2) && YyYellow.ContainsKey(cleck + 3))  //0123  -
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 1);
                            CleckList.Add(cleck + 2);
                            CleckList.Add(cleck + 3);
                        }
                        if (YyYellow.ContainsKey(cleck) & YyYellow.ContainsKey(cleck + 10) && YyYellow.ContainsKey(cleck + 20) && YyYellow.ContainsKey(cleck + 30)) //0102030 |
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 20);
                            CleckList.Add(cleck + 30);
                        }
                        if (YyYellow.ContainsKey(cleck) && YyYellow.ContainsKey(cleck + 1) && YyYellow.ContainsKey(cleck + 2) && YyYellow.ContainsKey(cleck + 10)) //0126 ㄴ
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 1);
                            CleckList.Add(cleck + 2);
                            CleckList.Add(cleck + 10);
                        }
                        if (YyYellow.ContainsKey(cleck) && YyYellow.ContainsKey(cleck + 10) && YyYellow.ContainsKey(cleck + 11) && YyYellow.ContainsKey(cleck + 12)) // 0678 ㄱ>
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 11);
                            CleckList.Add(cleck + 12);
                        }
                        if (YyYellow.ContainsKey(cleck) && YyYellow.ContainsKey(cleck + 1) && YyYellow.ContainsKey(cleck + 10) && YyYellow.ContainsKey(cleck + 11)) // 0167 ㅁ
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 1);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 11);
                        }
                        if (YyYellow.ContainsKey(cleck) && YyYellow.ContainsKey(cleck + 1) && YyYellow.ContainsKey(cleck + 2) && YyYellow.ContainsKey(cleck + 12)) // 0128 ㄴ> ㅂ
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 1);
                            CleckList.Add(cleck + 2);
                            CleckList.Add(cleck + 12);
                        }
                        if (YyYellow.ContainsKey(cleck) && YyYellow.ContainsKey(cleck + 10) && YyYellow.ContainsKey(cleck + 9) && YyYellow.ContainsKey(cleck + 8)) //2678
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 9);
                            CleckList.Add(cleck + 8);
                        }
                        if (YyYellow.ContainsKey(cleck) && YyYellow.ContainsKey(cleck + 9) && YyYellow.ContainsKey(cleck + 10) && YyYellow.ContainsKey(cleck + 11))  //ㅜ 자 모양 있어야함
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 9);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 11);
                        }
                        if (YyYellow.ContainsKey(cleck) && YyYellow.ContainsKey(cleck + 1) && YyYellow.ContainsKey(cleck + 2) && YyYellow.ContainsKey(cleck + 11)) // ㅗ 자 모양
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 1);
                            CleckList.Add(cleck + 2);
                            CleckList.Add(cleck + 11);
                        }
                        if (YyYellow.ContainsKey(cleck) && YyYellow.ContainsKey(cleck + 1) && YyYellow.ContainsKey(cleck + 9) && YyYellow.ContainsKey(cleck + 10)) // 계단 모양
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 1);
                            CleckList.Add(cleck + 9);
                            CleckList.Add(cleck + 10);
                        }
                        if (YyYellow.ContainsKey(cleck) && YyYellow.ContainsKey(cleck + 1) && YyYellow.ContainsKey(cleck + 11) && YyYellow.ContainsKey(cleck + 12)) // 반대방향 계단
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 1);
                            CleckList.Add(cleck + 11);
                            CleckList.Add(cleck + 12);
                        }
                        if (YyYellow.ContainsKey(cleck) && YyYellow.ContainsKey(cleck + 10) && YyYellow.ContainsKey(cleck + 11) && YyYellow.ContainsKey(cleck + 20)) //  ㅏ
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 11);
                            CleckList.Add(cleck + 20);
                        }
                        if (YyYellow.ContainsKey(cleck) && YyYellow.ContainsKey(cleck + 10) && YyYellow.ContainsKey(cleck + 9) && YyYellow.ContainsKey(cleck + 20)) //  ㅓ 
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 9);
                            CleckList.Add(cleck + 20);
                        }
                        if (YyYellow.ContainsKey(cleck) && YyYellow.ContainsKey(cleck + 1) && YyYellow.ContainsKey(cleck + 10) && YyYellow.ContainsKey(cleck + 20))
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 1);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 20);
                        }
                        if (YyYellow.ContainsKey(cleck) && YyYellow.ContainsKey(cleck + 9) && YyYellow.ContainsKey(cleck + 10) && YyYellow.ContainsKey(cleck + 19)) // 계단
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 9);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 19);
                        }
                        if (YyYellow.ContainsKey(cleck) && YyYellow.ContainsKey(cleck + 10) && YyYellow.ContainsKey(cleck + 11) && YyYellow.ContainsKey(cleck + 21)) //반대계단 
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 11);
                            CleckList.Add(cleck + 21);
                        }
                        if (YyYellow.ContainsKey(cleck) && YyYellow.ContainsKey(cleck + 1) && YyYellow.ContainsKey(cleck + 11) && YyYellow.ContainsKey(cleck + 21))  // ㄴ>
                        {
                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 1);
                            CleckList.Add(cleck + 11);
                            CleckList.Add(cleck + 21);
                        }
                        if (YyYellow.ContainsKey(cleck) && YyYellow.ContainsKey(cleck + 10) && YyYellow.ContainsKey(cleck + 19) && YyYellow.ContainsKey(cleck + 20)) //ㄱ
                        {

                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 19);
                            CleckList.Add(cleck + 20);
                        }
                        if (YyYellow.ContainsKey(cleck) && YyYellow.ContainsKey(cleck + 10) && YyYellow.ContainsKey(cleck + 20) && YyYellow.ContainsKey(cleck + 21)) //ㄱ>
                        {

                            CleckList.Add(cleck);
                            CleckList.Add(cleck + 10);
                            CleckList.Add(cleck + 20);
                            CleckList.Add(cleck + 21);
                        }

                        //이중에 하나가 맞다면 리스트 넣을떄 중복체크도 해야하네 중복된건 넣지말고 만약 조건안에 있다면 ....

                        CleckList.Sort(compareI); //정렬
                        for (int c = 0; c < CleckList.Count; c++)
                        {
                            if (c + 1 < CleckList.Count)
                            {
                                if (CleckList[c] == CleckList[c + 1])
                                {
                                    CleckList.Remove(CleckList[c]);
                                    c = -1;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }

                        Erase = false;

                        //for (int c = 0; c < CleckList.Count; c++)
                        //{
                        //
                        //}
                        #endregion
                    }

                }
            }



            if (CleckList.Count >= 4)
            {

                for (int c = 0; c < CleckList.Count; c++)
                {
                    int y = 0 + CleckList[c] / 10;
                   
                    Transform xxsize = Borad.GetChild(y);

                    for (int x = 0; x < xxsize.childCount; x++)
                    {
                        int yy = CleckList[c];

                        if (YyYellow.ContainsKey(yy))
                        {
                            if (YyYellow[CleckList[c]] == xxsize.GetChild(x).name)
                            {
                                xxsize.GetChild(x).gameObject.SetActive(false);

                            }
                        }
                        else
                        {
                            continue;
                        }

                    }

                    int pp = xxsize.childCount;
                    int cleck1 = 0;
                    for (int k = 0; k < xxsize.childCount; k++)
                    {

                        if (xxsize.childCount != 0)
                        {
                            if (xxsize.GetChild(k).gameObject.activeInHierarchy == false)
                            {
                                xxsize.GetChild(k).parent = null;
                                cleck1++;
                                k = -1;
                                //xxsize.transform.DetachChildren();
                            }
                            else
                            {
                                continue;
                            }
                            if (pp == cleck1)
                            {
                                break;
                            }
                        }
                    }
                }
                YyYellow.Clear();
            }
        }
        #endregion
        if (isBroken == false)
        {            
                Combo = -1;           
        }
    }
    //삭제했으면 내려주자
    void BrekenDown()
    {
        int enptyCol = 0;
        List<int> UpList = new List<int>();
        List<int> DownList = new List<int>();
        if (isBroken)
        {
            Combo += 1;
            for (int i = 1; i < Borad.childCount; i++)
            {
                Transform xsize = Borad.GetChild(i);//i번쨰줄 체크 위쪽줄
                UpList.Clear();
                DownList.Clear();
                YyUp.Clear();
                YyDown.Clear();
                int down = i - 1;
                Transform ysize = Borad.GetChild(down); //아래쪽 줄
                if (xsize.childCount != 0)
                {
                    for (int j = 0; j <= xsize.childCount - 1; j++)
                    {
                        YyUp.Add(int.Parse(xsize.GetChild(j).name) + i * 10 - 1, xsize.GetChild(j).name);
                        UpList.Add(int.Parse(xsize.GetChild(j).name) + i * 10 - 1);
                    }
                    for (int j = 0; j <= ysize.childCount - 1; j++)
                    {
                        YyDown.Add(int.Parse(ysize.GetChild(j).name) + down * 10 - 1, ysize.GetChild(j).name);
                    }
                }
                while (down >= 0 && YyUp.Count != 0)
                {
                    if (YyDown.Count == 0 && down == i - 1)
                    {
                        enptyCol += 1;
                    }
                    else if (YyDown.Count != 0 && down == i - 1)
                    {
                        for (int j = 0; j < UpList.Count; j++)
                        {
                            if (YyDown.ContainsKey(UpList[j] - 10))//아래에 있다
                            {
                                UpList.Remove(UpList[j]);
                                j = -1;//처음부터
                            }
                        }
                        if (UpList.Count != 0)
                        {
                            enptyCol += 1;
                        }
                    }
                    else if (down < i - 1 && UpList.Count != 0)
                    {
                        xsize = Borad.GetChild(down + 1);//i번쨰줄 체크 위쪽줄
                        ysize = Borad.GetChild(down); //아래쪽 줄
                        UpList.Clear();
                        for (int j = 0; j <= xsize.childCount - 1; j++)
                        {
                            YyUp.Add(int.Parse(xsize.GetChild(j).name) + (down + 1) * 10 - 1, xsize.GetChild(j).name);
                            UpList.Add(int.Parse(xsize.GetChild(j).name) + (down + 1) * 10 - 1);
                        }
                        for (int j = 0; j <= ysize.childCount - 1; j++)
                        {
                            YyDown.Add(int.Parse(ysize.GetChild(j).name) + down * 10 - 1, ysize.GetChild(j).name);
                        }
                        if (YyDown.Count == 0)
                        {
                            enptyCol += 1;
                            break;
                        }
                        else
                        {
                            for (int j = 0; j < UpList.Count; j++)
                            {
                                if (YyDown.ContainsKey(UpList[j] - 10))//아래에 있다
                                {
                                    UpList.Remove(UpList[j]);
                                    j = -1;//처음부터
                                }
                            }
                            if (UpList.Count != 0)
                            {
                                enptyCol += 1;
                            }
                        }
                    }
                    if (enptyCol > 0)
                    {
                        int Updown = 0;
                        for (int c = 0; c < UpList.Count; c++)
                        {
                            int y = down + 1;                         
                            Transform xxsize = Borad.GetChild(y);
                            if (Updown == UpList.Count)
                            {
                                break;
                            }
                            for (int x = 0; x < xxsize.childCount; x++)
                            {
                                if (YyUp[UpList[c]] == xxsize.GetChild(x).name)
                                {
                                    xxsize.GetChild(x).gameObject.transform.position += new Vector3(0, -1, 0);
                                    xxsize.GetChild(x).parent = Borad.Find((y - 1).ToString());
                                    Updown++;
                                }
                            }
                            xxsize = null;
                        }
                        enptyCol = 0;
                    }
                    down--;
                }
            }
        }
        else
        {
            for (int i = 1; i < Borad.childCount; i++)
            {
                Transform xsize = Borad.GetChild(i);//i번쨰줄 체크 위쪽줄
                UpList.Clear();
                DownList.Clear();
                YyUp.Clear();
                YyDown.Clear();
                int down = i - 1;
                Transform ysize = Borad.GetChild(down); //아래쪽 줄
                if (xsize.childCount != 0)
                {
                    for (int j = 0; j <= xsize.childCount - 1; j++)
                    {
                        YyUp.Add(int.Parse(xsize.GetChild(j).name) + i * 10 - 1, xsize.GetChild(j).name);
                        UpList.Add(int.Parse(xsize.GetChild(j).name) + i * 10 - 1);
                    }
                    for (int j = 0; j <= ysize.childCount - 1; j++)
                    {
                        YyDown.Add(int.Parse(ysize.GetChild(j).name) + down * 10 - 1, ysize.GetChild(j).name);
                    }
                }
                while (down >= 0 && YyUp.Count != 0)
                {
                    if (YyDown.Count == 0 && down == i - 1)
                    {
                        enptyCol += 1;
                    }
                    else if (YyDown.Count != 0 && down == i - 1)
                    {
                        for (int j = 0; j < UpList.Count; j++)
                        {
                            if (YyDown.ContainsKey(UpList[j] - 10))//아래에 있다
                            {
                                UpList.Remove(UpList[j]);
                                j = -1;//처음부터
                            }                       
                        }
                        if (UpList.Count != 0)
                        {
                            enptyCol += 1;
                        }
                    }
                    else if (down < i - 1 && UpList.Count != 0)
                    {
                        xsize = Borad.GetChild(down + 1);//i번쨰줄 체크 위쪽줄

                        ysize = Borad.GetChild(down); //아래쪽 줄
                        UpList.Clear();
                        for (int j = 0; j <= xsize.childCount - 1; j++)
                        {

                            YyUp.Add(int.Parse(xsize.GetChild(j).name) + (down + 1) * 10 - 1, xsize.GetChild(j).name);
                            UpList.Add(int.Parse(xsize.GetChild(j).name) + (down + 1) * 10 - 1);
                        }
                        for (int j = 0; j <= ysize.childCount - 1; j++)
                        {
                            YyDown.Add(int.Parse(ysize.GetChild(j).name) + down * 10 - 1, ysize.GetChild(j).name);
                        }                 
                        if (YyDown.Count == 0)
                        {
                            enptyCol += 1;
                            break;
                        }
                        else
                        {
                            for (int j = 0; j < UpList.Count; j++)
                            {
                                if (YyDown.ContainsKey(UpList[j] - 10))//아래에 있다
                                {
                                    UpList.Remove(UpList[j]);
                                    j = -1;//처음부터
                                }
                            }
                            if (UpList.Count != 0)
                            {
                                enptyCol += 1;
                            }
                        }
                    }
                    if (enptyCol > 0)
                    {
                        int Updown = 0;
                        for (int c = 0; c < UpList.Count; c++)
                        {                     
                            int y = down + 1; 
                            Transform xxsize = Borad.GetChild(y);
                            if (Updown == UpList.Count)
                            {
                                break;
                            }
                            for (int x = 0; x < xxsize.childCount; x++)
                            {
                                if (YyUp[UpList[c]] == xxsize.GetChild(x).name)
                                {
                                    xxsize.GetChild(x).gameObject.transform.position += new Vector3(0, -1, 0);
                                    xxsize.GetChild(x).parent = Borad.Find((y - 1).ToString());
                                    Updown++;
                                }
                            }
                            xxsize = null;
                        }
                        enptyCol = 0;
                    }
                    down--;
                }
            }
        }
        ComboTime = 0f;
    }
    
}
