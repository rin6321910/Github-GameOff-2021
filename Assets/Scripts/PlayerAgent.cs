using System;
using System.Linq;
using System.Collections;
using UnityEngine;

public class PlayerAgent : Agent
{
    [Header("Input")] public float autoClickInterval = 0.1f;
    private float _autoClickTimer;

    private Camera _camera;

    bool defaultControlsEnabled = true;

    [SerializeField] private SpriteRenderer AOECircle;

    private bool turnOnHighlight = false;

    private IEnumerator currentCoroutine = null;

    protected override void Awake()
    {
        base.Awake();
        _camera = Camera.main;
    }

    private void Update()
    {
        if (defaultControlsEnabled)
        {
            PlayerInput();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            StopAbilityInput();
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Change cursor back to default
            defaultControlsEnabled = true;
        }

        AOECircleFollowCursor();
        if (turnOnHighlight) HighlightUnitUnderMouseCursor();
    }

    private void PlayerInput()
    {
        // Right mouse button
        if (Input.GetMouseButtonDown(1))
        {
            unitEventHandler.RaiseEvent("OnMoveOrderIssued", CursorWorldPosition());
        }
        else if (Input.GetMouseButton(1))
        {
            // Auto-repeat click when held
            _autoClickTimer += Time.deltaTime;

            if (_autoClickTimer >= autoClickInterval)
            {
                _autoClickTimer = float.Epsilon;
                unitEventHandler.RaiseEvent("OnMoveOrderIssued", CursorWorldPosition());
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            _autoClickTimer = float.Epsilon;
        }

        // 'S' key
        if (Input.GetKeyDown(KeyCode.S))
        {
            unitEventHandler.RaiseEvent("OnStopOrderIssued", null);
        }
    }

    private Vector3 CursorWorldPosition()
    {
        Vector2 screenPosition = Input.mousePosition;
        Vector3 worldPosition = _camera.ScreenToWorldPoint(screenPosition);

        return new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
    }

    // Responsible for aiming abilities, coupled with AbilityInputType.cs
    public IEnumerator ProcessTargetInput(Ability ability)
    {
        defaultControlsEnabled = false;
        StopAbilityInput();

        if (ability.InputType == AbilityType.TargetPoint)
        {
            currentCoroutine = AbilityInputType.PointTargetInput(ability);
            yield return StartCoroutine(currentCoroutine);
        }
        else if (ability.InputType == AbilityType.TargetUnit)
        {
            turnOnHighlight = true;
            currentCoroutine = AbilityInputType.UnitTargetInput(ability);
            yield return StartCoroutine(currentCoroutine);
            turnOnHighlight = false;
        }
        else if (ability.InputType == AbilityType.TargetArea)
        {
            float radius = ability.AbilityStats["AOE Radius"];
            AOECircle.transform.localScale = new Vector3(radius * 1.7f, radius * 1.7f, 1.0f);
            AOECircle.enabled = true;
            currentCoroutine = AbilityInputType.AOETargetInput(ability);
            yield return StartCoroutine(currentCoroutine);
            AOECircle.enabled = false;
        }
        // else if (ability.InputType == AbilityType.NoTarget) yield return StartCoroutine(AbilityInputType.PointTargetInput(ability));

        defaultControlsEnabled = true;
    }

    public void StopAbilityInput()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            turnOnHighlight = false;
            AOECircle.enabled = false;
        }
    }

    private void AOECircleFollowCursor()
    {
        AOECircle.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
            Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
            0.0f
        );
    }

    private void HighlightUnitUnderMouseCursor()
    {
        string[] tags = { "Enemy" };
        LayerMask enemyMask = LayerMask.GetMask("Enemy");

        // Get target and check that it's valid
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
            direction: Vector2.zero, distance: Mathf.Infinity, layerMask: enemyMask);
        if (hit.collider != null)
        {
            Transform selection = hit.transform;
            if (tags.Contains(selection.tag)) // Check if its the target we want.
            {
                Unit selectedUnit = hit.collider.GetComponent<Unit>();
                Debug.Log(selection.gameObject.name);

                selectedUnit.GetComponentInChildren<UnitVFXManager>().HighlightOutline();
            }
        }
    }
}
