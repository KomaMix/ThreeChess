function getDiagonalMoves(cellId) {
    const possibleMoves = [];
    const piece = boardElementsState.cells[cellId].elements.figure;

    isKing = false;

    if (piece.figureInfo.figureType === 'King') {
        isKing = true;
    }

    const diagonals = movedElements.diagonals;

    // Фильтруем только те диагонали, в которых присутствует текущая ячейка
    const relevantDiagonals = diagonals.filter(diagonal => diagonal.includes(cellId));

    function processCell(targetCellId) {
        const cell = boardElementsState.cells[targetCellId];
        if (!cell) return true; // если клетка не найдена, прекращаем обход
        if (cell.elements.figure) {
            // Если клетка занята – проверяем цвет фигуры
            if (cell.elements.figure.figureInfo.figureColor !== piece.figureInfo.figureColor) {
                possibleMoves.push(targetCellId);
            }
            return true; // в любом случае прекращаем обход в этом направлении
        } else {
            // Если клетка пуста – обычная подсветка
            possibleMoves.push(targetCellId);
            return false; // продолжаем обход
        }
    }

    // Обходим каждую релевантную диагональ
    relevantDiagonals.forEach(diagonal => {
        const currentIndex = diagonal.indexOf(cellId);

        // Обход в сторону начала диагонали (уменьшение индекса)
        for (let i = currentIndex - 1; i >= 0; i--) {
            const targetCellId = diagonal[i];
            const shouldBreak = processCell(targetCellId);
            if (shouldBreak) break;
            if (isKing) break;
        }

        // Обход в сторону конца диагонали (увеличение индекса)
        for (let i = currentIndex + 1; i < diagonal.length; i++) {
            const targetCellId = diagonal[i];
            const shouldBreak = processCell(targetCellId);
            if (shouldBreak) break;
            if (isKing) break;
        }
    });

    return possibleMoves;
}

function getMainLinesMoves(cellId) {
    const possibleMoves = [];
    const piece = boardElementsState.cells[cellId].elements.figure;
    isKing = false;

    if (piece.figureInfo.figureType === 'King') {
        isKing = true;
    }

    // Получаем массив диагоналей из глобального состояния
    const mainLines = movedElements.mainLines; // предполагается, что это массив массивов строк

    // Фильтруем только те диагонали, в которых присутствует текущая ячейка
    const relevantLines = mainLines.filter(line => line.includes(cellId));

    function processCell(targetCellId) {
        const cell = boardElementsState.cells[targetCellId];
        if (!cell) return true; // если клетка не найдена, прекращаем обход
        if (cell.elements.figure) {
            // Если клетка занята – проверяем цвет фигуры
            if (cell.elements.figure.figureInfo.figureColor !== piece.figureInfo.figureColor) {
                possibleMoves.push(targetCellId);
            }
            return true; // в любом случае прекращаем обход в этом направлении
        } else {
            // Если клетка пуста – обычная подсветка
            possibleMoves.push(targetCellId);
            return false; // продолжаем обход
        }
    }

    // Обход каждой найденной линии, где присутствует cellId
    relevantLines.forEach(line => {
        const currentIndex = line.indexOf(cellId);

        // Проходим в сторону начала линии (уменьшение индекса)
        for (let i = currentIndex - 1; i >= 0; i--) {
            const targetCellId = line[i];
            const shouldBreak = processCell(targetCellId);
            if (shouldBreak) break;
            if (isKing) break;
        }

        // Проходим в сторону конца линии (увеличение индекса)
        for (let i = currentIndex + 1; i < line.length; i++) {
            const targetCellId = line[i];
            const shouldBreak = processCell(targetCellId);
            if (shouldBreak) break;
            if (isKing) break;
        }
    });

    return possibleMoves;
}

