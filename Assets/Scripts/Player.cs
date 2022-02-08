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

    private void Awake()
    {
        characterController = this.gameObject.GetComponent<CharacterController>();
        shadowSystem.Stop();
    }
    private void FixedUpdate()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Move();
        Look();
    }
    private void Move()
    {
        if (characterController.isGrounded)
        {
            moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            moveInput = Vector3.ClampMagnitude(moveInput, 1f);
            if (Input.GetButton("Fire3") && !isDashing)
            {

                StartCoroutine(Dash());
            }
            else
            {
                moveInput = transform.TransformDirection(moveInput * walkSpeed);
            }
            if (Input.GetButtonDown("Jump"))
            {
                playerAnimator.SetTrigger("Jump");
                moveInput.y = Mathf.Sqrt(jumpHeight * -2f * gravityScale);
            }
            if (Input.GetButtonDown("Fire1"))
            {
                playerAnimator.SetTrigger("Ability");
            }
        }
        moveInput.y += gravityScale * Time.deltaTime;
        characterController.Move(moveInput * Time.deltaTime);
        playerAnimator.SetFloat("Speed", moveInput.magnitude);
    }
    private void Look()
    {
        rotateInput.x = Input.GetAxis("Horizontal") * rotationSensivility * Time.deltaTime;
        transform.Rotate(Vector3.up * rotateInput.x);
    }
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
        yield return new WaitForSeconds(0.1f);
        characterController.Move(this.gameObject.transform.forward * dashDistance);
        transform.transform.GetChild(0).gameObject.SetActive(true);
    }
}
