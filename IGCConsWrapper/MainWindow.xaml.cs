using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using Microsoft.Win32;
using System.Linq;

namespace IGCConsWrapper
{
	public partial class MainWindow : Window
	{
		public ConsFolder consFolder;
		public SystemList fullSystemList;
		public BaseList fullBaseList;
		
		public MainWindow()
		{
			InitializeComponent();
			consFolder = null;
			hideSecondaryControls();
			addEventHandlers();
			fullBaseList = Helper.GetBaseList();
			fullSystemList = Helper.GetSystemList();
		}
		
		private void hideSecondaryControls()
		{
			this.gb_found.Visibility = Visibility.Collapsed;
			this.gb_popSource.Visibility = Visibility.Collapsed;
			this.pnl_popbtns.Visibility = Visibility.Collapsed;
			this.toolBar.Visibility = Visibility.Collapsed;
			this.menu_main.Visibility = Visibility.Collapsed;
			if (this.Height > 150)
			{
				this.MinHeight = 150;
				this.Height = 150;
			}
		}
		
		private void showSecondaryControls()
		{
			this.gb_found.Visibility = Visibility.Visible;
			this.gb_popSource.Visibility = Visibility.Visible;
			this.pnl_popbtns.Visibility = Visibility.Visible;
			this.toolBar.Visibility = Visibility.Visible;
			this.menu_main.Visibility = Visibility.Visible;
			this.rbtn_base.IsChecked = true;
			rbtn_base_Checked(this, new RoutedEventArgs());
			this.rbtn_int.IsChecked = true;
			this.btn_browsePopSource.Visibility = Visibility.Collapsed;
			this.txt_popSource.Visibility = Visibility.Collapsed;
			this.btn_copyPop.Visibility = Visibility.Collapsed;
			this.btn_copyBase.Visibility = Visibility.Collapsed;
			if (this.Height < 480) 
			{
				this.MinHeight = 355;
				this.Height = 480;
			}
		}
		
