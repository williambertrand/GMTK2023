using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum BaitType
{
    HAMBURGER,
    PHONE,
    MONEY
}

public class Bait : MonoBehaviour
{

    public bool humanFollowing = false;
    public bool onFloor = false;
    public float distance = 10f;

    public BaitType baitType;

    public delegate void OnDestroy();
    public OnDestroy onDestroy;

    public static BaitType GetRandomBait()
    {
        System.Array values = System.Enum.GetValues(typeof(BaitType));
        BaitType randBait = (BaitType)values.GetValue(Random.Range(0, values.Length));
        return randBait;
    }

    void LateUpdate()
    {
        if (this.onFloor && !this.humanFollowing)
        {
            var near = GameObject.FindGameObjectsWithTag("Human")
            .Where(x => x.GetComponent<Human>().preferredBait == this.baitType && Vector3.Distance(x.transform.position, this.transform.position) <= this.distance)
            .OrderBy(x => Vector3.Distance(x.transform.position, this.transform.position))
            .FirstOrDefault();

            if(near){
                this.humanFollowing = true;
                near.GetComponent<Human>().TriggerFollowBait();
            }
        }
        print(this.humanFollowing);
    }

    private void OnCollisionEnter(Collision collision)
    {
        this.onFloor = true;
        // onDestroy?.Invoke();
        // Destroy(gameObject);
    }
}
