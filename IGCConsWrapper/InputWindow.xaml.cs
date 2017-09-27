
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;

namespace IGCConsWrapper
{
	public partial class InputWindow : Window
	{
		public InputWindow(string title, string prompt, bool showFolderBrowser)
		{
			InitializeComponent();
			this.btn_browseFolder.Click += new RoutedEventHandler(btn_browseFolder_Click);
			this.btn_OK.Click += new RoutedEventHandler(btn_OK_Click);
			this.Title = title;
			this.txt_prompt.Text = prompt;
			if (showFolderBrowser) 
			{
				this.btn_browseFolder.Visibility = Visibility.Visible;
			}
			else
			{
				this.btn_browseFolder.Visibility = Visibility.Collapsed;
			}
		}
		
		private void btn_browseFolder_Click(Object sender, RoutedEventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
			dialog.ShowDialog();
			this.txt_usrFolder.Text = dialog.SelectedPath;
		}
		
		private void btn_OK_Click(Object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}
		
		public string Result
		{
			get { return this.txt_usrFolder.Text; }
		}
	}
}