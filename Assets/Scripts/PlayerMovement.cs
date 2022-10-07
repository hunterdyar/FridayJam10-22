using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float mouseMultiplier;
    [SerializeField] private float forceMultiplier;
    [SerializeField] private float moveSpeed;
    public ClickState _clickState;
    private Vector3 _initialMousePos;
    private Vector3 _currentMousePos;
    private Vector3 shootVector;
    //
    private Rigidbody _rigidbody;
    private LineRenderer _lineRenderer;
    private bool canShoot;
    private Camera _camera;
    private Vector3 moveInput;
    private Vector3 _desiredVelocity;
    
    //
    [SerializeField] private Material lineActiveMat;
    [SerializeField] private Material lineCooldownMat;
    
    //
    [SerializeField] private float cooldownTime;
    private float cooldownTimer;
    // Start is called before the first frame update
    void Awake()
    {
        _clickState = ClickState.Released;
        _rigidbody = GetComponent<Rigidbody>();
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        _lineRenderer.positionCount = 2;
        _initialMousePos = transform.position;
        _currentMousePos = transform.position;
        cooldownTimer = Mathf.Infinity;
        canShoot = true;
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer = cooldownTimer + Time.deltaTime;
        canShoot = cooldownTimer >= cooldownTime;
        
        var mousePos = Input.mousePosition;
        _currentMousePos = _camera.ScreenToWorldPoint( new Vector3(mousePos.x, mousePos.y, _camera.nearClipPlane));
        InputTick();

        if (_clickState == ClickState.Dragging)
        {
            shootVector =  _currentMousePos-_initialMousePos;
        }
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        LineRenderTick();
    }

    private void FixedUpdate()
    {
        MovementTick();
    }

    private void MovementTick()
    {
        _desiredVelocity = moveInput * moveSpeed;
        _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, _desiredVelocity, Time.fixedDeltaTime);
    }

    private void LineRenderTick()
    {
        if (_clickState == ClickState.Dragging)
        {
            _lineRenderer.enabled = true;
            _lineRenderer.SetPosition(0,transform.position);
            _lineRenderer.SetPosition(1,transform.position+(shootVector*mouseMultiplier));
        }
        else
        {
            _lineRenderer.enabled = false;
        }

        if (canShoot)
        {
            _lineRenderer.material = lineActiveMat;
        }
        else
        {
            _lineRenderer.material = lineCooldownMat;
        }
    }

    private void InputTick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _clickState = ClickState.Dragging;
            _initialMousePos = _currentMousePos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _clickState = ClickState.Released;
            Shoot();
        }
        
    }

    private void Shoot()
    {
        if (canShoot)
        {
            _rigidbody.AddForce(-shootVector*forceMultiplier,ForceMode.Impulse);
            _clickState = ClickState.Cooldown;
            cooldownTimer = 0;
        }
    }
}
