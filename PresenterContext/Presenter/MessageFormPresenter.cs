
namespace PresenterContext
{
	public class MessageFormPresenter : PresenterBase
	{

		public MessageFormPresenter(string message) : base(new MessageFormExample(message))
		{
			var form = Form as MessageFormExample;
			form.Presenter = this;
		}

	}
}
