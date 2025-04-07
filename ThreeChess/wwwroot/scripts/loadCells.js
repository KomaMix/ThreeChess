async function loadCells() {
    try {
        const [gameConfigResponse] = await Promise.all([
                fetch('/api/game-config')
            ]);

        //const cells = await cellsResponse.json();
        //const figuresMap = await figuresResponse.json();
        //const diagonals = await diagonalsResponse.json();
        //const mainLines = await mainLinesResponse.json();
        //const secondaryLines = await secondaryLinesResponse.json();
        const gameConf = await gameConfigResponse.json();

        console.log("Клетки:", gameConf.cellsLocation);
        console.log("Фигуры:", gameConf.figuresLocation);
        console.log("Диагонали:", gameConf.diagonals);
        console.log("Главные линии:", gameConf.mainLines);
        console.log("Побочные линии:", gameConf.secondaryLines);

        movedElements.diagonals = gameConf.diagonals;
        movedElements.mainLines = gameConf.mainLines;
        movedElements.secondaryLines = gameConf.secondaryLines;

        gameConfig.color = gameConf.color;



        renderBoard(gameConf.cellsLocation, gameConf.figuresLocation);
    } catch (error) {
        console.error('Error:', error);
    }
}