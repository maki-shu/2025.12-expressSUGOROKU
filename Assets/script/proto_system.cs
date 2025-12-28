using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

public class proto_system : MonoBehaviour
{
    private static proto_system _instance;
    public static proto_system Instance
    {
        get
        {
            if (_instance == null)
            {
                // ÉVÅ[Éìì‡Ç©ÇÁé©ìÆåüçı
                _instance = FindAnyObjectByType<proto_system>();

                if (_instance == null)
                {
                    UnityEngine.Debug.LogError("TutorialSystem Ç™ÉVÅ[ÉìÇ…ë∂ç›ÇµÇ‹ÇπÇÒÅI");
                }
            }
            return _instance;
        }
    }
    [HideInInspector] public int NowSta { get; set; } = 0;
    [HideInInspector] public int NextSta { get; set; } = 0;

    int Num_Dice = 0;
    int Type_Dice = 0;

    [SerializeField] private string Name_text1;
    [SerializeField] private string Name_text2;
    [SerializeField] private Button Button_GoNext;
    [SerializeField] private Button Button_Stop;
    [SerializeField] private Button Button_RollDice;

    private GameObject text1;
    private GameObject text2;
    private TextMeshProUGUI TMP_text1;
    private TextMeshProUGUI TMP_text2;

    private bool Active_RollDice = false;
    private bool Go_nextSta = false; //âwî≠é‘îªíË
    private bool nextSta = false;
    private bool stop = false;
    private bool Press_Button_GoNext = false;
    private bool Press_Button_Stop = false;
    private bool Press_Button_RollDice = false;
    private bool StopStation = false;
    private bool TestBool1 = false;

    private string[] TrainType = 
    {
        $"ïÅí ",   //1
        $"ã}çs",   //2
        $"í¥ì¡ã}"  //3
    };

    public class StationMap
    {
        public int Key { get; set; }
        public int StationLevel { get; set; }      //éÌï í‚é‘âwî‘çÜ(1Å®ïÅí í‚é‘âw 2Å®ã}çsí‚é‘âw 3Å®í¥ì¡ã}í‚é‘âw)
        public string StationName { get; set; }    // âwñº
    }

    List<StationMap> stationList = new List<StationMap>
    {
        new StationMap { Key = 0, StationLevel = 4, StationName = "îéëΩ" },
        new StationMap { Key = 1, StationLevel = 1, StationName = "ñkã„èB" },
        new StationMap { Key = 2, StationLevel = 3, StationName = "è¨ëq" },
        new StationMap { Key = 3, StationLevel = 2, StationName = "â∫ä÷" },
        new StationMap { Key = 4, StationLevel = 1, StationName = "å˙ã∑" },
        new StationMap { Key = 5, StationLevel = 1, StationName = "è¨åS" },
        new StationMap { Key = 6, StationLevel = 2, StationName = "ìøéR" },
        new StationMap { Key = 7, StationLevel = 1, StationName = "ä‚çë" },
        new StationMap { Key = 8, StationLevel = 1, StationName = "ã{ìá" },
        new StationMap { Key = 9, StationLevel = 3, StationName = "çLìá" },
        new StationMap { Key = 10, StationLevel = 1, StationName = "éOå¥" },
        new StationMap { Key = 11, StationLevel = 2, StationName = "ïüéR" },
        new StationMap { Key = 12, StationLevel = 1, StationName = "ëqï~" },
        new StationMap { Key = 13, StationLevel = 3, StationName = "â™éR" },
        new StationMap { Key = 14, StationLevel = 2, StationName = "ïPòH" },
        new StationMap { Key = 15, StationLevel = 1, StationName = "ñæêŒ" },
        new StationMap { Key = 16, StationLevel = 3, StationName = "ê_åÀ" },
        new StationMap { Key = 17, StationLevel = 3, StationName = "ëÂç„" },
        new StationMap { Key = 18, StationLevel = 3, StationName = "ãûìs" },
        new StationMap { Key = 19, StationLevel = 1, StationName = "î˙îiåŒ" },
        new StationMap { Key = 20, StationLevel = 1, StationName = "ïƒå¥" },
        new StationMap { Key = 21, StationLevel = 2, StationName = "äÚïå" },
        new StationMap { Key = 22, StationLevel = 3, StationName = "ñºå√âÆ" },
        new StationMap { Key = 23, StationLevel = 1, StationName = "éOâÕ" },
        new StationMap { Key = 24, StationLevel = 1, StationName = "ñLã¥" },
        new StationMap { Key = 25, StationLevel = 2, StationName = "ïlèº" },
        new StationMap { Key = 26, StationLevel = 3, StationName = "ê√â™" },
        new StationMap { Key = 27, StationLevel = 1, StationName = "ïxéméR" },
        new StationMap { Key = 28, StationLevel = 2, StationName = "îMäC" },
        new StationMap { Key = 29, StationLevel = 1, StationName = "è¨ìcå¥" },
        new StationMap { Key = 30, StationLevel = 3, StationName = "â°ïl" },
        new StationMap { Key = 31, StationLevel = 4, StationName = "ìåãû" }
    };

