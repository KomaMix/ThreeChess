function highlightDiagonalMoves(cellId, figureType) {
    isKing = false;

    if (figureType === 'King') {
        isKing = true;
    }

    // Получаем массив диагоналей из глобального состояния
    const diagonals = movedElements.diagonals; // предполагается, что это массив массивов строк

    // Фильтруем только те диагонали, в которых присутствует текущая ячейка
    const relevantDiagonals = diagonals.filter(diagonal => diagonal.includes(cellId));

    console.log("relevantDiagonals:", relevantDiagonals);

    relevantDiagonals.forEach(diagonal => {
        const currentIndex = diagonal.indexOf(cellId);

        // Проходим в сторону начала диагонали
        for (let i = currentIndex - 1; i >= 0; i--) {
            const targetCellId = diagonal[i];
            const cell = boardElementsState.cells[targetCellId];
            if (!cell) break;
            // Если в ячейке есть фигура, выделяем её (как потенциальное взятие) и прекращаем поиск в этом направлении
            if (cell.elements.figure) {
                cell.elements.path.classList.add('cell-highlighted');
                console.log(`added light ${targetCellId}`);
                break;
            } else {
                cell.elements.path.classList.add('cell-highlighted');
                console.log(`added light ${targetCellId}`);
            }

            if (isKing) {
                break;
            }
        }

        // Проходим в сторону конца диагонали
        for (let i = currentIndex + 1; i < diagonal.length; i++) {
            const targetCellId = diagonal[i];
            const cell = boardElementsState.cells[targetCellId];
            if (!cell) break;
            if (cell.elements.figure) {
                cell.elements.path.classList.add('cell-highlighted');
                break;
            } else {
                cell.elements.path.classList.add('cell-highlighted');
            }

            if (isKing) {
                break;
            }
        }
    });
}

function highlightMainLinesMoves(cellId, figureType) {
    isKing = false;

    if (figureType === 'King') {
        isKing = true;
    }

    // Получаем массив диагоналей из глобального состояния
    const mainLines = movedElements.mainLines; // предполагается, что это массив массивов строк

    // Фильтруем только те диагонали, в которых присутствует текущая ячейка
    const relevantLines = mainLines.filter(line => line.includes(cellId));

    console.log("relevantLines:", relevantLines);

    relevantLines.forEach(line => {
        const currentIndex = line.indexOf(cellId);

        // Проходим в сторону начала диагонали
        for (let i = currentIndex - 1; i >= 0; i--) {
            const targetCellId = line[i];
            const cell = boardElementsState.cells[targetCellId];
            if (!cell) break;
            // Если в ячейке есть фигура, выделяем её (как потенциальное взятие) и прекращаем поиск в этом направлении
            if (cell.elements.figure) {
                cell.elements.path.classList.add('cell-highlighted');
                console.log(`added light ${targetCellId}`);
                break;
            } else {
                cell.elements.path.classList.add('cell-highlighted');
                console.log(`added light ${targetCellId}`);
            }

            if (isKing) {
                break;
            }
        }

        // Проходим в сторону конца диагонали
        for (let i = currentIndex + 1; i < line.length; i++) {
            const targetCellId = line[i];
            const cell = boardElementsState.cells[targetCellId];
            if (!cell) break;
            if (cell.elements.figure) {
                cell.elements.path.classList.add('cell-highlighted');
                break;
            } else {
                cell.elements.path.classList.add('cell-highlighted');
            }

            if (isKing) {
                break;
            }
        }
    });
}

function highlightSecondaryLinesMoves(cellId, figureType) {
    isKing = false;

    if (figureType === 'King') {
        isKing = true;
    }

    // Получаем массив диагоналей из глобального состояния
    const secondaryLines = movedElements.secondaryLines; // предполагается, что это массив массивов строк

    // Фильтруем только те диагонали, в которых присутствует текущая ячейка
    const relevantLines = secondaryLines.filter(line => line.includes(cellId));

    console.log("relevantLines:", relevantLines);

    relevantLines.forEach(line => {
        const currentIndex = line.indexOf(cellId);

        // Проходим в сторону начала диагонали
        for (let i = currentIndex - 1; i >= 0; i--) {
            const targetCellId = line[i];
            const cell = boardElementsState.cells[targetCellId];
            if (!cell) break;
            // Если в ячейке есть фигура, выделяем её (как потенциальное взятие) и прекращаем поиск в этом направлении
            if (cell.elements.figure) {
                cell.elements.path.classList.add('cell-highlighted');
                console.log(`added light ${targetCellId}`);
                break;
            } else {
                cell.elements.path.classList.add('cell-highlighted');
                console.log(`added light ${targetCellId}`);
            }

            if (isKing) {
                break;
            }
        }

        // Проходим в сторону конца диагонали
        for (let i = currentIndex + 1; i < line.length; i++) {
            const targetCellId = line[i];
            const cell = boardElementsState.cells[targetCellId];
            if (!cell) break;
            if (cell.elements.figure) {
                cell.elements.path.classList.add('cell-highlighted');
                break;
            } else {
                cell.elements.path.classList.add('cell-highlighted');
            }

            if (isKing) {
                break;
            }
        }
    });
}

function clearHighlightedCells() {
    document.querySelectorAll('path').forEach(path => {
        path.classList.remove('cell-highlighted');
    });
}