
using System.Reflection;

namespace Domain.User
{


    public abstract class UserStatus : Enumeration<UserStatus>
    {
        protected UserStatus(int key, string name) : base(key, name) { }
        public abstract bool CanExecuteAction();
    }
    public abstract class Enumeration<TEnum> where TEnum : Enumeration<TEnum>
    {
        public int Key { get; }
        public string Name { get; }
        public Enumeration(int key, string name)
        {
            Key = key;
            Name = name;
        }
        private static Dictionary<int, Func<TEnum>> _keyFactories = FetchKeyFactories();
        private static Dictionary<string, Func<TEnum>> _nameFactories = FetchNameFactories();

        public static TEnum FromKey(int key)
        {
            return _keyFactories.TryGetValue(key, out Func<TEnum>? factory)
                ? factory()
                : throw new ArgumentException("Не поддерживаемый ключ перечисления");
        }
        public static TEnum FromName(string name)
        {
            return _nameFactories.TryGetValue(name, out Func<TEnum>? factory)
                ? factory()
                : throw new ArgumentException("Не поддерживаемое название перечисления");
        }
        public static Dictionary<int, Func<TEnum>> FetchKeyFactories()
        {
            Type enumType = typeof(TEnum);
            IEnumerable<Type> childs = enumType
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(enumType) && !t.IsAbstract);
            Dictionary<int, Func<TEnum>> _factories = [];
            foreach (Type entry in childs)
            {
                ConstructorInfo constructor = entry
                    .GetConstructors()
                    .First(c => c.GetParameters().Length == 0);
                Func<TEnum> factory = () => (TEnum)constructor.Invoke(null);
                TEnum enumeration = factory();
                int key = enumeration.Key;
                _factories.Add(key, factory);
            }
            return _factories;
        }
        public static Dictionary<string, Func<TEnum>> FetchNameFactories()
        {
            IEnumerable<Type> childs = FetchTypes();
            Dictionary<string, Func<TEnum>> _factories = [];
            foreach (Type entry in childs)
            {
                Func<TEnum> factory = CreateFactoryFromConstructor(entry);
                TEnum enumeration = factory();
                string name = enumeration.Name;
                _factories.Add(name, factory);
            }
            return _factories;
        }
        private static IEnumerable<Type> FetchTypes()
        {
            Type enumType = typeof(TEnum);
            return enumType.Assembly.GetTypes().Where(t => t.IsSubclassOf(enumType)&& !t.IsAbstract);
        }
        private static Func<TEnum> CreateFactoryFromConstructor(Type type)
        {
            ConstructorInfo constructor = type.GetConstructors()
                .First(c => c.GetParameters().Length == 0);
            Func<TEnum> factory = () => (TEnum)constructor.Invoke(null);
            return factory;
        }
    }
}
