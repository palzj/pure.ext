using System;
using System.Globalization;
using System.Reflection;

/// <summary>
/// 	Extension methods for the reflection meta data type "Type"
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// 	Creates and returns an instance of the desired type
    /// </summary>
    /// <param name = "type">The type to be instanciated.</param>
    /// <param name = "constructorParameters">Optional constructor parameters</param>
    /// <returns>The instanciated object</returns>
    /// <example>
    /// 	<code>
    /// 		var type = Type.GetType(".NET full qualified class Type")
    /// 		var instance = type.CreateInstance();
    /// 	</code>
    /// </example>
    public static object CreateInstance(this Type type, params object[] constructorParameters)
    {
        return CreateInstance<object>(type, constructorParameters);
    }

    /// <summary>
    /// 	Creates and returns an instance of the desired type casted to the generic parameter type T
    /// </summary>
    /// <typeparam name = "T">The data type the instance is casted to.</typeparam>
    /// <param name = "type">The type to be instanciated.</param>
    /// <param name = "constructorParameters">Optional constructor parameters</param>
    /// <returns>The instanciated object</returns>
    /// <example>
    /// 	<code>
    /// 		var type = Type.GetType(".NET full qualified class Type")
    /// 		var instance = type.CreateInstance&lt;IDataType&gt;();
    /// 	</code>
    /// </example>
    public static T CreateInstance<T>(this Type type, params object[] constructorParameters)
    {
        var instance = Activator.CreateInstance(type, constructorParameters);
        return (T)instance;
    }

    ///<summary>
    ///	Check if this is a base type
    ///</summary>
    ///<param name = "type"></param>
    ///<param name = "checkingType"></param>
    ///<returns></returns>
    /// <remarks>

    /// </remarks>
    public static bool IsBaseType(this Type type, Type checkingType)
    {
        while (type != typeof(object))
        {
            if (type == null)
                continue;

            if (type == checkingType)
                return true;

            type = type.BaseType;
        }
        return false;
    }

    ///<summary>
    ///	Check if this is a sub class generic type
    ///</summary>
    ///<param name = "generic"></param>
    ///<param name = "toCheck"></param>
    ///<returns></returns>
    /// <remarks>

    /// </remarks>
    public static bool IsSubclassOfRawGeneric(this Type generic, Type toCheck)
    {
        while (toCheck != typeof(object))
        {
            if (toCheck == null)
                continue;

            var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
            if (generic == cur)
                return true;
            toCheck = toCheck.BaseType;
        }
        return false;
    }

    /// <summary>
    /// Closes the passed generic type with the provided type arguments and returns an instance of the newly constructed type.
    /// </summary>
    /// <typeparam name="T">The typed type to be returned.</typeparam>
    /// <param name="genericType">The open generic type.</param>
    /// <param name="typeArguments">The type arguments to close the generic type.</param>
    /// <returns>An instance of the constructed type casted to T.</returns>
    public static T CreateGenericTypeInstance<T>(this Type genericType, params Type[] typeArguments) where T : class
    {
        var constructedType = genericType.MakeGenericType(typeArguments);
        var instance = Activator.CreateInstance(constructedType);
        return (instance as T);
    }

    #region Z.EXT

    /// <summary>
    ///     Creates an instance of the specified type using the constructor that best matches the specified parameters.
    /// </summary>
    /// <param name="type">The type of object to create.</param>
    /// <param name="bindingAttr">
    ///     A combination of zero or more bit flags that affect the search for the  constructor. If
    ///     is zero, a case-sensitive search for public constructors is conducted.
    /// </param>
    /// <param name="binder">
    ///     An object that uses  and  to seek and identify the  constructor. If  is null, the default
    ///     binder is used.
    /// </param>
    /// <param name="args">
    ///     An array of arguments that match in number, order, and type the parameters of the constructor
    ///     to invoke. If  is an empty array or null, the constructor that takes no parameters (the default constructor) is
    ///     invoked.
    /// </param>
    /// <param name="culture">
    ///     Culture-specific information that governs the coercion of  to the formal types declared for
    ///     the  constructor. If  is null, the  for the current thread is used.
    /// </param>
    /// <returns>A reference to the newly created object.</returns>
    public static Object CreateInstance(this Type type, BindingFlags bindingAttr, Binder binder, Object[] args, CultureInfo culture)
    {
        return Activator.CreateInstance(type, bindingAttr, binder, args, culture);
    }

    /// <summary>
    ///     Creates an instance of the specified type using the constructor that best matches the specified parameters.
    /// </summary>
    /// <param name="type">The type of object to create.</param>
    /// <param name="bindingAttr">
    ///     A combination of zero or more bit flags that affect the search for the  constructor. If
    ///     is zero, a case-sensitive search for public constructors is conducted.
    /// </param>
    /// <param name="binder">
    ///     An object that uses  and  to seek and identify the  constructor. If  is null, the default
    ///     binder is used.
    /// </param>
    /// <param name="args">
    ///     An array of arguments that match in number, order, and type the parameters of the constructor
    ///     to invoke. If  is an empty array or null, the constructor that takes no parameters (the default constructor) is
    ///     invoked.
    /// </param>
    /// <param name="culture">
    ///     Culture-specific information that governs the coercion of  to the formal types declared for
    ///     the  constructor. If  is null, the  for the current thread is used.
    /// </param>
    /// <param name="activationAttributes">
    ///     An array of one or more attributes that can participate in activation. This
    ///     is typically an array that contains a single  object. The  specifies the URL that is required to activate a
    ///     remote object.
    /// </param>
    /// <returns>A reference to the newly created object.</returns>
    public static Object CreateInstance(this Type type, BindingFlags bindingAttr, Binder binder, Object[] args, CultureInfo culture, Object[] activationAttributes)
    {
        return Activator.CreateInstance(type, bindingAttr, binder, args, culture, activationAttributes);
    }

    /// <summary>
    ///     Creates an instance of the specified type using the constructor that best matches the specified parameters.
    /// </summary>
    /// <param name="type">The type of object to create.</param>
    /// <param name="args">
    ///     An array of arguments that match in number, order, and type the parameters of the constructor
    ///     to invoke. If  is an empty array or null, the constructor that takes no parameters (the default constructor) is
    ///     invoked.
    /// </param>
    /// <param name="activationAttributes">
    ///     An array of one or more attributes that can participate in activation. This
    ///     is typically an array that contains a single  object. The  specifies the URL that is required to activate a
    ///     remote object.
    /// </param>
    /// <returns>A reference to the newly created object.</returns>
    public static Object CreateInstance(this Type type, Object[] args, Object[] activationAttributes)
    {
        return Activator.CreateInstance(type, args, activationAttributes);
    }

    /// <summary>
    ///     Creates an instance of the specified type using that type&#39;s default constructor.
    /// </summary>
    /// <param name="type">The type of object to create.</param>
    /// <returns>A reference to the newly created object.</returns>
    public static Object CreateInstance(this Type type)
    {
        return Activator.CreateInstance(type);
    }

    /// <summary>
    ///     Creates an instance of the specified type using that type&#39;s default constructor.
    /// </summary>
    /// <param name="type">The type of object to create.</param>
    /// <param name="nonPublic">
    ///     true if a public or nonpublic default constructor can match; false if only a public
    ///     default constructor can match.
    /// </param>
    /// <returns>A reference to the newly created object.</returns>
    public static Object CreateInstance(this Type type, Boolean nonPublic)
    {
        return Activator.CreateInstance(type, nonPublic);
    }

