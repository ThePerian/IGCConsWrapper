
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace IGCConsWrapper
{
	public static class Helper
	{
		public static void ShowNetworkName()
		{
			MessageBox.Show("Сетевое имя текущего пользователя: "
			                + Environment.NewLine
			                + Environment.UserName, 
			                "Информ-Групп Помощник",
			                MessageBoxButton.OK,
			                MessageBoxImage.Information);
		}
		
		public static SystemList GetSystemList()
		{
			SystemList systemList = null;
			string path = System.AppDomain.CurrentDomain.BaseDirectory + "\\systemlist.xml";
			if (!File.Exists(path)) 
			{
				Message.Show(Errorlevel.Error, "Не найден список систем systemlist.xml!");
				Application.Current.Shutdown(1);
				return systemList;
			}
			
			XmlSerializer serializer = new XmlSerializer(typeof(SystemList));
			
			try
			{
				StreamReader reader = new StreamReader(path);
				systemList = (SystemList)serializer.Deserialize(reader);
				reader.Close();
			}
			catch(Exception e)
			{
				Message.Show(Errorlevel.Error, e.Message);
			}
			return systemList;
		}
		
		public static BaseList GetBaseList()
		{
			BaseList baseList = null;
			string path = System.AppDomain.CurrentDomain.BaseDirectory + "\\baselist.xml";
			if (!File.Exists(path)) 
			{
				Message.Show(Errorlevel.Error, "Не найден список баз baselist.xml!");
				Application.Current.Shutdown(1);
				return baseList;
			}
			
			XmlSerializer serializer = new XmlSerializer(typeof(BaseList));
			
			try
			{
				StreamReader reader = new StreamReader(path);
				baseList = (BaseList)serializer.Deserialize(reader);
				reader.Close();
			}
			catch(Exception e)
			{
				Message.Show(Errorlevel.Error, e.Message);
			}
			
			return baseList;
		}
		
		/*
		public static void CopyPop(string pathFrom, ConsFolder consFolder)
		{
			string[] userBaseList = File.ReadAllLines(consFolder.baseListPath.FullName);
			BaseList fullBaseList = GetBaseList();
			foreach (string basename in userBaseList)
			{
				if (fullBaseList.HasBase(basename))
					CopyPopForBase(pathFrom, consFolder.receive.FullName, basename);
			}
		}
		
		private static void CopyPopForBase(string pathFrom, string pathTo, string basename)
		{
			foreach (string file in Directory.GetFiles(pathFrom))
			{
				if (file.Split('\\').Last().ToUpper().StartsWith(basename.ToUpper()))
				{
					File.Copy(file, pathTo);
				}
			}
		}
		
		public static void CopyBase(string pathFrom, ConsFolder consFolder, string basename)
		{
			foreach (string file in Directory.GetFiles(pathFrom + "\\" + basename))
			{
				File.Copy(file, consFolder.baseFolder + "\\" + file.Split('\\').Last());
			}
		}*/
	}
}
