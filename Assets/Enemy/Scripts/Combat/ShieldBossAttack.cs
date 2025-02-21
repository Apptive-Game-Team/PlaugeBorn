using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ShieldBossAttack : MonoBehaviour, IEnemyAttack
{
    [FormerlySerializedAs("ShieldProjectile")] [SerializeField] private GameObject shieldProjectile;
    [Header("Four Direction")]
    private readonly Vector2[] cardinalPoints = {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    private readonly Vector2[] diagonalPoints = {
        new Vector2(1, 1).normalized,
        new Vector2(-1, 1).normalized,
        new Vector2(1, -1).normalized,
        new Vector2(-1, -1).normalized
    };

    [Header("방패 저장소")]
    [SerializeField]
    private List<GameObject> shieldList = new List<GameObject>();
    [Header("방패 속도 설정")]
    private float shieldSpeed = 5.0f;
    private float shieldDuration = 2.0f;

    [Header("돌진 속도 설정")]
    private float dashSpeed = 10.0f;
    private float dashWaitingDuration = 1.0f;
    private float dashDuration = 0.5f;
    private Rigidbody2D EnemyRb;

    [Header("경고선 설정")]
    public GameObject redLinePrefab;
    public float lineLength;
    public float waitingDuration = 1.5f;
    private List<GameObject> warningLines = new List<GameObject>();
    
    [Header("나선 공격 변수")]
    private float angle = 0f;
    private const float ONE_EIGHTY = 180f;
    private const float PLUS_DEGREE = 10f;
    
    // TODO: 살짝 답답하군
    public void Start()
    {
        lineLength = shieldSpeed * shieldDuration;
        EnemyRb = GetComponent<Rigidbody2D>();
    }

    public void Attack()
    {
        StartCoroutine(ExecuteDashAttackAfterDelay());
    }

    public void ShieldAttack(float duration)
    {
        StartCoroutine(TotalShieldAttack(duration));
    }

    public void SpiralAttack()
    {
        StartCoroutine(ExecuteSpiralAttack());
    }

    // 방패 던지기 패턴

    private IEnumerator TotalShieldAttack(float duration)
    {
        ShieldBossEnemyAI ai = GetComponent<ShieldBossEnemyAI>();
        ai.IsBlocking = true;
        yield return StartCoroutine(ExecuteShieldAttackAfterDelay(cardinalPoints));
        // TODO: 매직넘버 지우기
        yield return new WaitForSeconds(duration);
        yield return StartCoroutine(ExecuteShieldAttackAfterDelay(diagonalPoints));

        yield return new WaitUntil(() => (shieldList.Count == 0) && (warningLines.Count == 0));

        ai.IsBlocking = false;
        ai.ChangeState(new MoveState()); 
    }
    private void ShieldPattern(Vector2[] Points)
    {
        foreach (var point in Points)
        {
            GameObject Shield = Instantiate(shieldProjectile, transform);
            shieldList.Add(Shield);
            Shield.SetActive(true);
            ShieldProjectileAttack temp = Shield.GetComponent<ShieldProjectileAttack>();
            temp.SetBossAttack(this);
            temp.SetDir(point);
            temp.StartCoroutine(temp.ShieldAttack());
        }
    }

    public void RemoveShield(GameObject Shield)
    {
        shieldList.Remove(Shield);
    }

    /// <summary>
    /// Creates and displays warning lines from the object's position in the specified directions.
    /// </summary>
    /// <param name="Points">An array of direction vectors for each warning line.</param>
    /// <param name="lineLength">The length to extend each line from the starting position.</param>
    private void ShowWarningLines(Vector2[] Points, float lineLength)
    {
        if (Points.Length == 0 || Points == null)
        {
            // Exception
            Debug.Log("No Points to make Warning Lines");
            return;
        }

        Vector2 startPosition = transform.position;
        foreach (Vector2 dir in Points)
        {
            GameObject lineObj = Instantiate(redLinePrefab, startPosition, Quaternion.identity);
            LineRenderer lr = lineObj.GetComponent<LineRenderer>();
            if (lr)
            {
                lr.SetPosition(0, startPosition);
                lr.SetPosition(1, startPosition + dir * lineLength);
            }
            warningLines.Add(lineObj);
        }
    }

    private IEnumerator ExecuteShieldAttackAfterDelay(Vector2[] Points)
    {
        ShowWarningLines(Points, this.lineLength);
        yield return new WaitForSeconds(waitingDuration);
        ClearWarningLines();

        ShieldPattern(Points);
    }

    // 대쉬 패턴
    private void DashPattern(Vector2[] dir)
    {
        EnemyRb = GetComponent<Rigidbody2D>();
        if (dir.Length > 0)
        {
            EnemyRb.AddForce(dir[0] * dashSpeed, ForceMode2D.Impulse);
        }
        else
        {
            Debug.Log("No Points to make Warning Lines");
        }
    }

    private IEnumerator ExecuteDashAttackAfterDelay()
    {
        ShieldBossEnemyAI ai = GetComponent<ShieldBossEnemyAI>();
        ai.IsBlocking = true;
        // 편법으로 하기.
        Vector2[] dir = new Vector2[] { GetComponent<ShieldBossEnemyMovement>().GetDirection().normalized };
        ShowWarningLines(dir, dashDuration * dashSpeed);
        yield return new WaitForSeconds(dashWaitingDuration);

        ClearWarningLines();

        DashPattern(dir);
        yield return new WaitForSeconds(dashDuration);
        EnemyRb.velocity = Vector3.zero;

        ai.IsBlocking = false;
        ai.ChangeState(new MoveState()); 
    }

    /// <summary>
    /// Destroy All GameObject in warningLines
    /// </summary>
    private void ClearWarningLines()
    {
        foreach(GameObject line in warningLines)
        {
            Destroy(line);
        }
        warningLines.Clear();
    }
    
    // 나선 패턴
        
    private IEnumerator ExecuteSpiralAttack()
    {
        ShieldBossEnemyAI ai = GetComponent<ShieldBossEnemyAI>();
        ai.IsBlocking = true;
        // TODO: 매직넘버 지우기
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(0.1f);
            SpiralFirePattern(angle);
            SpiralFirePattern(angle + ONE_EIGHTY / 2);
            SpiralFirePattern(angle + ONE_EIGHTY);
            SpiralFirePattern(angle + ONE_EIGHTY + ONE_EIGHTY / 2);
            
            angle += PLUS_DEGREE;
        }
        // 반대로 ㄱㄱ
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(0.1f);
            SpiralFirePattern(angle + ONE_EIGHTY + ONE_EIGHTY / 2);
            SpiralFirePattern(angle + ONE_EIGHTY);
            SpiralFirePattern(angle + ONE_EIGHTY / 2);
            SpiralFirePattern(angle);
            
            angle -= PLUS_DEGREE;
        }
        ai.IsBlocking = false;
        angle = 0f;
        ai.ChangeState(new MoveState()); 
    }

    private void SpiralFirePattern(float angle)
    {
        float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / ONE_EIGHTY);
        float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / ONE_EIGHTY);
        
        Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0);
        Vector2 bulDir = (bulMoveVector - transform.position).normalized;

        GameObject bul = BulletPool.Instance.GetBullet();
        bul.transform.position = transform.position;
        bul.transform.rotation = transform.rotation;
        bul.SetActive(true);
        bul.GetComponent<SpiralBulletAttack>().SetMoveDirection(bulDir);
    }
}
