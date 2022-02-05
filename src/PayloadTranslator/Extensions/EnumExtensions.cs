using System;
using System.Linq;
using System.Reflection;

namespace PayloadTranslator.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum genericEnum)
    {
        Type genericEnumType = genericEnum.GetType();
        MemberInfo[] memberInfo = genericEnumType.GetMember(genericEnum.ToString());
        if (memberInfo != null && memberInfo.Length > 0)
        {
            var attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (attribs != null && attribs.Count() > 0)
            {
                return ((System.ComponentModel.DescriptionAttribute)attribs.ElementAt(0)).Description;
            }
        }

        return genericEnum.ToString();
    }

    public static (string producer, string primaryName, string possibleNames) GetDescriptions(this Enum genericEnum)
    {
        Type genericEnumType = genericEnum.GetType();
        MemberInfo[] memberInfo = genericEnumType.GetMember(genericEnum.ToString());
        if (memberInfo != null && memberInfo.Length > 0)
        {
            var attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (attribs != null && attribs.Count() > 0)
            {
                var description = ((System.ComponentModel.DescriptionAttribute)attribs.ElementAt(0)).Description;
                if (description != null)
                {
                    var descriptions = description.Split(';');
                    return (descriptions[0], descriptions[1], descriptions[2]);
                }
            }
        }

        return (null, null, null);
    }
}
