using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Communication;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ThreeChess.Services;

var browsers = new IWebDriver[3];
var random = new Random();
const int TotalLobbies = 1;
RedisLobbyManager redisLobbyManager = new RedisLobbyManager("localhost:6379,abortConnect=false,connectTimeout=5000");
var allLobbies = redisLobbyManager.GetAllLobbies().Take(1000).ToList();

try
{
    // Инициализация браузеров
    for (int i = 1; i < browsers.Length; i++)
    {
        browsers[i] = CreateChromeDriver();
        RegisterUser(browsers[i], i + 1);
    }

    // Параллельное выполнение для каждого браузера
    Parallel.For(1, browsers.Length, i =>
    {
        ProcessLobbiesInOrder(browsers[i], i + 1);
    });

    Parallel.For(1, browsers.Length, i =>
    {
        MakeMovesInAllTabs(browsers[i], i + 1);
    });
}
finally
{
    foreach (var driver in browsers)
    {
        driver?.Quit();
    }
}

void MakeMovesInAllTabs(IWebDriver driver, int playerNumber)
{
    while (true)
    {
        string mainTab = driver.CurrentWindowHandle;
        var tabs = driver.WindowHandles.ToList();

        foreach (var tab in tabs)
        {
            if (tab == mainTab) continue;

            try
            {
                driver.SwitchTo().Window(tab);
                MakeMove(driver, playerNumber);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Игрок {playerNumber}] Ошибка в игре (вкладка {tabs.IndexOf(tab)}): {ex.Message}");
            }
        }

        driver.SwitchTo().Window(mainTab);
    }

    
}

// Новый метод для выполнения хода в текущей вкладке
void MakeMove(IWebDriver driver, int playerNumber)
{
    try
    {
        // Получаем цвет текущего игрока
        string controlledColor = (string)((IJavaScriptExecutor)driver).ExecuteScript(
            "return gameConfig.controlledColor;");

        string currentTurnColor = (string)((IJavaScriptExecutor)driver).ExecuteScript(
            "return gameConfig.currentTurnColor;");

        // Проверяем, может ли игрок сделать ход
        if (currentTurnColor != controlledColor)
        {
            Console.WriteLine($"[Игрок {playerNumber}] Сейчас не наш ход ({controlledColor} vs {currentTurnColor})");
            return;
        }

        // Используем JavaScript для получения клеток с фигурами нужного цвета
        string getCellsScript = @"
            var color = arguments[0];
            var result = [];
            for (var cellId in boardElementsState.cells) {
                var cell = boardElementsState.cells[cellId];
                if (cell.elements.figure && 
                    cell.elements.figure.figureInfo && 
                    cell.elements.figure.figureInfo.figureColor === color) {
                    result.push(cellId);
                }
            }
            return result;
        ";

        // Получаем список ID клеток с фигурами нужного цвета
        var cellIds = (IReadOnlyCollection<object>)((IJavaScriptExecutor)driver)
            .ExecuteScript(getCellsScript, controlledColor);

        if (cellIds.Count == 0)
        {
            Console.WriteLine($"[Игрок {playerNumber}] Нет доступных фигур");
            return;
        }

        // Выбираем случайную клетку с фигурой
        var randomCellId = cellIds.ElementAt(random.Next(cellIds.Count));
        var cellElement = driver.FindElement(By.CssSelector($"path[data-cell-id='{randomCellId}']"));

        // Кликаем по клетке с фигурой через JavaScript
        ((IJavaScriptExecutor)driver).ExecuteScript(
            "arguments[0].dispatchEvent(new MouseEvent('click', { bubbles: true }));",
            cellElement);
        Thread.Sleep(20); // Ждем подсветки клеток

        // Ищем доступные ходы (подсвеченные клетки)
        var highlightedCells = driver.FindElements(By.CssSelector(
            "path.cell-highlighted, path.cell-capture-highlight"));

        if (highlightedCells.Count == 0)
        {
            Console.WriteLine($"[Игрок {playerNumber}] Нет доступных ходов");
            return;
        }

        // Выбираем случайную клетку и делаем ход
        var randomHighlightedCell = highlightedCells[random.Next(highlightedCells.Count)];

        // Кликаем по подсвеченной клетке через JavaScript
        ((IJavaScriptExecutor)driver).ExecuteScript(
            "arguments[0].dispatchEvent(new MouseEvent('click', { bubbles: true }));",
            randomHighlightedCell);

        Console.WriteLine($"[Игрок {playerNumber}] Сделан ход: {controlledColor} с {randomCellId} на {randomHighlightedCell.GetAttribute("data-cell-id")}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Игрок {playerNumber}] Ошибка при выполнении хода: {ex.Message}");
    }
}



