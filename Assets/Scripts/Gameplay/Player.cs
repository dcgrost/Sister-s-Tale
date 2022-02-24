using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Animator playerAnimator;
    public GameObject target = null;
    [Header("General")]
    public float gravityScale = -20f;
    [Header("Movement")]
    public FixedJoystick joystick;
    public float walkSpeed = 5f;
    public float dashDistance = 5f;
    public float dashCD = 5f;
    [Header("Jump")]
    public float jumpHeight = 1.9f;
    [Header("Effects")]
    public ParticleSystem shadowSystem;
    public Image cooldownDash;

    Vector3 moveInput = Vector3.zero;
    CharacterController characterController;
    bool isDashing = false;
    public bool isMoving  = false;
    float gravityScaleTem = 0f;
    bool jumpButton = false;
    bool canMove = true;
    bool dashIsCoolingdown = false;
    GameObject[] targets = null;
    float minDistance = 10f;
    int tempi;
    float timeAnim;
    Vector3 moveWithCamera;
    Vector3 cameraForward;
    Vector3 cameraRight;

    private void Awake()
    {
        characterController = this.gameObject.GetComponent<CharacterController>();
    }
    private void FixedUpdate()
    {
        CameraDirection();
        if (canMove)
        {
            Move();
        }
        if (dashIsCoolingdown)
        {
            cooldownDash.fillAmount -= 1f / (dashCD + 0.1f) * Time.deltaTime;
        }
        playerAnimator.SetBool("IsGrounded", characterController.isGrounded);
        playerAnimator.SetBool("IsMoving", isMoving);
    }
    #region Move&Look
    private void Move()
    {
        if (characterController.isGrounded)
        {
            moveInput = new Vector3(joystick.Direction.x, 0f, joystick.Direction.y);
            moveInput = Vector3.ClampMagnitude(moveInput, 1f);
            moveWithCamera = moveInput.x * cameraRight + moveInput.z * cameraForward;
            characterController.transform.LookAt(characterController.transform.position + moveWithCamera);
            if (jumpButton)
            {
                Jump();
            }
        }
        moveWithCamera.y += gravityScale * Time.deltaTime;
        characterController.Move(moveWithCamera * walkSpeed * Time.deltaTime);
        playerAnimator.SetFloat("Direction", joystick.Direction.y);
        if (moveInput.magnitude > 0.1f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }
    private void CameraDirection()
    {
        cameraForward = playerCamera.transform.forward;
        cameraRight = playerCamera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;
    }
    #endregion
    #region Dash
    IEnumerator Dash()
    {
        StartCoroutine(InDash());
        isDashing = true;
        cooldownDash.fillAmount = 1f;
        dashIsCoolingdown = true;
        yield return new WaitForSeconds(dashCD + 0.1f);
        isDashing = false;
    }
    IEnumerator InDash()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        shadowSystem.Play();
        gravityScaleTem = gravityScale;
        gravityScale = 0f;
        yield return new WaitForSeconds(0.1f);
        characterController.Move(cameraForward * dashDistance);
        transform.GetChild(0).gameObject.SetActive(true);
        gravityScale = gravityScaleTem;
    }
    public void DashButton()
    {
        if (!isDashing)
        {
            StartCoroutine(Dash());
        }
    }
    #endregion
    #region Jump
    private void Jump()
    {
        playerAnimator.SetTrigger("Jump");
        moveWithCamera.y = Mathf.Sqrt(jumpHeight * -2f * gravityScale);
    }
    public void JumpButton()
    {
        StartCoroutine(WaitJumpButton());
    }
    IEnumerator WaitJumpButton()
    {
        jumpButton = true;
        yield return new WaitForSeconds(0.05f);
        jumpButton = false;
    }
    #endregion
    #region Ability
    public void TargetSelector()
    {
        target = null;
        targets = GameObject.FindGameObjectsWithTag("Interactable");
        float previousDistance = minDistance;
        for (int i = 0; i < targets.Length; i++)
        {
            float distance = (targets[i].transform.position - this.transform.position).magnitude;
            if (distance < minDistance)
            {
                if (distance <= previousDistance && targets[i].GetComponent<Interactable>().isTargetable)
                {
                    previousDistance = distance;
                    tempi = i;
                }
            }
        }
        if((targets[tempi].transform.position-this.transform.position).magnitude < minDistance)
        {
            target = targets[tempi];
        }
        else
        {
            target = null;
        }
        if (!target.GetComponent<Interactable>().isTargetable)
        {
            target = null;
        }
        if (target != null)
        {
            LaunchAbility();
        }
    }
    private void LaunchAbility()
    {
        if (characterController.isGrounded)
        {
            StartCoroutine(WaitAbility());
        }
    }
    IEnumerator WaitAbility()
    {
        transform.LookAt(target.transform);
        if (target.GetComponent<Interactable>().interactableType == InteractableType.Push)
        {
            timeAnim = 0.5f;
        }
        else
        {
            timeAnim = 0.75f;
        }
        canMove = false;
        playerAnimator.SetTrigger("Ability");
        yield return new WaitForSeconds(timeAnim);
        Ability();
        canMove = true;
    }
    private void Ability()
    {
        target.GetComponent<Interactable>().Action(this.gameObject.transform.position);
    }
    #endregion
}
