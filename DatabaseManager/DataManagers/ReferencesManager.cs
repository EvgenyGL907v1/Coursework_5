using System;
using System.Collections.Generic;

namespace DatabaseManager
{
	public class ReferencesManager : DataManager, ICargoData, IUnitData, IPortData, IVesselTypeData, IVesselClassData, IBankData
	{
		#region CARGO

		public void AddCargo(string name)
		{
			CargoUniqueCheck(name);

			string command = $"INSERT INTO Cargo (Name) VALUES (\"{name}\");";
			Database.ExecuteCommand(command);
		}

		public void EditCargo(int cargoId, string name)
		{
			CargoUniqueCheck(name);

			string command = $"UPDATE Cargo SET Name = \"{name}\" WHERE CargoId = {cargoId};";
			Database.ExecuteCommand(command);
		}

		public void DeleteCargo(int cargoId)
		{
			string command = $"DELETE FROM Cargo WHERE CargoId = {cargoId};";
			Database.ExecuteCommand(command);
		}

		public void CargoUniqueCheck(string name)
		{
			string request = $"SELECT * FROM Cargo WHERE Name = (\"{name}\");";
			var result = Database.GetRequest(request);

			if (result.Rows.Count > 0)
				throw new Exception("Такое название груза уже существует");

		}

		public List<(int, string)> GetCargoList()
		{
			string request = $"SELECT * FROM Cargo";
			var result = Database.GetRequest(request);

			List<(int, string)> list = new List<(int, string)>(0);

			TableDataManager tableData = new TableDataManager(result);

			for (int i = 0; i < result.Rows.Count; i++)
			{
				tableData.SetRow(i);
				list.Add((tableData.GetValue<int>("CargoId"), tableData.GetValue<string>("Name")));
			}

			return list;
		}

		#endregion

		#region UNITS

		public void AddUnit(string name)
		{
			UnitUniqueCheck(name);

			string command = $"INSERT INTO Units (Name) VALUES (\"{name}\");";
			Database.ExecuteCommand(command);
		}

		public void EditUnit(int id, string name)
		{
			UnitUniqueCheck(name);

			string command = $"UPDATE Units SET Name = \"{name}\" WHERE UnitId = {id};";
            Database.ExecuteCommand(command);
		}

		public void RemoveUnit(int id)
		{
			string command = $"DELETE FROM Units WHERE UnitId = {id};";
			Database.ExecuteCommand(command);
		}

		public void UnitUniqueCheck(string name)
		{
			string request = $"SELECT * FROM Units units WHERE units.Name = (\"{name}\");";
			var result = Database.GetRequest(request);

			if (result.Rows.Count > 0)
				throw new Exception("Такое название ед. измерения уже существует");
		}

		public List<(int, string)> GetUnitsList()
		{
			string request = $"SELECT * FROM Units";
			var result = Database.GetRequest(request);

			List<(int, string)> list = new List<(int, string)>(0);
			TableDataManager tableData = new TableDataManager(result);

			for (int i = 0; i < result.Rows.Count; i++)
			{
				tableData.SetRow(i);
				list.Add((tableData.GetValue<int>("UnitId"), tableData.GetValue<string>("Name")));
			}

			return list;
		}

		#endregion

		#region PORTS

		public void AddPort(string name)
		{
			PortUniqueCheck(name);

			string command = $"INSERT INTO SeaPorts (Name) VALUES (\"{name}\");";
			Database.ExecuteCommand(command);
		}

		public void EditPort(int id, string name)
		{
			PortUniqueCheck(name);

			string command = $"UPDATE SeaPorts SET Name = \"{name}\" WHERE PortId = {id};";
			Database.ExecuteCommand(command);
		}

		public void RemovePort(int id)
		{
			string command = $"DELETE FROM SeaPorts WHERE PortId = {id};";
			Database.ExecuteCommand(command);
		}

		public void PortUniqueCheck(string name)
		{
			string request = $"SELECT * FROM SeaPorts WHERE Name = (\"{name}\");";
			var result = Database.GetRequest(request);

			if (result.Rows.Count > 0)
				throw new Exception("Порт с таким названием уже существует");
		}

		public List<(int, string)> GetPortsList()
		{
			string request = $"SELECT * FROM Seaports";
			var result = Database.GetRequest(request);

			List<(int, string)> list = new List<(int, string)>(0);

			TableDataManager tableData = new TableDataManager(result);

			for (int i = 0; i < result.Rows.Count; i++)
			{
				tableData.SetRow(i);
				list.Add((tableData.GetValue<int>("PortId"), tableData.GetValue<string>("Name")));
			}

			return list;
		}

