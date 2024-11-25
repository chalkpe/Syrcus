using WebSocketSharp.Server;

namespace Syrcus.Services {
  public abstract class Service: WebSocketBehavior {
    public abstract void Enable ();
    public abstract void Disable ();
    public abstract void Update ();
  }
}
