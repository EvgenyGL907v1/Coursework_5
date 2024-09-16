using DatabaseManager;
using DataExport;
using PresenterContext;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CargoDocumentMenu
{
	public class CargoDocumentsListFormPresenter : PresenterBase
	{
		public CargoDocumentsListFormPresenter() : base(new ElementFormExample())
		{
			Title = "Выписка грузов";

			var table = CreateDataGridView(200, "Cargo");
			CreateButton("Экспот", Export);

			table.Size = table.Size + new System.Drawing.Size(100, 0);
			Form.Size = Form.Size + new System.Drawing.Size(100, 0);
		}

		private void Export(object obj, EventArgs args)
		{
			try
			{
				RegistersDataManager registersData = new RegistersDataManager();
				DocumentCreator.Instance().CreateCargoDocument(registersData.GetCargoInfos(0));

				MessageCaller.CallInfomationMessage("Документ успешно создан");
			}
			catch (Exception ex)
			{
				MessageCaller.CallErrorMessage(ex.Message);
			}
		}

		private void TableInit()
		{
			List<TableColumn> columns = new List<TableColumn>()
			{
				new TableColumn("ID", "ID Партии", TableColumnType.TEXT),
				new TableColumn("Cargo", "Груз", TableColumnType.TEXT),
				new TableColumn("Unit", "Ед. изм.", TableColumnType.TEXT),
				new TableColumn("Count", "Количество", TableColumnType.TEXT),
				new TableColumn("InsuredCount", "Застраховано", TableColumnType.TEXT),
			};

			GridViewPresenter gridCreate = new GridViewPresenter(columns, Tables["Cargo"]);

			foreach (DataGridViewColumn column in Tables["Cargo"].Columns)
				column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
		}

		private void TableLoad()
		{
			var dataManager = new RegistersDataManager();
			var list = dataManager.GetCargoInfos(0);

			foreach (var item in list)
				Tables["Cargo"].Rows.Add(item.ConsignmentId, item.CargoName, item.UnitName, item.Count, item.InsuredCount);
		}

		protected override void Load(object obj, EventArgs eventArgs)
		{
			base.Load(obj, eventArgs);
			TableInit();
			TableLoad();
		}
	}
}
