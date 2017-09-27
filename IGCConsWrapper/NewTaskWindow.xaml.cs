
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace IGCConsWrapper
{
	public partial class NewTaskWindow : Window
	{
		private ConsFolder consFolder;
		
		public NewTaskWindow(ConsFolder consFolder)
		{
			InitializeComponent();
			this.consFolder = consFolder;
			this.btn_OK.Click += new RoutedEventHandler(btn_OK_Click);
			this.pnl_checkboxes.Visibility = Visibility.Hidden;
			this.rb_weekly.Checked += new RoutedEventHandler(rb_weekly_Checked);
			this.rb_weekly.Unchecked += new RoutedEventHandler(rb_weekly_Unchecked);
			this.txt_username.Text = Environment.UserName;
			this.btn_scheduler.Click += new RoutedEventHandler(btn_scheduler_Click);
		}
		
		private void btn_scheduler_Click(Object sender, RoutedEventArgs e)
		{
			/*if (Environment.OSVersion.Version.Major >= 6)//for Vista and up
				Process.Start("taskschd.msc");
			else*/ //for XP and down
				Process.Start("control", "schedtasks"); //works for every OS?
		}
		
		private void rb_weekly_Checked(Object sender, RoutedEventArgs e)
		{
			this.pnl_checkboxes.Visibility = Visibility.Visible;
		}
		
		private void rb_weekly_Unchecked(Object sender, RoutedEventArgs e)
		{
			this.pnl_checkboxes.Visibility = Visibility.Hidden;
		}
		
		private void btn_OK_Click(Object sender, RoutedEventArgs e)
		{
			string taskName = this.txt_taskName.Text;
			Regex regex = new Regex(@"^[a-zA-Zа-яА-Я][0-9а-яА-Яa-zA-Z +-]*$");
			if (regex.IsMatch(taskName) == false)
			{
				Message.Show(Errorlevel.Warning, "Имя задачи должно начинаться с буквы и состоять\r\n" +
				             "только из букв, цифр, пробелов и символов +-");
				return;
			}
			string period = "";
			if (this.rb_daily.IsChecked == true)
			{
				period = "daily";
			}
			else if (this.rb_weekly.IsChecked == true)
			{
				period = "weekly /d ";
				if (this.cb_mon.IsChecked == true) period += "MON,";
				if (this.cb_tue.IsChecked == true) period += "TUE,";
				if (this.cb_wed.IsChecked == true) period += "WED,";
				if (this.cb_thu.IsChecked == true) period += "THU,";
				if (this.cb_fri.IsChecked == true) period += "FRI,";
				if (this.cb_sat.IsChecked == true) period += "SAT,";
				if (this.cb_sun.IsChecked == true) period += "SUN,";
				if (!period.EndsWith(","))
				{
					Message.Show(Errorlevel.Warning, "Выберите дни для запуска задания.");
					return;
				}
				period = period.Trim(',');
			}
			string startTime = this.txt_startTime.Text;
			regex = new Regex(@"^[0-2]\d:[0-5]\d$");
			if ((regex.IsMatch(startTime) == false)
			    || (int.Parse(startTime.Split(':')[0]) > 23))
			{
				Message.Show(Errorlevel.Warning, "Должно быть указано корректное время в формате ЧЧ:ММ!\r\n Например, 19:00.");
				return;
			}
			string username = this.txt_username.Text;
			if (username == "")
			{
				Message.Show(Errorlevel.Warning, "Укажите имя пользователя.");
				return;
			}
			string password = this.txt_password.Password;
			consFolder.MakeTask(taskName, period, startTime, username, password);
			this.Close();
		}
	}
}