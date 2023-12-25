using System;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

public interface ILoggerRepository
{
    void LogMessage(string message);
}

public class FileLoggerRepository : ILoggerRepository
{
    private readonly string filePath;

    public FileLoggerRepository(string filePath)
    {
        this.filePath = filePath;
    }

    public void LogMessage(string message)
    {
        File.AppendAllText(filePath, $"{DateTime.Now}: {message}\n");
    }
}

public class JsonLoggerRepository : ILoggerRepository
{
    private readonly string filePath;

    public JsonLoggerRepository(string filePath)
    {
        this.filePath = filePath;
    }

    public void LogMessage(string message)
    {
        var logEntry = new { Timestamp = DateTime.Now, Message = message };
        var serializedLogEntry = JsonConvert.SerializeObject(logEntry, (Newtonsoft.Json.Formatting)System.Xml.Formatting.Indented);

        File.AppendAllText(filePath, $"{serializedLogEntry}\n");
    }
}

public class MyLogger
{
    private readonly ILoggerRepository repository;

    public MyLogger(ILoggerRepository repository)
    {
        this.repository = repository;
    }

    public void Log(string message)
    {
        // Дополнительная логика, если необходимо
        repository.LogMessage(message);
    }
}

public class Logger
{
    public static void Main()
    {
        // Пример использования

        // Текстовый файл
        var fileLogger = new MyLogger(new FileLoggerRepository("log.txt"));
        fileLogger.Log("This is a log message to a text file");

        // JSON-файл
        var jsonLogger = new MyLogger(new JsonLoggerRepository("log.json"));
        jsonLogger.Log("This is a log message to a JSON file");
    }
}