		private void addEventHandlers()
		{
			this.btn_browseConsFolder.Click += new RoutedEventHandler(btn_browseConsFolder_Click);
			this.btn_openFolder.Click += new RoutedEventHandler(btn_openFolder_Click);
			this.rbtn_base.Checked += new RoutedEventHandler(rbtn_base_Checked);
			this.rbtn_system.Checked += new RoutedEventHandler(rbtn_system_Checked);
			this.btn_browsePopSource.Click += new RoutedEventHandler(btn_browsePopSource_Click);
			this.rbtn_other.Checked += new RoutedEventHandler(rbtn_other_Checked);
			this.rbtn_other.Unchecked += new RoutedEventHandler(rbtn_other_Unchecked);
			this.btn_doPop.Click += new RoutedEventHandler(btn_doPop_Click);
			this.btn_copyPop.Click += new RoutedEventHandler(btn_copyPop_Click);
			this.btn_start.Click += new RoutedEventHandler(btn_start_Click);
			this.btn_makeLink.Click += new RoutedEventHandler(btn_makeLink_Click);
			this.btn_reg.Click += new RoutedEventHandler(btn_reg_Click);
			this.btn_reindex0.Click += new RoutedEventHandler(btn_reindex0_Click);
			this.btn_quest.Click += new RoutedEventHandler(btn_quest_Click);
			this.btn_questInFolder.Click += new RoutedEventHandler(btn_questInFolder_Click);
			this.btn_makeBSL.Click += new RoutedEventHandler(btn_makeBSL_Click);
			this.btn_editBSL.Click += new RoutedEventHandler(btn_editBSL_Click);
			this.btn_checkBSL.Click += new RoutedEventHandler(btn_checkBSL_Click);
			this.btn_makeUSL.Click += new RoutedEventHandler(btn_makeUSL_Click);
			this.btn_editUSL.Click += new RoutedEventHandler(btn_editUSL_Click);
			this.btn_usr.Click += new RoutedEventHandler(btn_usr_Click);
			this.btn_usrInFolder.Click += new RoutedEventHandler(btn_usrInFolder_Click);
			this.btn_makePopBat.Click += new RoutedEventHandler(btn_makePopBat_Click);
			this.btn_makeTask.Click += new RoutedEventHandler(btn_makeTask_Click);
			this.btn_checkStat.Click += new RoutedEventHandler(btn_checkStat_Click);
			this.btn_makeStartKey.Click += new RoutedEventHandler(btn_makeStartKey_Click);
			this.btn_username.Click += new RoutedEventHandler(btn_username_Click);
			this.cb_selectAll.Checked += new RoutedEventHandler(cb_selectAll_Checked);
			this.cb_selectAll.Unchecked += new RoutedEventHandler(cb_selectAll_Unchecked);
			this.lb_list.SelectionChanged += new SelectionChangedEventHandler(lb_list_SelectionChanged);
			this.btn_reindex.Click += new RoutedEventHandler(btn_reindex_Click);
			this.btn_copyBase.Click += new RoutedEventHandler(btn_copyBase_Click);
			this.btn_refreshFolder.Click += new RoutedEventHandler(btn_refreshFolder_Click);
			
			this.btn_browseConsFolder.MouseEnter += new MouseEventHandler(btn_browseConsFolder_MouseEnter);
			this.btn_browsePopSource.MouseEnter += new MouseEventHandler(btn_browsePopSourse_MouseEnter);
			this.btn_checkBSL.MouseEnter += new MouseEventHandler(btn_checkBSL_MouseEnter);
			this.btn_checkStat.MouseEnter += new MouseEventHandler(btn_checkStat_MouseEnter);
			this.btn_copyBase.MouseEnter += new MouseEventHandler(btn_copyBase_MouseEnter);
			this.btn_copyPop.MouseEnter += new MouseEventHandler(btn_copyPop_MouseEnter);
			this.btn_doPop.MouseEnter += new MouseEventHandler(btn_doPop_MouseEnter);
			this.btn_editBSL.MouseEnter += new MouseEventHandler(btn_editBSL_MouseEnter);
			this.btn_editUSL.MouseEnter += new MouseEventHandler(btn_editUSL_MouseEnter);
			this.btn_makeBSL.MouseEnter += new MouseEventHandler(btn_makeBSL_MouseEnter);
			this.btn_makeLink.MouseEnter += new MouseEventHandler(btn_makeLink_MouseEnter);
			this.btn_makePopBat.MouseEnter += new MouseEventHandler(btn_makePopBat_MouseEnter);
			this.btn_makeStartKey.MouseEnter += new MouseEventHandler(btn_makeStartKey_MouseEnter);
			this.btn_makeTask.MouseEnter += new MouseEventHandler(btn_makeTask_MouseEnter);
			this.btn_makeUSL.MouseEnter += new MouseEventHandler(btn_makeUSL_MouseEnter);
			this.btn_openFolder.MouseEnter += new MouseEventHandler(btn_openFolder_MouseEnter);
			this.btn_quest.MouseEnter += new MouseEventHandler(btn_quest_MouseEnter);
			this.btn_questInFolder.MouseEnter += new MouseEventHandler(btn_questInFolder_MouseEnter);
			this.btn_refreshFolder.MouseEnter += new MouseEventHandler(btn_refreshFolder_MouseEnter);
			this.btn_reg.MouseEnter += new MouseEventHandler(btn_reg_MouseEnter);
			this.btn_reindex.MouseEnter += new MouseEventHandler(btn_reindex_MouseEnter);
			this.btn_reindex0.MouseEnter += new MouseEventHandler(btn_reindex0_MouseEnter);
			this.btn_start.MouseEnter += new MouseEventHandler(btn_start_MouseEnter);
			this.btn_username.MouseEnter += new MouseEventHandler(btn_username_MouseEnter);
			this.btn_usr.MouseEnter += new MouseEventHandler(btn_usr_MouseEnter);
			this.btn_usrInFolder.MouseEnter += new MouseEventHandler(btn_usrInFolder_MouseEnter);
		}
		
		private void btn_browseConsFolder_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Указать путь к файлу cons.exe";
		}
		