function getSecondaryLinesMoves(cellId) {
    const possibleMoves = [];
    const piece = boardElementsState.cells[cellId].elements.figure;
    isKing = false;

    if (piece.figureInfo.figureType === 'King') {
        isKing = true;
    }

    // Получаем массив диагоналей из глобального состояния
    const secondaryLines = movedElements.secondaryLines; // предполагается, что это массив массивов строк

    // Фильтруем только те диагонали, в которых присутствует текущая ячейка
    const relevantLines = secondaryLines.filter(line => line.includes(cellId));

    function processCell(targetCellId) {
        const cell = boardElementsState.cells[targetCellId];
        if (!cell) return true; // если клетка не найдена, прекращаем обход
        if (cell.elements.figure) {
            // Если клетка занята – проверяем цвет фигуры
            if (cell.elements.figure.figureInfo.figureColor !== piece.figureInfo.figureColor) {
                possibleMoves.push(targetCellId);
            }
            return true; // в любом случае прекращаем обход в этом направлении
        } else {
            // Если клетка пуста – обычная подсветка
            possibleMoves.push(targetCellId);
            return false; // продолжаем обход
        }
    }

    // Для каждой релевантной линии запускаем поиск в обе стороны
    relevantLines.forEach(line => {
        const currentIndex = line.indexOf(cellId);

        // Проходим в сторону начала линии (уменьшение индекса)
        for (let i = currentIndex - 1; i >= 0; i--) {
            const targetCellId = line[i];
            const shouldBreak = processCell(targetCellId);
            if (shouldBreak) break;
            if (isKing) break;
        }

        // Проходим в сторону конца линии (увеличение индекса)
        for (let i = currentIndex + 1; i < line.length; i++) {
            const targetCellId = line[i];
            const shouldBreak = processCell(targetCellId);
            if (shouldBreak) break;
            if (isKing) break;
        }
    });

    return possibleMoves;
}

function getKnightMoves(cellId) {
    const possibleMoves = [];
    const piece = boardElementsState.cells[cellId].elements.figure;
    const lineCombos = [
        { primary: movedElements.mainLines, secondary: movedElements.secondaryLines },
        { primary: movedElements.secondaryLines, secondary: movedElements.mainLines }
    ];

    // Функция-помощник: подсвечивает ячейку, если она существует на доске.
    function possibleMoveToCell(targetCellId) {
        const cell = boardElementsState.cells[targetCellId];
        if (!cell) return false;
        if (cell.elements.figure) {
            // Если в клетке есть фигура, проверяем её цвет
            if (cell.elements.figure.figureInfo.figureColor !== piece.figureInfo.figureColor) {
                return true;
            } else {
                return false;
            }
        } else {
            return true;
        }
    }

    // Для каждой комбинации линий
    lineCombos.forEach(combo => {
        const primaryLines = combo.primary;     // линии, по которым будем двигаться первым этапом
        const secondaryLines = combo.secondary; // линии для второго этапа

        // --- Последовательность 1: 1 шаг по primary, затем 2 шага по secondary ---
        // Находим все линии primary, где присутствует исходная ячейка
        primaryLines.forEach(line => {
            const startIndex = line.indexOf(cellId);
            if (startIndex === -1) return; // если текущая линия не содержит исходную ячейку

            // Делаем 1 ход в обе стороны по текущей линии
            const oneStepIndices = [];
            if (startIndex - 1 >= 0) oneStepIndices.push(startIndex - 1);
            if (startIndex + 1 < line.length) oneStepIndices.push(startIndex + 1);

            oneStepIndices.forEach(intermediateIndex => {
                const intermediateCellId = line[intermediateIndex];
                // Для каждой найденной промежуточной ячейки ищем линии secondary, где она присутствует
                secondaryLines.forEach(secLine => {
                    const secIndex = secLine.indexOf(intermediateCellId);
                    if (secIndex === -1) return;

                    // Делаем 2 хода по линии secondary: в обе стороны, если возможно
                    const twoStepIndices = [];
                    if (secIndex - 2 >= 0) twoStepIndices.push(secIndex - 2);
                    if (secIndex + 2 < secLine.length) twoStepIndices.push(secIndex + 2);

                    twoStepIndices.forEach(targetIndex => {
                        const targetCellId = secLine[targetIndex];
                        if (possibleMoveToCell(targetCellId)) {
                            possibleMoves.push(targetCellId);
                        }
                    });
                });
            });
        });
    });

    return possibleMoves;
}

