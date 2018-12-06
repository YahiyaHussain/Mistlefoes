using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Base : Highlightable {


    private int population;

    public int startingMaxPopulation;
    private int maxPopulation;

    public int startingPopulation;
    public Team startingTeam;

    private RectTransform rt;
    private Pool TCP;
    private Camera c;

    //canvas pool automatically assigns a canvas to this base
    [HideInInspector]
    public Transform myCanvas;
    private TextMeshProUGUI TMP;

    private EnemyAI EAI;

    float[] bounceScalesX;
    float[] bounceScalesY;

    float[] stretchScalesX;
    float[] stretchScalesY;
    float twoPI = (float) Math.PI;

    float originalScaleZ;
    float originalScaleX;
    float originalScaleY;

    Queue<popOp> populationOperations;
    public Team myTeam()
    {
        return currentTeam;
    }

    public int myPopulation()
    {
        return population;
    }

    public int myMaxPopulation()
    {
        return maxPopulation;
    }

    public void Capture(Team t)
    {
        currentTeam = t;
    }
    private void Update()
    {
        if (populationOperations.Count > 0)
        {
            popOp tempPopOP = populationOperations.Dequeue();
            switch (tempPopOP.type)
            {
                #region InvadingForce
                case popOpType.InvadingForce:
                    if (tempPopOP.unitsTeam.Equals(currentTeam))
                    {
                        population += tempPopOP.amountChanged;
                    }
                    else
                    {
                        if (population - tempPopOP.amountChanged < 0)
                        {
                            changeTeamTo(tempPopOP.unitsTeam);
                            makeCorrectTeamSprite();
                            population = tempPopOP.amountChanged - population;
                        }
                        else
                        {
                            population -= tempPopOP.amountChanged;
                        }
                    }
                    break;
                #endregion
                #region LeavingForce
                case popOpType.LeavingForce:
                    if (tempPopOP.unitsTeam.Equals(currentTeam))
                    {
                        population -= tempPopOP.amountChanged;
                    }
                    break;
                #endregion
                #region BombstrikeOnBase
                case popOpType.BombstrikeOnBase:
                    //Debug.Log("Before: " + population);
                    if (population - tempPopOP.amountChanged <= 0)
                    {
                        population = 1;
                    }
                    else
                    {
                        population -= tempPopOP.amountChanged;
                    }
                    //Debug.Log("After: " + population);
                    break;
                #endregion
                #region NewSoldierBorn
                case popOpType.NewSoldierBorn:
                    population++;
                    break;
                #endregion
                #region SoldierStarvedToDeath
                case popOpType.SoldierStarvedToDeath:
                    population--;
                    break;
                    #endregion
            }
            updatePopulationDisplay();
        }


    }
    public void affectPopulation(popOp populationOperation)
    {
        populationOperations.Enqueue(populationOperation);
    }
    private void updatePopulationDisplay()
    {
        TMP.text = population.ToString();
    }

    private void Start()
    {
        TCP = GameObject.FindGameObjectWithTag("TroopContainerPool").GetComponent<Pool>();
    currentTeam = startingTeam;
        initializeHighlightablePart();
        maxPopulation = startingMaxPopulation;
        population = startingPopulation;
        bounceScalesX = new float[48];
        bounceScalesY = new float[48];
        stretchScalesX = new float[48];
        stretchScalesY = new float[48];
        for(int i = 0; i < 48; i++)
        { 
            bounceScalesX[i] = ((48 - i) / 64.0f * Mathf.Sin(twoPI * i / 12));
            bounceScalesY[i] = ((48 - i) / 64.0f * Mathf.Sin(twoPI * i / 12));
            //stretchScalesY[i] = ((48 - i)) / 64.o
        }
        
        EAI = GameObject.FindGameObjectWithTag("EnemyAI").GetComponent<EnemyAI>();
        TMP = myCanvas.GetComponentInChildren<TextMeshProUGUI>();

        updatePopulationDisplay();

        originalScaleZ = transform.localScale.z;
        originalScaleX = transform.localScale.x;
        originalScaleY = transform.localScale.y;
    }
    private void Awake()
    {
        populationOperations = new Queue<popOp>();
    }
    public void startClone()
    {
        TCP = GameObject.FindGameObjectWithTag("TroopContainerPool").GetComponent<Pool>();
    originalScaleZ = transform.localScale.z;
        originalScaleX = transform.localScale.x;
        originalScaleY = transform.localScale.y;

        currentTeam = startingTeam;
        initializeHighlightablePart();
        maxPopulation = startingMaxPopulation;
        population = startingPopulation;
        bounceScalesX = new float[48];
        bounceScalesY = new float[48];

        for (int i = 0; i < 48; i++)
        {
            bounceScalesX[i] = ((48 - i) / 64.0f * Mathf.Sin(twoPI * i / 12));
            bounceScalesY[i] = ((48 - i) / 64.0f * Mathf.Sin(twoPI * i / 12));
        }

        EAI = GameObject.FindGameObjectWithTag("EnemyAI").GetComponent<EnemyAI>();
        TMP = myCanvas.GetComponentInChildren<TextMeshProUGUI>();

        makeCorrectTeamSprite();
        updatePopulationDisplay();
    }

    private void changeTeamTo(Team newTeam)
    {
        currentTeam = newTeam;
        makeCorrectTeamSprite();
        if (EAI.myTeam == newTeam)
            EAI.declareBaseMine(gameObject.GetComponent<Base>());
        else
            EAI.declareBaseNoLongerMine(gameObject.GetComponent<Base>());

        bob();
    }

    public IEnumerator bounceHelp()
    {

        for (int i = 0; i < 48  ; i +=2)
        {
            transform.localScale = new Vector3((-bounceScalesX[i] + 1f)* originalScaleX, (bounceScalesY[i] + 1f)* originalScaleY, 1);
            yield return new WaitForSeconds(.01f);
        }
        transform.localScale = new Vector3(originalScaleX, originalScaleY, originalScaleZ);
    }
    public void bounce()
    {
        StartCoroutine(bounceHelp());
    }
    private IEnumerator stretchHelp()
    {
        for (int i = 0; i < 48; i++)
        {
            transform.localScale = new Vector3((0.12f*bounceScalesX[i] + 1f) * originalScaleX, (-.25f*bounceScalesY[i] + 1f) * originalScaleY, 1);
            yield return null;
        }
        transform.localScale = new Vector3(originalScaleX, originalScaleY, originalScaleZ);
    }
    private IEnumerator smushHelp()
    {
        for (int i = 0; i < 48; i++)
        {
            transform.localScale = new Vector3((bounceScalesX[i] + 1f) * originalScaleX, (-0.1f*bounceScalesY[i] + 1f) * originalScaleY, 1);
            yield return new WaitForSeconds(.01f);
        }
        transform.localScale = new Vector3(originalScaleX, originalScaleY, originalScaleZ);
    }

    public void stretch()
    {
        StartCoroutine(stretchHelp());
    }
    public void smush()
    {
        StartCoroutine(smushHelp());
    }

    public void bob()
    {
        StartCoroutine(bobHelp());
    }
    private IEnumerator bobHelp()
    {
        for (int i = 0; i < 48; i += 2)
        {
            transform.localScale = new Vector3((1f * bounceScalesX[i] + 1f) * originalScaleX, (-1f * bounceScalesY[i] + 1f) * originalScaleY, 1);
            yield return null;
        }
    }
    public IEnumerator SendTroopsFromBaseToTargetBase(Base target, Team senderTeam)
    {
        //reference to this coroutine's troopcontainer
        TroopContainer tempTroopContainer;

        //you take half the population at a time
        int tempTroopNumber = population / 2;
        affectPopulation(new popOp(tempTroopNumber, senderTeam, popOpType.LeavingForce));
        stretch();
        //the troopcontainerpool sure is neat
        tempTroopContainer = TCP.GiveOutUnit().GetComponent<TroopContainer>();

        //the troopContainer determines starting location of all units so should be at the base's location
        tempTroopContainer.transform.position = transform.position;

        //its ready to live once again
        tempTroopContainer.gameObject.SetActive(true);

        //command the troopContainer To tell its units to go to target
        tempTroopContainer.GoToTargetBase(target.gameObject, tempTroopNumber, myTeam());
        yield return null;
    }
}

public struct popOp
{
    public int amountChanged;
    public Team unitsTeam;
    public popOpType type;
    public popOp(int amountChanged, Team unitsTeam, popOpType type)
    {
        this.amountChanged = amountChanged;
        this.unitsTeam = unitsTeam;
        this.type = type;
    }
}