		private void btn_browsePopSourse_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Указать путь к папке с файлами пополнения";
		}
		
		private void btn_checkBSL_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Проверить baselist.cfg на наличие всех необходимых баз";
		}
		
		private void btn_checkStat_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Проверить статистику пополнения";
		}
		
		private void btn_copyBase_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Скопировать выбранные базы из указанной папки в папку BASE";
		}
		
		private void btn_copyPop_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Скопировать пополнение для выбранных баз из указанной папки в папку RECEIVE";
		}
		
		private void btn_doPop_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Запустить пополнение";
		}
		
		private void btn_editBSL_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Редактировать baselist.cfg";
		}
		
		private void btn_editUSL_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Редактировать userlist.cfg";
		}
		
		private void btn_makeBSL_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Создать файл baselist.cfg в папке BASE";
		}
		
		private void btn_makeLink_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Создать ярлык на рабочем столе";
		}
		
		private void btn_makePopBat_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Создать файл popolnenie_ip.bat для настройки пополнения через планировщик задач";
		}
		
		private void btn_makeStartKey_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Создать файл start.key";
		}
		
		private void btn_makeTask_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Создать задание в планировщике для автоматического пополнения";
		}
		
		private void btn_makeUSL_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Создать файл userlist.cfg в папке BASE";
		}
		
		private void btn_openFolder_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Открыть папку КонсультантПлюс";
		}
		
		private void btn_quest_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Сформировать запросы в папке SEND";
		}
		
		private void btn_questInFolder_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Сформировать запросы в другой папке";
		}
		
		private void btn_refreshFolder_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Обновить информацию об установленной системе";
		}
		
		private void btn_reg_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Зарегистрировать системы";
		}
		
		private void btn_reindex_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Переиндексировать указанную базу";
		}
		
		private void btn_reindex0_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Сформировать объединенные словари";
		}
		
		private void btn_start_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Запустить КонсультантПлюс";
		}
		
		private void btn_username_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Показать имя текущего пользователя";
		}
		
		private void btn_usr_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Сформировать user-файл в папке RECEIVE";
		}
		
		private void btn_usrInFolder_MouseEnter(Object sender, MouseEventArgs e)
		{
			this.txt_status.Text = "Сформировать user-файл в другой папке";
		}
		
		private void btn_refreshFolder_Click(Object sender, RoutedEventArgs e)
		{
			if (!File.Exists(this.txt_consFolder.Text))
			{
				Message.Show(Errorlevel.Warning, "Указана некорректная папка.");
				return;
			}
			FileInfo exe = new FileInfo(this.txt_consFolder.Text);
			this.consFolder = new ConsFolder(exe.DirectoryName);
			if (!consFolder.isValid)
			{
				hideSecondaryControls();
				this.txt_consFolder.Clear();
				string notFound = consFolder.Validate();
				Message.Show(Errorlevel.Warning, "В указанной папке не найдено:", notFound);
			}
			else
			{
				showSecondaryControls();
			}
		}
		
		private void btn_copyBase_Click(Object sender, RoutedEventArgs e)
		{
			if ((this.rbtn_other.IsChecked == false)
			    ||(!Directory.Exists(this.txt_popSource.Text)))
			{
				Message.Show(Errorlevel.Normal, "Укажите путь к папке, в которой лежат базы.");
				return;
			}
			List<string> basenames = new List<string>();
			Task task = new Task();
			ProcessWindow window = new ProcessWindow();
			if (this.rbtn_base.IsChecked == true)
			{
				foreach (string item in this.lb_list.SelectedItems)
				{
					basenames.Add(item.ToUpper());
				}
			}
			else if (this.rbtn_system.IsChecked == true)
			{
				foreach (string sysname in this.lb_list.SelectedItems)
				{
					ConsSystem sys = fullSystemList.GetSystemByName(sysname);
					foreach (ConsBase sysitem in sys.baselist)
					{
						if (!basenames.Contains(sysitem.shortName.ToUpper()))
						{
							basenames.Add(sysitem.shortName.ToUpper());
						}
					}
				}
			}
			if (basenames.Count == 0) return;
			task.source = this.txt_popSource.Text;
			task.destination = this.consFolder.baseFolder.FullName;
			task.basenames = basenames;
			window.Show();
			window.CopyBase(task);
		}
		
		private void lb_list_SelectionChanged(Object sender, SelectionChangedEventArgs e)
		{
			if (this.lb_list.SelectedItems.Count == 0) this.lb_list.SelectedIndex = 0;
			
			if (this.rbtn_system.IsChecked == true)
			{
				CountSystems();
			}
			
			this.txt_status.Text = "Выбрано " + this.lb_list.SelectedItems.Count + " элементов.";
		}
		
		private void CountSystems()
		{
			int baseDefaultCount = 0;
			int baseFoundCount = 0;
			List<string> basesByDefault = new List<string>();
			foreach (string item in this.lb_list.SelectedItems)
			{
				ConsSystem conssystem = fullSystemList.GetSystemByName(item);
				foreach (ConsBase consbase in conssystem.baselist)
				{
					basesByDefault.Add(consbase.shortName);
				}
			}
			List<string> distinctBasesByDefault = basesByDefault.Distinct().ToList();
			baseDefaultCount = distinctBasesByDefault.Count;
			foreach (string basename in distinctBasesByDefault)
			{
				if (this.consFolder.baseList.Contains(basename.ToUpper())) baseFoundCount++;
			}
			this.tb_foundCount.Text = "Найдено систем: "
				+ this.consFolder.systemList.Count				
				+ ".\nБаз в выбранных системах: "
				+ baseDefaultCount
				+ ".\nИз них найдено: "
				+ baseFoundCount;
		}
		
		private void CountBases()
		{
			this.tb_foundCount.Text = "Найдено баз: " + this.consFolder.baseList.Count;
		}
		
		private void cb_selectAll_Unchecked(Object sender, RoutedEventArgs e)
		{
			this.lb_list.UnselectAll();
			this.lb_list.IsEnabled = true;
		}
		
		private void cb_selectAll_Checked(Object sender, RoutedEventArgs e)
		{
			this.lb_list.SelectAll();
			this.lb_list.IsEnabled = false;
		}
		
		private void btn_username_Click(Object sender, RoutedEventArgs e)
		{
			Helper.ShowNetworkName();
		}
		
		private void btn_makeStartKey_Click(Object sender, RoutedEventArgs e)
		{
			KeySelectionWindow window = new KeySelectionWindow();
			window.ShowDialog();
			if (window.DialogResult == true)
			{
				consFolder.MakeStartKey(window.GetStartKeys());
			}
		}
		
		private void btn_checkStat_Click(Object sender, RoutedEventArgs e)
		{
			this.txt_status.Text = "Идет проверка статистики...";
			consFolder.CheckUpdate(ref this.pb_status);
		}
		
		private void btn_makeTask_Click(Object sender, RoutedEventArgs e)
		{
			NewTaskWindow window = new NewTaskWindow(this.consFolder);
			window.ShowDialog();
		}
		
		private void btn_makePopBat_Click(Object sender, RoutedEventArgs e)
		{
			PopBatWindow window = new PopBatWindow(this.consFolder);
			window.ShowDialog();
		}
		
		private void btn_usrInFolder_Click(Object sender, RoutedEventArgs e)
		{
			InputWindow input = new InputWindow("Создание user-файла", 
			                                    "Укажите директорию, в которой необходимо создать файл", 
			                                    true);
			if ((input.ShowDialog() == true)&&(Directory.Exists(input.Result)))
				consFolder.User(input.Result);
		}
		
		private void btn_usr_Click(Object sender, RoutedEventArgs e)
		{
			consFolder.User();
		}
		
		private void btn_editUSL_Click(Object sender, RoutedEventArgs e)
		{
			consFolder.EditList(consFolder.userListPath);
		}
		
		private void btn_makeUSL_Click(Object sender, RoutedEventArgs e)
		{
			consFolder.MakeList(consFolder.userListPath);
		}
		
		private void btn_checkBSL_Click(Object sender, RoutedEventArgs e)
		{
			List<string> result = consFolder.ValidateBaselist();
			if (result.Count == 0) 
			{
				Message.Show(Errorlevel.Normal, "Все системы в порядке");
			}
			else
			{
				string message = "Не найдены следующие базы или они закомментированы: ";
				result.Insert(0, message);
				Message.Show(Errorlevel.Error, result.ToArray());
			}
		}
		
		private void btn_editBSL_Click(Object sender, RoutedEventArgs e)
		{
			consFolder.EditList(consFolder.baseListPath);
		}
		
		private void btn_makeBSL_Click(Object sender, RoutedEventArgs e)
		{
			consFolder.MakeList(consFolder.baseListPath);
		}
		
		private void btn_quest_Click(Object sender, RoutedEventArgs e)
		{
			if (!IsValidSelection()) return;
			
			if (this.cb_selectAll.IsChecked == true)
			{
				consFolder.Quest();
			}
			else
			{
				ConsBase selectedBase = fullBaseList.GetBaseByName(this.lb_list.SelectedItem.ToString());
				consFolder.Quest(selectedBase);
			}
		}
		
		private void btn_questInFolder_Click(Object sender, RoutedEventArgs e)
		{
			if (!IsValidSelection()) return;
			
			InputWindow input = new InputWindow("Формирование запросов", 
			                                    "Укажите папку для формирования запросов",
			                                    true);
			if ((input.ShowDialog() != true)||(!Directory.Exists(input.Result))) return;
			if (this.cb_selectAll.IsChecked == true)
			{
				consFolder.Quest(input.Result);
			}
			else
			{
				ConsBase selectedBase = fullBaseList.GetBaseByName(this.lb_list.SelectedItem.ToString());
				consFolder.Quest(input.Result, selectedBase);
			}
		}
		
		private void btn_reindex0_Click(Object sender, RoutedEventArgs e)
		{
			consFolder.Reindex0();
		}
		
		private void btn_reindex_Click(Object sender, RoutedEventArgs e)
		{
			if (this.cb_selectAll.IsChecked == true)
			{
				consFolder.Reindex();
			}
			else
			{
				ConsBase selectedBase = fullBaseList.GetBaseByName(this.lb_list.SelectedItem.ToString());
				consFolder.Reindex(selectedBase);
			}
		}
		
		private void btn_reg_Click(Object sender, RoutedEventArgs e)
		{
			consFolder.Reg();
		}
		
		private void btn_makeLink_Click(Object sender, RoutedEventArgs e)
		{
			consFolder.Group();
		}
		
		private void btn_start_Click(Object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show("Запустить с правами администратора?",
			                "Запуск Консультант Плюс",
			                MessageBoxButton.YesNo);
			if (result == MessageBoxResult.Yes)
			{
				consFolder.StartProgram(true);
			}
			else
			{
				consFolder.StartProgram();
			}
		}
		
		private void btn_copyPop_Click(Object sender, RoutedEventArgs e)
		{
			if ((this.rbtn_other.IsChecked == false)
			    ||(!Directory.Exists(this.txt_popSource.Text)))
			{
				Message.Show(Errorlevel.Normal, "Укажите путь к папке с файлами пополнения.");
				return;
			}
			List<string> basenames = new List<string>();
			if (this.rbtn_base.IsChecked == true)
			{
				foreach (string name in this.lb_list.SelectedItems)
				{
					basenames.Add(name);
				}
			}
			else if (this.rbtn_system.IsChecked == true)
			{
				foreach (string systemname in this.lb_list.SelectedItems)
				{
					foreach (ConsBase consBase in fullSystemList.GetSystemByName(systemname).baselist)
					{
						basenames.Add(consBase.shortName);
					}
				}
			}
			if (basenames.Count == 0) return;
			Task task;
			task.source = this.txt_popSource.Text;
			task.destination = this.consFolder.receive.FullName;
			task.basenames = basenames;
			ProcessWindow pWindow = new ProcessWindow();
			pWindow.Show();
			pWindow.CopyAns(task);
		}
		
		private bool IsValidSelection()
		{
			if ((this.rbtn_system.IsChecked == true)
			    ||((this.lb_list.SelectedItems.Count > 1)
			       &&(this.lb_list.SelectedItems.Count < this.lb_list.Items.Count)))
			{
				Message.Show(Errorlevel.Normal, "Для этой операции необходимо выбрать базу." +
				             "\nПереключитесь на список баз и выберите" +
				             "\nлибо одну базу, либо все базы.");
				return false;
			}
			return true;
		}
		
		private void btn_doPop_Click(Object sender, RoutedEventArgs e)
		{
			if (!IsValidSelection()) return;
			
			if (this.lb_list.SelectedItems.Count == this.lb_list.Items.Count)
			{
				if (this.rbtn_int.IsChecked == true) consFolder.ReceiveInet();
				if (this.rbtn_rec.IsChecked == true) consFolder.Receive();
				if ((this.rbtn_other.IsChecked.Value == true)&&(Directory.Exists(this.txt_popSource.Text)))
					consFolder.ReceiveFromFolder(this.txt_popSource.Text);
			}
			else
			{
				ConsBase selectedBase = Helper.GetBaseList().GetBaseByName(this.lb_list.SelectedItem.ToString());
				if (this.rbtn_int.IsChecked == true) consFolder.ReceiveInet(selectedBase);
				if (this.rbtn_rec.IsChecked == true) consFolder.Receive(selectedBase);
				if ((this.rbtn_other.IsChecked.Value == true)&&(Directory.Exists(this.txt_popSource.Text)))
					consFolder.ReceiveFromFolder(selectedBase, this.txt_popSource.Text);
			}
		}
		
		private void rbtn_other_Checked(Object sender, RoutedEventArgs e)
		{
			this.btn_browsePopSource.Visibility = Visibility.Visible;
			this.txt_popSource.Visibility = Visibility.Visible;
			this.btn_copyPop.Visibility = Visibility.Visible;
			this.btn_copyBase.Visibility = Visibility.Visible;
		}
		
		private void rbtn_other_Unchecked(Object sender, RoutedEventArgs e)
		{
			this.btn_browsePopSource.Visibility = Visibility.Collapsed;
			this.txt_popSource.Visibility = Visibility.Collapsed;
			this.btn_copyPop.Visibility = Visibility.Collapsed;
			this.btn_copyBase.Visibility = Visibility.Collapsed;
		}
		private void btn_browsePopSource_Click(Object sender, RoutedEventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
			dialog.ShowDialog();
			this.txt_popSource.Text = dialog.SelectedPath;
		}
		
		private void rbtn_base_Checked(Object sender, RoutedEventArgs e)
		{
			this.lb_list.Items.Clear();
			foreach (string item in consFolder.baseList)
			{
				this.lb_list.Items.Add(item);
			}
			CountBases();
		}
		
		private void rbtn_system_Checked(Object sender, RoutedEventArgs e)
		{
			this.lb_list.Items.Clear();
			foreach (string item in consFolder.systemList)
			{
				this.lb_list.Items.Add(item);
			}
			CountSystems();
		}
		
		private void btn_browseConsFolder_Click(Object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "Исполняемый файл К+(cons.exe)|cons.exe;*.lnk|Все файлы(*.*)|*.*";
			dialog.DereferenceLinks = true;
			dialog.ShowDialog();
			FileInfo exe;
			try 
			{
				exe = new FileInfo(dialog.FileName);
			}
			catch
			{
				hideSecondaryControls();
				this.txt_consFolder.Clear();
				return;
			}
			if (exe.Name.ToLower() != "cons.exe")
			{
				Message.Show(Errorlevel.Normal, "Укажите путь к файлу cons.exe");
				hideSecondaryControls();
				this.txt_consFolder.Clear();
				return;
			}
			this.txt_consFolder.Text = exe.FullName;
			this.consFolder = new ConsFolder(exe.DirectoryName);
			if (!consFolder.isValid)
			{
				hideSecondaryControls();
				this.txt_consFolder.Clear();
				string notFound = consFolder.Validate();
				Message.Show(Errorlevel.Warning, "Указана некорректная папка! Не найдено:", notFound);
			}
			else
			{
				showSecondaryControls();
			}
		}
		
		private void btn_openFolder_Click(Object sender, RoutedEventArgs e)
		{
			if ((consFolder == null)||(!consFolder.isValid)) return;
			consFolder.OpenRootFolder();
		}
	}
}