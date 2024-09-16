using System;
using System.Data;

namespace DatabaseManager
{
	public class TableDataManager
	{
		private DataTable _table;
		private int _rowNumber;

		public TableDataManager(DataTable table)
		{
			_table = table;
			_rowNumber = 0;
		}

		public void SetRow(int rowNum)
		{ 
			_rowNumber = rowNum;
		}

		public T GetValue<T>(string key)
		{
			var value = _table.Rows[_rowNumber][key];

			if (value == DBNull.Value || value == null)
				return default;

			return (T)Convert.ChangeType(value, typeof(T));
		}
	}
}
