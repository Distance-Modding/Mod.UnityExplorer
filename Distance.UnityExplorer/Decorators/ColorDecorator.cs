using Reactor.API.Logging.Base;

namespace Distance.UnityExplorer.Decorators
{
	public abstract class ColorDecorator : Decorator
	{
		protected readonly byte r, g, b;

		protected ColorDecorator(byte r, byte g, byte b)
		{
			this.r = r;
			this.g = g;
			this.b = b;
		}
	}
}
