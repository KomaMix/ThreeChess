async function loadCells() {
    try {
        const [cellsResponse,
            figuresResponse,
            diagonalsResponse,
            mainLinesResponse,
            secondaryLinesResponse,
            gameConfigResponse] = await Promise.all([
                fetch('/api/cells-location'),
                fetch('/api/figures-location'),
                fetch('/api/diagonals'),
                fetch('/api/main-lines'),
                fetch('/api/secondary-lines'),
                fetch('/api/game-config')
            ]);

        const cells = await cellsResponse.json();
        const figuresMap = await figuresResponse.json();
        const diagonals = await diagonalsResponse.json();
        const mainLines = await mainLinesResponse.json();
        const secondaryLines = await secondaryLinesResponse.json();
        const gameConf = await gameConfigResponse.json();

        console.log("Клетки:", cells);
        console.log("Фигуры:", figuresMap);
        console.log("Диагонали:", diagonals);
        console.log("Главные линии:", mainLines);

        movedElements.diagonals = diagonals;
        movedElements.mainLines = mainLines;
        movedElements.secondaryLines = secondaryLines;

        gameConfig.color = gameConf.color;



        renderBoard(cells, figuresMap);
    } catch (error) {
        console.error('Error:', error);
    }
}