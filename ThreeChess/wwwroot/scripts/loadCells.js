async function loadCells() {
    try {
        const [gameConfigResponse] = await Promise.all([
            fetch('/api/game-config')
        ]);

        const gameConf = await gameConfigResponse.json();

        console.log("Клетки:", gameConf.cellsLocation);
        console.log("Фигуры:", gameConf.figuresLocation);
        console.log("Диагонали:", gameConf.diagonals);
        console.log("Главные линии:", gameConf.mainLines);
        console.log("Побочные линии:", gameConf.secondaryLines);

        movedElements.diagonals = gameConf.diagonals;
        movedElements.mainLines = gameConf.mainLines;
        movedElements.secondaryLines = gameConf.secondaryLines;

        gameConfig.controlledColor = gameConf.controlledColor;
        gameConfig.currentTurnColor = gameConf.currentTurnColor;



        renderBoard(gameConf.cellsLocation, gameConf.figuresLocation);
    } catch (error) {
        console.error('Error:', error);
    }
}