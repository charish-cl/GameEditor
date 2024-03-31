using System.Reflection;
using GameDevKit;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using UnityEditor;

namespace GameDevKitEditor
{
    using UnityEngine;
    using System;
    using System.IO;

    public class SaveValueWindow<T> : OdinEditorWindow
    {
        public DataFormat dataFormat = DataFormat.Binary;

        protected virtual void OnEnable()
        {
            LoadData();
        }

        protected virtual void OnValidate()
        {
            SaveData();
        }

        private void SaveData()
        {
            string filePath = Path.Combine(Application.persistentDataPath, this.GetType().Name + ".byte");

            byte[] bytes = SerializationUtility.SerializeValue(this, dataFormat);

            File.WriteAllBytes(filePath, bytes);

            Debug.Log($"保存{this}");
            
        }
        private void LoadData()
        {
            string filePath = Path.Combine(Application.persistentDataPath, this.GetType().Name + ".byte");

            if (!File.Exists(filePath))
            {
                return;
            }
            byte[] bytes = File.ReadAllBytes(filePath);
            //TODO:有点小瑕疵，DeserializeValue好像有些值不能为空
            var deserializeValue = SerializationUtility.DeserializeValue<T>(bytes, dataFormat);

            
            // 使用反射拷贝值给自身，但只拷贝子类的成员
            Type type = this.GetType();
            Type baseType = typeof(T);
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (field.DeclaringType == baseType)
                {
                    Debug.Log($"{field.DeclaringType} {field.Name}");
                    var value = field.GetValue(deserializeValue);
                    field.SetValue(this, value);
                }
            }

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (prop.CanRead && prop.CanWrite && prop.DeclaringType == baseType)
                {
                    Debug.Log($"{prop.DeclaringType} {prop.Name}");

                    var value = prop.GetValue(deserializeValue);
                    prop.SetValue(this, value);
                }
            }
        }
    }
}