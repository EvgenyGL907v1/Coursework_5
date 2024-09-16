using System.Collections.Generic;

namespace DatabaseManager
{
	public interface ICargoData
	{
		void AddCargo(string name);
		void EditCargo(int id, string name);
		void DeleteCargo(int id);
		void CargoUniqueCheck(string name);
		List<(int, string)> GetCargoList();
	}
}
