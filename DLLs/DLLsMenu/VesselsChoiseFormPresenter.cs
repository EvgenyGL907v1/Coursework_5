using DatabaseManager;
using PresenterContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Menus
{

	public class VesselsChoiseFormPresenter : ChoiceFormPresenter
	{
		private RegistersDataManager _registersDataManager;
		private List<VesselData> _items;

		public VesselData SelectedVessel { get; set; }

		public VesselsChoiseFormPresenter()
		{
			_registersDataManager = new RegistersDataManager();
		}

		public override void TableInit()
		{
			base.TableInit();

			List<TableColumn> columns = new List<TableColumn>()
			{
				new TableColumn("Id", "#", TableColumnType.TEXT),
				new TableColumn("Vessel", "Название", TableColumnType.TEXT)
			};

			GridViewPresenter gridCreate = new GridViewPresenter(columns, Tables[MainTableKey]);

			foreach (DataGridViewColumn column in Tables[MainTableKey].Columns)
				column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
		}

		public override void SelectItem(object obj, EventArgs eventArgs)
		{
			base.SelectItem(obj, eventArgs);

			SelectedVessel = _items.Find(m => m.VesselId == SelectedId);

			Form.DialogResult = DialogResult.OK;
			Form.Close();
		}

		public override void TableLoad(object obj)
		{
			base.TableLoad(obj);
			_items = _registersDataManager.GetVesselsList();
			var table = Tables[MainTableKey];

			foreach (var item in _items)
			{
				table.Rows.Add(item.VesselId, item.Name);
			}
		}
	}
}