IWebDriver CreateChromeDriver()
{
    var options = new ChromeOptions();
    options.AddArgument("--disable-notifications");
    options.AddArgument("--start-maximized");
    return new ChromeDriver(options);
}

void RegisterUser(IWebDriver driver, int playerNumber)
{
    var username = $"bot{playerNumber}_{GenerateRandomString(8)}";
    driver.Navigate().GoToUrl("http://localhost:7288/Account/Register");

    new WebDriverWait(driver, TimeSpan.FromSeconds(10))
        .Until(d => d.Title.Contains("Регистрация"));

    driver.FindElement(By.Id("username")).SendKeys(username);
    driver.FindElement(By.Id("email")).SendKeys($"{username}@test.com");
    driver.FindElement(By.Id("password")).SendKeys("Pa$$w0rd!");
    driver.FindElement(By.CssSelector("button[type='submit']")).Click();

    new WebDriverWait(driver, TimeSpan.FromSeconds(10))
        .Until(d => d.Url.Contains("/Home/Index"));
}

void ProcessLobbiesInOrder(IWebDriver driver, int playerNumber)
{
    for (int lobbyId = 1; lobbyId <= TotalLobbies; lobbyId++)
    {
        try
        {
            // Создаем новую вкладку для каждой игры
            ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
            var tabs = driver.WindowHandles;
            driver.SwitchTo().Window(tabs.Last());

            var lobby = allLobbies[lobbyId];
            JoinSpecificLobby(driver, lobby.Id, playerNumber);

            // Возвращаемся к основной вкладке
            driver.SwitchTo().Window(tabs.First());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Игрок {playerNumber}] Ошибка в лобби {lobbyId}: {ex.Message}");
        }
    }
    driver.SwitchTo().Window(driver.WindowHandles.First());
}

void JoinSpecificLobby(IWebDriver driver, Guid targetLobbyId, int playerNumber)
{
    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

    for (int attempt = 1; attempt <= 3; attempt++)
    {
        try
        {
            driver.Navigate().GoToUrl("http://localhost:7288/Lobby/AllLobbiesPage");
            wait.Until(d => d.FindElements(By.CssSelector(".lobby-card")).Count > 0);

            // Начало повторяемого блока (3 раза)
            var lobbies = driver.FindElements(By.CssSelector(".lobby-card"))
                .OrderBy(e => e.GetAttribute("data-lobby-id"))
                .ToList();

            var targetLobby = lobbies.FirstOrDefault(e =>
                e.GetAttribute("data-lobby-id") == targetLobbyId.ToString());

            if (targetLobby == null)
                throw new Exception($"Лобби {targetLobbyId} не найдено");

            var joinButton = targetLobby.FindElement(By.CssSelector(".btn-join"));

            if (joinButton.GetAttribute("disabled") == "true")
                throw new Exception($"Лобби {targetLobbyId} уже заполнено");
            // Конец повторяемого блока

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", joinButton);
            wait.Until(d => d.Url.Contains($"/Lobby/{targetLobbyId}"));
            Console.WriteLine($"[Игрок {playerNumber}] Успешно вошел в лобби {targetLobbyId}");
            wait.Until(d => d.Title.Contains("Игровая доска"));
            return;
        }
        catch (Exception ex) when (attempt < 3)
        {
            Console.WriteLine($"[Игрок {playerNumber}] Попытка {attempt}/3: {ex.Message}");
            driver.Navigate().Refresh();
            Thread.Sleep(500);
        }
    }
    throw new Exception($"Не удалось присоединиться к лобби {targetLobbyId} после 3 попыток");
}

string GenerateRandomString(int length)
{
    const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
    return new string(Enumerable.Repeat(chars, length)
        .Select(s => s[random.Next(s.Length)]).ToArray());
}