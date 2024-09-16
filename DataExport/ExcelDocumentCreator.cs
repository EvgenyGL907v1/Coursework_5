using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseManager;
using Microsoft.Office.Interop.Excel;

namespace DataExport
{
	/// <summary>
	/// Класс для создания документа Excel и экспорта данных
	/// </summary>
	public class ExcelDocumentCreator
	{
		/// <summary>
		/// Метод для экспорта данных в Excel
		/// </summary>
		/// <param name="dataTittles">dataTittles - заголовки столбцов</param>
		/// <param name="data">data - двумерный массив данных</param>
		/// <param name="filePath">filePath - путь к папке для сохранения файла</param>
		/// <param name="fileName">fileName - имя файла Excel</param>
		public void ExportDataToExcel(string[] dataTittles, string[,] data, string filePath, string fileName)
		{
			int length = dataTittles.Length;

			// Создаем новое приложение Excel
			var excelApp = new Application();

			// Создаем новую рабочую книгу
			Workbook workbook = excelApp.Workbooks.Add(Type.Missing);

			// Получаем первый лист книги
			Worksheet worksheet = (Worksheet)workbook.ActiveSheet;

			// Задаем заголовки столбцов
			for (int i = 0; i < dataTittles.Length; i++)
			{
				worksheet.Cells[1, i + 1] = dataTittles[i];
			}

			// Заполняем данные
			// Строка, с которой начинаются данные - 2
			for (int i = 0; i < data.GetLength(0); i++)
			{
				for (int j = 0; j < data.GetLength(1); j++)
				{
					worksheet.Cells[i + 2, j + 1] = data[i, j];
				}
			}

			// Автонастройка ширины столбцов на основе содержимого
			worksheet.Columns.AutoFit();

			// Сохраняем файл Excel на диск
			string newFileName = Path.Combine(filePath, DateTime.Now.ToString("dd.MM.yyyy-HH.mm-") + fileName + ".xlsx");
			workbook.SaveAs(newFileName);

			// Закрываем рабочую книгу и приложение Excel
			workbook.Close();
			excelApp.Quit();
		}

	}
}
