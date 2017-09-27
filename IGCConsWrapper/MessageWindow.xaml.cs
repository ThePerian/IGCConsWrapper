
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
	/// Interaction logic for MessageWindow.xaml
	/// </summary>
	public partial class MessageWindow : Window
	{
		public MessageWindow(string title, string message)
		{
			InitializeComponent();
			this.Title = title;
			this.tb_message.Text = message;
			this.btn_OK.Click += new RoutedEventHandler(btn_OK_Click);
		}
		
		private void btn_OK_Click(Object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}