using Reactor.API.Logging;
using Reactor.API.Logging.Base;
using static Reactor.API.Extensions.StringExtensions;

namespace Distance.UnityExplorer.Decorators
{
	public class LabelDecorator : ColorDecorator
	{
		private readonly string label;

		public LabelDecorator(string label)
		: this(label, 255, 255, 255)
		{}

		public LabelDecorator(string label, byte r, byte g, byte b)
		: base(r, g, b)
		{
			this.label = label;
		}

		public override string Decorate(LogLevel logLevel, string input, string originalMessage, Sink sink)
		{
			return label.AnsiColorEncodeRGB(r, g, b);
		}
	}
}
