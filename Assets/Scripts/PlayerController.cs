using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    public float rotationSpeed = 10.0f;

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

    private void Start()
    {

        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    private void Update()
    {
        HandleMovement();
        UpdateAnimator();
    }

    void HandleMovement()               //이동 함수 제작
    {
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
    }
}
