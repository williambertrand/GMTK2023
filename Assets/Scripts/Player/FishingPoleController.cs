using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FishingPoleController : MonoBehaviour
{
    public GameObject baitPrefab;
    public Transform baitInitialPosition;
    private GameObject currentBait;
    [Header("Pole strength charging")]
    public float chargeRate = 10f;
    public float maxCharge = 10f;
    public float CurrentCharge { get; private set; }

    [Header("Pole angle direction")]
    public float maxAngle = 45f;
    public float angleSpeed = 1f;
    // 0 = center, -maxAngle = max left, maxAngle = maxRight
    public float currentAngle { get; private set; }

    [Header("Bait physics")]
    public float baitMass = 0.05f;
    private Vector3 force;

    [Header("Path projection")]
    [Range(10, 100)]
    public int numberOfPoints = 50;
    private LineRenderer lineRender;
    [Range(0.01f, 0.5f)]
    public float timeBetweenPoints = 0.25f;

    public FihsingPoleState fishingPoleState;

    // Start is called before the first frame update
    void Start()
    {
        this.lineRender = this.gameObject.GetComponent<LineRenderer>();
        this.lineRender.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (this.fishingPoleState == FihsingPoleState.IncreasingThrowCharge)
        {
            this.CurrentCharge += this.chargeRate * Time.deltaTime;

            if (this.CurrentCharge >= this.maxCharge)
            {
                this.CurrentCharge = this.maxCharge;
                this.fishingPoleState = FihsingPoleState.DecreasingThrowCharg;
            }

            this.DrawPath();
        }

        else if (this.fishingPoleState == FihsingPoleState.DecreasingThrowCharg)
        {
            this.CurrentCharge -= this.chargeRate * Time.deltaTime;

            if (this.CurrentCharge <= 0f)
            {
                this.CurrentCharge = 0f;
                this.fishingPoleState = FihsingPoleState.IncreasingThrowCharge;
            }

            this.DrawPath();
        }


        if (this.fishingPoleState == FihsingPoleState.Release && this.CurrentCharge > 0f)
        {
            this.CastBait();
        }
    }

    private void CastBait()
    {
        this.currentBait = GameObject.Instantiate(this.baitPrefab);
        this.currentBait.transform.position = this.baitInitialPosition.position;

        var rb = this.currentBait.GetComponent<Rigidbody>();
        rb.mass = this.baitMass;
        rb.velocity = this.force;

        this.CurrentCharge = 0f;
        this.fishingPoleState = FihsingPoleState.Release;
        this.lineRender.enabled = false;
    }


    public void CastPole(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            this.fishingPoleState = FihsingPoleState.IncreasingThrowCharge;
        }

        if (context.canceled)
        {
            this.fishingPoleState = FihsingPoleState.Release;
        }
    }

    private void DrawPath()
    {
        this.lineRender.enabled = true;
        this.lineRender.positionCount = Mathf.CeilToInt(this.numberOfPoints / this.timeBetweenPoints) + 1;
        
        //fuck quaternions, just add two vector to get the 45 angle and normalized it
        this.force = (Quaternion.AngleAxis(-45,transform.right) * transform.forward) * this.CurrentCharge;
        Vector3 velocity = this.force / this.baitMass;
        
        int i = 0;
        this.lineRender.SetPosition(i, this.baitInitialPosition.position);
        for (float time = 0; time < this.numberOfPoints; time += this.timeBetweenPoints)
        {
            i++;
            Vector3 newPoint = this.baitInitialPosition.transform.position + time * velocity;
            newPoint.y = this.baitInitialPosition.transform.position.y + velocity.y * time + (Physics.gravity.y / 2f * time * time);

            this.lineRender.SetPosition(i, newPoint);
        }
    }
}

public enum FihsingPoleState
{
    None = 0,
    IncreasingThrowCharge, 
    DecreasingThrowCharg,
    Release
}

