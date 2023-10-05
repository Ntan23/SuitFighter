using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;

    void Awake()
    {
        if(instance == null) instance = this;
    }
    #endregion

    #region BoolVariables
    private bool isShow;
    public bool isEnded;
    private bool isWin;
    private bool isTie;
    public bool canPlay;
    public bool isBattling;
    // private bool canPowerUp;
    // private bool canUseHint = true;
    private bool isAttack;
    private bool isDialogueShowed;
    #endregion

    #region FloatVariable
    private float timerEachRound;
    [SerializeField] private float maxTimeForEachRound;
    [SerializeField] private float timeToShowDialogue;
    private float randomValue, hintRandomValue;
    private float combinedProbability, paperProbability, scissorsProbability;
    #endregion

    #region IntegerVariables
    private int roundIndex, cardIndex, randomIndex, random;
    [SerializeField] private int levelIndex;
    [SerializeField] private int maxRound;
    //private int rockCount, paperCount, scissorsCount;
    private int healthCount;
    [SerializeField] private int enemyHealthCount;
    #endregion

    #region Vector3Varibles
    private Vector3[] cardInitialLocalPosition = new Vector3[3];
    private Vector3 enemyChoiceInitialPosition;
    #endregion

    #region StringVariables
    [SerializeField] private string oneHealthDialogue;
    [SerializeField] private string intialDialogue;
    [SerializeField] private string[] randomDialogue;
    #endregion

    #region OtherVariables
    [SerializeField] private EnemyChoice[] choices;
    [SerializeField] private Enemy3Dialogue[] enemy3Dialogues;
    //[SerializeField] private string[] hintsToBuy;
    private string[] generatedChoices = new string[7];
    private string[] generatedHints = new string[7];
    private string[] generatedDialogues = new string[7];
    //private string generatedBoughtHint;
    [SerializeField] private HintBoxUI hintBoxUI;
    [SerializeField] private DialogueHintUI[] dialogueHintUI;
    [SerializeField] private LivesCountUI[] livesCountUIs;
    [SerializeField] private VictoryLoseUI[] victoryLoseUI;
    [SerializeField] private Image AIChoiceImage;
    [SerializeField] private Image timerImage;
    [SerializeField] private Sprite rock, paper, scissors;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private GameObject[] cards;
    [SerializeField] private GameObject versusText;
    [SerializeField] private PlayerHandAnimation playerHandAnimation;
    [SerializeField] private EnemyAnimation enemyAnimation;
    [SerializeField] private Fade fade;
    [SerializeField] private GameObject tieText;
    [SerializeField] private GameObject rematchText;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private PillAndHintUI pillAndHintUI;
    private AudioManager am;
    #endregion

    void Start()
    {
        am = AudioManager.instance;

        healthCount = PlayerPrefs.GetInt("Health", 3);

        for(int i = 0; i < maxRound; i++)
        {
            GenerateRandomChoices(i);

            
            // randomIndex = Random.Range(0, choices.Length);
            // generatedChoices[i] = choices[randomIndex].choiceType;

            // if(generatedChoices[i] == "Rock") rockCount++;
            // if(generatedChoices[i] == "Paper") paperCount++;
            // if(generatedChoices[i] == "Scissors") scissorsCount++;

            // randomHintIndex = Random.Range(0, choices[randomIndex].hints.Length);
            // generatedHints[i] = choices[randomIndex].hints[randomHintIndex];
        }


        // if(rockCount > paperCount && rockCount > scissorsCount) generatedBoughtHint = hintsToBuy[0];
        // else if(paperCount > rockCount && paperCount > scissorsCount) generatedBoughtHint = hintsToBuy[1];
        // else if(scissorsCount > paperCount && scissorsCount > rockCount) generatedBoughtHint = hintsToBuy[2];
        // else 
        // {
        //     int random = Random.Range(0, hintsToBuy.Length);

        //     generatedBoughtHint = hintsToBuy[random];
        // }

        enemyChoiceInitialPosition = AIChoiceImage.transform.localPosition;

        timerEachRound = maxTimeForEachRound;
        roundIndex = 0;
    }

    void Update()
    {
        if(canPlay && !isEnded)
        {
            if(roundIndex == 0 && !isDialogueShowed && !isShow) 
            {
                if(levelIndex < 3)
                {
                    isShow = true;
                    ShowDialogue(intialDialogue);
                }
                else if(levelIndex == 3)
                {
                    isShow = true;
                    random = Random.Range(0, generatedDialogues.Length);
                    ShowDialogue(generatedDialogues[random]);
                }
            }

            timerEachRound -= Time.deltaTime;
            timerImage.fillAmount = timerEachRound/maxTimeForEachRound;

            if(timerEachRound <= 0)
            {   
                if(!isAttack) 
                {
                    dialogueHintUI[0].HideDialogue();
                    dialogueHintUI[1].HideDialogue();
                    StartCoroutine(AttackedWhenIdle());
                }
                //isDialogueShowed = false;
            }

            CheckDialogue();
            
            // if((Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) && canPowerUp && healthCount > 0) 
            // {
            //     Debug.Log("Decrese Enemy Health Instantly !");
            //     livesCountUIs[2].DecreaseHealth();
            //     canPowerUp = false;
            // }

            // if((Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) && canUseHint && healthCount > 1)
            // {
            //     Debug.Log("Show Hint");
            //     livesCountUIs[1].DecreaseHealth();
            //     dialogueHintUI[0].ShowDialogue(generatedHints[roundIndex]);
            //     canUseHint = false;
            // }

            // if(timerEachRound <= timeToShowDialogue)
            // {
            //     if(!isDialogueShowed) 
            //     {
            //         dialogueHintUI.ShowDialogueHint(generatedHints[roundIndex]);
            //         isDialogueShowed = true;
            //     }
            // }
        }
    }

    private void GenerateRandomChoices(int index)
    {
        randomValue = Random.value;

        //Debug.Log("Random Value 1 : " + randomValue + "(" + index + ")");

        if(randomValue <= choices[0].chances) 
        {
            generatedChoices[index] = choices[0].choiceType;
            GenerateHint(index, 0);

            if(levelIndex == 3) GenerateDialogue(index, 0);
        }
        else 
        {
            combinedProbability = 1.0f - choices[0].chances;

            paperProbability = choices[1].chances / combinedProbability;
            scissorsProbability = choices[2].chances / combinedProbability;

            randomValue = Random.value;
            // Debug.Log("Random Value 2 : " + randomValue  + "(" + index + ")");

            if(randomValue <= paperProbability) 
            {
                generatedChoices[index] = choices[1].choiceType;
                GenerateHint(index, 1);

                if(levelIndex == 3) GenerateDialogue(index, 1);
            }
            else 
            {
                generatedChoices[index] = choices[2].choiceType;
                GenerateHint(index, 2);

                if(levelIndex == 3) GenerateDialogue(index, 2);
            }
        }
    }

    private void GenerateHint(int index, int valueIndex)
    {
        if(levelIndex < 3)
        {
            hintRandomValue = Random.value;

            if(hintRandomValue <= 0.5f) generatedHints[index] = choices[valueIndex].hints[0];
            else generatedHints[index] = choices[valueIndex].hints[1];
        }
        
        if(levelIndex == 3)
        {
            int hintRandomIndex = Random.Range(0,3);

            generatedHints[index] = choices[valueIndex].hints[hintRandomIndex];
        }
    }

    private void GenerateDialogue(int index, int valueIndex)
    {
        hintRandomValue = Random.value;

        if(hintRandomValue <= 0.5f) generatedDialogues[index] = enemy3Dialogues[valueIndex].dialogues[0];
        else generatedDialogues[index] = enemy3Dialogues[valueIndex].dialogues[1];
    }

    public void Play(string myChoice)
    {
        if(canPlay && !isEnded)
        {
            isAttack = true;
            string randomChoice = generatedChoices[roundIndex];

            switch(randomChoice)
            {
                case "Rock":
                    switch(myChoice)
                    {
                        case "Rock":
                            cardIndex = 0;
                            isTie = true;
                            break;
                        case "Paper":
                            cardIndex = 1;
                            isWin = true;
                            isTie = false;
                            break;
                        case "Scissors":
                            cardIndex = 2;
                            isWin = false;
                            isTie = false;
                            break;
                    }

                    AIChoiceImage.sprite = rock;
                    break;

                case "Paper":
                    switch(myChoice)
                    {
                        case "Rock":
                            cardIndex = 0;
                            isWin = false;
                            isTie = false;
                            break;
                        case "Paper":
                            isTie = true;
                            cardIndex = 1;
                            break;
                        case "Scissors":
                            cardIndex = 2;
                            isWin = true;
                            isTie = false;
                            break;
                    }

                    AIChoiceImage.sprite = paper;
                    break;

                case "Scissors":
                    switch(myChoice)
                    {
                        case "Rock":
                            cardIndex = 0;
                            isWin = true;
                            isTie = false;
                            break;
                        case "Paper":
                            cardIndex = 1;
                            isWin = false;
                            isTie = false;
                            break;
                        case "Scissors":
                            isTie = true;
                            cardIndex = 2;
                            break;
                    }

                    AIChoiceImage.sprite = scissors;
                    break;
            }

            StartCoroutine(BattleAnimation(cardIndex));
        }
        else if(!canPlay) return;
    }

    public void BuyPowerPill(int cost) 
    {
        livesCountUIs[0].DecreaseHealth();
        livesCountUIs[1].UpdateLivesCount();
        pillAndHintUI.ChangeCanUsePill();
        //hintBoxUI.ShowHintBoxAndUpdate(generatedBoughtHint);
    }

    void ShowDialogue(string text)
    {
        isDialogueShowed = true;
        dialogueHintUI[1].ShowDialogue(text);
    }

    void CheckDialogue()
    {
        if(timerEachRound <= 4.0f)
        {
            if(isDialogueShowed)
            {
                dialogueHintUI[1].HideDialogue();
                isDialogueShowed = false;
            }
        }
    }

    IEnumerator BattleAnimation(int index)
    {
        canPlay = false;
        isBattling = true;
        timerEachRound = 0.0f;
        timerImage.fillAmount = 0.0f;
        yield return new WaitForSeconds(0.2f);
        LeanTween.scale(cards[index], Vector3.one, 0.5f);
        LeanTween.moveLocal(cards[index], new Vector3(-264.0f, 369.0f, 0.0f), 0.5f).setEaseSpring();
        LeanTween.scale(AIChoiceImage.gameObject, Vector3.one, 0.8f).setEaseSpring().setOnComplete(() => LeanTween.scale(versusText, Vector3.one, 0.5f));
        yield return new WaitForSeconds(1.8f);

        if(isWin && !isTie)
        {
            versusText.transform.localScale = Vector3.zero;
            LeanTween.scale(AIChoiceImage.gameObject, Vector3.zero, 0.8f);
            LeanTween.moveLocalX(cards[index], -7.0f, 0.5f).setEaseSpring().setOnComplete(() => LeanTween.scale(cards[index], new Vector3(1.2f, 1.2f, 1.2f), 0.3f));
            yield return new WaitForSeconds(1.5f);
            LeanTween.scale(cards[index], Vector3.one, 0.3f).setOnComplete(() => LeanTween.moveLocal(cards[index], cardInitialLocalPosition[index], 0.5f).setEaseSpring());
            yield return new WaitForSeconds(1.0f);

            if(index == 0) playerHandAnimation.PlayRockAnimation();
            if(index == 1) playerHandAnimation.PlayPaperAnimation();
            if(index == 2) playerHandAnimation.PlayScissorsAnimation();

            am.PlayPlayerAttackSFX();
            yield return new WaitForSeconds(0.3f);
            enemyAnimation.PlayAttackedAnimation();
            yield return new WaitForSeconds(0.5f);
            livesCountUIs[2].DecreaseHealth();
            playerHandAnimation.BackToIdle();
            enemyAnimation.BackToIdle();

            if(roundIndex <= maxRound - 1 && !isEnded)
            {
                if(roundIndex == maxRound -1 && enemyHealthCount > 0) 
                {
                    Debug.Log("Game Over");
                    timerEachRound = 0.0f;
                    timerImage.fillAmount = 0.0f;
                    victoryLoseUI[1].ShowUI();
                    isEnded = true;
                    canPlay = false;
                }

                if(roundIndex < maxRound - 1 && !isEnded)
                {
                    timerEachRound = maxTimeForEachRound;
                    roundIndex++;
                    isDialogueShowed = false;
                    UpdateRoundText();
                    if(enemyHealthCount > 0) canPlay = true;
                }
            }

            dialogueHintUI[0].HideDialogue();
            isBattling = false;
            isAttack = false;
            canPlay = true;

            if(roundIndex > 0 && !isDialogueShowed && !isEnded)
            {
                randomIndex = Random.Range(0, randomDialogue.Length);

                if(enemyHealthCount > 1) 
                {
                    if(levelIndex < 3) ShowDialogue(randomDialogue[randomIndex]);
                    else if(levelIndex == 3) ShowDialogue(generatedDialogues[roundIndex]);
                }
                else if(enemyHealthCount == 1) 
                {
                    if(levelIndex < 3) ShowDialogue(oneHealthDialogue);
                    else if(levelIndex == 3)
                    {
                        random = Random.Range(0, generatedDialogues.Length);
                        ShowDialogue(generatedDialogues[random]);
                    }
                }
            }

            pillAndHintUI.ChangeBackCanUseHint();
        }
        
        if(!isWin && !isTie)
        {
            versusText.transform.localScale = Vector3.zero;
            LeanTween.scale(cards[index], Vector3.zero, 0.8f);
            LeanTween.moveLocalX(AIChoiceImage.gameObject, -15.0f, 0.5f).setEaseSpring().setOnComplete(() => LeanTween.scale(AIChoiceImage.gameObject, new Vector3(1.2f, 1.2f, 1.2f), 0.3f));
            yield return new WaitForSeconds(1.5f);
            LeanTween.scale(AIChoiceImage.gameObject, Vector3.zero, 0.3f);
            LeanTween.scale(cards[index], Vector3.one, 0.3f).setOnComplete(() => LeanTween.moveLocal(cards[index], cardInitialLocalPosition[index], 0.5f).setEaseSpring());
            yield return new WaitForSeconds(1.0f);

            am.PlayPlayerAttackSFX();
            enemyAnimation.PlayAttackAnimation();
            yield return new WaitForSeconds(0.1f);
            cameraShake.ShakeCamera(5, 1.0f);
            playerHandAnimation.PlayGuardAnimation();
            yield return new WaitForSeconds(0.5f);
            livesCountUIs[1].DecreaseHealth();
            playerHandAnimation.BackToIdle();
            enemyAnimation.BackToIdle();

            AIChoiceImage.gameObject.transform.localPosition = enemyChoiceInitialPosition;
            
            if(roundIndex <= maxRound - 1 && !isEnded)
            {
                if(roundIndex == maxRound - 1 && !isEnded)
                {
                    if(enemyHealthCount > 0) 
                    {
                        Debug.Log("Game Over");
                        timerEachRound = 0.0f;
                        timerImage.fillAmount = 0.0f;
                        victoryLoseUI[1].ShowUI();
                        isEnded = true;
                        canPlay = false;
                    }
                }

                if(roundIndex < maxRound - 1)
                {
                    timerEachRound = maxTimeForEachRound;
                    roundIndex++;
                    isDialogueShowed = false;
                    UpdateRoundText();
                    if(enemyHealthCount > 0) canPlay = true;
                }
            }

            dialogueHintUI[0].HideDialogue();
            isBattling = false;
            isAttack = false;
            
            if(roundIndex > 0 && !isDialogueShowed && !isEnded)
            {
                randomIndex = Random.Range(0, randomDialogue.Length);

                if(enemyHealthCount > 1) 
                {
                    if(levelIndex < 3) ShowDialogue(randomDialogue[randomIndex]);
                    else if(levelIndex == 3) ShowDialogue(generatedDialogues[roundIndex]);
                }
                else if(enemyHealthCount == 1) 
                {
                    if(levelIndex < 3) ShowDialogue(oneHealthDialogue);
                    else if(levelIndex == 3)
                    {
                        random = Random.Range(0, generatedDialogues.Length);
                        ShowDialogue(generatedDialogues[random]);
                    }
                }
            }

            pillAndHintUI.ChangeBackCanUseHint();
        }

        if(isTie)
        {
            LeanTween.scale(versusText, Vector3.zero, 0.5f).setOnComplete(() => 
            {
                LeanTween.moveLocal(cards[index], cardInitialLocalPosition[index], 0.5f).setEaseSpring();
                LeanTween.scale(AIChoiceImage.gameObject, Vector3.zero, 0.5f).setOnComplete(() => 
                {
                    if(roundIndex <= maxRound - 1 && !isEnded)
                    {
                        if(roundIndex == maxRound - 1)
                        {
                            if(enemyHealthCount > 0) StartCoroutine(ResetScene());
                        }

                        if(roundIndex < maxRound - 1)
                        {
                            timerEachRound = maxTimeForEachRound;
                            roundIndex++;
                            isDialogueShowed = false;
                            if(enemyHealthCount > 0) canPlay = true;
                            UpdateRoundText();
                        }
                    }

                    dialogueHintUI[0].HideDialogue();
                    isBattling = false;
                    isAttack = false;

                    if(roundIndex > 0 && !isDialogueShowed && !isEnded)
                    {
                        randomIndex = Random.Range(0, randomDialogue.Length);

                        if(enemyHealthCount > 1) 
                        {
                            if(levelIndex < 3) ShowDialogue(randomDialogue[randomIndex]);
                            else if(levelIndex == 3) ShowDialogue(generatedDialogues[roundIndex]);
                        }
                        else if(enemyHealthCount == 1) 
                        {
                            if(levelIndex < 3) ShowDialogue(oneHealthDialogue);
                            else if(levelIndex == 3)
                            {
                                random = Random.Range(0, generatedDialogues.Length);
                                ShowDialogue(generatedDialogues[random]);
                            }
                        }
                    }

                    pillAndHintUI.ChangeBackCanUseHint();
                });
            });
        }
        // yield return new WaitForSeconds(2.0f);
        // LeanTween.scale(AIChoiceImage.gameObject, Vector3.zero, 0.5f).setEaseSpring().setOnComplete(() => 
        // {
        //     if(roundIndex < 5) 
        //     {
        //         roundIndex++;
        //         UpdateRoundText();
        //         timerEachRound = maxTimeForEachRound;
        //     }

        //     canPlay = true;
        // });
    }

    IEnumerator ResetScene()
    {
        canPlay = false;
        isEnded = true;
        timerEachRound = 0.0f;
        timerImage.fillAmount = 0.0f;
        LeanTween.scale(tieText, Vector3.one, 0.5f);
        yield return new WaitForSeconds(1.2f);
        LeanTween.scale(tieText, Vector3.zero, 0.5f).setOnComplete(() =>
        {
            LeanTween.scale(rematchText, Vector3.one, 0.5f);
        });
        yield return new WaitForSeconds(1.2f);
        fade.FadeOut();
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator AttackedWhenIdle()
    {
        canPlay = false;
        timerEachRound = 0.0f;
        am.PlayPlayerAttackSFX();
        enemyAnimation.PlayAttackAnimation();
        yield return new WaitForSeconds(0.1f);
        cameraShake.ShakeCamera(5, 1.0f);
        playerHandAnimation.PlayGuardAnimation();
        yield return new WaitForSeconds(0.5f);
        livesCountUIs[1].DecreaseHealth();
        playerHandAnimation.BackToIdle();
        enemyAnimation.BackToIdle();
        yield return new WaitForSeconds(1.0f);
        if(roundIndex < maxRound - 1 && !isEnded) 
        {
            roundIndex++;
            UpdateRoundText();
            dialogueHintUI[0].HideDialogue();
            timerEachRound = maxTimeForEachRound;
            isDialogueShowed = false;
            canPlay = true;
            pillAndHintUI.ChangeBackCanUseHint();

            if(roundIndex > 0 && !isDialogueShowed)
            {
                randomIndex = Random.Range(0, randomDialogue.Length);

                if(enemyHealthCount > 1) 
                {
                    if(levelIndex < 3) ShowDialogue(randomDialogue[randomIndex]);
                    else if(levelIndex == 3) ShowDialogue(generatedDialogues[roundIndex]);
                }
                else if(enemyHealthCount == 1) 
                {
                    if(levelIndex < 3) ShowDialogue(oneHealthDialogue);
                    else if(levelIndex == 3)
                    {
                        random = Random.Range(0, generatedDialogues.Length);
                        Debug.Log(random);
                        ShowDialogue(generatedDialogues[random]);
                    }
                }
            }
        }
        else if(roundIndex == maxRound - 1 && !isEnded)
        {
            pillAndHintUI.ChangeBackCanUseHint();

            if(enemyHealthCount > 0) 
            {
                Debug.Log("Game Over");
                victoryLoseUI[1].ShowUI();
                isEnded = true;
                timerEachRound = 0.0f;
                timerImage.fillAmount = 0.0f;
                canPlay = false;
            }
        }
    }

    public void DecreaseHealth(int cost) 
    {
        healthCount -= cost;

        if(healthCount == 0) 
        {
            Debug.Log("Game Over!");
            victoryLoseUI[1].ShowUI();
            isEnded = true;
            timerEachRound = 0.0f;
            timerImage.fillAmount = 0.0f;
            canPlay = false;
        }
    }

    public void DecreaseEnemyHealth(int cost)
    {
        enemyHealthCount -= cost;

        if(enemyHealthCount == 0 && roundIndex < maxRound - 1)
        {
            Debug.Log("You Win!");
            victoryLoseUI[0].ShowUI();
            isEnded = true;

            if(healthCount > 3 && healthCount <= 6) livesCountUIs[1].IncreaseHealth();
            if(healthCount <= 3)
            {
                for(int i = healthCount; i < 4; i++)
                {
                    livesCountUIs[1].IncreaseHealth();
                }
            }
            
            PlayerPrefs.SetInt("Health", healthCount);

            timerEachRound = 0.0f;
            timerImage.fillAmount = 0.0f;
            canPlay = false;
            if(levelIndex < 3) PlayerPrefs.SetInt("LevelReached", levelIndex + 1);
        }
        
        if(enemyHealthCount == 0 && roundIndex == maxRound - 1)
        {
            Debug.Log("You Win!");
            victoryLoseUI[0].ShowUI();
            isEnded = true;

            if(healthCount < 3)
            {
                for(int i = healthCount; i < 3; i++)
                {
                    livesCountUIs[1].IncreaseHealth();
                }
            }

            // else if(healthCount > 3) 
            // {
            //     for(int i = healthCount; i <= 3; i++)
            //     {
            //         livesCountUIs[1].DecreaseHealth();
            //     }
            // }

            PlayerPrefs.SetInt("Health", 3);
            if(levelIndex < 3) PlayerPrefs.SetInt("LevelReached", levelIndex + 1);

            timerEachRound = 0.0f;
            timerImage.fillAmount = 0.0f;
            canPlay = false;
        }
    }

    public void IncreaseHealth(int count) => healthCount += count;

    private void UpdateRoundText() => roundText.text = "Round " + (roundIndex + 1) + "/" + maxRound;

    public void SaveCardInitialPosition()
    {
        for(int i = 0; i < cards.Length; i++)
        {
            cardInitialLocalPosition[i] = cards[i].transform.localPosition;
        }
    }
    
    public void ChangeIsDialogueShowedValue() => isDialogueShowed = false;

    public int GetHealth() 
    {
        return healthCount;
    }

    public int GetEnemyHealth()
    {
        return enemyHealthCount;
    }

    public string GetHint()
    {
        return generatedHints[roundIndex];
    }

    public bool GetIsDialogueShowed()
    {
        return isDialogueShowed;
    }
}
