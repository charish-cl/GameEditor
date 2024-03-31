using System;
using System.Collections.Generic;
using System.Text;

namespace GameDevKit
{
    public class FCodeBuilder
    {
        StringBuilder builder = new StringBuilder();

        //保留符号后缀
        Stack<string> Stack = new Stack<string>();

        public FCodeBuilder AddClassAndNamSpace(string className, string NameSpace, string parentClass = null)
        {
            builder.AppendLine($"namespace {NameSpace}");
            AddCharacter();
            AddClass(className, parentClass);
            return this;
        }

        public FCodeBuilder AddClass(string className, string parentClass = null)
        {
            if (parentClass == null)
            {
                builder.AppendLine($"public class {className}");
            }
            else
            {
                builder.AppendLine($"public class {className} : {parentClass}");
            }

            AddCharacter();
            return this;
        }

        public FCodeBuilder AddStruct(string className)
        {
            builder.AppendLine($"public class {className}");
            AddCharacter();
            return this;
        }

        public FCodeBuilder AppendLine(string content)
        {
            builder.AppendLine(content);
            return this;
        }

        public FCodeBuilder AppendCharacterLine(string content)
        {
            builder.AppendLine("{");
            builder.AppendLine(content);
            builder.AppendLine("}");
            return this;
        }

        public FCodeBuilder Append(string content)
        {
            builder.Append(content);
            return this;
        }

        /// <summary>
        /// 吸收括号，函数具体描述
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public FCodeBuilder AppendContent(string content)
        {
            builder.AppendLine(content);
            EndCharacter();
            return this;
        }

        public FCodeBuilder AddMessageHeader(string className)
        {
            builder.AppendLine($"message {className}");
            AddCharacter();
            return this;
        }

        public FCodeBuilder AddMember(Type type, string className, bool isPublic = true)
        {
            string pre = "public";
            if (isPublic)
            {
                pre = "private";
            }

            builder.AppendLine($"{pre} {type} {className};");
            return this;
        }


        //参数为List元组,元组第一个为类型，第二个为名字，第三个为注释
        public FCodeBuilder AddMember(List<(Type, string, string)> list, bool isPublic = true)
        {
            string pre = "public";
            if (isPublic)
            {
                pre = "private";
            }

            foreach (var item in list)
            {
                builder.AppendLine($"{pre} {item.Item1} {item.Item2}; //{item.Item3}");
            }

            return this;
        }


        public FCodeBuilder AddEnumFromLis(string enumName, List<string> list, bool isPublic = true)
        {
            if (isPublic)
            {
                builder.AppendLine($"public enum {enumName}");
            }
            else
            {
                builder.AppendLine($"enum {enumName}");
            }

            AddCharacter();

            for (var i = 0; i < list.Count; i++)
            {
                builder.AppendLine($"{list[i]} = {i},");
            }

            EndCharacter();
            return this;
        }

        /// <summary>
        /// Proto的Enum有区别的
        /// </summary>
        /// <param name="enumName"></param>
        /// <param name="list"></param>
        /// <param name="isPublic"></param>
        /// <returns></returns>
        public FCodeBuilder AddProtoEnumLis(string enumName, List<string> list)
        {
            builder.AppendLine($"enum {enumName}");
            AddCharacter();

            for (var i = 0; i < list.Count; i++)
            {
                builder.AppendLine($"{list[i]} = {i};");
            }

            EndCharacter();
            return this;
        }

        private Dictionary<string, string> ProDic = new Dictionary<string, string>()
        {
            { "System.Single", "float" },
            { "System.Boolean", "bool" },
            { "System.Int32", "int32" },
            { "System.String", "string" },
        };

        public FCodeBuilder ClassAndEnumToProto(Type t)
        {
            var name = t.Name;
            if (t.IsEnum)
            {
                AddProtoEnumLis(name, ReflectionHelper.GetEnumPropToList(t));
            }
            else
            {
                int i = 1;
                var fieldInfos = t.GetAllFiels();
                AddMessageHeader(t.Name);
                foreach (var e in fieldInfos)
                {
                    var typeName = e.FieldType.ToString();
                    //基本类型
                    if (ProDic.ContainsKey(typeName))
                    {
                        typeName = ProDic[typeName];
                    }
                    //泛型以及其它
                    else
                    {
                        //泛型
                        if (typeName.Contains("`"))
                        {
                            typeName = typeName.SplitAfter('+').SplitBefore(']');
                            typeName = $"repeated {typeName}";
                        }
                        //类
                        else
                        {
                            typeName = typeName.SplitAfter('+');
                        }
                    }

                    builder.AppendLine($"{typeName} {e.Name} = {i++};");
                }

                EndCharacter();
            }

            return this;
        }

        public FCodeBuilder AddFunction(string className)
        {
            builder.AppendLine($"public void {className}()");
            AddCharacter();
            return this;
        }

        /// <summary>
        /// 加大括号
        /// </summary>
        public FCodeBuilder AddCharacter()
        {
            builder.AppendLine("{");
            Stack.Push("}");
            return this;
        }

        public FCodeBuilder AddNameSpace(params string[] s)
        {
            foreach (var s1 in s)
            {
                builder.AppendLine($"using {s1};");
            }


            return this;
        }

        public FCodeBuilder EndCharacter()
        {
            builder.AppendLine(Stack.Pop());
            return this;
        }

        public override string ToString()
        {
            while (Stack.Count != 0)
            {
                builder.AppendLine(Stack.Pop());
            }

            return builder.ToString();
        }

        public void Copy()
        {
            SystemHelper.Copy(this.ToString());
        }

        public void Clear()
        {
            builder.Clear();
            Stack.Clear();
        }

        public void CreateScript(string path, string name)
        {
            var s = ToString();
            FileKit.Createfile(path, name + ".cs", s);
        }
    }
}