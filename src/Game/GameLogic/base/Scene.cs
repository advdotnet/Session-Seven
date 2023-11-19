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
			var @namespace = scene.GetType().Namespace;

			var types = from t in Assembly.GetExecutingAssembly().GetTypes()
						where t.IsClass && typeof(Entity).IsAssignableFrom(t) && t.Namespace == @namespace && !t.IsAbstract
						select t;

			foreach (var entityType in types)
			{
				var constructor = entityType.GetConstructor(Type.EmptyTypes);
				if (null != constructor)
				{
					var instance = (Entity)Activator.CreateInstance(entityType);
					scene.Push(instance);
					Log.WriteLine("Adding " + entityType.Name + " to Scene " + @namespace);
				}
				else
				{
					Log.WriteLine("Skipping adding " + entityType.Name + " to Scene " + @namespace + " due to missing parameterless constructor.");
				}
			}
		}
	}
}
