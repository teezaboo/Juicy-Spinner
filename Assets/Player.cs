using System;
using UnityEngine;

using TMPro;

using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Pool DefinSoundEffect;
    public Pool PlayerDieSound;
    public float addAngleSpark = 0;
    public Pool PoolSparks;
    public ResourceManager ResourceManager;
    public Pool playerDieEffect;
    public Transform turretHead;
    public Transform TargetDash;

    public float moveSpeed = 5f; // ความเร็วในการเคลื่อนที่
    public float dashForce = 5f;  // ความเร็วในการพุ่ง
    public float addRotationSpeed; // ความเร็วในการเคลื่อนที่
    private float rotationSpeed = 0; // ความเร็วในการเคลื่อนที่
    public Transform bodyRotation; // หัวป้อมปืนที่หมุนได้
    public Transform EyeHead; // หัวป้อมปืนที่หมุนได้
    public float bounceForce = 5f;  // ปรับค่าแรงที่เด้ง
    private Rigidbody2D rb;
    public float addAngleEye = 0f; // ค่าเพิ่มเติมในการหมุน
    public GameObject gameover;
    public AudioSource manageadie;
    public AudioSource manageagameover;
    public int level =1;
    public int exp;
    public bool isMoving = false;
    private Vector3 initialMousePosition; // ตำแหน่งเมาส์เมื่อคลิก
    private float initialZRotation; // มุมหมุนเมื่อคลิก
    private Animator animator;
    public float speedRotationSword = 5f;
    float add;
    bool isref = false;
    private bool isActionTime = false; // ตัวแปรสำหรับตรวจสอบเวลาที่ใช้ในการทำ Action
    private float actionTimer = 0.0f; // ตัวแปรเพื่อนับเวลาที่ผ่านไป
    float oldcurrentMousePosition;
    bool isdown = false;
    bool t888 = false;
    public float powerPerBase = 0;
    public float powerPerAdd = 0;
    public Image powerref;
    public bool isStopMove = false;
    bool isPowerZero = false;
    public float add_powerPerBase;

    public float ScaleSize = 2.8f;
    Coroutine CoroutineStun;
    private void Start()
    {
        rotationSpeed = 0;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(ResourceManager.IsStopGame == true) return;
        Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RotateTurret();
        if(isStopMove == false && ResourceManager.IsStopGame == false){
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f; // ตั้งค่า z ให้เป็น 0 เพื่อทำให้ป้อมปืนหมุนในระนาบ 2D

            // คำนวณทิศทางการหมุน
            Vector3 direction = mousePosition - transform.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            EyeHead.rotation = Quaternion.Euler(new Vector3(0, 0, targetAngle + addAngleEye));
            bodyRotation.transform.eulerAngles = new Vector3(bodyRotation.transform.eulerAngles.x, bodyRotation.transform.eulerAngles.y, bodyRotation.transform.eulerAngles.z + rotationSpeed);
            if (Input.GetMouseButtonDown(0)){
                rotationSpeed = -addRotationSpeed*2;

                isMoving = true;
            }
            
            if (Input.GetMouseButtonDown(1)){
                rotationSpeed = -addRotationSpeed*2;
                    DashToTarget();
            }

            if (isMoving)
            {
                ChasePlayer(currentMousePosition);
            }else{
                if(rotationSpeed < 0){
                    rotationSpeed += 0.2f;
                }else{
                    rotationSpeed = 0;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                // ปล่อยเมาส์: หยุดเคลื่อนที่และหมุน
                isMoving = false;
            }
        }
    }
    private void ChasePlayer(Vector3 targetPosition)
    {
        // คำนวณทิศทางไปยังเป้าหมาย
        Vector3 direction = targetPosition - transform.position;

        float distance = Vector3.Distance(new Vector3(transform.position.x, transform.position.y, 0)
        , new Vector3(targetPosition.x, targetPosition.y, 0));

        if (distance <= 0.1f)
        {
            return;
        }
        // ตั้งค่าแกน Z เป็น 0 เพื่อเคลื่อนที่เฉพาะในแกน X และ Y เท่านั้น
        direction.z = 0;

        // ทำให้ทิศทางมีขนาด 1 เพื่อให้ความเร็วคงที่
        direction.Normalize();

        // ลดความเร็วในกรณีที่เคลื่อนที่แนวทะแยง (ทั้ง x และ y ไม่เท่ากับ 0)
        if (direction.x != 0 && direction.y != 0)
        {
            direction /= Mathf.Sqrt(2); // ลดความเร็วในแนวทะแยง
        }

        // เคลื่อนที่ในแกน X และ Y ด้วยความเร็วคงที่
        transform.position += new Vector3(direction.x * moveSpeed * Time.deltaTime, direction.y * moveSpeed * Time.deltaTime, 0);
    }

   public void DashToTarget()
    {
        // คำนวณทิศทางการแดช (ทำให้ทิศทางมีขนาด 1 ด้วย .normalized)
        Vector2 direction = (TargetDash.position - transform.position).normalized;

        // ลบความเร็วปัจจุบันก่อนที่จะใส่แรงใหม่
        rb.velocity = Vector2.zero;

        // ใส่แรงไปในทิศทางเป้าหมาย (กำหนดแรงคงที่เหมือนกระสุน)
        rb.AddForce(direction * dashForce, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // เมื่อ Player ชนกับ Collider อื่น
        if (collision.gameObject.CompareTag("Obstacle"))  // ตรวจว่าชนกับสิ่งก่อสร้าง
        {
            if(CoroutineStun != null) StopCoroutine(CoroutineStun);
            DefinSoundEffect.GetPool(transform.position);
            CoroutineStun = StartCoroutine(Stun(0.15f));
            // หาทิศทางการเด้ง (ในทิศตรงกันข้ามกับที่ชน)
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            Vector2 bounceDirection = collision.transform.position - transform.position;
            bounceDirection.Normalize();  // ทำให้เป็นเวกเตอร์หน่วย (normalize)
            
            // เพิ่มแรงเด้ง
            rb.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);
            if (collision.contacts.Length > 0)  // ตรวจสอบว่ามีจุดสัมผัสหรือไม่
            {
                Vector3 contactPoint = collision.contacts[0].point;  // ตำแหน่งจุดที่ชนกัน

                // คำนวณมุมที่ทำให้ออบเจกต์หันหน้าเข้าหาออบเจกต์ที่เราต้องการ
                Vector3 directionToTarget = (transform.position - (Vector3)contactPoint).normalized;  // หาทิศทางจากจุดชนไปยัง target
                float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;  // คำนวณมุมที่ต้องหมุน
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);  // หมุนรอบแกน Z ใน 2D

                // สปอนออบเจกต์พร้อมกับการหมุนที่กำหนด
                PoolSparks.GetPool(contactPoint, rotation.eulerAngles + new Vector3(0, 0, addAngleSpark));
            }
        }
        if (collision.gameObject.CompareTag("Trap"))  // ตรวจว่าชนกับสิ่งก่อสร้าง
        {
            enemyAttack();
        }
    }
    public void enemyAttack(){
        isStopMove = true;
        playerDieEffect.GetPool(transform.position);

        PlayerDieSound.GetPool(transform.position);
        Destroy(gameObject);
        ResourceManager.GameOver();
    }
    IEnumerator Stun(float i){
        yield return new WaitForSeconds(i);
    }
    public void AnimateFillAmount(float i)
    {
        powerref.fillAmount += i;
    }




    
    void RotateTurret()
    {
        // รับตำแหน่งเมาส์ในรูปแบบพิกัดโลก
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // ตั้งค่า z ให้เป็น 0 เพื่อทำให้ป้อมปืนหมุนในระนาบ 2D

        // คำนวณทิศทางการหมุน
        Vector3 direction = mousePosition - turretHead.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // คำนวณมุมปัจจุบันของหัวป้อมปืน
        float currentAngle = turretHead.rotation.eulerAngles.z;

        // คำนวณมุมที่ใกล้ที่สุดระหว่างมุมปัจจุบันกับมุมเป้าหมาย
        float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);

        // คำนวณจำนวนที่จะหมุนต่อเฟรม โดยใช้ความเร็วคงที่
        float rotationAmount = 999 * Time.deltaTime;

        // หมุนมุมป้อมปืนในทิศทางที่ถูกต้อง
        if (Mathf.Abs(angleDifference) <= rotationAmount)
        {
            // ถ้ามุมที่เหลือน้อยกว่าหรือเท่ากับ rotationAmount, ให้หมุนตรงไปที่มุมเป้าหมาย
            currentAngle = targetAngle;
        }
        else
        {
            // หมุนด้วยความเร็วที่กำหนด
            currentAngle += Mathf.Sign(angleDifference) * rotationAmount;
        }

        // ตั้งค่าการหมุนของหัวป้อมปืน
        turretHead.rotation = Quaternion.Euler(new Vector3(0, 0, currentAngle));
    }
}