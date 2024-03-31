using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameDevKit;
using GameEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace GameDevKitEditor
{
    [TreeWindow("数据创建工具")]
    public class DataBaseCreateWindow:OdinEditorWindow
    {
  
        [TypeFilter("GetFilteredTypeList")]
        [OnValueChanged("UpdateName")]
        public BaseDataBase A;
        

        [FolderPath]
        public string path = "Assets/Config/DataBase/";
        
        
        public string Name = "NewDataBase";
        
        public void UpdateName()
        {
            Name = A.GetType().Name;
        }
        public IEnumerable<Type> GetFilteredTypeList()
        {
            return TypeCache.GetTypesDerivedFrom<BaseDataBase>().Where(e=>!e.IsGenericType);
        }
    
        //打印A
        [Button("打印")]
        public void Print()
        {
            Debug.Log(A);
        }
        [Button("创建")]
        public void Create()
        { 
            //TODO:可读性很差Q^Q！
            FileKit.CreatDirectoryIfEmpty(path);
            var data =ScriptableObject.CreateInstance(A.GetType());
            data.Create(path,Name);
        }
    }
}