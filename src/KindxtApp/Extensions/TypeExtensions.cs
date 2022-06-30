using System.Reflection;

namespace Kindxt.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetAllTypes<T>()
        {
            return System.Reflection.Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface);
        }
    }
}
