using System.Collections.Generic;

namespace DatabaseManager
{
	public interface IUnitData
	{
		void AddUnit(string name);
		void RemoveUnit(int id);
		void EditUnit(int id, string name);
		void UnitUniqueCheck(string name);
		List<(int, string)> GetUnitsList();
	}
}
