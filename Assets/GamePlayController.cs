using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayController : MonoBehaviour
{
    public static GamePlayController Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField] int score;
    [SerializeField] int highscore;
    public Color[] template = { new Color32(255, 81, 81, 255), new Color32(255, 129, 82, 255), new Color32(255, 233, 82, 255), new Color32(163, 255, 82, 255), new Color32(82, 207, 255, 255), new Color32(170, 82, 255, 255) };

    private int currentTarget = 0;
    [SerializeField] Image colorImage;
    private int nextTarget = 0;
    [SerializeField] Image colorNextImage;
    private UIController uiController;

    private float time;
    [SerializeField] float timeToChangeColor;
    [SerializeField] float[] timeOfGame;

    [SerializeField] BackgroundController bgController;
    [SerializeField] AnimalSearching animalSearching;
    private int currentMapIndex;
    private int currentObjectIndex;

    private int remainingAnimals;
    private int rightIndex;
    private List<int> currentListAnswer;

    // Start is called before the first frame update
    void Start()
    {
        uiController = GetComponent<UIController>();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        UpdateSlider();

        if(time < 0)
        {
            GameOver();
        }
    }

    public void UpdateSlider()
    {
        uiController.UpdateSlider(time);
    }

    public void SetSlider(int index)
    {
        uiController.SetSlider(timeOfGame[index]);
    }

    public void OnPressHandle(int indexButton)
    {
        if(indexButton == rightIndex)
        {
            UpdateScore();
            NextTurn();
        }
        else
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        uiController.GameOver();
    }

    public void UpdateScore()
    {
        score++;
        if(highscore < score)
        {
            highscore = score;
            PlayerPrefs.SetInt("score", highscore);
            uiController.UpdateHighScore(highscore);
        }
        uiController.UpdateScore(score);
    }

    public void SetCurrentTarget(int target)
    {
        currentTarget = currentObjectIndex;
        animalSearching.SetTarget(currentTarget);
    }

    public void RandomMap()
    {
        currentMapIndex = bgController.HandleShowBG();
        allObject = animalSearching.GetAmountAnimals();
        int amountAllAnimal = allObject;
        int maxAnimalInMap = bgController.GetMaxAnimal();
        maxObject = (amountAllAnimal > maxAnimalInMap) ? maxAnimalInMap : amountAllAnimal;
        if(maxObject > 20) maxObject = 20;
        NextTurn();
    }

    private int maxObject;
    private int allObject;
    public void NextTurn()
    {
        remainingAnimals = Random.Range(3, maxObject);
        currentObjectIndex = Random.Range(0, allObject);
        bgController.SpawAnimals(currentObjectIndex, remainingAnimals);

        InitListAnswer();
        animalSearching.UpdateTextButton(currentListAnswer);

        SetCurrentTarget(currentObjectIndex);

        if(remainingAnimals > 15)
        {
            time = timeOfGame[2];
            SetSlider(2);
        }else if(remainingAnimals > 10)
        {
            time = timeOfGame[1];
            SetSlider(1);
        }
        else
        {
            time = timeOfGame[0];
            SetSlider(0);
        }
        
    }

    public void InitListAnswer()
    {
        currentListAnswer = new List<int> { 0, 0, 0, 0};
        rightIndex = Random.Range(0, 4);
        for(int i = 0; i < currentListAnswer.Count; i++)
        {
            if(rightIndex == i)
            {
                currentListAnswer[i] = remainingAnimals;
            }
            else
            {
                currentListAnswer[i] = Random.Range(1,25);
                while (currentListAnswer[i] == remainingAnimals)
                {
                    currentListAnswer[i] = Random.Range(1, 25);
                }
            }
        }
    }

    public void Reset()
    {
        Time.timeScale = 1;

        RandomMap();
        
        score = 1;
        uiController.UpdateScore(score);
        uiController.UpdateHighScore(PlayerPrefs.GetInt("score"));
    }

}
