using System.ComponentModel;
using System.Configuration.Install;

namespace Paradox.WinService
{
	[RunInstaller(true)]
	public partial class WinServiceInstaller : Installer
	{
		public WinServiceInstaller()
		{
			InitializeComponent();
		}		
	}
}
