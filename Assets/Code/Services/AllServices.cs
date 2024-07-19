using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Services
{
    public static class AllServices
    {
        private static readonly Dictionary<Type, IService> _services = new Dictionary<Type, IService>();

        static AllServices()
        {
            RegistrateService(new SceneLoadService());
        }
        
        public static T Get<T>()
        {
            if (_services.TryGetValue(typeof(T), out var service))
                return (T)service;
            
            Debug.LogError($"AllServices not contains service {typeof(T)}");
            return default(T);
        }

        public static T RegistrateService<T>(T service) where T : IService
        {
            _services.Add(typeof(T), service);
            return service;
        }
    }
}
