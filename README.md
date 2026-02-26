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
- [Архитектура проекта](#архитектура-проекта)
- [Установка и настройка](#установка-и-настройка)
- [Использование](#использование)
- [Команда проекта](#команда-проекта)
- [Источники](#источники)

## Технологии

Проект построен с использованием следующих технологий:

- **.NET 9** — основная платформа для разработки
- **C#** — язык программирования
- **WPF (Windows Presentation Foundation)** — фреймворк для пользовательского интерфейса
- **Entity Framework Core** — ORM для работы с базой данных
- **PostgreSQL** — реляционная база данных для хранения данных
- **ClosedXML** — библиотека для экспорта данных в Excel (.xlsx)
- **LiveChartsCore + SkiaSharp** — библиотека для построения диаграмм и графиков

## Функции

- **Управление товарами**: полный CRUD (Create, Read, Update, Delete) для товаров склада.
- **Категории товаров**: создание, редактирование и удаление категорий; фильтрация товаров по категории прямо из главного окна.
- **Удобная таблица**: просмотр всех товаров в сортируемой таблице с колонками ID, название, количество, цена и категория.
- **Поиск и фильтрация**: быстрый поиск по названию товара в сочетании с фильтром по категории; кнопка сброса для мгновенного возврата к полному списку.
- **Журналирование операций**: автоматическая запись всех операций создания, обновления и удаления с временными метками.
- **История изменений**: отдельное окно для просмотра полного журнала операций в хронологическом порядке.
- **Алерты на низкий запас**: автоматические всплывающие уведомления при снижении количества товара ниже критического уровня (150 единиц).
- **Экспорт в Excel**: асинхронный экспорт всего инвентаря в файл `.xlsx` на рабочий стол; таблица включает колонки ID, название, количество, цена и категория.
- **Отчёт по категориям**: круговая диаграмма (Pie Chart) с распределением количества товаров по категориям, открывается в отдельном окне.
- **Тёмная и светлая тема**: переключение темы оформления прямо из главного окна с помощью ToggleButton.
- **Кастомные диалоги**: собственные окна сообщений (`CustomMessageBox`) и подтверждений (`ConfirmDialog`), стилизованные под текущую тему.

## Архитектура проекта

```
Warehouse Management System/
├── Data/
│   └── WarehouseDbContext.cs       # Контекст Entity Framework Core
├── Models/
│   ├── Product.cs                  # Модель товара
│   ├── Category.cs                 # Модель категории
│   ├── Log.cs                      # Модель записи журнала
│   └── Order.cs                    # Модель заказа
├── ViewModels/
│   └── ProductViewModel.cs         # ViewModel для главного окна
├── Views/
│   └── ConfirmDialog.xaml(.cs)     # Диалог подтверждения действия
├── Themes/
│   ├── LightTheme.xaml             # Светлая тема
│   └── DarkTheme.xaml              # Тёмная тема
├── MainWindow.xaml(.cs)            # Главное окно приложения
├── AddProductWindow.xaml(.cs)      # Форма добавления товара
├── EditProductWindow.xaml(.cs)     # Форма редактирования товара
├── CategoryWindow.xaml(.cs)        # Управление категориями
├── LogWindow.xaml(.cs)             # История операций
├── ReportWindow.xaml(.cs)          # Отчёт с круговой диаграммой
├── CustomMessageBox.xaml(.cs)      # Кастомное окно сообщений
└── App.xaml(.cs)                   # Точка входа и глобальные стили
```

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

- **Главное окно**: приложение открывается на главном дашборде со списком всех товаров.
- **Добавить товар**: нажмите "Добавить товар" для открытия формы ввода; обязательно выберите категорию из выпадающего списка.
- **Редактировать товар**: выберите товар в таблице и нажмите "Редактировать товар".
- **Удалить товар**: выберите товар и нажмите "Удалить товар"; появится диалог подтверждения.
- **Поиск**: введите название в строку поиска и/или выберите категорию из фильтра, затем нажмите "Поиск". Для сброса нажмите "Сброс".
- **Управление категориями**: нажмите "Упр. категориями" для открытия окна создания, редактирования и удаления категорий.
- **История операций**: нажмите "История" для просмотра журнала всех действий с временными метками.
- **Отчёт**: нажмите "Отчёт" для открытия круговой диаграммы с распределением товаров по категориям.
- **Экспорт в Excel**: нажмите "Экспорт в Excel" — файл `Отчёт_склада.xlsx` сохранится на рабочий стол.
- **Смена темы**: переключите ToggleButton в правом углу панели кнопок для смены светлой/тёмной темы.

## Commits

- [x] Добавить базовую функциональность CRUD
- [x] Реализовать поиск товаров
- [x] Добавить журналирование операций
- [x] Интегрировать алерты на низкий запас товаров
- [x] Добавить систему категорий с управлением и фильтрацией
- [x] Добавить поддержку экспорта данных в Excel (ClosedXML)
- [x] Добавить тёмную тему (переключение на лету)
- [x] Добавить отчёт по категориям (круговая диаграмма, LiveChartsCore)
- [x] Реализовать кастомные диалоги (CustomMessageBox, ConfirmDialog)

## Команда проекта

- **MelFxrd** — Разработчик проекта  
  Свяжитесь со мной: [GitHub](https://github.com/MelFxrd) | [Twitch](https://twitch.tv/melfxrd) | [YouTube](https://www.youtube.com/@melfxrd/featured)

Если хотите заказать у меня озвучку, то вот мой тг: [Telegram](https://t.me/melfxrd)

## Источники

- [Документация Microsoft по WPF](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/)
- [Документация Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [Руководство по PostgreSQL](https://www.postgresql.org/docs/)
- [ClosedXML на GitHub](https://github.com/ClosedXML/ClosedXML)
- [LiveChartsCore — документация](https://livecharts.dev/)

---

<p align="right">(<a href="#top">вернуться наверх</a>)</p>