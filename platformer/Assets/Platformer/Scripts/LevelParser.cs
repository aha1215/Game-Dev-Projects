using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelParser : MonoBehaviour
{
    public string filename;
    [FormerlySerializedAs("Rock")] 
    public GameObject rockPrefab;
    
    [FormerlySerializedAs("Brick")] 
    public GameObject brickPrefab;
    
    [FormerlySerializedAs("QuestionBox")] 
    public GameObject questionBoxPrefab;
    
    [FormerlySerializedAs("Stone")] 
    public GameObject stonePrefab;

    [FormerlySerializedAs("Water")] 
    public GameObject waterPrefab;

    [FormerlySerializedAs("Flag")] 
    public GameObject flagPrefab;
    
    public Transform environmentRoot;

    // --------------------------------------------------------------------------
    void Start()
    {
        LoadLevel();
    }

    // --------------------------------------------------------------------------
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadLevel();
        }
    }

    // --------------------------------------------------------------------------
    private void LoadLevel()
    {
        string fileToParse = $"{Application.dataPath}{"/Resources/"}{filename}.txt";
        Debug.Log($"Loading level file: {fileToParse}");

        Stack<string> levelRows = new Stack<string>();

        // Get each line of text representing blocks in our level
        using (StreamReader sr = new StreamReader(fileToParse))
        {
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                levelRows.Push(line);
            }

            sr.Close();
        }

        // Go through the rows from bottom to top
        int row = 0;
        while (levelRows.Count > 0)
        {
            string currentLine = levelRows.Pop();

            int column = 0;
            char[] letters = currentLine.ToCharArray();
            foreach (var letter in letters)
            {
                // Instantiate a new GameObject that matches the type specified by letter
                // Position the new GameObject at the appropriate location by using row and column
                // Parent the new GameObject under levelRoot
                
                if (letter == 'x')
                {
                    var rockObject = Instantiate(rockPrefab, environmentRoot);
                    rockObject.transform.position = new Vector3(column + 0.5f, row + 0.5f, 0f);
                }

                if (letter == '?')
                {
                    var questionBoxObject = Instantiate(questionBoxPrefab, environmentRoot);
                    questionBoxObject.transform.position = new Vector3(column + 0.5f, row + 0.5f, 0f);
                }

                if (letter == 'b')
                {
                    var brickObject = Instantiate(brickPrefab, environmentRoot);
                    brickObject.transform.position = new Vector3(column + 0.5f, row + 0.5f, 0f);
                }
                
                if (letter == 's')
                {
                    var stoneObject = Instantiate(stonePrefab, environmentRoot);
                    stoneObject.transform.position = new Vector3(column + 0.5f, row + 0.5f, 0f);
                }

                if (letter == 'w')
                {
                    var waterObject = Instantiate(waterPrefab, environmentRoot);
                    waterObject.transform.position = new Vector3(column + 0.5f, row + 0.5f, 0f);
                }

                if (letter == 'f')
                {
                    var flagObject = Instantiate(flagPrefab, environmentRoot);
                    flagPrefab.transform.position = new Vector3(column + 0.5f, row + 0.5f, 0f);
                }
                
                column++;
            }
            row++;
        }
    }

    // --------------------------------------------------------------------------
    public void ReloadLevel()
    {
        foreach (Transform child in environmentRoot)
        {
           Destroy(child.gameObject);
        }
        LoadLevel();
    }
}
