using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void OnHumanCaught(Human h);

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
    public int RequiredCaptureCount;
    [SerializeField] private float _speedOnHook;

    public int scoreValue = 1;
    private Vector3 _onHookedPos;
    public OnHumanCaught onCaught;

    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        currentState = HumanState.NORMAL;

        _targetPos = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.z - TRAVEL_DIST
        );

        _anim = GetComponentInChildren<Animator>();
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

        if (_currentCaptureCount >= RequiredCaptureCount)
        {
            OnCaught();
        }
    }
    private void HandleOnTheLine()
    {
        // Get current "perent" the player has reeled in the Human
        float percentRemaining = 1 - ((float)_currentCaptureCount / RequiredCaptureCount);

        float originalDistance = Vector3.Distance(_onHookedPos, PlayerController.Instance.transform.position);
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);

        float step;
        if(distanceToPlayer > percentRemaining * originalDistance)
        {
            step = _speedOnHook * Time.deltaTime;
        } else
        {
            step = 0;
        }

        transform.position = Vector3.MoveTowards(transform.position, PlayerController.Instance.transform.position, step);
    }


    private void OnHooked()
    {
        _onHookedPos = transform.position;
        currentState = HumanState.ON_THE_LINE;
        _anim.SetTrigger("seeBait");
        SceneManager.LoadScene("Fishing", LoadSceneMode.Additive);
        // TODO: Handle coming back w/ result
    }

    private void OnCaught()
    {
        currentState = HumanState.CAUGHT;
        onCaught?.Invoke(this);
        PlayerController.Instance.OnHumanCaught(this);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isBait = true; //other.CompareTag("Bait")
        if (isBait)
        {
            Bait b = other.GetComponent<Bait>();
            if(b.baitType == preferredBait)
            {
                OnHooked();
                PlayerController.Instance.OnHumanHooked(this);
                Destroy(other.gameObject);
            }
            else
            {
                // TODO: Show an alert on wrong bait type
            }
        }
    }
}
