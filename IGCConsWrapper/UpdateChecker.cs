
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace IGCConsWrapper
{
	public class UpdateChecker
	{
		private List<string> userBaseList;
		private ProgressBar pbar;
		private BackgroundWorker worker;
		private DirectoryInfo baseFolder;
		
		private const int dateOfUpdate = 0;
		private const int baseShortName = 0;
		private const int updateFileName = 1;
		private const int docsAdded = 2;
		private const int docsChanged = 12;
		private const int totalDocs = 19;
		
		public UpdateChecker(DirectoryInfo baseFolder, List<string> userBaseList, ref ProgressBar pbar)
		{
			this.baseFolder = baseFolder;
			this.userBaseList = userBaseList;
			this.pbar = pbar;
			Initialize();
		}
		
		private void Initialize()
		{
			worker = new BackgroundWorker();
			worker.WorkerReportsProgress = true;
			worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
			worker.DoWork += new DoWorkEventHandler(worker_DoWork);
			worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
						
			pbar.Maximum = userBaseList.Count;
			pbar.Value = 0;
			pbar.Visibility = Visibility.Visible;
		}
		
		public void CheckUpdate()
		{
			worker.RunWorkerAsync();
		}
		
		private void worker_ProgressChanged(Object sender, ProgressChangedEventArgs e)
		{
			pbar.Value += e.ProgressPercentage;
		}
		
		private void worker_DoWork(Object sender, DoWorkEventArgs e)
		{
			string[] receiveLines;
			string lastRec;
			string[] lastRecSplit;
			List<Update> lastUpdateList = new List<Update>();
			string receiveinfPath;
					
			foreach (string baseName in userBaseList)
			{
				receiveinfPath = this.baseFolder.FullName + "\\" + baseName + "\\receive.inf";
				if (!File.Exists(receiveinfPath)) continue;
				
				receiveLines = File.ReadAllLines(receiveinfPath);
				for (int i = receiveLines.Length-1; i>=0; i--)
				{
					lastRec = receiveLines[i];
					lastRecSplit = lastRec.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
					if ((lastRecSplit[updateFileName].Split('#')[baseShortName].ToUpper()!=baseName.ToUpper())
					    ||((lastRecSplit[docsAdded]=="0")&&(lastRecSplit[docsChanged]=="0")))
						continue;
					lastUpdateList.Add(new Update(baseName,
					                          lastRecSplit[dateOfUpdate], 
					                          lastRecSplit[totalDocs]));
					break;
				}
				worker.ReportProgress(1);
			}
			
			e.Result = lastUpdateList;
		}
		
		private void worker_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
		{
			pbar.Visibility = Visibility.Collapsed;
			
			List<Update> update = e.Result as List<Update>;
			List<string> stringUpdate = new List<string>();
			stringUpdate.Add("Краткое		:: Количество	:: Последнее");//FIXME: output via table control
			stringUpdate.Add("название	:: документов	:: обновление");
			stringUpdate.Add("		:: 		::");
			foreach (Update item in update)
			{
				stringUpdate.Add(item.basename
				                 + "		:: " + item.docCount
				                 + "		:: " + item.lastUpdate);
			}
			string[] str = stringUpdate.ToArray();
			Message.Show(Errorlevel.Normal, str);
		}
	}
}
