using System.Collections.Generic;

namespace DatabaseManager
{
	public interface IRoutersData
	{
		void AddRout(string name, List<int> ports, VesselData vesselData);
		void EditRout(string name, int routId, List<int> ports, VesselData vesselData);
		void RemoveRout(int routId);
		List<(int, string)> GetRoutPorts(int routId);
		List<(int, string)> GetRoutsList();
		void RoutUniqueCheck(string name, int routId = 0);
		List<(int, string, int, string)> GetVesselsAndRout();
	}
}
