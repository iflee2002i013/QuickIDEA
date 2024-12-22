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
            var now = DateTime.Now;
            var dateStr = now.ToString("yyyy-MM-dd");
            var timeStr = now.ToString("HH:mm");
            var formattedContent = $"- {timeStr} {content}";
            
            var filePath = _config.MarkdownFilePath;
            
            // 确保文件夹存在
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(filePath)));
            
            // 如果文件不存在，创建文件
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, $"# {dateStr}\n{formattedContent}\n");
                return;
            }

            var lines = File.ReadAllLines(filePath).ToList();
            
            // 查找今天的日期标题
            var todayHeaderIndex = lines.FindIndex(line => line == $"# {dateStr}");
            
            if (todayHeaderIndex != -1)
            {
                // 如果找到今天的日期标题，在其下方插入内容
                lines.Insert(todayHeaderIndex + 1, formattedContent);
            }
            else
            {
                // 如果没有找到今天的日期标题，在第一个日期标题之前插入
                var anyHeaderIndex = lines.FindIndex(line => line.StartsWith("# "));
                
                if (anyHeaderIndex == -1)
                {
                    // 如果没有任何日期标题，添加到文件开头
                    lines.Insert(0, $"# {dateStr}");
                    lines.Insert(1, formattedContent);
                }
                else
                {
                    // 在第一个日期标题之前插入
                    lines.Insert(anyHeaderIndex, $"# {dateStr}");
                    lines.Insert(anyHeaderIndex + 1, formattedContent);
                }
            }

            File.WriteAllLines(filePath, lines);
        }
    }
} 