using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private GameObject _moveTutItem;
    [Header("Cast bait")]
    [SerializeField] private GameObject _castTutItem;
    [Header("Catch humans")]
    [SerializeField] private GameObject _catchTutItem;
    [SerializeField] private float _showCatchTutItemTime;
    private float _startedCatchTime;
    private bool _showingCatchItem;
    private bool _completedCast;

    private enum TutStep
    {
        MOVE,
        CAST,
        CATCH,
        DONE
    }

    private TutStep step;

    // Start is called before the first frame update
    void Start()
    {
        _showingCatchItem = false;
        _completedCast = false;
        step = TutStep.MOVE;
    }

    // Update is called once per frame
    void Update()
    {
        if(_showingCatchItem)
        {
            if (Time.time - _startedCatchTime >= _showCatchTutItemTime)
            {
                _catchTutItem.SetActive(false);
                _showingCatchItem = false;
                step = TutStep.DONE;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _moveTutItem.SetActive(false);
            _castTutItem.SetActive(true);
            step = TutStep.CAST;
        }
    }

    public void OnCastComplete()
    {
        if (step != TutStep.CAST) return;

        _completedCast = true;
        _castTutItem.SetActive(false);
        _startedCatchTime = Time.time;
        _showingCatchItem = true;
        _catchTutItem.SetActive(true);
        step = TutStep.CATCH;
    }
}
