# Система управления корпоративным складом (Corporate Warehouse Management System)
![Typing SVG](https://readme-typing-svg.herokuapp.com?color=%2336BCF7&lines=Система+управления+корпоративным+складом)

[![GitHub License](https://img.shields.io/github/license/MelFxrd/Corporate-Warehouse-Management-System)](https://github.com/MelFxrd/Corporate-Warehouse-Management-System/blob/main/LICENSE)
[![GitHub Issues](https://img.shields.io/github/issues/MelFxrd/Corporate-Warehouse-Management-System)](https://github.com/MelFxrd/Corporate-Warehouse-Management-System/issues)
[![GitHub Stars](https://img.shields.io/github/stars/MelFxrd/Corporate-Warehouse-Management-System)](https://github.com/MelFxrd/Corporate-Warehouse-Management-System/stargazers)
[![Build Status](https://img.shields.io/github/actions/workflow/status/MelFxrd/Corporate-Warehouse-Management-System/ci.yml)](https://github.com/MelFxrd/Corporate-Warehouse-Management-System/actions)
[![Code Coverage](https://img.shields.io/codecov/c/github/MelFxrd/Corporate-Warehouse-Management-System)](https://codecov.io/gh/MelFxrd/Corporate-Warehouse-Management-System)

Это десктопное приложение для управления инвентарем склада, предоставляющее полный набор функций для отслеживания товаров, включая добавление, редактирование, удаление и поиск. Система использует PostgreSQL для хранения данных и автоматически ведет журнал всех операций для аудита и анализа.

## Содержание

- [Технологии](#технологии)
- [Функции](#функции)
- [Установка и настройка](#установка-и-настройка)
- [Использование](#использование)
- [Команда проекта](#команда-проекта)
- [Источники](#источники)

## Технологии

Проект построен с использованием следующих технологий:

- **.NET 9** - Основная платформа для разработки
- **C#** - Язык программирования
- **WPF (Windows Presentation Foundation)** - Фреймворк для пользовательского интерфейса
- **Entity Framework Core** - ORM для работы с базой данных
- **PostgreSQL** - Реляционная база данных для хранения данных

## Функции

- **Управление товарами**: Полный CRUD (Create, Read, Update, Delete) для товаров склада.
- **Удобная таблица**: Просмотр всех товаров в удобной сортируемой таблице с колонками ID, название, количество и цена.
- **Поиск товаров**: Быстрый поиск по названию товара с помощью встроенной строки поиска.
- **Журналирование операций**: Автоматическая запись всех операций создания, обновления и удаления с временными метками.
- **История изменений**: Отдельное окно для просмотра полного журнала операций.
- **Алерты на низкий запас**: Автоматические всплывающие уведомления при снижении количества товара ниже критического уровня (150 единиц).

## Установка и настройка

### Требования

Для установки и запуска проекта необходимы:
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- Работающий сервер [PostgreSQL](https://www.postgresql.org/download/)

### Шаги установки

1. **Клонируйте репозиторий:**
   ```sh
   git clone https://github.com/MelFxrd/Corporate-Warehouse-Management-System.git
   cd Corporate-Warehouse-Management-System
   ```

2. **Настройте подключение к базе данных:**
   - Создайте базу данных PostgreSQL с именем `WarehouseDB`.
   - Откройте решение в Visual Studio.
   - Обновите строку подключения в файле `Warehouse Management System/Data/WarehouseDbContext.cs` с вашими учетными данными PostgreSQL:

     ```csharp
     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
     {
         // Замените на ваши данные: хост, имя пользователя и пароль
         optionsBuilder.UseNpgsql("Host=localhost;Database=WarehouseDB;Username=YOUR_USERNAME;Password=YOUR_PASSWORD");
     }
     ```

3. **Примените миграции базы данных:**
   - Откройте Package Manager Console в Visual Studio (`View -> Other Windows -> Package Manager Console`).
   - Выполните команду для применения миграций и создания схемы базы данных:
     ```powershell
     Update-Database
     ```

4. **Запустите приложение:**
   - Установите `Warehouse Management System` как стартовый проект.
   - Нажмите `F5` или кнопку "Start" в Visual Studio для сборки и запуска приложения.

## Использование

- **Главное окно**: Приложение открывается на главном дашборде, отображающем список всех товаров в инвентаре.
- **Добавить товар**: Нажмите кнопку "Добавить товар" для открытия формы ввода данных нового товара.
- **Редактировать товар**: Выберите товар в сетке и нажмите "Редактировать товар" для изменения его данных.
- **Удалить товар**: Выберите товар и нажмите "Удалить товар" для его удаления из инвентаря.
- **Поиск**: Введите название товара в строку поиска и нажмите "Поиск" для фильтрации списка.
- **Просмотр истории**: Нажмите кнопку "История" для открытия окна с хронологической записью всех операций.

## Commits

- [x] Добавить базовую функциональность CRUD
- [x] Реализовать поиск товаров
- [x] Добавить журналирование операций
- [x] Интегрировать алерты на низкий запас
- [ ] Добавить поддержку экспорта данных
- [ ] Реализовать многопользовательский режим
- [ ] Оптимизировать производительность для больших объемов данных

## Команда проекта

- **MelFxrd** — Разработчик проекта  
  Свяжитесь со мной: [GitHub](https://github.com/MelFxrd) | [Twitch](https://twitch.tv/melfxrd) | [YouTube](https://youtube.com/channel/melfxrd)

Если у вас есть вопросы или предложения, создайте issue в репозитории или свяжитесь со мною напрямую.

## Источники

- [Документация Microsoft по WPF](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/)
- [Документация Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [Руководство по PostgreSQL](https://www.postgresql.org/docs/)

---

<p align="right">(<a href="#top">вернуться наверх</a>)</p>