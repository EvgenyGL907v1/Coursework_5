using DatabaseManager;
using Menus;
using PresenterContext;
using System;
using System.Windows.Forms;
using DataExport;

namespace VesselsMenu
{
	public class VesselElementFormPresenter : ElementFormPresenter
	{
		private RegistersDataManager _registedDataManager;

		private int _selectedTypeId;
		private int _selectedClassId;

		private byte[] _image;

		public VesselElementFormPresenter(int elementId, ElementFormType type) : base(elementId, type)
		{
			_registedDataManager = new RegistersDataManager();

			CreateTextBox("Название");
			CreateTextBox("Номер");

			CreatePictureBox("Фото", (160, 100));
			CreateButton("Загрузить", LoadPicture);

			CreateComboBox("Тип судна", new EventHandler(TypeChoise));
			CreateComboBox("Класс судна", new EventHandler(ClassChoise));

			CreateDateTimePicker("Дата постройки");

			CreateTextBox("ФИО Капитана");

			if (type == ElementFormType.CREATE)
				CreateButton("Добавить", CreateElementEvent);
			else if (type == ElementFormType.UPDATE)
				CreateButton("Редактировать", EditElementEvent);
		}

		protected override void Load(object obj, EventArgs eventArgs)
		{
			base.Load(obj, eventArgs);
			if (Type == ElementFormType.UPDATE)
				LoadData();
		}

		private void LoadPicture(object obj, EventArgs eventArgs)
		{
			try
			{
				_image = ImageLoader.UploadImage();
				ImageLoader.DisplayImageFromDatabase(Pictures["Фото"], _image);
			}
			catch (Exception ex)
			{
				MessageCaller.CallErrorMessage(ex.Message);
			}
		}

		private void LoadData()
		{
			var vessel = _registedDataManager.GetVesselsList().Find(v => v.VesselId == CurrentElementId);

			TextBoxes["Название"].Text = vessel.Name;
			TextBoxes["Номер"].Text = vessel.RegNumber;
			TextBoxes["ФИО Капитана"].Text = vessel.CaptainName;

			ReferencesManager references = new ReferencesManager();

			_selectedTypeId = vessel.TypeId;
			_selectedClassId = vessel.ClassId;

			ComboBoxes["Тип судна"].Items.Add(references.GetTypesList().Find(t => t.Item1 == vessel.TypeId).Item2);
			ComboBoxes["Тип судна"].SelectedIndex = 0;
			ComboBoxes["Класс судна"].Items.Add(references.GetClassesList().Find(c => c.Item1 == vessel.ClassId).Item2);
			ComboBoxes["Класс судна"].SelectedIndex = 0;

			ImageLoader.DisplayImageFromDatabase(Pictures["Фото"], vessel.Image);

		}

		private void TypeChoise(object obj, EventArgs args)
		{
			VesselTypeChoiceFormPresenter typeChoice = new VesselTypeChoiceFormPresenter();
			if (typeChoice.ShowDialog() == DialogResult.OK)
			{
				var user = typeChoice.SelectedType.Item2;
				_selectedTypeId = typeChoice.SelectedType.Item1;

				var userBox = ComboBoxes["Тип судна"];
				userBox.Items.Clear();
				userBox.Items.Add(user);
				userBox.SelectedIndex = 0;
			}
		}

		private void ClassChoise(object obj, EventArgs args)
		{
			VesselClassChoiceFormPresenter classChoice = new VesselClassChoiceFormPresenter();
			if (classChoice.ShowDialog() == DialogResult.OK)
			{
				var user = classChoice.SelectedType.Item2;
				_selectedClassId = classChoice.SelectedType.Item1;

				var userBox = ComboBoxes["Класс судна"];
				userBox.Items.Clear();
				userBox.Items.Add(user);
				userBox.SelectedIndex = 0;
			}
		}

		public override void CreateElement()
		{
			base.CreateElement();

			if (_selectedClassId == -1)
				throw new Exception("Не выбран класс");

			if (_selectedTypeId == -1)
				throw new Exception("Не выбран тип");

			if (DateTimePickers["Дата постройки"].Value == null)
				throw new Exception("Дата не выбрана");

			VesselData vesselData = new VesselData()
			{
				Name = TextBoxes["Название"].Text,
				RegNumber = TextBoxes["Номер"].Text,

				CreateDate = DateTimePickers["Дата постройки"].Value.ToString("yyyy-MM-dd"),
				CaptainName = TextBoxes["ФИО Капитана"].Text,

				TypeId = _selectedTypeId,
				ClassId = _selectedClassId,

				Image = _image
			};

			_registedDataManager.Add(vesselData);
			MessageCaller.CallInfomationMessage("Информация о судне успешно записана");
			Form.Close();
		}

		public override void EditElement()
		{
			base.EditElement();

			if (_selectedClassId == -1)
				throw new Exception("Не выбран класс");

			if (_selectedTypeId == -1)
				throw new Exception("Не выбран тип");

			if (DateTimePickers["Дата постройки"].Value == null)
				throw new Exception("Дата не выбрана");

			VesselData vesselData = new VesselData()
			{
				VesselId = CurrentElementId,

				Name = TextBoxes["Название"].Text,
				RegNumber = TextBoxes["Номер"].Text,

				CreateDate = DateTimePickers["Дата постройки"].Value.ToString("yyyy-MM-dd"),
				CaptainName = TextBoxes["ФИО Капитана"].Text,

				TypeId = _selectedTypeId,
				ClassId = _selectedClassId,

				Image = _image
			};

			_registedDataManager.Edit(vesselData);
			MessageCaller.CallInfomationMessage("Информация о судне успешно изменена");
			Form.Close();
		}

	}
}
