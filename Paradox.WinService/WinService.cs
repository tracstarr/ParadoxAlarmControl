using System.ServiceProcess;
using ServiceStack;

namespace Paradox.WinService
{
	public partial class WinService : ServiceBase
	{
		private readonly AppHostHttpListenerBase appHost;
		private readonly string listeningOn;

		public WinService(AppHostHttpListenerBase appHost, string listeningOn)
		{
			this.appHost = appHost;
			this.listeningOn = listeningOn;

			this.appHost.Init();

			InitializeComponent();
		}

	    public void TestStart()
	    {
	        OnStart(null);
	    }

		protected override void OnStart(string[] args)
		{
			this.appHost.Start(listeningOn);
		}

		protected override void OnStop()
		{
			this.appHost.Stop();
		}
	}
}
