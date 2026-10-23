# ДЗ 0. Первый проект на .NET

Задание выполняется до занятия 1. В нём вы устанавливаете программы для курса, создаёте
и запускаете маленький проект на C#, а затем проверяете, что база заказов запускается в Docker.

Инструкция по установке: [установка .NET 10 SDK и Docker](https://algorthimization-course-vvodnoe.vercel.app/mdk0111-ado-net/ustanovka/index.html)  
Конспект: [`lectures/lesson-00-setup.md`](../../lectures/lesson-00-setup.md)

## Задание 1. Установить программы

Установите по инструкции .NET 10 SDK, расширение C# Dev Kit для VS Code и Docker Desktop.
Закройте и снова откройте VS Code, в терминале выполните:

```bash
dotnet --list-sdks
docker run hello-world
```

В списке SDK должна быть версия, которая начинается с `10.0`. Docker должен напечатать
строку `Hello from Docker!`. Вывод `dotnet --list-sdks` и строку `Hello from Docker!` скопируйте в отчёт.

## Задание 2. Создать и запустить проект HelloApp

1. Откройте в VS Code свой репозиторий `ado-net-start` и нажмите **Sync Changes**: появится эта папка.
2. Откройте терминал VS Code (`` Ctrl + ` ``) и перейдите в папку задания:

   ```bash
   cd homework/00-first-project
   ```

3. Создайте консольный проект. Команда создаёт папку `HelloApp` с файлами `HelloApp.csproj` и `Program.cs`:

   ```bash
   dotnet new console -n HelloApp
   ```

4. Запустите проект:

   ```bash
   cd HelloApp
   dotnet run
   ```

   Программа печатает `Hello, World!`. Эту строку команда `dotnet new console` записала в `Program.cs` при создании проекта.

5. Замените содержимое `HelloApp/Program.cs` программой ниже и впишите свои имя и группу:

   ```csharp
   // Русские буквы в терминале Windows.
   Console.OutputEncoding = System.Text.Encoding.UTF8;

   string name = "Анна Смирнова";
   string group = "ИСП-31";

   Console.WriteLine($"Привет! Меня зовут {name}, группа {group}.");
   Console.WriteLine($"Версия .NET: {Environment.Version}");
   Console.WriteLine($"Система: {System.Runtime.InteropServices.RuntimeInformation.OSDescription}");
   ```

6. Снова выполните `dotnet run`. Пример вывода:

   ```text
   Привет! Меня зовут Анна Смирнова, группа ИСП-31.
   Версия .NET: 10.0.13
   Система: Microsoft Windows 10.0.26100
   ```

   Версия .NET и название системы у вас будут свои: они зависят от обновлений и компьютера.

## Задание 3. Запустить базу заказов и OrdersApp

База запускается из корня репозитория, программа для проверки — заготовка практической работы 1.
Её код в этом задании не меняется.

```bash
cd ../../..                         # из HelloApp в корень репозитория
docker compose up -d
docker compose ps
cd practice/01-first-query/OrdersApp
dotnet run
```

`docker compose ps` показывает контейнер `orders-db` со статусом `Up` и портом `0.0.0.0:5433->5432/tcp`.
OrdersApp печатает 12 заказов, первая строка — `Заказ 1: shipped, 3380,00 руб.`
Разделитель в суммах зависит от языка системы: запятая или точка.
После работы базу можно остановить командой `docker compose down` в корне репозитория.

Если появилась ошибка, найдите её текст в таблице «Частые ошибки» в инструкции по установке
или в конспекте [`lesson-00-setup.md`](../../lectures/lesson-00-setup.md).

## Что сдать

| Файл | Что в нём |
|---|---|
| `HelloApp/Program.cs` | программа из задания 2 с вашими именем и группой |
| `HelloApp/HelloApp.csproj` | файл проекта, созданный командой `dotnet new console` |
| `report.md` | заполненный отчёт: вместо каждого `___` — вывод команды или ответ |

Папки `HelloApp/bin/` и `HelloApp/obj/` в коммит не попадают: их исключает `.gitignore`.

Коммит: `homework 00: первый проект HelloApp`.