    void Start()
    {
        text1 = GameObject.Find(Name_text1);
        TMP_text1 = text1.GetComponent<TextMeshProUGUI>();

        text2 = GameObject.Find(Name_text2);
        TMP_text2 = text2.GetComponent<TextMeshProUGUI>();

        Button_GoNext.onClick.AddListener(() => Press_Button_GoNext = true);
        Button_Stop.onClick.AddListener(() => Press_Button_Stop = true);
        Button_RollDice.onClick.AddListener(() => Press_Button_RollDice = true);

        UnityEngine.Debug.Log($"åªç›ÅFîéëΩ");
        TMP_text1.text = $"îéëΩ";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Press_Button_RollDice)
        {
            if (!Active_RollDice)
            {
                Active_RollDice = true;
                StartCoroutine(RollDice());
            }
            Press_Button_RollDice = false;
        }

        if(Press_Button_GoNext || Input.GetKeyDown(KeyCode.W))
        {
            nextSta = true;
            Press_Button_GoNext = false;
        }
        else
        {
            nextSta = false;
        }

        if (Press_Button_Stop || Input.GetKeyDown(KeyCode.S))
        {
            stop = true;
            Press_Button_Stop = false;
        }
        else
        {
            stop = false;
        }

        TMP_text2.text = $"{nextSta}";
    }

    IEnumerator RollDice()
    {
        Num_Dice = Random.Range(1, 4);

        if (stationList[NowSta].StationLevel == 1)
        {
            Type_Dice = 1;
        }
        else if(stationList[NowSta].StationLevel == 2)
        {
            Type_Dice = Random.Range(1, 3);
        }
        else
        {
            Type_Dice = Random.Range(1, 4);
        }

        switch (Type_Dice)
        {
            case 1:

                StartCoroutine(MoveTrain());
                break;

            case 2:

                StartCoroutine(MoveTrain());
                break;

            case 3:

                StartCoroutine(MoveTrain());
                break;
        }
        
        yield return new WaitForSeconds(1);
    }

    IEnumerator MoveTrain()
    {
        NextSta = NowSta;
        for (int MassProgression = Num_Dice; MassProgression > 0; MassProgression--)
        {
            NextSta++;

            while (stationList[NextSta].StationLevel < Type_Dice)
            {
                UnityEngine.Debug.Log($"{stationList[NextSta].StationName} í âﬂ");
                TMP_text1.text = $"{stationList[NextSta].StationName}";
                NextSta++;

                if (NextSta > stationList.Count - 1)
                {
                    UnityEngine.Debug.Log($"full");
                    break;
                }
            }

            if (stationList[NextSta].StationLevel == 4)
            {
                UnityEngine.Debug.Log($"Out of Service");
                break;
            }

            UnityEngine.Debug.Log($"{stationList[NextSta].StationName} í‚é‘");
            TMP_text1.text = $"{stationList[NextSta].StationName}";

            if (stationList[NextSta].StationLevel == 4)
            {
                UnityEngine.Debug.Log($"Out of Service");
                break;
            }

            if(MassProgression == 1)
            {
                continue;
            }

            yield return StartCoroutine(WaitSta());

            if (StopStation)
            {
                StopStation = false;
                break;
            }

        }

        if (NextSta < stationList.Count - 1)
        {
            NowSta = NextSta;
            UnityEngine.Debug.Log($"èoñ⁄ÅF{Num_Dice} éÌï ÅF{TrainType[Type_Dice - 1]} åªç›ÅF{stationList[NowSta].StationName}");
        }
        else
        {
            NowSta = stationList.Count - 1;
            UnityEngine.Debug.Log($"èoñ⁄ÅF{Num_Dice} éÌï ÅF{TrainType[Type_Dice - 1]} åªç›ÅF{stationList[NowSta].StationName}");
            GameEnd();
            yield break;
        }

        Active_RollDice = false;
    }

    IEnumerator WaitSta()
    {
        while (!nextSta)
        {
            /*if(!TestBool1)
            {
                TestBool1 = true;
                UnityEngine.Debug.Log($"Wait");
            }*/
            // +TestBool1 = false;

            if (stop)
            {
                StopStation = true;
                UnityEngine.Debug.Log($"stop");
                break;
            }
            yield return null;
        }
        //UnityEngine.Debug.Log($"GoNext");
        yield return new WaitForSeconds(0.5f);
    }

    void SetText(string text1, string text2)
    {

    }

    void GameEnd()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}