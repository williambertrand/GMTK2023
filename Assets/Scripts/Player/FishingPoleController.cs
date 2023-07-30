using System;
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

    [Header("Bait selector")]
    public GameObject hamburguer;
    public GameObject phone;
    public GameObject money;
    private int baitPos = 0;

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

    [Header("Line to bait after cast")]
    [SerializeField] private FishingLine _fishingLine;

    [Header("Tutorial")]
    [SerializeField] private Tutorial _tutorial;

    // Start is called before the first frame update
    void Start()
    {
        this.lineRender = this.gameObject.GetComponent<LineRenderer>();
        this.lineRender.enabled = false;

        this.hamburguer.SetActive(true);
        this.phone.SetActive(false);
        this.money.SetActive(false);
    }

    // Update is called once per frame

    private void UpdateCastForce()
    {
        this.force = (Quaternion.AngleAxis(-45, transform.right) * transform.forward) * this.CurrentCharge;
    }
    void Update()
    {
        if (this.fishingPoleState != FihsingPoleState.Release)
        {
            this.CurrentCharge = Mathf.Clamp(this.CurrentCharge + this.chargeRate * Time.deltaTime * (int)this.fishingPoleState, 0f, this.maxCharge);

            if (this.CurrentCharge == this.maxCharge)
            {
                this.fishingPoleState = FihsingPoleState.DecreasingThrowCharge;
            }
            else if (this.CurrentCharge == 0f)
            {
                this.fishingPoleState = FihsingPoleState.IncreasingThrowCharge;
            }

            this.force = (Quaternion.AngleAxis(this.baitInitialAngle, transform.right) * transform.forward) * this.CurrentCharge;

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
        AudioManager.Instance.PlayOneShot(AudioEvent.PLAYER_CAST);
        _fishingLine.CurrentBait = currentBait;
        this.currentBait.transform.position = this.baitInitialPosition.position;
        currentBait.GetComponent<Bait>().onDestroy += ResetBait;
        var rb = this.currentBait.GetComponent<Rigidbody>();
        rb.mass = this.baitMass;
        rb.velocity = this.force;

        this.CurrentCharge = 0f;
        this.fishingPoleState = FihsingPoleState.Release;
        this.lineRender.enabled = false;

        this.force = Vector3.zero;

        _tutorial.OnCastComplete();
    }

    public void ChangeBait(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float v = context.ReadValue<float>();
            this.baitPos += ((int)v);


            if (this.baitPos == -1) this.baitPos = 2;
            else if (this.baitPos == 3) this.baitPos = 0;

            

            switch (this.baitPos)
            {
                case 0:
                    this.hamburguer.SetActive(true);
                    this.phone.SetActive(false);
                    this.money.SetActive(false);
                    break;
                case 1:
                    this.hamburguer.SetActive(false);
                    this.phone.SetActive(true);
                    this.money.SetActive(false);
                    break;
                case 2:
                    this.hamburguer.SetActive(false);
                    this.phone.SetActive(false);
                    this.money.SetActive(true);
                    break;
            }
        }
    }

    public void CastPole(InputAction.CallbackContext context)
    {
        if (context.performed && PlayerController.Instance.canMove)
        {
            PlayerController.Instance.LockPlayer();
            this.fishingPoleState = FihsingPoleState.IncreasingThrowCharge;
        }

        if (context.canceled)
        {
            this.fishingPoleState = FihsingPoleState.Release;
        }
    }

    public void ReelBack(InputAction.CallbackContext context)
    {
        if (!PlayerController.Instance.canMove && context.performed)
        {
            PlayerController.Instance.UnlockPlayer();
            _fishingLine.CurrentBait = null;
            GameObject.Destroy(this.currentBait);
        }
    }

    public void ResetBait()
    {
        PlayerController.Instance.UnlockPlayer();
        _fishingLine.CurrentBait = null;
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
            this.lineRender.SetPosition(i, newPoint);

            // print(i + " "+ newPoint);
        }
    }
}

public enum FihsingPoleState
{
    IncreasingThrowCharge = 1,
    DecreasingThrowCharge = -1,
    Release = 0
}

