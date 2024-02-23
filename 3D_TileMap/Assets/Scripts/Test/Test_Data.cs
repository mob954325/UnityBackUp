using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Data : IComparable<Test_Data> // List.Sort()을 사용하기 위한 인터페이스
{
    public int x;
    public float y;
    public string z;

    // 생성자 만들기(x, y, z값 받기)
    public Test_Data(int x, float y, string z) 
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    // TestData의 리스트에서 Sort함수 사용할 수 있게 만들기 (기준은 z)
    // Sort() 내림차순 정리
    public int CompareTo(Test_Data other)
    {
        if(other == null)
            return 1;

        return other.z.CompareTo(this.z);
    }
    

    // == 명령어 오버로딩하기 (x값이 같으면 같다.)

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
        // obj는 Node 클래스고 this와 obj의 x,y가 같다.
        return obj is Test_Data other && this.x == other.x;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y, z);
    }
}
