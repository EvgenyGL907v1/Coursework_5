using System.Collections.Generic;

namespace DatabaseManager
{
	public interface IBankData
	{
		void AddBank(string name);
		void RemoveBank(int id);
		void EditBank(int id, string name);
		void BankUniqueCheck(string name);
		List<(int, string)> GetBanksList();
	}
}
