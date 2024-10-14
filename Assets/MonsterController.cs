using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Transform EyeHead; // หัวป้อมปืนที่หมุนได้
    public Transform EyeHead2; // หัวป้อมปืนที่หมุนได้
    public float addAngleEye = 0f; // ค่าเพิ่มเติมในการหมุน
    public float addAngleEye2 = 0f; // ค่าเพิ่มเติมในการหมุน
    public float fleeDistance = 10f;
    public Player playerScr;
    public float MaxHp = 100;
    public float Hp = 100;
    public Transform player;
    public float moveSpeed = 3f;
    public float maxMoveSpeed = 3f;
    private float MaxHpStock;
    private float AttackDamageStock;
    public float AttackDamage = 30f;
    public float AttackForce = 1f;
    private Camera mainCamera;
    [SerializeField] private GameManagers _gameManagers;
    [SerializeField] private MonsterSpawner _monsterSpawner;
    public List<Shader> shaders;
    public List<SpriteRenderer> sprites;
    public float attackDelay = 1f;
    private float nextAttackTime;
    private bool isAttackPlayer = false;
    private int minSec = 0;
    [SerializeField] private Pool _poolDie;
    [SerializeField] private LayerMask _obstacleLayerMask;
    [SerializeField] private float _obstacleCheckCircleRadius;
    [SerializeField] private float _obstacleCheckDistance;
    private Rigidbody2D _rigidbody;
    private RaycastHit2D[] _obstacleCollisions;
    private float _obstacleAvoidanceCooldown;
    private Vector2 _targetDirection;
    private const float safeDistance = 5f;

    private void Awake()
    {
        MaxHpStock = MaxHp;
        AttackDamageStock = AttackDamage;
        mainCamera = Camera.main;
        _rigidbody = GetComponent<Rigidbody2D>();
        _obstacleCollisions = new RaycastHit2D[10];
    }

    void Update()
    {
        if (player == null) return;

        if (Vector3.Distance(transform.position, player.position) > safeDistance)
        {
            _rigidbody.velocity = Vector2.zero;
            return;
        }

        FleeAndAvoidObstacles();
        MoveMonster();

        Vector3 direction = transform.position - player.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        EyeHead.rotation = Quaternion.Euler(new Vector3(0, 0, targetAngle + addAngleEye));
        EyeHead2.rotation = Quaternion.Euler(new Vector3(0, 0, targetAngle + addAngleEye2));

        if (isAttackPlayer)
        {
            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + attackDelay;
                playerScr.enemyAttack();
            }
        }
    }

    private void OnEnable()
    {
        Hp = MaxHp;
        moveSpeed = maxMoveSpeed;
        if (shaders.Count > 0)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].material.shader = shaders[i];
                sprites[i].color = Color.white;
            }
        }
    }

    private void FleeAndAvoidObstacles()
    {
        if (Vector3.Distance(transform.position, player.position) > fleeDistance)
        {
            _rigidbody.velocity = Vector2.zero;
            return;
        }

        Vector3 direction = (transform.position - player.position).normalized;
        _targetDirection = direction;

        var contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(_obstacleLayerMask);
        int numberOfCollisions = Physics2D.CircleCast(
            transform.position,
            _obstacleCheckCircleRadius,
            _targetDirection,
            contactFilter,
            _obstacleCollisions,
            _obstacleCheckDistance);

        for (int index = 0; index < numberOfCollisions; index++)
        {
            var obstacleCollision = _obstacleCollisions[index];
            if (obstacleCollision.collider.gameObject == gameObject)
            {
                continue;
            }
            if (_obstacleAvoidanceCooldown <= 0)
            {
                _obstacleAvoidanceCooldown = 1f;
            }
            _targetDirection += (Vector2)obstacleCollision.normal;
            _targetDirection.Normalize();
            break;
        }
        _obstacleAvoidanceCooldown -= Time.deltaTime;

        if (transform.position.x < player.position.x && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            EyeHead.transform.localScale = new Vector3(-EyeHead.transform.localScale.x, EyeHead.transform.localScale.y, EyeHead.transform.localScale.z);
            EyeHead2.transform.localScale = new Vector3(-EyeHead2.transform.localScale.x, EyeHead2.transform.localScale.y, EyeHead2.transform.localScale.z);
        }
        else if (transform.position.x > player.position.x && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            EyeHead.transform.localScale = new Vector3(-EyeHead.transform.localScale.x, EyeHead.transform.localScale.y, EyeHead.transform.localScale.z);
            EyeHead2.transform.localScale = new Vector3(-EyeHead2.transform.localScale.x, EyeHead2.transform.localScale.y, EyeHead2.transform.localScale.z);
        }
    }

    private void MoveMonster()
    {
        _rigidbody.velocity = _targetDirection * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playerScr != null)
        {
            if (_gameManagers.IsStopGame == true) return;
            if (other.gameObject.CompareTag("Player"))
            {
                playerScr.enemyAttack();
            }
        }
    }

    IEnumerator Attacked(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void TakeDamage()
    {
        _poolDie.GetPool(transform.position);
    //    _monsterSpawner.DeMonster();
        _gameManagers.DieCount++;
        gameObject.SetActive(false);
    }

    public void KnockBack(Vector3 knockbackPosition, float knockbackPower)
    {
        transform.GetComponent<Rigidbody2D>().AddForce(((Vector3)knockbackPosition - transform.position).normalized * knockbackPower * -1, ForceMode2D.Impulse);
        if (gameObject.activeSelf == false) return;
        StartCoroutine(Attacked(0.2f));
    }
}