using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ThreeChess.Services;

var browsers = new IWebDriver[3];
var random = new Random();
const int TotalLobbies = 30;
RedisLobbyManager redisLobbyManager = new RedisLobbyManager("localhost:6379,abortConnect=false,connectTimeout=5000");
var allLobbies = redisLobbyManager.GetAllLobbies().ToList();

try
{
    // Инициализация браузеров
    for (int i = 0; i < browsers.Length; i++)
    {
        browsers[i] = CreateChromeDriver();
        RegisterUser(browsers[i], i + 1);
    }

    // Параллельное выполнение для каждого браузера
    Parallel.For(0, browsers.Length, i =>
    {
        ProcessLobbiesInOrder(browsers[i], i + 1);
    });

}
finally
{
    foreach (var driver in browsers)
    {
        driver?.Quit();
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
    Task.Delay(1000000).Wait();
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