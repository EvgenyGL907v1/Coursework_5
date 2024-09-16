using DatabaseManager;
using Menus;
using PresenterContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsignmentsMenu
{
	public class CargoInfoCreatorFormPresenter : PresenterBase
	{
		private int _selectedCargoId = -1;
		private int _selectedUnitId = -1;

		public CargoInfo CargoInfo { get; private set; }

		public CargoInfoCreatorFormPresenter() : base(new ElementFormExample())
		{
			CreateComboBox("Груз", CargoChoise);
			CreateComboBox("Ед. изм", UnitChoise);

			CreateTextBox("Количество");
			CreateTextBox("Количество (Стрх)");

			CreateButton("Добавить", CreateInfo);
			CreateButton("Отмена");
		}

		private void CargoChoise(object obj, EventArgs args)
		{
			CargoChoiceFormPresenter cargoChoice = new CargoChoiceFormPresenter();
			if (cargoChoice.ShowDialog() == DialogResult.OK)
			{
				var cargo = cargoChoice.SelectedCargo.Item2;
				_selectedCargoId = cargoChoice.SelectedCargo.Item1;

				var userBox = ComboBoxes["Груз"];
				userBox.Items.Clear();
				userBox.Items.Add(cargo);
				userBox.SelectedIndex = 0;
			}
		}
		private void UnitChoise(object obj, EventArgs args)
		{
			UnitChoiceFormPresenter unitChoice = new UnitChoiceFormPresenter();
			if (unitChoice.ShowDialog() == DialogResult.OK)
			{
				var unit = unitChoice.SelectedUnit.Item2;
				_selectedUnitId = unitChoice.SelectedUnit.Item1;

				var userBox = ComboBoxes["Ед. изм"];
				userBox.Items.Clear();
				userBox.Items.Add(unit);
				userBox.SelectedIndex = 0;
			}
		}

		private void CreateInfo(object obj, EventArgs args)
		{
			try
			{
				if (_selectedCargoId == -1)
					throw new Exception("Груз не выбран");

				if (_selectedUnitId == -1)
					throw new Exception("Ед. изм. не выбрана");

				if (!int.TryParse(TextBoxes["Количество"].Text, out int count))
					throw new Exception("Количество - некорректный формат");

				if (!int.TryParse(TextBoxes["Количество (Стрх)"].Text, out int s_count))
					throw new Exception("Застрахованное количество - некорректный формат");

				if(count <= 0)
					throw new Exception("Количество - меньше или равно 0");

				if (s_count < 0)
					throw new Exception("Застрахованное количество - меньше 0");

				if (s_count > count)
					throw new Exception("Застрахованное количество больше общего количества");

				CargoInfo cargoInfo = new CargoInfo()
				{ 
					UnitId = _selectedUnitId,
					UnitName = ComboBoxes["Ед. изм"].SelectedItem.ToString(),
					CargoId = _selectedCargoId,
					CargoName = ComboBoxes["Груз"].SelectedItem.ToString(),

					Count = count,
					InsuredCount = s_count,
				};

				CargoInfo = cargoInfo;
				Form.DialogResult = DialogResult.OK;
				Form.Close();
			}
			catch (Exception ex)
			{
				MessageCaller.CallErrorMessage(ex.Message);
			}
		}
		private void Break(object obj, EventArgs args)
		{
			Form.DialogResult = DialogResult.Cancel;
			Form.Close();
		}
	}
}