function getPawnMoves(cellId) {
    const possibleMoves = [];
    const cellState = boardElementsState.cells[cellId];
    const pawn = cellState.elements.figure;
    // По умолчанию, если не задано, считается, что пешка не прошла половину доски
    if (typeof pawn.hasPassedHalfBoard === 'undefined') {
        pawn.hasPassedHalfBoard = false;
    }


    // Шаг 0: Находим основную линию, где находится пешка
    let pawnMainLine = null;
    let mainLineIndex = -1;
    for (let line of movedElements.mainLines) {
        const idx = line.indexOf(cellId);
        if (idx !== -1) {
            pawnMainLine = line;
            mainLineIndex = idx;
            break;
        }
    }
    if (!pawnMainLine) {
        return;
    }

    // Шаг 1: Определение направления хода пешки
    if (!pawn.hasPassedHalfBoard) {
        // Если пешка ближе к концу массива
        if (mainLineIndex > 3) {
            pawnMainLine = pawnMainLine.slice().reverse();
            mainLineIndex = pawnMainLine.indexOf(cellId);
        }
    } else {
        // Если пешка ближе к началу массива
        if (mainLineIndex < 4) {
            pawnMainLine = pawnMainLine.slice().reverse();
            mainLineIndex = pawnMainLine.indexOf(cellId);
        }
    }

    // Шаг 2: Находим побочную линию, где стоит пешка
    let pawnSecondaryLine = null;
    let secondaryLineIndex = -1;
    for (let line of movedElements.secondaryLines) {
        const idx = line.indexOf(cellId);
        if (idx !== -1) {
            pawnSecondaryLine = line;
            secondaryLineIndex = idx;
            break;
        }
    }
    if (!pawnSecondaryLine) {
        return;
    }

    // Шаг 3: Находим соседние клетки на побочной линии (они будут базой для диагональных ходов)
    const neighborSecondaryCells = [];
    if (secondaryLineIndex - 1 >= 0) neighborSecondaryCells.push(pawnSecondaryLine[secondaryLineIndex - 1]);
    if (secondaryLineIndex + 1 < pawnSecondaryLine.length) neighborSecondaryCells.push(pawnSecondaryLine[secondaryLineIndex + 1]);

    // Шаг 4: Для каждой соседней клетки ищем соответствующую основную линию и приводим её к нужной ориентации
    let neighborMainLines = [];
    neighborSecondaryCells.forEach(neighborCellId => {
        movedElements.mainLines.forEach(line => {
            if (line.indexOf(neighborCellId) !== -1) {
                let candidateLine = line;
                let idx = candidateLine.indexOf(neighborCellId);
                // Если индекс в найденной линии не совпадает с индексом пешки (mainLineIndex),
                // переворачиваем линию, чтобы "лицевой" стороной совпал порядок
                if (idx !== mainLineIndex) {
                    candidateLine = candidateLine.slice().reverse();
                    idx = candidateLine.indexOf(neighborCellId);
                }
                neighborMainLines.push({ line: candidateLine, cellId: neighborCellId, index: idx });
            }
        });
    });

    // Шаг 5: Определяем возможный ход вперед
    let possibleForwardCell = null;
    const targetIndex = mainLineIndex + 1;
    if (targetIndex >= 0 && targetIndex < pawnMainLine.length) {
        possibleForwardCell = pawnMainLine[targetIndex];
    }

    // Добавление прямого хода
    if (possibleForwardCell) {
        const forwardCellState = boardElementsState.cells[possibleForwardCell];
        if (forwardCellState && !forwardCellState.elements.figure) {
            possibleMoves.push(possibleForwardCell);
        }
    }

    // Определяем диагональные ходы для взятия
    const captureMoves = [];
    neighborMainLines.forEach(item => {
        const line = item.line;
        // Определяем индекс соседней клетки в этой линии
        const idx = line.indexOf(item.cellId);
        // Целевая клетка для взятия – сдвиг на forwardDirection относительно соседней клетки
        const targetNeighborIndex = idx + 1;
        if (targetNeighborIndex >= 0 && targetNeighborIndex < line.length) {
            captureMoves.push(line[targetNeighborIndex]);
        }
    });

    // Отдельно проверяем ходы через центр
    const centralCells = ['E4', 'D4', 'D5', 'I5', 'I9', 'E9'];
    if (!pawn.hasPassedHalfBoard && centralCells.includes(cellId)) {
        const centralIndex = centralCells.indexOf(cellId);
        const extraIndex1 = (centralIndex + 2) % centralCells.length;
        const extraIndex2 = (centralIndex - 2 + centralCells.length) % centralCells.length;
        const extraCells = [centralCells[extraIndex1], centralCells[extraIndex2]];
        extraCells.forEach(extraCell => {
            if (!captureMoves.includes(extraCell)) {
                captureMoves.push(extraCell);
            }
        });
    }



    // Подсветка двойного хода вперёд (на две клетки)
    if (!pawn.hasPassedHalfBoard && mainLineIndex === 1) {
        const doubleTargetIndex = mainLineIndex + 2;
        if (doubleTargetIndex < pawnMainLine.length) {
            const firstStepCell = pawnMainLine[mainLineIndex + 1];
            const doubleForwardCell = pawnMainLine[doubleTargetIndex];

            const firstStepCellState = boardElementsState.cells[firstStepCell];
            const doubleForwardCellState = boardElementsState.cells[doubleForwardCell];

            if (firstStepCellState && !firstStepCellState.elements.figure &&
                doubleForwardCellState && !doubleForwardCellState.elements.figure) {
                possibleMoves.push(doubleForwardCell);
            }
        }
    }


    // Добавление диагональных ходов
    captureMoves.forEach(targetCellId => {
        const targetCellState = boardElementsState.cells[targetCellId];
        if (targetCellState && targetCellState.elements.figure
            && targetCellState.elements.figure.figureInfo.figureColor !== gameConfig.controlledColor) {
            possibleMoves.push(targetCellId);
        }
    });

    return possibleMoves;
}

