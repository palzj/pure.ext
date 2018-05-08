
/// <summary>
/// ValueTypeExtensions
/// </summary>
public static class ValueTypeExtensions
{
    /// <summary>
    ///  是否为默认值
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "value">The value.</param>
    /// <returns>
    /// 	<c>true</c> if the specified value is empty; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsEmpty<T>(this T value) where T : struct
    {
        return value.Equals(default(T));
    }

    /// <summary>
    /// 是否不为默认值
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "value">The value.</param>
    /// <returns>
    /// 	<c>true</c> if the specified value is not empty; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNotEmpty<T>(this T value) where T : struct
    {
        return (value.IsEmpty() == false);
    }

    /// <summary>
    /// 转换为可空类型数据
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "value">The value.</param>
    /// <returns>The nullable type</returns>
    public static T? ToNullable<T>(this T value) where T : struct
    {
        return (value.IsEmpty() ? null : (T?)value);
    }

}
