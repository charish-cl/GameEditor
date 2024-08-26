using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GameDevKit
{
    public class FCode
    {
        private string filename;
        private string directoryname;
        private string namspacename;
        private string classname;
        private string path => Application.dataPath + $"/Script/Generate/{directoryname}/";
        
        private StringBuilder _builder = new StringBuilder();
        
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="instance_name">实例名</param>
        /// <param name="type">类型名</param>
        /// <param name="nullcontent">如果为空的语句return ....;</param>
        /// <returns></returns>
        public static string CreateInstance(string instance_name,string type,string nullcontent)
        {
            string temp = instance_name.Substring(1);
            //string lowername = char.ToLower(instance_name[0]) + temp;
            string lowername = "_"+ temp;
            string upername = char.ToUpper(instance_name[0]) + temp;
            string template =@"
            private static string _instance ;
            public static string Instance
            {
                get
                {
                    if ( _instance == null )
                    {
                        _instance = NullContent ;
                    }
                    return _instance ;
                }
                set
                {
                    _instance = value;
                }
            }";
            template=template.Replace("_instance", lowername);
            template=template.Replace("string", type);
            template=template.Replace("Instance", upername);
            template=template.Replace("NullContent", nullcontent);

            return template;
        }

        /// <summary>
        /// list转enum
        /// </summary>
        /// <param name="list"></param>
        /// <param name="enum_name"></param>
        /// <returns></returns>
        public static string LisToEnum(List<string> list,string enum_name)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"public enum {enum_name}");
            builder.AppendLine("{");
            foreach ( var x1 in list )
            {
                builder.AppendLine($"\t\t{x1},");
            }
            builder.AppendLine("}");
            SystemHelper.Copy(builder.ToString());
            return builder.ToString();
        }
    }
}