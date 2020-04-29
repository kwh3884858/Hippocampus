using System;
using System.Collections.Generic;
using UnityEngine;

public class SkyunionPlugin
{
	private static AndroidJavaObject _plugin;

	static SkyunionPlugin()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.igg.Skyunionbusiness.SkyunionPlugin"))
			{
				SkyunionPlugin.DebugLog(androidJavaClass.ToString());
				SkyunionPlugin._plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
				SkyunionPlugin.DebugLog(SkyunionPlugin._plugin.ToString());
			}
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin] " + ex.Message);
		}
	}

	public static void DebugLog(string pString)
	{
	}

	public static void InitPlugin()
	{
		try
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.igg.Skyunionbusiness.SkyunionPlugin"))
			{
				SkyunionPlugin._plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
			}
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin] " + ex.Message);
		}
	}

	public static void InitIggData()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("InitIggData", new object[0]);
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
		}
	}

	public static bool IsAppInstalled(string pAppId)
	{
		bool result = false;
		if (Application.platform == RuntimePlatform.Android)
		{
			return SkyunionPlugin._plugin.Call<bool>("IsAppInstalled", new object[]
			{
				pAppId
			});
		}
		SkyunionPlugin.DebugLog("[PushNotification]: Android code called from non-Android platform: " + Application.platform);
		return result;
	}

	public static void SetupIGGPlatform(string gameId, string secretKey, string paymentKey, string SenderId)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SetupIGGPlatform", new object[]
			{
				gameId,
				secretKey,
				paymentKey,
				SenderId
			});
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[PushNotification]:" + ex.Message);
		}
	}

	public static void RateGooglePlayApplication(string pStr)
	{
		string text = "market://details?id=" + pStr;
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("RateGooglePlayApplication", new object[]
			{
				text
			});
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[PushNotification]:" + ex.Message);
		}
	}

	public static void OpenGooglePlayStoreApp(string pUrl)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			string text = "com.igg.android.lordsonline_tw";
			SkyunionPlugin._plugin.Call("OpenGooglePlayStoreUrl", new object[]
			{
				pUrl,
				text
			});
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[PushNotification]:" + ex.Message);
		}
	}

	public static string GetDeviceName()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return string.Empty;
		}
		string result;
		try
		{
			result = SkyunionPlugin._plugin.Call<string>("getDeviceName", new object[0]);
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
			result = string.Empty;
		}
		return result;
	}

	public static int PlatformSdkVersion()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return 0;
		}
		int result;
		try
		{
			result = SkyunionPlugin._plugin.Call<int>("getSdkVersion", new object[0]);
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
			result = 0;
		}
		return result;
	}

	public static void AddShortcut(string gameName, string className)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("addShortcut", new object[]
			{
				gameName,
				className
			});
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
		}
	}

	public static void FacebookLike()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("FacebookLike", new object[0]);
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
		}
	}

	public static void LoadWebView(string pUrl)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("LoadWebView", new object[]
			{
				pUrl
			});
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
		}
	}

	public static void TapCashOfferWall()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("TapCashOfferWall", new object[0]);
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
		}
	}

	public static List<string> GetGoogleEmailAcount()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return null;
		}
		List<string> result;
		try
		{
			string[] array = SkyunionPlugin._plugin.Call<string[]>("GetGoogleAccountList", new object[0]);
			List<string> list = new List<string>();
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(array[i]);
			}
			result = list;
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
			result = null;
		}
		return result;
	}

	public static void LinkingGoogleAccount(string pId)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("LinkGoogleAccount", new object[]
			{
				pId
			});
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
		}
	}

	public static void LinkingGoogleAccount()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("LinkGoogleAccount", new object[0]);
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
		}
	}

	public static void GetProductList()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("GetProductList", new object[0]);
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
		}
	}

	public static void RateUs()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("RateUs", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void AutoLogin()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("AutoLogin", new object[0]);
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
		}
	}

	public static void GeustLogin()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (SkyunionPlugin._plugin == null)
		{
			SkyunionPlugin.InitPlugin();
		}
		try
		{
			SkyunionPlugin._plugin.Call("GeustLogin", new object[0]);
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:GeustLogin");
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
		}
	}

	public static void GoogleAccountLogin(string pName)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (SkyunionPlugin._plugin == null)
		{
			SkyunionPlugin.InitPlugin();
		}
		try
		{
			SkyunionPlugin._plugin.Call("GoogleAccountLogin", new object[]
			{
				pName
			});
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
		}
	}

	public static void GoogleAccountLogin()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (SkyunionPlugin._plugin == null)
		{
			SkyunionPlugin.InitPlugin();
		}
		try
		{
			SkyunionPlugin._plugin.Call("GoogleAccountLogin", new object[0]);
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
		}
	}

	public static void ShowAccount(bool bBind)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (SkyunionPlugin._plugin == null)
		{
			SkyunionPlugin.InitPlugin();
		}
		try
		{
			SkyunionPlugin._plugin.Call("ShowAccount", new object[]
			{
				bBind
			});
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
		}
	}

	public static void BuyProduct(string pId, int serverId)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("BuyProduct", new object[]
			{
				pId,
				serverId
			});
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
		}
	}

	public static void OpenPushNotification()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("OpenPushNotification", new object[0]);
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
		}
	}

	public static void ClosePushNotification()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("ClosePushNotification", new object[0]);
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
		}
	}

	public static void OpenGeTuiNotification()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("OpenGeTuiNotification", new object[0]);
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
		}
	}

	public static void ThirdPartPayment()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("ThirdPartPayment", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void TapjoyInit()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("TapjoyInit", new object[0]);
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
		}
	}

	public static void TapjoyOfferWall()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin.DebugLog("it will call TapjoyOfferWall");
			SkyunionPlugin._plugin.Call("TapjoyOfferWall", new object[0]);
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
		}
	}

	public static void SupportLiveOnLogin()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SupportLiveOnLogin", new object[0]);
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
		}
	}

	public static void SupportLiveOnShop()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SupportLiveOnShop", new object[0]);
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
		}
	}

	public static void SendTicket()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SendTicket", new object[0]);
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
		}
	}

	public static void VisitForum()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("ForumLink", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void TermsofService()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("TermsofService", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void PrivacyPolicy()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("PrivacyPolicy", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void LoadGameConfig()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("LoadGameConfig", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void LoadEventConfig()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("LoadEventConfig", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void LoadAdLog()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("LoadAdLog", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void LoginLog()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("LoginLog", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void InitFlurry(string apiKey, bool logUseHttps, bool enableCrashReporting, bool enableDebugging)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin.DebugLog("InitFlurry");
			SkyunionPlugin._plugin.Call("InitFlurry", new object[]
			{
				apiKey,
				logUseHttps,
				enableCrashReporting,
				enableDebugging,
				true
			});
		}
		catch (Exception)
		{
		}
	}

	public static void LogEvent(string eventId, Dictionary<string, string> parameters, bool timed)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap", new object[0]))
			{
				IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
				object[] array = new object[2];
				foreach (KeyValuePair<string, string> current in parameters)
				{
					using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", new object[]
					{
						current.Key + string.Empty
					}))
					{
						using (AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.lang.String", new object[]
						{
							current.Value + string.Empty
						}))
						{
							array[0] = androidJavaObject2;
							array[1] = androidJavaObject3;
							AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
						}
					}
				}
				SkyunionPlugin._plugin.Call("LogEvent", new object[]
				{
					eventId,
					androidJavaObject,
					timed
				});
			}
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog(ex.Message);
		}
	}

	public static void LogEvent(string eventId, string[] keyVals, bool timed)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin.DebugLog("LogEvent");
			SkyunionPlugin._plugin.Call("LogEvent", new object[]
			{
				eventId,
				keyVals,
				timed
			});
		}
		catch (Exception)
		{
		}
	}

	public static void EndTimedEvent(string eventId, string[] keyVals)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin.DebugLog("EndTimedEvent");
			SkyunionPlugin._plugin.Call("EndTimedEvent", new object[]
			{
				eventId,
				keyVals
			});
		}
		catch (Exception)
		{
		}
	}

	public static void EndTimedEvent(string eventId, Dictionary<string, string> parameters)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap", new object[0]))
			{
				IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
				object[] array = new object[2];
				foreach (KeyValuePair<string, string> current in parameters)
				{
					using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", new object[]
					{
						current.Key + string.Empty
					}))
					{
						using (AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.lang.String", new object[]
						{
							current.Value + string.Empty
						}))
						{
							array[0] = androidJavaObject2;
							array[1] = androidJavaObject3;
							AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
						}
					}
				}
				SkyunionPlugin._plugin.Call("EndTimedEvent", new object[]
				{
					eventId,
					androidJavaObject
				});
			}
		}
		catch (Exception)
		{
		}
	}

	public static void LogError(string errorId, string message, string errorClass)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("LogError", new object[]
			{
				errorId,
				message,
				errorClass
			});
		}
		catch (Exception)
		{
		}
	}

	public static void LogPageview()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("LogPageview", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void SetUserId(string userId)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("FlurrySetUserId", new object[]
			{
				userId
			});
		}
		catch (Exception)
		{
		}
	}

	public static void SetUserAge(int userAge)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("FlurrySetUserAge", new object[]
			{
				userAge
			});
		}
		catch (Exception)
		{
		}
	}

	public static void SetUserGender(bool isFemale)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SetUserGender", new object[]
			{
				isFemale
			});
		}
		catch (Exception)
		{
		}
	}

	public static void SetUserLocation(float latitude, float longitude)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SetUserLocation", new object[]
			{
				latitude,
				longitude
			});
		}
		catch (Exception)
		{
		}
	}

	public static void SetSessionTimeout(int seconds)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SetSessionTimeout", new object[]
			{
				seconds
			});
		}
		catch (Exception)
		{
		}
	}

	public static void SendMail(string MailTo, string subject, string UTCTime, string GameName, string GameVersion, string PlayerID, string Language, string DeviceType, string OsVersion)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SendMail", new object[]
			{
				MailTo,
				subject,
				UTCTime,
				GameName,
				GameVersion,
				PlayerID,
				Language,
				DeviceType,
				OsVersion
			});
		}
		catch (Exception)
		{
		}
	}

	public static void OpenFbByUrl(string url)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("OpenFbByUrl", new object[]
			{
				url
			});
		}
		catch (Exception)
		{
		}
	}

	public static void GoToNews(string pUrl, string key)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("GoToNews", new object[]
			{
				pUrl,
				key
			});
		}
		catch (Exception)
		{
		}
	}

	public static void UseExchangeCode()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("UseExchange", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void SubmitQuestion()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SubmitQuestion", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void Guide(string url)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("Guide", new object[]
			{
				url
			});
		}
		catch (Exception)
		{
		}
	}

	public static void SupportLiveOnLogin_GlobalEdition()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SupportLiveOnLogin_GlobalEdition", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void SupportLiveOnShop_GlobalEdition()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SupportLiveOnShop_GlobalEdition", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void SetAppsFlyerKey()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SetAppsFlyerKey", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void AppsFlyerSignUp()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("AppsFlyerSignUp", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void LaunchEvent()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("LaunchEvent", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void AppsFlyerTutorialCompletion()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("AppsFlyerTutorialCompletion", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void HeroStageCompletion()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("HeroStageCompletion", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void AppsFlyerAdvance(string type)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("AppsFlyerAdvance", new object[]
			{
				type
			});
		}
		catch (Exception)
		{
		}
	}

	public static void SetLocalNotification(int nid, string msg, int sec)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SetLocalNotification", new object[]
			{
				nid,
				msg,
				sec
			});
		}
		catch (Exception)
		{
		}
	}

	public static void CancelNotification(int nid)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("CancelNotification", new object[]
			{
				nid
			});
		}
		catch (Exception)
		{
		}
	}

	public static void SetFacebookSdkInitialize()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SetFacebookSdkInitialize", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void SetFacebookEventActivateApp()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SetFacebookEventActivateApp", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void SetFacebookEventDeactivateApp()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SetFacebookEventDeactivateApp", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void SetFacebookEventLaunched()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SetFacebookEventLaunched", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void CompleteRegistration(string registrationMethod)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("CompleteRegistration", new object[]
			{
				registrationMethod
			});
		}
		catch (Exception)
		{
		}
	}

	public static void SetFacebookEventCompletedTutorial()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SetFacebookEventCompletedTutorial", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void SetFacebookEventPurchases(double price, string num_items, string content_type, string content_id, string currency)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SetFacebookEventPurchases", new object[]
			{
				price,
				num_items,
				content_type,
				content_id,
				currency
			});
		}
		catch (Exception)
		{
		}
	}

	public static void SetFacebookEventSpentCredits(double value, string content_type, string content_id)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SetFacebookEventSpentCredits", new object[]
			{
				value,
				content_type,
				content_id
			});
		}
		catch (Exception)
		{
		}
	}

	public static void SetFacebookCustomEvent(string eventName, string iggID, string time, KeyValuePair<string, string> parameters)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			if (!parameters.Key.Equals(string.Empty))
			{
				SkyunionPlugin._plugin.Call("SetFacebookCustomEvent", new object[]
				{
					eventName,
					iggID,
					time,
					parameters.Key,
					parameters.Value
				});
			}
		}
		catch (Exception)
		{
		}
	}

	public static void SetFacebookCustomEvent(string eventName, string iggID, string time)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SetFacebookCustomEvent", new object[]
			{
				eventName,
				iggID,
				time
			});
		}
		catch (Exception)
		{
		}
	}

	public static void ShareOnFacebook()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("ShareOnFacebook", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void ShowFacebookDebug()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("ShowFacebookDebug", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void GetToken()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("GetToken", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void SwitchFacebook()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SwitchFacebook", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void RegisterCallback()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("RegisterCallback", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void ClearFacebookSession()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("ClearFacebookSession", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void NotificationUninitialize()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("NotificationUninitialize", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static bool CheckGooglePlayServicesUtil()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return false;
		}
		bool result;
		try
		{
			bool flag = SkyunionPlugin._plugin.Call<bool>("CheckGooglePlayServicesUtil", new object[0]);
			result = flag;
		}
		catch (Exception)
		{
			result = false;
		}
		return result;
	}

	public static void SetTragetLanguage(string str)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SetTragetLanguage", new object[]
			{
				str
			});
		}
		catch (Exception)
		{
		}
	}

	public static void Translate(string str)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("Translate", new object[]
			{
				str
			});
		}
		catch (Exception)
		{
		}
	}

	public static void TranslateBatch(string str)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("TranslateBatch", new object[]
			{
				str
			});
		}
		catch (Exception)
		{
		}
	}

	public static void Translate_Mail(string str)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("Translate_Mail", new object[]
			{
				str
			});
		}
		catch (Exception)
		{
		}
	}

	public static void TranslateBatch_Mail(string str)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("TranslateBatch_Mail", new object[]
			{
				str
			});
		}
		catch (Exception)
		{
		}
	}

	public static void TranslateBatch_Diplomatic(string str)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("TranslateBatch_Diplomatic", new object[]
			{
				str
			});
		}
		catch (Exception)
		{
		}
	}

	public static void Translate_KA(string str)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("Translate_KA", new object[]
			{
				str
			});
		}
		catch (Exception)
		{
		}
	}

	public static void Translate_AA(string str)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("Translate_AA", new object[]
			{
				str
			});
		}
		catch (Exception)
		{
		}
	}

	public static string GetCountry()
	{
		string text = string.Empty;
		if (Application.platform != RuntimePlatform.Android)
		{
			return text;
		}
		string result;
		try
		{
			text = SkyunionPlugin._plugin.Call<string>("GetCountry", new object[0]);
			result = text;
		}
		catch (Exception)
		{
			result = text;
		}
		return result;
	}

	public static void OpenThirdPartyPayment(int serverID)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("OpenThirdPartyPayment", new object[]
			{
				serverID
			});
		}
		catch (Exception)
		{
		}
	}

	public static void LoginWeChat()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("LoginWeChat", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void BindWeChat()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("BindToWeChat", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static bool isWXAppInstalled()
	{
		bool flag = false;
		if (Application.platform != RuntimePlatform.Android)
		{
			return flag;
		}
		bool result;
		try
		{
			flag = SkyunionPlugin._plugin.Call<bool>("isWXAppInstalled", new object[0]);
			result = flag;
		}
		catch (Exception)
		{
			Debug.LogError("isWXAppInstalled Exception");
			result = flag;
		}
		return result;
	}

	public static void GetWeChatProductList()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("GetWeChatProductList", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void WeChatPay(string itemId, string itemName, string price)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("WeChatPay", new object[]
			{
				itemId,
				itemName,
				price
			});
		}
		catch (Exception)
		{
		}
	}

	public static void AliPay(string itemId, string itemName, string price)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("AliPay", new object[]
			{
				itemId,
				itemName,
				price
			});
		}
		catch (Exception)
		{
		}
	}

	public static void UninitializeGeTu()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("UninitializeGeTu", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void AmazonActive()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("AmazonActive", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void AmazonDeactivate()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("AmazonDeactive", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void GetAmazonProductList()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("GetAmazonProductList", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void AmazonPay(string itemid)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("AmazonPay", new object[]
			{
				itemid
			});
		}
		catch (Exception)
		{
		}
	}

	public static void BindAmazon()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("BindAmazon", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void LoginAmazon()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("LoginAmazon", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void LogoutAmazon()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("LogoutAmazon", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void OpenAmazonNotification()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("OpenAmazonNotification", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void UninitializeAmazonADM()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("UninitializeAmazonNotification", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void RateAmazonApplication(string url)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("RateAmazonApplication", new object[]
			{
				url
			});
		}
		catch (Exception)
		{
		}
	}

	public static string GetExternalFilesDir()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return null;
		}
		string result;
		try
		{
			result = SkyunionPlugin._plugin.Call<string>("GetExternalFilesDir", new object[0]);
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
			result = null;
		}
		return result;
	}

	public static string GetFilesDir()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return null;
		}
		string result;
		try
		{
			result = SkyunionPlugin._plugin.Call<string>("GetFilesDir", new object[0]);
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
			result = null;
		}
		return result;
	}

	public static string GetObbDir()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return null;
		}
		string result;
		try
		{
			result = SkyunionPlugin._plugin.Call<string>("GetObbDir", new object[0]);
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
			result = null;
		}
		return result;
	}

	public static string GetPackageName()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return null;
		}
		string result;
		try
		{
			result = SkyunionPlugin._plugin.Call<string>("GetPackageName", new object[0]);
		}
		catch (Exception ex)
		{
			SkyunionPlugin.DebugLog("[SkyunionPlugin]:" + ex.Message);
			result = null;
		}
		return result;
	}

	public static void ShowExit()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("ShowExit", new object[0]);
		}
		catch (Exception)
		{
		}
	}

	public static void SetSystemUiVisibility()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		try
		{
			SkyunionPlugin._plugin.Call("SetSystemUiVisibility", new object[0]);
		}
		catch (Exception)
		{
		}
	}
}
