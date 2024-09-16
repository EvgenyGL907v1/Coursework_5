using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DatabaseManager;

namespace PresenterContext
{
	/// <summary> Базовое поведение Presenter </summary>
	public class PresenterBase
	{
		private string _title = "";

		// Уровень последнего элемента на окне
		private int _lastElementLocationY;

		public Form Form { get; private set; }
		protected DatabaseConnection Database => DatabaseConnection.Instance;

		public string Title
		{
			get { return _title; }
			set
			{ 
				_title = value;

				if(Form != null)
					Form.Text = _title;
			}
		}

		#region Elements

		protected Dictionary<string, Button> Buttons { get; private set; }
		protected Dictionary<string, TextBox> TextBoxes { get; private set; }
		protected Dictionary<string, CheckBox> CheckBoxes { get; private set; }
		protected Dictionary<string, DataGridView> Tables { get; private set; }
		protected Dictionary<string, ComboBox> ComboBoxes { get; private set; }
		protected Dictionary<string, DateTimePicker> DateTimePickers { get; private set; }
		protected Dictionary<string, PictureBox> Pictures { get; private set; }

		#endregion

		// Мин. размеры элементов интерфейса
		#region ELEMENTS_MIN_SIZE

		protected const int ButtonSize = 120;
		protected const int CheckBoxSize = 120;
		protected const int TextBoxSize = 230;
		protected const int ComboBoxSize = 230;

		#endregion

		// Расстояние между элементами интерфейса
		#region PADDING

		private const int ElementPadding = 10;
		private const int FormGrowth = ElementPadding * 7;

		#endregion

		public PresenterBase(Form form)
		{
			Buttons = new Dictionary<string, Button>();
			TextBoxes = new Dictionary<string, TextBox>();
			CheckBoxes = new Dictionary<string, CheckBox>();
			Tables = new Dictionary<string, DataGridView>();
			ComboBoxes = new Dictionary<string, ComboBox>();
			DateTimePickers = new Dictionary<string, DateTimePicker>();
			Pictures = new Dictionary<string, PictureBox>();

			_lastElementLocationY = 0;

			form.FormBorderStyle = FormBorderStyle.FixedSingle;
			form.AutoSize = false;
			form.MinimizeBox = false;
			form.MaximizeBox = false;

			form.Text = _title;
			form.Load += new EventHandler(Load);
			Form = form;
		}

		protected virtual void Load(object obj, EventArgs eventArgs)
		{ }

		#region MESSAGES

		/// <summary> Сообщение с вопросом (Да/Нет) </summary>
		protected bool QuestionDialog(string text, string title)
		{
			DialogResult result = MessageBox.Show(text, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			return result == DialogResult.Yes;
		}

		/// <summary> Сообщение уведомление </summary>
		protected void Message(string text, string title) => MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
		/// <summary> Сообщение об ошибке </summary>
		protected void ErrorMessage(string text, string title) => MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Error);

		#endregion

		#region CREATE_UI

		/// <summary> Размещение элемента на окне </summary>
		private void PasteElement(Control element, Point offset = new Point())
		{
			int width = (Form.Width - element.Size.Width) / 2;
			int height = _lastElementLocationY + ElementPadding;

			element.Location = new Point(width + offset.X, height + offset.Y);
			Form.Controls.Add(element);

			_lastElementLocationY = height + element.Size.Height;
			int formHeight = height + FormGrowth + element.Size.Height;

			Form.Size = new Size(Form.Size.Width, formHeight);
		}

		/// <summary> Создание кнопки на окне </summary>
		protected void CreateButton(string title, Action<object, EventArgs> buttonEvent = null)
		{ 
			Button newButton = new Button();
			newButton.Text = title;
			
			if(buttonEvent != null)
				newButton.Click += new EventHandler(buttonEvent);

			newButton.Size = new Size(ButtonSize, newButton.Size.Height);

			if (Buttons.Keys.Contains(title))
				throw new Exception($"{_title} - ключ {title} используется несколько раз");

			Buttons.Add(title, newButton);
			PasteElement(newButton);
		}

