using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    private enum HumanState
    {
        NORMAL,
        ON_THE_LINE,
        OFF_THE_LINE,
        CAUGHT
    }

    public float speed;
    private Vector3 _targetPos;

    private HumanState currentState;
    public BaitType preferredBait;

    private const float TRAVEL_DIST = 80.0f;

    private int _currentCaptureCount;
    [SerializeField] private int _requiredCaptureCount;
    [SerializeField] private float _speedOnHook;

    // Start is called before the first frame update
    void Start()
    {
        currentState = HumanState.NORMAL;

        _targetPos = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.z - TRAVEL_DIST
        );
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case HumanState.NORMAL:
                HandleNormalMovement();
                break;
            case HumanState.ON_THE_LINE:
                HandleOnTheLine();
                break;
            case HumanState.OFF_THE_LINE:
                // TODO: run off screen
                break;
            case HumanState.CAUGHT:
                break;
        }
    }

    private void HandleNormalMovement()
    {
        // Move our position a step closer to the target.
        var step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, step);

        if (Vector3.Distance(transform.position, _targetPos) < 0.01f)
        {
            
            Destroy(gameObject);
        }
    }

    public void OnReel()
    {
        _currentCaptureCount += 1;
    }
    private void HandleOnTheLine()
    {

        float step = _speedOnHook * Time.deltaTime;

        // Get current "perent" the player has reeled in the Human
        float percentCaptured = _currentCaptureCount / _requiredCaptureCount;
        Vector3 hookPos = (transform.position - PlayerController.Instance.transform.position).normalized * percentCaptured;

        transform.position = Vector3.MoveTowards(hookPos, _targetPos, step);
    }


    private void OnHooked()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bait"))
        {
            Bait b = other.GetComponent<Bait>();
            if(b.baitType == preferredBait)
            {
                OnHooked();
            }
            else
            {
                // TODO: Show an alert on wrong bait type
            }
        }
    }
}
