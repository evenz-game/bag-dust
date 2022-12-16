using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Realms.Sync;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class RankController : MonoBehaviour
{
    public UnityEvent onInitializedRank = new UnityEvent();
    public UnityEvent onSelectedNameCharacter = new UnityEvent();

    private int myScore;

    private MongoClient.Collection<SoloRanking> collection;
    private bool registeredRecord = false;
    private bool cantUseDB => collection == null;

    [Header("Rank Board")]
    [SerializeField]
    private RankBoardItem[] rankBoardItems;

    [Header("Rank")]
    [SerializeField]
    private TextMeshProUGUI textRank;

    [Header("Name")]
    [SerializeField]
    private TextMeshProUGUI[] textNameCharacters;
    private List<char> nameCharacters = new List<char>();       // A(65) ~ Z(90)
    [SerializeField]
    private int currentNameCharIndex = -1;

    [SerializeField]
    private RectTransform nameArrowTransform;

    [Header("Score")]
    [SerializeField]
    private TextMeshProUGUI textScore;

    private void Start()
    {
        myScore = MyPlayerPrefs.GetDustCount(1);
        textScore.text = myScore.ToString();

        foreach (var t in textNameCharacters)
            nameCharacters.Add('A');

        NextCharacter();

        Init();
    }

    private async void Init()
    {
        try
        {
            var app = App.Create("application-0-qkomx");
            var user = await app.LogInAsync(Credentials.Anonymous());
            var mongoClient = user.GetMongoClient("mongodb-atlas");
            var dbPlantInventory = mongoClient.GetDatabase("bagdust");
            collection = dbPlantInventory.GetCollection<SoloRanking>("solo-ranking");
        }
        finally
        {
            if (cantUseDB)
                onInitializedRank.Invoke();
            else
                InitRank(true);
        }
    }

    private async void InitRank(bool init = false)
    {
        var result = await collection.FindAsync(null, new { score = -1 });

        int rank = 1;
        int myRank = 1;
        bool findRank = false;

        foreach (var r in result)
        {
            if (rank <= 10)
                rankBoardItems[rank - 1].Init(rank, r.Name, r.Score);

            if (myScore > r.Score && !findRank)
            {
                myRank = rank;
                findRank = true;
            }

            rank++;
        }

        if (myScore == 0)
            myRank = result.Length + 1;

        if (init)
        {
            textRank.text = myRank.ToString();
            onInitializedRank.Invoke();
        }
    }

    private bool movable = false;
    public void ChangeCharacter(Vector2 axis)
    {
        // if (!isActive || isDone) return;
        if (cantUseDB) return;
        if (currentNameCharIndex >= textNameCharacters.Length) return;

        /* 클릭 형식으로 인식할 수 있도록 계산 */
        if (!movable)
        {
            movable = (axis.sqrMagnitude == 0);
            return;
        }
        else
            if (axis == Vector2.zero || axis.y == 0) return;

        /* On Key Down */
        int dir = -(int)Mathf.Sign(axis.y);

        // 글자 변경
        int newChar = (int)nameCharacters[currentNameCharIndex] + dir;
        if (newChar > 90) newChar = 65;
        if (newChar < 65) newChar = 90;

        nameCharacters[currentNameCharIndex] = (char)newChar;

        // UI 적용
        for (int i = 0; i < textNameCharacters.Length; i++)
            textNameCharacters[i].text = nameCharacters[i].ToString();

        movable = false;
    }

    public void NextCharacter()
    {
        if (currentNameCharIndex >= textNameCharacters.Length) return;

        if (currentNameCharIndex >= 0)
            onSelectedNameCharacter.Invoke();

        currentNameCharIndex++;

        // 마지막 글자까지 다 정하면 등록
        if (currentNameCharIndex == textNameCharacters.Length)
        {
            RegisterRecord();
            nameArrowTransform.gameObject.SetActive(false);
        }
        else
        {
            nameArrowTransform.transform.parent = textNameCharacters[currentNameCharIndex].transform;
            nameArrowTransform.localPosition = Vector3.zero;
        }
    }

    public async void RegisterRecord()
    {
        // 이름
        // char to string
        string name = "";
        foreach (char c in nameCharacters)
            name += c.ToString();

        var info = new SoloRanking();
        info.Name = name;
        info.Score = myScore;
        info.Date = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await collection.InsertOneAsync(info);

        InitRank();
    }
}


public class SoloRanking
{

    [BsonElement("_id")]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

    [BsonElement("name")]
    [BsonRepresentation(BsonType.String)]
    public string Name { get; set; }

    [BsonElement("score")]
    [BsonRepresentation(BsonType.Int32)]
    public int Score { get; set; }

    [BsonElement("date")]
    [BsonRepresentation(BsonType.Int64)]
    public long Date { get; set; }

    public override string ToString()
    {
        return $"{Name}, {Score}, {Date}";
    }
}
