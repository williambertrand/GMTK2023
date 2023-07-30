using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BaitType
{
    HAMBURGER,
    PHONE,
    MONEY
}
public class Bait : MonoBehaviour
{
    public BaitType baitType;

    public delegate void OnDestroy();
    public OnDestroy onDestroy;

    public static BaitType GetRandomBait()
    {
        System.Array values = System.Enum.GetValues(typeof(BaitType));
        BaitType randBait = (BaitType)values.GetValue(Random.Range(0, values.Length));
        return randBait;
    }

    private void OnCollisionEnter(Collision collision)
    {
        onDestroy?.Invoke();
        Destroy(gameObject);
    }
}
