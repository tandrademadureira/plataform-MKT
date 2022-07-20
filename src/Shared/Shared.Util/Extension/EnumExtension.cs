using System;
using System.ComponentModel;
using System.Linq;

namespace Shared.Util.Extension
{
    /// <summary>
    /// Extension for enums
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Get the enum description. To get the description, the enum must be decorated with the description attribute.
        /// <code>
        /// [System.ComponentModel.Description("Your Description")]
        /// </code>
        /// </summary>
        /// <param name="value">Enum object type of ***enum***</param>
        /// <returns>String with the description decorated in the enum.</returns>
        /// <example>
        /// Foo enum used in this example.
        /// <code>
        /// public enum FooEnum
        /// {
        ///     [Description("Bar Description")]
        ///     Bar = 1
        /// }
        /// </code>
        /// Example to get the description.
        /// <code>
        /// public class EnumDescriptionSample
        /// {
        ///     public void GetBarDescription()
        ///     {
        ///         var bar = GetDescriptionOfFooEnum(FooEnum.Bar);
        ///     }
        /// 
        ///     public string GetDescriptionOfBar(FooEnum fooEnum) => fooEnum.GetDescription();
        /// }
        /// </code>
        /// bar value is "Bar Description".
        /// </example>
        public static string GetDescription(this Enum value) => value.GetAttribute<DescriptionAttribute>()?.Description;

        /// <summary>
        /// Get the enum name.
        /// </summary>
        /// <typeparam name="TEnum">Enum type provider.</typeparam>
        /// <param name="value">Enum object type of ***enum***</param>
        /// <returns>String with name of the enum.</returns>
        /// <example>
        /// Foo enum used in this example.
        /// <code>
        /// public enum FooEnum
        /// {
        ///     [Description("Bar Description")]
        ///     Bar = 1
        /// }
        /// </code>
        /// Example to get the name.
        /// <code>
        /// public class EnumNameSample
        /// {
        ///     public void GetName()
        ///     {
        ///         var fooEnum = FooEnum.Bar;
        ///         var name = fooEnum.GetName<![CDATA[<FooEnum>]]>();
        ///     }
        /// }
        /// </code>
        /// name value is "Bar".
        /// </example>
        /// <exception cref="ArgumentException">If typeof ***TEnum*** is different of Enum struct, an exception is thrown with message <c>'Type provided must be an Enum.'</c>.</exception>
        public static string GetName<TEnum>(this Enum value) where TEnum : struct => Enum.GetName(typeof(TEnum), value);

        private static T GetAttribute<T>(this Enum enumType) where T : Attribute => enumType.GetType().GetField(enumType.ToString()).GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;
    }
}
