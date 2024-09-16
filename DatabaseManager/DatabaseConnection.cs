using System;
using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace DatabaseManager
{
	/// <summary> Доступ к базе данных </summary>
	public class DatabaseConnection
	{
		#region SINGLETON

		private static DatabaseConnection _instance;
		public static DatabaseConnection Instance
		{
			get
			{
				_instance = _instance ?? (_instance = new DatabaseConnection());
				return _instance;
			}
		}

		#endregion

		/// <summary> Постоянное соединение </summary>
		private SQLiteConnection _connection;

		public SQLiteConnection Connection => _connection;

		private DatabaseConnection()
		{
			string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
			_connection = new SQLiteConnection(connectionString);
		}

		/// <summary> Выполнение запроса в базу данных </summary>
		public DataTable GetRequest(string requestString)
		{
			if (_connection.State != ConnectionState.Open)
				_connection.Open();

			if (_connection.State != ConnectionState.Open)
				throw new Exception("Не удается выполнить подключение");

			SQLiteDataAdapter adapter = new SQLiteDataAdapter(requestString, _connection);
			DataTable dataTable = new DataTable();
			adapter.Fill(dataTable);

			return dataTable;
		}

		public void ExecuteCommand(string command)
		{
			if (_connection.State != ConnectionState.Open)
				_connection.Open();

			if (_connection.State != ConnectionState.Open)
				throw new Exception("Не удается выполнить подключение");

			SQLiteCommand SqlCommand = new SQLiteCommand(command, _connection);
			SqlCommand.ExecuteNonQuery();
		}

		public void CloseConnect() => _connection.Close();
	}
}
