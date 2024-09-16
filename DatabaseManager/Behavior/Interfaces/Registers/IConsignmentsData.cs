using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManager
{
	public struct CargoConsignment
	{
		public int ConsignmentId { get; set; }

		public int SendClientId { get; set; }
		public string SendClientName { get; set; }

		public int ReceivClientId { get; set; }
		public string ReceivClientName { get; set; }

		public string DepartDate { get; set; }
		public string ArrivalDate { get; set; }


		public int SendPortId { get; set; }
		public string SendPortName { get; set; }

		public int ReceivPortId { get; set; }
		public string ReceivPortName { get;set; }

		public string CustomNumber { get; set; }
	}

	public struct CargoInfo
	{
		public int ConsignmentId { get; set; }

		public int CargoId { get; set; }	
		public string CargoName { get; set;}

		public int UnitId { get; set; }
		public string UnitName { get; set; }

		public int Count { get; set; }
		public int InsuredCount { get; set; }
	}

	public interface IConsignmentsData
	{
		void AddCargoConsignment(CargoConsignment data, List<CargoInfo> cargoInfo);
		void EditCargoConsignment(CargoConsignment data, List<CargoInfo> cargoInfo);
		void DeleteCargoConsignment(int consignmentId);

		List<CargoConsignment> GetCargosList();
		List<CargoInfo> GetCargoInfos(int consignmentId);
	}
}
