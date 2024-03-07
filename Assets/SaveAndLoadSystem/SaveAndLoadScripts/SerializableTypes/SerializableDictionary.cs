using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, Tvalue> : Dictionary<TKey, Tvalue>, ISerializationCallbackReceiver
{
    
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<Tvalue> values = new List<Tvalue>();


    //save the dictionary to Lists

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach(KeyValuePair<TKey, Tvalue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // Load the dictionary from Lists
    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
        {
            Debug.LogError("Tried to deserialize a SerializableDictionary, but the amount of keys (" + keys.Count + ") does not math the number of values (" + values.Count + ") which indicates that something went wrong");
            
        }

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}