function getKingCastlingMoves(cellId) {
    const possibleMoves = [];
    const cellState = boardElementsState.cells[cellId];
    const king = cellState.elements.figure;

    if (typeof king.hasMoved === 'undefined') {
        king.hasMoved = false;
    }

    if (king.hasMoved) return;

    // Шаг 1: Ищем побочную линию (secondary line) с королем
    let kingSecondaryLine = null;
    let secondaryLineIndex = -1;
    for (let line of movedElements.secondaryLines) {
        const idx = line.indexOf(cellId);
        if (idx !== -1) {
            kingSecondaryLine = line;
            secondaryLineIndex = idx;
            break;
        }
    }

    if (!kingSecondaryLine) {
        return;
    }

    // Шаг 2: Поиск ладей на этой же побочной линии
    const directions = [
        { side: 'right', delta: 1, steps: 2 },  // В одну сторону
        { side: 'left', delta: -1, steps: 3 }   // В другую сторону
    ];

    directions.forEach(({ side, delta, steps }) => {
        let currentIndex = secondaryLineIndex;
        let pathClear = true;
        let rookCell = null;
        const cellsBetween = [];

        // Проверяем путь к ладье
        while (true) {
            currentIndex += delta;
            if (currentIndex < 0 || currentIndex >= kingSecondaryLine.length) break;

            const cell = kingSecondaryLine[currentIndex];
            const cellState = boardElementsState.cells[cell];

            if (currentIndex === kingSecondaryLine.length - 1 || currentIndex === 0) { // Последний шаг - позиция ладьи
                if (cellState?.elements?.figure?.figureInfo?.figureType === 'Rook') {
                    rookCell = cell;
                }
            } else { // Проверяем промежуточные клетки
                cellsBetween.push(cell);
                if (cellState?.elements?.figure) pathClear = false;
            }
        }

        // Проверяем условия для рокировки
        if (rookCell && pathClear) {
            const rook = boardElementsState.cells[rookCell].elements.figure;

            if (!rook.hasMoved && !king.hasMoved) {
                // Определяем целевую клетку для короля
                const targetIndex = secondaryLineIndex + 2 * delta;

                if (targetIndex >= 0 && targetIndex < kingSecondaryLine.length) {
                    const castlingCell = kingSecondaryLine[targetIndex];
                    const cellState = boardElementsState.cells[castlingCell];

                    possibleMoves.push(castlingCell);
                }
            }
        }
    });

    return possibleMoves;
}