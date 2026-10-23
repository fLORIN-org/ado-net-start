# Занятие 0. Установка .NET 10 SDK и Docker

Инструкция: [установка .NET 10 SDK и Docker](https://algorthimization-course-vvodnoe.vercel.app/mdk0111-ado-net/ustanovka/index.html)  
Домашнее задание: [`homework/00-first-project`](../homework/00-first-project/README.md)

## Что изучили

1. Для занятий нужны три программы: .NET 10 SDK собирает и запускает программы на C#,
   расширение C# Dev Kit добавляет в VS Code подсветку и подсказки, Docker Desktop запускает контейнер с PostgreSQL.
2. SDK отличается от Runtime: Runtime только запускает готовые программы, SDK ещё и собирает их из исходного кода.
   Runtime входит в SDK.
3. .NET 10 — версия с долгой поддержкой (LTS), исправления выходят до ноября 2028 года.
   Несколько версий SDK работают рядом, нужная проекту версия записана в файле `.csproj`.
4. PostgreSQL работает в контейнере Docker. Описание контейнера лежит в `docker-compose.yml`,
   и файл у всех студентов один, поэтому база у всех одинаковая: те же таблицы и строки.
5. На Windows контейнеры работают внутри WSL 2 — подсистемы Linux, встроенной в Windows.
6. `docker compose up -d` запускает базу в фоне. При первом запуске скрипт `db/init.sql` создаёт
   таблицы и заполняет их. `docker compose down` удаляет контейнер, и следующий запуск создаёт базу заново.
7. Запись `5433->5432` в колонке PORTS означает: порт 5433 на компьютере ведёт к порту 5432 внутри контейнера.
   Порт 5433 выбран, чтобы контейнер не мешал PostgreSQL, уже установленному на компьютере.
8. Терминал читает список папок с программами (PATH) один раз при запуске. После установки SDK
   терминал и VS Code нужно открыть заново, иначе команда `dotnet` не найдётся.

## Термины

| Термин | Значение |
|---|---|
| SDK (Software Development Kit) | набор для разработки: собирает программы из исходного кода и запускает их |
| Runtime | среда выполнения: только запускает готовые программы |
| LTS | версия с долгой поддержкой, получает исправления три года |
| `.csproj` | файл проекта: версия .NET, тип программы, подключённые библиотеки |
| NuGet | каталог библиотек .NET; `dotnet run` скачивает из него пакеты проекта, например Npgsql |
| образ Docker | шаблон контейнера, например `postgres:17` |
| контейнер | запущенный образ: внутри уже установлен и настроен PostgreSQL |
| `docker-compose.yml` | описание контейнеров проекта: образ, настройки, порт |
| WSL 2 | подсистема Linux в Windows, в которой Docker запускает контейнеры |

## Пример: новый консольный проект

```bash
dotnet new console -n HelloApp     # папка HelloApp с HelloApp.csproj и Program.cs
cd HelloApp
dotnet run                         # Hello, World!
```

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```

`OutputType` `Exe` — программа запускается из терминала, `TargetFramework` `net10.0` — версия .NET.

## Шпаргалка

| Что сделать | Команда |
|---|---|
| список установленных SDK | `dotnet --list-sdks` |
| проверить Docker | `docker run hello-world` |
| запустить базу (в корне репозитория) | `docker compose up -d` |
| посмотреть контейнеры | `docker compose ps` |
| остановить базу и вернуть исходные строки | `docker compose down` |
| запустить программу (в папке проекта) | `dotnet run` |

Строка подключения к базе курса: `Host=localhost;Port=5433;Database=orders;Username=student;Password=student`.

## Частые ошибки

| Сообщение | Причина | Как исправить |
|---|---|---|
| `The term 'dotnet' is not recognized` или `command not found: dotnet` | терминал открыт до установки SDK, в его PATH нет папки dotnet | закрыть VS Code и все терминалы, открыть заново |
| `Cannot connect to the Docker daemon` или `error during connect` | Docker Desktop не запущен или ещё запускается | запустить Docker Desktop, дождаться надписи Engine running |
| `Bind for 0.0.0.0:5433 failed: port is already allocated` | порт 5433 занят другим контейнером | `docker ps` покажет, кто занял порт; остановить: `docker stop <имя>` |
| `Failed to connect to 127.0.0.1:5433`, ниже `Connection refused` | контейнер с базой не запущен | `docker compose up -d` в корне репозитория, проверить `docker compose ps` |
| `28000: role "student" does not exist` | программа подключилась к другому PostgreSQL на порту 5432 | `Port=5433` в строке подключения |
| `error NU1301: Unable to load the service index for source https://api.nuget.org/v3/index.json` | нет доступа к каталогу NuGet | проверить интернет; в сети колледжа сообщить преподавателю |
| `Virtualization support not detected` | в BIOS выключена виртуализация, без неё WSL 2 не работает | на своём компьютере включить в BIOS Intel VT-x или AMD-V (SVM); в колледже сообщить преподавателю |
