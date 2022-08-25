using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace UnTranslated.ViewModels
{
    [INotifyPropertyChanged]
    internal partial class MainViewModel
    {

        public static string BackupPath = $@"{Environment.SpecialFolder.ApplicationData}\Untranslated";
        public MainViewModel()
        {
        }

        public void LoadTranslatioins()
        {
            // Добавляем backup
            Translations = new ObservableCollection<IAssets>();
            if (!Directory.Exists(BackupPath))
            {
                Directory.CreateDirectory($@"{BackupPath}");
                Assets.CopyAll($@"{GameFolder}\Assets", $@"{BackupPath}");// loading assets from game
            }
            Translations.Add(new Translation("Original", BackupPath, true));

            // Проверяем наличие папки переводов
            var tfolder = $@"{GameFolder}\Translations";
            if (Directory.Exists(tfolder))
                Directory.CreateDirectory(tfolder);

            // Добавляем остальные переводы
            var trs = Directory.GetDirectories(tfolder);
            foreach (var tr in trs)
                Translations.Add(new Translation(Path.GetFileName(tr), tr));
            foreach (var tr in Translations)
                if (!File.Exists($@"{tr.Path}\encoding.txt"))
                    File.Create($@"{tr.Path}\encoding.txt");
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(GameFolderIsCorrect))]
        string gamePath;
        string GameFolder => Path.GetDirectoryName(gamePath);
        public bool GameFolderIsCorrect
        {
            get
            {
                var gf = GameFolder; ;
                if (!Directory.Exists(gf)) return false;
                if (!File.Exists($@"{gf}\Unexplored.vshost.exe")) return false;
                if (!Directory.Exists($@"{gf}\Assets")) return false;
                LoadTranslatioins();
                return true;
            }
        }

        [ObservableProperty]
        Translation selectedTranslation;

        [RelayCommand]
        private void Launch()
        {
            Process.Start(GamePath);
        }

        [ObservableProperty]
        private ObservableCollection<IAssets> translations = new ObservableCollection<IAssets>();

        [RelayCommand]
        public void Open(Translation tr)
        {
            Process.Start("explorer.exe", Path.GetFullPath(tr.Path));
        }

        [RelayCommand]
        public void Load(Translation tr)
        {
            Assets.LoadEncodingMap($@"{tr.Path}\encoding.txt");
            Assets.CopyAll(tr.Path, $@"{GameFolder}\Assets"); // loading assets to game
            Launch();
        }

        [RelayCommand]
        public void Delete(Translation tr)
        {
            Directory.Delete(tr.Path, true);
            Translations.Remove(tr);
        }

        [RelayCommand]
        public void New(string name)
        {
            if (!String.IsNullOrWhiteSpace(name))
            {
                Directory.CreateDirectory($@"{GameFolder}\Translations\{name}");
                File.Create($@"{GameFolder}\Translations\encoding.exe");
                var tr = new Translation(name, Path.GetFullPath($@"{GameFolder}\Translations\" + name));
                Assets.CopyAll(BackupPath, tr.Path);// loading assets from backup
                Translations.Add(tr);
            }
        }
    }

    public static class Assets
    {
        public static bool CopyAll(string from, string to)
        {
            try
            {
                Directory.CreateDirectory($@"{to}\Sprites\Fonts");
                var fonts = Directory.GetFiles($@"{from}\Sprites\Fonts").Select(x => new FileInfo(x));
                foreach (var font in fonts)
                    File.Copy(font.FullName, $@"{to}\Sprites\Fonts\{font.Name}", true);

                Directory.CreateDirectory($@"{to}\Language");
                var langs = Directory.GetFiles($@"{from}\Language").Select(x => new FileInfo(x)); 
                foreach (var lang in langs)
                    CopyWithReplace(lang.FullName, $@"{to}\Language\{lang.Name}");
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public static bool LoadEncodingMap(string path)
        {
            try
            {
                using var file = new StreamReader(path);
                var rules = new Dictionary<string, string>();
                while (!file.EndOfStream)
                {
                    var line = file.ReadLine();
                    var rule = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    rules[rule[0]] = rule[1];
                }
                EncodingMap = rules;
            }
            catch (Exception ex)
            {
                return false;
                EncodingMap = new Dictionary<string, string>();
            }
            return true;
        }

        static Dictionary<string, string> EncodingMap { get; set; } = new Dictionary<string, string>();

        public static void CopyWithReplace(string from, string to)
        {
            using StreamReader input = new StreamReader(from);
            using StreamWriter output = new StreamWriter(to, false);
            var text = input.ReadToEnd();
            foreach (var rule in EncodingMap)
                text = text.Replace(rule.Key, rule.Value);
            output.Write(text);
        }


    }
}
