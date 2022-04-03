using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UI
{
    [CustomEditor(typeof(UIHolder))]
    public class UIHolderEditor : Editor
    {
        protected UIHolder _uiHolder;
        protected Type _viewType;

        private void OnEnable()
        {
            _uiHolder = (UIHolder) this.target;
            _className = _uiHolder.ClassName;

            if (string.IsNullOrEmpty(_className))
                return;
            _viewType = Util.GetType(_className);
            if (_viewType == null)
                return;
            _fieldInfos = GetTypeFieldInfos(_viewType);
        }

        private string _className = "";
        private List<FieldObject> _fieldInfos;

        public override void OnInspectorGUI()
        {
            if (_uiHolder == null)
                return;
            if (string.IsNullOrEmpty(_uiHolder.ClassName))
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("ClassName:");
                _className = EditorGUILayout.TextField(_className);
                if (GUILayout.Button("Bind"))
                {
                    if (string.IsNullOrEmpty(_className))
                        return;
                    _className = _className.Trim();
                    _viewType = Util.GetType(_className);
                    if (_viewType == null)
                    {
                        Debug.LogError($"_className:{_className} GetType is null");
                        return;
                    }

                    _uiHolder.ClassName = _className;
                    _fieldInfos = GetTypeFieldInfos(_viewType);
                }

                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label($"ClassName:{_className}");
                if (GUILayout.Button("UnBind"))
                {
                    if (EditorUtility.DisplayDialog("提示", "确认要解绑吗？解绑后数组无法恢复", "确定", "取消"))
                    {
                        _uiHolder.ClassName = "";
                    }
                }

                EditorGUILayout.EndHorizontal();
                if (_viewType == null)
                {
                    base.OnInspectorGUI();
                }
                else
                {
                    for (int i = 0; i < _fieldInfos.Count; i++)
                    {
                        IHolderObj holderObj = GetHolderObj(_fieldInfos[i]);
                        holderObj.DrawObj(_fieldInfos[i]);
                    }
                }
            }

            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }

        private IHolderObj GetHolderObj(FieldObject fieldInfo)
        {
            var fieldType = fieldInfo.Type;

            switch (fieldType)
            {
                case "UnityEngine.Vector3":
                    if (_uiHolder.Vector3Objs == null)
                        _uiHolder.Vector3Objs = new List<HolderVector3Obj>();
                    var Vector3Obj = _uiHolder.Vector3Objs.Find(x => x.VariableName.Equals(fieldInfo.Name));
                    if (Vector3Obj != null) return Vector3Obj;
                    Vector3Obj = new HolderVector3Obj {VariableName = fieldInfo.Name};
                    _uiHolder.Vector3Objs.Add(Vector3Obj);
                    return Vector3Obj;
                case "UnityEngine.Vector2":
                    if (_uiHolder.Vector2Objs == null)
                        _uiHolder.Vector2Objs = new List<HolderVector2Obj>();
                    var Vector2Obj = _uiHolder.Vector2Objs.Find(x => x.VariableName.Equals(fieldInfo.Name));
                    if (Vector2Obj != null) return Vector2Obj;
                    Vector2Obj = new HolderVector2Obj {VariableName = fieldInfo.Name};
                    _uiHolder.Vector2Objs.Add(Vector2Obj);
                    return Vector2Obj;
                case "UnityEngine.Color":
                    if (_uiHolder.ColorObjs == null)
                        _uiHolder.ColorObjs = new List<HolderColorObj>();
                    var ColorObj = _uiHolder.ColorObjs.Find(x => x.VariableName.Equals(fieldInfo.Name));
                    if (ColorObj != null) return ColorObj;
                    ColorObj = new HolderColorObj() {VariableName = fieldInfo.Name, VariableEntity = Color.white};
                    _uiHolder.ColorObjs.Add(ColorObj);
                    return ColorObj;
                case "System.Single":
                    if (_uiHolder.FloatObjs == null)
                        _uiHolder.FloatObjs = new List<HolderFloatObj>();
                    var FloatObj = _uiHolder.FloatObjs.Find(x => x.VariableName.Equals(fieldInfo.Name));
                    if (FloatObj != null) return FloatObj;
                    FloatObj = new HolderFloatObj() {VariableName = fieldInfo.Name};
                    _uiHolder.FloatObjs.Add(FloatObj);
                    return FloatObj;
                case "System.Int32":
                    if (_uiHolder.IntObjs == null)
                        _uiHolder.IntObjs = new List<HolderIntObj>();
                    var IntObj = _uiHolder.IntObjs.Find(x => x.VariableName.Equals(fieldInfo.Name));
                    if (IntObj != null) return IntObj;
                    IntObj = new HolderIntObj {VariableName = fieldInfo.Name};
                    _uiHolder.IntObjs.Add(IntObj);
                    return IntObj;
                case "System.String":
                    if (_uiHolder.StringObjs == null)
                        _uiHolder.StringObjs = new List<HolderStringObj>();
                    var StringObj = _uiHolder.StringObjs.Find(x => x.VariableName.Equals(fieldInfo.Name));
                    if (StringObj != null) return StringObj;
                    StringObj = new HolderStringObj {VariableName = fieldInfo.Name};
                    _uiHolder.StringObjs.Add(StringObj);
                    return StringObj;
                case "CustomType":
                    if (_uiHolder.CustomTypeObjs==null)
                        _uiHolder.CustomTypeObjs = new List<HolderCustomTypeObj>();
                    var CustomTypeObj = _uiHolder.CustomTypeObjs.Find(x => x.VariableName.Equals(fieldInfo.Name));
                    if (CustomTypeObj != null) return CustomTypeObj;
                    CustomTypeObj = new HolderCustomTypeObj {VariableName = fieldInfo.Name};
                    _uiHolder.CustomTypeObjs.Add(CustomTypeObj);
                    return CustomTypeObj;
                case "ArrayType":
                    if (_uiHolder.ArrayTypeObjs == null)
                        _uiHolder.ArrayTypeObjs = new List<HolderArrayTypeObj>();
                    var ArrayTypeObj = _uiHolder.ArrayTypeObjs.Find(x => x.VariableName.Equals(fieldInfo.Name));
                    if (ArrayTypeObj != null) return ArrayTypeObj;
                    ArrayTypeObj = new HolderArrayTypeObj {VariableName = fieldInfo.Name};
                    _uiHolder.ArrayTypeObjs.Add(ArrayTypeObj);
                    return ArrayTypeObj;
                default:
                    if (_uiHolder.Objs == null)
                        _uiHolder.Objs = new List<HolderObj>();
                    var Obj = _uiHolder.Objs.Find(x => x.VariableName.Equals(fieldInfo.Name));
                    if (Obj != null) return Obj;
                    Obj = new HolderObj {VariableName = fieldInfo.Name};
                    _uiHolder.Objs.Add(Obj);
                    return Obj;
            }
        }


        public List<FieldObject> GetTypeFieldInfos(Type type)
        {
            var result = new List<FieldObject>();
            var fields = type.GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                if (field.CustomAttributes.Count(x => x.AttributeType == typeof(ShowInspector)) > 0)
                {
                    if (typeof(ISerializableObj).IsAssignableFrom(field.FieldType))
                    {
                        if (field.FieldType.IsArray)
                        {
                        }
                        else
                        {
                            result.Add(GenerateCustomType(field));
                        }
                    }
                    else if (field.FieldType.IsArray)
                    {
                        result.Add(GenerateArrayType(field));
                    }
                    else
                    {
                        result.Add(GenerateFieldObject(field));
                    }
                }
            }

            return result;
        }

        private FieldObject GenerateArrayType(FieldInfo fieldInfo)
        {
            var fieldObject = new FieldObject();
            fieldObject.Name = fieldInfo.Name;
            fieldObject.Type = "ArrayType";
            fieldObject.FieldType = fieldInfo.FieldType.GetElementType();
            return fieldObject;
        }

        private FieldObject GenerateCustomType(FieldInfo fieldInfo)
        {
            var fieldObject = new FieldObject();
            fieldObject.Name = fieldInfo.Name;
            fieldObject.Type = "CustomType";
            fieldObject.ObjectTypes = new Dictionary<string, Type>();
            var fields = fieldInfo.FieldType.GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                fieldObject.ObjectTypes.Add(field.Name, field.FieldType);
            }

            return fieldObject;
        }

        private FieldObject GenerateFieldObject(FieldInfo field)
        {
            var fieldObject = new FieldObject();
            fieldObject.Name = field.Name;
            fieldObject.Type = field.FieldType.ToString();
            fieldObject.FieldType = field.FieldType;
            return fieldObject;
        }
    }
}