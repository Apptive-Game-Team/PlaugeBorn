using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * �÷��̾� ������ ���� ������ �� ���� ������ �ʿ伺�� ����
 * ���� ����Ǿ ������ �ʿ伺�� ����.
 * ���� �÷��̾� ������ ������ �̱��� ������Ʈ�� ����.
 */

public class StatsManager : MonoBehaviour
{
    private static StatsManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance.gameObject);
            InitStats();
        }
        else
        {
            if (_instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public static StatsManager Instance
    {
        get
        {
            // get ȣ��ÿ� _instace�� ���ٴ� ����, statsManager ���� ������Ʈ�� �������� �ʾҴٴ� �ǹ��̹Ƿ� ���� ���־�� ��
            if (_instance == null)
            {
                _instance = new GameObject("StatsManager").AddComponent<StatsManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    // �÷��̾� ���� ����
    private int hp;
    private int armor;
    private int attackPower;
    private int speed;


    // ������Ƽ
    public int Hp
    {
        get { return hp; }
        set { hp = value; }
    }
    public int Armor
    {
        get { return armor; }
        set { armor = value; }
    }
    public int AttackPower
    {
        get { return attackPower; }
        set { attackPower = value; }
    }
    public int Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitStats()
    {
        Hp = 100;
        Armor = 10;
        AttackPower = 10;
        Speed = 10;
    }
}
