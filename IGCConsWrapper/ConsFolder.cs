
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace IGCConsWrapper
{
	public class ConsFolder
	{
		public DirectoryInfo root {get; private set;}
		public FileInfo exe {get; private set;}
		public DirectoryInfo din {get; private set;}
		public DirectoryInfo baseFolder {get; private set;}
		public FileInfo baseListPath {get; private set;}
		public FileInfo userListPath {get; private set;}
		public DirectoryInfo receive {get; private set;}
		public bool isValid {get; private set;}
		public List<string> baseList {get; private set;}
		public List<string> systemList {get; private set;}
				
		public ConsFolder()
		{
		}
		
		public ConsFolder(string path)
		{
			this.Initialize(path);
			this.Validate();
			if (this.isValid) setLists();
		}
		
		public void Initialize(string path)
		{
			this.root = new DirectoryInfo(path);
			string root = this.root.ToString();
			this.exe = new FileInfo(root + "\\cons.exe");
			this.din = new DirectoryInfo(root + "\\DISTR\\DIN");
			this.baseFolder = new DirectoryInfo(root + "\\BASE");
			this.baseListPath = new FileInfo(root + "\\BASE\\baselist.cfg");
			this.userListPath = new FileInfo(root + "\\BASE\\userlist.cfg");
			this.receive = new DirectoryInfo(root + "\\RECEIVE");
			this.baseList = new List<string>();
			this.systemList = new List<string>();
		}
		
		public string Validate()
		{
			string result = String.Empty;
			this.isValid = false;
			if (!this.root.Exists)
			{
				result += root.ToString();
				return result;
			}
			if (!this.exe.Exists) result += Environment.NewLine + exe.ToString();
			if (!this.din.Exists) result += Environment.NewLine + din.ToString();
			if (!this.baseFolder.Exists) result += Environment.NewLine + baseFolder.ToString();
			if (!this.baseListPath.Exists) result += Environment.NewLine + baseListPath.ToString();
			//наличие userlist'а не обязательно
			//if (!this.userList.Exists) result += Environment.NewLine + userList.ToString();
			if (!this.receive.Exists) result += Environment.NewLine + receive.ToString();
			
			if (result == String.Empty) this.isValid = true;
			
			return result;
		}
		
		public void setLists()
		{
			try
			{
				BaseList fullBaseList = Helper.GetBaseList();
				foreach (string line in File.ReadAllLines(this.baseListPath.FullName))
				{
					string trimmedline = line.ToUpper().Trim('	',' ');
					if (fullBaseList.HasBase(trimmedline))
						this.baseList.Add(trimmedline);
				}
				this.systemList = GetSystemsFromDin();
			}
			catch (Exception e)
			{
				Message.Show(Errorlevel.Error, e.Message);
			}
		}
		
		public void StartProgram(bool asAdmin = false)
		{
			if (asAdmin) 
			{
				Process.Start(this.exe.FullName, " /adm");
			}
			else
			{
				Process.Start(this.exe.FullName);
			}
		}
		
		public void OpenRootFolder()
		{
			Process.Start(this.root.FullName);
		}
		
		public void Group()
		{
			Process.Start(this.exe.FullName, " /group");
		}
		
		public void Quest()
		{
			Process.Start(this.exe.FullName, " /base* /quest");
		}
		
		public void Quest(ConsBase baseName)
		{
			Process.Start(this.exe.FullName,
			              " /base_" + baseName.shortName
			              + " /quest");
		}
		
		public void Quest(string sendFolder)
		{
			Process.Start(this.exe.FullName,
			              " /senddir=" + '"' + sendFolder + '"'
			              + " /base* /quest");
		}
		
		public void Quest(string sendFolder, ConsBase baseName)
		{
			Process.Start(this.exe.FullName,
			              " /senddir=" + '"' + sendFolder + '"'
			              + " /base_" + baseName.shortName
			              + " /quest");
		}
		
		public void Reg()
		{
			Process.Start(this.exe.FullName, " /adm /reg");
		}
		
		public void Reindex0()
		{
			Process.Start(this.exe.FullName, " /adm /reindex0");
		}
		
		public void Reindex()
		{
			Process.Start(this.exe.FullName, "/adm /base* /reindex");
		}
		
		public void Reindex(ConsBase consBase)
		{
			Process.Start(this.exe.FullName, "/adm /base_" 
			              + consBase.shortName 
			              + " /reindex");
		}
		
		public void MakeList(FileInfo list)
		{
			if (!list.Exists)
			{
				try
				{
					list.Create();
				}
				catch (Exception e)
				{
					Message.Show(Errorlevel.Error, e.Message);
				}
			}
		}
		
		public void EditList(FileInfo list)
		{
			if (!list.Exists) MakeList(list);
			Process.Start(list.FullName);
		}
		
		public List<string> ValidateBaselist()
		{
			SystemList fullSystemList = Helper.GetSystemList();
			List<string> result = new List<string>();
			
			foreach (string system in this.systemList)
			{
				ConsSystem userSystem = fullSystemList.GetSystemByName(system);
				if (userSystem.Equals(ConsSystem.Empty)) continue;
				foreach (ConsBase basename in userSystem.baselist)
				{
					if (!this.baseList.Contains(basename.shortName.ToUpper()))
					{
						result.Add(basename.shortName);
					}
				}
			}
			
			return result;
		}
		
		private List<string> GetSystemsFromDin()
		{
			List<string> foundSystems = new List<string>();
			SystemList fullSystemList = Helper.GetSystemList();
			FileInfo[] din = this.din.GetFiles("*.DIN");
			foreach (var dinFile in din)
			{
				string systemNameFromDin = "";
				int i = 0;
				if (dinFile.Name.Contains("#"))
				{
					while ((dinFile.Name[i]!='#')&&(i<dinFile.Name.Length))
					{
						systemNameFromDin += dinFile.Name[i];
						i++;
					}
				}
				else
				{
					while (((dinFile.Name[i]<'0')||(dinFile.Name[i]>'9'))&&(i<dinFile.Name.Length))
					{
						systemNameFromDin += dinFile.Name[i];
						i++;
					}
				}
				if (fullSystemList.HasSystem(systemNameFromDin)) foundSystems.Add(systemNameFromDin);
			}
			return foundSystems;
		}
		
		public void User()
		{
			Process.Start(this.exe.FullName, " /usr");
		}
		
		public void User(string receiveFolder)
		{
			Process.Start(this.exe.FullName,
			              " /receivedir=" + '"' + receiveFolder + '"'
			              + " /usr");
		}
		
		public void MakeTask(string taskName, string period, string startTime, string username, string password)
		{
			string cmd = " /create"
				+ " /tn " + '"' + taskName + '"'
				+ " /tr " + '"' + this.root.FullName + "\\popolnenie_ip.bat" + '"'
				+ " /ru " + '"' + username + '"'
				+ " /rp " + '"' + password + '"'
				+ " /sc " + period
				+ " /st " + startTime
				+ " /v1 /f";
			Process.Start("schtasks", cmd);
		}
		
		public void MakeTask(string taskName, string period, string startTime)//устаревшее
		{
			string path = this.root.FullName + "\\new_task.bat"; 
			try
			{
				if (File.Exists(path)) File.Delete(path);
				File.AppendAllText(path, "@echo off\r\n"
               		+ "sc query Schedule | find /i " + '"' + "RUNNING"+ '"' + "\r\n"
					+ "if errorlevel 1 (sc start Schedule)\r\n"
					+ "for /f " + '"' + "tokens=2* delims=," + '"' + " %%i in ('schtasks /query /fo csv /v^|find /i " + '"' + "popolnenie_ip.bat" + '"' + "') do (set $name=%%i)\r\n"
					+ "for /f " + '"' + "tokens=2* delims=," + '"' + " %%i in ('schtasks /query /fo csv /v^|find /i " + '"' + "cons.exe /adm /base* /receive_inet /yes" + '"' + "') do (set $name=%%i)\r\n"
					+ "if '%$name%'=='' (\r\n"
					+ "schtasks /create /sc " + period + " /tn " + '"' + taskName + '"' + " /tr " + '"' + this.root.FullName + "\\popolnenie_ip.bat" + '"' + " /st " + startTime + " /v1\r\n"
					+ ") else (\r\n"
					+ "schtasks /delete /tn " + '"' + taskName + '"' + " /f\r\n"
					+ "schtasks /create /sc " + period + " /tn " + '"' + taskName + '"' + " /tr " + '"' + this.root.FullName + "\\popolnenie_ip.bat" + '"' + " /st " + startTime + " /v1\r\n)\r\n"
					+ "pause");
			}
			catch (Exception e)
			{
				Message.Show(Errorlevel.Error, e.Message);
				return;
			}
			Process.Start(path);
		}
		
		public void CheckUpdate(ref ProgressBar pbar)
		{
			UpdateChecker uc = new UpdateChecker(this.baseFolder, this.baseList, ref pbar);
			
			uc.CheckUpdate();
		}
						
		public void MakeStartKey(params string[] keys)
		{
			string start = this.root.FullName + "\\start.key";
			try
			{
				File.WriteAllText(start, "");
				foreach (string element in keys)
				{
					File.AppendAllText(start, " /" + element);
				}
			}
			catch (Exception e)
			{
				Message.Show(Errorlevel.Error, e.Message);
			}
		}
					
		public void MakePopBat(int inetTimeOut = 0, string tempInetDir = "")
		{
			string command = '"' + this.exe.FullName + '"' + " /adm /base* /receive_inet /yes";
			if (inetTimeOut != 0) command += " /inettimeout=" + inetTimeOut.ToString();
			if (tempInetDir != "") command += " /tempinetdir=" + '"' + tempInetDir + '"';
			try
			{
				File.WriteAllText(this.root.FullName + "\\popolnenie_ip.bat", command);
			}
			catch (Exception e)
			{
				Message.Show(Errorlevel.Error, e.Message);
			}
		}
		
		public void CheckBase()
		{
			//TODO: make???
		}
		
		public void ReceiveInet()
		{
			Process.Start(this.exe.FullName,
			              " /adm /base* /receive_inet /yes");
		}
		
		public void Receive()
		{
			Process.Start(this.exe.FullName,
			              " /adm /base* /receive /yes");
		}
		
		public void ReceiveFromFolder(string path)
		{
			Process.Start(this.exe.FullName,
			              " /adm /base* /receive"
			              + " /receivedir=" + '"' + path + '"'
			              + " /yes");
		}
		
		public void ReceiveInet(ConsBase consBase)
		{
			Process.Start(this.exe.FullName,
			              " /adm /base_" 
			              + consBase.shortName 
			              + " /receive_inet /yes");
		}
		
		public void Receive(ConsBase consBase)
		{
			Process.Start(this.exe.FullName,
			              " /adm /base_" 
			              + consBase.shortName 
			              + " /receive /yes");
		}
		
		public void ReceiveFromFolder(ConsBase consBase, string path)
		{
			Process.Start(this.exe.FullName,
			              " /adm /base_" 
			              + consBase.shortName
			              + " /receivedir=" + '"' + path + '"'
			              + " /yes");
		}
	}
}
