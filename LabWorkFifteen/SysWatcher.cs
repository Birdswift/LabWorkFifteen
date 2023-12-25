using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

// Интерфейс наблюдателя
public interface IObserver
{
    void Update(string fileName);
}

// Класс конкретного наблюдателя
public class FileObserver : IObserver
{
    public string Name { get; }

    public FileObserver(string name)
    {
        Name = name;
    }

    public void Update(string fileName)
    {
        Console.WriteLine($"Observer '{Name}' detected changes in file: {fileName}");
    }
}

// Класс субъекта (наблюдаемого объекта)
public class FileSubject
{
    private readonly List<IObserver> observers = new List<IObserver>();
    private string lastModifiedFile;

    public void Attach(IObserver observer)
    {
        observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.Update(lastModifiedFile);
        }
    }

    public void SimulateFileChange(string fileName)
    {
        lastModifiedFile = fileName;
        NotifyObservers();
    }
}

// Класс для проверки состояния директории по таймеру
public class DirectoryWatcher
{
    private readonly string directoryPath;
    private readonly FileSubject fileSubject;

    public DirectoryWatcher(string directoryPath, FileSubject fileSubject)
    {
        this.directoryPath = directoryPath;
        this.fileSubject = fileSubject;
    }

    public void StartWatching(int pollingIntervalMs)
    {
        // Запускаем таймер для проверки состояния директории
        Timer timer = new Timer(CheckDirectory, null, 0, pollingIntervalMs);
    }

    private void CheckDirectory(object state)
    {
        // Проверяем состояние директории (здесь просто симулируем изменение файла)
        var changedFiles = Directory.GetFiles(directoryPath).OrderBy(f => new FileInfo(f).LastWriteTime).ToList();
        if (changedFiles.Any())
        {
            // Берем последний измененный файл
            var lastModifiedFile = changedFiles.Last();
            fileSubject.SimulateFileChange(lastModifiedFile);
        }
    }
}

class SysWatcher
{
    static void Main()
    {
      
        var fileSubject = new FileSubject();

        var observer1 = new FileObserver("Observer 1");
        var observer2 = new FileObserver("Observer 2");

        fileSubject.Attach(observer1);
        fileSubject.Attach(observer2);

        var directoryWatcher = new DirectoryWatcher(@"C:\Users\gkras\OneDrive\Рабочий стол", fileSubject);


        directoryWatcher.StartWatching(5000);

        // Просто для демонстрации, симулируем изменение файла
        Console.WriteLine("Simulating file change...");
        fileSubject.SimulateFileChange(@"C:\Users\gkras\OneDrive\Рабочий стол\ticker.txt");

        Console.ReadLine();
    }
}
