const { contextBridge, ipcRenderer } = require('electron');

contextBridge.exposeInMainWorld('api', {
    saveResults: (results) => ipcRenderer.invoke('save-results', results)
});
