using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public RandomPoolAudio DieSoundEffect;
    public Pool_TextDamage Pool_TextDamage;
    public bool isJoker = false;
    public float bombDistance = 2f;
    public float addAngleEyeFollowPlayer = 0f; // ค่าเพิ่มเติมในการหมุน
    public bool isFollowingPlayer = false; // เพิ่มตัวแปรเพื่อกำหนดว่ามอนสเตอร์จะตามผู้เล่นหรือไม่
    public Animator animatorBomb;
    [SerializeField] private Pool _poolBoomb;
    [SerializeField] private Pool _poolDieMySelf;
    public float LifeDuration = 5f;
    public SpriteRenderer spriteRenderer;
    private Coroutine _lifeCoroutine;
    [SerializeField] private List<Pool> _poolDie;
    public SpriteRenderer myBodyImg;
    public List<Sprite> ImgBody;
    public int type;
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
    [SerializeField] private ResourceManager _gameManagers;
    [SerializeField] private MonsterSpawner _monsterSpawner;
    public List<Shader> shaders;
    public List<SpriteRenderer> sprites;
    public float attackDelay = 1f;
    private float nextAttackTime;
    private bool isAttackPlayer = false;
    private int minSec = 0;
    [SerializeField] private LayerMask _obstacleLayerMask;
    [SerializeField] private float _obstacleCheckCircleRadius;
    [SerializeField] private float _obstacleCheckDistance;
    private Rigidbody2D _rigidbody;
    private RaycastHit2D[] _obstacleCollisions;
    private float _obstacleAvoidanceCooldown;
    private Vector2 _targetDirection;
    private const float safeDistance = 5f;
    private Coroutine _CoroutineDelayToBomb;
    bool bombed = false;

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

        if(_gameManagers.IsStopGame == true) return;
        if (Vector3.Distance(transform.position, player.position) > safeDistance)
        {
            _rigidbody.velocity = Vector2.zero;
            return;
        }

        FleeAndAvoidObstacles();
        MoveMonster();
        
        if (isFollowingPlayer)
        {
            Vector3 direction = player.position - transform.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            EyeHead.rotation = Quaternion.Euler(new Vector3(0, 0, targetAngle + addAngleEye + addAngleEyeFollowPlayer));
            EyeHead2.rotation = Quaternion.Euler(new Vector3(0, 0, targetAngle + addAngleEye2 + addAngleEyeFollowPlayer));
        }
        else
        {
            Vector3 direction = transform.position - player.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            EyeHead.rotation = Quaternion.Euler(new Vector3(0, 0, targetAngle + addAngleEye));
            EyeHead2.rotation = Quaternion.Euler(new Vector3(0, 0, targetAngle + addAngleEye2));
        }


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
        if(isFollowingPlayer == true){
            fleeDistance += 10000;
        }
        if(ImgBody.Count > 0 || isJoker == true){
            type = Random.Range(0, ImgBody.Count);
            myBodyImg.sprite = ImgBody[type];
        }
        if(spriteRenderer != null || isJoker){
            spriteRenderer.color = Color.white;
            _lifeCoroutine = StartCoroutine(LerpColor());
        }else if(isJoker == false){
            _CoroutineDelayToBomb = StartCoroutine(DelayToBomb());
        }
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

    private IEnumerator LerpColor()
    {
        float elapsedTime = 0f;
        Color startColor = Color.white;
        Color endColor = Color.black;

        while (elapsedTime < LifeDuration)
        {
            elapsedTime += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / LifeDuration);
            yield return null;
        }
            _poolDieMySelf.GetPool(transform.position);
        gameObject.SetActive(false);
            DieSoundEffect.RandomPool(transform.position);
        
    }
    private IEnumerator DelayToBomb()
    {
        animatorBomb.Play("idle2");
        yield return new WaitForSeconds(LifeDuration);
        StartCoroutine(Bomb());
    }
    private IEnumerator Bomb()
    {
        bombed = true;
        animatorBomb.Play("monsterBombAni");
        yield return new WaitForSeconds(1f);
        _poolBoomb.GetPool(transform.position);
        TakeDamage();
    }

    private void FleeAndAvoidObstacles()
    {/*
        if(ImgBody.Count == 0 && bombed == false && isJoker == false){
            if (Vector3.Distance(transform.position, player.position) > bombDistance) {
                if(_CoroutineDelayToBomb != null){
                    StopCoroutine(_CoroutineDelayToBomb);
                }
                 StartCoroutine(Bomb());
            }
        }
        */
        if (Vector3.Distance(transform.position, player.position) > fleeDistance)
        {
            _rigidbody.velocity = Vector2.zero;
            return;
        }

        Vector3 direction;

        if (isFollowingPlayer)
        {
            direction = (player.position - transform.position).normalized; // ตามผู้เล่น
        }
        else
        {
            direction = (transform.position - player.position).normalized; // หนีจากผู้เล่น
        }

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
        if (isFollowingPlayer)
        {
            // ตามผู้เล่น
            if (transform.position.x > player.position.x && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                EyeHead.transform.localScale = new Vector3(-EyeHead.transform.localScale.x, EyeHead.transform.localScale.y, EyeHead.transform.localScale.z);
                EyeHead2.transform.localScale = new Vector3(-EyeHead2.transform.localScale.x, EyeHead2.transform.localScale.y, EyeHead2.transform.localScale.z);
            }
            else if (transform.position.x < player.position.x && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                EyeHead.transform.localScale = new Vector3(-EyeHead.transform.localScale.x, EyeHead.transform.localScale.y, EyeHead.transform.localScale.z);
                EyeHead2.transform.localScale = new Vector3(-EyeHead2.transform.localScale.x, EyeHead2.transform.localScale.y, EyeHead2.transform.localScale.z);
            }
        }
        else
        {
            // หนีจากผู้เล่น
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

    }

    private void MoveMonster()
    {
        _rigidbody.velocity = _targetDirection * moveSpeed;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
            if (_gameManagers.IsStopGame == true) return;
            if (other.gameObject.CompareTag("Player"))
            {
                TakeDamage();
            }
    }

    IEnumerator Attacked(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void TakeDamage()
    {
        if(isJoker){
            _poolBoomb.GetPool(transform.position);
        }else if(_poolBoomb != null){
            _poolBoomb.GetPool(transform.position);
        }else{
            _poolDie[type].GetPool(transform.position);
            _gameManagers.AddDieCount();
            _gameManagers.AddXScore();
                Pool_TextDamage.GetPool(transform.position, "+" + ((10*(_gameManagers.xScore)).ToString()));
            _gameManagers.AddScore(10);
            _gameManagers._bloodXScore.color = GetColor();
            DieSoundEffect.RandomPool(transform.position);
        }
    //    _monsterSpawner.DeMonster();
        gameObject.SetActive(false);
    }

    public Color GetColor(){
        switch(type){
            case 0:
                return new Color(89/255f, 255/255f, 0/255f, 1);
            case 1:
                return new Color(255/255f, 217/255f, 0/255f, 1);
            case 2:
                return new Color(255/255f, 0/255f, 51/255f, 1);
            case 3:
                return new Color(148/255f, 0/255f, 255/255f, 1);
            case 4:
                return new Color(190/255f, 255/255f, 131/255f, 1);
            case 5:
                return new Color(255/255f, 244/255f, 119/255f, 1);
            case 6:
                return new Color(255/255f, 169/255f, 0/255f, 1);
            case 7:
                return new Color(255/255f, 218/255f, 185/255f, 1);
            case 8:
                return new Color(236/255f, 255/255f, 59/255f, 1);
            case 9:
                return new Color(250/255f, 218/255f, 94/255f, 1);
            case 10:
                return new Color(255/255f, 0/255f, 80/255f, 1);
            case 11:
                return new Color(255/255f, 13/255f, 0/255f, 1);
            default:
                return spriteRenderer.color;
        }
    }

    public void KnockBack(Vector3 knockbackPosition, float knockbackPower)
    {
        transform.GetComponent<Rigidbody2D>().AddForce(((Vector3)knockbackPosition - transform.position).normalized * knockbackPower * -1, ForceMode2D.Impulse);
        if (gameObject.activeSelf == false) return;
        StartCoroutine(Attacked(0.2f));
    }
}