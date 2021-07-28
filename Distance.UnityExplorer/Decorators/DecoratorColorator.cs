using Reactor.API.Logging;
using Reactor.API.Logging.Base;
using static Reactor.API.Extensions.StringExtensions;

namespace Distance.UnityExplorer.Decorators
{
	public class DecoratorColorator : ColorDecorator
	{
		private readonly Decorator decorator;

		public DecoratorColorator(Decorator decorator, byte r, byte g, byte b)
		: base(r, g, b)
		{
			this.decorator = decorator;
		}

		public override string Decorate(LogLevel logLevel, string input, string originalMessage, Sink sink)
		{
			return decorator.Decorate(logLevel, input, originalMessage, sink).AnsiColorEncodeRGB(r, g, b);
		}
	}
}
