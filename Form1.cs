using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SoundModBuilder
{
    public partial class Form1 : Form
    {
        private readonly VOProject prj;
        private readonly List<bool> EventsListColors = new List<bool>();
        public readonly List<Commander> CommanderList = new List<Commander>();
        private bool bProjectChanged = false;
        public bool ProjectChanged { get => bProjectChanged; set { bProjectChanged = value; UpdateStrip(); } }
        private readonly string converter_folder = "./utils/converter_wem_to_mp3";
        public Form1()
        {
            InitializeComponent();
            string CurrentAppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string Ver = Properties.Settings.Default.AppVersion;
            if (Ver != CurrentAppVersion)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.AppVersion = CurrentAppVersion;
                Properties.Settings.Default.Save();
            }
            Utils.UpdateGamePath();
            Utils.UpdateGameVersion();
            InitCommandersList();
            prj = new VOProject();
            UpdateStrip();
            BuildToolStripMenuItem.Enabled = false;
            foreach (var p in from MKEvent evt in prj.EventsSFX
                              from MKState p in evt.Paths
                              select p)
            {
                listBoxEvents.Items.Add(p.ReadRu);
                EventsListColors.Add(false);
            }

            UpdateMenuItem();
            menuStrip1.Renderer = new MenuStripRenderer();
        }

        private void OpenSettingsWindow()
        {
            SettingsWindow sw = new SettingsWindow() { parent = this };
            if (DialogResult.OK == sw.ShowDialog(this))
            {
                InitCommandersList();
                UpdateMenuItem();
                listBoxEvents.Update();
                AdjustSplitterDistance();
            }
        }

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSettingsWindow();
        }

        private void UpdateMenuItem()
        {
            BuildToolStripMenuItem.Enabled = Directory.Exists(prj.SrcPath)
                && Directory.Exists(Properties.Settings.Default.GamePath)
                && File.Exists(Properties.Settings.Default.WwisePath)
                && File.Exists(Properties.Settings.Default.WwiseProject);
        }

        public class MKEvent
        {
            public string Name { get; set; }
            public string ExtId { get; set; }

            public List<MKState> Paths;
            internal MKEvent(XmlNode xnode)
            {
                Paths = new List<MKState>();
                foreach (XmlNode p in xnode.ChildNodes) // path
                {
                    if (p.NodeType != XmlNodeType.Comment)
                        Paths.Add(new MKState(p));
                }
            }

            internal bool Dump(XmlNode xnode, string CommanderID, string mod_path)
            {
                bool skip = true;
                foreach (MKState s in Paths)
                    if (s.FileList.Count > 0)
                    { skip = false; break; }
                if (skip) return true;
                XmlDocument xdoc = xnode.OwnerDocument;
                XmlNode ext = xnode.AppendChild(xdoc.CreateElement("ExternalEvent"));
                ext.AppendChild(xdoc.CreateElement("Name")).InnerText = Name;
                XmlNode xcont = ext.AppendChild(xdoc.CreateElement("Container"));
                xcont.AppendChild(xdoc.CreateElement("Name")).InnerText = "Voice";
                xcont.AppendChild(xdoc.CreateElement("ExternalId")).InnerText = ExtId;
                foreach (MKState s in Paths)
                    if (s.FileList.Count > 0)
                        if (!s.Dump(xcont, CommanderID, mod_path)) return false;
                return true;
            }

            internal void Load(XmlNode xnode)
            {
                foreach (XmlNode xevent in xnode.ChildNodes)
                    if (xevent.Attributes["name"].Value == Name)
                        foreach (MKState s in Paths)
                            s.Load(xevent); // path
            }
        }

        public class MKState
        {
            internal string Name { get; set; }
            internal string ReadRu { get; set; }
            internal List<State> StateList { get; set; }
            internal List<string> FileList { get; set; }
            internal MKState(XmlNode xnode)
            {
                Name = xnode.Attributes["name"].Value;
                ReadRu = xnode.Attributes["ru"].Value;
                StateList = new List<State>();
                foreach (XmlNode path in xnode.ChildNodes) // path
                {
                    StateList.Add(new State()
                    {
                        Name = path.Attributes["name"].Value,
                        Value = path.Attributes["value"].Value
                    });
                }
                FileList = new List<string>();
            }

            internal bool Dump(XmlNode xnode, string CommanderID, string mod_path)
            {
                XmlDocument xdoc = xnode.OwnerDocument;
                xnode = xnode.AppendChild(xdoc.CreateElement("Path"));
                XmlNode xlist = xnode.AppendChild(xdoc.CreateElement("StateList"));
                foreach (State s in StateList)
                {
                    XmlNode xstate = xlist.AppendChild(xdoc.CreateElement("State"));
                    xstate.AppendChild(xdoc.CreateElement("Name")).InnerText = s.Name;
                    xstate.AppendChild(xdoc.CreateElement("Value")).InnerText = s.Value;
                }
                if (CommanderID.Length > 0)
                {
                    XmlNode xstate = xlist.AppendChild(xdoc.CreateElement("State"));
                    xstate.AppendChild(xdoc.CreateElement("Name")).InnerText = "CrewName";
                    xstate.AppendChild(xdoc.CreateElement("Value")).InnerText = CommanderID;
                }

                xlist = xnode.AppendChild(xdoc.CreateElement("FilesList"));
                foreach (string fn in FileList)
                {
                    string WemNameExt = $"{Path.GetFileNameWithoutExtension(fn)}.wem";
                    if (File.Exists($"{mod_path}\\{WemNameExt}"))
                        xlist.AppendChild(xdoc.CreateElement("File")).AppendChild(xdoc.CreateElement("Name")).InnerText = WemNameExt;
                    else
                    {
                        MessageBox.Show($"Не найден файл '{mod_path}\\{WemNameExt}'", "Ошибка", MessageBoxButtons.OK);
                        return false;
                    }
                }
                return true;
            }

            internal void Load(XmlNode xnode)
            {
                foreach (XmlNode xpath in xnode.ChildNodes)
                    if (xpath.Attributes["name"].Value == Name)
                    {
                        XmlNode xfl = xpath.SelectSingleNode("FileList");
                        FileList.Clear();
                        foreach (XmlNode xfile in xfl?.ChildNodes)
                            FileList.Add(xfile.InnerText);
                        return;
                    }
            }
        }

        public class State
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        internal class VOProject
        {
            internal List<MKEvent> EventsSFX;
            internal string SrcPath { get; set; }
            internal string ModName { get; set; }

            private string m_ModDir;

            internal string ModDir { set { m_ModDir = value; } get { return m_ModDir.Length == 0 ? ModName : m_ModDir; } }
            internal string CommanderID { get; set; }
            internal string PrjPath { get; set; }
            internal VOProject()
            {
                SrcPath = "";
                ModName = "";
                ModDir = "";
                CommanderID = "";
                PrjPath = "";

                string game_events_xml = "game_events.xml";
                if (!File.Exists(game_events_xml))
                {
                    ErrorMsgBox($"Файл '{game_events_xml}' не найден.");
                    Environment.Exit(1);
                }

                EventsSFX = new List<MKEvent>();
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(game_events_xml);
                XmlElement xRoot = xDoc.DocumentElement;
                foreach (XmlNode xnode in xRoot.ChildNodes) // event
                {
                    if (xnode.NodeType != XmlNodeType.Comment)
                    {
                        MKEvent e = new MKEvent(xnode)
                        {
                            Name = xnode.Attributes["name"].Value,
                            ExtId = xnode.Attributes["extid"].Value,
                        };
                        EventsSFX.Add(e);
                    }
                }
            }

            internal bool GenerateModXML(string mod_path)
            {
                XmlDocument xDoc = new XmlDocument
                {
                    PreserveWhitespace = false
                };

                XmlNode xnode = xDoc.AppendChild(xDoc.CreateElement("AudioModification.xml"));
                xnode = xnode.AppendChild(xDoc.CreateElement("AudioModification"));
                xnode.AppendChild(xDoc.CreateElement("Name")).InnerText = ModName;

                foreach (MKEvent e in EventsSFX)
                {
                    if (!e.Dump(xnode, CommanderID, mod_path)) return false; ;
                }
                XmlElement xRoot = xDoc.DocumentElement;
                XmlNode xmldecl = xDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                xDoc.InsertBefore(xmldecl, xRoot);

                XmlWriterSettings settings = new XmlWriterSettings()
                {
                    Indent = true,
                    IndentChars = "\t",
                    Encoding = new UTF8Encoding(false),
                    NewLineChars = "\n"
                };

                try
                {
                    XmlWriter writer = XmlWriter.Create($"{mod_path}\\mod.xml", settings);
                    xDoc.Save(writer);
                    writer.Flush();
                    writer.Close();
                }
                catch (Exception ex)
                {
                    ErrorMsgBox(ex.Message);
                    return false;
                }
                return true;
            }

            internal bool Load(string path)
            {
                try
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(path);
                    XmlElement xRoot = xDoc.DocumentElement;
                    XmlNode xnode = xRoot.SelectSingleNode("project");
                    ModName = xnode?.SelectSingleNode("mod_name")?.InnerText;
                    ModDir = xnode?.SelectSingleNode("mod_path")?.InnerText;
                    SrcPath = xnode?.SelectSingleNode("src_path")?.InnerText;
                    CommanderID = xnode?.SelectSingleNode("cmdr_id")?.InnerText;

                    xnode = xRoot.SelectSingleNode("events");
                    foreach (MKEvent evt in EventsSFX)
                        evt.Load(xnode);
                    PrjPath = path;
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "Ошибка загрузки");
                    return false;
                }
            }

            internal bool Load()
            {
                OpenFileDialog dlg = new OpenFileDialog
                {
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Title = "Выберите файл проекта",
                    Filter = "Проекты (*.prj)|*.prj|Все файлы|*.*"
                };
                if (DialogResult.OK != dlg.ShowDialog()) return false;
                return Load(dlg.FileName);
            }

            internal bool Save()
            {
                if (PrjPath.Length == 0)
                {
                    SaveFileDialog dlg = new SaveFileDialog
                    {
                        Filter = "Проекты (*.prj)|*.prj|Все файлы|*.*"
                    };
                    if (DialogResult.OK != dlg.ShowDialog()) return false;
                    PrjPath = dlg.FileName;
                }
                XmlDocument xDoc = new XmlDocument
                {
                    PreserveWhitespace = false
                };
                xDoc.AppendChild(xDoc.CreateElement("root"));
                XmlElement xRoot = xDoc.DocumentElement;

                XmlElement xproject = xDoc.CreateElement("project");
                xRoot.AppendChild(xproject);
                XmlElement xnode = xDoc.CreateElement("mod_name");
                xnode.InnerText = ModName;
                xproject.AppendChild(xnode);
                xnode = xDoc.CreateElement("mod_path");
                xnode.InnerText = ModDir;
                xproject.AppendChild(xnode);
                xnode = xDoc.CreateElement("src_path");
                xnode.InnerText = SrcPath;
                xproject.AppendChild(xnode);
                xnode = xDoc.CreateElement("cmdr_id");
                xnode.InnerText = CommanderID;
                xproject.AppendChild(xnode);

                XmlElement xevents = xDoc.CreateElement("events");
                xRoot.AppendChild(xevents);

                foreach (MKEvent evt in EventsSFX)
                {
                    XmlElement xevent = xDoc.CreateElement("event");
                    xevent.SetAttribute("name", evt.Name);
                    xevent.SetAttribute("extid", evt.ExtId);
                    xevents.AppendChild(xevent);
                    foreach (MKState st in evt.Paths)
                    {
                        XmlElement xpath = xDoc.CreateElement("path");
                        xpath.SetAttribute("name", st.Name);
                        xevent.AppendChild(xpath);

                        XmlElement xStateList = xDoc.CreateElement("StateList");
                        xpath.AppendChild(xStateList);

                        foreach (State state in st.StateList)
                        {
                            XmlElement xstate = xDoc.CreateElement("condition");
                            xstate.SetAttribute("name", state.Name);
                            xstate.SetAttribute("value", state.Value);
                            xStateList.AppendChild(xstate);
                        }

                        XmlElement xFileList = xDoc.CreateElement("FileList");
                        xpath.AppendChild(xFileList);
                        st.FileList.Sort();
                        foreach (string fn in st.FileList)
                        {
                            XmlElement xstate = xDoc.CreateElement("file");
                            xstate.InnerText = fn;
                            xFileList.AppendChild(xstate);
                        }
                    }
                }

                XmlNode xmldecl = xDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                xDoc.InsertBefore(xmldecl, xRoot);

                XmlWriterSettings settings = new XmlWriterSettings()
                {
                    Indent = true,
                    IndentChars = "\t",
                    Encoding = new UTF8Encoding(false),
                    NewLineChars = "\n"
                };

                XmlWriter writer = XmlWriter.Create(PrjPath, settings);
                xDoc.Save(writer);
                writer.Flush();
                writer.Close();
                return true;
            }

            internal void Remove(string state_name, string file_name)
            {
                foreach (MKState st in from MKEvent evt in EventsSFX
                                       from MKState st in evt.Paths
                                       where st.ReadRu == state_name
                                       select st)
                {
                    st.FileList.Remove(file_name);
                    return;
                }
            }
        }

        private bool GenerateSourceXML()
        {
            if (!Directory.Exists(prj.SrcPath))
            {
                ErrorMsgBox("Папка исходников не найдена");
                return false;
            }
            Log("Генерируем Source.xml");
            XmlDocument xDoc = new XmlDocument
            {
                PreserveWhitespace = false
            };
            xDoc.AppendChild(xDoc.CreateElement("ExternalSourcesList"));
            XmlElement xRoot = xDoc.DocumentElement;
            xRoot.SetAttribute("SchemaVersion", "1");
            xRoot.SetAttribute("Root", "");

            XmlNode xmldecl = xDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xDoc.InsertBefore(xmldecl, xRoot);

            foreach (string fn in from MKEvent evt in prj.EventsSFX
                                  from MKState st in evt.Paths
                                  from string fn in st.FileList
                                  select fn)
                if (File.Exists(fn))
                {
                    XmlElement name = xDoc.CreateElement("Source");
                    name.SetAttribute("Path", fn);
                    name.SetAttribute("Conversion", "WOWS_WEM_CONVERSION");
                    xRoot.AppendChild(name);
                }

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
                Encoding = new UTF8Encoding(false),
                NewLineChars = "\n"
            };

            try
            {
                string TmpDir = $"{prj.SrcPath}\\Windows";
                if (Directory.Exists(TmpDir)) Directory.Delete(TmpDir, true);
                Directory.CreateDirectory(TmpDir);
                XmlWriter writer = XmlWriter.Create($"{TmpDir}\\Sources.xml", settings);
                xDoc.Save(writer);
                writer.Flush();
                writer.Close();
                Log("Сгенерирован Sources.xml");
            }
            catch (Exception ex)
            {
                ErrorMsgBox(ex.Message);
                return false;
            }
            return true;
        }

        private void ListBoxFiles_DragDropEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void ListBoxFiles_DragDrop(object sender, DragEventArgs e)
        {
            string item = listBoxEvents.Text;
            int idx = 0;
            bool warn_no_wave = false;
            bool is_file_added = false;
            foreach (MKEvent evt in prj.EventsSFX)
                foreach (MKState path in evt.Paths)
                {
                    if (path.ReadRu == item)
                    {
                        var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                        foreach (string file in files)
                        {
                            if (!file.ToLower().EndsWith(".wav"))
                                warn_no_wave = true;
                            else if (!path.FileList.Contains(file))
                            {
                                path.FileList.Add(file);
                                is_file_added = true;
                            }
                        }
                        if (is_file_added)
                        {
                            path.FileList.Sort();
                            EventsListColors[idx] = true;
                            listBoxFiles.Items.Clear();
                            foreach (string s in path.FileList)
                                listBoxFiles.Items.Add(s);
                        }
                        break;
                    }
                    idx++;
                }

            if (warn_no_wave)
                ErrorMsgBox("Добавляемые файлы должы иметь формат wav.");

            if (is_file_added)
            {
                listBoxEvents.Invalidate();
                SetStatusText(0, "Файл(ы) добавлен(ы)");
                ProjectChanged = true;
            }
        }

        private static void ConvertAndCopy(IProgress<string> progress, string conv_args, string copy_args, string temp_dir)
        {
            using (Process process = new Process())
            {
                progress.Report("Запускаем конвертацию...");
                process.StartInfo.FileName = Properties.Settings.Default.WwisePath;
                process.StartInfo.Arguments = conv_args;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.OutputDataReceived += (sendingProcess, outLine) =>
                {
                    if (!String.IsNullOrEmpty(outLine.Data)) progress.Report(outLine.Data);
                };

                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
                progress.Report("=================");
            }

            using (Process process = new Process())
            {
                progress.Report("Копируем wem файлы в папку с модом...");
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = copy_args;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.OutputDataReceived += (sendingProcess, outLine) =>
                {
                    if (!String.IsNullOrEmpty(outLine.Data)) progress.Report(outLine.Data);
                };
                process.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(866);
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
                progress.Report("=================");
            }

            progress.Report("Удаляем временную папку...");
            try
            {
                Directory.Delete(temp_dir, true);
            }
            catch
            {
                progress.Report("Ошибка удаления папки.");
            }
            finally
            {
                progress.Report("Папка удалена.");
            }
        }

        private void ListBoxEvents_Update()
        {
            string event_name = listBoxEvents.Text;
            SetStatusText(0, $"Выбрано '{event_name}'");
            foreach (MKState st in from MKEvent evt in prj.EventsSFX
                                   from MKState st in evt.Paths
                                   where st.ReadRu == event_name
                                   select st)
            {
                listBoxFiles.Items.Clear();
                foreach (string s in st.FileList) listBoxFiles.Items.Add(s);
                break;
            }
            AdjustSplitterDistance();
            buttonFiles.PerformClick();
        }

        private void ListBoxEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxEvents.SelectedIndex != -1) ListBoxEvents_Update();
        }

        private void ListBoxFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && listBoxEvents.SelectedIndex != -1)
            {
                int idx = listBoxFiles.SelectedIndices.Count;
                if (idx <= 0) return;

                string item_name = listBoxEvents.Text;
                foreach (var item in listBoxFiles.SelectedItems)
                    prj.Remove(item_name, item.ToString());

                while (idx > 0)
                {
                    idx -= 1;
                    listBoxFiles.Items.RemoveAt(listBoxFiles.SelectedIndices[idx]);
                }
                UpdateEventsList();
                SetStatusText(0, "Файл(ы) удален(ы)");
                ProjectChanged = true;
            }
        }

        private void ListBoxFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBoxEvents.SelectedIndex != -1 && listBoxFiles.SelectedIndex != -1)
            {
                string fn = listBoxFiles.Text;
                if (File.Exists(fn))
                {
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(fn);
                    player.Play();
                }
                else if (fn.EndsWith(".wav"))
                {
                    if (DialogResult.Yes == MessageBox.Show($"Файл '{fn}' не найден. Удалить запись?", null, MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    {
                        prj.Remove(listBoxEvents.Text, fn);
                        listBoxFiles.Items.RemoveAt(listBoxFiles.SelectedIndex);
                    }
                }
            }
        }

        private void OnProjectLoad()
        {
            UpdateStrip();
            UpdateMenuItem();
            WebBrowser_Navigate(prj.SrcPath);
            UpdateEventsList();
            listBoxEvents.SelectedIndex = 0;
            ListBoxEvents_Update();
            FileInfo fi = new FileInfo(prj.PrjPath);
            SetStatusText(0, $"Проект '{fi.Name}' загружен");
            Properties.Settings.Default.LastProject = prj.PrjPath;
            ProjectChanged = false;
        }

        private void CheckProjectChanges()
        {
            if (ProjectChanged &&
                 DialogResult.Yes == MessageBox.Show(
                    "Проект был изменён. Сохранить изменения?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question)
               ) ProjectSave();
        }

        private void ProjectLoad()
        {
            CheckProjectChanges();
            if (prj.Load())
                OnProjectLoad();
            else
                SetStatusText(0, "Загрузка отменена");
        }

        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProjectLoad();
        }

        private void UpdateStrip()
        {
            SetStatusText(1, $"Проект: {prj.ModName}{(ProjectChanged ? "*" : "")}");
            SetStatusText(2, $"Папка: {prj.SrcPath}");
        }

        private void SetStatusText(int idx, string text)
        {
            statusStrip1.Items[idx].Text = text;
        }

        private void Clean_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TryClear();
        }

        private void ClearFiles()
        {
            foreach (MKEvent evt in prj.EventsSFX)
                foreach (MKState state in evt.Paths)
                    state.FileList.Clear();
        }

        private bool FileListEmpty()
        {
            foreach (MKEvent evt in prj.EventsSFX)
                foreach (MKState state in evt.Paths)
                    if (state.FileList.Count > 0) return false;
            return true;
        }

        private bool TryClear()
        {
            if (FileListEmpty()) return true;
            if (DialogResult.No == MessageBox.Show(
                "Удалить все ассоциации файлов с событиями?",
                "Подтверждение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2))
                return false;

            ClearFiles();
            listBoxFiles.Items.Clear();
            listBoxLog.Items.Clear();
            UpdateEventsList();
            Log("Проект очищен.");
            ProjectChanged = true;
            return true;
        }

        private void OpenProjectOptionsWindow()
        {
            if (!Directory.Exists(Properties.Settings.Default.GamePath))
            {
                ErrorMsgBox($"Путь к игре не найден. '{Properties.Settings.Default.GamePath}'");
                OpenSettingsWindow();
                return;
            }

            if (Properties.Settings.Default.GameVer.Length == 0)
            {
                ErrorMsgBox("Версия игры не определена.");
                OpenSettingsWindow();
                return;
            }

            ProjectOptionsWindow wnd = new ProjectOptionsWindow()
            {
                ProjectName = prj.ModName,
                ProjectPath = prj.ModDir,
                ProjectSrcPath = prj.SrcPath,
                CommanderID = prj.CommanderID,
                parent = this,
            };

            ApplyTheme(wnd);
            if (DialogResult.OK == wnd.ShowDialog(this))
            {
                if (prj.ModName != wnd.ProjectName)
                {
                    prj.ModName = wnd.ProjectName;
                    ProjectChanged = true;
                }
                prj.ModDir = (wnd.ProjectPath.Length > 0) ? wnd.ProjectPath : prj.ModName;
                if (prj.SrcPath != wnd.ProjectSrcPath && Directory.Exists(wnd.ProjectSrcPath))
                {
                    prj.SrcPath = wnd.ProjectSrcPath;
                    WebBrowser_Navigate(prj.SrcPath);
                    ProjectChanged = true;
                }
                if (prj.CommanderID != wnd.CommanderID)
                {
                    prj.CommanderID = wnd.CommanderID;
                    ProjectChanged = true;
                }
                UpdateStrip();
                UpdateMenuItem();
            }
        }

        private void ProjectOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenProjectOptionsWindow();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProjectSave();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = "Проекты (*.prj)|*.prj|Все файлы|*.*"
            };
            if (DialogResult.OK != dlg.ShowDialog()) return;
            prj.PrjPath = dlg.FileName;
            ProjectSave();
        }

        public void ProjectSave()
        {
            while (prj.ModName.Length == 0)
            {
                ErrorMsgBox("Имя проекта не может быть пустым!");
                OpenProjectOptionsWindow();
            }

            while (!Directory.Exists(prj.SrcPath))
            {
                ErrorMsgBox("Не указана папка с исходными файлами!");
                OpenProjectOptionsWindow();
            }

            if (prj.Save())
            {
                FileInfo fi = new FileInfo(prj.PrjPath);
                SetStatusText(0, $"Проект '{fi.Name}' сохранён");
                ProjectChanged = false;
            }
            else
            {
                SetStatusText(0, "Ошибка сохранения");
            }
        }

        private void UpdateEventsList()
        {
            int idx = 0;
            foreach (MKEvent evt in prj.EventsSFX)
                foreach (MKState p in evt.Paths)
                    EventsListColors[idx++] = p.FileList.Count > 0;
            listBoxEvents.Invalidate();
        }

        private void LogProgress(string text)
        {
            SetStatusText(0, text);
        }

        private void Log(string text)
        {
            listBoxLog.Items.Add(text);
        }

        private async void BuildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProjectSave();
            buttonLog.PerformClick();
            listBoxLog.Items.Clear();
            Log("Журнал сборки:");
            // clear or create mod directory
            string ModDir = $@"{Properties.Settings.Default.GamePath}\bin\{Properties.Settings.Default.GameVer}\res_mods\banks\mods\{prj.ModDir}";
            try
            {
                DirectoryInfo di = new DirectoryInfo(ModDir);
                if (!di.Exists)
                    Directory.CreateDirectory(ModDir);
                else
                    foreach (FileInfo file in di.GetFiles()) file.Delete();
            }
            catch (Exception ex)
            {
                ErrorMsgBox(ex.Message);
                Log(ex.Message);
                return;
            }

            // bypass
            if (Properties.Settings.Default.UseExists || !CheckPaths(false))
            {
                List<string> processed = new List<string>();
                Log("Копируем существующие wem файлы в папку с модом...");
                prj.EventsSFX.ForEach((evt) =>
                {
                    evt.Paths.ForEach((state) =>
                    {
                        if (state.FileList.Count == 0) return;
                        int files_count = 0;
                        state.FileList.ForEach((path) =>
                        {
                            if (processed.Contains(path))
                            {
                                files_count++;
                                return;
                            }
                            processed.Add(path);
                            FileInfo fileInfo = new FileInfo(path);
                            string fileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
                            string src = $"{fileInfo.DirectoryName}\\{fileName}.wem";

                            if (!File.Exists(src))
                            {
                                if (fileName != "TISHINA")
                                {
                                    Log($"НЕ НАЙДЕН: {src}");
                                    return;
                                }
                                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                                src = $"{appDirectory}\\utils\\TISHINA.wem"; // try copy 'TISHINA.wem' from appDirectory
                                if (File.Exists(src))
                                {
                                    File.Copy(src, $"{ModDir}\\TISHINA.wem", true);
                                    Log($"Скопирован: {src}");
                                }
                                else
                                {
                                    Log($"НЕ НАЙДЕН: {src}");
                                    return;
                                }
                            }
                            else
                            {
                                File.Copy(src, $"{ModDir}\\{fileName}.wem", true);
                                Log($"Скопирован: {src}");
                            }
                            files_count++;
                        });
                        if (files_count == 0)
                        {
                            Log($"ОШИБКА: не найдены файлы для события '{state.ReadRu}'.");
                        }
                    });
                });
            }
            else
            {
                if (!CheckPaths(true)) return;

                // generate Source.xml
                if (!GenerateSourceXML())
                {
                    Log("Ошибка создания Source.xml");
                    SetStatusText(0, "Ошибка создания Source.xml");
                    return;
                }

                // convert wav to wem
                Log("=================");
                var progress = new Progress<string>(s => Log(s));
                await Task.Run(() => ConvertAndCopy(progress,
                    $"\"{Properties.Settings.Default.WwiseProject}\" -ConvertExternalSources \"{prj.SrcPath}\\Windows\\Sources.xml\" -ExternalSourcesOutput \"{prj.SrcPath}\" -verbose",
                    $"/c copy /b \"{prj.SrcPath}\\Windows\\*.wem\" \"{ModDir}\"",
                    $"{prj.SrcPath}\\Windows"
                    ));
            }

            // generate mod.xml
            Log("=================");
            Log("Создаём mod.xml");
            if (prj.GenerateModXML(ModDir))
            {
                Log("=================");
                Log("Готово.");
                SetStatusText(0, "Готово");
            }
            else
            {
                Log("Ошибка создания mod.xml");
                SetStatusText(0, "Ошибка сборки");
            }

            listBoxLog.TopIndex = listBoxLog.Items.Count - 1;
            AdjustSplitterDistance(true);
        }

        private void ExitCtrlXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CheckProjectChanges();
            Properties.Settings.Default.WindowLocation = Location;
            Properties.Settings.Default.WindowSize = Size;
            Properties.Settings.Default.Save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Location = Properties.Settings.Default.WindowLocation;
            Size = Properties.Settings.Default.WindowSize;
            var LastProjectPath = Properties.Settings.Default.LastProject;
            if (File.Exists(LastProjectPath) && prj.Load(LastProjectPath)) OnProjectLoad();
            UpdateTheme();
        }

        private void OpenDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(Properties.Settings.Default.GamePath))
            {
                ErrorMsgBox("Путь к игре указан неверно.");
                return;
            }

            if (Properties.Settings.Default.GameVer.Length == 0)
            {
                ErrorMsgBox("Версия игры не определена.");
                return;
            }

            string mod_path = $@"{Properties.Settings.Default.GamePath}\bin\{Properties.Settings.Default.GameVer}\res_mods\banks\mods\{prj.ModDir}";
            if (prj.ModDir.Length > 0 && Directory.Exists(mod_path))
                Process.Start(new ProcessStartInfo()
                {
                    FileName = mod_path,
                    UseShellExecute = true,
                    Verb = "open"
                });
            else
                ErrorMsgBox("Папка мода не существует.");
        }

        private void ButtonBack_Click(object sender, EventArgs e)
        {
            if (webBrowser1.CanGoBack) webBrowser1.GoBack();
        }

        private void ButtonForward_Click(object sender, EventArgs e)
        {
            if (webBrowser1.CanGoForward) webBrowser1.GoForward();
        }

        private void ButtonUp_Click(object sender, EventArgs e)
        {
            Uri uri = webBrowser1.Url;
            if (uri is null) return;
            DirectoryInfo parent = Directory.GetParent(uri.LocalPath);
            WebBrowser_Navigate(parent?.FullName);
        }

        private void ButtonGetDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog()
            {
                SelectedPath = prj.SrcPath,
                Description = "Выберите папку:",
                ShowNewFolderButton = true,
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                WebBrowser_Navigate(dialog.SelectedPath);
            }
        }

        private void ButtonHome_Click(object sender, EventArgs e)
        {
            WebBrowser_Navigate(prj.SrcPath);
        }

        private void WebBrowser_Navigate(string path)
        {
            if (Directory.Exists(path)) webBrowser1.Navigate(path);
        }

        private void WebBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            textBox1.Text = webBrowser1.Url.LocalPath;
            textBox1.ReadOnly = true;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (webBrowser1.Focused || textBox1.Focused)
                switch (keyData)
                {
                    case Keys.Alt | Keys.Left:
                        {
                            buttonBack.PerformClick();
                            webBrowser1.Focus();
                            return true;
                        }
                    case Keys.Alt | Keys.Right:
                        {
                            buttonForward.PerformClick();
                            webBrowser1.Focus();
                            return true;
                        }
                    case Keys.Back:
                    case Keys.Alt | Keys.Up:
                        {
                            buttonUp.PerformClick();
                            webBrowser1.Focus();
                            return true;
                        }
                    case Keys.Alt | Keys.Down:
                        {
                            buttonHome.PerformClick();
                            webBrowser1.Focus();
                            return true;
                        }
                    default:
                        break;
                }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ListBoxEvents_DrawItem(object sender, DrawItemEventArgs e)
        {
            if ((e.State & DrawItemState.Focus) != DrawItemState.Focus &&
                (e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e = new DrawItemEventArgs(e.Graphics,
                                          e.Font,
                                          e.Bounds,
                                          e.Index,
                                          e.State ^ (DrawItemState.Selected | DrawItemState.Focus),
                                          e.ForeColor,
                                          e.BackColor);

            e.DrawBackground();
            Graphics g = e.Graphics;
            Color ColorBack = EventsListColors[e.Index]
                ? Properties.Settings.Default.ColorBackEvtListItem
                : Properties.Settings.Default.ColorBackEvtListItemEmpty;
            g.FillRectangle(new SolidBrush(ColorBack), e.Bounds);
            g.DrawString(listBoxEvents.Items[e.Index].ToString(),
                e.Font,
               ((e.State & DrawItemState.Focus) == DrawItemState.Focus) ? Brushes.Blue : Brushes.Black,
                e.Bounds);
            e.DrawFocusRectangle();
        }

        private void CopyList(string dest, List<string> list)
        {
            foreach (var p in from MKEvent evt in prj.EventsSFX
                              from MKState p in evt.Paths
                              where p.ReadRu == dest
                              select p)
            {
                p.FileList = p.FileList.Union(list).ToList();
                p.FileList.Sort();
                return;
            }
        }

        // [ИСХ] -> [ВХ]
        private void CopyMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (prj.ModName.Length == 0) return;
            foreach (MKEvent evt in prj.EventsSFX)
                foreach (MKState p in evt.Paths)
                    if (p.FileList.Count > 0 && p.ReadRu.StartsWith("[ИСХ]"))
                        CopyList(p.ReadRu.Replace("[ИСХ]", "[ВХ]"), p.FileList);
            ProjectChanged = true;
            UpdateEventsList();
            ListBoxEvents_Update();
        }

        private static void ErrorMsgBox(string msg)
        {
            MessageBox.Show(msg, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void RunConverter(IProgress<string> progress, string FileName, string args)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = FileName;
                process.StartInfo.Arguments = args;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.OutputDataReceived += (sendingProcess, outLine) =>
                {
                    if (!String.IsNullOrEmpty(outLine.Data)) progress.Report(outLine.Data);
                };

                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
            }
        }

        private async void Convert_to_OGG()
        {
            if (!File.Exists($"{converter_folder}/ww2ogg.exe")) return;
            if (!File.Exists($"{converter_folder}/packed_codebooks_aoTuV_603.bin")) return;
            if (!File.Exists($"{converter_folder}/revorb.exe")) return;

            string ConvertPath = textBox1.Text;
            if (!Directory.Exists(ConvertPath)) return;
            var WemFiles = Directory.EnumerateFiles(ConvertPath, "*.wem");
            int max_counter = WemFiles.Count();
            if (max_counter == 0) return;
            int counter = 0;

            listBoxLog.Items.Clear();
            buttonLog.PerformClick();
            string msg_state = "конвертация";
            var progress_work = new Progress<string>(s => Log(s));
            var progress_complete = new MethodInvoker(delegate
            {
                LogProgress($"WEM -> OGG: {msg_state} {++counter * 100 / max_counter}%");
            });

            foreach (var path in WemFiles)
            {
                await Task.Run(() => RunConverter(progress_work,
                      $"{converter_folder}/ww2ogg.exe",
                      $"\"{path}\" --pcb {converter_folder}/packed_codebooks_aoTuV_603.bin"))
                     .ContinueWith(_ => Invoke(progress_complete));
            }

            counter = 0;
            msg_state = "постобработка";
            foreach (var path in Directory.EnumerateFiles(ConvertPath, "*.ogg"))
            {
                await Task.Run(() => RunConverter(progress_work, $"{converter_folder}/revorb.exe", $"\"{path}\""))
                    .ContinueWith(_ => Invoke(progress_complete));
            }
            /*
             if (to_mp3)
                        {
                            if (!File.Exists($"{converter_folder}/ffmpeg.exe")) return;
                            label2.Text = "Конвертация в mp3:";
                            var tasks = new List<Task>();
                            foreach (var path in Directory.EnumerateFiles(ConvertPath, "*.ogg"))
                            {
                                tasks.Add(Task.Run(() =>
                                    RunConverter(progress,
                                    $"{converter_folder}/ffmpeg.exe",
                                    $"-y -i \"{path}\" -acodec libmp3lame -b:a 256k \"{ConvertPath}\\{Path.GetFileNameWithoutExtension(path)}.mp3\""
                                    )));
                            }
                            Task.WaitAll(tasks.ToArray());
                            if (remove_tmp_ogg) DeleteFilesByMask("*.ogg");
                        }*/
            LogProgress("WEM -> OGG: выполнено.");
            Log("Готово.");
            listBoxLog.TopIndex = listBoxLog.Items.Count - 1;
            AdjustSplitterDistance();
        }

        private async void Convert_to_WAV()
        {
            const string vgmstream = "./utils/vgmstream/vgmstream-cli.exe";
            if (!File.Exists(vgmstream)) return;
            string ConvertPath = textBox1.Text;
            if (!Directory.Exists(ConvertPath)) return;

            var Files = Directory.EnumerateFiles(ConvertPath, "*.wem");
            int max_counter = Files.ToArray().Length;
            if (max_counter == 0) return;

            listBoxLog.Items.Clear();
            buttonLog.PerformClick();
            var progress_work = new Progress<string>(s => Log(s));
            int counter = 0;
            var progress_complete = new MethodInvoker(delegate { LogProgress($"WEM -> WAV: {++counter * 100 / max_counter}%"); });
            foreach (var path in Files)
            {
                await Task.Run(() => RunConverter(progress_work, vgmstream,
                        $"-o \"{ConvertPath}\\{Path.GetFileNameWithoutExtension(path)}.wav\" \"{path}\""))
                    .ContinueWith(_ => Invoke(progress_complete));
            }

            LogProgress($"WEM -> WAV: выполнено.");
            Log("Готово.");
            listBoxLog.TopIndex = listBoxLog.Items.Count - 1;
            AdjustSplitterDistance();
        }

        private void Convert_WEM2OGG_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Convert_to_OGG();
        }

        private void Convert_WEM2WAV_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Convert_to_WAV();
        }

        private async void DeleteFilesByMask(string mask)
        {
            if (MessageBox.Show($"Удалить все {mask} файлы из текущей папки?", "Удаление файлов",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                listBoxLog.Items.Clear();
                buttonLog.PerformClick();
                string dir = textBox1.Text;
                Log($"Удаление файлов {mask}");
                if (Directory.Exists(dir))
                    foreach (var path in Directory.EnumerateFiles(dir, mask))
                    {
                        Log(path);
                        await Task.Run(() => { File.Delete(path); });
                    }
                Log("Выполнено.");
                buttonLog.PerformClick();
                listBoxLog.TopIndex = listBoxLog.Items.Count - 1;
            }
        }

        private void RemoveWAV_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteFilesByMask("*.wav");
        }

        private void RemoveOGG_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteFilesByMask("*.ogg");
        }

        private void AdjustSplitterDistance(bool force = false)
        {
            if (!force && !Properties.Settings.Default.SplitterAutoSize) return;
            int maxlen = 0;
            Font font = new Font("Microsoft Sans Serif", 8);
            foreach (var itm in listBoxFiles.Visible ? listBoxFiles.Items : listBoxLog.Items)
            {
                int strlen = TextRenderer.MeasureText(itm.ToString(), font).Width;
                if (maxlen < strlen) maxlen = strlen;
            }
            if (maxlen < 10) return;
            if (maxlen < 390) maxlen = 390;
            maxlen += 10;
            if (splitContainer1.SplitterDistance < maxlen)
                splitContainer1.SplitterDistance = maxlen;
        }

        public string ItemNamebyCommanderID(string commID)
        {
            if (commID == "") return "";
            foreach (var commander in CommanderList)
            {
                if (commander.ID == commID) return commander.CBItemName();
            }
            return $"unknown ({commID})";
        }

        public string CommanderIDbyItemName(string value, string defvalue)
        {
            if (value == "") return "";
            foreach (var commander in CommanderList)
            {
                if (commander.CBItemName() == value) return commander.ID;
            }
            return defvalue;
        }

        private void Import_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!TryClear()) return;

            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Укажите файл mod.xml",
                Filter = "mod.xml|",
                FileName = "mod.xml",
                InitialDirectory = webBrowser1.Url?.LocalPath
            };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            prj.SrcPath = Directory.GetParent(dialog.FileName).FullName;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(dialog.FileName);
            XmlElement xRoot = xDoc.DocumentElement;

            // commanders 
            List<string> list_CommanderIDS = new List<string>();
            foreach (XmlNode xnode in xRoot.SelectSingleNode("AudioModification").SelectNodes("ExternalEvent"))
            {
                var xcont = xnode.SelectSingleNode("Container");
                string extid = xcont.SelectSingleNode("ExternalId").InnerText;
                if (extid == "VVO_Alarm_Map_Border")
                {
                    foreach (XmlNode xpath in xcont.SelectNodes("Path"))
                    {
                        var node = xpath["StateList"]?["State"];
                        if (node?["Name"]?.InnerText == "CrewName")
                            list_CommanderIDS.Add(node["Value"]?.InnerText);
                    }
                    break;
                }
            }

            if (list_CommanderIDS.Count == 1)
            {
                prj.CommanderID = list_CommanderIDS[0];
            }
            else if (list_CommanderIDS.Count > 1)
            {
                ChooseCommanderForm cf = new ChooseCommanderForm() { Items = list_CommanderIDS, parent = this };
                cf.ShowDialog(this);
                prj.CommanderID = cf.CommanderID;
            }
            else
            {
                prj.CommanderID = "";
            }

            foreach (XmlNode xnode in xRoot.SelectSingleNode("AudioModification").SelectNodes("ExternalEvent"))
            {
                var xcont = xnode.SelectSingleNode("Container");
                string extid = xcont.SelectSingleNode("ExternalId").InnerText;

                foreach (XmlNode xpath in xcont.SelectNodes("Path"))
                {
                    var pEvt = prj.EventsSFX.Find(evt => (evt.ExtId == extid));
                    if (pEvt == null) continue;
                    XmlNode xStateList = xpath.SelectSingleNode("StateList");
                    var _nv = new Dictionary<string, string>();
                    bool _br = false;
                    foreach (XmlNode xState in xStateList.SelectNodes("State"))
                    {
                        var name = xState.SelectSingleNode("Name").InnerText;
                        var value = xState.SelectSingleNode("Value").InnerText;
                        if (name == "CrewName")
                        {
                            if (value != prj.CommanderID) _br = true;
                            break;
                        }
                        _nv[name] = value;
                    }

                    if (_br)
                    {
                        continue;
                    }

                    var mkState = pEvt.Paths.Find(stl =>
                    {
                        var _check = new Dictionary<string, string>();
                        //if (extid == "VVO_Pilots_Status") _check["Plane_Type"] = "Torpedo";
                        stl.StateList.ForEach(st => _check[st.Name] = st.Value);
                        return _check.All(pair => _nv.TryGetValue(pair.Key, out string value) && value == pair.Value);
                    });

                    if (mkState != null)
                    {
                        XmlNode xFileList = xpath.SelectSingleNode("FilesList");
                        foreach (XmlNode xFile in xFileList.SelectNodes("File"))
                        {
                            string fn = $"{prj.SrcPath}\\{Path.GetFileNameWithoutExtension(xFile.InnerText)}.wav";
                            if (!mkState.FileList.Contains(fn))
                            {
                                mkState.FileList.Add(fn);
                            }
                        }
                        mkState.FileList.Sort();
                    }
                }
            }
            bProjectChanged = true;
            WebBrowser_Navigate(prj.SrcPath);
            UpdateStrip();
            UpdateEventsList();
            ListBoxEvents_Update();
            string item = ItemNamebyCommanderID(prj.CommanderID);
            if (item.Length == 0)
            {
                string mod_name = xRoot.SelectSingleNode("AudioModification").SelectSingleNode("Name").InnerText;
                Log($"Импортирован мод {mod_name}.");
            }
            else
                Log($"Импортирован мод {item}.");
        }

        internal void UpdateTheme()
        {
            ApplyTheme(this);
            ApplyMenuTheme(menuStrip1.Items);
        }

        public static Color GetBackColor()
        {
            return Properties.Settings.Default.DarkTheme ? Properties.Settings.Default.DarkModeBack : SystemColors.Control;
        }

        public static Color GetForeColor()
        {
            return Properties.Settings.Default.DarkTheme ? Properties.Settings.Default.DarkModeFore : SystemColors.ControlText;
        }

        private void ApplyMenuTheme(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                if (item is ToolStripDropDownItem menuItem)
                {
                    menuItem.DropDown.ForeColor = GetForeColor();
                }
            }
        }

        internal void ApplyTheme(Control ctrl)
        {
            ctrl.BackColor = GetBackColor();
            ctrl.ForeColor = GetForeColor();
            foreach (Control child in ctrl.Controls)
            {
                ApplyTheme(child);
            }
        }

        /////////////
        private class MenuStripRenderer : ToolStripProfessionalRenderer
        {
            public MenuStripRenderer() : base(new MyColors()) { }
        }

        private class MyColors : ProfessionalColorTable
        {
            // фон при наведении мыши, не относящихся в меню верхнего уровня
            public override Color MenuItemSelected => GetBackColor();

            // фон при наведении мыши
            public override Color MenuItemSelectedGradientBegin => GetBackColor();
            public override Color MenuItemSelectedGradientEnd => GetBackColor();

            // фон при выборе пункта
            public override Color MenuItemPressedGradientBegin => GetBackColor();
            public override Color MenuItemPressedGradientEnd => GetBackColor();

            // цвет отсутпа для иконки
            public override Color ImageMarginGradientBegin => GetBackColor();
            public override Color ImageMarginGradientMiddle => GetBackColor();
            public override Color ImageMarginGradientEnd => GetBackColor();

            // рамка пункта меню
            public override Color MenuItemBorder => GetForeColor();
            // общая рамка меню
            public override Color MenuBorder => GetForeColor();

            //public override Color ToolStripBorder => Color.Red;
            // цвет подчеркивания выбраннного пункта топ меню
            public override Color ToolStripDropDownBackground => GetBackColor();

            // цвет разделителя
            public override Color SeparatorDark => GetForeColor();

        }

        private void SplitContainer1_Paint(object sender, PaintEventArgs pe)
        {
            using (SolidBrush brush = new SolidBrush(SystemColors.ActiveBorder))
            {
                int pos = listBoxFiles.Location.Y;
                Rectangle rect = new Rectangle(splitContainer1.SplitterDistance, pos, 2, splitContainer1.Height - pos - 5);
                pe.Graphics.FillRectangle(brush, rect);
            }
        }

        private void AddSilence()
        {
            prj.EventsSFX.ForEach((evt) =>
            {
                evt.Paths.ForEach((state) =>
                {
                    if (state.FileList.Count == 0)
                    {
                        state.FileList.Add($"{prj.SrcPath}\\TISHINA.wav");
                    }
                });
            });
            UpdateEventsList();
            ListBoxEvents_Update();
            ProjectChanged = true;
        }

        private void AddSilenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSilence();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutDialog dlg = new AboutDialog();
            dlg.ShowDialog(this);
        }

        private void CheckNewVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppUpdater Updater = new AppUpdater();
            if (!Updater.Check())
            {
                MessageBox.Show("Вы используете последнюю версию программы.",
                    "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (MessageBox.Show("Доступна новая версия. Обновить программу?",
                "Обновление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CheckProjectChanges();
                Updater.Run();
            }
        }

        private void InitCommandersList()
        {
            int PeekLong(byte[] fs, int offs)
            {
                return (fs[3 + offs] << 24) + (fs[2 + offs] << 16) + (fs[1 + offs] << 8) + fs[0 + offs];
            }

            string PeekString(byte[] fs, int offs, int len)
            {
                if (len == 0) return string.Empty;
                return Encoding.UTF8.GetString(fs, offs, len);
            }

            Commander GetCommanderByID(string ID)
            {
                foreach (var item in CommanderList)
                {
                    if (item.ID == ID) return item;
                }
                return null;
            }

            Commander GetCommanderByName(string name)
            {
                foreach (var item in CommanderList)
                {
                    if (item.Name == name) return item;
                }
                return null;
            }

            string mods_path = $@"{Properties.Settings.Default.GamePath}\bin\{Properties.Settings.Default.GameVer}\res\banks\mod.xml";
            if (!File.Exists(mods_path))
            {
                ErrorMsgBox($"Файл '{mods_path}' не найден.");
                return;
            }
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(mods_path);
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot.SelectSingleNode("states").ChildNodes) // event
            {
                if ("CrewName" == xnode.Attributes["name"].Value)
                {
                    foreach (XmlNode xnode_value in xnode.SelectSingleNode("values").ChildNodes)
                    {
                        string name = xnode_value.Attributes["name"].Value;
                        if (name != "Unknown")
                        {
                            if (GetCommanderByID(name) == null) CommanderList.Add(new Commander { ID = name, Name = $"IDS_{name.ToUpper()}" });
                        }
                    }
                    break;
                }
            }

            // reading mo
            string global_mo = $@"{Properties.Settings.Default.GamePath}\bin\{Properties.Settings.Default.GameVer}\res\texts\ru\LC_MESSAGES\global.mo";
            if (!File.Exists(global_mo))
            {
                Log($"! Файл '{global_mo}' не найден.");
                return;
            }

            var content = File.ReadAllBytes(global_mo);
            int magic = PeekLong(content, 0);
            var version = PeekLong(content, 4);
            var numbers = PeekLong(content, 8);
            var offset = PeekLong(content, 12);
            var translated = PeekLong(content, 16);
            for (int i = 0; i < numbers; i++)
            {
                // origin len
                int ol = PeekLong(content, offset);
                // origin offset
                int oo = PeekLong(content, offset + 4);
                offset += 8;
                string msg = PeekString(content, oo, ol); // IDS_NAME

                // translated len
                int tl = PeekLong(content, translated);
                // translated offset
                int to = PeekLong(content, translated + 4);
                translated += 8;
                string tmsg = PeekString(content, to, tl);
                var cm = GetCommanderByName(msg);
                if (cm != null)
                {
                    cm.Name_RU = tmsg;
                }
            }

            // sort commanders list
            CommanderList.Sort(delegate (Commander a, Commander b)
            {
                var matcha = Regex.IsMatch(a.Name_RU, @"\p{IsCyrillic}");
                var matchb = Regex.IsMatch(b.Name_RU, @"\p{IsCyrillic}");
                if (matcha && !matchb) return -1;
                if (matchb && !matcha) return 1;
                return a.Name_RU.CompareTo(b.Name_RU);
            });
        }

        private void ButtonFiles_Click(object sender, EventArgs e)
        {
            listBoxLog.Hide();
            listBoxFiles.Show();
            listBoxFiles.Focus();
        }

        private void ButtonLog_Click(object sender, EventArgs e)
        {
            listBoxFiles.Hide();
            listBoxLog.Show();
            listBoxLog.Focus();
        }

        private void OpenSourceDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(prj.SrcPath))
                Process.Start(new ProcessStartInfo()
                {
                    FileName = prj.SrcPath,
                    UseShellExecute = true,
                    Verb = "open"
                });
        }

        private bool CheckPaths(bool verb)
        {
            listBoxLog.Items.Clear();
            buttonLog.PerformClick();
            Log("Проверка существования путей...");

            List<string> flist = new List<string>();
            foreach (var path in from MKEvent evt in prj.EventsSFX
                                 from MKState p in evt.Paths
                                 from string path in p.FileList
                                 select path)
            {
                if (!flist.Contains(path) && !File.Exists(path))
                {
                    flist.Add(path);
                    if (verb) Log($"! Не найден {path}");
                }
            }
            if (flist.Count() == 0)
            {
                Log("OK.");
                return true;
            }
            else
            {
                Log($"! Не найдено файлов: {flist.Count()}.");
                return false;
            }
        }

        private void CheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckPaths(true);
        }

        private void CopyPilotsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (prj.ModName.Length == 0) return;
            foreach (MKEvent evt in prj.EventsSFX)
                foreach (MKState p in evt.Paths)
                {
                    if (p.FileList.Count == 0) continue;
                    if (p.ReadRu.Contains("Торпедоносец") &&
                        (p.Name == "Pilots_Airborne" || p.Name == "Pilots_Atpos" ||
                        p.Name == "Pilots_Destroy" || p.Name == "Pilots_Engage" ||
                        p.Name == "Pilots_Land" || p.Name == "Pilots_Ready" ||
                        p.Name == "Pilots_Under_Attack"))
                    {
                        CopyList(p.ReadRu.Replace("Торпедоносец", "Бомбардировщик"), p.FileList);
                        CopyList(p.ReadRu.Replace("Торпедоносец", "Истребитель"), p.FileList);
                        continue;
                    }
                    if (p.ReadRu == "Катапультный истребитель возвращается")
                    {
                        CopyList("Пилоты (Кат. истребитель): Возвращаюсь", p.FileList);
                        continue;
                    }
                    if (p.ReadRu == "Катапультный истребитель уничтожен")
                    {
                        CopyList("Пилоты (Кат. истребитель): Уничтожен", p.FileList);
                        continue;
                    }
                    if (p.ReadRu == "Корректировщик возвращается")
                    {
                        CopyList("Пилоты (Корректировщик): Возвращаюсь", p.FileList);
                        continue;
                    }
                    if (p.ReadRu == "Корректировщик уничтожен")
                    {
                        CopyList("Пилоты (Корректировщик): Уничтожен", p.FileList);
                        continue;
                    }
                }
            ProjectChanged = true;
            UpdateEventsList();
            ListBoxEvents_Update();
        }

        private void ListBoxLog_DrawItem(object sender, DrawItemEventArgs e)
        {
            string text = (e.Index == -1) ? "" : listBoxLog.Items[e.Index].ToString();
            if ((e.State & DrawItemState.Focus) != DrawItemState.Focus &&
                (e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e = new DrawItemEventArgs(e.Graphics,
                                           e.Font,
                                           e.Bounds,
                                           e.Index,
                                           e.State ^ (DrawItemState.Selected | DrawItemState.Focus),
                                           e.ForeColor,
                                           e.BackColor);

            e.DrawBackground();
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(e.BackColor), e.Bounds);
            g.DrawString(text,
                e.Font,
                text.StartsWith("! ") ? Brushes.Red :
                (((e.State & DrawItemState.Focus) == DrawItemState.Focus) ? Brushes.WhiteSmoke : new SolidBrush(GetForeColor())),
                e.Bounds);
            e.DrawFocusRectangle();
        }
    }
}
