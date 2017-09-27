
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace IGCConsWrapper
{
	public struct Task
	{
		public string source;
		public string destination;
		public List<string> basenames;
	}
	
	public partial class ProcessWindow : Window
	{
		long size;
		int count;
		List<FileInfo> files;
		DateTime startTime;
		BackgroundWorker worker;
		
		public ProcessWindow()
		{
			InitializeComponent();
			size = 0;
			count = 0;
			files = new List<FileInfo>();
			this.btn_Cancel.Click += new RoutedEventHandler(btn_Cancel_Click);
		}
		
		private void btn_Cancel_Click(Object sender, RoutedEventArgs e)
		{
			if (this.worker.IsBusy)
			{
				this.worker.CancelAsync();
			}
			else
			{
				this.Close();
			}
		}
		
		public void CopyBase(Task task)
		{
			foreach(string item in task.basenames)
			{
				if (!Directory.Exists(task.source + "\\" + item)) continue;
				foreach(string file in Directory.GetFiles(task.source + "\\" + item))
				{
					FileInfo fileInfo = new FileInfo(file);
					if ((fileInfo.Name.ToUpper().StartsWith(item.ToUpper() + "."))
					    ||(fileInfo.Name.ToUpper().Equals("RECEIVE.INF")))
					{
						size += fileInfo.Length;
						count++;
						files.Add(fileInfo);
					}
				}
			}
			if (files.Count == 0) 
			{
				this.Close();
				return;
			}
			SetDefault(task);
			worker.RunWorkerAsync(task);
		}
		
		private void SetDefault(Task task)
		{
			this.txt_from.Text = "Что: " + files[files.Count - count].FullName;
			this.txt_to.Text = "Куда: " + task.destination;
			this.txt_filesLeft.Text = "Осталось файлов: " + count.ToString();
			this.txt_timeLeft.Text = "Осталось времени: ...";
			this.txt_timeSinceStart.Text = "С начала копирования прошло: ...";
			startTime = DateTime.Now;
			this.progressBar.Maximum = size;
			this.progressBar.Value = 0;
			worker = new BackgroundWorker();
			worker.WorkerReportsProgress = true;
			worker.WorkerSupportsCancellation = true;
			worker.DoWork += new DoWorkEventHandler(worker_DoWork);
			worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
			worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
		}
		
		public void CopyAns(Task task)
		{
			foreach(string item in task.basenames)
			{
				foreach(string file in Directory.GetFiles(task.source))
				{
					FileInfo fileInfo = new FileInfo(file);
					if ((fileInfo.Name.ToUpper().StartsWith(item.ToUpper()))
					    &&(fileInfo.Name.ToUpper().EndsWith(".ANS")))
					{
						size += fileInfo.Length;
						count++;
						files.Add(fileInfo);
					}
				}
			}
			if (files.Count == 0) 
			{
				this.Close();
				return;
			}
			SetDefault(task);
			worker.RunWorkerAsync(task);
		}
		
		private void worker_DoWork(Object sender, DoWorkEventArgs e)
		{
			Task task = (Task)e.Argument;
			foreach (FileInfo file in files)
			{
				if (worker.CancellationPending) 
				{
					e.Cancel = true;
					return;
				}
				try
				{
					if (file.Name.ToUpper().EndsWith(".ANS"))
					{
						File.Copy(file.FullName, task.destination + "\\" + file.Name, true);
					}
					else
					{
						string parentBase = file.FullName.Split('\\')[file.FullName.Split('\\').Length-2];
						if (!Directory.Exists(task.destination + "\\" + parentBase))
							Directory.CreateDirectory(task.destination + "\\" + parentBase);
						File.Copy(file.FullName, task.destination 
						          + "\\" 
						          + parentBase
						          + "\\"
						          + file.Name, true);
					}
					worker.ReportProgress(0);
				}
				catch (Exception ex)
				{
					Message.Show(Errorlevel.Error, ex.Message);
				}
			}
		}
		
		private void worker_ProgressChanged(Object sender, ProgressChangedEventArgs e)
		{
			this.progressBar.Value += files[files.Count - count].Length;
			size -= files[files.Count - count].Length;
			count--;
			if (count == 0) return;
			this.txt_from.Text = "Что: " + files[files.Count - count].FullName;
			this.txt_filesLeft.Text = "Осталось файлов: " 
				+ count 
				+ " (Примерно " 
				+ Math.Round((double)(size/1024/1024),2).ToString()
				+ " Мб)";
			TimeSpan timeSinceStart = new TimeSpan((DateTime.Now - startTime).Ticks);
			double minutesSinceStart = timeSinceStart.TotalMinutes;
			double bytesCopied = progressBar.Value/1024/1024;
			double bytesLeft = (progressBar.Maximum - progressBar.Value)/1024/1024;
			double minutesLeft = (minutesSinceStart * bytesLeft) / bytesCopied;
			this.txt_timeLeft.Text = "Осталось времени: " + Math.Round(minutesLeft, 2) + " мин";
			this.txt_timeSinceStart.Text = "C начала копирования прошло: " 
				+ timeSinceStart.Minutes + ":"
				+ timeSinceStart.Seconds;
		}
		
		private void worker_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Cancelled)
			{
				this.Title = "Операция отменена";
			}
			else
			{
				this.Title = "Операция завершена успешно";
			}
			this.btn_Cancel.Content = "OK";
			this.progressBar.Value = progressBar.Maximum;
			this.txt_filesLeft.Text = "Осталось файлов: 0";
			this.txt_timeLeft.Text = "Осталось времени: 0 мин";
		}
	}
}