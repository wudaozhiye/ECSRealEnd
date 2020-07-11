using System;
using System.IO;
using System.Xml;
using UnityEngine;

namespace Lockstep.Game
{
	public static class ProjectConfig
	{
		static ProjectConfig()
		{
			ProjectConfig.IsBuildWithUnity = true;
			ProjectConfig.DoInit("");
		}
		
		public static void DoInit(string pureConfigPath = "")
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.IgnoreComments = true;
			byte[] bytes = Resources.Load<TextAsset>("GlobalConfig").bytes;
			MemoryStream input = new MemoryStream(bytes);
			XmlReader xmlReader = XmlReader.Create(input, xmlReaderSettings);
			xmlDocument.Load(xmlReader);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("GlobalConfig");
			XmlElement xmlElement = xmlNode["PathConfig"];
			string innerText = xmlElement["UnityEditorPersistentPath"].InnerText;
			string innerText2 = xmlElement["UnityRuntimePersistentPath"].InnerText;
			string innerText3 = xmlElement["PureModePersistentPath"].InnerText;
			string innerText4 = xmlElement["PureModeStreamAssetPath"].InnerText;
			xmlReader.Close();
			bool isEditor = ProjectConfig.IsEditor;
			if (isEditor)
			{
				ProjectConfig._persistentDataPath = Path.Combine(Application.dataPath, innerText);
			}
			else
			{
				ProjectConfig._persistentDataPath = Path.Combine(Application.persistentDataPath, innerText2);
			}
			RuntimePlatform platform = Application.platform;
			if (platform != RuntimePlatform.IPhonePlayer)
			{
				if (platform != RuntimePlatform.Android)
				{
					ProjectConfig._streamingAssetsPath = Application.streamingAssetsPath;
				}
				else
				{
					ProjectConfig._streamingAssetsPath = "jar:file://" + Application.dataPath + "!/assets/";
				}
			}
			else
			{
				ProjectConfig._streamingAssetsPath = Application.dataPath + "/Raw/";
			}
			ProjectConfig.DataPath = ProjectConfig._persistentDataPath;
			ProjectConfig.StreamDataPath = ProjectConfig._streamingAssetsPath;
			ProjectConfig.ExcelPath = ProjectConfig.DataPath + ProjectConfig._relExcelPath;
			ProjectConfig.MapPath = Application.dataPath +"\\"+ ProjectConfig._relMapPath;
		}
		
		public static string StreamDataPath { get; private set; }
		
		public static string DataPath { get; private set; }
		
		public static string ExcelPath { get; private set; }
		
		public static string ExcelZipPath
		{
			get
			{
				return Path.Combine(ProjectConfig.StreamDataPath, "ExcelBytes.zip");
			}
		}
		
		public static string MapZipPath
		{
			get
			{
				return Path.Combine(ProjectConfig.StreamDataPath, "Maps.zip");
			}
		}
		
		public static string MapPath { get; private set; }
		
		public static void CopyFileFromSAPathToPDPath(string srcSAPath, string dstPDPath)
		{
			string sourceFileName = Path.Combine(ProjectConfig.StreamDataPath, srcSAPath);
			string destFileName = Path.Combine(ProjectConfig.DataPath, dstPDPath);
			File.Copy(sourceFileName, destFileName, true);
		}
		
		public static string GameName = "Tank";
		
		public static string _relExcelPath = "ExcelBytes/";
		
		public static string _relMapPath = "Maps/";
		
		public static string _persistentDataPath;
		
		public static string _streamingAssetsPath;
		
		public static bool IsEditor = false;
		
		public static bool IsBuildWithUnity = false;
	}
}
