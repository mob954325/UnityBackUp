using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Data : IComparable<Test_Data> // List.Sort()�� ����ϱ� ���� �������̽�
{
    public int x;
    public float y;
    public string z;

    // ������ �����(x, y, z�� �ޱ�)
    public Test_Data(int x, float y, string z) 
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    // TestData�� ����Ʈ���� Sort�Լ� ����� �� �ְ� ����� (������ z)
    // Sort() �������� ����
    public int CompareTo(Test_Data other)
    {
        if(other == null)
            return 1;

        return other.z.CompareTo(this.z);
    }
    

    // == ��ɾ� �����ε��ϱ� (x���� ������ ����.)

    public static bool operator == (Test_Data left, Test_Data right)
    {
        return left.x == right?.x;
    }

    public static bool operator != (Test_Data left, Test_Data right)
    {
        return left.x != right.x;
    }

    public override bool Equals(object obj)
    {
        // obj�� Node Ŭ������ this�� obj�� x,y�� ����.
        return obj is Test_Data other && this.x == other.x;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y, z);
    }
}
