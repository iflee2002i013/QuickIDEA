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
            
            // 查找今天的日期标题
            var todayHeaderIndex = lines.FindIndex(line => line.Trim() == dateHeader);
            
            if (todayHeaderIndex != -1)
            {
                // 如果找到今天的日期标题，在其下方插入新内容
                lines.Insert(todayHeaderIndex + 1, formattedContent);
            }
            else
            {
                // 如果没有找到今天的日期标题，在文件开头添加
                lines.Insert(0, "");  // 添加一个空行
                lines.Insert(0, formattedContent);
                lines.Insert(0, dateHeader);
            }

            File.WriteAllLines(filePath, lines);
        }
    }
} 