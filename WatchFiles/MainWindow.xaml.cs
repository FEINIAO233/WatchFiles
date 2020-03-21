using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WatchFiles.ViewModel;

namespace WatchFiles
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MainWindowViewModel viewModel = new MainWindowViewModel();
        List<FileTreeModel> fileTreeData = null;
        public MainWindow()
        {
            InitializeComponent();
            string dataDir = AppDomain.CurrentDomain.BaseDirectory + "Files\\";
            fileTreeData = GetAllFiles(new DirectoryInfo(dataDir)).OrderByDescending(s => s.FileName).ToList();
            departmentTree.ItemsSource = fileTreeData;
        }
        /// <summary>
        /// 获取目录集合
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public List<FileTreeModel> GetAllFiles(DirectoryInfo dir)
        {
            List<FileTreeModel> FileList = new List<FileTreeModel>();
            var fileinfo = dir.GetFileSystemInfos();
            foreach (FileSystemInfo i in fileinfo)
            {
                if (i is DirectoryInfo)
                {
                    FileTreeModel fileDir = new FileTreeModel()
                    {
                        FileName = i.Name,
                        FilePath = i.FullName,
                        FileType = (int)FileType.Folder,
                        Icon = "../refresh/folder.ico"
                    };
                    fileDir.Subitem = GetAllFiles((DirectoryInfo)i);
                    FileList.Add(fileDir);
                }
                if (i is FileInfo)
                {
                    if (i.Extension.Equals(".png") || i.Extension.Equals(".jpg") || i.Extension.Equals(".bmp") || i.Extension.Equals(".gif"))
                    {
                        FileList.Add(new FileTreeModel()
                        {
                            FileName = i.Name,
                            FilePath = i.FullName,
                            FileType = (int)FileType.Picture,
                            Icon = "../refresh/picture.ico"
                        });
                    }
                    if (i.Extension.Equals(".txt"))
                    {
                        FileList.Add(new FileTreeModel()
                        {
                            FileName = i.Name,
                            FilePath = i.FullName,
                            FileType = (int)FileType.Text,
                            Icon = "../refresh/Text.ico"
                        });
                    }
                }

            }
            return FileList;
        }
        /// <summary>
        /// Tab选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl && e.AddedItems != null && e.AddedItems.Count > 0 && e.AddedItems[0] is TabItem)
            {
                TabItem tabItem = e.AddedItems[0] as TabItem;
                if (tabItem.Header.ToString() == "文件预览")
                {
                    string dataDir = AppDomain.CurrentDomain.BaseDirectory + "Files\\";
                    fileTreeData = GetAllFiles(new DirectoryInfo(dataDir)).OrderByDescending(s => s.FileName).ToList();
                    departmentTree.ItemsSource = fileTreeData;
                }
            }
        }

        /// <summary>
        /// 文件树选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DepartmentTree_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (departmentTree.SelectedItem != null)
                {
                    FileTreeModel selectedTnh = departmentTree.SelectedItem as FileTreeModel;
                    viewModel.Model.SelectFileleName = selectedTnh.FileName;
                    ShowContent.Document.Blocks.Clear();

                    if (selectedTnh.FileType == (int)FileType.Picture)
                    {
                        BitmapImage imagesouce = new BitmapImage(new Uri(selectedTnh.FilePath));//Uri("图片路径")
                        //将剪贴板中的内容贴入RichTextBox中
                        Clipboard.Clear();   //清空剪贴板
                        Clipboard.SetImage(imagesouce);  //将Bitmap类对象写入剪贴板
                        ShowContent.Paste();
                    }
                    if (selectedTnh.FileType == (int)FileType.Text)
                    {
                        // 获取文件对象
                        FileStream file = File.OpenRead(selectedTnh.FilePath);

                        // 创建读取流,默认字符编码
                        StreamReader reader = new StreamReader(file, Encoding.Default);

                        string line = null;
                        while ((line = reader.ReadLine()) != null)
                        {
                            // txtBox 为RichTextBox控件名
                            ShowContent.AppendText(line + "\r\n");
                        }

                        // 关闭资源
                        reader.Close();
                        file.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 绑定数据源
            DataContext = viewModel;
        }


    }
}
