using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
#if UNITY_IOS
using DeadMosquito.IosGoodies;
#endif
public class AppstoreHandler : Singleton<AppstoreHandler> 
{
#if UNITY_IOS
	[DllImport ("__Internal")] private static extern void _OpenAppInStore(long appID);
	[DllImport("__Internal")] private static extern void _ShowOverlayForAppWithId(long appID);



	string[] trimmedstr;
#endif


	public void openAppInStore(string appID)
	{

	
			if (!Application.isEditor)
		{
#if UNITY_IOS
				trimmedstr = null;
		    trimmedstr = IGDevice.SystemVersion.Split(char.Parse("."));
			int version = int.Parse(trimmedstr[0]);
			if (version >= 13)
			{
				long appIDIOS;

				if (long.TryParse(appID, out appIDIOS))
				{
					_OpenAppInStore(appIDIOS);
				}

			}
			else
			{
				var AppURL = string.Format("https://apps.apple.com/app/id{0}", appID);
				Application.OpenURL(AppURL);
			}

			
#endif


		}
		else
		{	Debug.Log("AppstoreHandler:: Cannot open Appstore in Editor.");
		}
	}

	public void appstoreClosed()
	{	Debug.Log("AppstoreHandler:: Appstore closed.");
	}

	public void ShowOverlay(string appID)
	{
#if UNITY_IOS
		long appIDIOS;

		if (long.TryParse(appID, out appIDIOS))
		{
			_ShowOverlayForAppWithId(appIDIOS);

		}

#endif
	}

	public void CloseOverlay()
    {

    }
}	
