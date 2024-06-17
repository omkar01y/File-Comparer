

using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace FileComparer
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Syncfolder : Window
    {
        private string folder1path = "";
        private string folder2path = "";


        public Syncfolder()
        {
            InitializeComponent();

        }
        private void Browse_Folder1(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog o = new OpenFolderDialog();
            if (o.ShowDialog() == true)
            {
                folder1path = o.FolderName;
                Path1.Text = folder1path;
               displayroot(folder1path,folder1view);
            }
        }
        private void Browse_Folder(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog o = new OpenFolderDialog();
            if (o.ShowDialog() == true)
            {
                folder2path = o.FolderName;
                Path2.Text = folder2path;
            
                displayroot(folder2path,folder2view);
            }
        }
        private void foldersync(Object o,RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(folder1path) && !string.IsNullOrEmpty(folder2path))
            { 
                 DirectoryInfo source=new DirectoryInfo(folder1path);
                 DirectoryInfo destination=new DirectoryInfo(folder2path);
                syncContent(source, destination);
                syncContent(destination, source);
                displayroot(folder1path,folder1view);
                displayroot(folder2path,folder2view);
            }
        }
        private void syncContent(DirectoryInfo source, DirectoryInfo destination)
        {

            foreach(FileInfo file in source.GetFiles())
            {
                string destinationFilePath = Path.Combine(destination.FullName, file.Name);
                if(!File.Exists(destinationFilePath) || file.LastWriteTime > File.GetLastWriteTime(destinationFilePath))
                {
                    file.CopyTo(destinationFilePath);
                }
            }
            //recursively iterate sub directories
            foreach(DirectoryInfo subdir in source.GetDirectories())
            {
                DirectoryInfo dir = new DirectoryInfo(Path.Combine(destination.FullName,subdir.Name));
                if(!dir.Exists)
                {
                    dir.Create();   
                }
                syncContent(subdir, dir);
            }

        }
        private void displayroot(string folderpath,TreeView folderview)
        {
            folderview.Items.Clear();
            TreeViewItem rootnode = new TreeViewItem();
            rootnode.Header= folderpath;
            folderview.Items.Add(rootnode);
            DisplayDirectory(folderpath, rootnode);

        }
         private void DisplayDirectory(string folderpath,TreeViewItem root)
         {

            if (Directory.Exists(folderpath))
             {

                 DirectoryInfo rootDirectory = new DirectoryInfo(folderpath);
                 foreach (var directory in rootDirectory.GetDirectories())
                 {
                     TreeViewItem directoryItem = new TreeViewItem();
                     directoryItem.Header = directory.FullName;
                    root.Items.Add(directoryItem);
                     DisplayDirectory(directory.FullName , directoryItem);
                 }
                 foreach(var files in rootDirectory.GetFiles())
                {
                    TreeViewItem fileitem=new TreeViewItem();
                    fileitem.Header = files.FullName;
                    root.Items.Add(fileitem);
                }
             }
             else
             {
                 MessageBox.Show("doesnot exist");
             }

         }
        private void display(string folderpath,ListBox box)
        {
            string[] files = Directory.GetFiles(folderpath, "*.*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                 box.Items.Add(file);
            }

        }
        private void exitmenu(Object o, RoutedEventArgs e)
        {
            this.Hide();
            Home w = new Home();
            w.Show();

        }
    }
   
}
