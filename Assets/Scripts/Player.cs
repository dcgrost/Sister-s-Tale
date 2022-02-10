using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Animator playerAnimator;
    [Header("General")]
    public float gravityScale = -20f;
    [Header("Movement")]
    public FixedJoystick joystick;
    public float walkSpeed = 5f;
    public float dashDistance = 5f;
    public float dashCD = 5f;
    [Header("Rotation")]
    [Range(100f, 400f)] public float rotationSensivility = 300;
    [Header("Jump")]
    public float jumpHeight = 1.9f;
    [Header("Effects")]
    public ParticleSystem shadowSystem;

    Vector3 moveInput = Vector3.zero;
    Vector3 rotateInput = Vector3.zero;
    CharacterController characterController;
    bool isDashing = false;
    float gravityScaleTem = 0f;
    bool jumpButton = false;
    bool canMove = true;

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
    }
    #region Move&Look
    private void Move()
    {
        if (characterController.isGrounded)
        {
            moveInput = new Vector3(joystick.Direction.x, 0f, joystick.Direction.y);
            moveInput = Vector3.ClampMagnitude(moveInput, 1f);
            moveInput = transform.TransformDirection(moveInput * walkSpeed);
            if (jumpButton)
            {
                Jump();
            }
        }
        moveInput.y += gravityScale * Time.deltaTime;
        characterController.Move(moveInput * Time.deltaTime);
        playerAnimator.SetFloat("Speed", moveInput.magnitude);
    }
    private void Look()
    {
        rotateInput.x = joystick.Direction.x * rotationSensivility * Time.deltaTime;
        transform.Rotate(Vector3.up * rotateInput.x);
    }
    #endregion
    #region Dash
    IEnumerator Dash()
    {
        StartCoroutine(InDash());
        isDashing = true;
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
    public void Ability()
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
        yield return new WaitForSeconds(1f);
        Debug.Log(canMove);
        canMove = true;
    }
    #endregion
}
