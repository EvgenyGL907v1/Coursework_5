using PresenterContext;
using System;
using System.Reflection;
using MenuInterface;
using DatabaseManager;

namespace ShippingCompanyManager
{
	public static class FileLoader
	{
		public static MenuBase LoadFormFromDLL(string dllName, string className, MenuAccess menuAccess)
		{
			string fullTypeName = dllName + "." + className;
			Assembly assembly = Assembly.LoadFrom(dllName + ".dll");

			Type type = assembly.GetType(fullTypeName);
			object instance = Activator.CreateInstance(type, menuAccess);

			if (instance is MenuBase presenter)
				return presenter;

			throw new Exception($"Ошибка загрузки файла: {dllName}.dll");
	
		}
	}
}
