using System;

namespace WebApplication1.Data.Extensions
{
    public static class ReflectionExtensions
    {
        public static bool IsAssignableToGenericType(this Type type, Type genericType)
        {
            var interfaces = type.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == genericType)
                    return true;
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
                return true;
            var baseType = type.BaseType;
            return baseType == null ? false : baseType.IsAssignableToGenericType(genericType);
        }
    }
}