#if NET45
     /// <summary>
    ///     Creates a proxy for the well-known object indicated by the specified type and URL.
    /// </summary>
    /// <param name="type">The type of the well-known object to which you want to connect.</param>
    /// <param name="url">The URL of the well-known object.</param>
    /// <returns>A proxy that points to an endpoint served by the requested well-known object.</returns>
    public static Object GetObject(this Type type, String url)
        {
            return Activator.GetObject(type, url);
        }

        /// <summary>
        ///     Creates a proxy for the well-known object indicated by the specified type, URL, and channel data.
        /// </summary>
        /// <param name="type">The type of the well-known object to which you want to connect.</param>
        /// <param name="url">The URL of the well-known object.</param>
        /// <param name="state">Channel-specific data or null.</param>
        /// <returns>A proxy that points to an endpoint served by the requested well-known object.</returns>
        public static Object GetObject(this Type type, String url, Object state)
        {
            return Activator.GetObject(type, url, state);
        }
#endif

    /// <summary>
    ///     A Type extension method that creates an instance.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="bindingAttr">The binding attribute.</param>
    /// <param name="binder">The binder.</param>
    /// <param name="args">The arguments.</param>
    /// <param name="culture">The culture.</param>
    /// <returns>The new instance.</returns>
    public static T CreateInstance<T>(this Type @this, BindingFlags bindingAttr, Binder binder, Object[] args, CultureInfo culture)
    {
        return (T)Activator.CreateInstance(@this, bindingAttr, binder, args, culture);
    }

    /// <summary>
    ///     A Type extension method that creates an instance.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="bindingAttr">The binding attribute.</param>
    /// <param name="binder">The binder.</param>
    /// <param name="args">The arguments.</param>
    /// <param name="culture">The culture.</param>
    /// <param name="activationAttributes">The activation attributes.</param>
    /// <returns>The new instance.</returns>
    public static T CreateInstance<T>(this Type @this, BindingFlags bindingAttr, Binder binder, Object[] args, CultureInfo culture, Object[] activationAttributes)
    {
        return (T)Activator.CreateInstance(@this, bindingAttr, binder, args, culture, activationAttributes);
    }

    /// <summary>
    ///     A Type extension method that creates an instance.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="args">The arguments.</param>
    /// <param name="activationAttributes">The activation attributes.</param>
    /// <returns>The new instance.</returns>
    public static T CreateInstance<T>(this Type @this, Object[] args, Object[] activationAttributes)
    {
        return (T)Activator.CreateInstance(@this, args, activationAttributes);
    }

    /// <summary>
    ///     A Type extension method that creates an instance.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The new instance.</returns>
    public static T CreateInstance<T>(this Type @this)
    {
        return (T)Activator.CreateInstance(@this);
    }

    /// <summary>
    ///     A Type extension method that creates an instance.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="nonPublic">true to non public.</param>
    /// <returns>The new instance.</returns>
    public static T CreateInstance<T>(this Type @this, Boolean nonPublic)
    {
        return (T)Activator.CreateInstance(@this, nonPublic);
    }

    #endregion Z.EXT
}