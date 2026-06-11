using System.Collections;
using UnityEngine;
using TMPro; // TextMesh Proの名前空間をインポート
using UnityEngine.SceneManagement; // シーン管理用の名前空間をインポート

public class Judge : MonoBehaviour
{
    [SerializeField] private GameObject[] MessageObj; // プレイヤーに判定を伝えるゲームオブジェクト
    [SerializeField] private NotesManager notesManager; // スクリプト「notesManager」を入れる変数
    [SerializeField] private TextMeshProUGUI comboText; // コンボ数表示用のTextMeshProUGUIコンポーネント
    [SerializeField] private TextMeshProUGUI scoreText; // スコア表示用のTextMeshProUGUIコンポーネント
    [SerializeField] private GameObject finish;

    AudioSource audio;
    [SerializeField] AudioClip hitSound;

    private float endTime = 0; // 曲の終了時刻を格納する変数

    private int perfectScore = 2000;
    private int greatScore = 1500;
    private int badScore = 500;
    private int missScore = 0;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        endTime = notesManager.NotesTime[notesManager.NotesTime.Count - 1] + GManager.instance.StartTime + 2f; // 曲の終了時刻を設定し、2秒後にResultシーンに遷移
    }

    void Update()
{
    if (GManager.instance.Start)
    {
        if (notesManager.LaneNum.Count > 0) // リストが空でないことを確認
        {
            if (Input.GetKeyDown(KeyCode.D) && notesManager.LaneNum[0] == 0)
            {
                Judgement(GetABS(Time.time - (notesManager.NotesTime[0] + GManager.instance.StartTime)));
            }
            if (Input.GetKeyDown(KeyCode.F) && notesManager.LaneNum[0] == 1)
            {
                Judgement(GetABS(Time.time - (notesManager.NotesTime[0] + GManager.instance.StartTime)));
            }
            if (Input.GetKeyDown(KeyCode.J) && notesManager.LaneNum[0] == 2)
            {
                Judgement(GetABS(Time.time - (notesManager.NotesTime[0] + GManager.instance.StartTime)));
            }
            if (Input.GetKeyDown(KeyCode.K) && notesManager.LaneNum[0] == 3)
            {
                Judgement(GetABS(Time.time - (notesManager.NotesTime[0] + GManager.instance.StartTime)));
            }
        }

        if (notesManager.NotesTime.Count > 0 && Time.time > notesManager.NotesTime[0] + 0.2f + GManager.instance.StartTime)
        {
            message(3);
            deleteData();
            Debug.Log("MISS");
            GManager.instance.miss++;
            GManager.instance.combo = 0;
            UpdateUI(); // UIを更新
        }

        if (Time.time > endTime)
        {
            StartCoroutine(TransitionToResultScene()); // 2秒後にResultシーンに遷移
        }
    }
}


    void Judgement(float timeLag)
    {
        if (timeLag <= 0.10f)
        {
            Debug.Log("PERFECT");
            message(0);
            GManager.instance.perfect++;
            GManager.instance.combo++;
            GManager.instance.score += perfectScore; // スコア加算
            UpdateUI(); // UIを更新
            deleteData();
        }
        else if (timeLag <= 0.15f)
        {
            Debug.Log("GREAT");
            message(1);
            GManager.instance.great++;
            GManager.instance.combo++;
            GManager.instance.score += greatScore; // スコア加算
            UpdateUI(); // UIを更新
            deleteData();
        }
        else if (timeLag <= 0.20f)
        {
            Debug.Log("BAD");
            message(2);
            GManager.instance.bad++;
            GManager.instance.combo = 0;
            GManager.instance.score += badScore; // スコア加算
            UpdateUI(); // UIを更新
            deleteData();
        }
    }

    float GetABS(float num)
    {
        return Mathf.Abs(num);
    }

    void deleteData()
    {
        if (notesManager.NotesTime.Count > 0)
        {
            notesManager.NotesTime.RemoveAt(0);
            notesManager.LaneNum.RemoveAt(0);
            notesManager.NoteType.RemoveAt(0);
        }
    }

    void message(int judge)
    {
        Instantiate(MessageObj[judge], new Vector3(notesManager.LaneNum[0] - 1.5f, 0.76f, 0.15f), Quaternion.Euler(45, 0, 0));
    }

    void UpdateUI()
    {
        if (comboText != null)
        {
            comboText.text = "Combo: " + GManager.instance.combo;
        }
        if (scoreText != null)
        {
            scoreText.text = "Score: " + GManager.instance.score;
        }
    }

    IEnumerator TransitionToResultScene()
    {
        yield return new WaitForSeconds(2f); // 2秒待機
        SceneManager.LoadScene("Result"); // Resultシーンに遷移
    }
}
