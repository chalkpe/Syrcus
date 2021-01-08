using Dalamud.Game.Chat;
using Dalamud.Game.Chat.SeStringHandling;
using Dalamud.Game.Chat.SeStringHandling.Payloads;
using Dalamud.Game.Internal;
using Dalamud.Plugin;
using Newtonsoft.Json.Linq;
using Syrcus.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Syrcus {
  public class Plugin: IDalamudPlugin {
    public string Name => "Syrcus";

    public static WebSocketServer server { get; private set; }
    public static DalamudPluginInterface pi { get; private set; }
    private List<Service> services;

    public void Initialize (DalamudPluginInterface pluginInterface) {
      services = new List<Service> {
        new ChatService()
      };

      pi = pluginInterface;
      pi.Framework.OnUpdateEvent += OnUpdate;

      server = new WebSocketServer(10078);
      foreach (var serv in services) {
        serv.Enable();
      }
      server.Start();

      PluginLog.Information("Loaded");
    }

    private void OnUpdate (Framework framework) {
      foreach (var serv in services) {
        serv.Update();
      }
    }

    #region IDisposable Support
    protected virtual void Dispose (bool disposing) {
      if (!disposing) return;

      foreach (var serv in services) {
        serv.Disable();
      }
      server.Stop();
      pi.Dispose();
    }

    public void Dispose () {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
    #endregion
  }
}
