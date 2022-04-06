using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class MainUI
{
    [UI.ShowInspector] public Button btn1;
    [UI.ShowInspector] public Button btn2;
    [UI.ShowInspector] public Button[] btn3;
    [UI.ShowInspector] public Text txt1;
    [UI.ShowInspector] public Text txt2;
    [UI.ShowInspector] public Text[] txt3;
    [UI.ShowInspector] public GameObject obj1;
    [UI.ShowInspector] public GameObject[] obj2;
    [UI.ShowInspector] public int int1;
    [UI.ShowInspector] public int[] int2;
    [UI.ShowInspector] public string string1;
    [UI.ShowInspector] public string[] string2;
    [UI.ShowInspector] public float float1;
    [UI.ShowInspector] public Color color1;
    [UI.ShowInspector] public Vector2 ve21;
    [UI.ShowInspector] public Vector3 ve31;
    [UI.ShowInspector] public Vector3[] ve32;
    [UI.ShowCustomType] public TestObj testObj1;


    [Serializable]
    public class TestObj 
    {
        public Button btn1;
        public Button[] btn2;
        public Text txt1;
        public int int1;
        public string string1;
        public Color color1;
        
    }

    public void Test()
    {
        UI.UIHolder uiHolder;
        TestObj obj;
       
    }

}
