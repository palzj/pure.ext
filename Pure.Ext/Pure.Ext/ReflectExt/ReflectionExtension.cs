using System;
using System.Collections.Generic;
using System.Reflection;

public static class ReflectionExtension
{
    /// <summary>
    /// 获取所有基类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IEnumerable<Type> GetBaseTypes(this Type type)
    {
        Type baseType = type.BaseType;
        while (true)
        {
            if (baseType == null)
            {
                yield break;
            }
            yield return baseType;
            baseType = baseType.BaseType;
        }
    }

    /// <summary>
    /// 获取自定义属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="provider"></param>
    /// <param name="inherit"></param>
    /// <returns></returns>
    public static IEnumerable<T> GetCustomAttributes<T>(this ICustomAttributeProvider provider, bool inherit = false) where T : Attribute
    {
        return provider.GetCustomAttributes(typeof(T), inherit).Cast<T>();
    }

    /// <summary>
    /// 获取默认值
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static object GetDefaultValue(this Type type)
    {
        Guard.ArgumentNull(type, "type", null);
        if (!(!type.IsValueType || type.IsNullableType()))
        {
            return Activator.CreateInstance(type);
        }
        return null;
    }

    /// <summary>
    /// 获取直接接口类型
    /// </summary>
    /// <param name="type"></param>
    /// <param name="interfaceType"></param>
    /// <returns></returns>
    public static Type GetDirectImplementInterface(this Type type, Type interfaceType)
    {
        Guard.ArgumentNull(type, "type", null);
        Guard.ArgumentNull(interfaceType, "interfaceType", null);
        Guard.Argument(interfaceType.IsInterface, "interfaceType", Guard.GetString("NotInterfaceType", new object[] { interfaceType.FullName }));
        foreach (Type type2 in type.GetInterfaces())
        {
            if (interfaceType.IsAssignableFrom(type2))
            {
                return type2;
            }
        }
        return null;
    }

    /// <summary>
    /// 获取IEnumerable类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Type GetEnumerableElementType(this Type type)
    {
        if (!type.IsGenericType)
        {
            return type.GetElementType();
        }
        Type enumerableType = type.GetEnumerableType();
        if (enumerableType != null)
        {
            return enumerableType.GetGenericArguments()[0];
        }
        return null;
    }

    /// <summary>
    /// 获取IEnumerable类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Type GetEnumerableType(this Type type)
    {
        if ((type != null) && (type != typeof(string)))
        {
            if (type.IsArray)
            {
                return typeof(IEnumerable<>).MakeGenericType(new Type[] { type.GetElementType() });
            }
            if (type.IsGenericType)
            {
                foreach (Type type2 in type.GetGenericArguments())
                {
                    Type type3 = typeof(IEnumerable<>).MakeGenericType(new Type[] { type2 });
                    if (type3.IsAssignableFrom(type))
                    {
                        return type3;
                    }
                }
            }
            Type[] interfaces = type.GetInterfaces();
            if (interfaces.Length > 0)
            {
                foreach (Type type4 in interfaces)
                {
                    Type enumerableType = type4.GetEnumerableType();
                    if (enumerableType != null)
                    {
                        return enumerableType;
                    }
                }
            }
            if ((type.BaseType != null) && (type.BaseType != typeof(object)))
            {
                return type.BaseType.GetEnumerableType();
            }
        }
        return null;
    }

    /// <summary>
    /// 获取普通接口类型
    /// </summary>
    /// <param name="type"></param>
    /// <param name="genericDefinitionType"></param>
    /// <returns></returns>
    public static Type GetGenericImplementType(this Type type, Type genericDefinitionType)
    {
        foreach (Type type2 in type.GetHierarchyTypes())
        {
            if (type2.IsGenericType && (type2.GetGenericTypeDefinition() == genericDefinitionType))
            {
                return type2;
            }
        }
        return null;
    }

    /// <summary>
    /// 获取所有层次结果类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Type[] GetHierarchyTypes(this Type type)
    {
        List<Type> list = new List<Type> {
                type
            };
        list.AddRange(type.GetBaseTypes());
        list.AddRange(type.GetInterfaces());
        return list.ToArray();
    }

    /// <summary>
    /// 获取实现接口类型
    /// </summary>
    /// <param name="type"></param>
    /// <param name="interfaceType"></param>
    /// <returns></returns>
    public static Type GetImplementType(this Type type, Type interfaceType)
    {
        Guard.ArgumentNull(type, "type", null);
        Guard.ArgumentNull(interfaceType, "interfaceType", null);
        for (Type type2 = type.BaseType; type2 != typeof(object); type2 = type2.BaseType)
        {
            if (type2.IsImplementInterface(interfaceType))
            {
                return type2;
            }
        }
        return null;
    }

