async function loadCells(gameId) {
    try {
        const [gameConfigResponse] = await Promise.all([
            fetch(`/Game/game-config?gameId=${gameId}`)
        ]);

        const gameConf = await gameConfigResponse.json();

        console.log("Конфигурация игры:", gameConf);

        movedElements.diagonals = gameConf.diagonals;
        movedElements.mainLines = gameConf.mainLines;
        movedElements.secondaryLines = gameConf.secondaryLines;

        gameConfig.controlledColor = gameConf.controlledColor;
        gameConfig.currentTurnColor = gameConf.currentTurnColor;

        gameConfig.activePlayerIds = gameConf.activePlayerIds;       
        gameConfig.playerColors = gameConf.playerColors;
        gameConfig.playerGameTimes = gameConf.playerGameTimes;
        gameConfig.userId = gameConf.userId;
        gameConfig.moveHistory = gameConf.moveHistory;
        gameConfig.playerInfos = gameConf.playerInfos;

        if (gameConf.gameStatus !== "Wait") {
            lastServerUpdateTime = new Date();
        }


        renderBoard(gameConf.cellsLocation, gameConf.figuresLocation);

        updateTimers();
        startTimerUpdates();
        initialSetup();
    } catch (error) {
        console.error('Error:', error);
    }
}

function initialSetup() {
    higlightKingAtCheck();
}