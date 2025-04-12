function firstClickHandler(cell) {
    // Если в ячейке есть фигура
    if (boardElementsState.cells[cell.id].elements.figure) {
        clearHighlightedCells();

        const cellState = boardElementsState.cells[cell.id];
        if (cellState.elements.figure.figureInfo.figureColor !== gameConfig.controlledColor) {
            console.error("Нельзя ходить чужими фигурами");
            return;
        }


        const piece = boardElementsState.cells[cell.id].elements.figure;
        // Проверяем тип фигуры (сравнение с ожидаемыми строковыми значениями, например 'Queen' или 'Bishop')
        if (piece.figureInfo.figureType === 'Queen'
            || piece.figureInfo.figureType === 'Bishop'
            || piece.figureInfo.figureType === 'King') {
            // Выделяем диагональные ячейки, доступные для хода
            highlightDiagonalMoves(cell.id, piece.figureInfo.figureType);
        }

        if (piece.figureInfo.figureType === 'Queen'
            || piece.figureInfo.figureType == 'Rook'
            || piece.figureInfo.figureType === 'King') {
            highlightMainLinesMoves(cell.id, piece.figureInfo.figureType);
        }

        if (piece.figureInfo.figureType === 'Queen'
            || piece.figureInfo.figureType == 'Rook'
            || piece.figureInfo.figureType === 'King') {
            highlightSecondaryLinesMoves(cell.id, piece.figureInfo.figureType);
        }

        if (piece.figureInfo.figureType === 'Knight') {
            highlightKnightMoves(cell.id);
        }

        if (piece.figureInfo.figureType === 'Pawn') {
            highlightPawnMoves(cell.id);
        }

        last_click_id = cell.id;
    }
}

function secondClickHandler(cell) {
    if (last_click_id !== cell.id) {


        localMove(last_click_id, cell.id);

        last_click_id = undefined;
        clearHighlightedCells();
    }
}