using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DirTree.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            SelectSourceDirCommand = new RelayCommand(SelectSourceDir);
            GoCommand = new RelayCommand(Work);
            SaveCommand = new RelayCommand(Save);
            UseUnicode = true;
        }


        private string sourcedir;

        public string Sourcedir
        {
            get { return sourcedir; }
            set
            {
                sourcedir = value;
                this.RaisePropertyChanged("Sourcedir");
            }
        }

        private string result;

        public string Result
        {
            get { return result; }
            set
            {
                result = value;
                this.RaisePropertyChanged("Result");
            }
        }

        private bool useUnicode;

        public bool UseUnicode
        {
            get { return useUnicode; }
            set
            {
                useUnicode = value;
                this.RaisePropertyChanged("UseUnicode");
            }
        }


        public RelayCommand SelectSourceDirCommand { get; set; }
        public RelayCommand GoCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }


        private void SelectSourceDir()
        {
            VistaFolderBrowserDialog d = new VistaFolderBrowserDialog();
            if (d.ShowDialog() == true)
            {
                Sourcedir = d.SelectedPath;
            }
        }

        private string tabs = "";
        private string res = "";
        private void Work()
        {
            try
            {
                res = "";
                EnumDir(Sourcedir);
                Result = res;
            }
            catch (Exception e)
            {
                result = "操作过程中发生错误：\n" + e.Message;
            }
        }

        private void EnumDir(string dir)
        {
            res += ((dir==Sourcedir)? dir:(new DirectoryInfo(dir).Name)) + "\n";
            var files = System.IO.Directory.EnumerateFiles(dir);
            var dirs = System.IO.Directory.EnumerateDirectories(dir);
            foreach (string i in files)
            {
                if (files.Last() == i && dirs.Count() == 0)
                {
                    res += tabs + " └ " + new FileInfo(i).Name + "\n";
                }
                else
                {
                    res += tabs + " │ " + new FileInfo(i).Name + "\n";
                }
            }
            foreach (string i in dirs)
            {
                if (dirs.Last() == i)
                {
                    res += tabs + " └ ";
                    tabs = tabs + "　　";
                    EnumDir(i);
                    tabs = tabs.Substring(0, tabs.Length - 2);
                }
                else
                {
                    res += tabs + " ├ ";
                    tabs += " │ ";
                    EnumDir(i);
                    tabs = tabs.Substring(0, tabs.Length - 3);
                }
            }
        }
        private void Save()
        {
            Microsoft.Win32.SaveFileDialog d = new Microsoft.Win32.SaveFileDialog();
            d.DefaultExt = ".txt";
            d.Filter = "文本文档|*.txt";

            if (d.ShowDialog() == true)
            {
                StreamWriter fs = new StreamWriter(d.FileName, false,UseUnicode? Encoding.Unicode:Encoding.ASCII);
                fs.Write(Result);
                fs.Flush();
                fs.Close();
            }
        }
    }
}
