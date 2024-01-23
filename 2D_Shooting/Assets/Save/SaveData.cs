using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Json 유틸리티에서 사용하기 위해서는 직렬화 가능한 클래스 이어야한다 .
[Serializable]
public class SaveData
{
    public string[] rankerName;
    public int[] highScore;
}
