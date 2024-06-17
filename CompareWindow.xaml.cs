

using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace FileComparer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CompareWindow : Window
    {
        private string File1path = "";
        private string File2path = "";
        
        public CompareWindow()
        {
            InitializeComponent();
        }
        public CompareWindow(string name,string session):this()
        {
            LoadSession(name,session);
          
        }
      
        private void LoadSession(string name,string sessionData)
        {
            string[] names = name.Split(new[] { '=' });
            path1.Text = names[0];
            path2.Text = names[1];

            string[] parts = sessionData.Split(new[] {'~'});
            if (parts.Length == 2)
            {
                TextBox1.AppendText(parts[0]);
                TextBox2.AppendText(parts[1]);
            }
        }
        private void save(object o, RoutedEventArgs e)
        {
            SaveSession();
        }


        private void SaveSession()
        {
            TextRange r1 = new TextRange(TextBox1.Document.ContentStart, TextBox1.Document.ContentEnd);
            TextRange r2 = new TextRange(TextBox2.Document.ContentStart, TextBox2.Document.ContentEnd);
           string filetext1 = r1.Text;
            string filetext2 = r2.Text;
            string sessionName = $"{path1.Text}={path2.Text}";
            string session = $"{filetext1}~{filetext2}";
           
            SessionManager.AddSession(sessionName,session);
        }
        private void Browse_ButtonClick1(object sender, RoutedEventArgs e)
        {

            OpenFileDialog o = new OpenFileDialog();
            if (o.ShowDialog() == true)
            {

                File1path = o.FileName;
                path1.Text = File1path;
                string filecontent = File.ReadAllText(File1path);
                TextBox1.Document.Blocks.Clear();
                TextBox1.AppendText(filecontent);
                
            }
        }
        private void Browse_ButtonClick2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            if (o.ShowDialog() == true)
            {
                File2path = o.FileName;
                path2.Text = File2path;
                string filecontent = File.ReadAllText(File2path);
                TextBox2.Document.Blocks.Clear();
                TextBox2.AppendText(filecontent);
              
            }

        }
        
        private void compareFiles(Object sender, RoutedEventArgs e)
        {
            HighlightDifference(TextBox1, TextBox2);
        }

        private void HighlightDifference(RichTextBox textBox1, RichTextBox textBox2)
        {
            TextRange range1 = new TextRange(textBox1.Document.ContentStart, textBox1.Document.ContentEnd);
            TextRange range2 = new TextRange(textBox2.Document.ContentStart, textBox2.Document.ContentEnd);
            string text1 = range1.Text;
            string text2 = range2.Text;
           
            textBox1.Document.Blocks.Clear();
            textBox2.Document.Blocks.Clear();

            string[] lines1 = text1.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string[] lines2 = text2.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < Math.Max(lines1.Length, lines2.Length); i++)
            {
                Paragraph paragraph1 = new Paragraph();
                Paragraph paragraph2 = new Paragraph();

                if (i < lines1.Length && i < lines2.Length)
                {
                    string[] words1 = lines1[i].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] words2 = lines2[i].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    int j = 0;
                    while (j < Math.Max(words1.Length, words2.Length))
                    {
                        if (j < words1.Length && j < words2.Length && words1[j] == words2[j])
                        {
                            paragraph1.Inlines.Add(new Run(words1[j] + " "));
                            paragraph2.Inlines.Add(new Run(words2[j] + " "));

                        }
                        else
                        {
                            Run run1 = new Run(j < words1.Length ? words1[j] : "");
                            Run run2 = new Run(j < words2.Length ? words2[j] : "");

                            // Highlight differences
                            run1.Background = System.Windows.Media.Brushes.Pink;
                            run2.Background = System.Windows.Media.Brushes.Pink;


                            paragraph1.Inlines.Add(run1);
                            paragraph2.Inlines.Add(run2);
                        }
                        j++;
                    }
                }

                 textBox1.Document.Blocks.Add(paragraph1);
                 textBox2.Document.Blocks.Add(paragraph2);
                
            }
        }
        private void leftmerge(Object o,RoutedEventArgs e)
        {
            string previous = File.ReadAllText(File1path);
            string content=File.ReadAllText(File2path);
            string[] removeEmptylines=previous.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            string newcontent=string.Join(Environment.NewLine, removeEmptylines);
            string merge=newcontent+'\n'+content;
            TextBox1.Document.Blocks.Clear();
            File.WriteAllText(File1path,merge);
            string mergecontent = File.ReadAllText(File1path);
            TextBox1.AppendText(mergecontent);
            }
        private void rightmerge(object o,RoutedEventArgs e)
        {
            string content=File.ReadAllText(File1path);
            string previous=File.ReadAllText(File2path);
            string[] removeEmptylines=previous.Split(new string[] { Environment.NewLine },StringSplitOptions.RemoveEmptyEntries );
            string newcontent=string.Join(Environment.NewLine, removeEmptylines);
            TextBox2.Document.Blocks.Clear();
            string merge = newcontent+'\n'+content;
          
            using(StreamWriter writer=new StreamWriter(File2path))
            {

                    writer.Write(merge);
    
            }

            string mergecontent=File.ReadAllText(File2path);
            TextBox2.AppendText(mergecontent);
            
        
        }
      
        private void UndoButton(object o, RoutedEventArgs e)
        {
            TextBox1.Undo();
            TextBox2.Undo();
        }
        private void RedoButton(object o, RoutedEventArgs e)
        {
            TextBox1.Redo();
            TextBox2.Redo();
        }
        private void exitclick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Home w = new Home();
            w.Show();
        }


        private void NavigatetoMain(object sender, RoutedEventArgs e)
        {
            // Handle Exit menu item click
            this.Hide();
            Home w = new Home();
            w.Show();
        }
    }

}
    
