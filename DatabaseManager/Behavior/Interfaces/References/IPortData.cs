using System.Collections.Generic;

namespace DatabaseManager
{
	public interface IPortData
	{
		void AddPort(string name);
		void RemovePort(int id);
		void EditPort(int id, string name);
		void PortUniqueCheck(string name);
		List<(int, string)> GetPortsList();
	}
}
