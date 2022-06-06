using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI
{
    public class UIHolder : MonoBehaviour
    {
        private string ClassName;
        public List<HolderObj> Objs;
        public List<HolderStringObj> StringObjs;
        public List<HolderColorObj> ColorObjs;
        public List<HolderIntObj> IntObjs;
        public List<HolderFloatObj> FloatObjs;
        public List<HolderVector2Obj> Vector2Objs;
        public List<HolderVector3Obj> Vector3Objs;
        public List<HolderCustomTypeObj> CustomTypeObjs;
        public List<HolderArrayTypeObj> ArrayTypeObjs;

        #region get obj value
        
        public T GetVariableComponent<T>(string variableName) where T : Object
        {
            if (Objs == null || Objs.Count == 0)
                return null;
            T result = default(T);
            for (int i = 0; i < Objs.Count; i++)
            {
                if (Objs[i].VariableName.Equals(variableName))
                {
                    result = Objs[i] == null ? null : Objs[i].VariableEntity as T;
                    return result;
                }
            }

            return result;
        }
        public T[] GetVariableArrayComponent<T>(string variableName) where T : Object
        {
            if (ArrayTypeObjs == null || ArrayTypeObjs.Count == 0)
                return null;
            T[] result = null;
            for (int i = 0; i < ArrayTypeObjs.Count; i++)
            {
                if (ArrayTypeObjs[i].VariableName.Equals(variableName))
                {
                    return ArrayTypeObjs[i].GetEntity<T>();
                }
            }
            return result;
        }
        public int GetIntValue(string variableName)
        {
            if (IntObjs == null || IntObjs.Count == 0)
                return 0;
            int result = 0;
            for (int i = 0; i < IntObjs.Count; i++)
            {
                if (IntObjs[i].VariableName.Equals(variableName))
                {
                    result = IntObjs[i] == null ? 0 : IntObjs[i].VariableEntity;
                }
            }

            return result;
        }

        public string GetStringValue(string variableName)
        {
            if (StringObjs == null || StringObjs.Count == 0)
                return string.Empty;
            string result = string.Empty;
            for (int i = 0; i < StringObjs.Count; i++)
            {
                if (StringObjs[i].VariableName.Equals(variableName))
                {
                    result =string.IsNullOrEmpty(StringObjs[i].VariableEntity) ? "" : StringObjs[i].VariableEntity;
                }
            }

            return result;
        }

        public Color GetColorValue(string variableName)
        {
            if (ColorObjs == null || ColorObjs.Count == 0)
                return Color.clear;
            Color result = Color.clear;
            for (int i = 0; i < ColorObjs.Count; i++)
            {
                if (ColorObjs[i].VariableName.Equals(variableName))
                {
                    result = ColorObjs[i] == null ? Color.clear : ColorObjs[i].VariableEntity;
                }
            }

            return result;
        }

        public float GetFloatValue(string variableName)
        {
            if (FloatObjs == null || FloatObjs.Count == 0)
                return 0f;
            float result = 0f;
            for (int i = 0; i < FloatObjs.Count; i++)
            {
                if (FloatObjs[i].VariableName.Equals(variableName))
                {
                    result = FloatObjs[i] == null ? 0f : FloatObjs[i].VariableEntity;
                }
            }

            return result;
        }
        

        #endregion
        
    }

    #region Holder Obj

  
    public interface IHolderObj
    {
        void DrawObj(FieldObject fieldInfo);
    }

    [Serializable]
    public class HolderArrayTypeObj : IHolderObj
    {
        public string VariableName;
        public List<HolderObj> ObjEntity;
        public List<HolderStringObj> StringEntity;
        public List<HolderIntObj> IntEntity;
        public List<HolderFloatObj> FloatEntity;
        public List<HolderColorObj> ColorEntity;
        public int Size;
        public bool Expand;
        public T[] GetEntity<T>()where T : Object
        {
            T[] result= null;
            if (typeof(T) == typeof(string))
            {
                result = new T[StringEntity.Count];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = StringEntity[i].VariableEntity as T;
                }
            }
            else if (typeof(T) == typeof(int))
            {
                result = new T[IntEntity.Count];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = IntEntity[i].VariableEntity as T;
                }
            }
            else if (typeof(T) == typeof(float))
            {
                result = new T[FloatEntity.Count];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = FloatEntity[i].VariableEntity as T;
                }
            }
            else if (typeof(T) == typeof(Color))
            {
                result = new T[ColorEntity.Count];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = ColorEntity[i].VariableEntity as T;
                }
            }
            else
            {
                result = new T[ObjEntity.Count];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = ObjEntity[i].VariableEntity as T;
                }
            }

            return result;
        }

        public void DrawObj(FieldObject fieldInfo)
        {
            DrawObjWithSpace(fieldInfo.FieldType, "    ");
        }

        public void DrawObjWithSpace(Type type, string space, string mainNameSpace = "")
        {
#if UNITY_EDITOR
            Expand = UnityEditor.EditorGUILayout.Foldout(Expand, $"{mainNameSpace}{VariableName}");
            if (!Expand)
                return;
            Size = UnityEditor.EditorGUILayout.IntField($"{space}Size", Size);

            for (int i = 0; i < Size; i++)
            {
                if (type == typeof(string))
                {
                    if (StringEntity == null)
                        StringEntity = new List<HolderStringObj>();
                    var holderObj = StringEntity.Count > i ? StringEntity[i] : null;
                    if (holderObj == null)
                    {
                        holderObj = new HolderStringObj();
                        holderObj.VariableName = $"Element {i}";
                        StringEntity.Add(holderObj);
                    }

                    holderObj.DrawObjWithSpace(space);
                }
                else if (type == typeof(int))
                {
                    if (IntEntity == null)
                        IntEntity = new List<HolderIntObj>();
                    var holderObj = IntEntity.Count > i ? IntEntity[i] : null;
                    if (holderObj == null)
                    {
                        holderObj = new HolderIntObj();
                        holderObj.VariableName = $"Element {i}";
                        IntEntity.Add(holderObj);
                    }

                    holderObj.DrawObjWithSpace(space);
                }
                else if (type == typeof(float))
                {
                    if (FloatEntity == null)
                        FloatEntity = new List<HolderFloatObj>();
                    var holderObj = FloatEntity.Count > i ? FloatEntity[i] : null;
                    if (holderObj == null)
                    {
                        holderObj = new HolderFloatObj();
                        holderObj.VariableName = $"Element {i}";
                        FloatEntity.Add(holderObj);
                    }

                    holderObj.DrawObjWithSpace(space);
                }
                else if (type == typeof(Color))
                {
                    if (ColorEntity == null)
                        ColorEntity = new List<HolderColorObj>();
                    var holderObj = ColorEntity.Count > i ? ColorEntity[i] : null;
                    if (holderObj == null)
                    {
                        holderObj = new HolderColorObj();
                        holderObj.VariableName = $"Element {i}";
                        ColorEntity.Add(holderObj);
                    }

                    holderObj.DrawObjWithSpace(space);
                }
                else
                {
                    if (ObjEntity == null)
                        ObjEntity = new List<HolderObj>();
                    var holderObj = ObjEntity.Count > i ? ObjEntity[i] : null;
                    if (holderObj == null)
                    {
                        holderObj = new HolderObj();
                        holderObj.VariableName = $"Element {i}";
                        ObjEntity.Add(holderObj);
                    }

                    holderObj.DrawObjWithSpace(type, space);
                }
            }
#endif
        }
    }

    [Serializable]
    public class HolderCustomTypeObj : IHolderObj
    {
        public string VariableName;
        public List<HolderObj> ObjEntity;
        public List<HolderStringObj> StringEntity;
        public List<HolderIntObj> IntEntity;
        public List<HolderFloatObj> FloatEntity;
        public List<HolderColorObj> ColorEntity;
        public List<HolderArrayTypeObj> ObjArrayEntity;
        private bool Expand;

        public void DrawObj(FieldObject fieldInfo)
        {
#if UNITY_EDITOR
            var space = "    ";
            Expand = UnityEditor.EditorGUILayout.Foldout(Expand, fieldInfo.Name);
            if (!Expand)
                return;
            foreach (var item in fieldInfo.ObjectTypes)
            {
                if (item.Value == typeof(string))
                {
                    if (StringEntity == null)
                        StringEntity = new List<HolderStringObj>();
                    var holderObj = StringEntity.Find(x => x.VariableName.Equals(item.Key));
                    if (holderObj == null)
                    {
                        holderObj = new HolderStringObj();
                        holderObj.VariableName = item.Key;
                        StringEntity.Add(holderObj);
                    }
                    holderObj.DrawObjWithSpace(space);
                }
                else if (item.Value == typeof(int))
                {
                    if (IntEntity == null)
                        IntEntity = new List<HolderIntObj>();
                    var holderObj = IntEntity.Find(x => x.VariableName.Equals(item.Key));
                    if (holderObj == null)
                    {
                        holderObj = new HolderIntObj();
                        holderObj.VariableName = item.Key;
                        IntEntity.Add(holderObj);
                    }
                    holderObj.DrawObjWithSpace(space);
                }
                else if (item.Value == typeof(float))
                {
                    if (FloatEntity == null)
                        FloatEntity = new List<HolderFloatObj>();
                    var holderObj = FloatEntity.Find(x => x.VariableName.Equals(item.Key));
                    if (holderObj == null)
                    {
                        holderObj = new HolderFloatObj();
                        holderObj.VariableName = item.Key;
                        FloatEntity.Add(holderObj);
                    }
                    holderObj.DrawObjWithSpace(space);
                }
                else if (item.Value == typeof(Color))
                {
                    if (ColorEntity == null)
                        ColorEntity = new List<HolderColorObj>();
                    var holderObj = ColorEntity.Find(x => x.VariableName.Equals(item.Key));
                    if (holderObj == null)
                    {
                        holderObj = new HolderColorObj();
                        holderObj.VariableName = item.Key;
                        holderObj.VariableEntity = Color.white;
                        ColorEntity.Add(holderObj);
                    }
                    holderObj.DrawObjWithSpace(space);
                }
                else if (item.Value.IsArray)
                {
                    if (ObjArrayEntity == null)
                        ObjArrayEntity = new List<HolderArrayTypeObj>();
                    var holderObj = ObjArrayEntity.Find(x => x.VariableName.Equals(item.Key));
                    if (holderObj == null)
                    {
                        holderObj = new HolderArrayTypeObj();
                        holderObj.VariableName = item.Key;
                        ObjArrayEntity.Add(holderObj);
                    }
                    holderObj.DrawObjWithSpace(item.Value.GetElementType(), "        ", "    ");
                }
                else
                {
                    if (ObjEntity == null)
                        ObjEntity = new List<HolderObj>();
                    var holderObj = ObjEntity.Find(x => x.VariableName.Equals(item.Key));
                    if (holderObj == null)
                    {
                        holderObj = new HolderObj();
                        holderObj.VariableName = item.Key;
                        ObjEntity.Add(holderObj);
                    }
                    holderObj.DrawObjWithSpace(item.Value, space);
                }
            }
#endif
        }
    }

    [Serializable]
    public class HolderObj : IHolderObj
    {
        public string VariableName;
        public Object VariableEntity;

        public void DrawObj(FieldObject fieldInfo)
        {
            DrawObjWithSpace(fieldInfo.FieldType);
        }

        public void DrawObjWithSpace(Type type, string space = "")
        {
#if UNITY_EDITOR
            var entity = UnityEditor.EditorGUILayout.ObjectField($"{space}{VariableName}", VariableEntity, type, true);
            if (entity!=null&&!Equals(entity, VariableEntity))
            {
                VariableEntity = entity;
            }
#endif
        }
    }

    [Serializable]
    public class HolderVector2Obj : IHolderObj
    {
        public string VariableName;
        public Vector2 VariableEntity;

        public void DrawObj(FieldObject fieldInfo)
        {
            DrawObjWithSpace();
        }

        public void DrawObjWithSpace(string space = "")
        {
#if UNITY_EDITOR
            var entity = UnityEditor.EditorGUILayout.Vector2Field($"{space}{VariableName}", VariableEntity);
            if (!entity.Equals(VariableEntity))
            {
                VariableEntity = entity;
            }
#endif
        }
    }

    [Serializable]
    public class HolderVector3Obj : IHolderObj
    {
        public string VariableName;
        public Vector3 VariableEntity;

        public void DrawObj(FieldObject fieldInfo)
        {
            DrawObjWithSpace();
        }

        public void DrawObjWithSpace(string space = "")
        {
#if UNITY_EDITOR
            var entity = UnityEditor.EditorGUILayout.Vector3Field($"{space}{VariableName}", VariableEntity);
            if (!entity.Equals(VariableEntity))
            {
                VariableEntity = entity;
            }
#endif
        }
    }

    [Serializable]
    public class HolderColorObj : IHolderObj
    {
        public string VariableName;
        public Color VariableEntity;

        public void DrawObj(FieldObject fieldInfo)
        {
            DrawObjWithSpace();
        }

        public void DrawObjWithSpace(string space = "")
        {
#if UNITY_EDITOR
            var entity = UnityEditor.EditorGUILayout.ColorField($"{space}{VariableName}", VariableEntity);
            if (!entity.Equals(VariableEntity))
            {
                VariableEntity = entity;
            }
#endif
        }
    }

    [Serializable]
    public class HolderIntObj : IHolderObj
    {
        public string VariableName;
        public int VariableEntity;

        public void DrawObj(FieldObject fieldInfo)
        {
            DrawObjWithSpace();
        }

        public void DrawObjWithSpace(string space = "")
        {
#if UNITY_EDITOR
            var entity = UnityEditor.EditorGUILayout.IntField($"{space}{VariableName}", VariableEntity);
            if (!entity.Equals(VariableEntity))
            {
                VariableEntity = entity;
            }
#endif
        }
    }

    [Serializable]
    public class HolderStringObj : IHolderObj
    {
        public string VariableName;
        public string VariableEntity;

        public void DrawObj(FieldObject fieldInfo)
        {
            DrawObjWithSpace();
        }

        public void DrawObjWithSpace(string space = "")
        {
#if UNITY_EDITOR
            var entity = UnityEditor.EditorGUILayout.TextField($"{space}{VariableName}", VariableEntity);
            if (entity!=null&&!entity.Equals(VariableEntity))
            {
                VariableEntity = entity;
            }
#endif
        }
    }

    [Serializable]
    public class HolderFloatObj : IHolderObj
    {
        public string VariableName;
        public float VariableEntity;

        public void DrawObj(FieldObject fieldInfo)
        {
            DrawObjWithSpace();
        }

        public void DrawObjWithSpace(string space = "")
        {
#if UNITY_EDITOR
            var entity = UnityEditor.EditorGUILayout.FloatField($"{space}{VariableName}", VariableEntity);
            if (!entity.Equals(VariableEntity))
            {
                VariableEntity = entity;
            }
#endif
        }
    }

    public sealed class ShowInspector : Attribute
    {
    }
    public sealed class ShowCustomType : Attribute
    {
    }

    public class FieldObject
    {
        public string Name;
        public string Type;
        public Type FieldType;
        public Dictionary<string, Type> ObjectTypes;
    }

    #endregion
}
