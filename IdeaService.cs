using System;
using System.IO;
using System.Linq;

namespace InspirationRecorder
{
    public class IdeaService
    {
        private readonly Config _config;

        public IdeaService(Config config)
        {
            _config = config;
        }

        public void SaveIdea(string content)
        {
            var today = DateTime.Now;
            var dateHeader = $"# {today:yyyy-MM-dd}";
            var formattedContent = $"- {today:HH:mm} {content}";
            var filePath = _config.MarkdownFilePath;
            
            // 确保文件夹存在
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(filePath)));
            
            // 如果文件不存在，创建文件
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, $"{dateHeader}\n{formattedContent}\n");
                return;
            }

            var lines = File.ReadAllLines(filePath).ToList();
            var headerLineIndex = lines.FindIndex(line => 
                line.StartsWith("# ") && line != dateHeader);

            // 如果找不到其他日期的标题，就在文件开头添加
            if (headerLineIndex == -1)
            {
                if (!lines.Any() || lines[0] != dateHeader)
                {
                    lines.Insert(0, dateHeader);
                    lines.Insert(1, formattedContent);
                }
                else
                {
                    lines.Insert(1, formattedContent);
                }
            }
            else
            {
                // 如果已有今天的标题
                if (headerLineIndex > 0 && lines[headerLineIndex - 1] == dateHeader)
                {
                    lines.Insert(headerLineIndex, formattedContent);
                }
                else
                {
                    // 在其他日期标题之前插入新的日期和内容
                    lines.Insert(headerLineIndex, dateHeader);
                    lines.Insert(headerLineIndex + 1, formattedContent);
                }
            }

            File.WriteAllLines(filePath, lines);
        }
    }
} 