using System;
using System.Net.NetworkInformation;
using System.Text;

namespace CSharpWin32Common
{
	public class Pinger
	{
		// args[0] can be an IPaddress or host name. 
		public PingReply reply;
		public bool success = false;
		public Pinger(string host)
		{
			try {
				Ping pingSender = new Ping();
				PingOptions options = new PingOptions();

				// Use the default Ttl value which is 128, 
				// but change the fragmentation behavior.
				options.DontFragment = true;

				// Create a buffer of 32 bytes of data to be transmitted. 
				string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
				byte[] buffer = Encoding.ASCII.GetBytes(data);
				int timeout = 30;
				reply = pingSender.Send(host, timeout, buffer, options);
				if (reply.Status == IPStatus.Success)
				{
					success = true;
					//    //Console.WriteLine("Address: {0}", reply.Address.ToString());
					//    //Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
					//    //Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
					//    //Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
					//    //Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
					//    return reply;
				}
				else
				{
					Log.Debug("Ping failed trying again...");
					reply = pingSender.Send(host, timeout, buffer, options);
					if (reply.Status == IPStatus.Success)
					{
						success = true;
					}
				}
			}
			catch(Exception e)
			{
				Log.Error("Ping exception: " + e.Message);
			}
		}

		public static bool checkForVPNConnection()
		{
			//Does not work with Medassets VPN
			//if (NetworkInterface.GetIsNetworkAvailable())
			//{
			//    NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
			//    foreach (NetworkInterface Interface in interfaces)
			//    {
			//        Log.Message(Interface.Name + " " + Interface.NetworkInterfaceType.ToString() + " " + Interface.Description + " " + NetworkInterfaceType.Ppp.ToString());
			//        if (Interface.OperationalStatus == OperationalStatus.Up)
			//        {
			//            if ((Interface.NetworkInterfaceType == NetworkInterfaceType.Ppp) && (Interface.NetworkInterfaceType != NetworkInterfaceType.Loopback))
			//            {
			//                //IPv4InterfaceStatistics statistics = Interface.GetIPv4Statistics();
			//                //MessageBox.Show(Interface.Name + " "  + Interface.NetworkInterfaceType.ToString() + " " + Interface.Description);
							return true;
			//            }
			//        }
			//    }
			//}
			//return false;
		}
		private static bool IsUserInDomain()
		{
			//Not exactly what I wanted.
			//var prefix = WindowsIdentity.GetCurrent().Name.Split('\\')[0].ToUpperInvariant();
			//var domainName = Environment.UserDomainName;
			//var machineName = Environment.MachineName.ToUpperInvariant();
			//Log.Debug("UN prefix: " + prefix + " Machinename: " + machineName + " domainName: " + domainName);
			//if (prefix != machineName)
			//{
				return true;
			//}
			//else
			//{
			//    return false;
			//}
		}
	}
}
