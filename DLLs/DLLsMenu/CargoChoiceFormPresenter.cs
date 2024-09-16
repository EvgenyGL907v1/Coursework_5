﻿using DatabaseManager;
using PresenterContext;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Menus
{
	public class CargoChoiceFormPresenter : ChoiceFormPresenter
	{
		private ReferencesManager _referencesManager;
		private List<(int, string)> _items;

		public (int, string) SelectedCargo { get; set; }

		public CargoChoiceFormPresenter()
		{
			_referencesManager = new ReferencesManager();
		}

		public override void TableInit()
		{
			base.TableInit();

			List<TableColumn> columns = new List<TableColumn>()
			{
				new TableColumn("Id", "#", TableColumnType.TEXT),
				new TableColumn("Cargo", "Название", TableColumnType.TEXT)
			};

			GridViewPresenter gridCreate = new GridViewPresenter(columns, Tables[MainTableKey]);

			foreach (DataGridViewColumn column in Tables[MainTableKey].Columns)
				column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
		}

		public override void SelectItem(object obj, EventArgs eventArgs)
		{
			base.SelectItem(obj, eventArgs);

			var selectedData = _items.Find(m => m.Item1 == SelectedId);
			SelectedCargo = (selectedData.Item1, selectedData.Item2);

			Form.DialogResult = DialogResult.OK;
			Form.Close();
		}

		public override void TableLoad(object obj)
		{
			base.TableLoad(obj);
			_items = _referencesManager.GetCargoList();

			var table = Tables[MainTableKey];

			foreach (var item in _items)
			{
				table.Rows.Add(item.Item1, item.Item2);
			}
		}
	}
}
