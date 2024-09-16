using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager
{
	public struct ClientData
	{ 
		public int ClientId { get; set; }
		public string Name { get; set; }
		public string TaxNumber { get; set; }
		public int BankId { get; set; }
		public string BankName { get; set;}
	}

	public interface IClientData
	{
		void AddClient(ClientData clientData);
		void EditClient(ClientData clientData);
		void DeleteClient(int clientId);

		List<ClientData> GetClientsList();
	}
}
