
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;

namespace IGCConsWrapper
{
	/// <summary>
	/// Interaction logic for PopBatWindow.xaml
	/// </summary>
	public partial class PopBatWindow : Window
	{
		private ConsFolder consFolder;
		
		public PopBatWindow(ConsFolder consFolder)
		{
			InitializeComponent();
			this.btn_OK.Click += new RoutedEventHandler(btn_OK_Click);
			this.btn_browse.Click += new RoutedEventHandler(btn_browse_Click);
			this.cb_inettimeout.Checked += new RoutedEventHandler(cb_inettimeout_Checked);
			this.cb_inettimeout.Unchecked += new RoutedEventHandler(cb_inettimeout_Unchecked);
			this.cb_tempinetdir.Checked += new RoutedEventHandler(cb_tempinetdir_Checked);
			this.cb_tempinetdir.Unchecked += new RoutedEventHandler(cb_tempinetdir_Unchecked);
			this.consFolder = consFolder;
		}
		
		private void cb_inettimeout_Checked(Object sender, RoutedEventArgs e)
		{
			this.txt_inettimeout.IsEnabled = true;
		}
		
		private void cb_inettimeout_Unchecked(Object sender, RoutedEventArgs e)
		{
			this.txt_inettimeout.IsEnabled = false;
		}
		
		private void cb_tempinetdir_Checked(Object sender, RoutedEventArgs e)
		{
			this.txt_tempinetdir.IsEnabled = true;
			this.btn_browse.IsEnabled = true;
		}
		
		private void cb_tempinetdir_Unchecked(Object sender, RoutedEventArgs e)
		{
			this.txt_tempinetdir.IsEnabled = false;
			this.btn_browse.IsEnabled = false;
		}
		
		private void btn_browse_Click(Object sender, RoutedEventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
			dialog.ShowDialog();
			this.txt_tempinetdir.Text = dialog.SelectedPath;
		}
		
		private void btn_OK_Click(Object sender, RoutedEventArgs e)
		{
			int inettimeout = 0;
			string tempinetdir = "";
			if (this.cb_inettimeout.IsChecked == true)
			{
				Regex regex = new Regex(@"^[0-9]?[0-9]$");
				if ((regex.IsMatch(this.txt_inettimeout.Text) == false)
					|| (int.Parse(this.txt_inettimeout.Text) < 3)
					|| (int.Parse(this.txt_inettimeout.Text) > 60))
				{
					Message.Show(Errorlevel.Warning, "Время ожидания должно быть в диапазоне от 3 до 60.");
					return;
				}
				inettimeout = int.Parse(this.txt_inettimeout.Text);
			}
			if (this.cb_tempinetdir.IsChecked == true)
			{
				if (!Directory.Exists(this.txt_tempinetdir.Text))
				{
					Message.Show(Errorlevel.Warning, "Указан несуществующий путь.");
					return;
				}
				tempinetdir = this.txt_tempinetdir.Text;
			}
			this.consFolder.MakePopBat(inettimeout, tempinetdir);
			this.Close();
		}
	}
}