using System.Collections.Generic;

namespace DatabaseManager
{
	public interface IVesselTypeData
	{
		void AddType(string name);
		void RemoveType(int id);
		void EditType(int id, string name);
		void TypeUniqueCheck(string name);
		List<(int, string)> GetTypesList();
	}
}
