using System.Collections.Generic;

namespace WatchFiles.ViewModel
{
    public partial class MainWindowModel
    {

        private int tabIndex = 0;
        /// <summary>
        /// 顶部Tab索引
        /// </summary>
        public int TabIndex
        {
            get { return tabIndex; }
            set
            {
                tabIndex = value;
                RaisePropertyChanged("TabIndex");
            }
        }

        private List<FileTreeModel> _fileItem = new List<FileTreeModel>();
        public List<FileTreeModel> FileItem
        {
            get { return _fileItem; }
            set
            {
                _fileItem = value;
                RaisePropertyChanged("FileItem");
            }
        }
        /// <summary>
        /// 选中文件名称
        /// </summary>
        private string _selectFileleName = string.Empty;
        public string SelectFileleName
        {
            get { return _selectFileleName; }
            set
            {
                _selectFileleName = value;
                RaisePropertyChanged("SelectFileleName");
            }
        }

    }


    /// <summary>
    /// 文件树
    /// </summary>
    public class FileTreeModel
    {
        public FileTreeModel()
        {
            Id = 0;
            FileType = (int)ViewModel.FileType.Folder;
            Subitem = new List<FileTreeModel>();
        }
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 父类id
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public int FileType { get; set; }

        /// <summary>
        /// 文件图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 子项
        /// </summary>
        public List<FileTreeModel> Subitem { get; set; }

        /// <summary>
        /// 子项统计
        /// </summary>
        public string SubitemCount { get; set; }
    }
}