    /// <summary>
    /// 获取成员类型
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    public static Type GetMemberType(this MemberInfo member)
    {
        switch (member.MemberType)
        {
            case MemberTypes.Event:
                return (member as EventInfo).EventHandlerType;

            case MemberTypes.Field:
                return (member as FieldInfo).FieldType;

            case MemberTypes.Method:
                return (member as MethodInfo).ReturnType;

            case MemberTypes.Property:
                return (member as PropertyInfo).PropertyType;
        }
        return null;
    }

    /// <summary>
    /// 获取成员值
    /// </summary>
    /// <param name="member"></param>
    /// <param name="instance"></param>
    /// <returns></returns>
    public static object GetMemberValue(this MemberInfo member, object instance)
    {
        MemberTypes memberType = member.MemberType;
        if (memberType != MemberTypes.Field)
        {
            if (memberType == MemberTypes.Property)
            {
                return (member as PropertyInfo).GetValue(instance, null);
            }
            return null;
        }
        return (member as FieldInfo).GetValue(instance);
    }

    /// <summary>
    /// 获取当前非空类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Type GetNonNullableType(this Type type)
    {
        if (type.IsNullableType())
        {
            return type.GetGenericArguments()[0];
        }
        return type;
    }

    /// <summary>
    /// 获取当前可空类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Type GetNullableType(this Type type)
    {
        if (type.IsValueType && !type.IsNullableType())
        {
            return typeof(Nullable<>).MakeGenericType(new Type[] { type });
        }
        return type;
    }

    public static bool IsAnonymousType(this Type type)
    {
        Guard.ArgumentNull(type, "type", null);
        string fullName = type.FullName;
        return ((fullName.Length > 0x12) && (fullName.Substring(0, 0x12) == "<>f__AnonymousType"));
    }

    /// <summary>
    /// 是否是具体类型（((!type.IsAbstract && !type.IsGenericTypeDefinition) && !type.IsArray)）
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsConcreteType(this Type type)
    {
        return ((!type.IsAbstract && !type.IsGenericTypeDefinition) && !type.IsArray);
    }

    /// <summary>
    /// 是否定义
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="provider"></param>
    /// <param name="inherit"></param>
    /// <returns></returns>
    public static bool IsDefined<T>(this ICustomAttributeProvider provider, bool inherit = false) where T : Attribute
    {
        return provider.IsDefined(typeof(T), inherit);
    }

    /// <summary>
    /// 是否直接实现于某个接口类型
    /// </summary>
    /// <param name="type"></param>
    /// <param name="interfaceType"></param>
    /// <returns></returns>
    public static bool IsDirectImplementInterface(this Type type, Type interfaceType)
    {
        Guard.ArgumentNull(type, "type", null);
        Guard.ArgumentNull(interfaceType, "interfaceType", null);
        Guard.Argument(interfaceType.IsInterface, "interfaceType", Guard.GetString("NotInterfaceType", new object[] { interfaceType.FullName }));
        foreach (Type type2 in type.GetInterfaces())
        {
            if (!(type2 != interfaceType))
            {
                InterfaceMapping interfaceMap = type.GetInterfaceMap(type2);
                if ((interfaceMap.TargetMethods.Length > 0) && (interfaceMap.TargetMethods[0].DeclaringType == type))
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 是否实现于某个接口类型
    /// </summary>
    /// <param name="type"></param>
    /// <param name="interfaceType"></param>
    /// <returns></returns>
    public static bool IsImplementInterface(this Type type, Type interfaceType)
    {
        Guard.ArgumentNull(type, "type", null);
        Guard.ArgumentNull(interfaceType, "interfaceType", null);
        Guard.Argument(interfaceType.IsInterface, "interfaceType", Guard.GetString("NotInterfaceType", new object[] { interfaceType.FullName }));
        return interfaceType.IsAssignableFrom(type);
    }

    /// <summary>
    /// 是否是可空类型（Nullable）
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsNullableType(this Type type)
    {
        Guard.ArgumentNull(type, "type", null);
        return (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)));
    }

    /// <summary>
    /// 是否是数字类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsNumericType(this Type type)
    {
        type = type.GetNonNullableType();
        if (!type.IsEnum)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 是否是常用有效结构的属性类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsValidStructuralPropertyType(this Type type)
    {
        return ((!type.IsGenericTypeDefinition && !type.IsPointer) && !(type == typeof(object)));
    }

    /// <summary>
    /// 是否是常用有效结构的类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsValidStructuralType(this Type type)
    {
        return ((((!type.IsGenericType && !type.IsValueType) && (!type.IsPrimitive && !type.IsInterface)) && (!type.IsArray && !(type == typeof(string)))) && type.IsValidStructuralPropertyType());
    }

    /// <summary>
    /// 根据字符串解析为类型
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public static Type ParseType(this string typeName)
    {
        Type type = Type.GetType(typeName, false, true);
        if (type != null)
        {
            return type;
        }
        if (typeName.IndexOf("Version") == -1)
        {
            return null;
        }
        string str = typeof(string).Assembly.ToString();
        int index = str.IndexOf("Version");
        typeName = typeName + "," + str.Substring(index);
        return Type.GetType(typeName, false, true);
    }
}