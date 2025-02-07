using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * �� �ε� ���� ����� ������ �ʿ伺�� ���� ��ɵ�.
 * �ش� ��ɿ� ���� �ڵ� �ߺ����� ���̰� ����.
 * �׷��� �ű��� ������Ʈ�� ���� ���������� �����ؼ� ����� �� �ֵ��� ��.
 */

public class SceneController : MonoBehaviour
{
    private static SceneController _instance;
    // ������ private�� �����Ͽ� �ܺο��� ��ü �������� ���ϰ� ����.
    private SceneController() { }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance.gameObject);
        }
        else // �̹� �̱��� ������Ʈ�� �����ϴµ�
        {
            if (_instance != this) // ���� �ٸ� ��ü��� �ϳ��� �����ؾ� �� �̱��� ������Ʈ�� �� �� �̻��̶�� ��
            {
                Destroy(this.gameObject); // �׷��� �������־�� ��.
            }
        }
    }

    public static SceneController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("SceneController").AddComponent<SceneController>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    public void LoadScene(string sceneName)
    {
        try { SceneManager.LoadScene(sceneName); }
        catch { Debug.LogError("Scene " + sceneName + " not found."); }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
