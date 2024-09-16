using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using PresenterContext;

namespace DataExport
{
	public class ImageLoader
	{
		// Метод для загрузки изображения
		public static byte[] UploadImage()
		{
			byte[] imageData = null;

			// Создание диалогового окна для выбора файла
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.gif;*.png)|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG|All files (*.*)|*.*";
			openFileDialog.Title = "Select an Image File";

			// Отображение диалогового окна
			DialogResult result = openFileDialog.ShowDialog();

			// Если файл выбран, читаем его и возвращаем данные
			if (result == DialogResult.OK)
			{
				try
				{
					string imagePath = openFileDialog.FileName;

					using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
					{
						using (var reader = new BinaryReader(stream))
						{
							imageData = reader.ReadBytes((int)stream.Length);
						}
					}
				}
				catch (Exception ex)
				{
					MessageCaller.CallErrorMessage("Ошибка: обработки файла" + ex.Message);
				}
			}

			return imageData;
		}

		// Метод для отображения на форме
		public static void DisplayImageFromDatabase(PictureBox pictureBox, byte[] imageData)
		{
			if (imageData == null || imageData.Length == 0)
				return;

			using (var stream = new MemoryStream(imageData))
			{
				pictureBox.Image = Image.FromStream(stream);
			}
		}

	}
}
