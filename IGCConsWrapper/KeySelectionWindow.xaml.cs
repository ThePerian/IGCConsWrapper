
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace IGCConsWrapper
{
	/// <summary>
	/// Interaction logic for KeySelectionWindow.xaml
	/// </summary>
	public partial class KeySelectionWindow : Window
	{
		public KeySelectionWindow()
		{
			InitializeComponent();
			this.btn_OK.Click += new RoutedEventHandler(btn_OK_Click);
		}
		
		private void btn_OK_Click(Object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}
		
		public string[] GetStartKeys()
		{
			List<string> result = new List<string>();
			if (this.cb_adm.IsChecked == true) result.Add("adm");
			if (this.cb_defbrowser.IsChecked == true) result.Add("defbrowser");
			if (this.cb_inet.IsChecked == true) result.Add("inet");
			if (this.cb_noverbase.IsChecked == true) result.Add("noverbase");
			return result.ToArray();
		}
	}
}