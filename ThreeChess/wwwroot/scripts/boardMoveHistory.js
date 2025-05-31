function updateMovesHistory(moves) {
    const container = document.getElementById('moves-history');
    container.innerHTML = moves.length > 0
        ? moves.map((move, index) => {
            const turnColor = getTurnColorByIndex(index);
            return `
                <div class="move-item">
                    <span class="move-number">${index + 1}.</span>
                    <span class="move-cells ${turnColor}">${move.startCellId} → ${move.endCellId}</span>
                </div>
            `;
        }).join('')
        : '<div class="no-moves">История ходов пуста</div>';
}

function getTurnColorByIndex(index) {
    const colors = ['White', 'Black', 'Red'];
    return colors[index % 3];
}