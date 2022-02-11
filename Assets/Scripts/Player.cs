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
    public float walkBackSpeed = 3f;
    public float dashDistance = 5f;
    public float dashCD = 5f;
    [Header("Rotation")]
    [Range(100f, 400f)] public float rotationSensivility = 300;
    [Header("Jump")]
    public float jumpHeight = 1.9f;
    [Header("Effects")]
    public ParticleSystem shadowSystem;
    public Image cooldownDash;

    Vector3 moveInput = Vector3.zero;
    Vector3 rotateInput = Vector3.zero;
    CharacterController characterController;
    bool isDashing = false;
    bool isMoving  = false;
    float gravityScaleTem = 0f;
    bool jumpButton = false;
    bool canMove = true;
    bool dashIsCoilingdown = false;
    GameObject[] targets = null;
    float minDistance = 10f;
    int tempi;

    private void Awake()
    {
        characterController = this.gameObject.GetComponent<CharacterController>();
    }
    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
            Look();
        }
        if (dashIsCoilingdown)
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
            if (joystick.Direction.y > -0.25f)
            {
                moveInput = transform.TransformDirection(moveInput * walkSpeed);
            }
            else
            {
                moveInput = transform.TransformDirection(moveInput * walkBackSpeed);
            }
            if (jumpButton)
            {
                Jump();
            }
        }
        moveInput.y += gravityScale * Time.deltaTime;
        characterController.Move(moveInput * Time.deltaTime);
        playerAnimator.SetFloat("Direction", joystick.Direction.y);
        if (moveInput.magnitude > 0.4f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }
    private void Look()
    {
        rotateInput.x = joystick.Direction.x * rotationSensivility * Time.deltaTime;
        if (joystick.Direction.y > -0.25f)
        {
            transform.Rotate(Vector3.up * rotateInput.x);
        }
        else
        {
            transform.Rotate(Vector3.up * -rotateInput.x);
        }
    }
    #endregion
    #region Dash
    IEnumerator Dash()
    {
        StartCoroutine(InDash());
        isDashing = true;
        cooldownDash.fillAmount = 1f;
        dashIsCoilingdown = true;
        yield return new WaitForSeconds(dashCD + 0.1f);
        isDashing = false;
    }
    IEnumerator InDash()
    {
        transform.transform.GetChild(0).gameObject.SetActive(false);
        shadowSystem.Play();
        gravityScaleTem = gravityScale;
        gravityScale = 0f;
        yield return new WaitForSeconds(0.1f);
        characterController.Move(this.gameObject.transform.forward * dashDistance);
        transform.transform.GetChild(0).gameObject.SetActive(true);
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
        moveInput.y = Mathf.Sqrt(jumpHeight * -2f * gravityScale);
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
        canMove = false;
        playerAnimator.SetTrigger("Ability");
        Ability();
        yield return new WaitForSeconds(1f);
        canMove = true;
    }
    private void Ability()
    {
        target.GetComponent<Interactable>().Action(this.gameObject.transform.position);
    }
    #endregion
}
