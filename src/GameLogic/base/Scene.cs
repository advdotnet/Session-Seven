using STACK;
using STACK.Logging;
using System;
using System.Linq;
using System.Reflection;

namespace SessionSeven
{

    public static class SceneExtension
    {
        /// <summary>
        /// Adds all entities from the same namespace using reflection.
        /// </summary>
        /// <param name="scene">scene to add entities for</param>
        public static void AutoAddEntities(this Scene scene)
        {
            string @namespace = scene.GetType().Namespace;

            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && typeof(Entity).IsAssignableFrom(t) && t.Namespace == @namespace && !t.IsAbstract
                    select t;

            foreach (var EntityType in q)
            {
                var Constructor = EntityType.GetConstructor(Type.EmptyTypes);
                if (null != Constructor)
                {
                    var Instance = (Entity)Activator.CreateInstance(EntityType);
                    scene.Push(Instance);
                    Log.WriteLine("Adding " + EntityType.Name + " to Scene " + @namespace);
                }
                else
                {
                    Log.WriteLine("Skipping adding " + EntityType.Name + " to Scene " + @namespace + " due to missing parameterless constructor.");
                }
            }
        }
    }
}
