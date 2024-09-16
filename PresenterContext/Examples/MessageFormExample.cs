using System.Windows.Forms;

namespace PresenterContext
{
	public partial class MessageFormExample : Form
	{
		public MessageFormPresenter Presenter { get; set; }

		public MessageFormExample(string message)
		{
			InitializeComponent();
			ShowMessage(message);
		}

		public void ShowMessage(string message)
		{
			richTextBox1.Text = message;
		}
	}
}
