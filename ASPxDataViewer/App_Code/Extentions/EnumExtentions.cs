using System;
using System.ComponentModel;
using System.Reflection;

namespace Extentions
{
    /// <summary>
    /// Расширение Enum
    /// </summary>
    public static class EnumExtensions
    {

        /// <summary>
        /// Получить аттрибуты типа
        /// </summary>
        /// <typeparam name="T">тип</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            Type type = value.GetType();
            MemberInfo [] memberInfo = type.GetMember(value.ToString());
            object [] attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);

            if (attributes.Length > 0)
                return (T) attributes[0];
            
            return null;
        }


        /// <summary>
        /// Переводит Enum в строку с учетом аттрибута [Description] (если задан)
        /// </summary>
        /// <typeparam name="T">тип Enum</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToName<T>(this Enum value)
        {
            DescriptionAttribute attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? Enum.GetName(typeof(T), value) : attribute.Description;
        }
    }
}
