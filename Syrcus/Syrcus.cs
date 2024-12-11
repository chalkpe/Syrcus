using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Syrcus.Services;
using System.Collections.Generic;
using WebSocketSharp.Server;

namespace Syrcus;

public sealed class Plugin: IDalamudPlugin {
  public static WebSocketServer server { get; private set; }
  private List<Service> services;

  [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
  [PluginService] internal static IFramework Framework { get; private set; } = null!;
  [PluginService] internal static IChatGui ChatGui { get; private set; } = null!;

  [PluginService] internal static IGameGui GameGui { get; private set; } = null!;

  [PluginService] internal static IAddonLifecycle AddonLifecycle { get; private set; } = null!;

  public Plugin () {
    services = new List<Service> {
        new ChatService()
      };

    server = new WebSocketServer(10078);
    foreach (var serv in services) {
      serv.Enable();
    }
    server.Start();

    Framework.Update += OnUpdate;
  }

  private void OnUpdate (IFramework framework) {
    foreach (var serv in services) {
      serv.Update();
    }
  }


  public void Dispose () {

    foreach (var serv in services) {
      serv.Disable();
    }
    server.Stop();

    Framework.Update -= OnUpdate;
  }
}