		/// <summary> Создание элемента ввода текста на окне </summary>
		protected TextBox CreateTextBox(string title)
		{ 
			Label label = new Label();
			label.Text = title;
			label.Size = new Size(label.Size.Width, label.Size.Height);


			PasteElement(label, new Point(-TextBoxSize / 4, ElementPadding));

			TextBox textBox = new TextBox();
			textBox.Size = new Size(TextBoxSize, textBox.Size.Height);

			if(TextBoxes.Keys.Contains(title))
				throw new Exception($"{_title} - ключ {title} используется несколько раз");

			TextBoxes.Add(title, textBox);
			PasteElement(textBox);

			return textBox;
		}

		/// <summary> Создание элемента лог. переменной на окне </summary>
		protected void CreateCheckBox(string title)
		{ 
			CheckBox checkBox = new CheckBox();
			checkBox.Text = title;
			checkBox.Size = new Size(CheckBoxSize, checkBox.Size.Height);

			if (CheckBoxes.Keys.Contains(title))
				throw new Exception($"{_title} - ключ {title} используется несколько раз");

			CheckBoxes.Add(title, checkBox);
			PasteElement(checkBox);
		}

		/// <summary> Создание элемента выбора даты на окне </summary>
		protected DateTimePicker CreateDateTimePicker(string title)
		{
			Label label = new Label();
			label.Text = title;
			label.Size = new Size(label.Size.Width, label.Size.Height);

			PasteElement(label, new Point(-TextBoxSize / 4, ElementPadding));

			DateTimePicker dateTimePicker = new DateTimePicker();
			dateTimePicker.Format = DateTimePickerFormat.Short;

			DateTimePickers.Add(title, dateTimePicker);
			PasteElement(dateTimePicker);

			return dateTimePicker;
		}

		/// <summary> Создание таблицы </summary>
		protected DataGridView CreateDataGridView(int height, string key, Point offset = new Point())
		{ 
			DataGridView dataGridView = new DataGridView();
			dataGridView.Size = new Size(Form.Width, height);

			if (Tables.Keys.Contains(key))
				throw new Exception($"{_title} - ключ {key} используется несколько раз");

			Tables.Add(key, dataGridView);

			PasteElement(dataGridView, offset);

			dataGridView.AllowUserToAddRows = false;
			dataGridView.ReadOnly = true;
			dataGridView.AllowUserToDeleteRows = false;
			dataGridView.AllowUserToResizeColumns = false;

			dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dataGridView.MultiSelect = false;

			return dataGridView;
		}

		/// <summary> Создание элемента выбора </summary>
		protected ComboBox CreateComboBox(string title, EventHandler choice = null) 
		{
			Label label = new Label();
			label.Text = title;
			label.Size = new Size(label.Size.Width, label.Size.Height);

			PasteElement(label, new Point(-TextBoxSize / 4, ElementPadding));

			ComboBox comboBox = new ComboBox();
			comboBox.Size = new Size(ComboBoxSize, comboBox.Height);

			if(choice != null)
				comboBox.Click += choice;

			if (ComboBoxes.Keys.Contains(title))
				throw new Exception($"{_title} - ключ {title} используется несколько раз");

			ComboBoxes.Add(title, comboBox);
			PasteElement(comboBox);

			return comboBox;
		}

		protected PictureBox CreatePictureBox(string title, (int, int) Size, Point offset = new Point())
		{
			PictureBox pictureBox = new PictureBox();
			pictureBox.Size = new Size(Size.Item1, Size.Item2);
			pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

			if (Pictures.Keys.Contains(title))
				throw new Exception($"{_title} - ключ {title} используется несколько раз");

			Pictures.Add(title, pictureBox);
			PasteElement(pictureBox, offset);

			return pictureBox;
		}

		#endregion

		#region COMBO_BOX_CHOISE

		#endregion

		public void Show() => Form.Show();
		public DialogResult ShowDialog() => Form.ShowDialog();
		public void Loading() => Load(null, null);
	}
}
