using System.Collections.Generic;

namespace DatabaseManager
{
	public interface IVesselClassData
	{
		void AddClass(string name);
		void RemoveClass(int id);
		void EditClass(int id, string name);
		void ClassUniqueCheck(string name);
		List<(int, string)> GetClassesList();
	}
}