		#endregion

		#region VESSEL_TYPE

		public void AddType(string name)
		{
			TypeUniqueCheck(name);

			string command = $"INSERT INTO VesselTypes (Name) VALUES (\"{name}\");";
			Database.ExecuteCommand(command);
		}

		public void RemoveType(int id)
		{
			string command = $"DELETE FROM VesselTypes WHERE TypeId = {id};";
			Database.ExecuteCommand(command);
		}

		public void EditType(int id, string name)
		{
			TypeUniqueCheck(name);

			string command = $"UPDATE VesselTypes SET Name = \"{name}\" WHERE TypeId = {id};";
			Database.ExecuteCommand(command);
		}

		public void TypeUniqueCheck(string name)
		{
			string request = $"SELECT * FROM VesselTypes WHERE Name = (\"{name}\");";
			var result = Database.GetRequest(request);

			if (result.Rows.Count > 0)
				throw new Exception("Тип судна с таким названием уже существует");
		}

		public List<(int, string)> GetTypesList()
		{
			string request = $"SELECT * FROM VesselTypes";
			var result = Database.GetRequest(request);

			List<(int, string)> list = new List<(int, string)>(0);
			TableDataManager tableData = new TableDataManager(result);

			for (int i = 0; i < result.Rows.Count; i++)
			{
				tableData.SetRow(i);
				list.Add((tableData.GetValue<int>("TypeId"), tableData.GetValue<string>("Name")));
			}

			return list;
		}

		#endregion

		#region VESSEL_CLASS

		public void AddClass(string name)
		{
			ClassUniqueCheck(name);

			string command = $"INSERT INTO VesselClasses (Name) VALUES (\"{name}\");";
			Database.ExecuteCommand(command);
		}

		public void RemoveClass(int id)
		{
			string command = $"DELETE FROM VesselClasses WHERE ClassId = {id};";
			Database.ExecuteCommand(command);
		}

		public void EditClass(int id, string name)
		{
			ClassUniqueCheck(name);

			string command = $"UPDATE VesselClasses SET Name = \"{name}\" WHERE ClassId = {id};";
			Database.ExecuteCommand(command);
		}

		public void ClassUniqueCheck(string name)
		{
			string request = $"SELECT * FROM VesselClasses WHERE Name = (\"{name}\");";
			var result = Database.GetRequest(request);

			if (result.Rows.Count > 0)
				throw new Exception("Класс судна с таким названием уже существует");
		}

		public List<(int, string)> GetClassesList()
		{
			string request = $"SELECT * FROM VesselClasses";
			var result = Database.GetRequest(request);

			List<(int, string)> list = new List<(int, string)>(0);
			TableDataManager tableData = new TableDataManager(result);

			for (int i = 0; i < result.Rows.Count; i++)
			{
				tableData.SetRow(i);
				list.Add((tableData.GetValue<int>("ClassId"), tableData.GetValue<string>("Name")));
			}

			return list;
		}

		#endregion

		#region BANKS

		public void AddBank(string name)
		{
			BankUniqueCheck(name);
			string command = $"INSERT INTO Banks (Name) VALUES (\"{name}\");";
			Database.ExecuteCommand(command);
		}

		public void RemoveBank(int id)
		{
			string command = $"DELETE FROM Banks WHERE BankId = {id};";
			Database.ExecuteCommand(command);
		}

		public void EditBank(int id, string name)
		{
			BankUniqueCheck(name);

			string command = $"UPDATE Banks SET Name = \"{name}\" WHERE BankId = {id};";
			Database.ExecuteCommand(command);
		}

		public void BankUniqueCheck(string name)
		{
			string request = $"SELECT * FROM Banks WHERE Name = (\"{name}\");";
			var result = Database.GetRequest(request);

			if (result.Rows.Count > 0)
				throw new Exception("Банк с таким названием уже существует");
		}

		public List<(int, string)> GetBanksList()
		{
			string request = $"SELECT * FROM Banks";
			var result = Database.GetRequest(request);

			List<(int, string)> list = new List<(int, string)>(0);
			TableDataManager tableData = new TableDataManager(result);

			for (int i = 0; i < result.Rows.Count; i++)
			{
				tableData.SetRow(i);
				list.Add((tableData.GetValue<int>("BankId"), tableData.GetValue<string>("Name")));
			}

			return list;
		}

		#endregion
	}
}
