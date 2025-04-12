function firstClickHandler(cell) {
    const cellState = boardElementsState.cells[cell.id];

    // Если в ячейке есть фигура
    if (cellState.elements.figure) {
        clearHighlightedCells();

        const figure = cellState.elements.figure;

        if (figure.figureInfo.figureColor !== gameConfig.controlledColor) {
            console.error("Нельзя ходить чужими фигурами");
            return;
        }


        highlightMoves(cell);
    }
}

function secondClickHandler(cell) {
    const cellState = boardElementsState.cells[cell.id];

    if (last_click_id !== cell.id) {
        if (cellState.elements.figure) {
            const figure = cellState.elements.figure;
            if (figure.figureInfo.figureColor === gameConfig.controlledColor) {
                clearHighlightedCells();
                highlightMoves(cell);
                return;
            }
        }

        if (gameConfig.currentTurnColor === gameConfig.controlledColor) {
            localMove(last_click_id, cell.id);
        }
        

        last_click_id = undefined;
        clearHighlightedCells();
    }
}