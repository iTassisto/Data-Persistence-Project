using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartManager : MonoBehaviour
{
    [SerializeField] GameObject nameInput;
    [SerializeField] GameObject submitButton;
    [SerializeField] GameObject quitButton;
    [SerializeField] TextMeshProUGUI instructionText;

    public static StartManager Instance;

    public static string PlayerName { get; set; }
    public static string HighScoreName { get; set; }
    public static int HighScorePoints { get; set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHighScore();
        instructionText.text = $"High score : {HighScoreName} : {HighScorePoints}";
    }

    [Serializable]
    class SaveData
    {
        public string PlayerName;
        public int HighScore;
    }

    public static void SaveHighScore(string name, int points)
    {
        SaveData data = new SaveData();
        data.PlayerName = name;
        data.HighScore = points;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public static void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            HighScoreName = data.PlayerName;
            HighScorePoints = data.HighScore;
        }
    }

    public void StartGame()
    {
        if (string.IsNullOrEmpty(nameInput.GetComponent<TMP_InputField>().text))
        {
            instructionText.text = "Provide your name to start the game.";
        } else
        {
            if (nameInput != null && submitButton != null)
            {
                PlayerName = nameInput.GetComponent<TMP_InputField>().text;
                nameInput.SetActive(false);
                submitButton.SetActive(false);
                quitButton.SetActive(false);
                instructionText.gameObject.SetActive(false);
                SceneManager.LoadScene(1);
            }
        }        
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
