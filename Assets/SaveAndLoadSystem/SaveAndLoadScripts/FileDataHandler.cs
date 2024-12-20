using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool useEncryption = false;

    private readonly string encryptionCodeWord = "crow";
    private readonly string backupExtension = ".bak";

    public FileDataHandler(string dataDirPath, string dataFfileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFfileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load(string profileId, bool allowRestoreFromBackup = true)
    {

        //base case - if profile id is null

        if (profileId == null)
        {
            return null;
        }
        // use Path.Combine for different OS
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);

        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                // Load the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }


                //optionally decrypt the data
                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }
                // deserialize the data from Json back into the C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                if (allowRestoreFromBackup)
                {

                    Debug.LogWarning("Error occured when trying to load data from file: " + fullPath + "\n" + e);
                    bool rollBackSuccess = AttemptRollBack(fullPath);
                    if (rollBackSuccess)
                    {
                        loadedData = Load(profileId, false);
                    }
                }else
                {
                    Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
                }
            }
        }
        return loadedData;
    }

    public void Save(GameData data, string profileId)
    {

        if (profileId == null)
        {
            return;
        }
        // use Path.Combine for different OS
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        string backupFilePath = fullPath + backupExtension;
        try
        {
            // create the directory the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serialize the C# game data obect into Json
            string dataToStore = JsonUtility.ToJson(data, true);

            // optionally encrypt the data
            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            // write the serialzed data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

            //verify the newly saved file can be loaded successfully
            GameData verifiedGameData = Load(profileId);
            if (verifiedGameData != null)
            {
                File.Copy(fullPath, backupFilePath, true);
            }else
            {
                throw new Exception("Save file could not be verified");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        //LOOP OVER ALL DIRECTORY NAMES

        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach(DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            //defensive programming
            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skippping directory when oading all profiles because it does not contain data: " + profileId);
                continue;
            }

            // load the game for this profile and put it in the dictionary
            GameData profileData = Load(profileId);

            // defensive programming
            if (profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }else
            {
                Debug.LogError("Tried to loiad profile but something went wrong. ProfileId: " + profileId);
            }


        }


        return profileDictionary;
    }


    public string GetMostRecentlyUpdatedProfileId()
    {

        string mostRecentProfileId = null;
        Dictionary<string, GameData> profileGameData = LoadAllProfiles();
        foreach (KeyValuePair<string, GameData> pair in profileGameData)
        {
            string profileId = pair.Key;
            GameData gameData = pair.Value;


            //skip entry if gamedata is null
            if (gameData == null)
            {
                continue;
            }

            if (mostRecentProfileId == null)
            {
                mostRecentProfileId = profileId;
            }
            // otherwise compare to see which date is the most recent 
            else 
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profileGameData[mostRecentProfileId].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);

                if (newDateTime > mostRecentDateTime)
                {
                    mostRecentProfileId = profileId;
                }
            }

        }

        return mostRecentProfileId;
    }

    // XOR encryption
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char) (data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }

    private bool AttemptRollBack(string fullPath)
    {
        bool success = false;
        string backupFilePath = fullPath + backupExtension;

        try
        {
            if (File.Exists(backupFilePath))
            {
                File.Copy(backupFilePath, fullPath, true);
                success = true;
                Debug.LogWarning("Had to roll back to backup file at: " + backupFilePath);
            }
            else
            {
                throw new Exception("Tried to roll back, but no backup file esisst to roll back to");
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Error occured when trying to roll back to backup file at: " + backupFilePath + "\n" + e);
        }

        return success;
    }
}
