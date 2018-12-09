using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour 
{
    public GameObject MainMenu;
    public GameObject SelectMenu;
    public GameObject OptionsMenu;
    public GameObject HowToMenu;

    public GameObject ChristmasText;
    public GameObject HalloweenText;

    public GameObject ElfButton;
    public GameObject BonesButton;

    public GameObject DefaultHouse;
    public GameObject HalloweenHouse;
    public GameObject ChristmasHouse;

    public TextMeshProUGUI AIText; 

    private Team TeamSelected = Team.Merry;
    private AlgorithmType enemyAI = AlgorithmType.Random;
    private int LevelSelected = 0;

    //_________________Initialize___________________
    private void Awake()
    {
        DontDestroyOnLoad(transform.parent);
    }

    //_________________View Choices_________________
    public Team selectedTeam() { return TeamSelected; }

    public AlgorithmType chosenEnemyAI() { return enemyAI; }

    //__________________Main Menu____________________
    public void GoToHowToPlay ()
    {
        FindObjectOfType<AudioManager>().PlaySound("MenuClick");
        HowToMenu.gameObject.SetActive(true);
        MainMenu.gameObject.SetActive(false);
    }

    public void PlayGame()
    {
        FindObjectOfType<AudioManager>().PlaySound("MenuClick");
        FindObjectOfType<AudioManager>().StopMusic("OhComeAllYeHaunted");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
        foreach(Transform t in transform.parent)
        {
            if (t.tag == "MenuManager")
            {
                continue;
            }
            else
            {
                t.gameObject.SetActive(false);
            }
        }
    }

    public void GoToSelect()
    {
        FindObjectOfType<AudioManager>().PlaySound("MenuClick");
        SelectMenu.gameObject.SetActive(true);
        MainMenu.gameObject.SetActive(false);
        //ChristmasText.gameObject.SetActive(false);
        //HalloweenText.gameObject.SetActive(false);
    }

    public void GoToOptions()
    {
        FindObjectOfType<AudioManager>().PlaySound("MenuClick");
        OptionsMenu.gameObject.SetActive(true);
        MainMenu.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        FindObjectOfType<AudioManager>().PlaySound("MenuClick");
        Application.Quit();
    }

    //__________________Select Menu____________________

    public void ChristmasTeamSelect()
    {
        BonesButton.GetComponent<Button>().interactable = true;
        ElfButton.GetComponent<Button>().interactable = false;
        TeamSelected = Team.Merry;
    }

    public void HalloweenTeamSelect()
    {
        ElfButton.GetComponent<Button>().interactable = true;
        BonesButton.GetComponent<Button>().interactable = false;
        TeamSelected = Team.Spooky;
    }

    public void DefaultLevelSelect()
    {
        DefaultHouse.GetComponent<Button>().interactable = false;
        ChristmasHouse.GetComponent<Button>().interactable = true;
        HalloweenHouse.GetComponent<Button>().interactable = true;
        LevelSelected = 1;
    }

    public void ChristmasLevelSelect()
    {
        DefaultHouse.GetComponent<Button>().interactable = true;
        ChristmasHouse.GetComponent<Button>().interactable = false;
        HalloweenHouse.GetComponent<Button>().interactable = true;
        LevelSelected = 2;
    }

    public void HalloweenLevelSelect()
    {
        DefaultHouse.GetComponent<Button>().interactable = true;
        ChristmasHouse.GetComponent<Button>().interactable = true;
        HalloweenHouse.GetComponent<Button>().interactable = false;
        LevelSelected = 3;
    }

    public void PlayGameSpecific()
    {
        FindObjectOfType<AudioManager>().PlaySound("MenuClick");
    }

    public void PressRightArrow()
    {
        switch (AIText.text)
        {
            case "Easy":
                enemyAI = AlgorithmType.Greedy;
                AIText.text = "Norm";
                break;
            case "Norm":
                enemyAI = AlgorithmType.GreedyTurtle;
                AIText.text = "Hard";
                break;
            case "Hard":
                enemyAI = AlgorithmType.Random;
                AIText.text = "Easy";
                break;
        }
    }
    public void PressLeftArrow()
    {
        switch (AIText.text)
        {
            case "Easy":
                enemyAI = AlgorithmType.GreedyTurtle;
                AIText.text = "Hard";
                break;
            case "Norm":
                enemyAI = AlgorithmType.Random;
                AIText.text = "Easy";
                break;
            case "Hard":
                enemyAI = AlgorithmType.Greedy;
                AIText.text = "Norm";
                break;
        }
    }

    //__________________Options Menu____________________
    public void GoBackToMain()
    {
        FindObjectOfType<AudioManager>().PlaySound("MenuClick");
        SelectMenu.gameObject.SetActive(false);
        OptionsMenu.gameObject.SetActive(false);
        HowToMenu.gameObject.SetActive(false);
        MainMenu.gameObject.SetActive(true);
    }
}
