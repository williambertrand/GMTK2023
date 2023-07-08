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

    public Vector3 lookAtDuringCast;

    [Header("Pole angle direction (not used, future feature)")]
    public float maxAngle = 45f;
    public float angleSpeed = 1f;
    // 0 = center, -maxAngle = max left, maxAngle = maxRight
    public float currentAngle { get; private set; }

    [Header("Bait physics")]
    public float baitMass = 0.05f;
    public float baitInitialAngle = -45f;
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

    private void UpdateCastForce()
    {
        this.force = (Quaternion.AngleAxis(-45, transform.right) * transform.forward) * this.CurrentCharge;
    }
    void Update()
    {
        if(this.fishingPoleState != FihsingPoleState.Release){
            this.CurrentCharge = Mathf.Clamp(this.CurrentCharge + this.chargeRate * Time.deltaTime * (int) this.fishingPoleState, 0f, this.maxCharge);

            if (this.CurrentCharge == this.maxCharge)
            {
                this.fishingPoleState = FihsingPoleState.DecreasingThrowCharge;
            }
            else if(this.CurrentCharge == 0f){
                this.fishingPoleState = FihsingPoleState.IncreasingThrowCharge;
            }

            this.force = (Quaternion.AngleAxis(this.baitInitialAngle,transform.right) * transform.forward) * this.CurrentCharge;
            
            this.lineRender.enabled = true;
        }


        if (this.fishingPoleState == FihsingPoleState.Release && this.CurrentCharge > 0f)
        {
            this.CastBait();
        }
    }

    void LateUpdate()
    {
        this.DrawPath();
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

        this.force = Vector3.zero;
    }


    public void CastPole(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PlayerController.Instance.LockPlayer();
            this.fishingPoleState = FihsingPoleState.IncreasingThrowCharge;
        }

        if (context.canceled)
        {
            PlayerController.Instance.UnlockPlayer();
            this.fishingPoleState = FihsingPoleState.Release;
        }
    }

    private void DrawPath()
    {
        this.lineRender.positionCount = Mathf.CeilToInt(this.numberOfPoints / this.timeBetweenPoints) + 2;

        Vector3 velocity = this.force / this.baitMass;

        int i = 0;
        this.lineRender.SetPosition(i, this.baitInitialPosition.position);
        for (float time = 0; time < this.numberOfPoints; time += this.timeBetweenPoints)
        {
            i++;
            Vector3 newPoint = this.baitInitialPosition.transform.position + time * velocity;
            newPoint.y = this.baitInitialPosition.transform.position.y + velocity.y * time + (Physics.gravity.y / 2f * time * time);
            print(i);
            this.lineRender.SetPosition(i, newPoint);
        }
    }
}

public enum FihsingPoleState
{
    IncreasingThrowCharge = 1,
    DecreasingThrowCharge = -1,
    Release = 0
}

