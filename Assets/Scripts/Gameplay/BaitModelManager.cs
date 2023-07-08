using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct BaitModelDef
{
    public BaitType type;
    public GameObject model;
}

public class BaitModelManager : MonoBehaviour
{
    [SerializeField] public List<BaitModelDef> models;

    public GameObject GetModelForBaitType(BaitType type)
    {
        return models.Find((BaitModelDef def) => def.type == type).model;
    }
}
