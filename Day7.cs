namespace adventofcode2022;

public class Day7 : PuzzleBase
{
    class FileInfo
    {
        public FileInfo(string fileName, long size)
        {
            FileName = fileName;
            Size = size;
        }

        public string FileName { get; }
        public long Size { get; }
    }

    class DirectoryInfo
    {
        public DirectoryInfo()
        {
            Directories = new Dictionary<string, DirectoryInfo>();
            Files = new Dictionary<string, FileInfo>();
            size = null;
        }

        public Dictionary<string, DirectoryInfo> Directories { get; }
        public Dictionary<string, FileInfo> Files { get; }
        private long? size;

        public List<DirectoryInfo> GetAllDirectories()
        {
            return Directories.SelectMany(x => x.Value.GetAllDirectories()).Concat(new[]{this}).ToList();
        }

        public long Size()
        {
            if (size != null)
                return size!.Value;
            size = Directories.Select(x => x.Value.Size()).Sum() + Files.Select(x=>x.Value.Size).Sum();
            return size!.Value;
        }

        public void AddFile(List<string> path, FileInfo fileInfo)
        {
            if (path.Count == 0)
                Files[fileInfo.FileName] = fileInfo;
            else
            {
                if (!Directories.ContainsKey(path[0]))
                    Directories[path[0]] = new DirectoryInfo();
                Directories[path[0]].AddFile(path.Skip(1).ToList(), fileInfo);
            }
        }
    }

    public override void Solve()
    {
        var root = ReadDirectories();
        Console.WriteLine(Solve1(root));
        Console.WriteLine(Solve2(root));
    }

    private long Solve1(DirectoryInfo root)
    {
        long result = 0;
        foreach (var directory in root.GetAllDirectories())
        {
            long sum = directory.Size();
            if (sum <= 100000)
                result += sum;
        }
        return result;
    }
    
    private long Solve2(DirectoryInfo root)
    {
        var freeSpace = 70000000 - root.Size();
        long result = 70000000;
        foreach (var directory in root.GetAllDirectories())
        {
            long size = directory.Size();
            if (freeSpace + size >= 30000000 && result > size)
                result = size;
        }
        return result;
    }

    private DirectoryInfo ReadDirectories()
    {
        var lines = ReadLines();
        var root = new DirectoryInfo();
        var i = 0;
        var currentDirectory = new Stack<string>();
        while (i < lines.Count)
        {
            var line = lines[i];
            if (line.StartsWith("$ cd"))
            {
                var arg = line.Replace("$ cd ", "");
                if (arg == "/")
                    currentDirectory = new Stack<string>();
                else if (arg == "..")
                    currentDirectory.Pop();
                else
                    currentDirectory.Push(arg);
                i++;
            }
            else if (line.StartsWith("$ ls"))
            {
                var path = currentDirectory.Reverse().ToList();
                i++;
                for (; i < lines.Count && !lines[i].StartsWith("$"); i++)
                {
                    var args = lines[i].Split(" ");
                    if (args[0] != "dir")
                        root.AddFile(path, new FileInfo(args[1], long.Parse(args[0])));
                }
            }
            else
            {
                throw new Exception($"Unexpected input: {line}");
            }
        }

        return root;
    }
}