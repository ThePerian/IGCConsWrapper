
using System;
using System.Windows;

namespace IGCConsWrapper
{
	public enum Errorlevel
	{
		Normal,
		Warning,
		Error
	}
	
	public static class Message
	{
		public static void Show(Errorlevel errorlevel, params string[] message)
		{
			MessageBoxImage image;
			switch (errorlevel)
			{
				case Errorlevel.Normal: image = MessageBoxImage.Information; break;
				case Errorlevel.Warning: image = MessageBoxImage.Warning; break;
				case Errorlevel.Error: image = MessageBoxImage.Error; break;
				default: image = MessageBoxImage.Information; break;
			}
			
			string inline = "";
			foreach (string line in message)
			{
				inline += line + Environment.NewLine;
			}
			
			if (message.Length > 8)
			{
				MessageWindow window = new MessageWindow("Информ-Групп Помощник", inline);
				window.ShowDialog();
			}
			else 
			{
				MessageBox.Show(inline, "Информ-Групп Помощник", MessageBoxButton.OK, image);
			}
		}
	}
}
