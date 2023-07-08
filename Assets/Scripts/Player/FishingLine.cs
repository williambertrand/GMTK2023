using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLine : MonoBehaviour
{

    private LineRenderer _lineRenderer;
    public GameObject CurrentBait;

    [SerializeField] private Transform _baitInitialPos;

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentBait != null)
        {
            _lineRenderer.enabled = true;
            _lineRenderer.SetPosition(0, _baitInitialPos.position);
            _lineRenderer.SetPosition(1, CurrentBait.transform.position);
        } else
        {
            _lineRenderer.enabled = false;
        }
    }
}
