using System.IO;
using System.Collections.Generic;

namespace DirScanner
{
    class Util
    {
        public static string PrintFileSizeHuman(long size)
        {
            if (size < 1024)
            {
                return size + " bytes";
            }
            else if (size < 1024 * 1024)
            {
                return (size / 1024) + " KB";
            }
            else if (size < 1024 * 1024 * 1024)
            {
                return (size / (1024 * 1024)) + " MB";
            }
            else
            {
                return (size / (1024 * 1024 * 1024)) + " GB";
            }
        }
    }

    class Table
    {
        public List<string> Headers;
        public List<List<string>> Rows;

        public Table()
        {
            Headers = new List<string>();
            Rows = new List<List<string>>();
        }
        public void PrintTable(int indent = 0)
        {
            string indentStr = "";
            for (int i = 0; i < indent; i++)
            {
                indentStr += " ";
            }
            List<int> maxLengths = new List<int>();
            for (int i = 0; i < Headers.Count; i++)
            {
                maxLengths.Add(Headers[i].Length);
            }
            for (int i = 0; i < Rows.Count; i++)
            {
                for (int j = 0; j < Rows[i].Count; j++)
                {
                    if (Rows[i][j].Length > maxLengths[j])
                    {
                        maxLengths[j] = Rows[i][j].Length;
                    }
                }
            }
            Console.Write(indentStr + "| ");
            for (int i = 0; i < Headers.Count; i++)
            {
                Console.Write(Headers[i]);
                for (int j = Headers[i].Length; j < maxLengths[i]; j++)
                {
                    Console.Write(" ");
                }
                Console.Write(" | ");
            }
            Console.WriteLine();
            Console.Write(indentStr + "| ");
            for (int i = 0; i < Headers.Count; i++)
            {
                Console.Write(new string('-', maxLengths[i]) + " | ");
            }
            Console.WriteLine();
            for (int i = 0; i < Rows.Count; i++)
            {
                Console.Write(indentStr + "| ");
                for (int j = 0; j < Rows[i].Count; j++)
                {
                    Console.Write(Rows[i][j]);
                    for (int k = Rows[i][j].Length; k < maxLengths[j]; k++)
                    {
                        Console.Write(" ");
                    }
                    Console.Write(" | ");
                }
                Console.WriteLine();
            }
        }
    }

    enum GroupKind
    {
        ByDirectoryOrFile,
        ByTypes,
        ByExtension,
        BySizeGroups,
        ByFirstLetter,

    }
    class GroupResult
    {
        public string Kind { get; set; }
        public long FileCount { get; set; }
        public long TotalSize { get; set; }
        public long AvgSize { get { return TotalSize / FileCount; } }
        public long MaxSize { get; set; }
        public long MinSize { get; set; }

        public void PrintRow(Table table)
        {
            List<string> row = new List<string>();
            row.Add(Kind);
            row.Add(FileCount.ToString());
            row.Add(Util.PrintFileSizeHuman(TotalSize));
            row.Add(Util.PrintFileSizeHuman(AvgSize));
            row.Add(Util.PrintFileSizeHuman(MaxSize));
            row.Add(Util.PrintFileSizeHuman(MinSize));
            table.Rows.Add(row);

        }

        public void Push(FileInfo file)
        {
            FileCount++;
            TotalSize += file.Length;
            if (MaxSize < file.Length)
            {
                MaxSize = file.Length;
            }
            if (MinSize > file.Length)
            {
                MinSize = file.Length;
            }
        }
        public void Push(DirectoryInfo dir)
        {
            FileCount++;

        }

    }
    class Grouping
    {
        static IDictionary<string, string> _extensionToType = new Dictionary<string, string>{
            {".txt", "Document"},
            {".doc", "Document"},
            {".docx", "Document"},
            {".pdf", "Document"},
            {".ppt", "Document"},
            {".pptx", "Document"},
            {".xls", "Document"},
            {".xlsx", "Document"},
            {".jpg", "Image"},
            {".jpeg", "Image"},
            {".png", "Image"},
            {".gif", "Image"},
            {".bmp", "Image"},
            {".mp3", "Audio"},
            {".wav", "Audio"},
            {".mp4", "Video"},
            {".avi", "Video"},
            {".mov", "Video"},
            {".mkv", "Video"},
            {".wmv", "Video"},
            {".flv", "Video"},
            {".m4a", "Audio"},
            {".m4v", "Video"},
            {".py", "Program"},
            {".c", "Program"},
            {".cpp", "Program"},
            {".cs", "Program"},
            {".java", "Program"},
            {".ts", "Program"},
            {".js", "Program"},
            {".dmg", "Disk"},
            {".iso", "Disk"},
            {".zip", "Archive"},
            {".rar", "Archive"},
            {".7z", "Archive"},
            {".tar", "Archive"},
            {".gz", "Archive"},
            {".bz2", "Archive"},

        };

