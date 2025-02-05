using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// CustomEditor�� ����Ͽ� AbstractDungeonGenerator �Ǵ� �� �ڽ� Ŭ�������� �ν����� UI�� Ŀ���͸���¡
/// �̷��� �����ν� �÷��� �߿� �� �� ��ġ�� ��ư�� ������ ���� �����ϴ� ���� �ƴ�,
/// �÷��� ���� �ƴϴ���, �ν����� â���� ��ư�� ���� ���� ������.
/// �̷ν� ���� �÷��̰� ����Ǵ��� ������ ���� ���� �״�� ���� ��.
/// </summary>
[CustomEditor(typeof(AbstractDungeonGenerator), true)]
public class RandomDungeonGeneratorEditor : Editor
{
    AbstractDungeonGenerator generator; // DungeonGenerator ������Ʈ�� �ν��Ͻ��� ������ ����

    private void Awake()
    {
        // �����Ϳ��� ���õ� ���� ������Ʈ�� ������Ʈ�� AbstractDungeonGenerator Ÿ������ ĳ�����ؼ� generator�� �Ҵ�
        generator = (AbstractDungeonGenerator)target; 
    }

    // �ν����� GUI�� �׸� �� ȣ��Ǵ� �Լ�
    public override void OnInspectorGUI()
    {
        // �⺻������ �����Ǵ� �ν����� UI ��ҵ��� �׷���
        base.OnInspectorGUI();

        // "Create Dungeon" ��ư�� �ν����Ϳ� �߰�
        if (GUILayout.Button("Create Dungeon"))
        {
            // ��ư�� Ŭ���Ǹ� GenerateDungeon �޼��带 ȣ���Ͽ� ������ ����
            generator.GenerateDungeon();
        }
    }
}
