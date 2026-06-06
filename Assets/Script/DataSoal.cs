using UnityEngine;
using System.Collections.Generic; 
using System;
[Serializable]
public class DataSoal
{
    [TextArea(2,10)] public string Soal;
    public List<string>jawaban;
    public int indexJawabanBenar;
}
