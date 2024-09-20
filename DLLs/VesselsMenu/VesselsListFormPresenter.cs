using DatabaseManager;
using PresenterContext;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace VesselsMenu
{
	internal class VesselsListFormPresenter : ListFormPresenter
	{
		private RegistersDataManager _registersDataManager;

		public VesselsListFormPresenter() : base()
		{
			_registersDataManager = new RegistersDataManager();
		}

		private void TableInit()
		{
			List<TableColumn> columns = new List<TableColumn>()
			{
				new TableColumn("Id", "#", TableColumnType.TEXT),
				new TableColumn("Name", "Название", TableColumnType.TEXT),
				new TableColumn("ClassName", "Класс", TableColumnType.TEXT),
				new TableColumn("TypeNumber", "Тип", TableColumnType.TEXT),
				new TableColumn("Date", "Дата постройки", TableColumnType.TEXT),

                new TableColumn("RegNumber", "Номер судна", TableColumnType.TEXT),
                new TableColumn("CaptainName", "ФИО капитана", TableColumnType.TEXT),

            };
            GridViewPresenter gridViewPresenter = new GridViewPresenter(columns, Tables[TableKey]);
            //gridViewPresenter.DataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            
        }

		protected override void ButtonsInit(ElementsListFormExample formExample)
		{
			base.ButtonsInit(formExample);
			TableInit();
		}

		public override void Add()
		{
			base.Add();

			VesselElementFormPresenter accessElement = new VesselElementFormPresenter(0, ElementFormType.CREATE);
			accessElement.ShowDialog();
			TableUpdate();
		}

		public override void Edit()
		{
			base.Edit();

			if (SelectedId == -1)
				throw new Exception("Не выбрана строка");

			VesselElementFormPresenter accessElement = new VesselElementFormPresenter(SelectedId, ElementFormType.UPDATE);
			accessElement.ShowDialog();
			TableUpdate();
		}

		public override void Delete()
		{
			if (MessageBox.Show($"Вы уверены, что хотите удалить запись?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
                base.Delete();

                if (SelectedId == -1)
                    throw new Exception("Не выбрана строка");

                _registersDataManager.DeleteVessel(SelectedId);
                TableUpdate();
            }
		}

		public override void TableUpdate()
		{
			base.TableUpdate();
			var vessels = _registersDataManager.GetVesselsList();

			Tables[TableKey].Rows.Clear();

            foreach (var vessel in vessels)
            {
                DateTime createDate = DateTime.Parse(vessel.CreateDate);
                Tables[TableKey].Rows.Add(vessel.VesselId, vessel.Name, vessel.ClassName, vessel.TypeName, createDate.ToString("dd.MM.yyyy"), vessel.RegNumber, vessel.CaptainName);
            }
        }
	}
}
