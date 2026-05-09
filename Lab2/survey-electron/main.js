const { app, BrowserWindow, ipcMain, dialog } = require('electron');
const fs = require('fs');
const path = require('path');

function createWindow() {
    const win = new BrowserWindow({
        width: 600,
        height: 480,
        resizable: false,
        webPreferences: {
            nodeIntegration: false,
            contextIsolation: true,
            preload: path.join(__dirname, 'preload.js')
        },
        title: 'Форма опитування'
    });

    win.loadFile('index.html');
    win.setMenuBarVisibility(false);
}

ipcMain.handle('save-results', async (event, results) => {
    const { filePath, canceled } = await dialog.showSaveDialog({
        title: 'Зберегти результати',
        defaultPath: 'results.txt',
        filters: [{ name: 'Text Files', extensions: ['txt'] }]
    });

    if (canceled || !filePath) return { success: false };

    const lines = results.map(r => `Питання: ${r.question}\nВідповідь: ${r.answer}`);
    const content = lines.join('\n\n');

    fs.writeFileSync(filePath, content, 'utf-8');
    return { success: true };
});

app.whenReady().then(createWindow);

app.on('window-all-closed', () => {
    app.quit();
});
