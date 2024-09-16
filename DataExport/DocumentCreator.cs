using DatabaseManager;
using System;
using System.Collections.Generic;
using WinForms = System.Windows.Forms;

namespace DataExport
{
	public class DocumentCreator
	{
		// Константы для директорий и названий файлов
		private const string DocumentRoutSample = "Маршруты";
		private const string DocumentCargoSample = "Грузы";

		private static DocumentCreator _instance;

		// Получение экземпляра класса DocumentMaster (реализация паттерна Singleton)
		public static DocumentCreator Instance()
		{
			if (_instance == null)
				_instance = new DocumentCreator();

			return _instance;
		}

		private DocumentCreator()
		{ }

		/// <summary>
		/// Выбор директории для сохранения документов
		/// </summary>
		/// <returns>Путь к выбранной директории</returns>
		private string GetFolder()
		{
			WinForms.FolderBrowserDialog folderBrowser = new WinForms.FolderBrowserDialog();
			WinForms.DialogResult result = folderBrowser.ShowDialog();

			if (result != WinForms.DialogResult.OK)
				throw new Exception("Папка не выбрана");

			if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
				return folderBrowser.SelectedPath;

			throw new Exception("Папка имеет некорректный формат");
		}

		/// <summary>
		/// Создание документа в формате excel
		/// </summary>
		/// <param name="applicationModel">Маршруты</param>
		public void CreateRoutsDocument(List<(int, string, int, string)> routs)
		{
			ExcelDocumentCreator excelDocumentCreator = new ExcelDocumentCreator();

			string[] titles = new string[] { "Id", "Маршрут", "Корабль" };

			string[,] data = new string[routs.Count, titles.Length];

			// Заполняем данные для таблицы
			for (int i = 0; i < routs.Count; i++)
			{
				var item = routs[i];

				data[i, 0] = item.Item1.ToString();
				data[i, 1] = item.Item2.ToString();
				data[i, 2] = item.Item4.ToString();
			}

			string filePath = GetFolder();
			excelDocumentCreator.ExportDataToExcel(titles, data, filePath, DocumentRoutSample);
		}

		/// <summary>
		/// Создание документа в формате excel
		/// </summary>
		/// <param name="applicationModel">Грузы</param>
		public void CreateCargoDocument(List<CargoInfo> cargoList)
		{
			ExcelDocumentCreator excelDocumentCreator = new ExcelDocumentCreator();

			string[] titles = new string[] { "ID Партии", "Груз", "Ед. измерения", "Количество", "Застраховано" };

			string[,] data = new string[cargoList.Count, titles.Length];

			// Заполняем данные для таблицы
			for (int i = 0; i < cargoList.Count; i++)
			{
				var item = cargoList[i];

				data[i, 0] = item.ConsignmentId.ToString();
				data[i, 1] = item.CargoName.ToString();
				data[i, 2] = item.UnitName.ToString();
				data[i, 3] = item.Count.ToString();
				data[i, 4] = item.InsuredCount.ToString();
			}

			string filePath = GetFolder();
			excelDocumentCreator.ExportDataToExcel(titles, data, filePath, DocumentRoutSample);
		}

	}
}
