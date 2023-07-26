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

    public BubbleThought bubbleThought;
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
    public HumanSpawner Spawner;

    private Animator _anim;

    public BoxCollider movementBounds;

    private bool _isFacingLeft;

    // Start is called before the first frame update
    void Start()
    {
        currentState = HumanState.NORMAL;
        _anim = GetComponentInChildren<Animator>();
        ChooseNewTargetPos();

        this.bubbleThought.type = this.preferredBait;
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

        Vector3 dir = transform.position - _targetPos;

        if (dir.x < 0 && !_isFacingLeft)
        {
            Flip();
        }
        else if (dir.x > 0 && _isFacingLeft)
        {
            Flip();
        }

        if (dir.x != 0)
        {
            _anim.SetInteger("dir", 1);
        }
        else if (dir.z > 0)
        {
            _anim.SetInteger("dir", 2);
        }
        else if (dir.z < 0)
        {
            _anim.SetInteger("dir", 3);
        }

        if (Vector3.Distance(transform.position, _targetPos) < 0.01f)
        {
            ChooseNewTargetPos();
            //Destroy(gameObject);
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
        if (distanceToPlayer > percentRemaining * originalDistance)
        {
            step = _speedOnHook * Time.deltaTime;
        }
        else
        {
            step = 0;
        }

        transform.position = Vector3.MoveTowards(transform.position, PlayerController.Instance.transform.position, step);
    }


    private void OnHooked()
    {
        _onHookedPos = transform.position;
        currentState = HumanState.ON_THE_LINE;
        AudioManager.Instance.StopAll();
        AudioManager.Instance.PlayOneShot(AudioEvent.HUMAN_HOOKED);
        _anim.SetTrigger("seeBait");
        Spawner.OnHumanHooked(this);
        // TODO: Handle going to mini game scene and coming back w/ result
        var op = SceneManager.LoadSceneAsync("RhythmMinigame", LoadSceneMode.Additive);
        StartCoroutine(LoadAsyncScene(op));
    }

    IEnumerator LoadAsyncScene(AsyncOperation asyncLoad)
    {

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        GamePlayManager gamePlayManager =  GameObject.FindGameObjectWithTag("GameManager").GetComponent<GamePlayManager>();
        //TODO: Add human type, difficult 
        MinigameSceneController minigameSceneController = SceneManager.GetSceneByName("RhythmMinigame").GetRootGameObjects()[0].GetComponent<MinigameSceneController>();
        minigameSceneController.Init(HumanType.GoofyOrange, SongLevel.Normal, preferredBait);

        //TODO: cache this
       gamePlayManager.GoToMinigame();

        // TODO: Handle coming back and updating the state, but for now just destroy
        Destroy(gameObject);
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
            if (b.baitType == preferredBait)
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

    private void ChooseNewTargetPos()
    {
        int axis = Random.Range(0, 2);
        _targetPos = RandomPointInBounds(axis);

    }

    private Vector3 RandomPointInBounds(int axis)
    {
        float x;
        float z;
        if (axis == 0)
        {
            x = Random.Range(movementBounds.bounds.min.x, movementBounds.bounds.max.x);
            z = transform.position.z;
        }
        else
        {
            x = transform.position.x;
            z = Random.Range(movementBounds.bounds.min.z, movementBounds.bounds.max.z);
        }

        return new Vector3(x, 2.5f, z);
    }

    private void Flip()
    {
        _isFacingLeft = !_isFacingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
