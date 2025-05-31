function startTimerUpdates() {
    // Останавливаем предыдущий интервал, если был
    if (timerUpdateInterval) clearInterval(timerUpdateInterval);

    // Запускаем новый интервал обновления каждые 10 мс
    timerUpdateInterval = setInterval(() => {
        clientUpdateTimers();
    }, 100);
}

function clientUpdateTimers() {
    // Если нет времени последнего обновления - выходим
    if (!lastServerUpdateTime) return;

    // Рассчитываем прошедшее время
    const now = new Date();
    const elapsed = now - lastServerUpdateTime;

    // Находим текущего игрока
    const currentPlayerId = Object.keys(gameConfig.playerColors).find(
        id => gameConfig.playerColors[id] === gameConfig.currentTurnColor
    );

    // Обновляем только таймер текущего игрока
    if (currentPlayerId && gameConfig.playerGameTimes[currentPlayerId]) {
        // Вычисляем новое значение времени
        const currentTimeMs = timeSpanToMilliseconds(gameConfig.playerGameTimes[currentPlayerId]);
        const newTime = currentTimeMs - elapsed;

        // Обновляем отображение
        const color = gameConfig.playerColors[currentPlayerId];
        const el = document.getElementById(`timer-${color}`);
        if (el) el.textContent = formatMilliseconds(newTime);
    }
}

function updateTimers() {
    gameConfig.activePlayerIds.forEach(userId => {
        const el = document.getElementById(`timer-${gameConfig.playerColors[userId]}`);
        const ts = gameConfig.playerGameTimes[userId];
        if (el && ts) el.textContent = formatTimeSpan(ts);
    });
}


function formatTimeSpan(tsString) {
    const [timePart, fractionPart] = tsString.split('.');
    const [hh, mm, ss] = timePart.split(':').map(Number);
    const totalSeconds = hh * 3600 + mm * 60 + ss;

    let tenths = '0';
    if (fractionPart) {
        const ms = parseInt(fractionPart.padEnd(7, '0').slice(0, 7));
        tenths = Math.floor(ms / 100_000).toString();
    }

    const minutes = Math.floor(totalSeconds / 60).toString().padStart(2, '0');
    const seconds = (totalSeconds % 60).toString().padStart(2, '0');

    return `${minutes}:${seconds}.${tenths}`;
}


function formatMilliseconds(ms) {
    if (ms <= 0) return "00:00.0";

    const totalSeconds = ms / 1000;
    const minutes = Math.floor(totalSeconds / 60);
    const seconds = Math.floor(totalSeconds % 60);
    const tenths = Math.floor((ms % 1000) / 100);

    return `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}.${tenths}`;
}

function timeSpanToMilliseconds(timeSpan) {
    const parts = timeSpan.split(':');
    const hours = parseInt(parts[0]);
    const minutes = parseInt(parts[1]);
    const secondsParts = parts[2].split('.');
    const seconds = parseInt(secondsParts[0]);
    const milliseconds = secondsParts[1] ? parseInt(secondsParts[1].substring(0, 3)) : 0;

    return (hours * 3600 + minutes * 60 + seconds) * 1000 + milliseconds;
}


function formatTime(timestamp) {
    const date = new Date(timestamp);
    return `${date.getHours().toString().padStart(2, '0')}:${date.getMinutes().toString().padStart(2, '0')}`;
}