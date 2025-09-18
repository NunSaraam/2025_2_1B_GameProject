using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    public float rotationSpeed = 10.0f;

    [Header("점프 설정")]
    public float jumpHeight = 2.0f;
    public float gravity = -9.81f;                              //중력 속도 추가
    public float landingDuration = 0.3f;                        //착지 후 찾기 모션 지속 시간 (해당 지속 시간 이후에 캐릭터가 움직일 수 있게)

    [Header("공격 설정")]
    public float attackDuration = 0.8f;                         //공격 지속 시간
    public bool canMoveWhileAttacking = false;                  //공격 중 이동 가능 여부 판단 bool

    [Header("컴포넌트")]
    public Animator animator;                                   //컴포넌트 하위에 Animator이 존재하기 때문에

    private CharacterController controller;         
    private Camera playerCamera;

    //현재 상태 값들
    private float currentSpeed;

    private bool isAttacking = false;
    private float attackTimer;

    private bool isLanding = false;                             //착지 중인지 확인
    private float landingTimer;                                 //착지 타이머

    private Vector3 velocity;                               
    private bool isGrounded;                                    //땅이 있는지 판별
    private bool wasGrounded;                                   //직전 프레임에 땅에 있었는지 판단

    private void Start()
    {

        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    private void Update()
    {
        CheckGrounded();
        HandleLanding();
        HandleMovement();
        UpdateAnimator();
        HandleAttack();
        HandleJump();
    }

    void HandleMovement()               //이동 함수 제작
    {
        //공격 중이거나 착지 중일 때 움직임 제한
        if ((isAttacking && !canMoveWhileAttacking) || isLanding)
        {
            currentSpeed = 0;
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)                       //둘 중 하나라도 입력 되었을 때
        {
            //카메라가 보는 방향의 앞쪽으로 설정
            Vector3 cameraForward = playerCamera.transform.forward;
            Vector3 cameraRight = playerCamera.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;

            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * vertical + cameraRight * horizontal;            //이동 방향 설정

            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = runSpeed;
            }
            else
            {
                currentSpeed = walkSpeed;
            }

            controller.Move(moveDirection *  currentSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            currentSpeed = 0;
        }
    }

    void UpdateAnimator()
    {
        //전체 최대속도 (runSpeed) 기준으로 0 ~ 1 계산
        float animatorSpeed = Mathf.Clamp01(currentSpeed / runSpeed);
        animator.SetFloat("Speed", animatorSpeed);
        animator.SetBool("isGrounded", isGrounded);

        bool isFalling = !isGrounded && velocity.y < -0.1f;             //캐릭터가 Y축 속도가 음수러 넘어가면 떨어지고 있다고 판단
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isLanding", isLanding);
    }

    void CheckGrounded()
    {
        wasGrounded = isGrounded;
        isGrounded = controller.isGrounded;                             //캐릭터 컨트롤러에서 상태 값을 받아온다.

        if (!isGrounded && wasGrounded)                                 //지금 프레임은 땅이 아니고, 이전 프레임은 땅일 때
        {
            Debug.Log("떨어지기 시작");
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f;

            if (!wasGrounded && animator != null)                       //착지를 진행
            {
                isLanding = true;
                landingTimer = landingDuration;
            }
        }
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (animator != null)
            {
                animator.SetTrigger("JumpTrigger");
            }
        }
        if (!isGrounded)                                                //땅 위에 있지 않을 경우
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleLanding()
    {
        if (isLanding)
        {
            landingTimer -= Time.deltaTime;                             //랜딩 타이머 시간 만큼 못 움직이게

            if (landingTimer <= 0)
            {
                isLanding = false;
            }
        }
    }

    void HandleAttack()
    {
        if (isAttacking)                                                //공격 중일 때
        {
            attackTimer -= Time.deltaTime;                              //공격 타이머를 감소

            if (attackTimer <= 0)
            {
                isAttacking = false;
            }

        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && !isAttacking)       //공격 중이 아닐 때 키를 누르면 공격
        {
            isAttacking = true;                                     //공격중 표시
            attackTimer = attackDuration;                           //타이머 리필

            if (animator != null)
            {
                animator.SetTrigger("AttackTrigger");
            }
        }
    }
}
