using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace DatabaseManager
{
	public class RegistersDataManager : DataManager, IRoutersData, IClientData, IVesselsData, IConsignmentsData
	{

		#region ROUTES

		private void SetRoutVessel(int routId, VesselData vesselDat)
		{
			string command = $"UPDATE Routes SET VesselId = {vesselDat.VesselId} WHERE RouteId = {routId}";
			Database.ExecuteCommand(command);
		}

		public void AddRout(string name, List<int> ports, VesselData vesselData)
		{
			RoutUniqueCheck(name);

			if (ports.GroupBy(x => x).Any(g => g.Count() > 1))
				throw new Exception("Порты в маршруте не могу повторяться");

			string command = $"INSERT INTO Routes (Name) VALUES (\"{name}\");";
			Database.ExecuteCommand(command);

			string request = $"SELECT * FROM Routes WHERE Name = (\"{name}\");";

			var result = Database.GetRequest(request);
			TableDataManager tableData = new TableDataManager(result);
			int routId = tableData.GetValue<int>("RouteId");

			foreach (int portId in ports)
			{
				string addCommand = $"INSERT INTO RoutersPorts (RouteId, PortId) VALUES ({routId}, {portId})";
				Database.ExecuteCommand(addCommand);
			}

			if (vesselData.VesselId != 0)
				SetRoutVessel(routId, vesselData);
		}


		public void EditRout(string name, int routId, List<int> ports, VesselData vesselData)
		{
			RoutUniqueCheck(name, routId);

			if (ports.GroupBy(x => x).Any(g => g.Count() > 1))
				throw new Exception("Порты в маршруте не могу повторяться");

			RemoveRout(routId);
			string commandAdd = $"INSERT INTO Routes (RouteId, Name) VALUES ({routId},\"{name}\");";
			Database.ExecuteCommand(commandAdd);

			foreach (int portId in ports)
			{
				string command = $"INSERT INTO RoutersPorts (RouteId, PortId) VALUES ({routId}, {portId})";
				Database.ExecuteCommand(command);
			}

			if (vesselData.VesselId != 0)
				SetRoutVessel(routId, vesselData);
		}

		public List<(int, string)> GetRoutPorts(int routId)
		{
			string request = $"SELECT * FROM RoutersPorts rout JOIN Seaports ports ON ports.PortId = rout.PortId WHERE rout.RouteId = {routId}";
			var result = Database.GetRequest(request);

			List<(int, string)> ports = new List<(int, string)>(0);

			TableDataManager tableData = new TableDataManager(result);

			for (int i = 0; i < result.Rows.Count; i++)
			{
				tableData.SetRow(i);
				ports.Add((tableData.GetValue<int>("PortId"), tableData.GetValue<string>("Name")));
			}

			return ports;
		}

		public List<(int, string)> GetRoutsList()
		{
			string request = $"SELECT * FROM Routes";
			var result = Database.GetRequest(request);

			List<(int, string)> routs = new List<(int, string)>(0);

			TableDataManager dataTable = new TableDataManager(result);

			for (int i = 0; i < result.Rows.Count; i++)
			{
				dataTable.SetRow(i);
				routs.Add((dataTable.GetValue<int>("RouteId"), dataTable.GetValue<string>("Name")));
			}

			return routs;
		}

		public void RemoveRout(int routId)
		{
			string DPcommand = $"DELETE FROM RoutersPorts WHERE RouteId = {routId};";
			Database.ExecuteCommand(DPcommand);

			string command = $"DELETE FROM Routes WHERE RouteId = {routId};";
			Database.ExecuteCommand(command);
		}

		public void RoutUniqueCheck(string name, int routId = 0)
		{
			string request = $"SELECT * FROM Routes WHERE Name = (\"{name}\") {(routId != 0 ? ($"AND RouteId != {routId}") : (""))};";
			var result = Database.GetRequest(request);

			if (result.Rows.Count > 0)
				throw new Exception("Маршрут с таким названием уже существует");
		}

		public List<(int, string, int, string)> GetVesselsAndRout()
		{
			string request = $"SELECT r.RouteId,r.Name, COALESCE(v.VesselId, 0) AS VesselId,COALESCE(v.Name, \"-\") AS VesselName FROM Routes r LEFT JOIN Vessels v ON r.VesselId = v.VesselId;";
			var result = Database.GetRequest(request);

			TableDataManager dataTable = new TableDataManager(result);
			List<(int, string, int, string)> list = new List<(int, string, int, string)>(0);

			for (int i = 0; i < result.Rows.Count; i++)
			{
				dataTable.SetRow(i);
				list.Add((dataTable.GetValue<int>("RouteId"), dataTable.GetValue<string>("Name"), dataTable.GetValue<int>("VesselId") ,dataTable.GetValue<string>("VesselName")));
			}

			return list;
		}

		#endregion

		#region CLIENTS

		public void AddClient(ClientData clientData)
		{
			string command = $"INSERT INTO Clients (Name, TaxNumber, BankId) VALUES ('{clientData.Name}', '{clientData.TaxNumber}', {clientData.BankId});";
			Database.ExecuteCommand(command);
		}

		public void DeleteClient(int clientId)
		{
			string command = $"DELETE FROM Clients WHERE ClientId = {clientId};";
			Database.ExecuteCommand(command);
		}

        public void DeleteVessel(int vesselId)
		{
            string command = $"DELETE FROM Vessels WHERE VesselId = {vesselId};";
            Database.ExecuteCommand(command);
        }

        public void EditClient(ClientData clientData)
		{
			string command = $"UPDATE Clients SET Name = '{clientData.Name}', TaxNumber = '{clientData.TaxNumber}', BankId = {clientData.BankId}  WHERE ClientId = {clientData.ClientId};";
			Database.ExecuteCommand(command);
		}

		public List<ClientData> GetClientsList()
		{
			List<ClientData> list = new List<ClientData>(0);

			string request = $"SELECT c.ClientId, c.Name, c.TaxNumber, b.BankId, b.Name AS BankName FROM Clients c JOIN Banks b ON c.BankId = b.BankId";
			var result = Database.GetRequest(request);

			TableDataManager tableData = new TableDataManager(result);
			for (int i = 0; i < result.Rows.Count; i++)
			{
				tableData.SetRow(i);

				list.Add(new ClientData()
				{
					ClientId = tableData.GetValue<int>("ClientId"),
					Name = tableData.GetValue<string>("Name"),
					TaxNumber = tableData.GetValue<string>("TaxNumber"),
					BankId = tableData.GetValue<int>("BankId"),
					BankName = tableData.GetValue<string>("BankName")
				}); 
			}

			return list;
		}

        #endregion

        #region VESSELS

        public void Add(VesselData vesselData)
        {
            // Убедимся, что поле Image не null, а является пустым массивом, если изображения нет
            vesselData.Image = vesselData.Image ?? new byte[0];

            // Используем параметризованный запрос для вставки данных в таблицу Vessels
            string query = "INSERT INTO Vessels (RegNumber, Name, TypeId, ClassId, CreateDate, CaptainName, VesselImage) " +
                           "VALUES (@regNumber, @name, @typeId, @classId, @createDate, @captainName, @image);";

            using (var command = new SQLiteCommand(query, Database.Connection))
            {
                command.Parameters.AddWithValue("@regNumber", vesselData.RegNumber);
                command.Parameters.AddWithValue("@name", vesselData.Name);
                command.Parameters.AddWithValue("@typeId", vesselData.TypeId);
                command.Parameters.AddWithValue("@classId", vesselData.ClassId);
                command.Parameters.AddWithValue("@createDate", vesselData.CreateDate);
                command.Parameters.AddWithValue("@captainName", vesselData.CaptainName);

                // Если изображение есть, вставляем его, иначе передаём NULL
                if (vesselData.Image.Length > 0)
                {
                    command.Parameters.Add("@image", System.Data.DbType.Binary).Value = vesselData.Image;
                }
                else
                {
                    command.Parameters.AddWithValue("@image", DBNull.Value);
                }

                command.ExecuteNonQuery();
            }
        }

        public void Edit(VesselData vesselData)
        {
            // Убедимся, что поле Image не null, а является пустым массивом, если изображения нет
            vesselData.Image = vesselData.Image ?? new byte[0];

            // Параметризованный запрос для обновления данных
            string query = "UPDATE Vessels SET RegNumber = @regNumber, Name = @name, TypeId = @typeId, ClassId = @classId, " +
                           "CreateDate = @createDate, CaptainName = @captainName WHERE VesselId = @vesselId;";

            using (var command = new SQLiteCommand(query, Database.Connection))
            {
                command.Parameters.AddWithValue("@regNumber", vesselData.RegNumber);
                command.Parameters.AddWithValue("@name", vesselData.Name);
                command.Parameters.AddWithValue("@typeId", vesselData.TypeId);
                command.Parameters.AddWithValue("@classId", vesselData.ClassId);
                command.Parameters.AddWithValue("@createDate", vesselData.CreateDate);
                command.Parameters.AddWithValue("@captainName", vesselData.CaptainName);
                command.Parameters.AddWithValue("@vesselId", vesselData.VesselId);

                command.ExecuteNonQuery();
            }

            // Обновляем изображение, если оно присутствует
            if (vesselData.Image != null && vesselData.Image.Length > 0)
            {
                string queryImage = "UPDATE Vessels SET VesselImage = @image WHERE VesselId = @vesselId;";

                using (var commandImage = new SQLiteCommand(queryImage, Database.Connection))
                {
                    commandImage.Parameters.Add("@image", System.Data.DbType.Binary).Value = vesselData.Image;
                    commandImage.Parameters.AddWithValue("@vesselId", vesselData.VesselId);

                    commandImage.ExecuteNonQuery();
                }
            }
        }

        public void Delete(VesselData vesselData)
		{
			string command = $"DELETE FROM Vessels WHERE VesselId = {vesselData.VesselId};";
			Database.ExecuteCommand(command);
		}

		public List<VesselData> GetVesselsList()
		{
			string request = $"SELECT v.VesselId,v.RegNumber,v.Name,v.TypeId,v.ClassId, v.CreateDate, v.CaptainName,c.Name AS ClassName,t.Name AS TypeName,v.VesselImage  FROM Vessels v JOIN VesselTypes t ON t.TypeId = v.TypeId JOIN VesselClasses c ON c.ClassId = v.ClassId";
			var result = Database.GetRequest(request);

			TableDataManager tableData = new TableDataManager(result);
			List<VesselData> list = new List<VesselData>(0);

			for (int i = 0; i < result.Rows.Count; i++)
			{
				tableData.SetRow(i);

                list.Add(new VesselData()
				{
					VesselId = tableData.GetValue<int>("VesselId"),
					RegNumber = tableData.GetValue<string>("RegNumber"),
					Name = tableData.GetValue<string>("Name"),
					TypeId = tableData.GetValue<int>("TypeId"),
					TypeName = tableData.GetValue<string>("TypeName"),
					ClassId = tableData.GetValue<int>("ClassId"),
					ClassName = tableData.GetValue<string>("ClassName"),
					CreateDate = tableData.GetValue<string>("CreateDate"),
					CaptainName = tableData.GetValue<string>("CaptainName"),

					Image = tableData.GetValue<byte[]>("VesselImage")
				});
			}

			return list;
		}

		#endregion

		#region CONSIGNMENTS

		public void AddCargoConsignment(CargoConsignment data, List<CargoInfo> cargoInfo)
		{
			string command = $"INSERT INTO Consignments (SenderId, RecipientId, CustomNumber, DepartDate, ArrivalDate, SendPortId, ReceivPortId)" +
				$" VALUES ({data.SendClientId}, {data.ReceivClientId}, '{data.CustomNumber}', '{data.DepartDate}', '{data.ArrivalDate}', {data.SendPortId}, {data.ReceivPortId});";

			Database.ExecuteCommand(command);

			string dataId = $"SELECT * FROM Consignments WHERE SenderId = {data.SendClientId} AND RecipientId = {data.ReceivClientId} AND CustomNumber = '{data.CustomNumber}'";
			var id = int.Parse(Database.GetRequest(dataId).Rows[0]["ConsignmentId"].ToString());

			foreach (var cargo in cargoInfo)
			{
				string cargoCommand = $"INSERT INTO ConsignmentCargo (ConsignmentId, CargoId, Count, UnitId, InsuredCount) " +
					$"VALUES ({id}, {cargo.CargoId}, {cargo.Count}, {cargo.UnitId}, {cargo.InsuredCount});";
				Database.ExecuteCommand(cargoCommand);
			}
		}

		public void EditCargoConsignment(CargoConsignment data, List<CargoInfo> cargoInfo)
		{
			Database.ExecuteCommand($"DELETE FROM ConsignmentCargo WHERE ConsignmentId = {data.ConsignmentId};");
			//Database.ExecuteCommand($"update Consignments SET SenderId = {data.SendClientId}, RecipientId = {data.ReceivClientId}, CustomNumber = '{data.CustomNumber}', DepartDate = '{data.DepartDate}', ArrivalDate = '{data.ArrivalDate}', SendPortId = {data.SendPortId}, ReceivPortId = {data.ReceivPortId} WHERE {data.ConsignmentId}");
			Database.ExecuteCommand($"UPDATE Consignments SET SenderId = {data.SendClientId}, RecipientId = {data.ReceivClientId}, CustomNumber = '{data.CustomNumber}', DepartDate = '{data.DepartDate}', ArrivalDate = '{data.ArrivalDate}', SendPortId = {data.SendPortId}, ReceivPortId = {data.ReceivPortId} WHERE ConsignmentId = {data.ConsignmentId}");

			var id = data.ConsignmentId;

			foreach (var cargo in cargoInfo)
			{
				string cargoCommand = $"INSERT INTO ConsignmentCargo (ConsignmentId, CargoId, Count, UnitId, InsuredCount) " +
					$"VALUES ({id}, {cargo.CargoId}, {cargo.Count}, {cargo.UnitId}, {cargo.InsuredCount});";
				Database.ExecuteCommand(cargoCommand);
			}
		}


		public void DeleteCargoConsignment(int consignmentId)
		{
            Database.ExecuteCommand($"DELETE FROM ConsignmentCargo WHERE ConsignmentId = {consignmentId};");
            Database.ExecuteCommand($"DELETE FROM Consignments WHERE ConsignmentId = {consignmentId};");
		}

		public List<CargoConsignment> GetCargosList()
		{
			List<CargoConsignment> list = new List<CargoConsignment>(0);

			string request = $"SELECT c.ConsignmentId, c.SenderId, s.Name AS SenderName, c.RecipientId, r.Name AS RecipientName, c.CustomNumber, c.DepartDate, c.ArrivalDate, c.SendPortId, sp.Name AS SendPortName, c.ReceivPortId, rp.Name AS ReceivPortName \r\nFROM Consignments c JOIN Clients s ON c.SenderId = s.ClientId JOIN Clients r ON r.ClientId = c.RecipientId JOIN Seaports sp ON sp.PortId = c.SendPortId JOIN Seaports rp ON rp.PortId = c.ReceivPortId;";
			var result = Database.GetRequest(request);

			TableDataManager tableData = new TableDataManager(result);

			for (int i = 0; i < result.Rows.Count; i++)
			{
				tableData.SetRow(i);

				list.Add(new CargoConsignment()
				{
					ConsignmentId = tableData.GetValue<int>("ConsignmentId"),
					SendClientId = tableData.GetValue<int>("SenderId"),
					SendClientName = tableData.GetValue<string>("SenderName"),
					ReceivClientId = tableData.GetValue<int>("RecipientId"),
					ReceivClientName = tableData.GetValue<string>("RecipientName"),

					CustomNumber = tableData.GetValue<string>("CustomNumber"),
					DepartDate = tableData.GetValue<string>("DepartDate"),
					ArrivalDate = tableData.GetValue<string>("ArrivalDate"),

					SendPortId = tableData.GetValue<int>("SendPortId"),
					SendPortName = tableData.GetValue<string>("SendPortName"),
					ReceivPortId = tableData.GetValue<int>("ReceivPortId"),
					ReceivPortName = tableData.GetValue<string>("ReceivPortName"),

				});
			}

			return list;
		}

		public List<CargoInfo> GetCargoInfos(int consignmentId)
		{
			List<CargoInfo> cargos = new List<CargoInfo>();

			string request = $"SELECT c.CargoId, c.Name AS CargoName, u.UnitId,u.Name AS UnitName,cc.Count,cc.InsuredCount,cc.ConsignmentId " +
				$"FROM ConsignmentCargo cc JOIN Cargo c ON c.CargoId = cc.CargoId JOIN Units u ON u.UnitId = cc.UnitId {(consignmentId == 0 ? ("") : ($"WHERE cc.ConsignmentId = {consignmentId}"))} ;";
			var result = Database.GetRequest(request);

			TableDataManager tableData = new TableDataManager(result);

			for (int i = 0; i < result.Rows.Count; i++)
			{
				tableData.SetRow(i);

				cargos.Add(new CargoInfo()
				{ 
					CargoId = tableData.GetValue<int>("CargoId"),
					CargoName = tableData.GetValue<string>("CargoName"),

					UnitId = tableData.GetValue<int>("UnitId"),
					UnitName = tableData.GetValue<string>("UnitName"),

					Count = tableData.GetValue<int>("Count"),
					InsuredCount = tableData.GetValue<int>("InsuredCount"),

					ConsignmentId = tableData.GetValue<int>("ConsignmentId")
				});
			}

			return cargos;
		}



		#endregion
	}
}
