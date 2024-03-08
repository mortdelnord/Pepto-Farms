using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;


public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugginh")]
    [SerializeField] private bool initializeDataIfNull = false;

    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;


    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler dataHandler;

    private string selectedProfileId = "";
    
    public static DataPersistenceManager instance { get; private set; }

    



    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying Newest one ");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        if (disableDataPersistence)
        {
            Debug.LogWarning("Data persistence is disabaled");
        }
        
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
        if (overrideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Overrode sleected profile id with test profile id");
        }

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("On scene loaded Called");
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();

    }
    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("On scene unlaoded called");
        SaveGame();
    }



    public void ChangeSelectedProfileId(string newProfileId)
    {
        this.selectedProfileId = newProfileId;

        LoadGame();
    }
   
    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {

        if (disableDataPersistence)
        {
            return;
        }
        //TODO - Load any saved data from a file handler
        this.gameData = dataHandler.Load(selectedProfileId);

        // start new game if the data is null and and we're configured for debugging

        if (this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }

        //if no datat can be Loaded, initialize to a new game
        if (this.gameData == null)
        {
            Debug.Log("No Data was found. A new Game needs to be started before dtata can be loaded");
            return;
        }
        //TODO - push the Loaded data to all othe scripts that need it
        foreach( IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

        Debug.Log("Loaded death count = " + gameData.deathCount);

    }

    public void SaveGame()
    {
        if (disableDataPersistence)
        {
            return;
        }

        if (this.gameData == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }
        //TODO - pass the data to other scripts so they can Update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }


        gameData.lastUpdated = System.DateTime.Now.ToBinary();
        //Debug.Log("Saved death count = " + gameData.deathCount);

        //TODO - save that data to a file using the data handler
        dataHandler.Save(gameData, selectedProfileId);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }
}