        List<long> _sizeGroups = new List<long>{
            1024,
            1024 * 1024,
            1024 * 1024 * 1024,
        };
        GroupKind kind;
        public IDictionary<string, GroupResult> GroupingResults { get; set; }

        public Grouping(GroupKind kind)
        {
            this.kind = kind;
            GroupingResults = new Dictionary<string, GroupResult>();
        }
        public void AddToGrouping(FileInfo file)
        {
            switch (kind)
            {
                case GroupKind.ByDirectoryOrFile:
                    GetOrCreateResultByKind("File").Push(file);
                    break;
                case GroupKind.ByTypes:
                    string ext = Path.GetExtension(file.FullName);
                    if (ext.Length > 0)
                    {
                        string type = _extensionToType.ContainsKey(ext) ? _extensionToType[ext] : "Other";

                        GetOrCreateResultByKind(type).Push(file);
                    }
                    break;
                case GroupKind.ByExtension:
                    GetOrCreateResultByKind(Path.GetExtension(file.FullName)).Push(file);
                    break;
                    case GroupKind.ByFirstLetter:
                        GetOrCreateResultByKind(file.Name[0].ToString()).Push(file);
                        break;
                case GroupKind.BySizeGroups:
                    long size = file.Length;
                    foreach (var group in _sizeGroups)
                    {
                        if (size < group)
                        {
                            GetOrCreateResultByKind(Util.PrintFileSizeHuman(group)).Push(file);
                            break;
                        }
                    }
                    break;
            }
        }
        public void AddToGrouping(DirectoryInfo dir)
        {
            switch (kind)
            {
                case GroupKind.ByDirectoryOrFile:
                    GetOrCreateResultByKind("Directory").Push(dir);
                    break;
            }
        }
        public void PrintGrouping(int indent)
        {

            Console.WriteLine($"{new string(' ', indent)}Grouping by {kind}");
            Console.WriteLine("");

            Table table = new Table();
            table.Headers.Add("Kind");
            table.Headers.Add("FileCount");
            table.Headers.Add("TotalSize");
            table.Headers.Add("AvgSize");
            table.Headers.Add("MaxSize");
            table.Headers.Add("MinSize");
            // sort by total size
            var sorted = GroupingResults.OrderByDescending(x => x.Value.TotalSize);
            foreach (var kvp in sorted)
            {
                kvp.Value.PrintRow(table);
            }

            table.PrintTable(indent + 4);

        }

        private GroupResult GetOrCreateResultByKind(string key)
        {
            if (!GroupingResults.ContainsKey(key))
            {
                GroupResult result = new GroupResult();
                result.Kind = key;
                GroupingResults[key] = result;
            }
            return GroupingResults[key];
        }

    }

    class DirScanner
    {
        static void Main(string[] args)
        {
            Queue<string> dirs = new Queue<string>();
            List<Grouping> groupings = new List<Grouping>();
            groupings.Add(new Grouping(GroupKind.ByDirectoryOrFile));
            groupings.Add(new Grouping(GroupKind.ByTypes));
            groupings.Add(new Grouping(GroupKind.ByExtension));
            groupings.Add(new Grouping(GroupKind.ByFirstLetter));
            groupings.Add(new Grouping(GroupKind.BySizeGroups));

            dirs.Enqueue(@"/Users/alufers/Downloads");
            long i = 0;
            while (dirs.Count > 0)
            {
                try
                {
                    string dir = dirs.Dequeue();
                    DirectoryInfo dirInfo = new DirectoryInfo(dir);
                    List<FileInfo> files = dirInfo.GetFiles().ToList();
                    foreach (FileInfo file in files)
                    {

                        foreach (Grouping grouping in groupings)
                        {
                            grouping.AddToGrouping(file);
                        }

                    }
                    List<DirectoryInfo> dirsInDir = dirInfo.GetDirectories().ToList();
                    foreach (DirectoryInfo dirInDir in dirsInDir)
                    {
                        foreach (Grouping grouping in groupings)
                        {
                            grouping.AddToGrouping(dirInDir);
                        }
                        dirs.Enqueue(dirInDir.FullName);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error while scanning: " + e.Message);
                }

                i++;
                if (i % 1000 == 0)
                {
                    Console.Write($"scanning {i}/{dirs.Count} dirs...\r");
                }
            }
            foreach (Grouping grouping in groupings)
            {
                grouping.PrintGrouping(4);
            }
        }


    }
}
