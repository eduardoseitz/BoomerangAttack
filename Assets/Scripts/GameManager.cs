using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Declarations
    // Singleton
    public static GameManager instance;

    public bool IsGameRunning { get; private set; }
    public int BoomerangCount { get; private set; }
    public int FruitsCount { get; private set; }
    public int StarCount { get; private set; }
    public int ScoreCount { get; private set; }

    public Level[] levels;

    [SerializeField] float endGameDelay = 3f;

    private int _currentLevel;
    private bool _hasShowedCredits;
    #endregion

    #region Main Methods
    private void Awake()
    { 
        // Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.R))
            RestartLevel();*/
    }
    #endregion

    #region Helper Methods
    public void UseBoomerang()
    {
        if (BoomerangCount > 0)
        {
            BoomerangCount--;
            UIManager.instance.UpdateBoomerangsOnHud();
        }

        if (BoomerangCount == 0)
        {
            Invoke(nameof(EndGame), endGameDelay);
        }
    }

    public void KillFruit()
    {
        FruitsCount--;
        UIManager.instance.UpdateFruitsOnHud();
        if (FruitsCount == 0)
        {
            Invoke(nameof(EndGame), endGameDelay);
        }
    }

    public void LoadLevel(int levelNumber)
    {
        _currentLevel = levelNumber;
        StartCoroutine(LoadSceneAsync(levels[levelNumber].sceneIndex));
    }

    public void ExitLevel()
    {
        IsGameRunning = false;
        StartCoroutine(UnloadSceneAsync(levels[_currentLevel].sceneIndex));
    }

    /*public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }*/

    private void SetupLevel()
    {
        // Play music
        AudioManager.instance.SwapMusic(levels[_currentLevel].musicClip);

        // Reset values
        BoomerangCount = 4;
        //FruitsCount = FindObjectsOfType<FruitBehaviour>().Length;
        FruitsCount = 3;

        // Update hud
        UIManager.instance.UpdateChargeHud(0);
        UIManager.instance.UpdateBoomerangsOnHud();
        UIManager.instance.UpdateFruitsOnHud();
        UIManager.instance.ShowHUD();

        // Unpause Game
        IsGameRunning = true;

        Debug.Log($"Level set with {BoomerangCount} boomerangs and {FruitsCount} fruits.");
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation _loadingOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        while (_loadingOperation.isDone == false)
        {
            Debug.Log($"Level 0{_currentLevel + 1} has loaded {_loadingOperation.progress} %");
            yield return new WaitForSeconds(0.1f);
        }

        SetupLevel();
    }
    private IEnumerator UnloadSceneAsync(int sceneIndex)
    {
        AsyncOperation _loadingOperation = SceneManager.UnloadSceneAsync(sceneIndex);
        while (_loadingOperation.isDone == false)
        {
            Debug.Log($"Level 0{_currentLevel + 1} has unloaded {_loadingOperation.progress} %");
            yield return new WaitForSeconds(0.1f);
        }

        if (_currentLevel == levels.Length - 1 && levels[_currentLevel].starCount > 0 && _hasShowedCredits == false)
        {
            UIManager.instance.ShowCredits();
            _hasShowedCredits = true;
        }
        else
            UIManager.instance.ShowLevelSelector();
    }

    private void EndGame()
    {
        if (IsGameRunning)
        {
            Debug.Log("Game Over");
            IsGameRunning = false;

            if (FruitsCount == 0)
            {
                Debug.Log("You won!");

                // Unlock next level
                if (_currentLevel + 1 < levels.Length)
                    levels[_currentLevel + 1].isUnlocked = true;


                    // Update end game UI
                    StarCount = BoomerangCount + 1;
                ScoreCount = StarCount * 1000 + (Random.Range(101, 499));
                if (StarCount > levels[_currentLevel].starCount)
                {
                    levels[_currentLevel].starCount = StarCount;
                }
                if (ScoreCount > levels[_currentLevel].scoreRecord)
                {
                    levels[_currentLevel].scoreRecord = ScoreCount;

                    UIManager.instance.ShowEndCanvas("Nicely done!", "Score: " + ScoreCount, StarCount, true);
                }
                else
                {
                    UIManager.instance.ShowEndCanvas("Nicely done!", "Score: " + ScoreCount, StarCount, false);
                }

                // Play sound
                AudioManager.instance.PlayWinSound();
            }
            else
            {
                Debug.Log("You lost!");

                // Update hud
                UIManager.instance.ShowEndCanvas("Oh no, you lost!", "Better luck next time", 0, false);
                
                // Play sound
                AudioManager.instance.PlayLoseSound();
            }
        }
    }
    #endregion
}
