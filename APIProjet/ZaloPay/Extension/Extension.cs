﻿using System.Reflection;

namespace APIProject.ZaloPay.Extension
{
    public static class ObjectExtension
    {
        public static Dictionary<string, string> AsParams(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name.ToLower(),
                propInfo => propInfo.GetValue(source, null)?.ToString()
            );
        }
    }
}
