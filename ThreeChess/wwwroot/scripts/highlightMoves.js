function highlightMoves(cell) {
    const piece = boardElementsState.cells[cell.id].elements.figure;
    // Проверяем тип фигуры (сравнение с ожидаемыми строковыми значениями, например 'Queen' или 'Bishop')
    if (piece.figureInfo.figureType === 'Queen'
        || piece.figureInfo.figureType === 'Bishop'
        || piece.figureInfo.figureType === 'King') {
        // Выделяем диагональные ячейки, доступные для хода
        cells = getDiagonalMoves(cell.id);
        highlightCellsArray(cells);
    }

    if (piece.figureInfo.figureType === 'Queen'
        || piece.figureInfo.figureType == 'Rook'
        || piece.figureInfo.figureType === 'King') {
        cells = getMainLinesMoves(cell.id);
        highlightCellsArray(cells);
    }

    if (piece.figureInfo.figureType === 'Queen'
        || piece.figureInfo.figureType == 'Rook'
        || piece.figureInfo.figureType === 'King') {
        cells = getSecondaryLinesMoves(cell.id);
        highlightCellsArray(cells);
    }

    if (piece.figureInfo.figureType === 'Knight') {
        cells = getKnightMoves(cell.id);
        highlightCellsArray(cells);
    }

    if (piece.figureInfo.figureType === 'King') {
        cells = getKingCastlingMoves(cell.id);
        highlightCellsArray(cells);
    }

    if (piece.figureInfo.figureType === 'Pawn') {
        cells = getPawnMoves(cell.id);
        highlightCellsArray(cells);
    }

    last_click_id = cell.id;
}

function highlightCellsArray(cellIds) {
    cellIds.forEach(cellId => highlightCell(cellId))
}

function highlightCell(targetCellId) {
    const cell = boardElementsState.cells[targetCellId];
    if (!cell) {
        console.error(`нет клетки с id ${targetCellId}`)
    }
    if (cell.elements.figure) {
        cell.elements.path.classList.add('cell-capture-highlight');
    } else {
        cell.elements.path.classList.add('cell-highlighted');
    }
}

function clearHighlightedCells() {
    document.querySelectorAll('path').forEach(path => {
        path.classList.remove('cell-highlighted');
        path.classList.remove('cell-capture-highlight');
    });
}